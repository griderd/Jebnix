using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerbalScriptEngine
{
    class ReservedWords
    {
        public static string[] Operators 
        {
            get
            {
                return new string[] { "+", "-", "*", "/", "%", "^", "&", "and", "|", "or", "!", "not", "~", "++", "--" };
            }
        }

        public static string[] Keywords
        {
            get
            {
                return new string[] { "set",
                                      "declare", 
                                      "parameter",
                                      "local", 
                                      "clearscreen", 
                                      "print" };
            }
        }

        public static bool IsReserved(string s)
        {
            return (Operators.Contains(s) & Keywords.Contains(s));
        }

        public static bool IsString(string s)
        {
            return s.StartsWith("\"") & s.EndsWith("\"");
        }
    }
}
