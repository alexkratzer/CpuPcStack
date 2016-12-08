using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace cpsLIB {
    public enum prio {info, warning, error, fatal_error}
    public class log {      
        private DateTime timestamp;
        private prio prio;
        private Frame f;
        private string msg;

        public log(prio _prio, string _msg)
        {
            this.prio = _prio;
            this.msg = _msg;
            this.timestamp = DateTime.Now;
        }

        public log(prio _prio, string _msg, Frame _f)
        {
            this.prio = _prio; 
            this.f = _f;
            this.msg = _msg;
            this.timestamp = DateTime.Now;
        }

        //public override string ToString()
        //{
        //    return "[" + msg + "]";
        //}


        public String Timestamp
        {
            get { return timestamp.ToString("hh:mm:ss.fff"); }
            //set { timestamp = value; }
        }

        public string Msg {
            get { return msg; }
            set { msg = value; }
        }

        public Frame F
        {
            get { return f; }
            set { f = value; }
        }
        public prio Prio
        {
            get { return prio; }
            set { prio = value; }
        }
        public string Header
        {
            get
            {
                if (f != null)
                    return f.ShowHeaderAsByteArry();
                else
                    return "- - -";
            }
        }
        public string Payload {
          
            get {
                if (f != null)
                    return f.ShowFrameAsByteArry();
                else
                    return "- - -";
            }
        }
        public string Key
        {
            get
            {
                if (f != null)
                    return f.GetKey();
                else
                    return "- - -";
            }
        }
    }
}

/*
namespace cpsLIB
{
    public class log_NOT_USED

    {
        private const string path = "log.txt";
        public static void msg(Object o, String msg)
        {
            String log = DateTime.Now.ToString() + msg + Environment.NewLine;
            File.AppendAllText(path, log);
        }
        public static void exception(Object o, String msg, Exception ex)
        {
            String log = DateTime.Now.ToString() + " +++ EXCEPTION +++ [" + o.ToString() + "] " + msg + " -> " + ex.ToString() + Environment.NewLine;
            File.AppendAllText(path, log);
        }
        public static void clear()
        {
            if (File.Exists(path))
                File.Delete(path);
        }
        public static string FilePath()
        {
            return path;
        }
    }
}

*/