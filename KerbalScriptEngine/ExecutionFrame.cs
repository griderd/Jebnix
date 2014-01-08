using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerbalScriptEngine
{
    class ExecutionFrame
    {
        public int LineNumber { get; private set; }
        public int TokenOffset { get; private set; }

        public ExecutionFrame(int lineNumber, int tokenOffset)
        {
            LineNumber = lineNumber;
            TokenOffset = tokenOffset;
        }
    }
}
