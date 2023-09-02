using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityCommon;
using ZNet;

namespace Server.Master
{
    class MasterServer : Base.BaseServer
    {
        public ConcurrentDictionary<ZNet.RemoteID, ServerType> RemoteClients = new ConcurrentDictionary<ZNet.RemoteID, ServerType>();

        public MasterServer(FormServer f, UnityCommon.Server t, int portnum) : base(f, t, portnum)
        {
        }

        protected override void BeforeServerStart(out StartOption param)
        {
            param = new StartOption();

            param.m_IpAddressListen = Properties.Settings.Default.MasterIp;
            param.m_PortListen = Properties.Settings.Default.MasterPort;
            param.m_MaxConnectionCount = 5000;
            param.m_UpdateTimeMs = 1000;

            m_Core.SetKeepAliveOption(60);
            //m_Core.update_event_handler = ScheduleTask;
            m_Core.message_handler = CoreMessage;
            m_Core.exception_handler = CoreException;


            //마스터 서버에서만 발생하는 이벤트 처리 : 마스터 클라이언트 서버 입장 시점
            m_Core.master_server_join_handler = (ZNet.RemoteID remote, string description, int type, ZNet.NetAddress addr) =>
            {
                RemoteClients.TryAdd(remote, (ServerType)type);
                form.printf("마스터 Client Join remoteID({0}) {1} type({2})", remote, description, type);
            };
            //마스터 서버에서의 접속 해제 이벤트 -> 마스터 클라이언트의 퇴장
            m_Core.client_disconnect_handler = (ZNet.RemoteID remote) =>
            {
                ServerType temp;
                RemoteClients.TryRemove(remote, out temp);
                form.printf("마스터 Client Leave remoteID({0}) type({1})", remote, temp);
            };

            form.ButtonEnable(new System.EventHandler(this.button1_Click));
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

        protected override void NewCore()
        {
            //인자값이 마스터 서버이므로 재정의
            m_Core = new ZNet.CoreServerNet(true);
        }

        bool ShutDown = false;
        bool ShutDownReady = false;
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
            if (tick % 3 == 0)
            {
                if (ShutDown)
                {
                    if (ShutDownReady == false)
                    {
                        string log = "서버 종료중. 남은 서버 수:" + m_Core.GetCountClient();
                        form.printf(log);
                        Log._log.Warn(log);

                        if (m_Core.GetCountClient() == 0)
                        {
                            ShutDownReady = true;
                            tick = 0;
                        }
                    }
                    else
                    {
                        System.Windows.Forms.Application.Exit();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ShutDown) return;
            ShutDown = true;
            form.ButtonDisable();

            string log = "서버 일괄 종료.";
            form.printf(log);
            Log._log.Warn(log);

            ShutDownServers();
        }

        public void ShutDownServers()
        {
            foreach (var client in RemoteClients)
            {
                Log._log.Info("서버 종료 요청. " + client.Value);
                proxy.master_all_shutdown((ZNet.RemoteID)client.Key, ZNet.CPackOption.Basic, "shutdown");
            }
        }

    }
}
