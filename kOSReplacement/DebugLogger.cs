using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Jebnix
{
    public class DebugLogger
    {
        static Queue<string> values;

        public static bool Initialized { get; private set; }

        public static void Initialize()
        {
            values = new Queue<string>();
            Initialized = true;
        }

        public static void Log(string s)
        {
            CheckInit();
            Debug.WriteLine(s);
            values.Enqueue(s);
        }

        public static void Log(Exception ex)
        {
            CheckInit();
            Debug.WriteLine(ex);
            values.Enqueue(ex.Message + "\n" + ex.StackTrace);
        }

        public static string GetNextLog()
        {
            CheckInit();
            if (values.Count > 0) return values.Dequeue();
            else return "";
        }

        private static void CheckInit()
        {
            if (!Initialized) throw new Exceptions.JebnixClassNotInitializedException("DebugLogger");
        }
    }
}
