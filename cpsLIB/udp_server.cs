using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace cpsLIB
{
    class udp_server
    {
        net_udp _sender;
        Thread _srvThread;
        UdpClient listener = null;
        int _srvPort;
        private volatile bool listening;

        public udp_server(net_udp FrmMain, string port)
        {
            this.listening = false;
            _sender = FrmMain;
            if(!int.TryParse(port, out _srvPort))
                _sender.server_message("udp_server -> error convert Port to int: " + port);
            //_srvPort = Convert.ToInt32(port);
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
            Thread.Sleep(500); //Verzögerung: Beim Tool starten wird erste logmeldung sporadisch zu früh an gui gesendet
            try
            {
                listener = new UdpClient(_srvPort);
            }
            catch (SocketException)
            {
                _sender.server_message("udp_server -> Exception making UdpClient @Port: " + _srvPort);
            }

            if (listener != null)
            {
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, _srvPort);

                try
                {
                    _sender.server_message("udp_server receive -> waiting for udp MESSAGE @ port: " + _srvPort);
                    while (this.listening)
                    {
                        byte[] bytes = listener.Receive(ref groupEP);
                        //_FrmMain.srv_msg("udp_server receive MESSAGE from: " + groupEP.Address.ToString() + ":" + groupEP.Port.ToString());

                        if (bytes == null || bytes.Length == 0)
                            _sender.server_message(groupEP.Address.ToString() + ":" + groupEP.Port.ToString() + 
                                "udp_server receive ########### EMPTY MESSAGE ################# ");

                        Frame f = new Frame(bytes, groupEP.Address.ToString(), groupEP.Port.ToString());
                        //im konstruktor für empfangs frames gibt es keine Log Liste
                        //f.ChangeState(FrameWorkingState.received, "udp server received new frame");
                        _sender.receive(f);
                    }
                }
                catch (System.Net.Sockets.SocketException se)
                {
                    if(!se.ToString().Contains("Ein Blockierungsvorgang wurde durch einen Aufruf von WSACancelBlockingCall unterbrochen"))
                        _sender.server_message("udp_server receive SocketException: " + se.ToString()); 
                }
                catch (Exception e)
                {
                    _sender.server_message("udp_server receive EXEPTION: " + e.ToString());
                }
                finally
                {
                    listener.Close();
                    _sender.server_message("STOP udp_server");
                }
            }
        }
    }
}
