using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLIB.Service
{
    public class QnaService
    {
        const int DefaultPageSize = 10;

        readonly GameEntities db;
        public QnaService(GameEntities db)
        {
            this.db = db;
        }

        /// <summary>
        /// 질문 유형 목록
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<String, String>> GetArticleTypes()
        {
            var types = db.qna_articletype
                        .OrderBy(a => a.sequence)
                        .ToList()
                        .Select(a => new KeyValuePair<String, String>(a.atticletype, a.label)).ToList();

            return types;
                        
        }

        /// <summary>
        /// 질문 등록
        /// </summary>
        /// <param name="form"></param>
        /// <param name="userId"></param>
        public void WriteArticle(Model.QnaForm form, string userId)
        {
            var playerId = db.Player.Where(a => a.UserID == userId).Select(a => a.Id).FirstOrDefault();

            var record = new qna();
            record.question = form.v_content;
            record.articleType = form.questionType;
            record.CreatedOn = DateTime.Now;
            record.PhoneNo1 = form.v_Hphone1;
            record.PhoneNo2 = form.v_Hphone2;
            record.PhoneNo3 = form.v_Hphone3;
            record.PlayerId = playerId;
            record.title = form.v_title;

            db.Entry(record).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
        }
        
        public List<qna> GetList(string userId, int pageNo)
        {
            if (pageNo < 1)
                pageNo = 1;

            var page = db.qna.Where(a => a.Player.UserID == userId)
                .OrderBy(a => a.id)
                .Skip((pageNo - 1) * DefaultPageSize)
                .Take(10);

            return page.ToList();
        }

        public List<qna> GetList(int startIndex, int pageSize, bool ascendingIdSort, string search)
        {
            IQueryable<qna> result = db.qna;
            if (!string.IsNullOrEmpty(search))
                result = result.Where(a => (a.title.Contains(search) || a.question.Contains(search)));      // 제목 또는 질문 본문에서 검색

            result = ascendingIdSort ? result.OrderBy(a => a.id) : result.OrderByDescending(a => a.id);
            result = result.Skip(startIndex).Take(pageSize);
            return result.ToList();
        }

        /// <summary>
        /// 해당 유저가 몇 개의 Q&A 페이지를 가지고 있나 (페이징용)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public int GetTotalPageCount(string userId, int pageSize)
        {
            return (int) Math.Ceiling((double) db.qna.Where(a => a.Player.UserID == userId).Count() / (double) pageSize);
        }

        /// <summary>
        /// 특정 이용자의 Q&A 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="qnaNo"></param>
        /// <returns></returns>
        public qna LoadArticle(string userId, int qnaNo)
        {
            var article = db.qna.Where(a => a.Player.UserID == userId && a.id == qnaNo).FirstOrDefault();
            return article;
        }
        /// <summary>
        /// 특정 Q&A 보기
        /// </summary>
        /// <param name="qnaNo"></param>
        /// <returns></returns>
        public qna LoadArticle(int qnaNo)
        {
            var article = db.qna.Where(a => a.id == qnaNo).FirstOrDefault();
            return article;
        }

        /// <summary>
        /// 시스템에 몇 개의 Q&A 가 있는지 개수
        /// </summary>
        /// <returns></returns>
        public int GetTotalQuestions()
        {
            return db.qna.Count();
        }

        /// <summary>
        /// 응답 처리가 안된 Q&A 개수
        /// </summary>
        /// <returns></returns>
        public int GetTotalUnreadQuestions()
        {
            return db.qna.Where(a => a.AnsweredOn == null).Count();
        }

        /// <summary>
        /// Q&A 에 응답 처리
        /// </summary>
        /// <param name="QnaId"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public bool UpdateAnsweer(int QnaId, string answer)
        {
            var data = db.qna.Where(a => a.id == QnaId).FirstOrDefault();
            if (data == null)
                return false;

            data.answer = answer;
            data.AnsweredOn = DateTime.Now;
            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();

            return true;
        }
    }
}
