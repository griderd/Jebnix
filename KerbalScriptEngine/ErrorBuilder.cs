using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KerboScriptEngine.Compiler;

namespace KerboScriptEngine
{
    class ErrorBuilder
    {
        public enum ErrorType
        {
            RuntimeError,
            SyntaxError,
            InternalError
        }

        public static void BuildError(Token token, string message, ref List<string> errList)
        {
            BuildError(token.FileName, token.LineNumber, ErrorType.SyntaxError, message, ref errList);
        }
        
        public static void BuildError(string filename, int lineNumber, ErrorType type, string message, ref List<string> errlist)
        {
            StringBuilder s = new StringBuilder();

            switch (type)
            {
                case ErrorType.InternalError:
                    s.Append("Internal error"); break;

                case ErrorType.RuntimeError:
                    s.Append("Runtime error"); break;

                case ErrorType.SyntaxError:
                    s.Append("Syntax error"); break;
            }

            s.Append(" (");
            s.Append(filename);
            s.Append(":");
            s.Append(lineNumber);
            s.Append("): ");
            s.Append(message);

            errlist.Add(s.ToString());
        }

        public static void BuildError(string filename, int lineNumber, ErrorType type, Exception ex, ref List<string> errlist)
        {
            BuildError(filename, lineNumber, type, "(" + ex.ToString() + ") " + ex.Message, ref errlist);
        }
    }
}
