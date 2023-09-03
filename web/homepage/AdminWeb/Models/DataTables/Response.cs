using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminWeb.Models.DataTables
{
    // DataTable 응답의 기본 형태 (data 필드가 필수!)
    public class DataTableResponse
    {

        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public DataTableResponse() { }
    }

    public class MemberData
    {
        public string UserID;
        public string Name;
        public string Nickname;
        public string PhoneNo;
        public string CreatedOn;
        public string RecentLogin;
        public string Point;
        public string Status;
        public bool isQuit;

        public static MemberData Create(DBLIB.Player src)
        {
            var data = new MemberData();
            data.UserID = src.UserID;
            data.Name = src.UserName;
            data.Nickname = src.NickName;
            data.PhoneNo = src.PhoneNo;
            data.CreatedOn = src.CreatedOnDate.ToString("yyyy-MM-dd HH:mm");
            data.RecentLogin = src.LastLoginDateDate.HasValue ? src.LastLoginDateDate.Value.ToString("yyyy-MM-dd HH:mm") : "-";
            data.Point = src.Point.ToString("N0");
            data.isQuit = src.Quit;
            data.Status = data.isQuit ? "탈퇴" : "";
            return data;
        }
    }



    [Serializable]
    public class MemberDataTableResponse : DataTableResponse
    {
        public List<MemberData> data { get; set; }
    }

    public class NoticeData
    {
        public int id;
        public string title;
        public string createdOn;
        public string lastModifiedOn;
        public Boolean isActive, isPopup;
        public string content;

        public static NoticeData Create(DBLIB.WebNoticeList entry, bool includeContent = false)
        {
            var data = new NoticeData();
            data.createdOn = entry.createdOn.ToString("yyyy-MM-dd HH:mm");
            data.lastModifiedOn = entry.lastmodifiedOn.HasValue ? entry.lastmodifiedOn.Value.ToString("yyyy-MM-dd HH:mm") : "-";
            data.id = entry.id;
            data.isActive = entry.IsActive;
            data.isPopup = entry.isPopup;
            data.title = entry.title;
            if (includeContent)
                data.content = entry.content;
            return data;
        }
    }

    [Serializable]
    public class NoticeDataTableResponse : DataTableResponse
    {
        public List<NoticeData> data { get; set; }
    }

    public class QAData
    {
        public long id;
        public string title;
        public string userName;
        public string articleTypeName;
        public string createdOn;
        public string phoneNo;
        public string answeredOn;

        public string question;
        public string answer;

        public static QAData Create(DBLIB.qna entry, bool includeContent = false)
        {
            var data = new QAData();
            data.id = entry.id;
            data.title = entry.title;
            data.userName = (string.IsNullOrEmpty(entry.Player.UserName) ? entry.Player.NickName : entry.Player.UserName)  + "(" + entry.Player.UserID + ")";
            data.articleTypeName = entry.qna_articletype.label;
            data.createdOn = entry.CreatedOn.ToString("yyyy-MM-dd HH:mm");
            data.phoneNo = entry.PhoneNo1 + "-" + entry.PhoneNo2 + "-" + entry.PhoneNo3;
            data.answeredOn = entry.AnsweredOn.HasValue ? entry.AnsweredOn.Value.ToString("yyyy-MM-dd HH:mm") : "";

            if (includeContent)
            {
                data.question = entry.question;
                data.answer = entry.answer;
            }
            return data;
        }
    }

    [Serializable]
    public class QADataTableResponse : DataTableResponse
    {
        public List<QAData> data { get; set; }
    }

    public class MemoData
    {
        public long id;
        public int playerId;
        public int adminId;
        public string userName;
        public string adminName;
        public string message;
        public string createdOn;

        public static MemoData Create(DBLIB.PlayerAdminMemo entry)
        {
            var data = new MemoData();
            data.id = entry.id;
            data.userName = (string.IsNullOrEmpty(entry.Player.NickName) ? entry.Player.UserName : entry.Player.NickName) + "(" + entry.Player.UserID + ")";
            data.adminName = (string.IsNullOrEmpty(entry.AdminUser.Name) ? entry.AdminUser.Title : entry.AdminUser.Name) + "(" + entry.AdminUser.Title + ")";
            data.playerId = entry.PlayerId;
            data.adminId = entry.AdminId;
            data.message = entry.message;
            data.createdOn = entry.createdOn.ToString("yyyy-MM-dd HH:mm");

            return data;
        }
    }

    [Serializable]
    public class MemoDataTableResponse : DataTableResponse
    {
        public List<MemoData> data { get; set; }
    }

    // 상품

    public class ProductData
    {
        public int id;
        public string ptype;
        public string pname;
        public string img;
        public int value1;
        public string string1;

        public static ProductData Create(DBLIB.ProductList src)
        {
            var data = new ProductData();
            data.id = src.Id;
            data.ptype = src.ptype;
            data.pname = src.pname;
            data.img = src.img;
            data.value1 = src.value1.GetValueOrDefault();
            data.string1 = src.string1;
            return data;
        }
    }
    
    [Serializable]
    public class ProductDataTableResponse : DataTableResponse
    {
        public List<ProductData> data { get; set; }
    }

    public class PurchaseData
    {
        public int id;
        public string pid;
        public string pname;
        public long paidvalue1;
        public long paidvalue2;
        public long paidvalue3;
        public long paidvalue4;
        public string paidstring1;
        public string paidstring2;
        public string paidstring3;
        public string paidstring4;
        public string purchasekind;
        public string price;
        public string sale;
        public string saleweb;

        public int productid;
        public string productname;

        public static PurchaseData Create(DBLIB.PurchaseList src)
        {
            var data = new PurchaseData();
            data.id = src.id;
            data.pid = src.pid;
            data.pname = src.pname;
            data.paidvalue1 = src.paidvalue1.GetValueOrDefault();
            data.paidvalue2 = src.paidvalue2.GetValueOrDefault();
            data.paidvalue3 = src.paidvalue3.GetValueOrDefault();
            data.paidvalue4 = src.paidvalue4.GetValueOrDefault();
            data.paidstring1 = src.paidstring1;
            data.paidstring2 = src.paidstring2;
            data.paidstring3 = src.paidstring3;
            data.paidstring4 = src.paidstring4;
            data.purchasekind = src.purchase_kind;
            data.price = src.price.ToString("N0");
            data.sale = src.sale ? "Y" : "N";
            data.saleweb = src.saleweb ? "Y" : "N";
            // 모 상품에 대한 정보
            data.productid = src.productid;                         
            data.productname = src.ProductList.pname;
            return data;
        }


    }

    [Serializable]
    public class PurchaseDataTableResponse : DataTableResponse
    {
        public List<PurchaseData> data { get; set; }
    }

    public class PurchaseLogData
    {
        public long Id;
        public int UserId;
        public string ptype;
        public string pid;
        public string pname;
        public string purchase_kind;
        public int price;
        public string Date;
        public string Location;
        public string PlayerID;
        public string NickName;

        public static PurchaseLogData Create(DBLIB.V_LogPurchase src)
        {
            var data = new PurchaseLogData();
            data.Id = src.Id;
            data.UserId = src.UserId;
            data.ptype = src.ptype;
            data.pid = src.pid;
            data.pname = src.pname;
            data.purchase_kind = src.purchase_kind;
            data.price = src.price;
            data.Date = src.Date.ToString("yyyy-MM-dd HH:mm");
            data.Location = src.Location;
            data.PlayerID = src.PlayerID;
            data.NickName = src.NickName;
            return data;
        }


    }

    [Serializable]
    public class PurchaseLogDataTableResponse : DataTableResponse
    {
        public List<PurchaseLogData> data { get; set; }
    }

    public class CouponData
    {
        public int Id;
        public string Serial;
        public int Pay;
        public string CreateDate;
        public string UseDate;

        public static CouponData Create(DBLIB.Coupon src)
        {
            var data = new CouponData();
            data.Id = src.Id;
            data.Serial = src.Serial;
            data.Pay = src.Pay;
            data.CreateDate = src.CreateDate.ToString("yyyy-MM-dd HH:mm");
            data.UseDate = src.UseDate.HasValue ? src.UseDate.Value.ToString("yyyy-MM-dd HH:mm") : "";
            return data;
        }
    }

    [Serializable]
    public class CouponDataTableResponse : DataTableResponse
    {
        public List<CouponData> data { get; set; }
    }

    public class CouponLogData
    {
        public int Id;
        public string Serial;
        public int Pay;
        public string CreateDate;
        public string UseDate;
        public string PlayerID;
        public string NickName;

        public static CouponLogData Create(DBLIB.V_LogCoupon src)
        {
            var data = new CouponLogData();
            data.Id = src.Id;
            data.Serial = src.Serial;
            data.Pay = src.Pay;
            data.CreateDate = src.CreateDate.ToString("yyyy-MM-dd HH:mm");
            data.UseDate = src.UseDate.HasValue ? src.UseDate.Value.ToString("yyyy-MM-dd HH:mm") : "";
            data.PlayerID = src.PlayerID;
            data.NickName = src.NickName;
            return data;
        }
    }

    [Serializable]
    public class CouponLogDataTableResponse : DataTableResponse
    {
        public List<CouponLogData> data { get; set; }
    }
}