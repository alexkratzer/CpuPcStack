﻿using System;
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
    public class CpsNet
    {
        //flags to control all connections
        public Int16 MaxSYNCResendTrys = 3; //Anzahl der erlaubten Wiederholungen bei SYNC Telegram
        public Int16 WATCHDOG_WORK = 2000; //Erlaubte Zeitdauer in ms bis PLC geantwortet haben muss
        public bool SendFramesCallback = false; //es werden die "zu sendenden frames" als callback zurückgeliefert
        public bool SendOnlyIfConnected = false; 

        //private vars
        private static IcpsLIB _FrmMain;
        private udp_server _udp_server;
        //private udp_client _udp_client;
        System.Collections.Concurrent.ConcurrentQueue<Frame> _fstack = null;
        private List<Frame> LFrameRcv;

        private List<CpsClient> ListConnection = new List<CpsClient>();

        //TODO: alle frames in liste speichern
        //System.Collections.Concurrent.ConcurrentQueue<Frame> _fstackLog = null;

        //connection parameter
        public int TotalFramesFinished = 0; //Frames die auf eine anfrage hin empfangen wurden und verarbeitet werden können
        public int TotalFramesSend = 0; //send frame count
        public TimeSpan TimeRcvAnswerMin = TimeSpan.MaxValue;
        public TimeSpan TimeRcvAnswerMax = TimeSpan.MinValue;
 
        //Constructor
        public CpsNet(IcpsLIB FrmMain)
        {
            _FrmMain = FrmMain;
            _fstack = new System.Collections.Concurrent.ConcurrentQueue<Frame>();
            LFrameRcv = new List<Frame>();
            StackWorker();
        }
  
        #region client
        //public string GetClients()
        //{
        //    string s = "";
        //    foreach (CpsClient cc in ListConnection)
        //        s += cc.ToDetailedString() + Environment.NewLine;
        //    return s;
        //}

        public CpsClient newClient(string ip, string port)
        { 
                CpsClient client = new CpsClient(ip, port);
                ListConnection.Add(client);
                return client;
        }

        public void send(Frame f)
        {
            if (ConnectVerifyState(f, udp_state.connected) || f.GetHeaderFlag(FrameHeaderFlag.SYNC))
            {
                f.ChangeState(FrameWorkingState.inWork, "Frame put to Stack");
                _fstack.Enqueue(f);
                TotalFramesSend++;
                f.client.send(f);
            }
            else
                f.ChangeState(FrameWorkingState.error, "Remote udp_state NOT connected - NO Frame is send");

            //der App wird mitgeteilt das dieses frame verschickt wurde
            if (SendFramesCallback)
                _FrmMain.interprete_frame(f);
        }

        /// <summary>
        /// sends sync frame to plc
        /// </summary>
        /// <param name="cc">CpsClient (ip/port)</param>
        /// <returns>ListConnection.Count</returns>
        public int send_SYNC(CpsClient cc)
        {
            foreach (CpsClient listCC in ListConnection)
            {
                if (listCC.IsEqual(cc))
                {
                    Frame f = new Frame(listCC);
                    f.SetHeaderFlag(FrameHeaderFlag.SYNC);
                    send(f);
                    listCC.state = udp_state.unknown;
                    return ListConnection.Count;
                }
            }

            //no connection found -> make new one
            ListConnection.Add(cc);
            //send_SYNC(cc); 
            Frame fe = new Frame(cc);
            fe.SetHeaderFlag(FrameHeaderFlag.SYNC);
            send(fe);
            cc.state = udp_state.unknown;

            return ListConnection.Count;
        }

        private bool ConnectVerifyState(Frame f, udp_state matchState)
        {
            if (SendOnlyIfConnected)
            {
                if (ListConnection.Count > 0)
                {
                    foreach (CpsClient cs in ListConnection)
                    {
                        if(cs.IsEqual(f.client))
                        //if (cs.ip.Equals(f.RemoteIp) && cs.iport == f.RemotePort)
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
        public void serverSTART(int port)
        {
            //thread sleep if serverStart is called at gui construktor
            Thread.Sleep(500);
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

        public void receive(Frame f)
        {
            SetStateConnected(f);

            //remove frame from "InWork Jobs" 
            if (!_fstack.IsEmpty)
            {
                foreach (Frame frameStack in _fstack)
                    if (frameStack.GetIndex() == f.GetIndex())
                    {
                        //+++++++++++++++++ matching rcv frame to frame in stack +++++++++++++++++++
                        if (takeFrameFromStack(frameStack))
                        {
                            TotalFramesFinished++;
                            frameStack.TimeRcvAnswer = f.TimeCreated - frameStack.TimeCreated;
                            if (frameStack.TimeRcvAnswer > TimeRcvAnswerMax)
                                TimeRcvAnswerMax = frameStack.TimeRcvAnswer;
                            if (frameStack.TimeRcvAnswer < TimeRcvAnswerMin)
                                TimeRcvAnswerMin = frameStack.TimeRcvAnswer;

                            f.ChangeState(FrameWorkingState.finish, "ack received - drop this one [time: " + frameStack.TimeRcvAnswer.Milliseconds + "]");
                        }
                        else
                            f.ChangeState(FrameWorkingState.error, "ERROR dequeue Frame from stack... ");

                        frameStack.AnswerFrame = f;
                        break;
                    }
            }
            else
            {
                f.ChangeState(FrameWorkingState.error, "received udp frame without request -> _fstack.IsEmpty");
                server_message("received udp frame without request -> _fstack.IsEmpty");
            }

            //put received frame in list 
            LFrameRcv.Add(f);

            //received frame will be passed to the main application
            _FrmMain.interprete_frame(f);
        }

        /// <summary>
        /// frame received -> connection to frame is set to: connected
        /// </summary>
        /// <param name="f"></param>
        private void SetStateConnected(Frame f)
        {
            foreach (CpsClient cs in ListConnection)
                if (cs.RemoteIp == f.client.RemoteIp) //hier wichtig das nur die ip verglichen wird. port ist unterschiedlich
                {
                    cs.state = udp_state.connected;
                    return;
                }

            //wenn keine connection zu dem frame gefunden wurde wird fehler gemeldet
            f.ChangeState(FrameWorkingState.warning, "no connection found to: " + f.client + " make new one!");
            //TODO: könnte hier eine neue connection anlegen....

            //if(var.make_connection_OnRcvUnknownFrame)
            ListConnection.Add(new CpsClient(f.client.RemoteIp,f.client.RemotePortStr));

        }

        #endregion

        #region getter
        public string GetSendFrames() { 
        
            string s = "";
            foreach (Frame f in LFrameRcv)
                s += f.ToString() + Environment.NewLine;
            return s;
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
        /// </summary>
        /// <param name="f"></param>
        //private bool putFrameToStack(Frame f)
        //{
        //    /// funktionalität entfernt -> wenn benötigt muss im frame header der index maskiert werden

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
                            if (f.GetHeaderFlag(FrameHeaderFlag.SYNC) )
                            {
                                if (f.SendTrys < MaxSYNCResendTrys)
                                {
                                    f.SendTrys++;
                                    f.LastSendDateTime = DateTime.Now;
                                    f.ChangeState(FrameWorkingState.warning, "repeat send: (" + f.SendTrys.ToString() + ")");
                                    f.client.send(f); //TODO: return bool auswerten
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

        #region cleanup
        public void cleanup() {
            serverSTOP();
            Thread.Sleep(100);
        }
        #endregion


    }
}
