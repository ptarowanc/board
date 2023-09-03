using DBLIB.Model;
using DBLIB.Service;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.Controllers
{
    public class LoginController : Controller
    {
        readonly ILog log = log4net.LogManager.GetLogger(typeof(LoginController));
        readonly AdminUserService adminUserService;

        public LoginController(AdminUserService adminUserService)
        {
            this.adminUserService = adminUserService;
        }

        /// <summary>
        /// 로그인 UI
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                //return HttpContext.Current.GetOwinContext().Authentication;
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            log.Info("관리자[" + User.Identity.GetUserId() + "] 로그아웃");
            AuthenticationManager.SignOut();
            return Redirect("/Login");
        }

        [HttpPost]
        /// <summary>
        /// 로그인 처리
        /// </summary>
        /// <returns></returns>
        public JsonResult PerformLogin(string userId, string pwd)
        {
            var res = new StandardResult();
            try
            {
                var entry = adminUserService.Find(userId, pwd);
                res = entry != null ? StandardResult.createSucceeded() : StandardResult.createError("로그인 정보를 확인해 주세요");

                if(entry != null)
                {
                    var claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                    var list = new List<Claim>();
                    list.Add(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String));
                    list.Add(new Claim(ClaimTypes.Name, entry.Name, ClaimValueTypes.String));
                    list.Add(new Claim(ClaimTypes.SerialNumber, entry.Id.ToString(), ClaimValueTypes.Integer32));
                    claimsIdentity.AddClaims(list);

                    // 로그인 결과를 세션 쿠키에 적용
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, claimsIdentity);

                    HttpContext.User = new ClaimsPrincipal(AuthenticationManager.AuthenticationResponseGrant.Principal);
                }
            }
            catch(Exception e)
            {
                res = StandardResult.createError(e.Message);
            }
            return Json(res);
        }
    }
}