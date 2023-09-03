using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserWeb.Models
{
    /// <summary>
    /// 회원 가입 폼 데이터
    /// </summary>
    public class MyInfoForm
    {
        public string m_pwd_org { get; set; }
        public string m_pwd_new1 { get; set; }
        public string m_pwd_new2 { get; set; }
    }
}