using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace UserWeb.Models
{
    // KCP 에서 응답으로 전달된 데이터
    public class KCPResponseData
    {
        public String site_cd;
        public String ordr_idxx;
        public String res_cd;
        public String req_tx;
        public String cert_no;
        public String enc_cert_data2;
        public String dn_hash;


        //public String enc_cert_data;
        //public String up_hash;
        //public String param_opt_1;
        //public String param_opt_2;
        //public String param_opt_3;

        // 아래는 위 enc_cert_data 에서 추출한 개별 데이터
        public String comm_id;
        public String phone_no;
        public String user_name;
        public String birth_day;
        public String sex_code;
        public String local_code;
        public String ci;
        public String di;
        public String ci_url;
        public String di_url;
        public String web_siteid;
        public String res_msg;

        public String cert_enc_use;

        public Boolean IsSucceeded
        {
            get
            {
                return res_cd != null && res_cd == "0000";
            }
        }

        // 전문 해독
        void Decrypt()
        {
            // 정상 종료된게 아니면 디코딩 필요 없음
            if (res_cd == null || res_cd != "0000")                
                return;

            // #1. cert_enc_use != "Y" 인 경우를 어떻게 다루는지 메뉴얼에 없음 -_-
            if (cert_enc_use == null || cert_enc_use != "Y")
                throw new NotSupportedException("cert_enc_use must be 'Y'.");

            var ct_cert = new ct_cli_comLib.CTKCP();

            // dn_hash 검증
            // KCP 가 리턴해 드리는 dn_hash 와 사이트 코드, 요청번호 , 인증번호를 검증하여
            // 해당 데이터의 위변조를 방지합니다
            string veri_str = site_cd + ordr_idxx + cert_no;

            if (ct_cert.lf_CT_CLI__check_valid_hash(Properties.Settings.Default.KCP_ENC_KEY, dn_hash, veri_str).Equals("FAIL"))
            {
                //오류 처리 영역 ( dn_hash 변조 위험있음)
                throw new InvalidOperationException("mismatch KCP hash");
            }

            ct_cert.lf_CT_CLI__decrypt_enc_cert(Properties.Settings.Default.KCP_ENC_KEY, site_cd, cert_no, enc_cert_data2);

//#if DEBUG
//            Debug.Write("======================복호화 데이터=======================");
//            Debug.Write("복호화 이동통신사 코드 :" + ct_cert.lf_CT_CLI__get_key_value("comm_id"));
//            Debug.Write("복호화 전화번호        :" + ct_cert.lf_CT_CLI__get_key_value("phone_no"));
//            Debug.Write("복호화 이름            :" + ct_cert.lf_CT_CLI__get_key_value("user_name"));
//            Debug.Write("복호화 생년월일        :" + ct_cert.lf_CT_CLI__get_key_value("birth_day"));
//            Debug.Write("복호화 성별코드        :" + ct_cert.lf_CT_CLI__get_key_value("sex_code"));
//            Debug.Write("복호화 내/외국인 정보  :" + ct_cert.lf_CT_CLI__get_key_value("local_code"));
//            Debug.Write("복호화 CI              :" + ct_cert.lf_CT_CLI__get_key_value("ci"));
//            Debug.Write("복호화 DI              :" + ct_cert.lf_CT_CLI__get_key_value("di"));
//            Debug.Write("복호화 CI_URL          :" + HttpUtility.UrlDecode(ct_cert.lf_CT_CLI__get_key_value("ci_url")));
//            Debug.Write("복호화 DI_URL          :" + HttpUtility.UrlDecode(ct_cert.lf_CT_CLI__get_key_value("di_url")));
//            Debug.Write("복호화 WEB_SITEID      :" + ct_cert.lf_CT_CLI__get_key_value("web_siteid"));
//            Debug.Write("복호화 결과코드        :" + ct_cert.lf_CT_CLI__get_key_value("res_cd"));
//            Debug.Write("복호화 결과메시지      :" + ct_cert.lf_CT_CLI__get_key_value("res_msg"));
//            Debug.Write("==========================================================");
//#endif

            phone_no = ct_cert.lf_CT_CLI__get_key_value("phone_no");
            comm_id = ct_cert.lf_CT_CLI__get_key_value("comm_id");
            user_name = ct_cert.lf_CT_CLI__get_key_value("user_name");
            birth_day = ct_cert.lf_CT_CLI__get_key_value("birth_day");
            sex_code = ct_cert.lf_CT_CLI__get_key_value("sex_code");
            local_code = ct_cert.lf_CT_CLI__get_key_value("local_code");
            ci = ct_cert.lf_CT_CLI__get_key_value("ci");
            di = ct_cert.lf_CT_CLI__get_key_value("di");
            ci_url = ct_cert.lf_CT_CLI__get_key_value("ci_url");
            di_url = ct_cert.lf_CT_CLI__get_key_value("di_url");
            web_siteid = ct_cert.lf_CT_CLI__get_key_value("web_siteid");

            res_cd = ct_cert.lf_CT_CLI__get_key_value("res_cd");
            res_msg = HttpUtility.UrlDecode( ct_cert.lf_CT_CLI__get_key_value("res_msg") );

            ct_cert = null;
        }

        // HTTP 로 전달된 값을 저장
        public static KCPResponseData Parse(HttpRequestBase req)
        {
            var res = new KCPResponseData();

            var fields = typeof(KCPResponseData).GetFields();
            foreach (var field in fields)
            {
                if (req.Form.AllKeys.Contains(field.Name))
                {
                    string value = req.Form.Get(field.Name);
                    field.SetValue(res, value);
                }
            }

            res.Decrypt();
            
            return res;
        }
    }
}