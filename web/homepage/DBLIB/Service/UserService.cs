using DBLIB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DBLIB.Service
{
    public class UserService
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(UserService));
        readonly GameEntities vongGameDB;

        public UserService(GameEntities db)
        {
            this.vongGameDB = db;
        }

        /// <summary>
        /// 사용 가능한 계정인지
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsAvailableId(string id)
        {
            bool cant = vongGameDB.Player.Where(a => a.UserID == id).Count() == 0;
            if (cant == false) return cant;
            cant = vongGameDB.AdminUser.Where(a => a.AdminId == id).Count() == 0;
            if (cant == false) return cant;
            return true;
        }
        public bool IsAvailableNick(string nickname)
        {
            return vongGameDB.Player.Where(a => a.NickName == nickname).Count() == 0;
        }
        public bool IsAvailableCode(string code)
        {
            var refUser = vongGameDB.AdminUser.Where(a => a.Name.ToUpper() == code.ToUpper() && a.UpperMemberId != 0).FirstOrDefault();
            if (refUser == null)
            {
                // 추천인 없음
                return false;
            }
            else if (refUser.Type < 4)
            {
                // 매장, 총판, 지사만 추천 가능
                return false;
            }
            return true;
        }
        public int GetIdByUserId(string userId)
        {
            var user = vongGameDB.Player.Where(a => a.UserID == userId).FirstOrDefault();
            if (user == null)
                return 0;
            else
                return user.Id;
        }

        public Regex numericOnly = new Regex("[0-9]+");
        public Regex alphaOnly = new Regex("[A-Za-z]+");
        public Regex alphaNumericOnly = new Regex("[A-Za-z0-9]+");
        //public Regex koreanOnly = new Regex("[가-힣]+");
        public Regex koreansOnly = new Regex("[가-힣ㄱ-ㅎ]+");
        public Regex koreanAlphaOnly = new Regex("[A-Za-z가-힣ㄱ-ㅎ]+");
        public Regex koreanAlphaNumericOnly = new Regex("[A-Za-z0-9가-힣ㄱ-ㅎ]+");
        public Regex passwordCHK = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$"); // 6자리 이상
        public Regex koreanOnly = new Regex("^[가-힣]+$");
        public Regex koreanNumericOnly = new Regex("^[가-힣0-9]+$");
        public Regex alphaNumericOnly2 = new Regex("^[A-Za-z0-9]+$");
        /// <summary>
        /// 특정 인증 식별자에 해당하는 유효한 회원이 있는지 확인해서 있으면 ID 반환
        /// </summary>
        /// <param name="ci"></param>
        /// <param name="di"></param>
        /// <returns></returns>
        public Player GetExistingMemberId(string ci, string di)
        {
            // 테스트인증 패스
            if (ci == "simulator__ci_value") return null;

            // 탈퇴 안하고 같은 CI 값을 가지는 사람을 찾자~
            Player player = vongGameDB.Player.Where(a => a.Quit == false && a.ci == ci).FirstOrDefault();
            return player;
        }

        public enum CreatedFrom
        {
            MOBILE,
            WEB
        }

        /// <summary>
        /// 회원 가입
        /// </summary>
        /// <param name="id">로그인 ID</param>
        /// <param name="name">이름</param>
        /// <param name="nickname">닉네임</param>
        /// <param name="password">비밀번호</param>
        /// <param name="myBoxPassword"></param>
        /// <param name="phoneNo">전화번호</param>
        /// <param name="ci">인증 CI 값</param>
        /// <param name="di">인증 DI 값</param>
        /// <param name="certNo">인증 거래번호</param>
        /// <param name="message">처리 결과</param>
        /// <returns></returns>
        public bool AddUser(CreatedFrom createdFrom, string id, string name, string nickname, string password, string myBoxPassword, string phoneNo, string recomCode, string ci, string di, string certNo, string IP, out string message)
        {
            message = string.Empty;

            try
            {
                if (password.Length < 6 || password.Length > 20 /*|| !userService.passwordCHK.IsMatch(form.m_pwd1)*/)
                    throw new YoloException("비밀번호는 영문, 숫자, 특수문자 조합 6~20자로 가능합니다");
                if (name.Length < 1 || name.Length > 10 || !koreanOnly.IsMatch(name))
                    throw new YoloException("이름은 한글만 가능합니다");
                if (id.Length < 6 || id.Length > 20 || !alphaNumericOnly2.IsMatch(id))
                    throw new YoloException("아이디는 영문,숫자 조합으로 6~20자만 가능합니다");
                if (nickname.Length < 2 || nickname.Length > 7 || !koreanNumericOnly.IsMatch(nickname))
                    throw new YoloException("닉네임은 한글, 숫자 2~7자만 가능합니다");

                log.Info("회원 가입 처리 시작. ID = " + id);

                // Player 생성
                {

                    var resultParam = new System.Data.Entity.Core.Objects.ObjectParameter("Out_Result", typeof(short));
                    vongGameDB.SP_WebJoinProc(id, name, nickname, phoneNo, password, recomCode, IP, createdFrom.ToString(), certNo, ci, di, resultParam);
                    log.Info("가입결과 : " + resultParam.Value);

                    byte result = (byte)resultParam.Value;
                    if (result == 0)
                    {
                        log.Info("회원 가입 성공. ID = " + id);
                    }
                    else
                    {
                        switch (result)
                        {
                            case 1:
                                throw new YoloException("사용할 수 없는 아이디입니다. 다른 아이디를 입력해주세요.");
                            case 2:
                                throw new YoloException("사용할 수 없는 닉네임입니다. 다른 닉네임을 입력해주세요.");
                            case 3:
                                throw new YoloException("유효하지 않은 추천인입니다.");
                            default:
                                throw new YoloException("회원가입에 실패하였습니다. 고객센터에 문의해주세요.");
                        }
                    }
                }

                log.Info("회원 가입 끝. ID = " + id);
            }
            catch (YoloException e)
            {
                log.Warn("회원 가입 처리 중 요구 사항 오류", e);
                message = e.Message;
                return false;
            }
            catch (Exception e)
            {
                log.Error("회원 가입 처리 중 오류", e);
                message = "회원 가입 처리 중 오류가 발생하였습니다";
                return false;
            }

            return true;
        }

        public Player TryLogin(string id, string password)
        {
            var player2 = vongGameDB.Player.Where(a => (a.Quit == false)).FirstOrDefault();


            // ID 확인
            var player = vongGameDB.Player.Where(a => (a.UserID == id & a.Active && a.Quit == false)).FirstOrDefault();
            if (player == null)
                return null;

            string passwordHash;
            // Yolo이외 계정인지 확인
            if (player.CreatedFrom.Contains("JUST") || player.CreatedFrom.Contains("BGC"))
            {
                passwordHash = CryptoHelper.makePassword2(password);
            }
            else
            {
                passwordHash = CryptoHelper.makePassword(password);
            }

            //log.Debug("비밀번호 비교. 입력 = " + passwordHash + ", DB = " + player.Password + ", 동일 = " + (password == player.Password));

            // 비밀번호 확인
            if (passwordHash != player.Password)
                return null;

            return player;
        }

        public class PlayerGameScore
        {
            public int MatgoWin;
            public int MatgoLose;
            public int BadugiWin;
            public int BadugiLose;

            /// <summary>
            /// 무료머니
            /// </summary>
            public decimal FreeMoney;
            /// <summary>
            /// 금고 (무료머니)
            /// </summary>
            public decimal SafeMoney;
            /// <summary>
            /// 유료 머니
            /// </summary>
            public decimal PayMoney;

            /// <summary>
            /// 캐쉬
            /// </summary>
            public int Cash;

            /// <summary>
            /// 적립금
            /// </summary>
            public long Mileage;

            public int HaveFriend;

            public int Type;
            public long Mileage2;

        }

        public PlayerGameScore GetPlayerInfo(string playerId)
        {
            var entry = vongGameDB.V_WEB_PlayerInfo.Where(a => a.UserID == playerId).FirstOrDefault();
            PlayerGameScore info = new PlayerGameScore();
            if (entry != null)
            {
                info.BadugiLose = entry.BadugiLose.HasValue ? entry.BadugiLose.Value : 0;
                info.BadugiWin = entry.BadugiWin.HasValue ? entry.BadugiWin.Value : 0;
                info.MatgoLose = entry.MatgoLose.HasValue ? entry.MatgoLose.Value : 0;
                info.MatgoWin = entry.MatgoWin.HasValue ? entry.MatgoWin.Value : 0;

                info.Cash = entry.Cash.HasValue ? entry.Cash.Value : 0;

                info.FreeMoney = entry.FreeMoney.HasValue ? entry.FreeMoney.Value : 0;
                info.SafeMoney = entry.SafeMoney.HasValue ? entry.SafeMoney.Value : 0;
                info.PayMoney = entry.PayMoney.HasValue ? entry.PayMoney.Value : 0;

                info.Mileage = entry.Mileage;
                info.HaveFriend = entry.FriendId.HasValue ? entry.FriendId.Value : 0;
                if (info.HaveFriend != 0)
                {
                    var entry2 = vongGameDB.AdminUser.Where(a => a.Id == info.HaveFriend).FirstOrDefault();
                    if (entry2 != null)
                    {
                        info.Type = entry2.Type;
                        info.Mileage2 = entry2.PointSettlement;
                    }
                }
            }
            else
            {
                log.Warn("UserId[" + playerId + "] 의 게임 정보 없음");
            }

            return info;
        }

        /// <summary>
        /// 탈퇴 처리
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pwd"></param>
        /// <param name="reason"></param>
        public void QuitUser(string id, string pwd, string reason)
        {
            // ID 확인
            var player = vongGameDB.Player.Where(a => a.UserID == id).FirstOrDefault();
            if (player == null)
                throw new Exception("이미 처리된 회원입니다.");

            string passwordHash;
            // Yolo이외 계정인지 확인
            if (player.CreatedFrom.Contains("JUST") || player.CreatedFrom.Contains("BGC"))
            {
                passwordHash = CryptoHelper.makePassword2(pwd);
            }
            else
            {
                passwordHash = CryptoHelper.makePassword(pwd);
            }
            var passcheck = vongGameDB.Player.Where(a => a.UserID == id && a.Password == passwordHash).FirstOrDefault();
            if (passcheck == null)
                throw new Exception("비밀번호를 확인해 주세요");

            // 상태 변경
            player.Active = false;
            player.Quit = true;
            player.QuitReason = reason;
            vongGameDB.Entry(player).State = System.Data.Entity.EntityState.Modified;
            vongGameDB.SaveChanges();
        }

        public int GetAdminMemoCount(string userId)
        {
            return vongGameDB.PlayerAdminMemo.Where(a => a.Player.UserID == userId).Count();
        }
        public List<PlayerAdminMemo> GetAdminMemoList(string userId, int pageNo)
        {
            if (pageNo < 1)
                pageNo = 1;

            int offset = (pageNo - 1) * Constant.PAGE_SIZE;

            return vongGameDB.PlayerAdminMemo.Where(a => a.Player.UserID == userId).OrderByDescending(a => a.id).Skip(offset).Take(10).ToList();
        }

        public void DeleteAdminMemo(string userId, int[] memoIdList)
        {
            using (var tr = vongGameDB.Database.BeginTransaction())
            {
                foreach (var memoId in memoIdList)
                {
                    var entry = vongGameDB.PlayerAdminMemo.Where(a => a.Player.UserID == userId && a.id == memoId).FirstOrDefault();
                    if (entry != null)
                    {
                        vongGameDB.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                        log.Info("User[" + userId + "] 의 관리자쪽지 [" + memoId + "] 삭제");
                    }
                    else
                    {
                        log.Info("User[" + userId + "] 의 관리자쪽지 [" + memoId + "] 를 찾을 수 없음");
                    }
                }
                vongGameDB.SaveChanges();
                tr.Commit();
            }
        }

        public bool UseMyRoomItem(string UserId, int ItemId, out string message)
        {
            var userNo = vongGameDB.Player.Where(a => a.UserID == UserId).FirstOrDefault().Id;

            var out_result = new ObjectParameter("Out_Result", typeof(byte));
            var res = vongGameDB.SP_PlayerMyRoomAction(userNo, ItemId, "use", out_result).ToList();

            if (out_result.Value is DBNull || out_result.Value == null)
                throw new Exception("아이템 사용 결과를 읽을 수 없습니다[1]");

            byte? result = out_result.Value as byte?;
            if (!result.HasValue)
                throw new Exception("아이템 사용 결과를 읽을 수 없습니다[2]");

            bool succeed = result.Value == 0;
            message = result.Value == 0 ? "착용되었습니다." :
                        result.Value == 1 ? "처리를 할 수 없습니다" :
                        result.Value == 2 ? "보유중인 아이템이 아닙니다" :
                        result.Value == 3 ? "이미 사용중인 아이템입니다" :
                        result.Value == 4 ? "사용 기간이 만료된 아이템입니다" :
                        result.Value == 5 ? "사용할 수 없는 아이템입니다" : "";

            return succeed;
        }

        bool CanChangeNickname(int id)
        {
            var MonthAgo = DateTime.Now.AddMonths(-1);
            int count = vongGameDB.LogPlayerNicknameChange.Where(a => a.userNo == id && a.changedWhen > MonthAgo).Count();
            return count < 2;
        }

        /// <summary>
        /// 닉네임 변경 가능한가~
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CanChangeNickname(string userId)
        {
            var player = vongGameDB.Player.Where(a => a.UserID == userId).FirstOrDefault();
            if (player == null)
                return false;

            // 최근 한달 사이 2회 변경 금지
            return CanChangeNickname(player.Id);
        }

        /// <summary>
        /// 닉네임 변경
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="newNickname"></param>
        /// <returns></returns>
        public bool ChangeNickname(string UserId, string newNickname, out string message)
        {
            message = "";

            try
            {
                // 한글 2~7자 확인
                if (newNickname.Length < 2 || newNickname.Length > 7 || CheckEnglish(newNickname))
                {
                    message = "한글, 숫자 2~7글자만 입력할 수 있습니다.";
                    return false;
                }

                using (var tr = vongGameDB.Database.BeginTransaction())
                {
                    // 아이디 중복

                    var player = vongGameDB.Player.Where(a => a.UserID == UserId).FirstOrDefault();
                    if (player == null)
                        throw new YoloException("알수없는 사용자 ID 입니다 [" + UserId + "]");

                    var player2 = vongGameDB.Player.Where(a => a.NickName == newNickname).FirstOrDefault();
                    if (player2 != null && player2.NickName == newNickname)
                        throw new YoloException("이미 사용중인 닉네임 입니다. [" + newNickname + "]");

                    // 최근 한달 사이 2회 변경 금지
                    if (!CanChangeNickname(player.Id))
                        throw new YoloException("닉네임은 한달 사이 2번 넘게 변경 할 수 없습니다");

                    var originalNickname = player.NickName;

                    // Player 정보 갱신
                    player.NickName = newNickname;
                    vongGameDB.Entry(player).State = System.Data.Entity.EntityState.Modified;

                    // 기록 추가
                    var log = new LogPlayerNicknameChange();
                    log.userNo = player.Id;
                    log.from = originalNickname;
                    log.to = newNickname;
                    log.changedWhen = DateTime.Now;
                    vongGameDB.Entry(log).State = System.Data.Entity.EntityState.Added;

                    vongGameDB.SaveChanges();

                    tr.Commit();
                    message = "변경되었습니다";
                }
                return true;
            }
            catch (YoloException e)
            {
                message = e.Message;
                log.Warn("변경 처리 중 오류", e);
            }
            catch (Exception e)
            {
                message = "오류가 발생하였습니다";
                log.Error("닉네임 변경 오류", e);
            }

            return false;
        }
        /// <summary>
        /// 비밀번호 변경
        /// </summary>
        public bool ChangePassword(string UserId, string newPassword, string newPassword2, string oldPassword, out string message)
        {
            message = "";

            try
            {
                // 새 비밀번호 확인
                if (newPassword.Length < 6 || newPassword.Length > 20 || alphaNumericOnly.IsMatch(newPassword) == false)
                    throw new YoloException("새 비밀번호는 영문과 숫자 조합으로 6~20자만 가능합니다");
                if (newPassword != newPassword2)
                    throw new YoloException("입력하신 새 비밀번호가 일치하지 않습니다");

                // 현재 비밀번호 확인
                var result = TryLogin(UserId, oldPassword);
                if (result == null)
                    throw new YoloException("현재 비밀번호를 확인해 주세요");

                using (var tr = vongGameDB.Database.BeginTransaction())
                {
                    var player = vongGameDB.Player.Where(a => a.UserID == UserId).FirstOrDefault();
                    if (player == null)
                        throw new YoloException("알수없는 사용자 ID 입니다 [" + UserId + "]");

                    // 비밀번호 변경
                    string passwordHash;
                    // Yolo이외 계정인지 확인
                    if (player.CreatedFrom.Contains("JUST") || player.CreatedFrom.Contains("BGC"))
                    {
                        passwordHash = CryptoHelper.makePassword2(newPassword);
                    }
                    else
                    {
                        passwordHash = CryptoHelper.makePassword(newPassword);
                    }
                    log.Info("이용자[" + UserId + "]의 비밀번호[" + passwordHash + "] 변경");

                    // Player 정보 갱신
                    player.Password = passwordHash;
                    vongGameDB.Entry(player).State = System.Data.Entity.EntityState.Modified;

                    vongGameDB.SaveChanges();

                    tr.Commit();
                    message = "변경되었습니다";
                }
                return true;
            }
            catch (YoloException e)
            {
                message = e.Message;
                log.Warn("변경 처리 중 오류", e);
            }
            catch (Exception e)
            {
                message = "오류가 발생하였습니다";
                log.Error("비밀번호 변경 오류", e);
            }

            return false;
        }
        #region #. CheckEnglish
        /// <summary>
        /// 영문체크
        /// </summary>
        /// <param name="letter">문자
        /// 
        public static bool CheckEnglish(string letter)
        {
            bool IsCheck = true;

            Regex engRegex = new Regex(@"[a-zA-Z]");
            Boolean ismatch = engRegex.IsMatch(letter);

            if (!ismatch)
            {
                IsCheck = false;
            }

            return IsCheck;
        }
        #endregion

        #region #. CheckNumber
        /// <summary>
        /// 숫자체크
        /// </summary>
        /// <param name="letter">문자
        /// 
        public static bool CheckNumber(string letter)
        {
            bool IsCheck = true;

            Regex numRegex = new Regex(@"[0-9]");
            Boolean ismatch = numRegex.IsMatch(letter);

            if (!ismatch)
            {
                IsCheck = false;
            }

            return IsCheck;
        }
        #endregion

        #region #. CheckEnglishNumber
        /// <summary>
        /// 영문/숫자체크
        /// </summary>
        /// <param name="letter">문자
        /// 
        public static bool CheckEnglishNumber(string letter)
        {
            bool IsCheck = true;

            Regex engRegex = new Regex(@"[a-zA-Z]");
            Boolean ismatch = engRegex.IsMatch(letter);
            Regex numRegex = new Regex(@"[0-9]");
            Boolean ismatchNum = numRegex.IsMatch(letter);

            if (!ismatch && !ismatchNum)
            {
                IsCheck = false;
            }

            return IsCheck;
        }
        #endregion

        /// <summary>
        /// 탈퇴한 회원을 제외한 모든 회원수
        /// </summary>
        /// <returns></returns>
        public int GetTotalMemberCount()
        {
            return vongGameDB.Player.Where(a => a.Quit == false).Count();
        }

        /// <summary>
        /// 총 회원 수 (탈퇴 + 현존)
        /// </summary>
        /// <returns></returns>
        public int GetTotalAccumulatedMemberCount()
        {
            return vongGameDB.Player.Count();
        }

        public List<Player> GetMemberPaging(int start, int size, OrderDirectionEnum userIdOrder, string search)
        {
            IQueryable<Player> entries = vongGameDB.Player;
            if (!string.IsNullOrEmpty(search))
                entries = entries.Where(a => a.UserID.Contains(search));

            var sortedEntries = userIdOrder == OrderDirectionEnum.asc ? entries.OrderBy(a => a.UserID) : entries.OrderByDescending(a => a.UserID);

            return sortedEntries.Skip(start).Take(size).ToList();
        }

        public int GetMemberUserListCount(int adminId)
        {
            return vongGameDB.V_MemberUserList.Where(a => a.FriendID == adminId).Count();
        }
        public List<DBLIB.V_MemberUserList> GetMemberUserList(int adminId)
        {
            return vongGameDB.V_MemberUserList.Where(a => a.FriendID == adminId).ToList();
        }
        public List<DBLIB.LogMemberDailyPoint> GetMemberUserList(int adminId, int days)
        {
            var t = DateTime.Now.AddDays((double)(-days));
            var t2 = DateTime.Now.AddDays((double)((-days) + 1));
            return vongGameDB.LogMemberDailyPoint.Where(a => a.ShopId == adminId && a.Date >= t && a.Date <= t2).ToList();
        }

        public int GetMemberBonusListCount(int adminId)
        {
            return vongGameDB.V_MemberFriendList.Where(a => a.FriendID == adminId).Count();
        }
        public List<DBLIB.V_MemberFriendList> GetMemberBonusList(int adminId)
        {
            return vongGameDB.V_MemberFriendList.Where(a => a.FriendID == adminId).ToList();
        }
        public List<DBLIB.LogMemberDailyPoint2> GetMemberBonusList(int adminId, int days)
        {
            var t = DateTime.Now.AddDays((double)(-days));
            var t2 = DateTime.Now.AddDays((double)((-days) + 1));
            return vongGameDB.LogMemberDailyPoint2.Where(a => a.ShopId == adminId && a.Date >= t && a.Date <= t2).ToList();
        }

        public List<V_FriendList> GetFriendListData(int adminId)
        {
            List<V_FriendList> data = new List<V_FriendList>();

            // 친구목록 재귀 검색
            var MyFriends = GetFriendListLow(adminId);

            data.AddRange(MyFriends);

            return data;
        }

        private List<V_FriendList> GetFriendListLow(int FriendId)
        {
            List<V_FriendList> data = new List<V_FriendList>();

            // 내 친구목록
            data.AddRange(vongGameDB.V_FriendList.Where(a => a.FriendID == FriendId).ToList());

            var F = vongGameDB.AdminUser.Where(a => a.UpperMemberId == FriendId).ToList();

            if (F.Count == 0) return data;

            foreach (var f in F)
            {
                //// 내 밑단계 친구목록
                //var LowFriends = vongGameDB.V_FriendList.Where(a => a.FriendID == f.Id).ToList();
                //data.AddRange(LowFriends);

                // 내 밑단계 친구목록 재귀 검색
                var LowFriendData = GetFriendListLow(f.Id);
                if (LowFriendData.Count > 0)
                {
                    data.AddRange(LowFriendData);
                }
            }

            return data;
        }

        public List<V_FriendList> GetFriendSanpShotListData(int adminId, int dayBack)
        {
            List<V_FriendList> data = new List<V_FriendList>();

            // 친구목록 재귀 검색
            var MyFriends = GetFriendSanpShotListLow(adminId, dayBack);

            data.AddRange(MyFriends);

            return data;
        }

        private List<V_FriendList> GetFriendSanpShotListLow(int FriendId, int dayBack)
        {
            List<V_FriendList> data = new List<V_FriendList>();
            List<FriendDaySnapshot> dataSnapshot = new List<FriendDaySnapshot>();

            DateTime sDate = DateTime.Now.AddDays(-dayBack-1);
            DateTime eDate = sDate.AddDays(1);

            // 내 친구목록
            dataSnapshot.AddRange(vongGameDB.FriendDaySnapshot.Where(a => a.FriendID == FriendId && a.Date > sDate && a.Date <= eDate).ToList());
            foreach (var tDss in dataSnapshot)
            {
                V_FriendList tempdata = new V_FriendList();

                tempdata.FriendID = tDss.FriendID;
                tempdata.FriendName = tDss.FriendName;
                tempdata.UserID = tDss.UserID;
                tempdata.NickName = tDss.NickName;
                tempdata.MatgoWin = tDss.MatgoWin;
                tempdata.MatgoLose = tDss.MatgoLose;
                tempdata.MatgoBetMoney = tDss.MatgoBetMoney;
                tempdata.BadugiWin = tDss.BadugiWin;
                tempdata.BadugiLose = tDss.BadugiLose;
                tempdata.BadugiBetMoney = tDss.BadugiBetMoney;
                tempdata.FreeMoney = tDss.FreeMoney;
                tempdata.SafeMoney = tDss.SafeMoney;

                data.Add(tempdata);
            }

            var F = vongGameDB.AdminUser.Where(a => a.UpperMemberId == FriendId).ToList();

            if (F.Count == 0) return data;

            foreach (var f in F)
            {
                //// 내 밑단계 친구목록
                //var LowFriends = vongGameDB.V_FriendList.Where(a => a.FriendID == f.Id).ToList();
                //data.AddRange(LowFriends);

                // 내 밑단계 친구목록 재귀 검색
                var LowFriendData = GetFriendSanpShotListLow(f.Id, dayBack);
                if (LowFriendData.Count > 0)
                {
                    data.AddRange(LowFriendData);
                }
            }

            return data;
        }

        public int GetMyLogCount(string userId)
        {
            var userNo = vongGameDB.Player.Where(a => a.UserID == userId).FirstOrDefault().Id;

            return vongGameDB.V_LogDetail.Where(a => a.UserId == userNo && (a.LogTypeID == 7 || a.LogTypeID == 8 || a.LogTypeID == 0 || a.LogTypeID == 1 || a.LogTypeID == 10 || a.LogTypeID == 11 || a.LogTypeID == 12)).Count();
        }
        public List<V_LogDetail> GetMyLogList(string userId, int pageNo)
        {
            var userNo = vongGameDB.Player.Where(a => a.UserID == userId).FirstOrDefault().Id;

            if (pageNo < 1)
                pageNo = 1;

            int offset = (pageNo - 1) * Constant.PAGE_SIZE;

            return vongGameDB.V_LogDetail.Where(a => a.UserId == userNo && (a.LogTypeID == 7 || a.LogTypeID == 8 || a.LogTypeID == 0 || a.LogTypeID == 1 || a.LogTypeID == 10 || a.LogTypeID == 11 || a.LogTypeID == 12)).OrderByDescending(a => a.Time).Skip(offset).Take(10).ToList();
        }
        public EventJackpotHunter GetEventJackpotHunterInfo(string userId)
        {
            var userNo = vongGameDB.Player.Where(a => a.UserID == userId).FirstOrDefault().Id;

            return vongGameDB.EventJackpotHunter.Where(a => a.UserId == userNo).FirstOrDefault();
        }

        
        public DateTime GetRestrictedTime(string userId)
        {
            var dt = vongGameDB.Player.Where(a => a.UserID == userId).FirstOrDefault().SelfRestrictedDate;

            return dt;
        }

        /// <summary>
        /// 탈퇴 처리
        /// </summary>
        /// <param name="id"></param>
        /// <param name="day"></param>
        public bool RestrictUser(string UserId, string day, out string message)
        {
            message = "";

            try
            {
                if (string.IsNullOrEmpty(day))
                    throw new YoloException("제한할 날짜(일수)를 입력하세요.");

                int numDay = 0;
                if (int.TryParse(day, out numDay) == false)
                    throw new YoloException("제한할 날짜(일수)를 입력하세요.");

                if (numDay <= 0 || numDay > 30)
                    throw new YoloException("최소 1일, 최대 30일까지 설정할 수 있습니다.");

                // 날짜 확인, 오직 늘릴수만 있다.
                var oldRestrictedTime = GetRestrictedTime(UserId);
                var newRestrictedTime = DateTime.Now.AddDays(numDay);
                if (oldRestrictedTime > newRestrictedTime)
                {
                    throw new YoloException("이미 설정된 날짜 이전으로는 설정할 수 없습니다.");
                }

                using (var tr = vongGameDB.Database.BeginTransaction())
                {
                    var player = vongGameDB.Player.Where(a => a.UserID == UserId).FirstOrDefault();
                    if (player == null)
                        throw new YoloException("알수없는 사용자 ID 입니다 [" + UserId + "]");

                    // 이용제한 날짜 변경
                    player.SelfRestrictedDate = newRestrictedTime;
                    log.Info("이용자[" + UserId + "]의 이용제한 자가설정[" + newRestrictedTime.ToString() + "] 변경");

                    vongGameDB.Entry(player).State = System.Data.Entity.EntityState.Modified;

                    vongGameDB.SaveChanges();

                    tr.Commit();
                    message = newRestrictedTime.ToString("현재부터 yyyy년 MM월 dd일 hh시 mm분 ss초까지 게임에 접속하실 수 없습니다.");
                }
                return true;
            }
            catch (YoloException e)
            {
                message = e.Message;
                //log.Warn("처리 오류", e);
            }
            catch (Exception e)
            {
                message = "오류가 발생하였습니다";
                log.Error("이용제한 자가설정 오류", e);
            }

            return false;
        }

    }
}
