using DBLIB;
using DBLIB.Service;

using System;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using UserWeb.Models;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Text;

namespace UserWeb.Service
{
    public class LoginHelper
    {
        ILog log = log4net.LogManager.GetLogger(typeof(LoginHelper));
        
        readonly CryptoHelper cryptoHelper;

        const string ID_SAVE_COOKIE_ENTRY = "idsave";
        const string LOGIN_DATA_ENTRY = "data";

        const string PASSWORD_TOKEN = "VONGVONG";

        public LoginHelper(CryptoHelper c)
        {
            this.cryptoHelper = c;
        }

        /// <summary>
        /// 쿠키에 저장된 ID 
        /// </summary>
        /// <returns></returns>
        public string GetSavedLoginID()
        {
            var savedLoginIdEntry = HttpContext.Current.Request.Cookies[ID_SAVE_COOKIE_ENTRY];
            if (savedLoginIdEntry != null)
                return savedLoginIdEntry.Value;
            return string.Empty;
        }
        
        public string LoginId
        {
            get
            {
                var data = GetLoginData();
                return data != null ? data.Id: string.Empty;
            }
        }
        public string Nickname
        {
            get
            {
                var data = GetLoginData();
                return data != null ? data.UserName: string.Empty;
            }
        }


        public LoginData GetLoginData()
        {
            var entry = HttpContext.Current.Request.Cookies[LOGIN_DATA_ENTRY];
            if (entry == null || String.IsNullOrEmpty(entry.Value))
                return null;   // 로그인 데이터가 없음

            string encrypted = CryptoHelper.Decrypt(entry.Value, PASSWORD_TOKEN);
            try
            {
                var result = JsonConvert.DeserializeObject<LoginData>(encrypted);
                return result;
            }
            catch (Exception e)
            {
                log.Error("쿠키 해석 중 오류", e);
            }
            return null;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
                // return HttpContext.GetOwinContext().Authentication;
            }
        }

        public void UpdateNickname(string newNickname)
        {
            var yolo = YoloIdentity.CreateIdentity(HttpContext.Current.User.Identity.GetUserId(), newNickname, HttpContext.Current.User.Identity.GetPhone());

            HttpContext.Current.User.AddUpdateClaim(ClaimTypes.Name, newNickname);
        }

        public void MakeLogon(string userId, Player player, bool keepLoginId)
        {
            var yolo = YoloIdentity.CreateIdentity(userId, player.NickName, player.PhoneNo);

            // 로그인 결과 쿠키에 셋팅
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, yolo);

            HttpContext.Current.User = new ClaimsPrincipal(AuthenticationManager.AuthenticationResponseGrant.Principal);

            //AuthenticationManager.User.Identity;

            /*
            LoginData loginData = new LoginData();
            loginData.Id = userId;
            loginData.UserName = name;
            
            var JSONed = JsonConvert.SerializeObject(loginData);
            string encrypted = CryptoHelper.Encrypt(JSONed, PASSWORD_TOKEN);
            
            HttpContext.Current.Response.SetCookie(new HttpCookie(LOGIN_DATA_ENTRY, encrypted));
            log.Info("이용자[" + userId + "] 로그인 쿠키 설정. 데이터 = [" + encrypted + "]");
            */

            // ID 저장 여부 설정
            if (keepLoginId)
            {
                var cookie = HttpContext.Current.Request.Cookies[ID_SAVE_COOKIE_ENTRY];
                if (cookie != null)
                {
                    cookie.Value = userId;
                }
                else
                {
                    cookie = new HttpCookie(ID_SAVE_COOKIE_ENTRY);
                    cookie.Value = userId;
                    cookie.Expires = new DateTime().AddMinutes(10);

                }

                HttpContext.Current.Response.Cookies.Set(cookie);
                log.Info("이용자[" + userId + "] ID 저장 쿠키 설정");
            }
            else
            {
                // ID 저장 필드 제거
                HttpContext.Current.Response.Cookies.Remove(ID_SAVE_COOKIE_ENTRY);
                log.Info("이용자[" + userId + "] ID 저장 쿠키 삭제");
            }
        }

        public void SetPreLogin(string data1, string data2)
        {
            HttpContext.Current.Response.SetCookie(new HttpCookie("session_01", Convert.ToBase64String(CryptoHelper.EncryptStringToBytes_Aes(data1))));
            HttpContext.Current.Response.SetCookie(new HttpCookie("session_02", Convert.ToBase64String(CryptoHelper.EncryptStringToBytes_Aes(data2))));
        }

        public void MakeLogout()
        {
            log.Info("로그 아웃");

            AuthenticationManager.SignOut();
            HttpContext.Current.Response.SetCookie(new HttpCookie("session_01", ""));
            HttpContext.Current.Response.SetCookie(new HttpCookie("session_02", ""));
            //HttpContext.Current.Response.SetCookie(new HttpCookie(LOGIN_DATA_ENTRY, string.Empty));
        }
    }
}