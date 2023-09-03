using AdminWeb.Models.DataTables;
using DBLIB.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.Controllers
{
    /// <summary>
    /// 상품 관리
    /// </summary>
    public class ProductController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(NoticeController));
        readonly ProductService productService;

        public ProductController(ProductService productService)
        {
            this.productService = productService;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public JsonResult ProductList(Models.DataTables.DataTableRequestParameter parameters)
        {
            var productCount = productService.GetAllProductCount();

            var response = new Models.DataTables.ProductDataTableResponse();
            response.recordsFiltered = response.recordsTotal = productCount;

            var UserIdColumn = parameters.order.Where(a => a.column == 0).FirstOrDefault();
            var UserIdColumnOrder = UserIdColumn != null ? UserIdColumn.dir : DBLIB.Service.OrderDirectionEnum.asc;
            log.Info("소트 = " + UserIdColumn.dir);

            var entries = productService.GetProductList(parameters.start, parameters.length, parameters.search.value);
            response.data = entries.Select(a => ProductData.Create(a)).ToList();

            return Json(response);
        }

        [Authorize]
        public JsonResult ProductLog(Models.DataTables.DataTableRequestParameter parameters)
        {
            var productCount = productService.GetAllProductCount();

            var response = new Models.DataTables.ProductDataTableResponse();
            response.recordsFiltered = response.recordsTotal = productCount;

            var UserIdColumn = parameters.order.Where(a => a.column == 0).FirstOrDefault();
            var UserIdColumnOrder = UserIdColumn != null ? UserIdColumn.dir : DBLIB.Service.OrderDirectionEnum.asc;
            log.Info("소트 = " + UserIdColumn.dir);

            var entries = productService.GetProductList(parameters.start, parameters.length, parameters.search.value);
            response.data = entries.Select(a => ProductData.Create(a)).ToList();

            return Json(response);
        }
    }
}