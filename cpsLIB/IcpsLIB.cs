using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cpsLIB
{
    public interface IcpsLIB
    {
        void interprete_frame(object o);
        void logSendRcv(object o);
        void logMsg(string msg);
    }
}
