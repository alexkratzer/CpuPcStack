using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cpsLIB
{
    public interface IcpsLIB
    {
        void interprete_frame(object o);  //o == Frame
        //void logSendRcv(object o); //o == Frame
        void logMsg(string msg);
    }
}
