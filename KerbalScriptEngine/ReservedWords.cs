using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KerboScriptEngine
{
    class ReservedWords
    {
        private static string[][] precedence = new string[][]
        {
            new string[] {","},
            new string[] {"|", "or"},
            new string[] {"&", "and"},
            new string[] {"=", "==", "!="},
            new string[] {"<", "<=", ">", ">=" },
            new string[] {"+", "-"},
            new string[] {"*", "/", "%" },
            new string[] {"!", "~", "not", "^"},
            new string[] {"++", "--", ":"}
        };

        public static int GetPrecedence(string op)
        {
            for (int i = 0; i < precedence.Length; i++)
            {
                if (precedence[i].Contains(op)) return i;
            }
            return -1;
        }

        public static string[] Operators 
        {
            get
            {
                return new string[] { 
                    "+", "-", "*", "/", "%", "^", "&",
                    "and", "|", "or", "!", "not", "~", 
                    "++", "--", "=", "==", ">", "<", 
                    ">=", "<=", "!=" };
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

        public static bool IsValidIdentifier(string s)
        {
            return Regex.IsMatch(s, @"[a-zA-Z_]+[a-zA-Z_0-9]*(\[[0-9]+/])?");
        }
    }
}
