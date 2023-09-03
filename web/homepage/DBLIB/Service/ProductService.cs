using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLIB.Service
{
    public class ProductService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ProductService));

        readonly GameEntities vongGameDb;
        public ProductService(GameEntities vongGameEntities)
        {
            this.vongGameDb = vongGameEntities;
        }

        /*
        public enum ProductTypeEnum
        {
            Avatar,
            Event,
            Card
        }

        // 상품 목록를 검색
        public ProductList[] GetAvailableProductList(ProductTypeEnum productType)
        {
            var ptype = productType == ProductTypeEnum.Avatar ? "avatar" :
                        productType == ProductTypeEnum.Card ? "card" :
                        productType == ProductTypeEnum.Event ? "evt" : "";

            return vongGameDb.ProductList.Where(a => a.ptype == ptype).OrderBy(a => a.Id).ToArray();
        }
        */
        
        /// <summary>
        /// 구매 가능한 항목을 반환한다 (API 용)
        /// </summary>
        /// <param name="ptype"></param>
        public List<V_MobileShop> GetPurchasableItemList(string ptype)
        {
            return vongGameDb.V_MobileShop.Where(a => a.ptype == ptype).ToList();
        }

        /// <summary>
        /// 마이룸 목록 (API 용)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<V_PlayerItemList> GetMyRoomItemList(string userId)
        {
            int id = vongGameDb.Player.Where(a => a.UserID == userId).Select(a => a.Id).First();
            log.Info("마이룸 불러오기 요청 userId:" + userId + " id:" + id);
            return vongGameDb.V_PlayerItemList.Where(a => a.UserId == id).ToList();
        }

        // 구매할 수 있는 물품 목록
        public V_WEB_ShopItem[] GetAvailableShopList(string ptype)
        {
            return vongGameDb.V_WEB_ShopItem.Where(a => a.ptype == ptype).OrderByDescending(a => a.id).ToArray();
                    
        }

        // 구매 처리
        public bool BuyAvatar(string userId, string payType, int purchaseId, out string message, out string ViewLabelString)
        {
            message = "";
            var BuyResult = new ObjectParameter("BuyResult", typeof(bool));
            var ResultMessage = new ObjectParameter("ResultMessage", typeof(string));
            var ViewLabel = new ObjectParameter("ViewLabel", typeof(string));
            int Result = 0;

            try
            {
                Result = vongGameDb.SP_BuyAvatar(userId, payType, purchaseId, BuyResult, ResultMessage, ViewLabel);
            }
            catch (Exception e)
            {
                message = "구매 처리 중 오류가 발생하였습니다";
                ViewLabelString = "";
                return false;
            }

            ViewLabelString = (string)ViewLabel.Value;
            message = (string)ResultMessage.Value;
            log.Info("User[" + userId + "]의 [" + purchaseId + "] 구매 처리 결과 ==> " + BuyResult.Value + " :: " + ResultMessage.Value);
            return Convert.ToBoolean(BuyResult.Value);
        }

        /// <summary>
        /// 이용자가 보유하는 아이템 목록을 전부 반환
        /// 유효 기간이 지난 아이템은 제외
        /// </summary>
        /// <param name="userId">이용자 Login ID</param>
        /// <returns></returns>
        public List<DBLIB.V_PlayerItemList> GetHavingItems(string userId)
        {
            /*
            var list = vongGameDb.PlayerItemList
                    .Where(a => a.Player.UserID == userId && a.ExpireDate == null || a.ExpireDate > DateTime.Now)
                    .OrderByDescending(a => a.ExpireDate)
                    .ToList();
                    */
            int id = vongGameDb.Player.Where(a => a.UserID == userId).Select(a => a.Id).FirstOrDefault();
            var list = vongGameDb.V_PlayerItemList
                    .Where(a => a.UserId == id && (a.ExpireDate == null || a.ExpireDate > DateTime.Now))
                    .OrderByDescending(a => a.ExpireDate)
                    .ToList();

            log.Info("이용자[" + userId + "] 의 보유 아이템 " + list.Count() + " 개 찾음");
            return list;
        }

        /// <summary>
        /// 전체 상품 개수
        /// </summary>
        /// <returns></returns>
        public int GetAllProductCount()
        {
            return vongGameDb.ProductList.Count();
        }

        public int GetAllPurchaseItemCount()
        {
            return vongGameDb.PurchaseList.Count();
        }

        public int GetAllPurchaseLogCount()
        {
            return vongGameDb.V_LogPurchase.Count();
        }

        public List<ProductList> GetProductList(int start, int length, string search)
        {
            IQueryable<ProductList> list = vongGameDb.ProductList;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(a => a.pname.Contains(search) || a.img.Contains(search));

            //list = list.OrderBy(a => a.pname);
            list = list.OrderBy(a => a.Id);

            list = list.Skip(start).Take(length);
            return list.ToList();
        }
        public List<ProductList> GetAllProductList()
        {
            return vongGameDb.ProductList.ToList();
        }

        public List<PurchaseList> GetPurchaseList(int start, int length, int sortField, OrderDirectionEnum sortOrder, string search)
        {
            IQueryable<PurchaseList> list = vongGameDb.PurchaseList;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(a => a.pname.Contains(search) || a.ProductList.pname.Contains(search));

            if(sortField == 0)
                if (sortOrder == OrderDirectionEnum.asc)
                    list = list.OrderBy(a => a.id);
                else
                    list = list.OrderByDescending(a => a.id);
            else
                if (sortOrder == OrderDirectionEnum.asc)
                    list = list.OrderBy(a => a.pname);
                else
                    list = list.OrderByDescending(a => a.pname);

            list = list.Skip(start).Take(length);
            return list.ToList();
        }

        public List<V_LogPurchase> GetPurchaseLogList(int start, int length, int sortField, OrderDirectionEnum sortOrder, string search)
        {
            IQueryable<V_LogPurchase> list = vongGameDb.V_LogPurchase;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(a => a.pname.Contains(search) || a.PlayerID.Contains(search) || a.NickName.Contains(search));

            if (sortField == 0)
                if (sortOrder == OrderDirectionEnum.asc)
                    list = list.OrderBy(a => a.Id);
                else
                    list = list.OrderByDescending(a => a.Id);
            else
                if (sortOrder == OrderDirectionEnum.asc)
                list = list.OrderBy(a => a.pname);
            else
                list = list.OrderByDescending(a => a.pname);

            list = list.Skip(start).Take(length);
            return list.ToList();
        }

        public int RegisterProduct(string ptype, string pname, string img, int value1, string string1)
        {
            var entry = new ProductList();
            entry.img = img;
            entry.pname = pname;
            entry.ptype = ptype;
            entry.value1 = value1;
            entry.string1 = string1;
            entry.value2 = 0;
            entry.string2 = "";
            entry.value3 = 0;
            entry.string3 = "";
            entry.value4 = 0;
            entry.string4 = "";
            entry.value5 = 0;
            entry.string5 = "";

            vongGameDb.ProductList.Add(entry);
            vongGameDb.SaveChanges();

            int newId = entry.Id;
            log.Info("새로 추가된 [" + ptype + "][" + pname + "]의 ID 는 " + newId);
            return newId;
        }

        public void UpdateProduct(int id, string ptype, string pname, string img, int value1, string string1)
        {
            var entry = vongGameDb.ProductList.Where(a => a.Id == id).FirstOrDefault();
            entry.img = img;
            entry.pname = pname;
            entry.ptype = ptype;
            entry.value1 = value1;
            entry.string1 = string1;
            entry.value2 = 0;
            entry.string2 = "";
            entry.value3 = 0;
            entry.string3 = "";
            entry.value4 = 0;
            entry.string4 = "";
            entry.value5 = 0;
            entry.string5 = "";

            vongGameDb.Entry(entry).State = System.Data.Entity.EntityState.Modified;
            vongGameDb.SaveChanges();
        }

        public ProductList GetItem(int id)
        {
            return vongGameDb.ProductList.Where(a => a.Id == id).FirstOrDefault();
        }
        public PurchaseList GetPurchaseItem(int id)
        {
            return vongGameDb.PurchaseList.Where(a => a.id == id).FirstOrDefault();
        }

        public void UpdatePurchaseItem(int purchaseId, int productId, string pname, string pid,
                                            long paidvalue1, long paidvalue2, long paidvalue3, long paidvalue4,
                                            string paidstring1, string paidstring2, string paidstring3, string paidstring4,
                                            string ptype, int price, bool isSale, bool isSaleWeb)
        {
            PurchaseList entry = vongGameDb.PurchaseList.Where(a => a.id == purchaseId).FirstOrDefault();
            if (entry == null)
                throw new Exception("지정된 판매 상품을 찾을 수 없습니다");

            entry.productid = productId;
            entry.pid = pid;
            entry.pname = pname;
            entry.paidstring1 = paidstring1;
            entry.paidstring2 = paidstring2;
            entry.paidstring3 = paidstring3;
            entry.paidstring4 = paidstring4;
            if (!string.IsNullOrEmpty(paidstring1))
                entry.paidvalue1 = paidvalue1;
            if (!string.IsNullOrEmpty(paidstring2))
                entry.paidvalue2 = paidvalue2;
            if (!string.IsNullOrEmpty(paidstring3))
                entry.paidvalue3 = paidvalue3;
            if (!string.IsNullOrEmpty(paidstring4))
                entry.paidvalue4 = paidvalue4;
            entry.purchase_kind = ptype;
            entry.price = price;
            entry.sale = isSale;
            entry.saleweb = isSaleWeb;

            vongGameDb.Entry(entry).State = System.Data.Entity.EntityState.Modified;
            vongGameDb.SaveChanges();
        }

        public void RegisterPurchaseItem(int productId, string pname, string pid,
                                            long paidvalue1, long paidvalue2, long paidvalue3, long paidvalue4, 
                                            string paidstring1, string paidstring2, string paidstring3, string paidstring4, 
                                            string ptype, int price, bool isSale, bool isSaleWeb, int vieworder, string img)
        {
            PurchaseList entry = new PurchaseList();
            entry.productid = productId;
            entry.pid = pid;
            entry.pname = pname;
            entry.paidstring1 = paidstring1;
            entry.paidstring2 = paidstring2;
            entry.paidstring3 = paidstring3;
            entry.paidstring4 = paidstring4;
            if (!string.IsNullOrEmpty(paidstring1))
                entry.paidvalue1 = paidvalue1;
            if (!string.IsNullOrEmpty(paidstring2))
                entry.paidvalue2 = paidvalue2;
            if (!string.IsNullOrEmpty(paidstring3))
                entry.paidvalue3 = paidvalue3;
            if (!string.IsNullOrEmpty(paidstring4))
                entry.paidvalue4 = paidvalue4;
            entry.purchase_kind = ptype;
            entry.price = price;
            entry.sale = isSale;
            entry.saleweb = isSaleWeb;
            entry.vieworder = vieworder;
            entry.img = img;

            vongGameDb.PurchaseList.Add(entry);
            vongGameDb.SaveChanges();
        }
    }
}
