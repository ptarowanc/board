using DBLIB.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.Controllers
{
    public class QAController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(QAController));
        readonly QnaService qnaService;

        public QAController(QnaService qnaService)
        {
            this.qnaService = qnaService;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public JsonResult QAList(Models.DataTables.DataTableRequestParameter parameters)
        {
            var response = new Models.DataTables.QADataTableResponse();
            response.recordsFiltered = response.recordsTotal = qnaService.GetTotalQuestions();

            var UserIdColumn = parameters.order.Where(a => a.column == 0).FirstOrDefault();
            var UserIdColumnOrder = UserIdColumn != null ? UserIdColumn.dir : DBLIB.Service.OrderDirectionEnum.asc;
            log.Info("소트 = " + UserIdColumn.dir);

            var entries = qnaService.GetList(parameters.start, parameters.length, UserIdColumnOrder == OrderDirectionEnum.asc, parameters.search.value);
            response.data = entries.Select(a => Models.DataTables.QAData.Create(a)).ToList();

            return Json(response);
        }
    }
}