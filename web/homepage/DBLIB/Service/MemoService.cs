using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLIB.Service
{
    public class MemoService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ProductService));

        const int DefaultPageSize = 10;

        readonly GameEntities db;
        public MemoService(GameEntities db)
        {
            this.db = db;
        }

        /// <summary>
        /// 관리자 쪽지 등록
        /// </summary>
        /// <param name="form"></param>
        /// <param name="userId"></param>
        public void WriteMemo(Model.QnaForm form, string userId, int adminId)
        {
            var playerId = db.Player.Where(a => a.UserID == userId).Select(a => a.Id).FirstOrDefault();

            var record = new PlayerAdminMemo();
            record.message = form.v_content;
            record.createdOn = DateTime.Now;
            record.PlayerId = playerId;
            record.AdminId = adminId;

            db.Entry(record).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
        }

        public PlayerAdminMemo GetItem(int id)
        {
            return db.PlayerAdminMemo.Where(a => a.id == id).FirstOrDefault();
        }

        public List<PlayerAdminMemo> GetList(string userId, int pageNo)
        {
            if (pageNo < 1)
                pageNo = 1;

            var page = db.PlayerAdminMemo.Where(a => a.Player.UserID == userId)
                .OrderBy(a => a.id)
                .Skip((pageNo - 1) * DefaultPageSize)
                .Take(10);

            return page.ToList();
        }

        public List<PlayerAdminMemo> GetList(int startIndex, int pageSize, bool ascendingIdSort, string search)
        {
            IQueryable<PlayerAdminMemo> result = db.PlayerAdminMemo;
            if (!string.IsNullOrEmpty(search))
                result = result.Where(a => (a.message.Contains(search)));      // 본문에서 검색

            result = ascendingIdSort ? result.OrderBy(a => a.id) : result.OrderByDescending(a => a.id);
            result = result.Skip(startIndex).Take(pageSize);
            return result.ToList();
        }

        /// <summary>
        /// 해당 유저가 몇 개의 메모 페이지를 가지고 있나 (페이징용)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public int GetTotalPageCount(string userId, int pageSize)
        {
            return (int) Math.Ceiling((double) db.PlayerAdminMemo.Where(a => a.Player.UserID == userId).Count() / (double) pageSize);
        }

        /// <summary>
        /// 특정 이용자의 메모 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memoNo"></param>
        /// <returns></returns>
        public PlayerAdminMemo LoadArticle(string userId, int memoNo)
        {
            var article = db.PlayerAdminMemo.Where(a => a.Player.UserID == userId && a.id == memoNo).FirstOrDefault();
            return article;
        }
        /// <summary>
        /// 특정 메모 보기
        /// </summary>
        /// <param name="memoNo"></param>
        /// <returns></returns>
        public PlayerAdminMemo LoadArticle(int memoNo)
        {
            var article = db.PlayerAdminMemo.Where(a => a.id == memoNo).FirstOrDefault();
            return article;
        }

        /// <summary>
        /// 시스템에 몇 개의 메모가 있는지 개수
        /// </summary>
        /// <returns></returns>
        public int GetTotalMemo()
        {
            return db.PlayerAdminMemo.Count();
        }

        public int RegisteMemo(int userId, int adminId, string message)
        {
            var entry = new PlayerAdminMemo();

            entry.PlayerId = userId;
            entry.AdminId = adminId;
            entry.message = message;
            entry.createdOn = DateTime.Now;

            db.PlayerAdminMemo.Add(entry);
            db.SaveChanges();
            int newId = entry.id;
            log.Info(System.Reflection.MethodBase.GetCurrentMethod().Name +" [" + userId + "][" + adminId + "][" + message + "]> ID: " + newId);

            return newId;
        }

        public bool UpdateMemo(int memoId, int adminId, string message)
        {
            var data = db.PlayerAdminMemo.Where(a => a.id == memoId).FirstOrDefault();
            if (data == null)
                return false;

            data.message = message;
            data.AdminId = adminId;
            data.createdOn = DateTime.Now;

            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
            
            log.Info(System.Reflection.MethodBase.GetCurrentMethod().Name +" [" + adminId + "][" + message + "]> ID: " + memoId);

            return true;
        }

        public bool DeleteMemo(int id)
        {
            try
            {
                PlayerAdminMemo list = db.PlayerAdminMemo.Where(a => a.id == id).FirstOrDefault();
                if (list == null)
                    throw new Exception("쪽지를 찾을 수 없습니다");

                db.Entry(list).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

                log.Info("쪽지[" + id + "] 삭제 완료");
                return true;
            }
            catch (Exception e)
            {
                log.Error("쪽지 삭제 중 오류", e);
            }
            return false;
        }
    }
}
