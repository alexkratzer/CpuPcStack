using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cpsLIB
{
    public class CpsClient
    {
        //public Int16 check_trys = 0;
        public string RemoteIp = "";
        public int RemotePort;
        public string RemotePortStr;
        public udp_state state;

        public CpsClient(string ip, string port)
        {
            this.RemoteIp = ip;
            RemotePortStr = port;

            if (int.TryParse(port, out RemotePort))
                RemotePortStr = port;
            else
                RemotePortStr = "ERROR: " + port.ToString();

            state = udp_state.unknown;
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
    }
}
