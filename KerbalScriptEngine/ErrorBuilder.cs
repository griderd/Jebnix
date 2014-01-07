using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerbalScriptEngine
{
    class ErrorBuilder
    {
        public enum ErrorType
        {
            RuntimeError,
            SyntaxError,
            InternalError
        }
        
        public static void BuildError(LineInfo line, ErrorType type, string message, ref List<string> errlist)
        {
            StringBuilder s = new StringBuilder();

            switch (type)
            {
                case ErrorType.InternalError:
                    s.Append("Internal error"); break;

                case ErrorType.RuntimeError:
                    s.Append("Runtime error"); break;

                case ErrorType.SyntaxError:
                    s.Append("Syntax error:"); break;
            }

            s.Append(" (");
            s.Append(line.Filename);
            s.Append(":");
            s.Append(line.LineNumber);
            s.Append(",");
            s.Append(line.ColumnOffset);
            s.Append(") - ");
            s.Append(message);

            errlist.Add(s.ToString());
        }

        public static void BuildError(LineInfo line, ErrorType type, Exception ex, ref List<string> errlist)
        {
            BuildError(line, type, "(" + ex.ToString() + ") " + ex.Message, ref errlist);
        }
    }
}
