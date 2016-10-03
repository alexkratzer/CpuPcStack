using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace cpsLIB
{
    //TODO: für jede neue client anfrage eine liste mit status der verbindung verwalten.
    //als key für den datensatz die Remote IP verwenden
    
    public enum udp_state { unknown, connected, disconnected, error }
    //public enum msg_type { undef, info, warning, error }
    public class cmd
    {
        public Int16 MaxSYNCResendTrys = 3; //Anzahl der erlaubten Wiederholungen bei SYNC Telegram
        public Int16 WATCHDOG_WORK = 2000; //Erlaubte Zeitdauer in ms bis PLC geantwortet haben muss
        public bool SendFramesCallback = true; //es werden die "zu sendenden frames" als callback zurückgeliefert
        public bool SendOnlyIfConnected = false;

        public static IcpsLIB _FrmMain;
        private udp_server _udp_server;
        private udp_client _udp_client;
        System.Collections.Concurrent.ConcurrentQueue<Frame> _fstack = null;

        private List<connectStatus> ListConnectStatus = new List<connectStatus>();

        //TODO: alle frames in liste speichern
        //System.Collections.Concurrent.ConcurrentQueue<Frame> _fstackLog = null;

        public int TotalFramesFinished = 0; //Frames die auf eine anfrage hin empfangen wurden und verarbeitet werden können
 
        //Constructor
        public cmd(IcpsLIB FrmMain)
        {
            _FrmMain = FrmMain;
            _udp_client = new udp_client();
            _fstack = new System.Collections.Concurrent.ConcurrentQueue<Frame>();
            //_fstackLog = new System.Collections.Concurrent.ConcurrentQueue<Frame>();
            StackWorker();
        }

        #region client
        public void send(Frame f)
        {
            if (ConnectVerifyState(f, udp_state.connected) || FrameType.SYNC.ToString().Equals(f._type))
            {
                if (putFrameToStack(f))
                {
                    _udp_client.send(f);
                    f.ChangeState(FrameWorkingState.send, "msg from UDPclient to app");
                }
            }
            else
                f.ChangeState(FrameWorkingState.error, "Remote udp_state NOT connected - NO Frame is send");

            //der App wird mitgeteilt das dieses frame verschickt wurde
            if (SendFramesCallback)
                _FrmMain.interprete_frame(f);
        }

        public void ConnectionCheck(string ip, string port)
        {
            foreach (connectStatus cs in ListConnectStatus)
            {
                if (cs.ip.Equals(ip) && cs.sport == port)
                {
                    Frame f = new Frame(ip, port.ToString(), FrameType.SYNC.ToString(), cs.check_trys);
                    send(f);
                    cs.check_trys++;
                    cs.state = udp_state.unknown;
                    return;
                }
            }
            //wenn keine connection gefunden wurde wird eine neue angelegt
            ListConnectStatus.Add(new connectStatus(ip, port));
            ConnectionCheck(ip, port);

        }

        private bool ConnectVerifyState(Frame f, udp_state matchState)
        {
            if (SendOnlyIfConnected)
            {
                if (ListConnectStatus.Count > 0)
                {
                    foreach (connectStatus cs in ListConnectStatus)
                    {
                        if (cs.ip.Equals(f.RemoteIp) && cs.iport == f.RemotePort)
                        {
                            f.ChangeState(FrameWorkingState.inWork, "ConnectVerifyState (Soll: " + matchState + " / IST: " +cs.state + ")" );
                            if (cs.state == matchState)
                                return true;
                            else
                                return false;
                        }
                    }
                    return false;
                }
                else
                {
                    f.ChangeState(FrameWorkingState.error, "ConnectVerifyState with no connections in ListConnectStatus");
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region server
        public void serverSTART(string port)
        {
            _udp_server = new udp_server(this, port);
        }
        public void serverSTOP()
        {
            if (_udp_server != null)
                _udp_server.stop();
        }

        public void server_message(string msg)
        {
            _FrmMain.logMsg(msg);
        }

        public void receive(FrameRcv f)
        {
            //TODO: handle different IP requests in list
            
            ConnectStateChange(f, udp_state.connected);

            //received frame will be passed to the main application
            _FrmMain.interprete_frame(f);

            //remove frame from "InWork Jobs" 
            if (!_fstack.IsEmpty)
            {
                foreach (Frame frameStack in _fstack)
                    if (frameStack._sequenzeNumber == f._sequenzeNumber)
                    {
                        //+++++++++++++++++ matching rcv frame to frame in stack +++++++++++++++++++
                        if (takeFrameFromStack(frameStack))
                        {
                            TotalFramesFinished++;
                            f.ChangeState(FrameWorkingState.finish, "takeFrameFromStack - drop this one");
                        }
                        else
                            f.ChangeState(FrameWorkingState.error, "ERROR dequeue Frame from stack... ");

                        return;
                    }
            }
            f.ChangeState(FrameWorkingState.error, "received udp frame without request...");
            _FrmMain.logMsg("received udp frame without request...");
        }
        private void ConnectStateChange(FrameRcv f, udp_state state)
        {
            foreach (connectStatus cs in ListConnectStatus)
                if (cs.ip.Equals(f.RemoteIp)) //Sende und Remote Port sind unterschiedlich -> cs.iport == f.RemotePort
                {
                    cs.state = state;
                    return;
                }
            //wenn keine connection zu dem frame gefunden wurde wird fehler gemeldet
            f.ChangeState(FrameWorkingState.warning, "no connection found to frame with IP: " + f.RemoteIp);
        }

        #endregion

        #region handle frame stack
        /// <summary>
        /// frame aus stack löschen
        /// </summary>
        /// <returns></returns>
        private bool takeFrameFromStack(Frame f)
        {
            if (_fstack.TryDequeue(out f))
            {
                f.ChangeState(FrameWorkingState.finish, "Dequeue frame from stack");
                return true;
            }

            f.ChangeState(FrameWorkingState.error, "ERROR dequeue Frame from stack... ");
            return false;

        }

        /// <summary>
        /// neuen frame für cpu in puffer legen
        /// wenn ein identisches frame (index wird nicht bewertet) 
        /// bereits vorhanden ist wird dieses nicht erneut abgelegt
        /// sondern in _fstackLog eingereiht
        /// </summary>
        /// <param name="f"></param>
        private bool putFrameToStack(Frame f)
        {
            if (!_fstack.IsEmpty)
                foreach (Frame frame in _fstack)
                {
                    if (frame.isEqualExeptIndex(f))
                    {
                        f.ChangeState(FrameWorkingState.error, "Frame already in send buffer");
                        //_fstackLog.Enqueue(f);
                        Thread.Sleep(100);
                        return false;
                    }
                }

            f.ChangeState(FrameWorkingState.inWork, "Frame put to Stack");
            _fstack.Enqueue(f);
            return true;
        }

        public int InWorkFrameCount()
        {
            if (_fstack != null)
                return _fstack.Count;
            else
                return 0;
        }

        ///// <summary>
        ///// gibt alle frames im stack als string aus
        ///// </summary>
        //public string GetStackAsString() {

        //    string s = String.Empty;
        //    foreach (Frame f in _fstack)
        //        s += f.GetDetailedString() + Environment.NewLine;
        //    return s;
        //}
        #endregion
        #region thread worker
        Thread ThreadStackWorker;
        private void StackWorker()
        {
            ThreadStackWorker = new Thread(new ThreadStart(StackWorker_fkt));
            ThreadStackWorker.IsBackground = true;
            ThreadStackWorker.Start();
        }

        /// <summary>
        /// verifiziert alle "in arbeit" befindlichen frames und überwacht die bearbeitungszeit
        /// </summary>
        private void StackWorker_fkt()
        {
            while (true)
            {
                if (!_fstack.IsEmpty)
                {
                    foreach (Frame f in _fstack)
                    {
                        if (f.LastSendDateTime.AddMilliseconds(WATCHDOG_WORK) < DateTime.Now)
                        {
                            //hit Watchdog
                            if (f._type.Equals(FrameType.SYNC.ToString()))
                            {
                                if (f.SendTrys < MaxSYNCResendTrys)
                                {
                                    f.SendTrys++;
                                    f.LastSendDateTime = DateTime.Now;
                                    f.ChangeState(FrameWorkingState.warning, "repeat send: (" + f.SendTrys.ToString() + ")");
                                    _udp_client.send(f);
                                }
                                else
                                {
                                    f.ChangeState(FrameWorkingState.error, "stop sending at try: (" + f.SendTrys.ToString() + ")");
                                    takeFrameFromStack(f);
                                }
                            }
                            else
                            {
                                f.ChangeState(FrameWorkingState.error, "no answer to sendrequest");
                                takeFrameFromStack(f);
                            }
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }
        #endregion

        class connectStatus
        {
            public Int16 check_trys = 0;
            public string ip="";
            public int iport;
            public string sport;
            public udp_state state;
            public connectStatus(string ip, string port) { 
                this.ip = ip;
                sport = port;
                int.TryParse(port, out iport);
                
                state = udp_state.unknown;
            }
        }
    }
}
