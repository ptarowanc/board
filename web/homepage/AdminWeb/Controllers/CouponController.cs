using AdminWeb.Models.DataTables;
using DBLIB.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.Controllers
{
    // 쿠폰
    public class CouponController : Controller
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(CouponController));
        CouponService couponService;

        public CouponController(CouponService couponService)
        {
            this.couponService = couponService;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public JsonResult CouponList(Models.DataTables.DataTableRequestParameter parameters)
        {
            var response = new CouponDataTableResponse();
            response.recordsFiltered = response.recordsTotal = couponService.GetAllCouponCount();

            var firstSortColumn = parameters.order.FirstOrDefault();
            var firstSortColumnOrder = firstSortColumn != null ? firstSortColumn.dir : DBLIB.Service.OrderDirectionEnum.asc;

            log.Info("소트. firstSortColumn=" + firstSortColumn + ", firstSortColumnOrder=" + firstSortColumnOrder);

            var entries = couponService.GetCouponList(parameters.start, parameters.length, firstSortColumn != null ? firstSortColumn.column : 0, firstSortColumnOrder, parameters.search.value);
            response.data = entries.Select(a => CouponData.Create(a)).ToList();

            return Json(response);
        }
        
        [Authorize]
        public ActionResult Log()
        {
            return View();
        }

        [Authorize]
        public JsonResult CouponLog(Models.DataTables.DataTableRequestParameter parameters)
        {
            var response = new CouponLogDataTableResponse();
            response.recordsFiltered = response.recordsTotal = couponService.GetAllCouponLogCount();

            var firstSortColumn = parameters.order.FirstOrDefault();
            var firstSortColumnOrder = firstSortColumn != null ? firstSortColumn.dir : DBLIB.Service.OrderDirectionEnum.asc;

            log.Info("소트. firstSortColumn=" + firstSortColumn + ", firstSortColumnOrder=" + firstSortColumnOrder);

            var entries = couponService.GetCouponLogList(parameters.start, parameters.length, firstSortColumn != null ? firstSortColumn.column : 0, firstSortColumnOrder, parameters.search.value);
            response.data = entries.Select(a => CouponLogData.Create(a)).ToList();

            return Json(response);
        }
    }
}