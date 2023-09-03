using DBLIB.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserWeb.Models;

namespace UserWeb.Controllers
{
    public class DownloadController : Controller
    {
        readonly ProductService productService;

        public DownloadController(ProductService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// 맞고 모바일 다운로드
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MatgoMobile()
        {
            return View();
        }

        string DownloadPath = "~/Downloads/";

        public DownloadResult MatgoMobileLink()
        {
            string fileName = UserWeb.Properties.Settings.Default.DOWNLOAD_MATGOAPK_FILENAME;
            string filePath = Path.Combine(Server.MapPath(DownloadPath), fileName);
            
            return new DownloadResult
            {
                FileName = fileName,
                Path = DownloadPath + fileName
            };
        }

        /// <summary>
        /// 바둑이 모바일 다운로드
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BadugiMobile()
        {
            return View();
        }

        public DownloadResult BadugiMobileLink()
        {
            string fileName = UserWeb.Properties.Settings.Default.DOWNLOAD_BADUGIAPK_FILENAME;
            string filePath = Path.Combine(Server.MapPath(DownloadPath), fileName);

            return new DownloadResult
            {
                FileName = fileName,
                Path = DownloadPath + fileName
            };
        }

        public DownloadResult InstallerLink()
        {
            string fileName = UserWeb.Properties.Settings.Default.DOWNLOAD_INSTALL_FILENAME;
            string filePath = Path.Combine(Server.MapPath(DownloadPath), fileName);

            return new DownloadResult
            {
                FileName = fileName,
                Path = DownloadPath + fileName
            };
        }
        public DownloadResult InstallerLink2()
        {
            string fileName = UserWeb.Properties.Settings.Default.DOWNLOAD_INSTALL2_FILENAME;
            string filePath = Path.Combine(Server.MapPath(DownloadPath), fileName);

            return new DownloadResult
            {
                FileName = fileName,
                Path = DownloadPath + fileName
            };
        }
        public DownloadResult ImageLink()
        {
            string fileName = UserWeb.Properties.Settings.Default.DOWNLOAD_IMAGE_FILENAME;
            string filePath = Path.Combine(Server.MapPath(DownloadPath), fileName);

            return new DownloadResult
            {
                FileName = fileName,
                Path = DownloadPath + fileName
            };
        }
    }
}