using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLIB.Service
{
    public class WebNoticeService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WebNoticeService));
        readonly GameEntities db;

        public WebNoticeService(GameEntities gameDb)
        {
            this.db = gameDb;
        }

        /// <summary>
        /// 메인 팝업 표시할 텍스트
        /// </summary>
        /// <returns></returns>
        public String getActiveMainPopupNotice()
        {
            var entry = db.WebNoticeList.Where(a => a.IsActive && a.isPopup).OrderBy(a => a.id).FirstOrDefault();
            if (entry != null)
                return entry.content;

            return "";
        }

        public WebNoticeList[] getNoticeList(int pageNo, int pageSize)
        {
            if (pageNo == 0)
                pageNo = 1;

            int pageIndex = (pageNo - 1) * pageSize;
            var arr = db.WebNoticeList.Where(a => a.IsActive)
                                    .OrderByDescending(a => a.createdBy)
                                    .Skip(pageIndex)
                                    .Take(pageSize)
                                    .ToArray();
            return arr;
        }
        public List<WebNoticeList> getNoticeList(int startIndex, int pageSize, bool ascending, string search)
        {
            IQueryable<WebNoticeList> arr = db.WebNoticeList;

            if (!string.IsNullOrEmpty(search))
                arr = arr.Where(a => a.title.Contains(search));

            if (ascending)
                arr = arr.OrderBy(a => a.id);
            else
                arr = arr.OrderByDescending(a => a.id);

            arr = arr.Skip(startIndex).Take(pageSize);
            return arr.ToList();
        }

        public int getTotalVisibleNoticeCount()
        {
            return db.WebNoticeList.Where(a => a.IsActive).Count();
        }
        public int getTotalNoticeCount()
        {
            return db.WebNoticeList.Count();
        }

        /// <summary>
        /// 특정 공지 사항을 검색
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public WebNoticeList getNotice(int no)
        {
            return db.WebNoticeList.Where(a => a.id == no).FirstOrDefault();
        }
                
        public bool AddWebNotice(string title, string content, bool visible, bool popup, int adminId)
        {
            try
            {
                WebNoticeList list = new WebNoticeList();
                list.IsActive = visible;
                list.isPopup = popup;
                list.title = title;
                list.content = content;
                list.createdOn = DateTime.Now;
                list.createdBy = adminId;

                db.WebNoticeList.Add(list);
                db.SaveChanges();

                log.Info("공지 저장 완료");
                return true;
            }catch(Exception e)
            {
                log.Error("공지 등록 중 오류", e);
            }
            return false;
        }

        public bool EditWebNotice(int articleNo, string title, string content, bool visible, bool popup, int adminId)
        {
            try
            {
                WebNoticeList list = db.WebNoticeList.Where(a => a.id == articleNo).FirstOrDefault();
                if (list == null)
                    throw new Exception("지정된 공지를 찾을 수 없습니다");

                list.IsActive = visible;
                list.isPopup = popup;
                list.title = title;
                list.content = content;
                list.lastmodifiedOn = DateTime.Now;
                list.lastModifiedBy = adminId;

                db.Entry(list).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                log.Info("공지 변경 완료");
                return true;
            }
            catch (Exception e)
            {
                log.Error("공지 변경 중 오류", e);
            }
            return false;
        }

        public bool DeleteWebNotice(int id)
        {
            try
            {
                WebNoticeList list = db.WebNoticeList.Where(a => a.id == id).FirstOrDefault();
                if (list == null)
                    throw new Exception("지정된 공지를 찾을 수 없습니다");

                db.Entry(list).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

                log.Info("공지[" + id + "] 삭제 완료");
                return true;
            }
            catch (Exception e)
            {
                log.Error("공지 삭제 중 오류", e);
            }
            return false;
        }
    }
}
