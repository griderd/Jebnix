using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerboScriptEngine
{
    class ExecutionFrame
    {
        public LineInfo Line { get; private set; }

        public ExecutionFrame(string condition, LineInfo line)
        {
            Line = new LineInfo(condition, line.Filename, line.LineNumber, line.ColumnOffset, line.Process);
        }
    }
}
