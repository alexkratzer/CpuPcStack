using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace UdpPlcLIB
{
    public enum udp_state { unknown, connected, disconnected, error }
    public enum msg_type { undef, info, warning, error }
    public class net_udp /*: projectIUdpPlcLib.IUdpPlcLib*/
    {
       
        // intern
        private IUdpPlcLib _FrmMain;
        private udp_server _udp_server;
        private udp_client _udp_client;
        System.Collections.Concurrent.ConcurrentQueue<Frame> _fstack = null;
        private const Int16 MAXCheckTrys = 5; //Anzahl der erlaubten Wiederholungen bei SYNC Telegram
        private const Int16 MAXSendTrys = 1; //Anzahl der erlaubten Wiederholungen bei Telegram die versendet werden
        System.Collections.Concurrent.ConcurrentQueue<Frame> _fstackLog = null;

        /// <summary>
        /// status schnittstelle
        /// </summary>
        public udp_state state;
        public int TotalFramesSend = 0;
        public int TotalFramesReceive = 0;
        public int TotalFramesFinished = 0; //Frames die auf eine anfrage hin empfangen wurden und verarbeitet werden können
        public Int16 check_trys;
       
        //Constructor
        public net_udp(IUdpPlcLib FrmMain) {
            this._FrmMain = FrmMain;
            state = udp_state.unknown;
            _udp_client = new udp_client(this);
            _fstack = new System.Collections.Concurrent.ConcurrentQueue<Frame>();
            _fstackLog = new System.Collections.Concurrent.ConcurrentQueue<Frame>();
            StackWorker();
        }

        #region client
        public void send(Frame f)
        {
            if (putFrameToStack(f))
                _udp_client.send(f);
        }
        public void client_message(Frame f)
        {
            f.sender = FrameSender.client;
            _FrmMain.logSendRcv(f);
            TotalFramesSend++;
        }
        #endregion

        #region server
        public void serverSTART(string port) {
            _udp_server = new udp_server(this, port);
        }
        public void serverSTOP() {
            if (_udp_server != null)
                _udp_server.stop();
        }

        public void server_message(string msg) {
            _FrmMain.logMsg(msg);
        }

        public void receive(Frame f)
        {
            _FrmMain.interprete_frame(f);
            TotalFramesReceive++;
            state = udp_state.connected;

            //remove frame from "InWork Jobs" Stack
            if (!_fstack.IsEmpty){
                foreach (Frame frame in _fstack)
                    if (frame._index == f._index)
                    {
                        if (takeFrameFromStack(frame))
                        {
                            TotalFramesFinished++;
                            frame.ChangeState(FrameWorkingState.done, "takeFrameFromStack");
                            _fstackLog.Enqueue(frame);
                            _FrmMain.logSendRcv(frame);
                            _FrmMain.logSendRcv(f);
                        }
                        else
                        {
                            _FrmMain.logSendRcv(f.ChangeState(FrameWorkingState.error, "ERROR dequeue Frame from stack... "));
                            _FrmMain.logSendRcv(frame);   
                        }
                        return;
                    }
                }
            f.ChangeState(FrameWorkingState.error, "received udp frame without request...");
            _FrmMain.logSendRcv(f); 
        }
        #endregion

        public void reset()
        {

            state = udp_state.unknown;
            TotalFramesSend = 0;
            TotalFramesReceive = 0;
            TotalFramesFinished = 0;
            check_trys = 0;
        }

        #region check connection
        public void check_connection(string ip, string port){
            Frame f = new Frame(FrameType.SYNC.ToString(), check_trys, ip, port);
            send(f);
            check_trys++;
            state = udp_state.disconnected;
        }
        #endregion

        #region handle frame stack       
        /// <summary>
        /// neuen frame von stack holen und über tcp client versenden
        /// </summary>
        /// <returns></returns>
        private bool takeFrameFromStack(Frame f)
        {
            if (_fstack.IsEmpty)
                return false;
            else
                if (_fstack.TryDequeue(out f))
                {
                    _fstackLog.Enqueue(f);
                    return true;
                }
                else
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
            if(!_fstack.IsEmpty)
                foreach (Frame frame in _fstack)
                    if (frame.isEqualExeptIndex(f))
                    {
                        f.ChangeState(FrameWorkingState.error_double, "Frame already in send buffer");
                        _fstackLog.Enqueue(f);
                        _FrmMain.logSendRcv(f);
                        return false;
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

        public int fstackLogCount()
        {
            if (_fstackLog != null)
                return _fstackLog.Count;
            else
                return 0;
        }

        /// <summary>
        /// gibt alle frames im stack als string aus
        /// </summary>
        public string GetStackAsString() {

            string s = String.Empty;
            foreach (Frame f in _fstack)
                s += f.GetDetailedString() + Environment.NewLine;
            return s;
        }

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
                    DateTime range_time = DateTime.Now;
                    range_time.AddSeconds(5);

                    foreach (Frame f in _fstack)
                    {
                        if (range_time > f.LastSendDateTime)
                            if (f.SendTrys <= MAXSendTrys)
                            {
                                
                                f.SendTrys++;
                                f.ChangeState(FrameWorkingState.warning_resend, "repeat send: (" + f.SendTrys.ToString() + ")");
                                f.LastSendDateTime = DateTime.Now;
                                _FrmMain.logSendRcv(f);
                                
                                _udp_client.send(f);
                            }
                            else
                            {
                                f.ChangeState(FrameWorkingState.error_sendTrysLimit, "stop sending at try: (" + f.SendTrys.ToString() + ")");                               
                                _fstackLog.Enqueue(f);
                                takeFrameFromStack(f);
                                _FrmMain.logSendRcv(f);
                            }
                    }
                    
                }
                Thread.Sleep(100);
            }

        }
        #endregion
        #endregion
    }
}
