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
    
    public enum udp_state { connected, disconnected }
    //public enum msg_type { undef, info, warning, error }
    public class CpsNet
    {
        //flags to control all connections
        public Int16 MaxSYNCResendTrys = 3; //Anzahl der erlaubten Wiederholungen bei SYNC Telegram
        public Int16 WATCHDOG_WORK = 2000; //Erlaubte Zeitdauer in ms bis PLC geantwortet haben muss
        public bool SendFramesCallback = false; //es werden die "zu sendenden frames" als callback zurückgeliefert
        public bool SendOnlyIfConnected = true; //TRUE => ohne Verbindungsaufbau über SYNC werden keine Frames gesendet

        //private vars
        private static IcpsLIB _FrmMain;
        private Server _udp_server;
        private FrmStatusLog LogFSL;
        //System.Collections.Concurrent.ConcurrentQueue<Frame> _fstack = null;

        //public da CpsClient schreibt
        public System.Collections.Concurrent.ConcurrentDictionary<string, Frame> _fstack = null;// new System.Collections.Concurrent.BlockingCollection<Frame>(100);

        private System.Collections.Concurrent.ConcurrentDictionary<string, Frame> LFrameRcv = null; //Log of all received frames
        private List<Client> ListClients = new List<Client>();

        //TODO: alle frames in liste speichern
        //System.Collections.Concurrent.ConcurrentQueue<Frame> _fstackLog = null;

        //connection parameter
                                                //Frames die auf eine anfrage hin empfangen wurden und verarbeitet werden können
        private int TotalFramesFinished = 0;    //TODO: bisher nicht verwendet da receive counter ausgewertet wird
                                             
        public int TotalFramesSend = 0; //send frame count
        public TimeSpan TimeRcvAnswerMin = TimeSpan.MaxValue;
        private TimeSpan TimeRcvAnswerMax = TimeSpan.MinValue;
        //private TimeSpan TimeRcvAvg = TimeSpan.Zero;
 
        //Constructor
        public CpsNet(IcpsLIB FrmMain, bool ModeDbg = false)
        {
            _FrmMain = FrmMain;
            _fstack = new System.Collections.Concurrent.ConcurrentDictionary<string, Frame>();
            LFrameRcv = new System.Collections.Concurrent.ConcurrentDictionary<string, Frame>();
            
            StackWorker();

            if (ModeDbg)
            {
                LogFSL = new FrmStatusLog();
                LogFSL.Show();
            }
        }

        public void logMsg(log _log)
        { 
            if(LogFSL!=null && LogFSL.Visible)
                LogFSL.AddLog(_log);

            //Ausgabe der LogMsg auch in Main Frm Log Window
            /*
            string msg = _log.Timestamp + " " + _log.Prio.ToString() + " ";
            if (_log.Msg != null)
                msg += _log.Msg;
            if (_log.F != null)
                msg += _log.F;
            _FrmMain.logMsg(msg);
             * */
        }

        public void FrmLogShow(bool visible) {
            LogFSL.Visible = visible;
        }
    
        #region client
        public Client newClient(string ip, string port)
        {
            Client client = new Client(ip, port);  
            ListClients.Add(client);
            logMsg(new log(prio.info, "make new client: " + client.ToString()));
            return client;
        }

        
//globale sendefunktion für alle clients
        public bool send(Frame f)
        {
            //send_SYNC einbauen
            //    foreach (CpsClient listCC in ListClients)
            //    {
            //        if (listCC.IsEqual(cc))
            //        {
            //            Frame f = new Frame(listCC);
            //            f.SetHeaderFlag(FrameHeaderFlag.SYNC);
            //            send(f);
            //            listCC.state = udp_state.unknown;
            //            return;
            //        }
            //    }

            if (CheckIfConnected(f)){
            //{
                f.ChangeState(FrameWorkingState.inWork, "Frame put to Stack");

            //der App wird mitgeteilt das dieses frame verschickt wurde
                if (SendFramesCallback)
                    _FrmMain.interprete_frame(f);

                //check if frame is allready on stack
                if (_fstack.ContainsKey(f.GetKey())) ;
                    //TODO: über controlls kommen hier viele frames rein
                //logMsg(new log(prio.error, "send frame which is allready on stack", f));
                else
                {
                    if (_fstack.TryAdd(f.GetKey(), f))
                    {
                        logMsg(new log(prio.info, "-> send frame @" + f.client.RemoteIp, f));
                        TotalFramesSend++;
                        f.LastSendDateTime = DateTime.Now;
                        f.client.send(f);
                        return true;
                    }
                    else
                        //logMsg("ERROR add frame to _fstack");
                        logMsg(new log(prio.error, "ERROR add frame @ _fstack", f));
                }
            }
            else
                f.ChangeState(FrameWorkingState.error, "Remote udp_state NOT connected - NO Frame is send");
            return false;

            
        }
        private bool CheckIfConnected(Frame f) {
            if (!SendOnlyIfConnected)
                return true;

            if(f.client.state.Equals(udp_state.connected) || (f.GetHeaderFlag(FrameHeaderFlag.SYNC)))            
                    return true;
            return false;
        }


        //TODO: funktionalität von send_SYNC in send einbauen
       /// <summary>
        /// sends sync frame to plc
        /// </summary>
        /// <param name="cc">CpsClient (ip/port)</param>
        /// <returns>ListConnection.Count</returns>
        //public void send_SYNC(CpsClient cc)
        //{
        //    foreach (CpsClient listCC in ListClients)
        //    {
        //        if (listCC.IsEqual(cc))
        //        {
        //            Frame f = new Frame(listCC);
        //            f.SetHeaderFlag(FrameHeaderFlag.SYNC);
        //            send(f);
        //            listCC.state = udp_state.unknown;
        //            return;
        //        }
        //    }

        //    //no connection found -> make new one
        //    ListClients.Add(cc);
        //    //send_SYNC(cc); //rekursiever aufruf -> nochmal drüber schlafen
        //    Frame fe = new Frame(cc);
        //    fe.SetHeaderFlag(FrameHeaderFlag.SYNC);
        //    fe.ChangeState(FrameWorkingState.warning, "no connection found -> make new one");
        //    send(fe);
        //    cc.state = udp_state.unknown;
        //}


        #endregion

        #region server
        public void serverSTART(string port)
        {
            _udp_server = new Server(this, port);
        }
        public void serverSTART(int port)
        {
            //thread sleep if serverStart is called at gui construktor
            //Thread.Sleep(500);
            _udp_server = new Server(this, port);
        }
        public void serverSTOP()
        {
            if (_udp_server != null)
                _udp_server.stop();
        }

        public void receive(Frame f)
        {
            logMsg(new log(prio.info,"<- receive frame", f));

            /*
            foreach (Client cs in ListClients)
                if (cs.RemoteIp == f.client.RemoteIp) //hier wichtig das nur die ip verglichen wird. port ist unterschiedlich
                {
                    cs.state = udp_state.connected;
                }

            //wenn keine connection zu dem frame gefunden wurde wird fehler gemeldet
            f.ChangeState(FrameWorkingState.warning, "no connection found to: " + f.client + " make new one!");
            logMsg(new log(prio.warning, "no connection found to: " + f.client + " make new one!", f));
             * */

            //remove frame from "InWork Jobs" 
            if (!_fstack.IsEmpty)
            {
                Frame frameStack;
                if (_fstack.TryGetValue(f.GetKey(), out frameStack))
                {
                    foreach (Client cs in ListClients)
                        if (cs.RemoteIp == f.client.RemoteIp) //hier wichtig das nur die ip verglichen wird. port ist unterschiedlich
                            cs.state = udp_state.connected;

                    TotalFramesFinished++;

                    //send/receive calc time min/max
                    f.TimeRcvAnswer = f.TimeCreated - frameStack.TimeCreated;
                    if (f.TimeRcvAnswer > TimeRcvAnswerMax)
                        TimeRcvAnswerMax = f.TimeRcvAnswer;
                    if (f.TimeRcvAnswer < TimeRcvAnswerMin)
                        TimeRcvAnswerMin = f.TimeRcvAnswer;

                    takeFrameFromStack(frameStack.GetKey());
                }
                else
                    logMsg(new log(prio.error, "TryGetValue from _fstack == FALSE  ", f));
            }
            else
            {
                f.ChangeState(FrameWorkingState.error, "received udp frame without request -> _fstack.IsEmpty");
                logMsg(new log(prio.warning, "received udp frame without request", f));
            }

            //logMsg("[- put received frame in list: " + f.ToString());
            //put received frame in list 

            if (!LFrameRcv.TryAdd(f.GetKey(), f))
                logMsg(new log(prio.error, "ERROR add frame to LFrameRcv", f));
            //logMsg(new log(prio.info, "f.TimeRcvAnswer" + f.TimeRcvAnswer, f));

            //received frame will be passed to the main application
            _FrmMain.interprete_frame(f);
        }


        #endregion

        #region getter
        public string GetStatus() {

            //send/receive calc time avg
            Int64 RcvTimeAvg = 0;
            if (LFrameRcv.Any())
            {
                    foreach (KeyValuePair<string, Frame> fList in LFrameRcv)
                    //foreach (Frame fList in LFrameRcv)
                        RcvTimeAvg += fList.Value.TimeRcvAnswer.Milliseconds;
                    RcvTimeAvg = RcvTimeAvg / LFrameRcv.Count;
                
            }

            return "[Frame min: " + TimeRcvAnswerMin.Milliseconds.ToString() + 
                " max: " + TimeRcvAnswerMax.Milliseconds.ToString() +
                " avg: " + RcvTimeAvg.ToString() + 
                " @work: " + InWorkFrameCount() + 
                //" done: " + TotalFramesFinished.ToString() +
                " send: " + TotalFramesSend + 
                " rcv: " + LFrameRcv.Count().ToString() +
                " clients: " + ListClients.Count.ToString() + 
                "]";
        }
        #endregion

        #region handle frame stack
        /// <summary>
        /// frame aus stack löschen
        /// </summary>
        /// <returns></returns>
        private bool takeFrameFromStack(string key)
        {
            Frame f;
            if (_fstack.TryRemove(key, out f))
            {
                f.ChangeState(FrameWorkingState.finish, "Dequeue frame from stack");
                //logMsg(new log(prio.info, "takeFrameFromStack", f));
                return true;
            }

            logMsg(new log(prio.error, "ERROR dequeue frame from stack... ", f));
            return false;

        }

        /// <summary>
        /// neuen frame für cpu in puffer legen
        /// wenn ein identisches frame (index wird nicht bewertet) 
        /// bereits vorhanden ist wird dieses nicht erneut abgelegt
        /// </summary>
        /// <param name="f"></param>
        //private bool putFrameToStack(Frame f)
        //{
        //    ///TODO: funktionalität entfernt -> wenn benötigt muss im frame header der index maskiert werden

        //    //if (!_fstack.IsEmpty)
        //    //    foreach (Frame frame in _fstack)
        //    //    {
        //    //        if ( (frame.getPayload() == f.getPayload()) && (f.heaheader.Equals(f.header))
        //    //        //if (frame.isEqualExeptIndex_WASTE(f))
        //    //        {
        //    //            f.ChangeState(FrameWorkingState.error, "Frame already in send buffer");
                        
        //    //            Thread.Sleep(100);
        //    //            return false;
        //    //        }
        //    //    }

        //    f.ChangeState(FrameWorkingState.inWork, "Frame put to Stack");
        //    _fstack.Enqueue(f);
        //    return true;
        //}

        public int InWorkFrameCount()
        {
            if (_fstack != null)
                return _fstack.Count;
            else
                return 0;
        }
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
                    foreach (KeyValuePair<string, Frame> f in _fstack)
                    {
                        if (f.Value.LastSendDateTime.AddMilliseconds(WATCHDOG_WORK) < DateTime.Now)
                        {
                            //hit Watchdog
                            if (f.Value.GetHeaderFlag(FrameHeaderFlag.SYNC))
                            {
                                if (f.Value.SendTrys < MaxSYNCResendTrys)
                                {
                                    f.Value.SendTrys++;
                                    f.Value.LastSendDateTime = DateTime.Now;
                                    f.Value.ChangeState(FrameWorkingState.warning, "repeat send: (" + f.Value.ToString() + ")");
                                    logMsg(new log(prio.warning, "repeat send", f.Value));
                                    f.Value.client.send(f.Value); //TODO: return bool auswerten
                                }
                                else
                                {
                                    f.Value.ChangeState(FrameWorkingState.error, "stop sending at try: (" + f.Value.SendTrys.ToString() + ")");
                                    logMsg(new log(prio.error, "stop sending at try: (" + f.Value.SendTrys.ToString() + ")", f.Value));
                                    takeFrameFromStack(f.Key);
                                }
                            }
                            else
                            {
                                f.Value.ChangeState(FrameWorkingState.error, "no answer to sendrequest");
                                logMsg(new log(prio.error, "no answer to sendrequest", f.Value));
                                takeFrameFromStack(f.Key);
                            }
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }
        #endregion

        #region cleanup
        public void cleanup() {
            serverSTOP();
            Thread.Sleep(100);
        }
        #endregion


    }
}
