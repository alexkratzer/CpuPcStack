using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace cpsLIB
{
    class NOTUSEDudp_client
    {
        Thread _clientThread;
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
                f.ChangeState(FrameWorkingState.send, "send udp frame @: " + f.client);
                udpClient.Send(f.GetByteArray(), f.GetByteArray().Length, f.client.RemoteIp, f.client.RemotePort);
                udpClient.Close();
            }
            catch (Exception e) {
                f.ChangeState(FrameWorkingState.error, "Exception send Frame: " + e.Message);
            }
        }
    }
}
