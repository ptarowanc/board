using DBLIB.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserWeb.Models;

namespace UserWeb.Controllers
{
    public class CustomerController : Controller
    {
        readonly WebNoticeService webNoticeService;
        const int defaultPageSize = 30;
        const int defaultPageSet = 10;

        public CustomerController(WebNoticeService webNoticeService)
        {
            this.webNoticeService = webNoticeService;
        }

        public ActionResult NoticeView(int n_no, String PageNo, String StartPage, String PasgeSize)
        {
            var data = webNoticeService.getNotice(n_no);
            ViewBag.rs_d_insert = data.lastmodifiedOn.HasValue ? data.lastmodifiedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") : data.createdOn.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.rs_v_title = data.title;
            ViewBag.rs_v_contents = data.content;
            ViewBag.n_no = n_no;
            ViewBag.PageNo = PageNo;
            ViewBag.StartPage = StartPage;
            ViewBag.PasgeSize = PasgeSize;

            return View();
        }

        public ActionResult NoticeList(int? PageNo, int? StartPage)
        {
            NoticeViewListData data = new NoticeViewListData();
            data.webNoticeList = webNoticeService.getNoticeList(PageNo.GetValueOrDefault(1), Properties.Settings.Default.PAGING_SIZE);
            data.pageNo = PageNo.GetValueOrDefault(1);
            //data.pageSet = PageSet;
            //data.pageSize = PageSize;
            data.startPage = StartPage.GetValueOrDefault(1);
            data.totalPages = (int) Math.Ceiling( (double)webNoticeService.getTotalVisibleNoticeCount() / (double) Properties.Settings.Default.PAGING_SIZE) ;

            return View(data);
        }

    }
}