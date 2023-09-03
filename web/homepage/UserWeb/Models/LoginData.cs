using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace UserWeb.Models
{
    /// <summary>
    /// 로그인 성공 후 쿠키에 들어갈 데이터
    /// </summary>
    public class LoginData : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        
    }
}