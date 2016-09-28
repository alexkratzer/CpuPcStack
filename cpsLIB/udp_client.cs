using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace cpsLIB
{
    class udp_client
    {
        net_udp _sender;
        Thread _clientThread;
        
        public udp_client(net_udp FrmMain)
        {
            this._sender = FrmMain;
        }

        Frame f;
        public void send(Frame f)
        {
            this.f = f;
            _clientThread = new Thread(new ThreadStart(send_fkt));
            _clientThread.IsBackground = true;
            _clientThread.Start();
        }

        private void send_fkt()
        {
            UdpClient udpClient = new UdpClient();
            try
            {
                f.ChangeState(FrameWorkingState.send, "send udp frame @: " + f.RemoteIp + ":" +  f.RemotePort);
                udpClient.Send(f.bytes(), f.length(), f.RemoteIp, f.RemotePort);
                udpClient.Close();
            }
            catch (Exception e) {
                f.ChangeState(FrameWorkingState.error_send, "Exception send Frame: " + e.Message);
            }
            _sender.client_message(f);
        }
    }
}
