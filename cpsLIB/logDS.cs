using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cpsLIB
{

    [Serializable]
    class logDS
    {
        msg_type _msg_type = msg_type.undef;
        DateTime _datetime = DateTime.MinValue;
        string _msg = String.Empty;

        public logDS(msg_type msg_type, string msg)
        {
            _msg_type = msg_type;
            _datetime = DateTime.Now;
            _msg = msg;
        }
        public logDS(string msg)
        {
            _msg = msg;
        }

        public string ToString(bool datetime, bool error, bool warning, bool info, bool undef)
        {
            string s = String.Empty;

            if ((this._msg_type.Equals(msg_type.error) && error) || (this._msg_type.Equals(msg_type.warning) && warning) ||
                (this._msg_type.Equals(msg_type.info) && info) || (this._msg_type.Equals(msg_type.undef) && undef))
            {
                if (datetime)
                    s += "(" + _datetime.ToString() + ") ";
                s += _msg + Environment.NewLine;
            }
            return s;
        }

        public msg_type get_msg_type()
        {
            return _msg_type;
        }
    }
}
