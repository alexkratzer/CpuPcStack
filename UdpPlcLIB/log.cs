﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UdpPlcLIB
{
    public class log
    {
        private const string path = "log.txt";
        public static void msg(Object o, String msg)
        {
            String log = DateTime.Now.ToString("MM-dd HH.mm.ss.fff") +/* " [" + o.ToString() + "] " +*/ msg + Environment.NewLine;
            File.AppendAllText(path, log);
        }
        public static void exception(Object o, String msg, Exception ex)
        {
            String log = DateTime.Now.ToString("MM-dd HH.mm.ss.fff") + " +++ EXCEPTION +++ [" + o.ToString() + "] " + msg + " -> " + ex.ToString() + Environment.NewLine;
            File.AppendAllText(path, log);
        }
        public static void clear() {
            if (File.Exists(path))
                File.Delete(path);
        }
        public static string FilePath() {
            return path;
        }
    }
}