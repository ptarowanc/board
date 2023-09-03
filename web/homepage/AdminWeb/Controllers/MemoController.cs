using DBLIB;
using DBLIB.Model;
using DBLIB.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.Controllers
{
    public class MemoController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MemoController));
        readonly MemoService memoService;

        public MemoController(MemoService memoService)
        {
            this.memoService = memoService;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public class AdminMemoData
        {
            public List<PlayerAdminMemo> items;
            public int totalPages;
            public int currentPageNo;
            public int startPage;
        }

        [Authorize]
        public JsonResult MemoList(Models.DataTables.DataTableRequestParameter parameters)
        {
            var response = new Models.DataTables.MemoDataTableResponse();
            response.recordsFiltered = response.recordsTotal = memoService.GetTotalMemo();

            var UserIdColumn = parameters.order.Where(a => a.column == 0).FirstOrDefault();
            var UserIdColumnOrder = UserIdColumn != null ? UserIdColumn.dir : DBLIB.Service.OrderDirectionEnum.asc;
            log.Info("소트 = " + UserIdColumn.dir);

            var entries = memoService.GetList(parameters.start, parameters.length, UserIdColumnOrder == OrderDirectionEnum.asc, parameters.search.value);
            response.data = entries.Select(a => Models.DataTables.MemoData.Create(a)).ToList();

            return Json(response);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteMemo(int id)
        {
            StandardResult result = null;
            try
            {
                bool ok = memoService.DeleteMemo(id);
                result = ok ? StandardResult.createSucceeded() : StandardResult.createError("삭제 할 수 없습니다");
            }
            catch (Exception e)
            {
                result = StandardResult.createError(e.Message);
                log.Error("쪽지 삭제 중 오류", e);
            }
            return Json(result);
        }
    }
}