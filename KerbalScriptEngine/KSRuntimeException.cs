using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.Types;

namespace KerboScriptEngine
{
    public class KSRuntimeException : Exception
    {
        private Process process;
        private string[] callStack;

        public Process Process
        {
            get { return process; }
        }

        public KSRuntimeException(string message, Exception innerException, Process p)
            : base(message, innerException)
        {
            process = p;
        }

        private void UnwindStack()
        {
            List<string> stack = new List<string>();

            Stack<int> s = process.CallStack;
            while (s.Count > 0)
            {
                int frame = s.Pop() - 1;
                stack.Add("at \"" + process.SourceScript.DebuggerInfo.GetSymbolByAddress(frame));
            }
            callStack = stack.ToArray();
        }
    }
}
