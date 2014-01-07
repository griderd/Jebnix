using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerbalScriptEngine
{
    class ExecutionState
    {
        public enum Status
        {
            Normal,
            IfStatement,
            ElseStatement,
            UntilLoop,
            WhileLoop,
            WhenBlock
        }


    }
}
