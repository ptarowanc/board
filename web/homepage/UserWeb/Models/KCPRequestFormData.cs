using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using DBLIB.Service;
using UserWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using DBLIB;
using UserWeb.Service;
using DBLIB.Model;
using System.Diagnostics;


namespace UserWeb.Models
{
    // KCP 인증 요청 페이지에 들어갈 정보
    public class KCPRequestFormData
    {
        public string ordr_idxx;
        public string user_name;
        public string up_hash;
        public string web_siteid;
        public string param_opt_1;
        public string param_opt_2;
        public string param_opt_3;
        public string Ret_URL;
        public string web_siteid_hashYN;

        public string sex_code;
        public string local_code;

        void MakeHash()
        {
            var ct_cert = new ct_cli_comLib.CTKCP();

            //if (!(web_siteid_hashYN != null && web_siteid_hashYN == "Y"))
            //web_siteid = Properties.Settings.Default.KCP_WEBSITE_ID;

            up_hash = ct_cert.lf_CT_CLI__make_hash_data(Properties.Settings.Default.KCP_ENC_KEY, Properties.Settings.Default.KCP_SITE_CODE + ordr_idxx + user_name + "00" + "00" + "00" + sex_code + local_code + web_siteid);
            param_opt_1 = DBLIB.Service.CryptoHelper.Encrypt(up_hash, Properties.Settings.Default.KCP_SITE_CODE);
            ct_cert = null;
        }

        // 폼 값을 바로 멤버 변수에 설정... 간단한 리플렉션 이용해서 처리... 
        public static KCPRequestFormData Parse(HttpRequestBase request)
        {
            var form = new KCPRequestFormData();
            var fields = typeof(KCPRequestFormData).GetFields();
            foreach (var field in fields)
            {
                string value = request.Form[field.Name];
                field.SetValue(form, value);
            }

            // form.ordr_idxx = "201806191529335203936";
            form.sex_code = "";
            form.local_code = "01"; // 내국인
            // TODO :: 고정값은 여기서 설정 

            form.MakeHash();

            return form;
        }
    }
}