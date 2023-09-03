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
using Newtonsoft.Json;

namespace UserWeb.Controllers
{

    public class MemberController : Controller
    {
        readonly LoginHelper loginHelper;
        readonly UserService userService;
        readonly QnaService qnaService;
        readonly ProductService productService;
        readonly ChargeService chargeService;

        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MemberController));

        public MemberController(UserService userService, LoginHelper loginHelper, QnaService qnaService, ProductService productService, ChargeService chargeService)
        {
            this.userService = userService;
            this.loginHelper = loginHelper;
            this.qnaService = qnaService;
            this.productService = productService;
            this.chargeService = chargeService;
        }
        
        // GET: Member/Join01
        public ActionResult Join01()
        {
            return View();
        }
        /// <summary>
        /// 본인 인증
        /// </summary>
        /// <param name="agreement"></param>
        /// <returns></returns>
        // GET: Member/Join02
        public ActionResult Join02(String agreement)
        {
            if (String.IsNullOrEmpty(agreement) || agreement != "Y")
            {
                Response.Redirect("Join01");
            }

            //Response.Redirect("Join03");
            return View();
        }

        public const String InternalFormDataPassword = "YOLOZ";
        
        [HttpPost]
        public ActionResult CertProc(String req_tx)
        {
            // Request 통해서 한방에 object 로
            log.Info("KCP 요청 진입 - [" + req_tx + "]");
            Debug.Assert(req_tx == "cert");
                        
            if (String.Compare(req_tx, "cert", true) == 0)
            {
                UrlHelper urlHelper = new UrlHelper(Url.RequestContext);

                // KCP 에 인증 받으러 ㄱㄱㄱ
                KCPRequestFormData form = KCPRequestFormData.Parse(Request);
                //if(form.web_siteid_hashYN != null && form.web_siteid_hashYN == "Y")
                //form.web_siteid = Properties.Settings.Default.KCP_WEBSITE_ID;
                
                form.Ret_URL = urlHelper.Action("AfterKCP", "Member", null, HttpContext.Request.Url.Scheme);     // 여기로 돌아오도록 URL 설정
                log.Info("본인 인증 절차 시작 [" + JsonConvert.SerializeObject(form) + "]");

                return View("JumpToKCP", form);
            }
            else
            {
                log.Info("BAD Request-Context [" + req_tx + "]");
            }

            return null;
        }

        [HttpPost]
        public ActionResult AfterKCP(string req_tx)
        {
            log.Info("KCP 응답 진입 - [" + req_tx + "]");
            Debug.Assert(req_tx == "auth" || req_tx == "otp_auth" || req_tx == "cert");

            if (String.Compare(req_tx, "auth", true) == 0 || String.Compare(req_tx, "otp_auth", true) == 0)
            {
                // 인증 결과 도착
                KCPResponseData data = KCPResponseData.Parse(Request);
                log.Info("본인 인증 결과 [" + JsonConvert.SerializeObject(data));

                string crypted = DBLIB.Service.CryptoHelper.Encrypt(data, InternalFormDataPassword);         // 인증 결과를 통채로 암호화해서 전달해서 Join03 으로 받을 수 있게 한다
                return View("AfterKCP", (object)crypted);          // 일단 떠 있는 팝업은 닫고 메인 화면의 Join03 에서 다시 분기되도록
            }
            else
            {
                log.Info("BAD Request-Context [" + req_tx + "]");
            }

            return null;
        }

        /// <summary>
        /// 회원 가입 입력 폼
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //public ActionResult Join03(string data)
        //{
        //    return View();
        //}

        public ActionResult Join03(string data)
        {
            string json = DBLIB.Service.CryptoHelper.Decrypt(data, InternalFormDataPassword);
            KCPResponseData result = JsonConvert.DeserializeObject<KCPResponseData>(json);      // KCP 에서 받은 데이터

            // 본인 인증 받았다!
            if (result.IsSucceeded)
            {

                // 나이제한 확인
                if (Int32.Parse(result.birth_day.Substring(0, 4)) > (DateTime.Now.Year - 18))
                {
                    string Massage = "미성년자는 서비스를 이용하실 수 없습니다.";
                    return View("CertFail", new Tuple<string, string>("", Massage));
                }

                // 중복 가입 확인
                Player registeredUserId = userService.GetExistingMemberId(result.ci, result.di);
                if (registeredUserId != null && registeredUserId.Quit == false)
                {
                    string Massage = registeredUserId.UserName + "님은 [ " + registeredUserId.UserID + " ] 계정으로 가입되어 있습니다.";
                    return View("CertFail", new Tuple<string, string>("", Massage));
                }

                // 가입폼
                List<string> phoneTokens = MakePrettyPhoneNo(result.phone_no).Split(new char[] { '-' }).ToList();
                while (phoneTokens.Count() < 3)
                    phoneTokens.Add("");

                return View(new Tuple<KCPResponseData, string, string[]>(result, data, phoneTokens.ToArray()));
            }

            // 인증 못 받음...
            return View("CertFail", new Tuple<string, string>(result.res_cd, HttpUtility.UrlDecode(result.res_msg)));
            //return View("ErrorFromKCP", result);
        }

        static Regex onlyDigits = new Regex("\\d+");

        public static string MakePrettyPhoneNo(String src)
        {
            switch (src.Length)
            {
                case 11:    // 010-1234-5678
                    return src.Substring(0, 3) + "-" + src.Substring(3, 4) + "-" + src.Substring(7, 4);
                case 10:    // 010-123-4567
                    return src.Substring(0, 3) + "-" + src.Substring(3, 3) + "-" + src.Substring(6, 4);
                case 9:     // 02-123-4567
                    return src.Substring(0, 2) + "-" + src.Substring(2, 3) + "-" + src.Substring(5, 4);
                default:
                    return src;
            }
        }

        /// <summary>
        /// 실제 가입 처리
        /// </summary>
        /// <returns></returns>
        public ActionResult JoinProc(Models.JoinForm form)
        {

            string phone = form.m_phone1 + "-" + form.m_phone2 + "-" + form.m_phone3;
            try
            {
                if (string.IsNullOrWhiteSpace(form.m_id))
                    throw new ArgumentException("아이디를 입력하세요");
                if (string.IsNullOrWhiteSpace(form.m_name))
                    throw new ArgumentException("이름을 입력하세요");
                if (string.IsNullOrWhiteSpace(form.m_nick))
                    throw new ArgumentException("닉네임을 입력하세요");
                if (string.IsNullOrWhiteSpace(form.m_pwd1) || string.IsNullOrWhiteSpace(form.m_pwd2))
                    throw new ArgumentException("비밀번호를 입력하세요");
                if (form.m_pwd1 != form.m_pwd2)
                    throw new ArgumentException("비밀번호가 일치하지 않습니다");
            }
            catch (ArgumentException e)
            {
                ViewBag.message = e.Message;
                return View("../common/SimpleAlertView");
            }

            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }

            string msg;
            bool registerOk = userService.AddUser(UserService.CreatedFrom.WEB, form.m_id, form.m_name, form.m_nick, form.m_pwd1, "", phone, form.m_recomid, "", "", "", ipAddress, out msg);

            if (registerOk)
            {
                ViewBag.message = "회원 가입을 축하드립니다";
                ViewBag.redirect = "/";
                ViewBag.frame = "parent";

                try
                {
                    if (string.IsNullOrWhiteSpace(form.m_id) || string.IsNullOrWhiteSpace(form.m_pwd1))
                        throw new Exception("ID, 비밀번호를 확인해 주세요");

                    loginHelper.SetPreLogin(form.m_id, form.m_pwd1);

                    var result = userService.TryLogin(form.m_id, form.m_pwd1);
                    if (result == null)
                        throw new Exception("ID 또는 비밀번호를 확인해 주세요");

                    loginHelper.MakeLogon(form.m_id, result, false);
                }
                catch (Exception e)
                {
                    log.ErrorFormat("회원가입 후 로그인 에러 :{0}",e.ToString());
                    ViewBag.message = "잠시후 다시 시도해주시기 바랍니다.";
                    return View("../common/SimpleAlertView");
                }

                return View("../common/SimpleAlertAndRedirectView");
            }
            else
            {
                ViewBag.message = msg;
                return View("../common/SimpleAlertView");
            }
        }

        //public ActionResult JoinProc(Models.JoinForm form)
        //{
        //    string phone = "";
        //    KCPResponseData certResult = null;
        //    try
        //    {
        //        // 인증 데이터 복원
        //        if (string.IsNullOrEmpty(form.kcp_cert_data))
        //            throw new Exception("KCP 인증 데이터를 찾을 수 없습니다");
        //        // KCP 에서 받은 인증 정보를 다시 복구
        //        var json = DBLIB.Service.CryptoHelper.Decrypt(form.kcp_cert_data, InternalFormDataPassword);
        //        if (String.IsNullOrEmpty(json))
        //            throw new Exception("KCP 인증 데이터를 찾을 수 없습니다(2)");
        //        certResult = JsonConvert.DeserializeObject<KCPResponseData>(json);
        //        if (certResult == null)
        //            throw new Exception("KCP 인증 데이터를 찾을 수 없습니다(3)");

        //        form.m_name = certResult.user_name;
        //        phone = MakePrettyPhoneNo(certResult.phone_no);

        //        if (string.IsNullOrWhiteSpace(form.m_id))
        //            throw new ArgumentException("아이디를 입력하세요");
        //        if (string.IsNullOrWhiteSpace(form.m_name))
        //            throw new ArgumentException("이름을 입력하세요");
        //        if (string.IsNullOrWhiteSpace(form.m_nick))
        //            throw new ArgumentException("닉네임을 입력하세요");
        //        if (string.IsNullOrWhiteSpace(form.m_pwd1) || string.IsNullOrWhiteSpace(form.m_pwd2))
        //            throw new ArgumentException("비밀번호를 입력하세요");
        //        if (form.m_pwd1 != form.m_pwd2)
        //            throw new ArgumentException("비밀번호가 일치하지 않습니다");
        //        //if (string.IsNullOrWhiteSpace(form.m_bank_pwd))
        //        //throw new ArgumentException("보관함 비밀번호를 입력하세요");
        //        //if (string.IsNullOrWhiteSpace(form.m_phone1) || string.IsNullOrWhiteSpace(form.m_phone2) || string.IsNullOrWhiteSpace(form.m_phone3))
        //        //throw new ArgumentException("휴대폰 번호를 입력하세요");

        //        // 추천인이 없는 경우 기본 매장 처리?
        //        //if (string.IsNullOrEmpty(form.m_recomid))
        //        //    form.m_recomid = "홍길동";

        //        if (form.m_name.Length < 1 || form.m_name.Length > 10 || userService.alphaNumericOnly.IsMatch(form.m_name))
        //            throw new ArgumentException("이름은 한글만 가능합니다");
        //        if (form.m_id.Length < 6 || form.m_id.Length > 20 || !userService.alphaOnly.IsMatch(form.m_id.First().ToString()) || userService.koreansOnly.IsMatch(form.m_id))
        //            throw new ArgumentException("아이디는 영문과 숫자 조합으로 6~20자만 가능합니다");
        //        if (form.m_nick.Length < 2 || form.m_nick.Length > 7 || userService.alphaNumericOnly.IsMatch(form.m_nick))
        //            throw new ArgumentException("닉네임은 한글 2~7자만 가능합니다");

        //        if (!userService.IsAvailableId(form.m_id))
        //            throw new ArgumentException("이미 사용중인 아이디입니다");
        //        if (!userService.IsAvailableNick(form.m_nick))
        //            throw new ArgumentException("이미 사용중인 닉네임입니다");
        //        if (form.m_recomid == null)
        //        {
        //            form.m_recomid = "";
        //        }
        //        if (form.m_recomid.Length > 0 && userService.IsAvailableCode(form.m_recomid))
        //            throw new ArgumentException("유효하지 않은 추천인입니다");
        //        /*
        //        if(!string.IsNullOrEmpty(form.m_recomid ) && userService.IsAvailableId(form.m_recomid))
        //            throw new ArgumentException("잘못된 추천인 아이디입니다");
        //        if (form.m_recomid == form.m_id)
        //            throw new ArgumentException("추천인에 본인 아이디를 입력할 수 없습니다");
        //            */
        //    }
        //    catch (ArgumentException e)
        //    {
        //        ViewBag.message = e.Message;
        //        return View("../common/SimpleAlertView");
        //    }

        //    string msg;
        //    //bool registerOk = userService.AddUser(form.m_id, form.m_name, form.m_nick, form.m_pwd1, form.m_bank_pwd, phone, out msg);
        //    bool registerOk = userService.AddUser(UserService.CreatedFrom.WEB, form.m_id, form.m_name, form.m_nick, form.m_pwd1, "", phone, form.m_recomid, certResult.ci, certResult.di, certResult.cert_no, out msg);

        //    if (registerOk)
        //    {
        //        ViewBag.message = "회원 가입을 축하드립니다. 로그인 후 이용 가능합니다";
        //        ViewBag.redirect = "/";
        //        ViewBag.frame = "parent";
        //        return View("../common/SimpleAlertAndRedirectView");
        //    }
        //    else
        //    {
        //        ViewBag.message = "서버 오류로 요청을 처리하지 못했습니다.";
        //        return View("../common/SimpleAlertView");
        //    }
        //}
        [Authorize]
        public ActionResult MyInfo()
        {
            ViewBag.Id = User.Identity.GetUserId();
            ViewBag.Name = User.Identity.GetUserName();
            ViewBag.Phone = User.Identity.GetPhone();

            return View();
        }

        [Authorize]
        public ActionResult MyInfoProc(MyInfoForm form)
        {
            string message = "";
            bool ok = userService.ChangePassword(User.Identity.GetUserId(), form.m_pwd_new1, form.m_pwd_new2, form.m_pwd_org, out message);

            if (ok)
            {
                ViewBag.message = "정보 수정 완료";
                ViewBag.redirect = "/";
                ViewBag.frame = "parent";

                return View("../common/SimpleAlertAndRedirectView");
            }
            else
            {
                ViewBag.message = message;
                return View("../common/SimpleAlertView");
            }
            //string message = "준비중입니다.";
            //bool ok =userService.ChangeNickname(User.Identity.GetUserId(), v_nick, out message);

            //if (ok)
            //{
            //    loginHelper.UpdateNickname(v_nick);
            //}
            //else
            //{
            //    ViewBag.message = message;
            //    return View("../common/SimpleAlertAndClose");
            //}

            //ViewBag.Id = User.Identity.GetUserId();
            //ViewBag.Name = User.Identity.GetUserName();
            //ViewBag.Phone = User.Identity.GetPhone();

            //return View();
        }

        /// <summary>
        /// 구입한 아바타 목록을 보여준다
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MyRoom()
        {
            var items = productService.GetHavingItems(User.Identity.GetUserId());
            return View(items);
        }

        [HttpPost]
        [Authorize]
        public JsonResult MyRoomAction(int itemId)
        {
            StandardResult res = new StandardResult();

            try
            {
                string message;
                bool ok = userService.UseMyRoomItem(User.Identity.GetUserId(), itemId, out message);

                res = ok ? StandardResult.createSucceeded(message) : StandardResult.createError(message);
            }
            catch (Exception e)
            {
                log.Error("이용자[" + User.Identity.GetUserId() + "]의 아이템[" + itemId + "] 사용 중 오류", e);
                res = StandardResult.createError(e.Message);
            }

            return Json(res);
        }

        /// <summary>
        /// 마일리지 교환 UI
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Exchange(int? PageNo, int? StartPage)
        {
            ExchangeViewData data = new ExchangeViewData();

            data.pageNo = PageNo.GetValueOrDefault(1);
            data.startPage = StartPage.GetValueOrDefault(1);
            data.totalPages = (int)Math.Ceiling((double)chargeService.GetMileageExchangeHistoryCount(User.Identity.GetUserId()) / (double)Properties.Settings.Default.PAGING_SIZE);

            data.availPoint = chargeService.GetAvailMileage(User.Identity.GetUserId());
            data.availPoint2 = chargeService.GetAvailMileage2(User.Identity.GetUserId());
            data.items = chargeService.GetMileageExchangeHistory(User.Identity.GetUserId(), data.pageNo, Properties.Settings.Default.PAGING_SIZE);
            return View(data);
        }

        // 마일리지 교환 처리
        [HttpPost]
        [Authorize]
        public JsonResult PerformExchange(long? mileage)
        {
            StandardResult res = new StandardResult();

            try
            {
                if (!mileage.HasValue)
                    throw new Exception("전환할 마일리지를 입력해 주세요");
                
                if (mileage.Value <= 0)
                    throw new Exception("최소 1 마일리지 이상 입력해 주세요");

                string message;
                bool ok = chargeService.ExchangeMileage(User.Identity.GetUserId(), mileage.Value, out message);

                res = ok ? StandardResult.createSucceeded(message) : StandardResult.createError(message);
            }catch(Exception e)
            {
                log.Error("이용자[" + User.Identity.GetUserId() + "] 마일리지 교환 중 오류", e);
                res = StandardResult.createError(e.Message);
            }

            return Json(res);
        }
 
        public ActionResult Logout()
        {
            loginHelper.MakeLogout();
            return Redirect("/");
        }

        /// <summary>
        /// 탈퇴
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MemOut()
        {
            return View();
        }

        /// <summary>
        /// 탈퇴 처리
        /// </summary>
        /// <param name="m_pwd"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult PerformMemberOut(string m_pwd, string reason)
        {
            StandardResult result = null;
            try
            {
                if (string.IsNullOrEmpty(m_pwd))
                    throw new Exception("비밀번호를 입력해 주세요");
                if (string.IsNullOrEmpty(reason))
                    throw new Exception("탈퇴 사유를 입력해 주세요");

                // 비활성화+사유 등록
                userService.QuitUser(User.Identity.GetUserId(), m_pwd, reason);

                // 로그아웃
                loginHelper.MakeLogout();
                result = StandardResult.createSucceeded();
            }catch(Exception e)
            {
                result = StandardResult.createError(e.Message);
            }

            return Json(result);
        }

        public class AdminMemoData
        {
            public List<PlayerAdminMemo> items;
            public int totalPages;
            public int currentPageNo;
            public int startPage;
        }

        [Authorize]
        public ActionResult Memo(int? PageNo, int? StartPage)
        {
            int currentPageNo = PageNo.HasValue ? PageNo.Value : 1;                 
            
            AdminMemoData data = new AdminMemoData();
            data.items = userService.GetAdminMemoList(User.Identity.GetUserId(), currentPageNo); ;

            data.currentPageNo = currentPageNo;
            data.totalPages = (int) Math.Ceiling (  (double) userService.GetAdminMemoCount(User.Identity.GetUserId()) / (double) Properties.Settings.Default.PAGING_SIZE ) ;
            data.startPage = StartPage.GetValueOrDefault(1);

            return View(data);
        }

        /// <summary>
        /// 관리자 쪽지 삭제
        /// </summary>
        /// <param name="memo_list"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult MemoDel(int[] memo_list)
        {
            userService.DeleteAdminMemo(User.Identity.GetUserId(), memo_list);
            ViewBag.message = "삭제되었습니다";
            ViewBag.redirect = "/member/memo";
            ViewBag.frame = "parent";
            return View("../common/SimpleAlertAndRedirectView");
        }

        // Q&A
        public class PopupAuthorizeAttribute : AuthorizeAttribute
        {
            private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
            {
                validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
            }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }

            protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
            {
                return base.OnCacheAuthorization(httpContext);
            }
            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                bool isAuthorized = false;
                if (filterContext == null)
                {
                    throw new ArgumentNullException("filterContext");
                }
                if (AuthorizeCore(filterContext.HttpContext))
                {
                    HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                    cache.SetProxyMaxAge(new TimeSpan(0L));
                    cache.AddValidationCallback(CacheValidateHandler, null);
                    isAuthorized = true;
                }

                filterContext.Controller.ViewData["OpenAuthorizationPopup"] = !isAuthorized;
            }
        }
        [PopupAuthorize]
        public ActionResult QnA(int? PageNo, int? StartPage)
        {
            var data = new QnaPagingData();
            data.PageNo = PageNo.GetValueOrDefault(1);
            data.List = qnaService.GetList(User.Identity.GetUserId(), data.PageNo);
            data.TotalPages = qnaService.GetTotalPageCount(User.Identity.GetUserId(), Properties.Settings.Default.PAGING_SIZE);
            data.StartPage = StartPage.GetValueOrDefault(1);
            return View(data);
        }

        // 문의 등록
        [Authorize]
        public ActionResult QnAWrite()
        {
            return View(qnaService.GetArticleTypes());
        }

        /// <summary>
        /// 전송된 문의 내용 DB 에 등록
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult PerformQnAWrite(QnaForm form)
        {
            StandardResult result = new StandardResult();
            var articleTypeKeys = qnaService.GetArticleTypes().Select(a => a.Key).ToList();

            try
            {
                // test
                if (string.IsNullOrEmpty(form.v_title))
                    throw new Exception("제목을 입력해주세요");
                if (string.IsNullOrEmpty(form.v_content))
                    throw new Exception("본문을 입력해주세요");
                if (string.IsNullOrEmpty(form.questionType) || !articleTypeKeys.Contains(form.questionType))
                    throw new Exception("질문 유형이 잘못되었습니다");

                qnaService.WriteArticle(form, User.Identity.GetUserId());

                result = StandardResult.createSucceeded();
            }catch(Exception e)
            {
                result = StandardResult.createError(e.Message);
            }

            return Json(result);
        }

        /// <summary>
        /// QnA 보기
        /// </summary>
        /// <param name="n_no"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult QnaView(int n_no)
        {
            var userId = User.Identity.GetUserId();
            qna article = qnaService.LoadArticle(userId, n_no);
            if(article == null)
            {
                log.Error("1:1문의글을 찾을 수 없음. UserID = " + userId + ", articleNo = " + n_no);
                return Redirect("/member/qna");
            }

            return View(article);
        }

        /// <summary>
        /// 닉네임 변경
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult NickChange()
        {
            ViewBag.nickname = User.Identity.GetUserName();
            ViewBag.CanChange = userService.CanChangeNickname(User.Identity.GetUserId());
            return View();
        }

        [Authorize]
        public ActionResult UpdateNickname(string v_nick)
        {
            string message = "";
            bool ok =userService.ChangeNickname(User.Identity.GetUserId(), v_nick, out message);
            ViewBag.message = message;

            if (ok)
                loginHelper.UpdateNickname(v_nick);

            return View("../common/SimpleAlertAndClose");
        }


        /// <summary>
        /// 쿠폰 UI
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Coupon(int? PageNo, int? StartPage)
        {
            CouponViewData data = new CouponViewData();

            data.pageNo = PageNo.GetValueOrDefault(1);
            data.startPage = StartPage.GetValueOrDefault(1);
            data.totalPages = (int)Math.Ceiling((double)chargeService.GetCouponHistoryCount(User.Identity.GetUserId()) / (double)Properties.Settings.Default.PAGING_SIZE);

            data.items = chargeService.GetCouponHistory(User.Identity.GetUserId(), data.pageNo, Properties.Settings.Default.PAGING_SIZE);
            data.items.ForEach(x => x.Serial = (x.Serial.Insert(12, "-")).Insert(8,"-").Insert(4,"-"));

            return View(data);
        }

        // 쿠폰 처리
        [HttpPost]
        [Authorize]
        public JsonResult PerformCoupon(string coupon)
        {
            StandardResult res = new StandardResult();
            coupon = coupon.Replace(" ", "").Replace("-", "");
            try
            {
                if (coupon.Length != 16)
                    throw new Exception("쿠폰 일련번호는 16자리입니다.\n확인 후 입력해주세요");

                string message;
                bool ok = chargeService.UseCoupon(User.Identity.GetUserId(), coupon, out message);

                res = ok ? StandardResult.createSucceeded(message) : StandardResult.createError(message);
            }
            catch (Exception e)
            {
                log.Error("이용자[" + User.Identity.GetUserId() + "] 쿠폰 사용 중 오류", e);
                res = StandardResult.createError(e.Message);
            }

            return Json(res);
        }

        // 캐쉬충천 페이지
        [Authorize]
        public ActionResult cash()
        {
            return View();
        }

        [Authorize]
        public ActionResult cash2()
        {
            return Content("<script language='javascript' type='text/javascript'>alert('결제 프로세스 준비중');window.location = '/Main/index';</script>");
        }

        // GET: Member/MemberList
        public ActionResult MemberList(string date)
        {
            // 날짜 확인
            UserWeb.Models.MemberListData data = new MemberListData();

            if (date != null)
            {
                data.date = date;
            }

            return View(data);
        }

        [Authorize]
        public ActionResult MemberUserList(string date)
        {
            var FriendID = userService.GetPlayerInfo(User.Identity.GetUserId()).HaveFriend;

            if (date != null)
            {
                DateTime temp;
                if (DateTime.TryParse(date, out temp))
                {
                    var days = (DateTime.Now - temp).Days;
                    if (days > 0)
                    {
                        var obj2 = new { data = userService.GetMemberUserList(FriendID, days) };
                        return Json(obj2, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            var obj = new { data = userService.GetMemberUserList(FriendID) };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult MemberBonusList(string date)
        {
            var FriendID = userService.GetPlayerInfo(User.Identity.GetUserId()).HaveFriend;

            if (date != null)
            {
                DateTime temp;
                if (DateTime.TryParse(date, out temp))
                {
                    var days = (DateTime.Now - temp).Days;
                    if(days > 0)
                    {
                        var obj2 = new { data = userService.GetMemberBonusList(FriendID, days) };
                        return Json(obj2, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            var obj = new { data = userService.GetMemberBonusList(FriendID) };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FriendList()
        {
            return View();
        }

        [Authorize]
        public ActionResult FriendListData(int? day)
        {
            var FriendID = userService.GetPlayerInfo(User.Identity.GetUserId()).HaveFriend;

            if (day.HasValue && day >= 1 && day <= 3)
            {
                // 스냅샷
                var snapshot = new { data = userService.GetFriendSanpShotListData(FriendID, day.Value) };
                return Json(snapshot, JsonRequestBehavior.AllowGet);
            }

            var obj = new { data = userService.GetFriendListData(FriendID) };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult MyLog(int? PageNo, int? StartPage)
        {
            MyLogData data = new MyLogData();

            data.pageNo = PageNo.GetValueOrDefault(1);
            data.startPage = StartPage.GetValueOrDefault(1);
            data.totalPages = (int) Math.Ceiling (  (double) userService.GetMyLogCount(User.Identity.GetUserId()) / (double) Properties.Settings.Default.PAGING_SIZE ) ;

            data.items = userService.GetMyLogList(User.Identity.GetUserId(), data.pageNo);

            return View(data);
        }

        [Authorize]
        public ActionResult EventJH()
        {
            EventJackpotHunter data = userService.GetEventJackpotHunterInfo(User.Identity.GetUserId());

            if(data == null)
            {
                data = new EventJackpotHunter();
            }

            return View(data);
        }

        /// <summary>
        /// 이용제한 자가설정
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SelfRestricted()//RestrictedSelfSetting()
        {
            RestrictedViewData data = new RestrictedViewData();

            data.restrictedTime = userService.GetRestrictedTime(User.Identity.GetUserId());

            if(data.restrictedTime > DateTime.Now)
            {
                data.isRestricted = true;
            }
            else
            {
                data.isRestricted = false;
            }

            return View(data);
        }

        /// <summary>
        /// 이용제한 설정
        /// </summary>
        /// <param name="m_pwd"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult SelfRestrictedSetting(SetRestrictForm form)
        {
            string message = "";

            // 이용제한 설정
            bool ok = userService.RestrictUser(User.Identity.GetUserId(), form.m_setnum, out message);

            if (ok)
            {
                ViewBag.message = message;
                ViewBag.redirect = "/";
                ViewBag.frame = "parent";

                return View("../common/SimpleAlertAndRedirectView");
            }
            else
            {
                ViewBag.message = message;
                return View("../common/SimpleAlertView");
            }
        }

    }
}