using Server.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZNet;

namespace Server.User
{
    class LoginServer : UserServer
    {
        //클라이언트 목록
        public ConcurrentDictionary<ZNet.RemoteID, CPlayer> RemoteClients = new ConcurrentDictionary<ZNet.RemoteID, CPlayer>();
        // 로비리스트 목록
        public ConcurrentDictionary<ZNet.RemoteID, ZNet.MasterInfo> lobby_list = new ConcurrentDictionary<ZNet.RemoteID, ZNet.MasterInfo>();

        int UserLimit = 3000; // 접속자 수 제한

        public LoginServer(FormServer f, UnityCommon.Server t, int portnum) : base(f, t, portnum)
        {
        }

        protected override void BeforeServerStart(out StartOption param)
        {
            base.BeforeServerStart(out param);
            param.m_UpdateTimeMs = 1000;
            m_Core.update_event_handler = ScheduleTask;

            stub.master_all_shutdown = ShutDownServer;

            stub.Chat = Chat;
            stub.request_LoginKey = RequestLoginKey;

            stub.request_Login = RequestLogin;
            stub.request_lobby_list = RequestLobbyList;
            //해당 서버 타입의 조건을 검사한 후 응답 처리
            stub.request_go_lobby = RequestGoLobby;

            m_Core.client_join_handler = ClientJoinHandler; // ★
            m_Core.client_leave_handler = ClientLeaveHandler; // ★

            m_Core.move_server_start_handler = MoveServerStart;
            m_Core.move_server_failed_handler = MoveServerFailed;
            m_Core.message_handler = CoreMessage;
            m_Core.exception_handler = CoreException;

            m_Core.server_join_handler = ServerJoinHandler;
            m_Core.server_leave_handler = ServerLeaveHandler;

            m_Core.server_master_join_handler = ServerMasterJoinHandler;
            m_Core.server_master_leave_handler = ServerMasterLeaveHandler;

            m_Core.server_refresh_handler = RefreshHandler;

        }
        #region Client Request Handler
        bool Chat(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string msg)
        {
            form.printf("Remote[{0}] msg : {1}", remote, msg);
            proxy.Chat(remote, ZNet.CPackOption.Basic, msg);
            return true;
        }
        bool RequestLoginKey(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string key)
        {
            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc) == false) return false;
            string ip = rc.m_ip;

            Task.Run(async () =>
            {
                UserData dummy = new UserData();

                var result = await Task.Run(() =>
                {
                    //form.printf("[로그인 인증] 로그인 시도\n");

                    if (ServerMaintenance == true)
                    {
                        return 8; // 서버 점검중
                    }

                    try
                    {
                        // 접속자 수 확인
                        int Data_CurrentUser = Simple.Data.Database.Open().GameCurrentUser.GetCount();
                        if (Data_CurrentUser >= UserLimit)
                        {
                            return 9; // 접속자 수 제한
                        }

                        dynamic Data_BlockIP = Simple.Data.Database.Open().AdminBlockIP.FindAllByIP(ip).FirstOrDefault();
                        if (Data_BlockIP != null && Data_BlockIP.Blocking > DateTime.Now)
                        {
                            return 6; // 차단된 IP
                        }

                        dynamic Data_Player = Simple.Data.Database.Open().Player.FindAllByUserID(id).FirstOrDefault();
                        if (Data_Player == null)
                        {
                            return 4; // 없는 계정
                        }

                        dynamic Data_Password = Simple.Data.Database.Open().PlayerPassword.FindAllByUserID(Data_Player.Id).FirstOrDefault();
                        if (Data_Password != null && Data_Password.Password == key)
                        {
                            // 인증에 사용된 키는 파기되어 재사용 불가능
                            Simple.Data.Database.Open().PlayerPassword.UpdateByUserId(UserId: Data_Player.Id, Password: "");
                            Simple.Data.Database.Open().LogUserLogin.Insert(UserId: Data_Player.Id, GameId: GameId, IP: ip, Pass: true);
                        }
                        else
                        {
                            Simple.Data.Database.Open().LogUserLogin.Insert(UserId: Data_Player.Id, GameId: GameId, IP: ip, Pass: false);
                            return 3; // 비밀번호 틀림
                        }

                        dynamic Data_BlockUser = Simple.Data.Database.Open().AdminBlockUser.FindAllByUserId(Data_Player.Id).FirstOrDefault();
                        if (Data_BlockUser != null && Data_BlockUser.Blocking > DateTime.Now)
                        {
                            return 7; // 차단된 회원
                        }

                        dynamic Data_Current = Simple.Data.Database.Open().GameCurrentUser.FindAllByUserID(Data_Player.Id).FirstOrDefault();
                        if (Data_Current != null)
                        {
                            if (Data_Current.AutoPlay == true)
                            {
                                return 5; // 자동치기 처리중
                            }
                            else
                            {
                                return 2; // 중복 접속
                            }
                        }

                        // 접속 정보 확인
                        dummy.ID = Data_Player.Id;
                        dummy.userID = Data_Player.UserID;
                        dummy.nickName = Data_Player.NickName;
                        dummy.member_point = Data_Player.Point;

                        // 매장 정보 확인
                        if (Data_Player.ShopId != 0)
                        {
                            dynamic Data_Shop = Simple.Data.Database.Open().AdminUser.FindAllById(Data_Player.ShopId).FirstOrDefault();

                            dummy.shop_name = Data_Shop.Title;
                        }
                        else
                        {
                            dummy.shop_name = "";
                        }

                        // 보유 아이템
                        bool AvatarUsing = false;
                        int DefaultAvatarId = 0;
                        string DefaultAvatar = "";
                        int DefaultAvatarVoice = 0;

                        bool CardUsing = false;
                        int DefaultCardId = 0;
                        string DefaultCard = "";

                        dynamic Data_Item = Simple.Data.Database.Open().V_PlayerItemList.FindAllByUserId(dummy.ID);
                        dummy.avatar = "";
                        dummy.avatar_card = "";
                        if (Data_Item == null || Data_Item.ToList().Count == 0)
                        {
                            Simple.Data.Database.Open().PlayerItemList.Insert(UserId: dummy.ID, ItemId: 10, Count: 1, Using: true);
                            Simple.Data.Database.Open().PlayerItemList.Insert(UserId: dummy.ID, ItemId: 11, Count: 1, Using: false);
                            Simple.Data.Database.Open().PlayerItemList.Insert(UserId: dummy.ID, ItemId: 22, Count: 1, Using: true);
                            Simple.Data.Database.Open().PlayerItemList.Insert(UserId: dummy.ID, ItemId: 28, Count: 1, Using: true);
                            Data_Item = Simple.Data.Database.Open().V_PlayerItemList.FindAllByUserId(dummy.ID);
                        }
                        foreach (var row in Data_Item.ToList())
                        {
                            // 아이템 만료됐으면 기본으로 변경
                            switch (row.ptype)
                            {
                                case "avatar":
                                    {
                                        if (row.Using == false)
                                        {
                                            if (AvatarUsing == false && row.value2 == 1)
                                            {
                                                DefaultAvatarId = row.Id;
                                                DefaultAvatar = row.string1;
                                                DefaultAvatarVoice = row.value1;
                                            }
                                            continue;
                                        }

                                        if (row.ExpireDate != null && row.ExpireDate < DateTime.Now)
                                        {
                                            Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: row.Id, Using: false);
                                        }
                                        else
                                        {
                                            dummy.avatar = row.string1;
                                            dummy.voice = row.value1;
                                            AvatarUsing = true;
                                        }
                                    }
                                    break;
                                case "card":
                                    {
                                        if (row.Using == false)
                                        {
                                            if (CardUsing == false && row.value2 == 1)
                                            {
                                                DefaultCardId = row.Id;
                                                DefaultCard = row.string1;
                                            }
                                            continue;
                                        }

                                        if (row.ExpireDate != null && row.ExpireDate < DateTime.Now)
                                        {
                                            Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: row.Id, Using: false);
                                        }
                                        else
                                        {
                                            dummy.avatar_card = row.string1;
                                            CardUsing = true;
                                        }
                                    }
                                    break;
                            }
                        }
                        // 만료된 아이템은 기본 아이템으로 변경 착용
                        if (DefaultAvatarId != 0 && AvatarUsing == false)
                        {
                            Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: DefaultAvatarId, Using: true);
                            dummy.avatar = DefaultAvatar;
                            dummy.voice = DefaultAvatarVoice;
                        }
                        if (DefaultCardId != 0 && CardUsing == false)
                        {
                            Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: DefaultCardId, Using: true);
                            dummy.avatar_card = DefaultCard;
                        }

                        dynamic Data_Money = Simple.Data.Database.Open().PlayerGameMoney.FindAllByUserID(dummy.ID).FirstOrDefault();
                        if (Data_Money == null)
                        {
                            bool FullMember = false; // 정회원 여부
                            long GiveGameMoney = 0;
                            long GivePayMoney = 0;
                            dynamic Data_GiveMoney = Simple.Data.Database.Open().GameGiveMoney.FindAllByMoneyType(1).FirstOrDefault(); // 무료머니
                            if (FullMember)
                                GiveGameMoney = Data_GiveMoney.FullMemberMoney;
                            else
                                GiveGameMoney = Data_GiveMoney.MemberMoney;
                            Data_GiveMoney = Simple.Data.Database.Open().GameGiveMoney.FindAllByMoneyType(2).FirstOrDefault(); // 유료머니
                            if (FullMember)
                                GivePayMoney = Data_GiveMoney.FullMemberMoney;
                            else
                                GivePayMoney = Data_GiveMoney.MemberMoney;

                            Simple.Data.Database.Open().PlayerGameMoney.Insert(UserId: dummy.ID, Cash: 0, GameMoney: GiveGameMoney, PayMoney: GivePayMoney);
                            Data_Money = Simple.Data.Database.Open().PlayerGameMoney.FindAllByUserID(dummy.ID).FirstOrDefault();
                        }
                        dummy.cash = Data_Money.Cash;
                        dummy.money_pay = (long)Data_Money.PayMoney;
                        dummy.money_free = (long)Data_Money.GameMoney;

                        //dynamic Data_Lotto = Simple.Data.Database.Open().EventLotto.FindAllByUserID(dummy.ID);
                        //dummy.charm = Data_Lotto.ToList().Count;
                        dummy.charm = 0;

                        dynamic Data_SafeBox = Simple.Data.Database.Open().PlayerSafeBox.FindAllByUserID(dummy.ID).FirstOrDefault();
                        if (Data_SafeBox == null)
                        {
                            Simple.Data.Database.Open().PlayerSafeBox.Insert(UserId: dummy.ID);
                            Data_SafeBox = Simple.Data.Database.Open().PlayerSafeBox.FindAllByUserID(dummy.ID).FirstOrDefault();
                        }
                        dummy.bank_money_pay = (long)Data_SafeBox.SafeMoney2;
                        dummy.bank_money_free = (long)Data_SafeBox.SafeMoney;

                        dynamic Data_Matgo = Simple.Data.Database.Open().PlayerMatgo.FindAllByUserID(dummy.ID).FirstOrDefault();
                        if (Data_Matgo == null)
                        {
                            Simple.Data.Database.Open().PlayerMatgo.Insert(UserId: dummy.ID);
                            Data_Matgo = Simple.Data.Database.Open().PlayerMatgo.FindAllByUserID(dummy.ID).FirstOrDefault();
                        }
                        dummy.winCount = Data_Matgo.Win;
                        dummy.loseCount = Data_Matgo.Lose;

                        dynamic Data_Badugi = Simple.Data.Database.Open().PlayerBadugi.FindAllByUserID(dummy.ID).FirstOrDefault();
                        if (Data_Badugi == null)
                        {
                            Simple.Data.Database.Open().PlayerBadugi.Insert(UserId: dummy.ID);
                        }

                        //dummy.topMission = new List<CPlayerAgent.MissionData>();
                        //dynamic Data_Mission = Simple.Data.Database.Open().PlayerMatgoMission.FindAllByUserID(dummy.ID).FirstOrDefault();
                        //if (Data_Mission == null)
                        //{
                        //    NewMissionData(ref dummy.topMission);
                        //    Simple.Data.Database.Open().PlayerMatgoMission.Insert(UserId: dummy.ID, Mission1: dummy.topMission[0].type, Complete1: dummy.topMission[0].isComplete, Mission2: dummy.topMission[1].type, Complete2: dummy.topMission[1].isComplete, Mission3: dummy.topMission[2].type, Complete3: dummy.topMission[2].isComplete, Mission4: dummy.topMission[3].type, Complete4: dummy.topMission[3].isComplete, Mission5: dummy.topMission[4].type, Complete5: dummy.topMission[4].isComplete, Mission6: dummy.topMission[5].type, Complete6: dummy.topMission[5].isComplete, Mission7: dummy.topMission[6].type, Complete7: dummy.topMission[6].isComplete, Mission8: dummy.topMission[7].type, Complete8: dummy.topMission[7].isComplete, Mission9: dummy.topMission[8].type, Complete9: dummy.topMission[8].isComplete, Mission10: dummy.topMission[9].type, Complete10: dummy.topMission[9].isComplete);
                        //    Data_Mission = Simple.Data.Database.Open().PlayerMatgoMission.FindAllByUserID(dummy.ID).FirstOrDefault();
                        //}
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission1, Data_Mission.Complete1);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission2, Data_Mission.Complete2);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission3, Data_Mission.Complete3);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission4, Data_Mission.Complete4);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission5, Data_Mission.Complete5);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission6, Data_Mission.Complete6);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission7, Data_Mission.Complete7);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission8, Data_Mission.Complete8);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission9, Data_Mission.Complete9);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission10, Data_Mission.Complete10);

                        // 로그인처리
                        Simple.Data.Database.Open().Player.UpdateById(Id: dummy.ID, LastLoginDateDate: DateTime.Now, LastIP: ip);
                        DB_User_Login(dummy.ID, ip);

                        return 1;
                    }
                    catch (Exception e)
                    {
                        form.printf("[로그인 인증] 예외발생 {0}\n", e.ToString());
                        DB_User_Logout(dummy.ID, 2);
                    }

                    return 0;
                });


                // 상단의 DB Task가 완료되는 시점이므로 다시 유저가 유효한 상태인지 확인합니다
                if (RemoteClients.TryGetValue(remote, out rc) == false)
                {
                    form.printf("[로그인 인증] 로그인 취소 {0}\n", id);
                    DB_User_Logout(dummy.ID, 2);
                    return;
                }

                // 인증 성공
                if (result == 1)
                {
                    rc.data = dummy;
                    rc.joined = true;
                    //form.printf("[로그인 인증] 로그인 성공 {0}\n", id);
                    proxy.response_LoginKey(remote, ZNet.CPackOption.Basic, true);
                }
                else
                {
                    string ResultType;
                    if (result == 2) // 중복 로그인
                        ResultType = "이미 접속중인 아이디입니다.";
                    else if (result == 5) // 자동치기
                        ResultType = "이전 게임에서 퇴장하여 자동치기가 진행중입니다.\n잠시후 다시 시도하세요.";
                    else if (result == 6) // 차단된 IP
                        ResultType = "차단된 IP 입니다.\n고객센터에 문의해주세요.";
                    else if (result == 7) // 차단된 회원
                        ResultType = "차단된 아이디 입니다.\n고객센터에 문의해주세요.";
                    else if (result == 8) // 서버 점검중
                        ResultType = "서버 점검중 입니다.\n홈페이지의 공지사항을 확인하세요.";
                    else if (result == 9) // 접속인원 수 제한
                        ResultType = "접속 인원이 초과했습니다.\n잠시후 다시 시도해 주시기 바랍니다.";
                    else // 입력정보 오류, 접속 오류
                        ResultType = "접속에 실패했습니다.\n오류가 계속 발생한다면\n고객센터에 문의해주세요.";

                    form.printf("[로그인 인증] 로그인 실패 {0}, {1}\n", id, result);

                    // 인증 실패를 알려줌
                    proxy.response_LoginKey(remote, ZNet.CPackOption.Basic, false, ResultType);
                }
                return;
            });

            return true;
        }
        bool RequestLogin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string pass)
        {
            // 유효한 유저인지 확인
            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc) == false) return false;
            string ip = rc.m_ip;

            Task.Run(async () =>
            {
                UserData dummy = new UserData();

                var result = await Task.Run(() =>
                {
                    //form.printf("[로그인] 로그인 시작\n");

                    if (ServerMaintenance == true)
                    {
                        return 8; // 서버 점검중
                    }

                    try
                    {
                        // 접속자 수 확인
                        int Data_CurrentUser = Simple.Data.Database.Open().GameCurrentUser.GetCount();
                        if (Data_CurrentUser >= UserLimit)
                        {
                            return 9; // 접속자 수 제한
                        }

                        dynamic Data_BlockIP = Simple.Data.Database.Open().AdminBlockIP.FindAllByIP(ip).FirstOrDefault();
                        if (Data_BlockIP != null && Data_BlockIP.Blocking > DateTime.Now)
                        {
                            return 6; // 차단된 IP
                        }

                        dynamic Data_Player = Simple.Data.Database.Open().Player.FindAllByUserID(id).FirstOrDefault();
                        if (Data_Player == null)
                        {
                            return 4; // 없는 계정
                        }

                        SHA1 sha = SHA1.Create();
                        if (Data_Player.Password != HexStringFromBytes(sha.ComputeHash(Encoding.UTF8.GetBytes("YOLO" + pass))))
                        {
                            // 비밀번호 틀림
                            return 3;
                        }

                        Simple.Data.Database.Open().LogUserLogin.Insert(UserId: Data_Player.Id, GameId: GameId, IP: ip, Pass: true);

                        // 로그인 키 생성
                        string key_ = HexStringFromBytes(sha.ComputeHash(Encoding.UTF8.GetBytes(id + DateTime.Now.ToString() + "vong")));

                        dynamic Data_Password = Simple.Data.Database.Open().PlayerPassword.FindAllByUserID(Data_Player.Id).FirstOrDefault();
                        if (Data_Password == null)
                        {
                            Simple.Data.Database.Open().PlayerPassword.Insert(UserId: Data_Player.Id, Password: key_, CreatedOnUtc: DateTime.Now);
                        }
                        else
                        {
                            Simple.Data.Database.Open().PlayerPassword.UpdateByUserId(UserId: Data_Player.Id, Password: key_, CreatedOnUtc: DateTime.Now);
                        }

                        dynamic Data_BlockUser = Simple.Data.Database.Open().AdminBlockUser.FindAllByUserId(Data_Player.Id).FirstOrDefault();
                        if (Data_BlockUser != null && Data_BlockUser.Blocking > DateTime.Now)
                        {
                            return 7; // 차단된 회원
                        }

                        dynamic Data_Current = Simple.Data.Database.Open().GameCurrentUser.FindAllByUserID(Data_Player.Id).FirstOrDefault();
                        if (Data_Current != null)
                        {
                            if (Data_Current.AutoPlay == true)
                            {
                                return 5; // 자동치기 처리중
                            }
                            else
                            {
                                return 2; // 중복 접속
                            }
                        }

                        // 접속 정보 확인
                        dummy.ID = Data_Player.Id;
                        dummy.userID = Data_Player.UserID;
                        dummy.nickName = Data_Player.NickName;
                        dummy.member_point = Data_Player.Point;

                        // 매장 정보 확인
                        if (Data_Player.ShopId != 0)
                        {
                            dynamic Data_Shop = Simple.Data.Database.Open().AdminUser.FindAllById(Data_Player.ShopId).FirstOrDefault();

                            dummy.shop_name = Data_Shop.Title;
                        }
                        else
                        {
                            dummy.shop_name = "";
                        }

                        // 보유 아이템
                        bool AvatarUsing = false;
                        int DefaultAvatarId = 0;
                        string DefaultAvatar = "";
                        int DefaultAvatarVoice = 0;

                        bool CardUsing = false;
                        int DefaultCardId = 0;
                        string DefaultCard = "";

                        dynamic Data_Item = Simple.Data.Database.Open().V_PlayerItemList.FindAllByUserId(dummy.ID);
                        dummy.avatar = "";
                        dummy.avatar_card = "";
                        if (Data_Item == null || Data_Item.ToList().Count == 0)
                        {
                            Simple.Data.Database.Open().PlayerItemList.Insert(UserId: dummy.ID, ItemId: 10, Count: 1, Using: true);
                            Simple.Data.Database.Open().PlayerItemList.Insert(UserId: dummy.ID, ItemId: 11, Count: 1, Using: false);
                            Simple.Data.Database.Open().PlayerItemList.Insert(UserId: dummy.ID, ItemId: 22, Count: 1, Using: true);
                            Simple.Data.Database.Open().PlayerItemList.Insert(UserId: dummy.ID, ItemId: 28, Count: 1, Using: true);
                            Data_Item = Simple.Data.Database.Open().V_PlayerItemList.FindAllByUserId(dummy.ID);
                        }
                        foreach (var row in Data_Item.ToList())
                        {
                            // 아이템 만료됐으면 기본으로 변경
                            switch (row.ptype)
                            {
                                case "avatar":
                                    {
                                        if (row.Using == false)
                                        {
                                            if (AvatarUsing == false && row.value2 == 1)
                                            {
                                                DefaultAvatarId = row.Id;
                                                DefaultAvatar = row.string1;
                                                DefaultAvatarVoice = row.value1;
                                            }
                                            continue;
                                        }

                                        if (row.ExpireDate != null && row.ExpireDate < DateTime.Now)
                                        {
                                            Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: row.Id, Using: false);
                                        }
                                        else
                                        {
                                            dummy.avatar = row.string1;
                                            dummy.voice = row.value1;
                                            AvatarUsing = true;
                                        }
                                    }
                                    break;
                                case "card":
                                    {
                                        if (row.Using == false)
                                        {
                                            if (CardUsing == false && row.value2 == 1)
                                            {
                                                DefaultCardId = row.Id;
                                                DefaultCard = row.string1;
                                            }
                                            continue;
                                        }

                                        if (row.ExpireDate != null && row.ExpireDate < DateTime.Now)
                                        {
                                            Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: row.Id, Using: false);
                                        }
                                        else
                                        {
                                            dummy.avatar_card = row.string1;
                                            CardUsing = true;
                                        }
                                    }
                                    break;
                            }
                        }
                        // 만료된 아이템은 기본 아이템으로 변경 착용
                        if (DefaultAvatarId != 0 && AvatarUsing == false)
                        {
                            Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: DefaultAvatarId, Using: true);
                            dummy.avatar = DefaultAvatar;
                            dummy.voice = DefaultAvatarVoice;
                        }
                        if (DefaultCardId != 0 && CardUsing == false)
                        {
                            Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: DefaultCardId, Using: true);
                            dummy.avatar_card = DefaultCard;
                        }

                        dynamic Data_Money = Simple.Data.Database.Open().PlayerGameMoney.FindAllByUserID(dummy.ID).FirstOrDefault();
                        if (Data_Money == null)
                        {
                            bool FullMember = false; // 정회원 여부
                            long GiveGameMoney = 0;
                            long GivePayMoney = 0;
                            dynamic Data_GiveMoney = Simple.Data.Database.Open().GameGiveMoney.FindAllByMoneyType(1).FirstOrDefault(); // 무료머니
                            if (FullMember)
                                GiveGameMoney = Data_GiveMoney.FullMemberMoney;
                            else
                                GiveGameMoney = Data_GiveMoney.MemberMoney;
                            Data_GiveMoney = Simple.Data.Database.Open().GameGiveMoney.FindAllByMoneyType(2).FirstOrDefault(); // 유료머니
                            if (FullMember)
                                GivePayMoney = Data_GiveMoney.FullMemberMoney;
                            else
                                GivePayMoney = Data_GiveMoney.MemberMoney;

                            Simple.Data.Database.Open().PlayerGameMoney.Insert(UserId: dummy.ID, Cash: 0, GameMoney: GiveGameMoney, PayMoney: GivePayMoney);
                            Data_Money = Simple.Data.Database.Open().PlayerGameMoney.FindAllByUserID(dummy.ID).FirstOrDefault();
                        }
                        dummy.cash = Data_Money.Cash;
                        dummy.money_pay = (long)Data_Money.PayMoney;
                        dummy.money_free = (long)Data_Money.GameMoney;

                        //dynamic Data_Lotto = Simple.Data.Database.Open().EventLotto.FindAllByUserID(dummy.ID);
                        //dummy.charm = Data_Lotto.ToList().Count;
                        dummy.charm = 0;

                        dynamic Data_SafeBox = Simple.Data.Database.Open().PlayerSafeBox.FindAllByUserID(dummy.ID).FirstOrDefault();
                        if (Data_SafeBox == null)
                        {
                            Simple.Data.Database.Open().PlayerSafeBox.Insert(UserId: dummy.ID);
                            Data_SafeBox = Simple.Data.Database.Open().PlayerSafeBox.FindAllByUserID(dummy.ID).FirstOrDefault();
                        }
                        dummy.bank_money_pay = (long)Data_SafeBox.SafeMoney2;
                        dummy.bank_money_free = (long)Data_SafeBox.SafeMoney;

                        dynamic Data_Matgo = Simple.Data.Database.Open().PlayerMatgo.FindAllByUserID(dummy.ID).FirstOrDefault();
                        if (Data_Matgo == null)
                        {
                            Simple.Data.Database.Open().PlayerMatgo.Insert(UserId: dummy.ID);
                            Data_Matgo = Simple.Data.Database.Open().PlayerMatgo.FindAllByUserID(dummy.ID).FirstOrDefault();
                        }
                        dummy.winCount = Data_Matgo.Win;
                        dummy.loseCount = Data_Matgo.Lose;

                        dynamic Data_Badugi = Simple.Data.Database.Open().PlayerBadugi.FindAllByUserID(dummy.ID).FirstOrDefault();
                        if (Data_Badugi == null)
                        {
                            Simple.Data.Database.Open().PlayerBadugi.Insert(UserId: dummy.ID);
                        }

                        //dummy.topMission = new List<CPlayerAgent.MissionData>();
                        //dynamic Data_Mission = Simple.Data.Database.Open().PlayerMatgoMission.FindAllByUserID(dummy.ID).FirstOrDefault();
                        //if (Data_Mission == null)
                        //{
                        //    NewMissionData(ref dummy.topMission);
                        //    Simple.Data.Database.Open().PlayerMatgoMission.Insert(UserId: dummy.ID, Mission1: dummy.topMission[0].type, Complete1: dummy.topMission[0].isComplete, Mission2: dummy.topMission[1].type, Complete2: dummy.topMission[1].isComplete, Mission3: dummy.topMission[2].type, Complete3: dummy.topMission[2].isComplete, Mission4: dummy.topMission[3].type, Complete4: dummy.topMission[3].isComplete, Mission5: dummy.topMission[4].type, Complete5: dummy.topMission[4].isComplete, Mission6: dummy.topMission[5].type, Complete6: dummy.topMission[5].isComplete, Mission7: dummy.topMission[6].type, Complete7: dummy.topMission[6].isComplete, Mission8: dummy.topMission[7].type, Complete8: dummy.topMission[7].isComplete, Mission9: dummy.topMission[8].type, Complete9: dummy.topMission[8].isComplete, Mission10: dummy.topMission[9].type, Complete10: dummy.topMission[9].isComplete);
                        //    Data_Mission = Simple.Data.Database.Open().PlayerMatgoMission.FindAllByUserID(dummy.ID).FirstOrDefault();
                        //}
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission1, Data_Mission.Complete1);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission2, Data_Mission.Complete2);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission3, Data_Mission.Complete3);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission4, Data_Mission.Complete4);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission5, Data_Mission.Complete5);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission6, Data_Mission.Complete6);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission7, Data_Mission.Complete7);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission8, Data_Mission.Complete8);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission9, Data_Mission.Complete9);
                        //AddMissionData(ref dummy.topMission, Data_Mission.Mission10, Data_Mission.Complete10);

                        // 로그인처리
                        Simple.Data.Database.Open().Player.UpdateById(Id: dummy.ID, LastLoginDateDate: DateTime.Now, LastIP: ip);
                        DB_User_Login(dummy.ID, ip);

                        return 1;
                    }
                    catch (Exception e)
                    {
                        form.printf("[로그인] 예외발생 {0},{1}\n", id, e.ToString());
                        DB_User_Logout(dummy.ID, 2);
                    }

                    return 0;
                });


                // 상단의 DB Task가 완료되는 시점이므로 다시 유저가 유효한 상태인지 확인합니다
                if (RemoteClients.TryGetValue(remote, out rc) == false)
                {
                    DB_User_Logout(dummy.ID, 2);
                    form.printf("[로그인] 로그인 취소 {0}\n", id);
                    return;
                }

                // 인증 성공
                if (result == 1)
                {
                    rc.data = dummy;
                    rc.joined = true;
                    //form.printf("[로그인] 로그인 성공 {0}\n", id);
                    proxy.response_Login(remote, ZNet.CPackOption.Basic, true);
                }
                else
                {
                    string ResultType;
                    if (result == 2) // 중복 로그인
                        ResultType = "이미 접속중인 아이디입니다.";
                    else if (result == 5) // 자동치기
                        ResultType = "이전 게임에서 퇴장하여 자동치기가 진행중입니다.\n잠시후 다시 시도하세요.";
                    else if (result == 6) // 차단된 IP
                        ResultType = "차단된 IP 입니다.\n고객센터에 문의해주세요.";
                    else if (result == 7) // 차단된 회원
                        ResultType = "차단된 아이디 입니다.\n고객센터에 문의해주세요.";
                    else if (result == 8) // 서버 점검중
                        ResultType = "서버 점검중 입니다.\n홈페이지의 공지사항을 확인하세요.";
                    else if (result == 9) // 접속인원 수 제한
                        ResultType = "접속 인원이 초과했습니다.\n잠시후 다시 시도해 주시기 바랍니다.";
                    else // 입력정보 오류, 접속 오류
                        ResultType = "아이디 또는 비밀번호를 다시 확인해주세요.";

                    form.printf("[로그인] 로그인 실패 {0}, {1}\n", id, result);

                    // 인증 실패를 알려줌
                    proxy.response_Login(remote, ZNet.CPackOption.Basic, false, ResultType);
                }
                return;
            });

            return true;
        }
        bool RequestLobbyList(ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            //proxy.notify_lobby_list(remote, ZNet.CPackOption.Basic, this.lobby_list);
            return true;
        }
        bool RequestGoLobby(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string lobbyname)
        {
            // 실제 해당 서버의 검증 작업
            ZNet.MasterInfo[] svr_array;
            m_Core.GetServerList((int)ServerType.Lobby, out svr_array);
            if (svr_array == null) return false;

            foreach (var obj in svr_array)
            {
                // 지정한 로비 서버 이름 확인
                if (lobbyname == obj.m_Description)
                {
                    // 이동 파라미터 구성
                    ZNet.ArrByte param_buffer;
                    Common.MoveParam param = new Common.MoveParam();
                    param.moveTo = Common.MoveParam.ParamMove.MoveToLobby;
                    param.ChannelNumber = 1; // 기본채널
                    Common.Common.ServerMoveParamWrite(param, out param_buffer);

                    // 여기서 내부패킷으로 자동적으로 서버이동이 처리 된다
                    m_Core.ServerMoveStart(remote, obj.m_Addr, param_buffer, new Guid());

                    //form.printf("MoveParam_1 {0} {1} {2}", param.moveTo, param.roomJoin, param.room_id);
                    return true;
                }
            }
            return true;
        }

        #endregion Client Request Handler

        #region Server Handler

        void ClientJoinHandler(ZNet.RemoteID remote, ZNet.NetAddress addr, ZNet.ArrByte move_server, ZNet.ArrByte move_param)
        {
            //서버이동으로 입장한 경우
            if (move_server.Count > 0)
            {
                CPlayer rc;
                Common.Common.ServerMoveComplete(move_server, out rc);
                rc.m_ip = addr.m_ip;

                //form.printf("move server complete  {0}", rc.data.userID);
                RemoteClients.TryAdd(remote, rc);
            }
            else
            {
                CPlayer rc = new CPlayer();
                rc.data.ID = 0;
                rc.data.userID = string.Empty;
                rc.m_ip = addr.m_ip;

                RemoteClients.TryAdd(remote, rc);
            }

            //입장시 로비 리스트 보냄
            proxy.notify_lobby_list(remote, ZNet.CPackOption.Basic, this.lobby_list);
            //form.printf("Client {0} is Join {1}:{2}. Current={3}\n", remote, addr.m_ip, addr.m_port, RemoteClients.Count);

        }
        void ClientLeaveHandler(ZNet.RemoteID remote, bool bMoveServer)
        {
            // 서버 이동중이 아닌상태에서 퇴장하는 경우 로그아웃에 대한 처리를 해줍니다
            if (bMoveServer == false)
            {
                CPlayer rc;
                if (RemoteClients.TryGetValue(remote, out rc))
                {
                    if (rc.data.ID != 0)
                    {
                        DB_User_Logout(rc.data.ID);
                    }
                }
            }

            CPlayer temp;
            RemoteClients.TryRemove(remote, out temp);
            //form.printf("Client {0} Leave. Current={1}\n", remote, RemoteClients.Count);

        }
        void MoveServerStart(ZNet.RemoteID remote, out ZNet.ArrByte buffer)
        {
            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc) == false)
            {
                buffer = null;
                return;
            }

            // 여기서는 이동할 서버로 동기화 시킬 유저 데이터를 구성하여 buffer에 넣어둔다 -> 완료서버에서 해당 데이터를 그대로 받게된다
            Common.Common.ServerMoveStart(rc, out buffer);

            Console.WriteLine("move server start  {0} {1}", rc.data.userID, rc.data.userID);
        }
        private void MoveServerFailed(ZNet.ArrByte move_param)
        {
            CPlayer rc;
            Common.Common.ServerMoveComplete(move_param, out rc);

            if (rc != null)
            {
                form.printf("MoveServerFailed. {0}", rc.data.userID);
                DB_User_Logout(rc.data.ID);
            }
        }
        private void CoreMessage(ResultInfo resultInfo)
        {
            switch (resultInfo.m_Level)
            {
                case IResultLevel.IMsg:
                    Log._log.Info("[CoreMsg]" + resultInfo.msg);
                    break;
                case IResultLevel.IWrn:
                    Log._log.Warn("[CoreMsg]" + resultInfo.msg);
                    break;
                case IResultLevel.IErr:
                    Log._log.Error("[CoreMsg]" + resultInfo.msg);
                    break;
                case IResultLevel.ICri:
                    Log._log.Fatal("[CoreMsg]" + resultInfo.msg);
                    break;
                default:
                    Log._log.Fatal("[CoreMsg]" + resultInfo.msg);
                    break;
            }
        }
        private void CoreException(Exception e)
        {
            Log._log.Fatal("[Exception]" + e.ToString());
        }
        void ServerJoinHandler(ZNet.RemoteID remote, ZNet.NetAddress addr)
        {
            form.printf(string.Format("서버P2P맴버 입장 remoteID {0}", remote));

            // 입장한 시점 : 이 서버의 세부 정보는 이후 refresh_handler를 통해 들어오면 처리해야한다
        }
        void ServerLeaveHandler(ZNet.RemoteID remote, ZNet.NetAddress addr)
        {
            form.printf(string.Format("서버P2P맴버 퇴장 remoteID {0}", remote));

            // 퇴장한 서버에 대한 처리
            if (lobby_list.ContainsKey(remote))
            {
                Task.Run(() =>
                {
                    try
                    {
                        Simple.Data.Database.Open().GameCurrentUser.DeleteByRoomId(RoomId: 0);
                    }
                    catch (Exception e)
                    {
                        form.printf("ServerLeave {0}\n", e.ToString());
                    }
                });
            }

            ZNet.MasterInfo temp;
            lobby_list.TryRemove(remote, out temp);
        }
        void ServerMasterJoinHandler(ZNet.RemoteID remote, ZNet.RemoteID myRemoteID)
        {
            form.printf(string.Format("마스터서버에 입장성공 remoteID {0}", myRemoteID));
        }
        void ServerMasterLeaveHandler()
        {
            form.printf(string.Format("마스터서버와 연결종료!!!"));
            // run_program = false;    // 자동 종료처리를 위해
        }
        void RefreshHandler(ZNet.MasterInfo master_info)
        {
            // 로비 리스트 정보 갱신
            ZNet.MasterInfo temp;
            lobby_list.TryRemove(master_info.m_remote, out temp);
            if ((UnityCommon.Server)master_info.m_ServerType == UnityCommon.Server.Lobby)
            {
                lobby_list.TryAdd(master_info.m_remote, master_info);
            }
            //Log._log.InfoFormat("서버P2P remote:{0} type:{1}[{2}] current:{3} addr:{4}:{5}",
            //master_info.m_remote,
            //(UnityCommon.Server)master_info.m_ServerType,
            //master_info.m_Description,
            //master_info.m_Clients,
            //master_info.m_Addr.m_ip,
            //master_info.m_Addr.m_port
            //);
        }
        void DB_User_Login(int userId, string ip)
        {
            Task.Run(() =>
            {
                try
                {
                    Simple.Data.Database.Open().GameCurrentUser.Insert(UserId: userId, Locate: 0, GameId: GameId, ChannelId: 0, RoomId: 0, IP: ip, AutoPlay: false);
                }
                catch (Exception e)
                {
                    form.printf("DB_User_Login 예외발생 {0}\n", e.ToString());
                }
            });
        }
        void DB_User_Logout(int userId, int delaySec = 0)
        {
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(1000 * delaySec);
                    Simple.Data.Database.Open().GameCurrentUser.DeleteByUserId(UserId: userId);
                }
                catch (Exception e)
                {
                    form.printf("DB_User_Logout 예외발생 {0}\n", e.ToString());
                }
            });
        }

        public override void NetLoop(object sender, ElapsedEventArgs e)
        {
            m_Core.NetLoop();
        }
        int tick = 0;
        private void ScheduleTask()
        {
        }
        public override void ServerTask(object sender, ElapsedEventArgs e_)
        {
            ++this.tick;

            if (this.tick % 13 == 0)
            {
                //DisplayStatus(m_Core);

                if (this.ShutDown)
                {
                    // 세션 없으면 프로그램 종료
                    if (RemoteClients.Count == 0)
                    {
                        Log._log.Info("서버 종료. ShutDown");
                        System.Windows.Forms.Application.Exit();
                    }
                    else
                    // 모든 세션 종료
                        if (this.CountDown < DateTime.Now)
                    {
                        if (Froce)
                            m_Core.CloseAllClient();
                        else
                            m_Core.CloseAllClientForce();
                        Froce = true;
                    }
                    form.printf("세션 종료중. 남은 세션 수:" + RemoteClients.Count);
                    Log._log.Info("세션 종료중. 남은 세션 수:" + RemoteClients.Count);
                }
            }
        }
        bool ShutDownServer(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string msg)
        {
            ServerMaintenance = true;
            ServerMsg = "서버 점검중입니다.";
            ShutDown = true;
            CountDown = DateTime.Now.AddMinutes(1);

            form.printf("서버종료 요청 받음.");
            Log._log.Warn("서버종료 요청 받음.");

            return true;
        }
        #endregion Server Handler
    }
}
