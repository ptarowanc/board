using Server.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZNet;

namespace Server.Common
{    /// <summary>
     /// 서버이동시 검사할 파라미터
     /// </summary>
    public class MoveParam
    {
        public enum ParamMove : int
        {
            MoveToLogin = 0,
            MoveToLobby,
            MoveToRoom // 중계서버로 이동하는 것
        }
        public enum ParamRoom : int
        {
            RoomNull = 0,
            RoomMake,
            RoomJoin
        }
        public ParamMove moveTo;
        public ParamRoom roomJoin;
        public Guid room_id;
        public int lobby_remote;
        public int ChannelNumber;
        public int roomStake;
        public string roomPassword;
        public int RelayID; // 릴레이 서버 위치
        public RemoteID session; // 이동하기 전 서버에서의 ID
        public void From(ParamMove _to, ParamRoom _join, Guid id, int _lobby, int _ChannelNumber, int _roomStake, string _roomPassword, int _RelayID)
        {
            moveTo = _to;
            roomJoin = _join;
            room_id = id;
            lobby_remote = _lobby;
            ChannelNumber = _ChannelNumber;
            roomStake = _roomStake;
            roomPassword = _roomPassword;
            RelayID = _RelayID;
        }

    }
    public class Common
    {
        // 서버이동시 동기화할 유저 데이터 구성
        static public void ServerMoveStart(CPlayer rc, out ZNet.ArrByte buffer)
        {
            CMessage msg = new CMessage();
            msg.Write(rc.data.ID);
            msg.Write(rc.data.userID);
            msg.Write(rc.data.nickName);
            msg.Write(rc.data.avatar);
            msg.Write(rc.data.cash);
            msg.Write(rc.data.money_pay);
            msg.Write(rc.data.money_free);
            msg.Write(rc.data.bank_money_pay);
            msg.Write(rc.data.bank_money_free);
            msg.Write(rc.data.winCount);
            msg.Write(rc.data.loseCount);
            msg.Write(rc.data.mileage);
            msg.Write(rc.data.charm);
            msg.Write(rc.data.voice);
            msg.Write(rc.data.avatar_card);
            msg.Write(rc.data.member_point);
            msg.Write(rc.data.shop_name);
            msg.Write(rc.data.shopId);
            msg.Write(rc.data.IPFree);
            msg.Write(rc.data.ShopFree);
            msg.Write(rc.data.RelayID);
            msg.Write(rc.data.Old);
            msg.Write(rc.data.Restrict);
            msg.Write((int)111);
            buffer = msg.m_array;
        }

        // 서버이동 완료시 동기화할 유저 데이터 복구
        static public void ServerMoveComplete(ZNet.ArrByte buffer, out CPlayer data)
        {
            CPlayer rc = new CPlayer();

            CMessage msg = new CMessage();
            msg.m_array = buffer;
            msg.Read(out rc.data.ID);
            msg.Read(out rc.data.userID);
            msg.Read(out rc.data.nickName);
            msg.Read(out rc.data.avatar);
            msg.Read(out rc.data.cash);
            msg.Read(out rc.data.money_pay);
            msg.Read(out rc.data.money_free);
            msg.Read(out rc.data.bank_money_pay);
            msg.Read(out rc.data.bank_money_free);
            msg.Read(out rc.data.winCount);
            msg.Read(out rc.data.loseCount);
            msg.Read(out rc.data.mileage);
            msg.Read(out rc.data.charm);
            msg.Read(out rc.data.voice);
            msg.Read(out rc.data.avatar_card);
            msg.Read(out rc.data.member_point);
            msg.Read(out rc.data.shop_name);
            msg.Read(out rc.data.shopId);
            msg.Read(out rc.data.IPFree);
            msg.Read(out rc.data.ShopFree);
            msg.Read(out rc.data.RelayID);
            msg.Read(out rc.data.Old);
            msg.Read(out rc.data.Restrict);
            data = rc;
        }
        static public void ServerMoveComplete(Server.Engine.UserData userData, out CPlayer data)
        {
            CPlayer rc = new CPlayer();

            rc.data = userData;
            data = rc;

            // 서버이동 입장인 경우 즉시 인증완료 상태로 세팅
        }

        static public void ServerMoveParamWrite(MoveParam param, out ZNet.ArrByte buffer)
        {
            if (param.roomPassword == null) param.roomPassword = "";
            CMessage msg = new CMessage();
            msg.Write((int)param.moveTo);
            msg.Write((int)param.roomJoin);
            msg.Write(param.room_id);
            msg.Write(param.lobby_remote);
            msg.Write(param.ChannelNumber);
            msg.Write(param.roomStake);
            msg.Write(param.roomPassword);
            buffer = msg.m_array;
        }
        static public void ServerMoveParamRead(ArrByte buffer, out MoveParam param)
        {
            param = new MoveParam();
            CMessage msg = new CMessage();
            msg.Write(buffer);
            msg.ResetPosition();
            //msg.m_array = buffer;
            int _moveTo;
            int _roomJoin;
            msg.Read(out _moveTo);
            msg.Read(out _roomJoin);
            msg.Read(out param.room_id);
            msg.Read(out param.lobby_remote);
            msg.Read(out param.ChannelNumber);
            msg.Read(out param.roomStake);
            msg.Read(out param.roomPassword);
            param.moveTo = (MoveParam.ParamMove)_moveTo;
            param.roomJoin = (MoveParam.ParamRoom)_roomJoin;
        }
        static public void ServerMoveParamRead2(CMessage buffer, out MoveParam param)
        {
            param = new MoveParam();
            CMessage msg = buffer;
            //msg.m_array = buffer;
            int _moveTo;
            int _roomJoin;
            msg.Read(out _moveTo);
            msg.Read(out _roomJoin);
            msg.Read(out param.room_id);
            msg.Read(out param.lobby_remote);
            msg.Read(out param.ChannelNumber);
            msg.Read(out param.roomStake);
            msg.Read(out param.roomPassword);
            param.moveTo = (MoveParam.ParamMove)_moveTo;
            param.roomJoin = (MoveParam.ParamRoom)_roomJoin;
        }

        static public void ServerMoveParamWrite(MoveParam param, CPlayer rc, out ZNet.ArrByte buffer)
        {
            if (param.roomPassword == null) param.roomPassword = "";
            CMessage msg = new CMessage();
            msg.Write((int)param.moveTo);
            msg.Write((int)param.roomJoin);
            msg.Write(param.room_id);
            msg.Write(param.lobby_remote);
            msg.Write(param.ChannelNumber);
            msg.Write(param.roomStake);
            msg.Write(param.roomPassword);
            msg.Write(param.RelayID);

            if (rc == null)
            {
                msg.Write(false);
            }
            else
            {
                msg.Write(true);
                msg.Write(rc.data.ID);
                msg.Write(rc.data.userID);
                msg.Write(rc.data.nickName);
                msg.Write(rc.data.avatar);
                msg.Write(rc.data.cash);
                msg.Write(rc.data.money_pay);
                msg.Write(rc.data.money_free);
                msg.Write(rc.data.bank_money_pay);
                msg.Write(rc.data.bank_money_free);
                msg.Write(rc.data.winCount);
                msg.Write(rc.data.loseCount);
                msg.Write(rc.data.mileage);
                msg.Write(rc.data.charm);
                msg.Write(rc.data.voice);
                msg.Write(rc.data.avatar_card);
                msg.Write(rc.data.member_point);
                msg.Write(rc.data.shop_name);
                msg.Write(rc.data.shopId);
                msg.Write(rc.data.IPFree);
                msg.Write(rc.data.ShopFree);
                msg.Write(rc.data.RelayID);
                msg.Write(rc.data.Old);
                msg.Write(rc.data.Restrict);
                msg.Write((int)111);
            }

            buffer = msg.m_array;
        }
        static public void ServerMoveParamRead(ZNet.ArrByte buffer, out MoveParam param, out CPlayer rc)
        {
            // 서버이동 정보
            param = new MoveParam();
            CMessage msg = new CMessage();
            msg.m_array = buffer;
            int _moveTo;
            int _roomJoin;
            msg.Read(out _moveTo);
            msg.Read(out _roomJoin);
            msg.Read(out param.room_id);
            msg.Read(out param.lobby_remote);
            msg.Read(out param.ChannelNumber);
            msg.Read(out param.roomStake);
            msg.Read(out param.roomPassword);
            msg.Read(out param.RelayID);
            param.moveTo = (MoveParam.ParamMove)_moveTo;
            param.roomJoin = (MoveParam.ParamRoom)_roomJoin;

            // 유저이동 정보
            bool rcExist;
            msg.Read(out rcExist);
            rc = new CPlayer();
            if (rcExist)
            {
                msg.Read(out rc.data.ID);
                msg.Read(out rc.data.userID);
                msg.Read(out rc.data.nickName);
                msg.Read(out rc.data.avatar);
                msg.Read(out rc.data.cash);
                msg.Read(out rc.data.money_pay);
                msg.Read(out rc.data.money_free);
                msg.Read(out rc.data.bank_money_pay);
                msg.Read(out rc.data.bank_money_free);
                msg.Read(out rc.data.winCount);
                msg.Read(out rc.data.loseCount);
                msg.Read(out rc.data.mileage);
                msg.Read(out rc.data.charm);
                msg.Read(out rc.data.voice);
                msg.Read(out rc.data.avatar_card);
                msg.Read(out rc.data.member_point);
                msg.Read(out rc.data.shop_name);
                msg.Read(out rc.data.shopId);
                msg.Read(out rc.data.IPFree);
                msg.Read(out rc.data.ShopFree);
                msg.Read(out rc.data.RelayID);
                msg.Read(out rc.data.Old);
                msg.Read(out rc.data.Restrict);
            }

        }
        static public void ServerMoveParamRead2(ZNet.ArrByte buffer, out MoveParam param, out CPlayer rc)
        {
            // 서버이동 정보
            param = new MoveParam();
            CMessage msg = new CMessage();
            ZNet.CMessage Msg2 = new ZNet.CMessage();
            Msg2.m_array = buffer;
            unsafe
            {
                byte[] Datas = Msg2.GetData();
                //Rmi.Marshaler.Write(Msg, Datas.Count());
                fixed (byte* pData = &Datas[4])
                {
                    msg.Write(pData, Datas.Count());
                }
            }
            msg.ResetPosition();
            int _moveTo;
            int _roomJoin;

            msg.Read(out _moveTo);
            msg.Read(out _roomJoin);
            msg.Read(out param.room_id);
            msg.Read(out param.lobby_remote);
            msg.Read(out param.ChannelNumber);
            msg.Read(out param.roomStake);
            msg.Read(out param.roomPassword);
            msg.Read(out param.RelayID);
            param.moveTo = (MoveParam.ParamMove)_moveTo;
            param.roomJoin = (MoveParam.ParamRoom)_roomJoin;

            // 유저이동 정보
            bool rcExist;
            msg.Read(out rcExist);
            rc = new CPlayer();
            if (rcExist)
            {
                msg.Read(out rc.data.ID);
                msg.Read(out rc.data.userID);
                msg.Read(out rc.data.nickName);
                msg.Read(out rc.data.avatar);
                msg.Read(out rc.data.cash);
                msg.Read(out rc.data.money_pay);
                msg.Read(out rc.data.money_free);
                msg.Read(out rc.data.bank_money_pay);
                msg.Read(out rc.data.bank_money_free);
                msg.Read(out rc.data.winCount);
                msg.Read(out rc.data.loseCount);
                msg.Read(out rc.data.mileage);
                msg.Read(out rc.data.charm);
                msg.Read(out rc.data.voice);
                msg.Read(out rc.data.avatar_card);
                msg.Read(out rc.data.member_point);
                msg.Read(out rc.data.shop_name);
                msg.Read(out rc.data.shopId);
                msg.Read(out rc.data.IPFree);
                msg.Read(out rc.data.ShopFree);
                msg.Read(out rc.data.RelayID);
                msg.Read(out rc.data.Old);
                msg.Read(out rc.data.Restrict);
            }

        }

        //static public void DisplayStatus(ZNet.CoreServerNet svr)
    }
}
