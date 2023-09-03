using DBLIB.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserWeb.Controllers
{
    public class GameApiController : Controller
    {
        readonly ProductService productService;

        public GameApiController(ProductService productService)
        {
            this.productService = productService;
        }

        public class ShopItem1
        {
            public string pid;
            public string pname;
            public string img;
            public string freemoney;
            public string paymoney;
            public int period;
            public string purchase_kind;
            public int price;
        }
        public class ShopItem2
        {
            public string pid;
            public string pname;
            public string img;
            public string cash;
            public string purchase_kind;
            public int price;
        }

        public class CashShopItemListResponse
        {
            public string error = "";
            public ShopItem1[] avatar;
            public ShopItem1[] card;
            public ShopItem1[] evt;
            public ShopItem2[] charge;
        }

        /// <summary>
        /// 상점 목록 API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CashShopItemList()
        {
            var result = new CashShopItemListResponse();

            try
            {
                var AvatarList = productService.GetPurchasableItemList("avatar");
                var CardList = productService.GetPurchasableItemList("card");
                var EventList = productService.GetPurchasableItemList("evt");
                var ChargeList = productService.GetPurchasableItemList("charge");

                result.avatar = AvatarList
                                    .Select(a => new ShopItem1()
                                    {
                                        pid = a.pid,
                                        pname = a.pname,
                                        img = a.img,
                                        freemoney = a.paidstring1,
                                        paymoney = a.paidstring2,
                                        period = Convert.ToInt32(a.paidvalue3.GetValueOrDefault(0)),
                                        purchase_kind = a.purchase_kind,
                                        price = a.price
                                    })
                                    .ToArray();
                result.card = CardList
                                    .Select(a => new ShopItem1()
                                    {
                                        pid = a.pid,
                                        pname = a.pname,
                                        img = a.img,
                                        freemoney = a.paidstring1,
                                        paymoney = a.paidstring2,
                                        period = Convert.ToInt32(a.paidvalue3.GetValueOrDefault(0)),
                                        purchase_kind = a.purchase_kind,
                                        price = a.price
                                    })
                                    .ToArray();
                result.evt = EventList
                                    .Select(a => new ShopItem1()
                                    {
                                        pid = a.pid,
                                        pname = a.pname,
                                        img = a.img,
                                        freemoney = a.paidstring1,
                                        paymoney = a.paidstring2,
                                        period = Convert.ToInt32(a.paidvalue3.GetValueOrDefault(0)),
                                        purchase_kind = a.purchase_kind,
                                        price = a.price
                                    })
                                    .ToArray();
                result.charge = ChargeList
                                    .Select(a => new ShopItem2()
                                    {
                                        pid = a.pid,
                                        pname = a.pname,
                                        img = a.img,
                                        cash = Convert.ToString(a.paidvalue1.GetValueOrDefault(0)),
                                        purchase_kind = a.purchase_kind,
                                        price = a.price
                                    })
                                    .ToArray();
            }
            catch
            {
                result.error = "목록을 불러오지 못했습니다.";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 상점 목록 API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CashShopItemListBadugi()
        {
            var result = new CashShopItemListResponse();

            try
            {
                var AvatarList = productService.GetPurchasableItemList("avatar");
                var CardList = productService.GetPurchasableItemList("card2");
                var EventList = productService.GetPurchasableItemList("evt");
                var ChargeList = productService.GetPurchasableItemList("charge");

                result.avatar = AvatarList
                                    .Select(a => new ShopItem1()
                                    {
                                        pid = a.pid,
                                        pname = a.pname,
                                        img = a.img,
                                        freemoney = a.paidstring1,
                                        paymoney = a.paidstring2,
                                        period = Convert.ToInt32(a.paidvalue3.GetValueOrDefault(0)),
                                        purchase_kind = a.purchase_kind,
                                        price = a.price
                                    })
                                    .ToArray();
                result.card = CardList
                                    .Select(a => new ShopItem1()
                                    {
                                        pid = a.pid,
                                        pname = a.pname,
                                        img = a.img,
                                        freemoney = a.paidstring1,
                                        paymoney = a.paidstring2,
                                        period = Convert.ToInt32(a.paidvalue3.GetValueOrDefault(0)),
                                        purchase_kind = a.purchase_kind,
                                        price = a.price
                                    })
                                    .ToArray();
                result.evt = EventList
                                    .Select(a => new ShopItem1()
                                    {
                                        pid = a.pid,
                                        pname = a.pname,
                                        img = a.img,
                                        freemoney = a.paidstring1,
                                        paymoney = a.paidstring2,
                                        period = Convert.ToInt32(a.paidvalue3.GetValueOrDefault(0)),
                                        purchase_kind = a.purchase_kind,
                                        price = a.price
                                    })
                                    .ToArray();
                result.charge = ChargeList
                                    .Select(a => new ShopItem2()
                                    {
                                        pid = a.pid,
                                        pname = a.pname,
                                        img = a.img,
                                        cash = Convert.ToString(a.paidvalue1.GetValueOrDefault(0)),
                                        purchase_kind = a.purchase_kind,
                                        price = a.price
                                    })
                                    .ToArray();
            }
            catch
            {
                result.error = "목록을 불러오지 못했습니다.";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public class MyRoomItemListEntry
        {
            public string pid;
            public string pname;
            public string img;
            public string expire;
            public int count;
            public bool use;
        }

        public class MyRoomItemListResponse
        {
            public string error = "";
            public MyRoomItemListEntry[] avatar;
            public MyRoomItemListEntry[] card;
            public MyRoomItemListEntry[] evt;
        }

        [HttpGet]
        public JsonResult MyRoomItemList(string UserId)
        {
            MyRoomItemListResponse res = new MyRoomItemListResponse();

            try
            {
                var all = productService.GetMyRoomItemList(UserId);
                var avatar = all.Where(a => a.ptype == "avatar").Select(a => new MyRoomItemListEntry() { pid = a.Id.ToString(), pname = a.pname, img = a.img, count = a.Count, use = a.Using, expire = a.ExpireDate.HasValue ? a.ExpireDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "" }).ToArray();
                var card = all.Where(a => a.ptype == "card").Select(a => new MyRoomItemListEntry() { pid = a.Id.ToString(), pname = a.pname, img = a.img, count = a.Count, use = a.Using, expire = a.ExpireDate.HasValue ? a.ExpireDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "" }).ToArray();
                var evt = all.Where(a => a.ptype == "evt").Select(a => new MyRoomItemListEntry() { pid = a.Id.ToString(), pname = a.pname, img = a.img, count = a.Count, use = a.Using, expire = a.ExpireDate.HasValue ? a.ExpireDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "" }).ToArray();

                res.avatar = avatar;
                res.card = card;
                res.evt = evt;
            }
            catch//( Exception e)
            {
                res.error = "목록을 불러오지 못했습니다. ※UserId="+ UserId;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult MyRoomItemListBadugi(string UserId)
        {
            MyRoomItemListResponse res = new MyRoomItemListResponse();

            try
            {
                var all = productService.GetMyRoomItemList(UserId);
                var avatar = all.Where(a => a.ptype == "avatar").Select(a => new MyRoomItemListEntry() { pid = a.Id.ToString(), pname = a.pname, img = a.img, count = a.Count, use = a.Using, expire = a.ExpireDate.HasValue ? a.ExpireDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "" }).ToArray();
                var card = all.Where(a => a.ptype == "card2").Select(a => new MyRoomItemListEntry() { pid = a.Id.ToString(), pname = a.pname, img = a.img, count = a.Count, use = a.Using, expire = a.ExpireDate.HasValue ? a.ExpireDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "" }).ToArray();
                var evt = all.Where(a => a.ptype == "evt").Select(a => new MyRoomItemListEntry() { pid = a.Id.ToString(), pname = a.pname, img = a.img, count = a.Count, use = a.Using, expire = a.ExpireDate.HasValue ? a.ExpireDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "" }).ToArray();

                res.avatar = avatar;
                res.card = card;
                res.evt = evt;
            }
            catch//( Exception e)
            {
                res.error = "목록을 불러오지 못했습니다. ※UserId=" + UserId;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public class VersionCheckResponse
        {
            public string error = "";
            public string version;
            public string market;
        }

        [HttpGet]
        public JsonResult VersionCheck()
        {
            VersionCheckResponse res = new VersionCheckResponse();
            try
            {
                res.version = UserWeb.Properties.Settings.Default.API_MATGO_VERSION;
                res.market = "market://details?id="+ UserWeb.Properties.Settings.Default.MARKET_ID_MATGO;
            }
            catch
            {
                res.error = "목록을 불러오지 못했습니다.";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult VersionCheckBadugi()
        {
            VersionCheckResponse res = new VersionCheckResponse();
            try
            {
                res.version = UserWeb.Properties.Settings.Default.API_BADUGI_VERSION;
                res.market = "market://details?id="+ UserWeb.Properties.Settings.Default.MARKET_ID_BADUGI;
            }
            catch
            {
                res.error = "목록을 불러오지 못했습니다.";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}