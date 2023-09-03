using AdminWeb.Models.DataTables;
using DBLIB.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.Controllers
{
    // 구매 아이템 관리
    public class PurchaseController : Controller
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(PurchaseController));
        ProductService productService;

        public PurchaseController(ProductService productService)
        {
            this.productService = productService;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public JsonResult PurchaseList(Models.DataTables.DataTableRequestParameter parameters)
        {
            var productCount = productService.GetAllPurchaseItemCount();

            var response = new PurchaseDataTableResponse();
            response.recordsFiltered = response.recordsTotal = productCount;

            var firstSortColumn = parameters.order.FirstOrDefault();
            var firstSortColumnOrder = firstSortColumn != null ? firstSortColumn.dir : DBLIB.Service.OrderDirectionEnum.asc;

            log.Info("소트. firstSortColumn=" + firstSortColumn + ", firstSortColumnOrder=" + firstSortColumnOrder);

            var entries = productService.GetPurchaseList(parameters.start, parameters.length, firstSortColumn != null ? firstSortColumn.column : 0, firstSortColumnOrder, parameters.search.value);
            response.data = entries.Select(a => PurchaseData.Create(a)).ToList();

            return Json(response);
        }
        
        [Authorize]
        public ActionResult Log()
        {
            return View();
        }

        [Authorize]
        public JsonResult PurchaseLog(Models.DataTables.DataTableRequestParameter parameters)
        {
            var response = new PurchaseLogDataTableResponse();
            response.recordsFiltered = response.recordsTotal = productService.GetAllPurchaseLogCount(); ;

            var firstSortColumn = parameters.order.FirstOrDefault();
            var firstSortColumnOrder = firstSortColumn != null ? firstSortColumn.dir : DBLIB.Service.OrderDirectionEnum.asc;

            log.Info("소트. firstSortColumn=" + firstSortColumn + ", firstSortColumnOrder=" + firstSortColumnOrder);

            var entries = productService.GetPurchaseLogList(parameters.start, parameters.length, firstSortColumn != null ? firstSortColumn.column : 0, firstSortColumnOrder, parameters.search.value);
            response.data = entries.Select(a => PurchaseLogData.Create(a)).ToList();

            return Json(response);
        }
    }
}