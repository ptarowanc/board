using DBLIB.Service;
using UserWeb.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using UserWeb.Models;
using DBLIB.Model;

namespace UserWeb.Controllers
{
    public class GameController : Controller
    {
        readonly ProductService productService;
        readonly GameRankingService gameRankingService;
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(GameController));
        readonly EventService eventService;
        readonly UserService userService;

        public GameController(ProductService productService, GameRankingService gameRankingService, EventService eventService, UserService userService)
        {
            this.productService = productService;
            this.gameRankingService = gameRankingService;
            this.eventService = eventService;
            this.userService = userService;
        }

        public ActionResult GameDownload()
        {
            return View();
        }

        /// <summary>
        /// 바둑이 실시간 랭킹 TOP 20
        /// </summary>
        /// <returns></returns>
        public ActionResult Game01(string s_day)
        {
            GameSelectionData data = new GameSelectionData();
            data.s_day = s_day;
            data.items = gameRankingService.GetRankingList(DBLIB.Model.GameTypeEnum.Baduki, 10);
            return View(data);
        }
        public ActionResult Game02(string s_day)
        {
            GameSelectionData data = new GameSelectionData();
            data.s_day = s_day;
            data.items = gameRankingService.GetRankingList(DBLIB.Model.GameTypeEnum.Matgo, 10);
            return View(data);
        }
        public ActionResult Game03()
        {
            return View();
        }

        /// <summary>
        /// 다운로드 안내
        /// </summary>
        /// <returns></returns>
        public ActionResult GameInfo()
        {
            return View();
        }

        public ActionResult CustomerService()
        {
            return View();
        }

        public ActionResult EventLotto(int? PageNo, int? StartPage, int? eventNumber)
        {
            EventLottoData data = new EventLottoData();

            data.pageNo = PageNo.GetValueOrDefault(1);
            data.totalPages = eventService.GetTotalPageCountEventLottoEnter(User.Identity.GetUserId(), Properties.Settings.Default.PAGING_SIZE);
            data.startPage = StartPage.GetValueOrDefault(1);

            string userId = User.Identity.GetUserId();
            if (User.Identity.IsAuthenticated)
            {
                data.UserData = true;
                data.UserLottoCount = eventService.GetUserEventLottoCount(userId);
                data.UserLottoEnterList = eventService.GetUserEventLottoEnterList(userId, data.pageNo, Properties.Settings.Default.PAGING_SIZE);
            }
            else
            {
                data.UserData = false;
            }

            data.LottoResultCount = eventService.GetEventLottoResultCount();
            data.Lotto = eventService.GetEventLotto();
            data.LottoResult = eventService.GetEventLottoResultList(eventNumber.GetValueOrDefault(0));

            return View(data);
        }

        //[Authorize]
        /// <summary>
        /// 아바타샵 리스트
        /// </summary>
        /// <returns></returns>
        public ActionResult Avatar(short? ViewLabel)
        {
            AvatarShopModel data = new AvatarShopModel();

            data.webShopItemListAvata = productService.GetAvailableShopList("avatar");
            //data.webShopItemListAvata = productService.GetAvailableShopList("avatar2");
            data.webShopItemListCard = productService.GetAvailableShopList("card");
            if (User.Identity.IsAuthenticated)
            {
                data.Login = true;
                var playerinfo = userService.GetPlayerInfo(User.Identity.GetUserId());
                data.Cash = playerinfo.Cash;
                data.GameMoney = (long)playerinfo.FreeMoney;
                data.GameMoney2 = (long)playerinfo.PayMoney;
                data.SafeMoney = (long)playerinfo.SafeMoney;
                data.Mileage = (long)playerinfo.Mileage;
            }
            else
            {
                data.Login = false;
            }

            if(ViewLabel.HasValue)
            {
                data.View = ViewLabel.Value;
            }
            else
            {
                data.View = 0;
            }

            return View(data);
        }
        
        //[Authorize]
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

            if(ViewLabelString == "avatar")
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
            ViewBag.redirect = "/game/avatar?ViewLabel=" + ViewLabel.ToString();
            ViewBag.frame = "parent";
            return View("../common/SimpleAlertAndRedirectView");
        }
        
        // 로또 이벤트 응모
        [HttpPost]
        [Authorize]
        public JsonResult EventLottoEnter(int? number0, int? number1, int? number2, int? number3, int? number4, int? number5)
        {
            StandardResult res = new StandardResult();

            int nummax = 45;
            try
            {
                if (!number0.HasValue || !number1.HasValue || !number2.HasValue || !number3.HasValue || !number4.HasValue || !number5.HasValue)
                    throw new Exception("응모번호를 입력해주세요.");

                if ((number0.Value <= 0 || number0.Value > nummax || number1.Value <= 0 || number1.Value > nummax || number2.Value <= 0 || number2.Value > nummax ||
                    number3.Value <= 0 || number3.Value > nummax || number4.Value <= 0 || number4.Value > nummax || number5.Value <= 0 || number5.Value > nummax) ||
                    (number0.Value == number1.Value || number0.Value == number2.Value || number0.Value == number3.Value || number0.Value == number4.Value || number0.Value == number5.Value ||
                    number1.Value == number2.Value || number1.Value == number3.Value || number1.Value == number4.Value || number1.Value == number5.Value ||
                    number2.Value == number3.Value || number2.Value == number4.Value || number2.Value == number5.Value ||
                    number3.Value == number4.Value || number3.Value == number5.Value ||
                    number4.Value == number5.Value)
                    )
                    throw new Exception("응모번호를 다시 확인해주세요.");

                string message;
                bool ok = eventService.EventLottoEnter(User.Identity.GetUserId(), number0.Value, number1.Value, number2.Value, number3.Value, number4.Value, number5.Value, out message);

                res = ok ? StandardResult.createSucceeded(message) : StandardResult.createError(message);
            }
            catch (Exception e)
            {
                log.Error("이용자[" + User.Identity.GetUserId() + "] 로또 이벤트 응모 오류", e);
                res = StandardResult.createError(e.Message);
            }

            return Json(res);
        }
    }
}