using AdminWeb.Models;
using AdminWeb.Models.DataTables;
using DBLIB.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.Controllers
{
    public class MainController : Controller
    {
        readonly UserService userService;
        readonly QnaService qnaService;
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MainController));

        public MainController(UserService userService, QnaService qnaService)
        {
            this.userService = userService;
            this.qnaService = qnaService;
        }

        [Authorize]
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// Q&A 카운터
        /// </summary>
        /// <returns></returns>
        public JsonResult QNACounter()
        {
            var res = new CounterResponse();
            try
            {
                res.result = "OK";
                res.wait = qnaService.GetTotalUnreadQuestions();
                res.total = qnaService.GetTotalQuestions();
            }
            catch (Exception e)
            {
                res.result = "ERROR";
                res.reason = e.Message;
            }
            return Json(res);
        }

        /// <summary>
        /// 상단 회원수 표시
        /// </summary>
        /// <returns></returns>
        public JsonResult MemberCounter()
        {
            var res = new CounterResponse();
            try
            {
                res.result = "OK";
                res.total = userService.GetTotalAccumulatedMemberCount();
            }
            catch(Exception e)
            {
                res.result = "ERROR";
                res.reason = e.Message;
            }
            return Json(res);
        }
        
        /// <summary>
        /// DataTable 용 데이터 요청 처리
        /// https://www.codeproject.com/Tips/1011531/Using-jQuery-DataTables-with-Server-Side-Processin
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult MemberList(DataTableRequestParameter parameters)
        {
            var response = new MemberDataTableResponse();
            response.recordsTotal = userService.GetTotalAccumulatedMemberCount();
           
            var UserIdColumn = parameters.order.Where(a => a.column == 0).FirstOrDefault();
            var UserIdColumnOrder = UserIdColumn != null ? UserIdColumn.dir : OrderDirectionEnum.asc;
            log.Info("소트 = " + UserIdColumn.dir);

            var entries = userService.GetMemberPaging(parameters.start, parameters.length, UserIdColumnOrder, parameters.search.value);
            response.recordsFiltered = response.recordsTotal; // entries.Count();
            response.data = entries.Select(a => MemberData.Create(a)).ToList();
            
            return Json(response);
        }
    }
}