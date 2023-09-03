using DBLIB.Model;
using DBLIB.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserWeb.Models;

namespace UserWeb.Controllers
{
    public class MobileWebViewController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MobileWebViewController));
        readonly UserService userService;

        public MobileWebViewController(UserService userService)
        {
            this.userService = userService;
        }

        public static string generateKcpHash(string web_siteid_hashYN, string ordr_idxx, string web_siteid, string user_name, string sex_code, string local_code)
        {
            var ct_cert = new ct_cli_comLib.CTKCP();

            if (!(web_siteid_hashYN != null && web_siteid_hashYN == "Y"))
                web_siteid = "";

            return ct_cert.lf_CT_CLI__make_hash_data(Properties.Settings.Default.KCP_ENC_KEY, Properties.Settings.Default.KCP_SITE_CODE + ordr_idxx + web_siteid + user_name + "00" + "00" + "00" + sex_code + local_code);
        }

        public ActionResult RegisterForm(string req_tx)
        {
            //if (String.IsNullOrEmpty(req_tx))
            //{   // 앱에서 처음 진입 하는 경우

            //    string orderId = Guid.NewGuid().ToString();
            //    log.Info("본인 인증 요청 ID = " + orderId);

            //    string upHash = generateKcpHash(null, orderId, "", "", "", "");

            //    UrlHelper urlHelper = new UrlHelper(Url.RequestContext);
            //    var RetUrl = Request.Url.AbsoluteUri; // urlHelper.Action("RegisterForm", null, null, HttpContext.Request.Url.Scheme);     // 나한테 돌아와~
            //    return View("StartCert", new Tuple<String, string, string>(RetUrl, orderId, upHash));
            //}
            //else if(req_tx == "otp_auth" || req_tx == "auth")
            //{   // KCP 인증 받으러 갔다가 돌아오는 길
            //    var response = Models.KCPResponseData.Parse(Request);

            //    if (response.IsSucceeded)
            //    {   // 본인 인증 성공

            //        // 나이제한 확인
            //        if (Int32.Parse(response.birth_day.Substring(0, 4)) > (DateTime.Now.Year - 18))
            //        {
            //            string Massage = "미성년자는 서비스를 이용하실 수 없습니다.";
            //            return View("CertFail", new Tuple<string, string>("", Massage));
            //        }

            //        // #1. 이미 가입한 회원인지 확인
            //        var entry = userService.GetExistingMemberId(response.ci, response.di);
            //        if(entry != null)
            //        {
            //            // 이미 가입한 회원~
            //            string Massage = entry.UserName + "님은 [ " + entry.UserID + " ] 계정으로 가입되어 있습니다.";
            //            return View("CertFail", new Tuple<string,string>("", Massage));
            //            //return View("ExistingMember", entry);
            //        }

            //        // #2. 표시용 전화번호
            //        List<string> phoneTokens = MemberController.MakePrettyPhoneNo(response.phone_no).Split(new char[] { '-' } ).ToList();
            //        while (phoneTokens.Count < 3)
            //            phoneTokens.Add("-");

            //        return View("RegisterForm", new Tuple<Models.KCPResponseData, string, string[]>(response
            //                                                                                            , DBLIB.Service.CryptoHelper.Encrypt(response, MemberController.InternalFormDataPassword)
            //                                                                                            , phoneTokens.ToArray()));
            //    }
            //    else
            //    {   // 본인 인증 실패

            //        return View("CertFail", new Tuple<string,string>(response.res_cd, response.res_msg));
            //        //return View("CertFail", response);
            //    }
            //}
            //else
            //{
            //    log.Info("알 수 없음 요청 식별자 [" + req_tx + "]");
            //}

            //return null;

            return View("RegisterForm");
        }

        public enum CheckType
        {
            UserID,
            Nickname
        }

        [HttpPost]
        public JsonResult CheckExist(CheckType type, string id)
        {
            var result = new StandardResult();
            try
            {
                string val = type == CheckType.UserID ? (userService.IsAvailableId(id) ? "NEW" : "EXIST")
                                                       : (userService.IsAvailableNick(id) ? "NEW" : "EXIST");

                result.result = "OK";
                result.reason = val == "NEW" ? "" : "이미 사용중입니다";
                result.data = val;
            }
            catch(Exception e)
            {
                result = StandardResult.createError(e.Message);
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult PerformReg(string LoginID, 
                                        string LoginPassword, string LoginPassword2, 
                                        string UserName, 
                                        string Nickname, 
                                        string PhoneNo1, string PhoneNo2, string PhoneNo3, string RecomCode, string data)
        {
            var result = new StandardResult();

            try
            {
                //UserWeb.Models.KCPResponseData certData = null;
                //try
                //{
                //    string raw = DBLIB.Service.CryptoHelper.Decrypt(data, MemberController.InternalFormDataPassword);
                //    certData = JsonConvert.DeserializeObject<KCPResponseData>(raw);
                //}
                //catch(Exception e)
                //{
                //    throw new InvalidOperationException("인증 데이터를 복원 할 수 없습니다", e);
                //}

                //UserName = certData.user_name;

                if (string.IsNullOrWhiteSpace(LoginID))
                    throw new YoloException("아이디를 입력하세요");
                if (string.IsNullOrWhiteSpace(UserName))
                    throw new YoloException("이름을 입력하세요");
                if (string.IsNullOrWhiteSpace(Nickname))
                    throw new YoloException("닉네임을 입력하세요");
                if (string.IsNullOrWhiteSpace(LoginPassword) || string.IsNullOrWhiteSpace(LoginPassword2))
                    throw new YoloException("비밀번호를 입력하세요");
                if (LoginPassword != LoginPassword2)
                    throw new YoloException("비밀번호가 일치하지 않습니다");

                string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = Request.ServerVariables["REMOTE_ADDR"];
                }

                string message;
                //bool ok = userService.AddUser(UserService.CreatedFrom.MOBILE, LoginID, UserName, Nickname, LoginPassword, "", MemberController.MakePrettyPhoneNo( certData.phone_no), RecomCode, certData.ci, certData.di, certData.cert_no, ipAddress, out message);
                bool ok = userService.AddUser(UserService.CreatedFrom.MOBILE, LoginID, UserName, Nickname, LoginPassword, "", MemberController.MakePrettyPhoneNo( $"010-{PhoneNo2}-{PhoneNo3}" ), RecomCode, "","","", ipAddress, out message);

                result = ok ? StandardResult.createSucceeded() : StandardResult.createError(message);
            }
            catch(YoloException e)
            {
                result = StandardResult.createError(e.Message);
            }
            catch (Exception e)
            {
                result = StandardResult.createError("가입 처리 중 오류가 발생하였습니다");
                log.Error("모바일 가입 중 오류", e);
            }

            return Json(result);
        }
    }
}