using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Guid = System.Guid;
using ZNet;

namespace Server.Engine
{
    // 유저 정보 (DB)
    public struct UserData
    {
        public int ID;
        public string userID;
        public string nickName;
        public string avatar; // 캐릭터 아바타
        public int cash;
        public long money_pay;
        public long money_free;
        public long bank_money_pay;
        public long bank_money_free;
        public int winCount;
        public int loseCount;
        public int mileage;
        public int charm;
        public int voice; // 캐릭터 목소리
        public string avatar_card; // 카드 스킨
        public long member_point;
        public string shop_name;
        public int shopId;
        public bool IPFree; // IP 제한 (동일 IP 방 입장 제한)
        public bool ShopFree; // 매장 제한 (동일 매장 ID 입장 제한)
        public int RelayID;
        public bool Old;
        public int Option1;
        public int Option2;
        public bool Restrict;

        public UserData(UserData d)
        {
            ID = d.ID;
            userID = d.userID;
            nickName = d.nickName;
            avatar = d.avatar;
            cash = d.cash;
            money_pay = d.money_pay;
            money_free = d.money_free;
            bank_money_pay = d.bank_money_pay;
            bank_money_free = d.bank_money_free;
            winCount = d.winCount;
            loseCount = d.loseCount;
            mileage = d.mileage;
            charm = d.charm;
            voice = d.voice;
            avatar_card = d.avatar_card;
            member_point = d.member_point;
            shop_name = d.shop_name;
            shopId = d.shopId;
            IPFree = d.IPFree;
            ShopFree = d.ShopFree;
            RelayID = d.RelayID;
            Old = d.Old;
            Option1 = d.Option1;
            Option2 = d.Option2;
            Restrict = d.Restrict;
        }
    }
    public struct History
    {
        public int from; // 0:client, 1:server
        public PacketType id;
        public DateTime time;

        public History(int _from, PacketType _id)
        {
            from = _from;
            id = _id;
            time = DateTime.Now;
        }
    }
    public class CPlayer
    {
        //public object PlayerLock = new object();

        public UserStatus status = UserStatus.None;
        public UserData data;

        public Guid roomID; // 룸서버에 있을경우 방ID
        public int channelNumber; // 로비서버에 있을경우 채널번호
        public KeyValuePair<RemoteID, RemoteID> Remote;
        public byte player_index;
        public CPlayerAgent agent;

        public string m_ip;
        public ushort m_Port;
        public DateTime roomTime; // 룸에 입장한 시간

        public List<string> friendList; // 친구목록
        public long ChangeMoney;
        public int GameResult;

        // 방장 여부
        public bool Operator;

        // 딜비
        public long GameDealMoney;
        public long JackPotDealMoney;

        // 회원 레벨
        public int UserLevel;
        public int GameLevel;
        public int GameLevel2;

        // 일일 손익 머니
        public long DayChangeMoney;

        // 추방 경고
        public bool KickCount;

        public MessageTemp currentMsg;   // 자동처리할때 넘길 현재 패킷
        public bool isActionExecute;   // 게임 행동의 처리 여부
        public DateTime actionTimeLimit; // 네트워크 지연 시 AI로 전환시킬 제한시간
        public ConcurrentQueue<MessageTemp> QueueMsg; // 처리할 목록

        public DateTime RoomRequestTime;  // 방이동, 나가기 요청 시간 조정

        public int RelayID;

        //public List<History> history = new List<History>();

        // 로딩중 
        public bool PacketReady;

        // 행운의 복권 이벤트 참여 횟수
        public int EventLuckyLottoCount;

        public bool DB;

        public CPlayer()
        {
            this.status = UserStatus.None;
            channelNumber = 0; // 로비서버에 있을경우 채널번호

            roomTime = DateTime.Now; // 룸에 입장한 시간

            friendList = new List<string>(); // 친구목록
            ChangeMoney = 0;
            GameResult = 0;

            Operator = false;

            GameDealMoney = 0;
            JackPotDealMoney = 0;

            UserLevel = 0;
            GameLevel = 0;

            DayChangeMoney = 0;

            KickCount = false;

            currentMsg = null;
            isActionExecute = false;
            actionTimeLimit = DateTime.Now.AddSeconds(25);

            RoomRequestTime = DateTime.Now;

            PacketReady = false;

            DB = false;
        }

        public void Reset()
        {
            this.roomID = Guid.Empty;
            this.roomTime = DateTime.Now;
            this.isActionExecute = false;
            this.currentMsg = null;
            this.status = UserStatus.None;

            this.actionTimeLimit = DateTime.Now.AddSeconds(25);
            this.RoomRequestTime = DateTime.Now;
            this.KickCount = false;
            this.GameDealMoney = 0;
            this.JackPotDealMoney = 0;
            this.ChangeMoney = 0;
            this.GameResult = 0;
        }
    }

    public enum UserStatus : byte
    {
        None,           // 로딩중, 방에서 관전중
        RoomStay,       // 방에서 대기중
        RoomReady,      // 방에서 준비중
        RoomPlay,       // 방에서 게임중
        RoomPlayOut,    // 방에서 게임중 접속종료
        RoomOut     // 연결종료중
    }
}
