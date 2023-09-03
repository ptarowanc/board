using DBLIB.Model;
using DBLIB.Service;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserWeb.Models;
using UserWeb.Service;

namespace UserWeb.Controllers
{
    public class MainController : Controller
    {
        readonly ProductService productService;
        readonly WebNoticeService webNoticeService;
        readonly LoginHelper loginHelper;
        readonly UserService userService;
        //readonly GameRankingService gameRankingService;

        public MainController(ProductService productService, WebNoticeService webNoticeService, LoginHelper loginHelper, UserService userService/*, GameRankingService gameRankingService*/)
        {
            this.productService = productService;
            this.webNoticeService = webNoticeService;
            this.loginHelper = loginHelper;
            this.userService = userService;
            //this.gameRankingService = gameRankingService;
        }

        // GET: Main
        public ActionResult Index(string ReturnUrl)
        {
            MainViewModel mv = new MainViewModel();
            mv.popupNotice = webNoticeService.getActiveMainPopupNotice();   // 팝업 공지
            mv.noticeList = webNoticeService.getNoticeList(1, 5);          // 공지사항 리스트;
            mv.IsLogon = User.Identity.IsAuthenticated;

            if (User.Identity.IsAuthenticated)
            {
                // 로그인한 상태
                ViewBag.LoginId = User.Identity.GetUserId();
                ViewBag.Nickname = User.Identity.GetUserName(); ;
                ViewBag.PlayerInfo = userService.GetPlayerInfo(User.Identity.GetUserId());
            }
            else
            {
                ViewBag.returnUrl = ReturnUrl;
            }
            return View(mv);
        }

        /// <summary>
        /// 게임 랭크 html 내리기
        /// </summary>
        /// <param name="rank_no">1 : 바둑이, 2 : 맞고</param>
        /// <returns></returns>
        public ActionResult GameRank(string rank_no)
        {
            //var data = gameRankingService.GetRankingList(rank_no == "1" ? DBLIB.Model.GameTypeEnum.Baduki : DBLIB.Model.GameTypeEnum.Matgo, 10);
            //return View(data);
            return View();
        }

        /// <summary>
        /// 공통 :: 화면 중간에 있는 공지 사항 출력용
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult MiddleNoticeBar()
        {
            MainViewModel mv = new MainViewModel();
            mv.noticeList = webNoticeService.getNoticeList(1, 5);          // 공지사항 리스트;

            return View(mv);
        }

        public ActionResult YouthPolicy()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult TermConditions()
        {
            return View();
        }

        public ActionResult CashPolicy()
        {
            return View();
        }

        public ActionResult AvatarBuyProc(int no)
        {
            string message = "";
            string ViewLabelString = "";
            short ViewLabel = 0;

            if (User.Identity.IsAuthenticated)
            {
                productService.BuyAvatar(User.Identity.GetUserId(), "cash", no, out message, out ViewLabelString);
            }
            else
            {
                message = "로그인 후 구매할 수 있습니다.";
            }

            if (ViewLabelString == "avatar")
            {
                ViewLabel = 0; // 아바타
            }
            else if (ViewLabelString == "card")
            {
                ViewLabel = 1; // 화투패
            }
            else if (ViewLabelString == "card2")
            {
                ViewLabel = 2; // 포커카드
            }

            ViewBag.message = message;
            ViewBag.redirect = "/Main/Index";
            ViewBag.frame = "parent";
            return View("../common/SimpleAlertAndRedirectView");
        }

        [HttpPost]
        /// <summary>
        /// 로그인 폼 전송
        /// </summary>
        /// <returns></returns>
        public JsonResult PerformLogin(string m_id, string m_pw, string returnUrl, string m_save)
        {
            // ID, PWD 유효성 체크
            // 정보 불러와서 로그인 상태 만들기
            // 페이지 이동 

            try
            {
                if (string.IsNullOrWhiteSpace(m_id) || string.IsNullOrWhiteSpace(m_pw))
                    throw new Exception("ID, 비밀번호를 확인해 주세요");

                loginHelper.SetPreLogin(m_id, m_pw);

                var result = userService.TryLogin(m_id, m_pw);
                if (result == null)
                    throw new Exception("ID 또는 비밀번호를 확인해 주세요");

                loginHelper.MakeLogon(m_id, result, false);

                var res = StandardResult.createSucceeded();
                res.data = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
                return Json(res);
            }
            catch (Exception e)
            {
                return Json(StandardResult.createError(e.ToString(), e.Message));
            }
        }

        [ChildActionOnly]
        public ActionResult LoginBox(string returnUrl)
        {
            ViewBag.idsave = loginHelper.GetSavedLoginID();
            ViewBag.isLogon = User.Identity.IsAuthenticated;
            if (User.Identity.IsAuthenticated)
            {
                // 로그인한 상태
                ViewBag.LoginId = User.Identity.GetUserId();
                ViewBag.Nickname = User.Identity.GetUserName(); ;
                ViewBag.PlayerInfo = userService.GetPlayerInfo(User.Identity.GetUserId());
            }
            else
            {
                ViewBag.returnUrl = returnUrl;
            }
            return View();
        }

        [ChildActionOnly]
        public ActionResult IsLogin()
        {
            ViewBag.isLogon = User.Identity.IsAuthenticated;

            return View();
        }
    }
}