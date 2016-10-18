using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace cpsLIB
{
    public class CpsClient
    {
        //public Int16 check_trys = 0;
        public string RemoteIp = "";
        public int RemotePort;
        public string RemotePortStr;
        public udp_state state;
        public List<Frame> LFrame; //log all frames send over this udp connection

        public CpsClient(string ip, string port)
        {
            this.RemoteIp = ip;
            RemotePortStr = port;

            if (int.TryParse(port, out RemotePort))
                RemotePortStr = port;
            else
                RemotePortStr = "ERROR: " + port.ToString();

            state = udp_state.unknown;
            LFrame = new List<Frame>();
        }

        public override string ToString()
        {
            return RemoteIp + ":" + RemotePortStr;
        }

        public bool IsEqual(CpsClient cc) {
            if ( (RemoteIp == cc.RemoteIp) && (RemotePort == cc.RemotePort) )
                return true;
            else
                return false;
        }

        #region udp client
        Thread _clientThread;

        public void send(Frame f)
        {
            LFrame.Add(f);
            //_clientThread = new Thread(new ThreadStart(send_fkt));
            _clientThread = new Thread(()=> send_fkt(f));
            _clientThread.IsBackground = true;
            _clientThread.Start();
        }

        private void send_fkt(Frame f)
        {
            UdpClient udpClient = new UdpClient();
            try
            {
                f.ChangeState(FrameWorkingState.send, "send udp frame @: " + f.client);
                udpClient.Send(f.GetByteArray(), f.GetByteArray().Length, f.client.RemoteIp, f.client.RemotePort);
                //############################################################################################################
                //TODO: hier ist f.client nicht mehr notwendig. evtl ist referen von cpsClient in Frame nicht mehr notwendig
                udpClient.Close();
            }
            catch (Exception e)
            {
                f.ChangeState(FrameWorkingState.error, "Exception send Frame: " + e.Message);
            }
        }
        #endregion
    }
}
