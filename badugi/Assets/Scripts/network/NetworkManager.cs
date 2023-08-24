using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rmi;
using CommonBadugi;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Events;
using Guid = System.Guid;
using System.Reflection;
using System.Linq;
using ZNet;

public interface IServerMessage
{
    void OnRecv();
}
public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

#if UNITY_ANDROID
    // 모바일버전에만 사용
    public enum LOBBYLOCATION { LOBBY, SHOP, MYROOM };
    LOBBYLOCATION LobbyLocation = LOBBYLOCATION.LOBBY;
#endif

    internal Guid ProtocolVersion = new Guid("F637FD3E-D2BC-42DC-82CD-C8E4DFD710BD");

    public CoreClientNet NetGame;
    internal SS.Proxy client;
    internal SS.Stub server;

    List<string> received_texts;
    public ServerType server_now = ServerType.Login;  // 현재 서버 위치
    public ServerType server_tag = ServerType.Login;  // 목표 서버 위치

    Queue<CRecvedMsg> waiting_packets;
    public Queue<CRecvedMsg> GameWaitingPackets;
    List<string> lobbys;
    public int Channel = 0;
    public int JoinOption = 1;
    public int MakeOption = 1;

    public string IdentifierID = "";
    public string IdentifierKey = "";
    public StartType mStartType = StartType.Free;

    public GameObject PopupMessage;
    public GameObject Notice;

    public int PopupLevel = 0;

    public void PopupLevelUp() { if (PopupLevel < 2) PopupLevel++; }
    public void PopupLevelDown() { if (PopupLevel > 0) PopupLevel--; }

    public CPackOption pkOption = CPackOption.Basic;

#if UNITY_ANDROID
    public void SetLobbyLocation(LOBBYLOCATION location) { LobbyLocation = location; }
    public LOBBYLOCATION GetLobbyLocation() { return LobbyLocation; }
    public Rmi.Marshaler.LobbyUserInfo UserInfo;
    public long lobbyJackpotMoney;
#endif

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

#if !UNITY_ANDROID
        string[] arguments = Environment.GetCommandLineArgs();
        try
        {
            IdentifierID = arguments[1];
            IdentifierKey = arguments[2];
            int tempStartType;
            if (int.TryParse(arguments[3], out tempStartType))
            {
                mStartType = (StartType)tempStartType;
            }
            else
            {
                mStartType = StartType.Paid;
            }

            foreach (var a in arguments) Debug.LogError(a);
        }
        catch (Exception)
        {
            mStartType = StartType.Free;
        }
#endif

        if (mStartType == StartType.Paid)
        {
            Channel = 1;
        }
        else
        {
            Channel = 5;
        }

        DontDestroyOnLoad(gameObject);

        received_texts = new List<string>();
        waiting_packets = new Queue<CRecvedMsg>();
        GameWaitingPackets = new Queue<CRecvedMsg>();
    }

    void MakeCore(out CoreClientNet core, out SS.Proxy csproxy, out SS.Stub scstub)
    {
        core = new CoreClientNet();
        csproxy = new SS.Proxy();
        scstub = new SS.Stub();
        core.AttachProxy(csproxy);
        core.AttachStub(scstub);
        StartCoroutine(sequential_packet_handler());

        // Core Handler
        {
            core.move_fail_handler = ServerMoveFail;
            core.server_join_handler = ServerJoin;
            core.server_connect_result_handler = ServerConnect;
            core.server_leave_handler = ServerLeave;
            core.message_handler = ServerMessage;

            //core.ServerOnlineHandler = (RemoteOnlineEventArgs args) => { Debug.LogFormat("Core Handler. {0}", MethodBase.GetCurrentMethod().Name); };
            //core.ServerOfflineHandler = (RemoteOfflineEventArgs args) => { Debug.LogFormat("Core Handler. {0}", MethodBase.GetCurrentMethod().Name); };
            //core.SynchronizeServerTimeHandler = () => { /*printf("Core Handler. {0}", MethodBase.GetCurrentMethod().Name);*/ };
            //core.ChangeServerUdpStateHandler = (ErrorType reason) => { /*printf("Core Handler. {0}", MethodBase.GetCurrentMethod().Name);*/ };
            //core.ChangeP2PRelayStateHandler = (HostID remoteHostID, ErrorType reason) => { Debug.LogFormat("Core Handler. {0}", MethodBase.GetCurrentMethod().Name); };
            //core.P2PMemberLeaveHandler = (HostID memberHostID, HostID groupHostID, int memberCount) => { Debug.LogFormat("Core Handler. {0}", MethodBase.GetCurrentMethod().Name); };
            //core.P2PMemberJoinHandler = (HostID memberHostID, HostID groupHostID, int memberCount, ByteArray customField) => { Debug.LogFormat("Core Handler. {0}", MethodBase.GetCurrentMethod().Name); };
            //core.LeaveServerHandler = LeaveServerHandler; //★
            //core.JoinServerCompleteHandler = JoinServerCompleteHandler; //★
            //core.ReceivedUserMessageHandler = (HostID sender, RmiContext rmiContext, ByteArray payload) => { Debug.LogFormat("Core Handler. {0}", MethodBase.GetCurrentMethod().Name); };
            //core.NoRmiProcessedHandler = (RmiID rmiID) => { Debug.LogFormat("Core Handler. {0}", MethodBase.GetCurrentMethod().Name); Debug.LogFormat("rmiID:{0}", rmiID); };
            //core.ExceptionHandler = (HostID remoteID, Exception e) => { Debug.LogFormat("Core Handler. {0}", MethodBase.GetCurrentMethod().Name); Debug.LogFormat("e:{0}", e.ToString()); };
            //core.ErrorHandler = (ErrorInfo errorInfo) => { Debug.LogFormat("Core Handler. {0}", MethodBase.GetCurrentMethod().Name); };
            //core.WarningHandler = (ErrorInfo errorInfo) => { Debug.LogFormat("Core Handler. {0}", MethodBase.GetCurrentMethod().Name); };
            //core.InformationHandler = (ErrorInfo errorInfo) => { Debug.LogFormat("Core Handler. {0}", MethodBase.GetCurrentMethod().Name); };
            //core.P2PMemberOfflineHandler = (RemoteOfflineEventArgs args) => { Debug.LogFormat("Core Handler. {0}", MethodBase.GetCurrentMethod().Name); };
            //core.P2PMemberOnlineHandler = (RemoteOnlineEventArgs args) => { Debug.LogFormat("Core Handler. {0}", MethodBase.GetCurrentMethod().Name); };
        }

        // Server Handler
        {
            //// 서버이동
            //scstub.ServerMoveStart = ServerMoveStart;
            //scstub.ServerMoveEnd = ServerMoveEnd;

            //// 로비 서버
            //scstub.ResponseLobbyKey = ResponseLobbyKey; // 패킷큐 X

            //scstub.NotifyUserInfo = NotifyUserInfo;
            //scstub.NotifyUserList = NotifyUserList;
            //scstub.NotifyRoomList = NotifyRoomList;
            //scstub.ResponseChannelMove = ResponseChannelMove;
            //scstub.ResponseLobbyMessage = ResponseLobbyMessage;
            //scstub.ResponseBank = ResponseBank;
            //scstub.NotifyJackpotInfo = NotifyJackpotInfo;
            //scstub.NotifyLobbyMessage = NotifyLobbyMessage;
            //// 모바일
            //scstub.ResponsePurchaseList = ResponsePurchaseList;
            //scstub.ResponsePurchaseAvailability = ResponsePurchaseAvailability;
            //scstub.ResponsePurchaseReceiptCheck = ResponsePurchaseReceiptCheck;
            //scstub.ResponsePurchaseResult = ResponsePurchaseResult;
            //scstub.ResponsePurchaseCash = ResponsePurchaseCash;
            //scstub.ResponseMyroomList = ResponseMyroomList;
            //scstub.ResponseMyroomAction = ResponseMyroomAction;

            //// 게임서버
            //scstub.GameResponseRoomOutRsvp = GameResponseRoomOutRsvp;
            //scstub.GameResponseRoomOut = GameResponseRoomOut;
            //scstub.GameResponseRoomMove = GameResponseRoomMove;
            //scstub.GameRoomIn = GameRoomIn;
            //scstub.GameRoomReady = GameRoomReady;
            //scstub.GameStart = GameStart;
            //scstub.GameDealCards = GameDealCards;
            //scstub.GameUserIn = GameUserIn;
            //scstub.GameSetBoss = GameSetBoss;
            //scstub.GameNotifyStat = GameNotifyStat;
            //scstub.GameRoundStart = GameRoundStart;
            //scstub.GameChangeTurn = GameChangeTurn;
            //scstub.GameRequestBet = GameRequestBet;
            //scstub.GameResponseBet = GameResponseBet;
            //scstub.GameChangeRound = GameChangeRound;
            //scstub.GameRequestChangeCard = GameRequestChangeCard;
            //scstub.GameResponseChangeCard = GameResponseChangeCard;
            //scstub.GameCardOpen = GameCardOpen;
            //scstub.GameOver = GameOver;

            //scstub.GameRoomInfo = GameRoomInfo;
            //scstub.GameKickUser = GameKickUser;
            //scstub.GameEventInfo = GameEventInfo;
            //scstub.GameUserInfo = GameUserInfo;
            //scstub.GameUserOut = GameUserOut;
            //scstub.GameEventStart = GameEventStart;
            //scstub.GameEvent2Start = GameEvent2Start;
            //scstub.GameEventRefresh = GameEventRefresh;
            //scstub.GameEventEnd = GameEventEnd;
            //scstub.GameMileageRefresh = GameMileageRefresh;
            //scstub.GameEventNotify = GameEventNotify;
            //scstub.GameCurrentInfo = GameCurrentInfo;
            //scstub.GameEntrySpectator = GameEntrySpectator;
            //scstub.GameNotifyMessage = GameNotifyMessage;
        }
    }

    void Start()
    {
        MakeCore(out this.NetGame, out this.client, out this.server);

        // Connection
        {
            NetGame.Connect(
#if TEST
            "192.168.0.15",
#else
            "xzygames.com",
#endif
            21010/*tcp port*/,
                uint.MaxValue,/*protocol version*/
                0/*udp disable=0*/,
                true/*mobile*/,
                false/*RecoveryUse*/
            );
        }

#if UNITY_ANDROID
        SceneManager.sceneLoaded += (Scene arg0, LoadSceneMode arg1) =>
        {
            Canvas cvs = GetComponent<Canvas>();
            if (cvs != null)
            {
                cvs.worldCamera = Camera.main;
            }
        };
#endif

    }

    #region Unity Handler
    private void OnLevelWasLoaded(int level)
    {
        if (level == 3) // 로비
        {
            LobbyIn();
        }
        else if (level == 4) // 스토어
        {
            if (MyInfoManager.Instance != null && UserInfo != null)
            {
                MyInfoManager.Instance.ResetMyInfo(UserInfo);
                LobbyJackpotManager.Instance.JackpotMoney = lobbyJackpotMoney;
            }
        }
        else if (level == 5) // 마이룸
        {
            if (MyInfoManager.Instance != null && UserInfo != null)
            {
                MyInfoManager.Instance.ResetMyInfo(UserInfo);
                LobbyJackpotManager.Instance.JackpotMoney = lobbyJackpotMoney;
            }
        }
    }
    void OnApplicationQuit()
    {
        //NetSession.Disconnect();
        //NetGame.Disconnect();
        if (NetGame != null)
        {
            //NetGame.ForceLeave();
            NetGame.Destroy();
        }
    }
    void Update()
    {
        NetGame.NetLoop();
    }
    #endregion

    #region Client Network
    public void ReceiveMsg(CRecvedMsg msg)
    {
        CRecvedMsg clone = new CRecvedMsg();
        clone.remote = msg.remote;
        clone.pkop = msg.pkop;
        clone.msg = msg.msg;
        clone.pkID = msg.pkID;
        if (server_now == ServerType.Room)
        {
            //PlayGameUI.Instance.waiting_packets.Enqueue(clone);
            StartCoroutine(onReceive(clone));
        }
        else
        {
            this.waiting_packets.Enqueue(clone);
        }
    }
    IEnumerator onReceive(CRecvedMsg clone)
    {
        while (true)
        {
            if (PlayGameUI.Instance == null) yield return null;
            else break;
        }
        PlayGameUI.Instance.OnReceive(clone);
    }
    IEnumerator sequential_packet_handler()
    {
        while (true)
        {
            // 로비에 있을때만 처리
            if (this.waiting_packets.Count <= 0 || (Application.loadedLevel < 2 || Application.loadedLevel > 5))
            {
                yield return null;
                continue;
            }

            var msg = this.waiting_packets.Dequeue();
            RemoteID remote = msg.remote;
            CPackOption pkOption = msg.pkop;
            CMessage __msg = msg.msg;
            PacketType packetType = msg.pkID;

            switch (packetType)
            {
                // 로그인 패킷 목록
                case SS.Common.ResponseLogin:
                    {
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        string resultMessage; Rmi.Marshaler.Read(__msg, out resultMessage);

                        bool bRet = ResponseLogin(remote, pkOption, result, resultMessage);
                    }
                    break;
                case SS.Common.NotifyLobbyList:
                    {
                        System.Collections.Generic.List<string> lobbyList; Rmi.Marshaler.Read(__msg, out lobbyList);

                        bool bRet = NotifyLobbyList(remote, pkOption, lobbyList);
                    }
                    break;
                case SS.Common.ResponseLoginKey:
                    {
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        string resultMessage; Rmi.Marshaler.Read(__msg, out resultMessage);

                        bool bRet = ResponseLoginKey(remote, pkOption, result, resultMessage);
                    }
                    break;
                case SS.Common.ResponseGameOptions:
                    {
                        bool bRet = ResponseGameOptions(remote, pkOption, __msg);
                    }
                    break;
                // 로비 패킷 목록
                case SS.Common.NotifyUserInfo:
                    {
                        Rmi.Marshaler.LobbyUserInfo info = new Rmi.Marshaler.LobbyUserInfo();
                        Rmi.Marshaler.Read(__msg, out info);
                        this.UserInfo = info;
                        MyInfoManager.Instance.ResetMyInfo(info);
                    }
                    break;
                case SS.Common.NotifyUserList:
                    {
                        //List<Rmi.Marshaler.LobbyUserList> infos = new List<Rmi.Marshaler.LobbyUserList>();
                        //int count;
                        //Rmi.Marshaler.Read(__msg, out count);
                        //for (int i = 0; i < count; i++)
                        //{
                        //    Rmi.Marshaler.LobbyUserList info;
                        //    Rmi.Marshaler.Read(__msg, out info);
                        //    infos.Add(info);
                        //}

                        //UserListManager.Instance.SetData(infos);
                    }
                    break;
                case SS.Common.NotifyRoomList:
                    {
#if UNITY_ANDROID
                        if (LobbyLocation == LOBBYLOCATION.LOBBY)
#endif
                        {
                            List<Rmi.Marshaler.RoomInfo> room_list;
                            Rmi.Marshaler.Read(__msg, out Channel);
                            Rmi.Marshaler.Read(__msg, out room_list);
                            ChannerManager.Instance.SetData(room_list, Channel);
                        }
                    }
                    break;
                case SS.Common.ResponseChannelMove:
                    {
                        // not use
                    }
                    break;
                case SS.Common.ResponseLobbyMessage:
                    {
#if UNITY_ANDROID
                        if (LobbyLocation == LOBBYLOCATION.LOBBY)
#endif
                        {
                            // 로비 메세지
                            string errCode;
                            Rmi.Marshaler.Read(__msg, out errCode);

                            PopupLevelUp();
                            PopupMessage.SetActive(true);
                            PopupMessage.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = errCode;

                            LobbyPopup.Instance.isJoinRoom = false;
                            LobbyPopup.Instance.isMakeRoom = false;
                        }
                    }
                    break;
                case SS.Common.ResponseBank:
                    {
#if UNITY_ANDROID
                        if (LobbyLocation == LOBBYLOCATION.LOBBY)
#endif
                        {
                            bool isSuccess;
                            Rmi.Marshaler.Read(__msg, out isSuccess);
                            int type;
                            Rmi.Marshaler.Read(__msg, out type);

                            if (!isSuccess) Debug.LogError("금고실패 code : " + type);
                            else LobbyPopup.Instance.HideBank();
                        }
                    }
                    break;
                case SS.Common.NotifyJackpotInfo:
                    {
                        long jackpotMoney;
                        Rmi.Marshaler.Read(__msg, out jackpotMoney);
                        //LobbyJackpotManager.Instance.JackpotMoney = jackpotMoney;
                        NetworkManager.Instance.lobbyJackpotMoney = jackpotMoney;
                    }
                    break;
                case SS.Common.NotifyLobbyMessage:
                    {
                        int messageType; Rmi.Marshaler.Read(__msg, out messageType); // 공지 타입 (기본값:0)
                        string message; Rmi.Marshaler.Read(__msg, out message); // 공지 문자열
                        int sec; Rmi.Marshaler.Read(__msg, out sec); // 시간
                        StartCoroutine(NoticeRefresh(message, sec));
                    }
                    break;
                // 로비 패킷 목록 끝

                // 모바일 패킷
                case SS.Common.ResponsePurchaseList:
                    {
                        //string json; // 상점 목록 json 데이터
                        //Rmi.Marshaler.Read(__msg, out json);
                        //if(CBStoreList != null)
                        //{
                        //    CBStoreList.Invoke(json);
                        //    CBStoreList.RemoveAllListeners();
                        //}
                        string PurchaseList = ""; // 상점 목록 json 데이터
                        int count = 0;
                        string json = "";

                        PurchaseList += "{ \"avatar\":[";
                        Rmi.Marshaler.Read(__msg, out count);
                        for (int i = 0; i < count; i++)
                        {
                            Rmi.Marshaler.Read(__msg, out json);
                            if (i == 0)
                                PurchaseList += json;
                            else
                                PurchaseList += "," + json;
                        }
                        PurchaseList += "], \"card\":[";
                        Rmi.Marshaler.Read(__msg, out count);
                        for (int i = 0; i < count; i++)
                        {
                            Rmi.Marshaler.Read(__msg, out json);
                            if (i == 0)
                                PurchaseList += json;
                            else
                                PurchaseList += "," + json;
                        }
                        PurchaseList += "], \"evt\":[";
                        Rmi.Marshaler.Read(__msg, out count);
                        for (int i = 0; i < count; i++)
                        {
                            Rmi.Marshaler.Read(__msg, out json);
                            if (i == 0)
                                PurchaseList += json;
                            else
                                PurchaseList += "," + json;
                        }
                        PurchaseList += "], \"charge\":[";
                        Rmi.Marshaler.Read(__msg, out count);
                        for (int i = 0; i < count; i++)
                        {
                            Rmi.Marshaler.Read(__msg, out json);
                            if (i == 0)
                                PurchaseList += json;
                            else
                                PurchaseList += "," + json;
                        }
                        PurchaseList += "]}";


                        if (CBStoreList != null)
                        {
                            CBStoreList.Invoke(PurchaseList);
                            CBStoreList.RemoveAllListeners();
                        }
                    }
                    break;
                case SS.Common.ResponsePurchaseAvailability:
                    {
                        bool available; // 구매 가능 여부
                        string reason; // 구매 불가 시, 안내 메시지
                        Rmi.Marshaler.Read(__msg, out available);
                        Rmi.Marshaler.Read(__msg, out reason);
                        if (CBCheckEnablePurchase != null)
                        {
                            CBCheckEnablePurchase.Invoke(available, reason);
                            CBCheckEnablePurchase.RemoveAllListeners();
                        }
                    }
                    break;
                case SS.Common.ResponsePurchaseReceiptCheck:
                    {
                        bool result; // 영수증 검증 성공 여부 
                        Guid token; // 게임 서버에서 발급하는 결제 토큰 (검증 실패 시 Guid.Empty)
                        Rmi.Marshaler.Read(__msg, out result);
                        Rmi.Marshaler.Read(__msg, out token);
                        if (CBCheckPurchaseReceipt != null)
                        {
                            CBCheckPurchaseReceipt.Invoke(result, token);
                            CBCheckPurchaseReceipt.RemoveAllListeners();
                        }
                    }
                    break;
                case SS.Common.ResponsePurchaseResult:
                    {
                        bool result; // 구매 성공 여부
                        string reason; // 구매 실패 시, 안내 메시지
                        Rmi.Marshaler.Read(__msg, out result);
                        Rmi.Marshaler.Read(__msg, out reason);
                        if (CBPurchaseResult != null)
                        {
                            CBPurchaseResult.Invoke(result, reason);
                            CBPurchaseResult.RemoveAllListeners();
                        }
                    }
                    break;
                case SS.Common.ResponsePurchaseCash:
                    {
                        bool result; // 구매 성공 여부
                        string reason; // 구매 실패 시, 안내 메시지
                        Rmi.Marshaler.Read(__msg, out result);
                        Rmi.Marshaler.Read(__msg, out reason);

                        if (CBPurchaseCash != null)
                        {
                            CBPurchaseCash.Invoke(result, reason);
                            CBPurchaseCash.RemoveAllListeners();
                        }
                    }
                    break;
                case SS.Common.ResponseMyroomList:
                    {
                        string json; // 마이룸 목록 json 데이터
                                     // expire : 아이템 사용 기한
                                     // count : 아이템 갯수
                        Rmi.Marshaler.Read(__msg, out json);
                        if (CBMyRoomList != null)
                        {
                            CBMyRoomList.Invoke(json);
                            CBMyRoomList.RemoveAllListeners();
                        }
                    }
                    break;
                case SS.Common.ResponseMyroomAction:
                    {
                        string pid; // 사용한 상품 Id
                        bool result; // 사용 결과
                        string reason; // 사용 불가 시, 안내 메시지

                        Rmi.Marshaler.Read(__msg, out pid);
                        Rmi.Marshaler.Read(__msg, out result);
                        Rmi.Marshaler.Read(__msg, out reason);

                        if (CBMyRoomAction != null)
                        {
                            CBMyRoomAction.Invoke(pid, result, reason);
                            CBMyRoomAction.RemoveAllListeners();
                        }
                    }
                    break;
            }

            yield return null;
        }
    }
    IEnumerator NoticeRefresh(string message, int sec)
    {
        Notice.SetActive(true);

        Notice.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = message;

        yield return new WaitForSeconds(sec);

        Notice.SetActive(false);
    }
    public void LoginAccount(string userid, string password)
    {
        if (server_now == ServerType.Login)
        {
            // 최초 로그인 DB인증 시도 요청
            client.RequestLogin(RemoteID.Remote_Server, CPackOption.Encrypt, userid, password);
        }
    }
    public void MakeRoom(string pw = "")
    {
        if (server_now == ServerType.Lobby)
        {
            server_tag = ServerType.Room;
            client.RequestRoomMake(RemoteID.Remote_Server, CPackOption.Encrypt, ChannerManager.Instance.NowChannel, MakeOption, pw);
        }
    }
    public void JoinRoom2(int chanID, int roomNumber, string pw)
    {
        if (server_now == ServerType.Lobby)
        {
            server_tag = ServerType.Room;
            client.RequestRoomJoinSelect(RemoteID.Remote_Server, CPackOption.Encrypt, chanID, roomNumber, pw);
        }
    }
    public void JoinRoom(int option)
    {
        if (server_now == ServerType.Lobby)
        {
            server_tag = ServerType.Room;
            client.RequestRoomJoin(RemoteID.Remote_Server, CPackOption.Basic, ChannerManager.Instance.NowChannel, option);
        }
    }
    public void RoomOutRsvn(bool isRsvp)
    {
        if (server_now == CommonBadugi.ServerType.Room)
        {
            client.GameRoomOutRsvn(RemoteID.Remote_Server, CPackOption.Basic, isRsvp);
        }
    }
    public void RoomOut()
    {
        if (server_now == ServerType.Room)
        {
            server_tag = ServerType.Lobby;
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameRoomOut;
            Msg.WriteStart(msgID, pkOption, 0, true);
            client.PacketSend(RemoteID.Remote_Server, pkOption, Msg);
        }
    }
    public void RoomMove()
    {
        if (server_now == ServerType.Room)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameRoomMove;
            Msg.WriteStart(msgID, pkOption, 0, true);
            client.PacketSend(RemoteID.Remote_Server, pkOption, Msg);
        }
    }
    public void Bank(int option, long money, string password)
    {
        if (server_now == ServerType.Lobby)
        {
            server_tag = ServerType.Lobby;
            client.RequestBank(RemoteID.Remote_Server, CPackOption.Encrypt, option, money, password);
        }
    }
    public void LobbyIn()
    {
        if (server_now == ServerType.Lobby)
        {
            ChannerManager.Instance.NowChannel = Channel;
            client.RequestChannelMove(RemoteID.Remote_Server, CPackOption.Basic, ChannerManager.Instance.NowChannel);
            client.RequestJoinInfo(RemoteID.Remote_Server, CPackOption.Basic);
        }
    }
    public void RoomIn()
    {
        if (server_now == ServerType.Room)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameRoomInUser;
            Msg.WriteStart(msgID, pkOption, 0, true);
            client.PacketSend(RemoteID.Remote_Server, pkOption, Msg);
        }
    }
    public void request_channel_move(int NowChannel)
    {
        if (server_now == ServerType.Lobby)
        {
            client.RequestChannelMove(RemoteID.Remote_Server, CPackOption.Basic, NowChannel);
        }
    }
    public void printf(string txt, params object[] args)
    {
        Debug.Log(string.Format(txt, args));
    }
    public void PopupServerDisconnect(string text = "※안내※\n서버와 연결이 끊어졌습니다.")
    {
        PopupLevelUp();
        PopupMessage.SetActive(true);
        PopupMessage.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = text;
    }
    #endregion

    #region Core Handler
    void ServerMoveFail()
    {
        PopupLevelUp();
        PopupMessage.SetActive(true);
        PopupMessage.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "잠시후 다시 시도해주십시오.";

        if (LobbyPopup.Instance != null)
        {
            LobbyPopup.Instance.isJoinRoom = false;
            LobbyPopup.Instance.isMakeRoom = false;
        }

        if (NetGame != null && NetGame.IsNetworkConnect())
        {
            NetGame.ForceLeave();
            PopupServerDisconnect();
        }
    }
    void ServerJoin(ZNet.ConnectionInfo info)
    {
        if (info.moved)
        {
            // 서버이동이 성공한 시점 : 위치를 목표했던 서버로 설정
            server_now = server_tag;
            printf("서버이동성공 [{0}:{1}] {2}", info.addr.m_ip, info.addr.m_port, server_now);
            if (server_now == ServerType.Room)
            {
                SceneManager.LoadScene("M_Game");
            }
            else if (server_now == ServerType.Lobby)
            {
                SceneManager.LoadScene("M_Lobby");
            }
        }
        else
        {
            // 최초 입장의 성공시점 : 위치를 로그인 서버로 설정
            server_now = ServerType.Login;
            printf("서버입장성공 {0}", server_now);

            // 서버 연결 성공후 보내기
            if (IdentifierID != "" && IdentifierKey != "")
            {
                //printf("런처 -> 로그인 [{0}:{1}]", IdentifierID, IdentifierKey);
                client.RequestLoginKey(ZNet.RemoteID.Remote_Server, ZNet.CPackOption.Basic, IdentifierID, IdentifierKey);
            }
            else
            {
                // 서버 연결 실패★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★
                //PopupServerDisconnect();
            }
        }
    }
    void ServerConnect(bool isConnectSuccess)
    {
        if (isConnectSuccess)
        {
        }
        else
        {
            // 서버 연결 실패
            PopupServerDisconnect();
        }
    }
    void ServerLeave(ZNet.ConnectionInfo info)
    {
        if (info.moved)
        {
            printf("서버이동을 위해 퇴장, 이동할서버 [{0}:{1}]", info.addr.m_ip, info.addr.m_port);
        }
        else
        {
            PopupServerDisconnect();
        }

        server_now = ServerType.Lobby;
    }
    void ServerMessage(ZNet.ResultInfo result)
    {
        string str_msg = "Msg : ";
        str_msg += result.msg;
        printf(str_msg);
    }

    #endregion

    #region Login Server Handler
    bool ResponseLogin(RemoteID remote, CPackOption rmiContext, bool result, string resultMessage)
    {
        if (result)
        {
            client.RequestLobbyList(remote, CPackOption.Basic);
            Unitycoding.EventHandler.Execute("OnLogin");
            if (server_now == ServerType.Login)
            {
                // 일단 간단한 처리를 위해 첫번째 로비서버를 골라서 들어가도록 처리
                server_tag = ServerType.Lobby;
                server_now = server_tag;
                foreach (var lobby in lobbys)
                {
                    client.RequestGoLobby(remote, CPackOption.Basic, lobby);
                    break;
                }
            }
        }
        else
        {
            PopupLevelUp();
            PopupMessage.SetActive(true);
            PopupMessage.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = resultMessage;
        }
        return true;
    }
    bool NotifyLobbyList(RemoteID remote, CPackOption rmiContext, System.Collections.Generic.List<string> lobbyList)
    {
        lobbys = lobbyList.ToList();
        return true;
    }
    bool ResponseLoginKey(RemoteID remote, CPackOption rmiContext, bool result, string resultMessage)
    {
        if (result)
        {
            client.RequestLobbyList(remote, CPackOption.Basic);
            Unitycoding.EventHandler.Execute("OnLogin");
            if (server_now == ServerType.Login)
            {
                // 일단 간단한 처리를 위해 첫번째 로비서버를 골라서 들어가도록 처리
                server_tag = ServerType.Lobby;
                server_now = server_tag;
                foreach (var lobby in lobbys)
                {
                    client.RequestGoLobby(remote, CPackOption.Basic, lobby);
                    break;
                }
            }
        }
        else
        {
            PopupLevelUp();
            PopupMessage.SetActive(true);
            PopupMessage.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = resultMessage;
        }

        return true;
    }
    bool ResponseGameOptions(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        //CMessage msg = new CMessage(data);

        //try
        //{
        //    int OptionVoice;
        //    Rmi.Marshaler.Read(msg, out OptionVoice);
        //    SoundType = OptionVoice;
        //    DiffSoundType = OptionVoice;

        //    int OptionBetType;
        //    Rmi.Marshaler.Read(msg, out OptionBetType);
        //    BetButtonType = OptionBetType;
        //    DiffBetButtonType = OptionBetType;
        //}
        //catch (Exception e)
        //{
        //    // 옵션값 설정 실패
        //}

        return true;
    }
    #endregion

    #region Lobby Server Handler
    bool ResponseLobbyKey(RemoteID remote, CPackOption rmiContext, string key)
    {
        //if (key != "")
        //{
        //    IdentifierKey = key;
        //    try
        //    {
        //        System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + "/Badugi/Badugi.exe", IdentifierID + " " + IdentifierKey);
        //        Application.Quit();
        //    }
        //    catch (Exception)
        //    {
        //        Application.Quit();
        //    }
        //}
        return true;
    }

    CRecvedMsg GetMsg(CMessage Msg_, RemoteID remote_, CPackOption rmiContext_, PacketType mRmiID_)
    {
        CRecvedMsg rmsg = new CRecvedMsg();
        rmsg.msg = Msg_;
        rmsg.remote = remote_;
        rmsg.pkop = rmiContext_;
        rmsg.pkID = mRmiID_;
        return rmsg;
    }

    bool NotifyUserInfo(RemoteID remote, CPackOption rmiContext, Rmi.Marshaler.LobbyUserInfo userInfo)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, userInfo);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.NotifyUserInfo));
        return true;
    }
    bool NotifyUserList(RemoteID remote, CPackOption rmiContext, System.Collections.Generic.List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, System.Collections.Generic.List<string> lobbyFriendList)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, lobbyUserInfos);
        Rmi.Marshaler.Write(msg, lobbyFriendList);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.NotifyUserList));
        return true;
    }
    bool NotifyRoomList(RemoteID remote, CPackOption rmiContext, int channelID, System.Collections.Generic.List<Rmi.Marshaler.RoomInfo> roomInfos)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, channelID);
        Rmi.Marshaler.Write(msg, roomInfos);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.NotifyRoomList));
        return true;
    }
    bool ResponseChannelMove(RemoteID remote, CPackOption rmiContext, int chanID)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, chanID);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.ResponseChannelMove));
        return true;
    }
    bool ResponseLobbyMessage(RemoteID remote, CPackOption rmiContext, string message)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, message);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.ResponseLobbyMessage));
        return true;
    }
    bool ResponseBank(RemoteID remote, CPackOption rmiContext, bool result, int resultType)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, result);
        Rmi.Marshaler.Write(msg, resultType);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.ResponseBank));
        return true;
    }
    bool NotifyJackpotInfo(RemoteID remote, CPackOption rmiContext, long jackpot)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, jackpot);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.NotifyJackpotInfo));
        return true;
    }
    bool NotifyLobbyMessage(RemoteID remote, CPackOption rmiContext, int type, string message, int period)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, type);
        Rmi.Marshaler.Write(msg, message);
        Rmi.Marshaler.Write(msg, period);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.NotifyLobbyMessage));
        return true;
    }
    // 모바일
    bool ResponsePurchaseList(RemoteID remote, CPackOption rmiContext, System.Collections.Generic.List<string> Purchase_avatar, System.Collections.Generic.List<string> Purchase_card, System.Collections.Generic.List<string> Purchase_evt, System.Collections.Generic.List<string> Purchase_charge)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, Purchase_avatar);
        Rmi.Marshaler.Write(msg, Purchase_card);
        Rmi.Marshaler.Write(msg, Purchase_evt);
        Rmi.Marshaler.Write(msg, Purchase_charge);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.ResponsePurchaseList));
        return true;
    }
    bool ResponsePurchaseAvailability(RemoteID remote, CPackOption rmiContext, bool available, string reason)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, available);
        Rmi.Marshaler.Write(msg, reason);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.ResponsePurchaseAvailability));
        return true;
    }
    bool ResponsePurchaseReceiptCheck(RemoteID remote, CPackOption rmiContext, bool result, System.Guid token)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, result);
        Rmi.Marshaler.Write(msg, token);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.ResponsePurchaseReceiptCheck));
        return true;
    }
    bool ResponsePurchaseResult(RemoteID remote, CPackOption rmiContext, bool result, string reason)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, result);
        Rmi.Marshaler.Write(msg, reason);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.ResponsePurchaseResult));
        return true;
    }
    bool ResponsePurchaseCash(RemoteID remote, CPackOption rmiContext, bool result, string reason)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, result);
        Rmi.Marshaler.Write(msg, reason);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.ResponsePurchaseCash));
        return true;
    }
    bool ResponseMyroomList(RemoteID remote, CPackOption rmiContext, string json)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, json);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.ResponseMyroomList));
        return true;
    }
    bool ResponseMyroomAction(RemoteID remote, CPackOption rmiContext, string pid, bool result, string reason)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, pid);
        Rmi.Marshaler.Write(msg, result);
        Rmi.Marshaler.Write(msg, reason);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.ResponseMyroomAction));
        return true;
    }
    #endregion

    #region Game Server Handler
    bool GameResponseRoomMove(RemoteID remote, CPackOption rmiContext, bool move, string errorMessage)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, move);
        Rmi.Marshaler.Write(msg, errorMessage);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.GameResponseRoomMove));
        return true;
    }
    bool GameResponseRoomOutRsvp(RemoteID remote, CPackOption rmiContext, byte player_index, bool IsRsvp)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, player_index);
        Rmi.Marshaler.Write(msg, IsRsvp);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.GameResponseRoomOutRsvp));
        return true;
    }
    bool GameResponseRoomOut(RemoteID remote, CPackOption rmiContext, bool result)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, result);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.GameResponseRoomOut));
        return true;
    }
    bool GameRoomIn(RemoteID remote, CPackOption rmiContext, bool result)
    {
        CMessage msg = new CMessage();
        Rmi.Marshaler.Write(msg, result);
        waiting_packets.Enqueue(GetMsg(msg, remote, rmiContext, SS.Common.GameRoomIn));
        return true;
    }
    bool GameRoomReady(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameRoomReady));
        return true;
    }
    bool GameStart(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameStart));
        return true;
    }
    bool GameDealCards(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameDealCards));
        return true;
    }
    bool GameUserIn(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameUserIn));
        return true;
    }
    bool GameSetBoss(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameSetBoss));
        return true;
    }
    bool GameNotifyStat(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameNotifyStat));
        return true;
    }
    bool GameRoundStart(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameRoundStart));
        return true;
    }
    bool GameChangeTurn(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameChangeTurn));
        return true;
    }
    bool GameRequestBet(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameRequestBet));
        return true;
    }
    bool GameResponseBet(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameResponseBet));
        return true;
    }
    bool GameChangeRound(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameChangeRound));
        return true;
    }
    bool GameRequestChangeCard(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameRequestChangeCard));
        return true;
    }
    bool GameResponseChangeCard(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameResponseChangeCard));
        return true;
    }
    bool GameCardOpen(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameCardOpen));
        return true;
    }
    bool GameOver(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameOver));
        return true;
    }
    bool GameRoomInfo(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameRoomInfo));
        return true;
    }
    bool GameKickUser(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameKickUser));
        return true;
    }
    bool GameEventInfo(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameEventInfo));
        return true;
    }
    bool GameUserInfo(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameUserInfo));
        return true;
    }
    bool GameUserOut(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameUserOut));
        return true;
    }
    bool GameEventStart(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameEventStart));
        return true;
    }
    bool GameEvent2Start(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameEvent2Start));
        return true;
    }
    bool GameEventRefresh(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameEventRefresh));
        return true;
    }
    bool GameEventEnd(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameEventEnd));
        return true;
    }
    bool GameMileageRefresh(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameMileageRefresh));
        return true;
    }
    bool GameEventNotify(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameEventNotify));
        return true;
    }
    bool GameCurrentInfo(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameCurrentInfo));
        return true;
    }
    bool GameEntrySpectator(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameEntrySpectator));
        return true;
    }
    bool GameNotifyMessage(RemoteID remote, CPackOption rmiContext, CMessage data)
    {
        waiting_packets.Enqueue(GetMsg(data, remote, rmiContext, SS.Common.GameNotifyMessage));
        return true;
    }
    #endregion

    #region Mobile Request
    class EVENT<T0> : UnityEvent<T0> { }
    class EVENT<T0, T1> : UnityEvent<T0, T1> { }
    class EVENT<T0, T1, T2> : UnityEvent<T0, T1, T2> { }

    EVENT<string> CBStoreList = new EVENT<string>();
    EVENT<bool, string> CBCheckEnablePurchase = new EVENT<bool, string>();
    EVENT<bool, Guid> CBCheckPurchaseReceipt = new EVENT<bool, Guid>();
    EVENT<bool, string> CBPurchaseResult = new EVENT<bool, string>();
    EVENT<bool, string> CBPurchaseCash = new EVENT<bool, string>();

    EVENT<string> CBMyRoomList = new EVENT<string>();
    EVENT<string, bool, string> CBMyRoomAction = new EVENT<string, bool, string>();

    public void GetStoreList(UnityAction<string> argCallback)
    {
        CBStoreList.RemoveListener(argCallback);
        CBStoreList.AddListener(argCallback);
        client.RequestPurchaseList(RemoteID.Remote_Server, CPackOption.Basic);
    }
    public void CheckEnablePurchase(string pid, UnityAction<bool, string> argCallback)
    {
        CBCheckEnablePurchase.RemoveListener(argCallback);
        CBCheckEnablePurchase.AddListener(argCallback);
        client.RequestPurchaseAvailability(RemoteID.Remote_Server, CPackOption.Basic, pid);
    }
    public void CheckPurchaseReceipt(string receipt, UnityAction<bool, Guid> argCallback)
    {
        CBCheckPurchaseReceipt.RemoveListener(argCallback);
        CBCheckPurchaseReceipt.AddListener(argCallback);
        client.RequestPurchaseReceiptCheck(RemoteID.Remote_Server, CPackOption.Basic, receipt);

    }
    public void PurchaseResult(Guid token, UnityAction<bool, string> argCallback)
    {
        CBPurchaseResult.RemoveListener(argCallback);
        CBPurchaseResult.AddListener(argCallback);
        client.RequestPurchaseResult(RemoteID.Remote_Server, CPackOption.Basic, token);
    }
    public void PurchaseCash(string pid, UnityAction<bool, string> argCallback)
    {
        CBPurchaseCash.RemoveListener(argCallback);
        CBPurchaseCash.AddListener(argCallback);
        client.RequestPurchaseCash(RemoteID.Remote_Server, CPackOption.Basic, pid);
    }
    public void GetMyRoomList(UnityAction<string> argCallback)
    {
        CBMyRoomList.RemoveListener(argCallback);
        CBMyRoomList.AddListener(argCallback);
        client.RequestMyroomList(RemoteID.Remote_Server, CPackOption.Basic);
    }
    public void ChangeMyRoomAction(string pid, UnityAction<string, bool, string> argCallback)
    {
        CBMyRoomAction.RemoveListener(argCallback);
        CBMyRoomAction.AddListener(argCallback);
        client.RequestMyroomAction(RemoteID.Remote_Server, CPackOption.Basic, pid);
    }
    #endregion
}