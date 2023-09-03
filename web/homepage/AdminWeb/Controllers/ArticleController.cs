using AdminWeb.Models;
using DBLIB.Model;
using DBLIB.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.Controllers
{
    public class ArticleController : Controller
    {
        readonly WebNoticeService webNoticeService;
        readonly QnaService qnaService;
        readonly ProductService productService;
        readonly MemoService memoService;
        readonly UserService userService;
        
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ArticleController));

        public ArticleController(WebNoticeService webNoticeService, QnaService qnaService, ProductService productService, MemoService memoService, UserService userService)
        {
            this.webNoticeService = webNoticeService;
            this.qnaService = qnaService;
            this.productService = productService;
            this.memoService = memoService;
            this.userService = userService;
        }

        // GET: Article
        [Authorize]
        public ActionResult Write()
        {
            ViewBag.no = 0;
            return View();
        }

        [Authorize]
        public ActionResult EditNotice(int no)
        {
            ViewBag.no = no;
            return View("Write");
        }

        public class WriteNoticeForm
        {
            public int editingArticleNo { get; set; }
            public string title { get; set; }
            [AllowHtml] public string content { get; set; }
            public string SetVisible { get; set; }
            public string SetPopup { get; set; }
        }

        [Authorize]
        public JsonResult WriteNotice(WriteNoticeForm form )
        {
            StandardResult result = null;
            try
            {
                bool adding = form.editingArticleNo == 0;

                if (string.IsNullOrWhiteSpace(form.title))
                    throw new Exception("제목을 입력해 주세요");
                if(string.IsNullOrWhiteSpace(form.content))
                    throw new Exception("본문1을 입력해 주세요");
                bool setVisible = form.SetVisible != null && form.SetVisible == "Y";
                bool setPopup = form.SetPopup != null && form.SetPopup == "Y";

                if(adding)
                    webNoticeService.AddWebNotice(form.title, form.content, setVisible, setPopup, User.Identity.GetAdminId());
                else
                    webNoticeService.EditWebNotice(form.editingArticleNo, form.title, form.content, setVisible, setPopup, User.Identity.GetAdminId());
                result = StandardResult.createSucceeded();
            }
            catch (Exception e)
            {
                result = StandardResult.createError(e.Message);
            }

            return Json(result);
        }

        [Authorize]
        // 공지 사항을 내려주는 기능
        public JsonResult LoadNotice(int id)
        {
            StandardResult result = null;
            try
            {
                var notice = webNoticeService.getNotice(id);
                if (notice == null)
                    throw new Exception("지정된 공지를 찾을 수 없습니다");

                var data = Models.DataTables.NoticeData.Create(notice, true);

                result = StandardResult.createSucceeded();
                result.data = data;
            }
            catch(Exception e)
            {
                result = StandardResult.createError(e.Message);
                log.Error("공지 검색 오류", e);
            }
            return Json(result);
        }

        // Q&A 
        [Authorize]
        public ActionResult answer(int id)
        {
            return View(id);
        }


        public JsonResult LoadQna(int id)
        {
            StandardResult result = null;
            try
            {
                var notice = qnaService.LoadArticle(id);
                if (notice == null)
                    throw new Exception("지정된 q&a 항목을 찾을 수 없습니다");

                var data = Models.DataTables.QAData.Create(notice, true);

                result = StandardResult.createSucceeded();
                result.data = data;
            }
            catch (Exception e)
            {
                result = StandardResult.createError(e.Message);
                log.Error("Q&A 검색 오류", e);
            }
            return Json(result);
        }


        public class UpdateAnswerForm
        {
            public int editingArticleNo { get; set; }
            [AllowHtml] public string answer { get; set; }
        }
        [Authorize]
        public JsonResult UpdateAnswer(UpdateAnswerForm form)
        {
            StandardResult result = null;
            try
            {
                bool adding = form.editingArticleNo == 0;

                if (string.IsNullOrWhiteSpace(form.answer))
                    throw new Exception("응답 내용을 입력해 주세요");

                bool updateOk = qnaService.UpdateAnsweer(form.editingArticleNo, form.answer);
                result = updateOk ? StandardResult.createSucceeded() : StandardResult.createError("변경할 수 없습니다");
            }
            catch (Exception e)
            {
                log.Error("응답 처리 중 오류가 발생하였습니다", e);
                result = StandardResult.createError(e.Message);
            }

            return Json(result);
        }

        [Authorize]
        public ActionResult NewProduct()
        {
            return View();
        }

        [Authorize]
        public ActionResult EditProduct(int id)
        {
            var data = productService.GetItem(id);
            return View(data);
        }

        [Authorize]
        [HttpPost]
        public JsonResult RegNewProduct(ProductForm form)
        {
            StandardResult result = null;
            try
            {
                if (string.IsNullOrEmpty(form.ptype))
                    throw new Exception("상품 유형을 선택해 주세요");

                if ( (form.ptype != "charge" && form.ptype != "evt") && form.img == null)
                    throw new Exception("상품 이미지를 등록해 주세요");
                if (string.IsNullOrEmpty(form.pname))
                    throw new Exception("상품명을 입력해 주세요");
                if (string.IsNullOrEmpty(form.string1))
                    throw new Exception("상품식별자를 입력해 주세요");

                // 파일 시스템에 저장될 이름
                string filename = "";
                if (form.img != null)
                {
                    // 원래의 파일 이름에 timestamp 를 붙여서 파일이 중복되지 않도록
                    filename = System.IO.Path.GetFileNameWithoutExtension(form.img.FileName);// + DateTime.Now.ToString("yyyyMMddHHmmss");

                    string savePath = System.IO.Path.Combine(Properties.Settings.Default.PRODUCT_IMG_PATH, form.ptype, filename + ".png");
                    log.Info("이미지를 저장할 경로 = " + savePath);

                    form.img.SaveAs(savePath);
                }

                productService.RegisterProduct(form.ptype, form.pname, filename, form.value1, form.string1);

                result = StandardResult.createSucceeded();
                log.Info("상품 등록 완료");

            }catch(Exception e)
            {
                log.Error("상품 등록 중 오류", e);
                result = StandardResult.createError(e.Message);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult UpdateProduct(ProductForm form)
        {
            StandardResult result = null;
            try
            {
                if (form.id.HasValue == false)
                    throw new Exception("원 항목을 찾을 수 없습니다");

                var currentProduct = productService.GetItem(form.id.Value);
                if(currentProduct == null)
                    throw new Exception("원 항목을 찾을 수 없습니다");

                if (string.IsNullOrEmpty(form.ptype))
                    throw new Exception("상품 유형을 선택해 주세요");

                if (form.ptype != currentProduct.ptype &&  (form.ptype != "charge" && form.ptype != "evt") && form.img == null)
                    throw new Exception("상품 이미지를 등록해 주세요");

                if (string.IsNullOrEmpty(form.pname))
                    throw new Exception("상품명을 입력해 주세요");

                if (string.IsNullOrEmpty(form.string1))
                    throw new Exception("상품식별자를 입력해 주세요");

                // 파일 시스템에 저장될 이름
                string filename = currentProduct.img;
                if (form.img != null)
                {
                    // 원래의 파일 이름에 timestamp 를 붙여서 파일이 중복되지 않도록
                    filename = System.IO.Path.GetFileNameWithoutExtension(form.img.FileName);// + DateTime.Now.ToString("yyyyMMddHHmmss");
//                    filename = System.IO.Path.GetFileNameWithoutExtension(form.img.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss");

                    string savePath = System.IO.Path.Combine(Properties.Settings.Default.PRODUCT_IMG_PATH, form.ptype, filename + ".png");
                    log.Info("이미지를 저장할 경로 = " + savePath);

                    form.img.SaveAs(savePath);
                }

                productService.UpdateProduct(form.id.Value, form.ptype, form.pname, filename, form.value1, form.string1);

                result = StandardResult.createSucceeded();
                log.Info("상품 등록 완료");

            }
            catch (Exception e)
            {
                log.Error("상품 등록 중 오류", e);
                result = StandardResult.createError(e.Message);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult ProductList()
        {
            StandardResult result = null;
            try
            {
                result = StandardResult.createSucceeded();
                result.data = productService.GetAllProductList().Select(a => new Tuple<int, string, string>(a.Id, a.pname, a.string1));
            }
            catch (Exception e)
            {
                result = StandardResult.createError("ERROR", e.Message);
                log.Error("상품 목록 검색 오류", e);
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult NewPurchase()
        {
            return View();
        }

        public class RegNewPurchaseForm
        {
            public int? id { get; set; }
            public string productid { get; set; }
            public string pid { get; set; }
            public string pname { get; set; }
            public string paidstring1 { get; set; }
            public string paidvalue1 { get; set; }
            public string paidstring2 { get; set; }
            public string paidvalue2 { get; set; }
            public string paidstring3 { get; set; }
            public string paidvalue3 { get; set; }
            public string paidstring4 { get; set; }
            public string paidvalue4 { get; set; }
            public string ptype { get; set; }
            public string price { get; set; }
            public string sale { get; set; }
            public string saleweb { get; set; }
            public string vieworder { get; set; }
            public string img { get; set; }
        }

        // 판매 상품 등록 
        [HttpPost]
        [Authorize]
        public JsonResult RegNewPurchase(RegNewPurchaseForm form)
        {
            StandardResult result = null;
            try
            {
                if (string.IsNullOrEmpty(form.productid))
                    throw new Exception("상품을 선택해 주세요");

                if (string.IsNullOrEmpty(form.pid))
                    throw new Exception("상품 식별자를 입력해 주세요");

                int productId = 0;
                int.TryParse(form.productid, out productId);
                if (productService.GetItem(productId) == null)
                    throw new Exception("사용할 수 없는 상품 입니다");

                if (string.IsNullOrWhiteSpace(form.pname))
                    throw new Exception("판매 상품명을 입력해 주세요");

                if (string.IsNullOrEmpty(form.ptype))
                    throw new Exception("구매 유형을 선택해 주세요");

                long paidvalue1=0, paidvalue2=0, paidvalue3=0, paidvalue4=0;
                //long.TryParse(form.paidvalue1, out paidvalue1);
                //long.TryParse(form.paidvalue2, out paidvalue2);
                long.TryParse(form.paidvalue3, out paidvalue3);
                //long.TryParse(form.paidvalue4, out paidvalue4);

                int price;
                int.TryParse(form.price, out price);

                int vieworder;
                int.TryParse(form.vieworder, out vieworder);

                productService.RegisterPurchaseItem(productId, form.pname, form.pid, paidvalue1, paidvalue2, paidvalue3, paidvalue4, form.paidstring1, form.paidstring2, form.paidstring3, form.paidstring4, form.ptype, price, "Y" == form.sale, "Y" == form.saleweb, vieworder, form.img);

                result = StandardResult.createSucceeded();
            }
            catch (Exception e)
            {
                result = StandardResult.createError(e.Message);
                log.Error("판매 상품 등록 중 오류", e);
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult EditPurchase(int id)
        {
            var data = productService.GetPurchaseItem(id);
            if (data == null)
                return null;

            return View(data);
        }

        [HttpPost]
        [Authorize]
        public JsonResult UpdatePurchase(RegNewPurchaseForm form)
        {
            StandardResult result = null;
            try
            {
                if (!form.id.HasValue)
                    throw new Exception("판매 상품 번호가 없습니다");

                if (string.IsNullOrEmpty(form.productid))
                    throw new Exception("상품을 선택해 주세요");

                if (string.IsNullOrEmpty(form.pid))
                    throw new Exception("상품 식별자를 입력해 주세요");

                int productId = 0;
                int.TryParse(form.productid, out productId);
                if (productService.GetItem(productId) == null)
                    throw new Exception("사용할 수 없는 상품 입니다");

                if (string.IsNullOrWhiteSpace(form.pname))
                    throw new Exception("판매 상품명을 입력해 주세요");

                if (string.IsNullOrEmpty(form.ptype))
                    throw new Exception("구매 유형을 선택해 주세요");

                long paidvalue1, paidvalue2, paidvalue3, paidvalue4;
                long.TryParse(form.paidvalue1, out paidvalue1);
                long.TryParse(form.paidvalue2, out paidvalue2);
                long.TryParse(form.paidvalue3, out paidvalue3);
                long.TryParse(form.paidvalue4, out paidvalue4);

                int price;
                int.TryParse(form.price, out price);

                productService.UpdatePurchaseItem(form.id.Value, productId, form.pname, form.pid, paidvalue1, paidvalue2, paidvalue3, paidvalue4, form.paidstring1, form.paidstring2, form.paidstring3, form.paidstring4, form.ptype, price, "Y" == form.sale, "Y" == form.saleweb);

                result = StandardResult.createSucceeded();
            }
            catch (Exception e)
            {
                result = StandardResult.createError(e.Message);
                log.Error("판매 상품 등록 중 오류", e);
            }
            return Json(result);
        }


        [Authorize]
        public ActionResult NewMemo()
        {
            return View();
        }

        [Authorize]
        public ActionResult EditMemo(int id)
        {
            var data = memoService.GetItem(id);
            return View(data);
        }

        [Authorize]
        [HttpPost]
        public JsonResult RegNewMemo(MemoForm form)
        {
            StandardResult result = null;
            try
            {
                if (string.IsNullOrEmpty(form.userId))
                    throw new Exception("회원 계정을 입력해 주세요");

                if (string.IsNullOrEmpty(form.message))
                    throw new Exception("내용을 입력해 주세요");

                int userid = userService.GetIdByUserId(form.userId);

                if(userid == 0)
                    throw new Exception("없는 회원 계정입니다.");

                int adminid = User.Identity.GetAdminId();

                memoService.RegisteMemo(userid, adminid, form.message);

                result = StandardResult.createSucceeded();
                log.Info("쪽지 저장 완료");

            }
            catch (Exception e)
            {
                log.Error("쪽지 저장 중 오류", e);
                result = StandardResult.createError(e.Message);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult UpdateMemo(MemoForm form)
        {
            StandardResult result = null;
            try
            {
                var currentMemo = memoService.GetItem(form.id.Value);
                if (currentMemo == null)
                    throw new Exception("원 항목을 찾을 수 없습니다");

                if (string.IsNullOrEmpty(form.message))
                    throw new Exception("내용을 입력해 주세요");
                
                int adminid = User.Identity.GetAdminId();
                
                memoService.UpdateMemo(form.id.Value, adminid, form.message);

                result = StandardResult.createSucceeded();
                log.Info("쪽지 수정 완료");

            }
            catch (Exception e)
            {
                log.Error("쪽지 저장 중 오류", e);
                result = StandardResult.createError(e.Message);
            }
            return Json(result);
        }
    }
}