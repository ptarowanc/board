using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guid = System.Guid;
using ZNet;

namespace Rmi
{
    public class Marshaler : ZNet.Marshaler
    {
        public static void Write(CMessage msg, ArrByte value)
        {
            if (value.Count == 0) return;
            msg.Write(value);
        }
        public static void Read(CMessage msg, out ArrByte value)
        {
            value = new ArrByte();
            msg.Read(out value);
        }
        public static void Write(CMessage msg, Server.Common.MoveParam value)
        {
            Write(msg, (int)value.moveTo);
            msg.Write((int)value.roomJoin);
            msg.Write(value.room_id);
            msg.Write(value.lobby_remote);
            msg.Write(value.ChannelNumber);
            msg.Write(value.roomStake);
            msg.Write(value.roomPassword);
            msg.Write(value.RelayID);
        }
        public static void Read(CMessage msg, out Server.Common.MoveParam value)
        {
            value = new Server.Common.MoveParam();
            int moveTo;
            Read(msg, out moveTo);
            value.moveTo = (Server.Common.MoveParam.ParamMove)moveTo;
            int roomJoin;
            Read(msg, out roomJoin);
            value.roomJoin = (Server.Common.MoveParam.ParamRoom)roomJoin;
            msg.Read(out value.room_id);
            msg.Read(out value.lobby_remote);
            msg.Read(out value.ChannelNumber);
            msg.Read(out value.roomStake);
            msg.Read(out value.roomPassword);
            msg.Read(out value.RelayID);
        }

        // 룸에서 받는 유저 정보
        public class UserInfo
        {
            public string userID;       // 사용자 ID
            public string nickName;     // 닉네임
            public long money_game;     // 게임머니
            public long money_var;      // 변동머니
            public string avatar;       // 아바타
            public int voice;           // 목소리
            public int win;             // 승
            public int lose;            // 패
        }
        public static void Write(CMessage msg, UserInfo value)
        {
            Write(msg, value.userID);
            msg.Write(value.nickName);
            msg.Write(value.money_game);
            msg.Write(value.money_var);
            msg.Write(value.avatar);
            msg.Write(value.voice);
            msg.Write(value.win);
            msg.Write(value.lose);
        }
        public static void Read(CMessage msg, out UserInfo value)
        {
            value = new UserInfo();
            Read(msg, out value.userID);
            msg.Read(out value.nickName);
            msg.Read(out value.money_game);
            msg.Read(out value.money_var);
            msg.Read(out value.avatar);
            msg.Read(out value.voice);
            msg.Read(out value.win);
            msg.Read(out value.lose);
        }

        // 로비에서 받는 유저 정보
        public class LobbyUserInfo
        {
            public string nickName;     // 닉네임
            public string avatar;       // 아바타
            public long money_free;     // 무료 머니
            public long money_pay;     // 유료 머니
            public long bank_money_free;     // 금고 무료 머니
            public long bank_money_pay;     // 금고 유료 머니(미사용)
            public long cash;           // 금괴
            public int win;             // 승
            public int lose;            // 패
            public long member_point;        // 적립금
            public string shop_name;     // 매장이름 (친구목록)
        }
        public static void Write(CMessage msg, LobbyUserInfo value)
        {
            Write(msg, value.nickName);
            msg.Write(value.avatar);
            msg.Write(value.money_free);
            msg.Write(value.money_pay);
            msg.Write(value.bank_money_free);
            msg.Write(value.bank_money_pay);
            msg.Write(value.cash);
            msg.Write(value.win);
            msg.Write(value.lose);
            msg.Write(value.member_point);
            msg.Write(value.shop_name);
        }
        public static void Read(CMessage msg, out LobbyUserInfo value)
        {
            value = new LobbyUserInfo();
            Read(msg, out value.nickName);
            msg.Read(out value.avatar);
            msg.Read(out value.money_free);
            msg.Read(out value.money_pay);
            msg.Read(out value.bank_money_free);
            msg.Read(out value.bank_money_pay);
            msg.Read(out value.cash);
            msg.Read(out value.win);
            msg.Read(out value.lose);
            msg.Read(out value.member_point);
            msg.Read(out value.shop_name);
        }

        // 로비에서 받는 방 정보
        public class RoomInfo
        {
            public System.Guid roomID;         // 방 고유번호
            public int chanID;          // 채널 번호
            public int chanType;        // 채널 타입
            public bool chanFree;        // 채널 타입
            public int number;          // 방번호
            public int stakeType;       // 판돈 타입
            public int baseMoney;   // 기본 판돈 머니
            public long minMoney;   // 입장 최소 머니
            public long maxMoney;    // 입장 최대 머니
            public int userCount;       // 유저 수
            public bool restrict = false;       // 입장제한 (true 면 입장 불가)
            public bool eventRestrict = false;  // 이벤트 중이라 입장제한 (true 면 입장 불가)
            public int remote_svr;      // 서버구분용 remoteID번호 (입장할때 방이 어떤 서버에 존재하는지 정보)
            public int remote_lobby;    // 서버구분용 remoteID번호 (원래입장시점의 로비서버 - 방에서 나갈때 사용)
            public bool needPassword;   // 비밀번호방 여부
            public string roomPassword = "";	// 비밀번호
            public ConcurrentDictionary<int, string> userList = new ConcurrentDictionary<int, string>(); // 유저 목록
            public ConcurrentDictionary<int, int> userListShop = new ConcurrentDictionary<int, int>(); // 유저 목록 샵
            public bool RestrictionShop = false; // 샵 입장 제한
            public bool RestrictionUser = false; // 유저 입장 제한
            public bool RestrictionRun = false; // 먹튀 입장 제한
        }
        public static void Write(CMessage msg, RoomInfo value)
        {
            Write(msg, value.roomID);
            msg.Write(value.chanID);
            msg.Write(value.chanType);
            msg.Write(value.number);
            msg.Write(value.stakeType);
            msg.Write(value.userCount);
            msg.Write((value.restrict || value.eventRestrict));
            msg.Write(value.remote_svr);
            msg.Write(value.remote_lobby);
            msg.Write(value.needPassword);
        }
        public static void Read(CMessage msg, out RoomInfo value)
        {
            value = new RoomInfo();
            Read(msg, out value.roomID);
            msg.Read(out value.chanID);
            msg.Read(out value.chanType);
            msg.Read(out value.number);
            msg.Read(out value.stakeType);
            msg.Read(out value.userCount);
            msg.Read(out value.restrict);
            msg.Read(out value.remote_svr);
            msg.Read(out value.remote_lobby);
            msg.Read(out value.needPassword);
        }

        // 로비에서 받는 다른 유저 정보
        public class LobbyUserList
        {
            public int ID; // 유저ID
            public RemoteID RemoteID;
            public string nickName;     // 닉네임
            public long FreeMoney;      // 보유머니
            public long PayMoney;      // 보유머니2
            public int chanID;          // 채널 ID. 0: 대기실
            public int roomNumber;      // 방번호
        }
        public static void Write(CMessage msg, LobbyUserList value)
        {
            Write(msg, value.nickName);
            msg.Write(value.FreeMoney);
            msg.Write(value.PayMoney);
            msg.Write(value.chanID);
            msg.Write(value.roomNumber);
        }
        public static void Read(CMessage msg, out LobbyUserList value)
        {
            value = new LobbyUserList();
            Read(msg, out value.nickName);
            msg.Read(out value.FreeMoney);
            msg.Read(out value.PayMoney);
            msg.Read(out value.chanID);
            msg.Read(out value.roomNumber);
        }

        #region Container
        public static void Write(CMessage msg, List<string> b)
        {
            Int32 data = b.Count;
            Write(msg, data);

            foreach (var obj in b)
            {
                Write(msg, obj);
            }
        }
        public static void Read(CMessage msg, out List<string> b)
        {
            Int32 data;
            Read(msg, out data);

            b = new List<string>();

            for (Int32 i = 0; i < data; i++)
            {
                string _value;
                Read(msg, out _value);
                b.Add(_value);
            }
        }
        public static void Write(CMessage msg, List<int> b)
        {
            Int32 data = b.Count;
            Write(msg, data);

            foreach (var obj in b)
            {
                Write(msg, obj);
            }
        }
        public static void Read(CMessage msg, out List<int> b)
        {
            Int32 data;
            Read(msg, out data);

            b = new List<int>();

            for (Int32 i = 0; i < data; i++)
            {
                int _value;
                Read(msg, out _value);
                b.Add(_value);
            }
        }
        public static void Write(CMessage msg, List<LobbyUserInfo> b)
        {
            Int32 data = b.Count;
            Write(msg, data);

            foreach (var obj in b)
            {
                Write(msg, obj);
            }
        }
        public static void Read(CMessage msg, out List<LobbyUserInfo> b)
        {
            Int32 data;
            Read(msg, out data);

            b = new List<LobbyUserInfo>();

            for (Int32 i = 0; i < data; i++)
            {
                LobbyUserInfo _value;
                Read(msg, out _value);
                b.Add(_value);
            }
        }
        public static void Write(CMessage msg, List<RoomInfo> b)
        {
            Int32 data = b.Count;
            Write(msg, data);

            foreach (var obj in b)
            {
                Write(msg, obj);
            }
        }
        public static void Read(CMessage msg, out List<RoomInfo> b)
        {
            Int32 data;
            Read(msg, out data);

            b = new List<RoomInfo>();

            for (Int32 i = 0; i < data; i++)
            {
                RoomInfo _value;
                Read(msg, out _value);
                b.Add(_value);
            }
        }
        public static void Write(CMessage msg, List<LobbyUserList> b)
        {
            Int32 data = b.Count;
            Write(msg, data);

            foreach (var obj in b)
            {
                Write(msg, obj);
            }
        }
        public static void Read(CMessage msg, out List<LobbyUserList> b)
        {
            Int32 data;
            Read(msg, out data);

            b = new List<LobbyUserList>();

            for (Int32 i = 0; i < data; i++)
            {
                LobbyUserList _value;
                Read(msg, out _value);
                b.Add(_value);
            }
        }
        public static void Write(CMessage msg, Dictionary<RemoteID, string> b)
        {
            Int32 data = b.Count;
            Write(msg, data);

            foreach (KeyValuePair<RemoteID, string> obj in b)
            {
                Write(msg, obj.Key);
                Write(msg, obj.Value);
            }
        }
        public static void Read(CMessage msg, out Dictionary<RemoteID, string> b)
        {
            Int32 data;
            Read(msg, out data);

            b = new Dictionary<RemoteID, string>();

            for (Int32 i = 0; i < data; i++)
            {
                RemoteID _key;
                string _value;
                Read(msg, out _key);
                Read(msg, out _value);
                b.Add(_key, _value);
            }
        }
        public static void Write(CMessage msg, ConcurrentDictionary<RemoteID, string> b)
        {
            Int32 data = b.Count;
            Write(msg, data);

            foreach (KeyValuePair<RemoteID, string> obj in b)
            {
                Write(msg, obj.Key);
                Write(msg, obj.Value);
            }
        }
        public static void Read(CMessage msg, out ConcurrentDictionary<RemoteID, string> b)
        {
            Int32 data;
            Read(msg, out data);

            b = new ConcurrentDictionary<RemoteID, string>();

            for (Int32 i = 0; i < data; i++)
            {
                RemoteID _key;
                string _value;
                Read(msg, out _key);
                Read(msg, out _value);
                b.TryAdd(_key, _value);
            }
        }
        #endregion
    }
}
