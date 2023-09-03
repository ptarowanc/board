using DBLIB.Model;
using DBLIB.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.Controllers
{
    /// <summary>
    /// 공지 관련 컨트롤러
    /// </summary>
    public class NoticeController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(NoticeController));
        readonly WebNoticeService webNoticeService;

        public NoticeController(WebNoticeService webNoticeService)
        {
            this.webNoticeService = webNoticeService;
        }
        
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public JsonResult NoticeList(Models.DataTables.DataTableRequestParameter parameters)
        {
            var response = new Models.DataTables.NoticeDataTableResponse();
            response.recordsTotal = webNoticeService.getTotalNoticeCount();
            response.recordsFiltered = response.recordsTotal; // entries.Count();

            var UserIdColumn = parameters.order.Where(a => a.column == 0).FirstOrDefault();
            var UserIdColumnOrder = UserIdColumn != null ? UserIdColumn.dir : DBLIB.Service.OrderDirectionEnum.asc;
            log.Info("소트 = " + UserIdColumn.dir);

            var entries = webNoticeService.getNoticeList(parameters.start, parameters.length, UserIdColumnOrder == OrderDirectionEnum.asc, parameters.search.value);
            response.data = entries.Select(a => Models.DataTables.NoticeData.Create(a)).ToList();

            return Json(response);
        }

        /// <summary>
        /// 공지 사항 삭제 API
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public JsonResult DeleteNotice(int id)
        {
            StandardResult result = null;
            try
            {
                bool ok = webNoticeService.DeleteWebNotice(id);
                result = ok ? StandardResult.createSucceeded() : StandardResult.createError("삭제 할 수 없습니다");
            }catch(Exception e)
            {
                result = StandardResult.createError(e.Message);
                log.Error("공지 삭제 중 오류", e);
            }
            return Json(result);
        }
    }
}