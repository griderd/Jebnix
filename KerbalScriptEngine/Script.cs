using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.Types;
using KerboScriptEngine.Debugger;

namespace KerboScriptEngine
{
    class Script
    {
        string name;
        JObject[] bytecode;
        DebugInfo debugInfo;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public JObject[] Bytecode
        {
            get
            {
                return bytecode;
            }
        }

        public DebugInfo DebuggerInfo
        {
            get
            {
                return debugInfo;
            }
        }


        public Script(string name, JObject[] bytecode, DebugInfo debugInfo)
        {
            this.name = name;
            this.bytecode = bytecode;
            this.debugInfo = debugInfo;
        }

        public Process ToProcess()
        {
            return new Process(this);
        }
    }
}
