using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserWeb.Models
{
    /// <summary>
    /// 회원 가입 폼 데이터
    /// </summary>
    public class JoinForm
    {
        public string m_name { get; set; }
        public string m_id { get; set; }
        public string m_nick { get; set; }
        public string m_pwd1 { get; set; }
        public string m_pwd2 { get; set; }
        public string m_phone1 { get; set; }
        public string m_phone2 { get; set; }
        public string m_phone3 { get; set; }
        public string m_recomid { get; set; }

        // 암호화된 인증 정보 (KCP 에서 Join03 통해 전달)
        public string kcp_cert_data { get; set; }
    }
}