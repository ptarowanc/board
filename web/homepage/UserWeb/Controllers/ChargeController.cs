using DBLIB.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserWeb.Service;

namespace UserWeb.Controllers
{
    public class ChargeController : Controller
    {
        readonly ChargeService chargeService;

        public ChargeController(ChargeService chargeService)
        {
            this.chargeService = chargeService;
        }

        // 충전 안내
        public ActionResult cashcharge()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                ViewBag.message = "로그인이 필요한 메뉴입니다";
                return View("../common/SimpleAlertAndClose");
            }

            ViewBag.userName = User.Identity.Name;
            ViewBag.userCash = chargeService.GetCurrentAvailableCash(User.Identity.GetPlayerName());
            return View();
        }

        public enum PGMethod
        {
            Trans,          // 계좌이체
            Culture,        // 문화상품권
            Prepaidcard     // 선불카드
        }

        [Authorize]
        [HttpPost]
        public ActionResult CashChargeProc(PGMethod pg_method, string n_cash_no)
        {
            return View();
        }
    }
}