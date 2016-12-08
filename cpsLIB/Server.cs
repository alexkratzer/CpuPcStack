using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace cpsLIB
{
    class Server
    {
        CpsNet _sender;
        Thread _srvThread;
        UdpClient listener = null;
        int _srvPort;
        private volatile bool listening;

        public Server(CpsNet CpsNet, string port)
        {
            this.listening = false;
            _sender = CpsNet;
            if(!int.TryParse(port, out _srvPort))
                _sender.logMsg(new log(prio.error, "udp_server -> error convert Port to int: " + port));
            initSrv();
        }
        public Server(CpsNet FrmMain, int port)
        {
            this.listening = false;
            _sender = FrmMain;
            _srvPort = port;
            initSrv();
        }

        public void initSrv()
        {
            if (!this.listening)
            {
                _srvThread = new Thread(new ThreadStart(receive));
                _srvThread.IsBackground = true;
                this.listening = true;
                _srvThread.Start();
            }
        }

        public void stop()
        {
            this.listening = false;
            if(listener!=null)
                listener.Close();   
        }

        public void receive()
        {
            ////// TODO ########################################################################### IBS ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //Thread.Sleep(500); //Verzögerung: Beim Tool starten wird erste logmeldung sporadisch zu früh an gui gesendet

            try
            {
                listener = new UdpClient(_srvPort);
            }
            catch (SocketException)
            {
                _sender.logMsg(new log(prio.fatal_error, "udp_server -> Exception making UdpClient @Port: " + _srvPort));
            }

            if (listener != null)
            {
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, _srvPort);

                try
                {
                    _sender.logMsg(new log(prio.info, "server -> listening @port: " + _srvPort));
                    while (this.listening)
                    {
                        byte[] bytes = listener.Receive(ref groupEP);
                        //_sender.logMsg("udp_server receive MESSAGE from: " + groupEP.Address.ToString() + ":" + groupEP.Port.ToString());
                        
                        if (bytes == null || bytes.Length == 0)
                            _sender.logMsg(new log(prio.warning, groupEP.Address.ToString() + ":" + groupEP.Port.ToString() + 
                                "udp_server receive EMPTY MESSAGE"));

                        Frame f = new Frame(new Client(groupEP.Address.ToString(), groupEP.Port.ToString()), bytes );
                        f.ChangeState(FrameWorkingState.received, "udp server received new frame");
                        _sender.receive(f);
                    }
                }
                catch (System.Net.Sockets.SocketException se)
                {
                    if(!se.ToString().Contains("Ein Blockierungsvorgang wurde durch einen Aufruf von WSACancelBlockingCall unterbrochen"))
                        _sender.logMsg(new log(prio.fatal_error, "udp_server receive SocketException: " + se.ToString())); 
                }
                catch (Exception e)
                {
                    _sender.logMsg(new log(prio.fatal_error, "udp_server receive EXEPTION: " + e.ToString()));
                }
                finally
                {
                    listener.Close();
                    _sender.logMsg(new log(prio.fatal_error, "STOP udp_server"));
                }
            }
        }
    }
}
