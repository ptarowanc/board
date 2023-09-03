using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLIB.Service
{
    public class AdminUserService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AdminUserService));

        readonly GameEntities vongGameDB;
        public AdminUserService(GameEntities db)
        {
            vongGameDB = db;
        }

        /// <summary>
        /// 지정된 ID, password 에 맞는 관리자 계정 정보를 찾는다
        /// </summary>
        /// <param name="userId">Admin ID</param>
        /// <param name="pwd">(옵션)</param>
        /// <returns>조건에 맞는 계정이 있는 경우 계정 entry</returns>
        public AdminUser Find(string userId, string pwd)
        {
            string passwordHash;

            passwordHash = CryptoHelper.makePassword(pwd);

            log.Debug("계정[" + userId + "], 비밀번호 [" + passwordHash + "]");

            return pwd != null ? vongGameDB.AdminUser.Where(a => a.AdminId == userId && a.Type <= 2 && a.UserPassword == passwordHash).FirstOrDefault() :
                                 vongGameDB.AdminUser.Where(a => a.AdminId == userId && a.Type <= 2).FirstOrDefault();
        }

    }
}
