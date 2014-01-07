using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KerbalScriptEngine
{
    public class Interpreter
    {
        List<ScriptProcess> processes;
        int currentProcess;

        Dictionary<string, Value> globalVariables;

        public Interpreter()
        {
            processes = new List<ScriptProcess>();
            globalVariables = new Dictionary<string, Value>();
            currentProcess = 0;
        }

        public void ExecuteThread()
        {
            if (processes.Count < currentProcess)
            {
                processes[currentProcess].ExecuteNext();
                currentProcess++;
            }
            else
            {
                currentProcess = 0;
            }
        }

        public bool HasVariable(string name)
        {
            return globalVariables.ContainsKey(name);
        }

        public bool TryGetVariable(string name, out Value value)
        {
            if (globalVariables.ContainsKey(name))
            {
                value = globalVariables[name];
                return true;
            }
            else
            {
                value = Value.NullValue;
                return false;
            }
        }

        public Value GetVariable(string name)
        {
            if (HasVariable(name))
                return globalVariables[name];
            else
                return Value.NullValue;
        }

        public void SetVariable(string name, Value val)
        {
            if (HasVariable(name))
                globalVariables[name] = val;
            else
                globalVariables.Add(name, val);
        }

        public bool MakeThread(string file)
        {
            if (File.Exists(file))
            {
                processes.Add(new ScriptProcess(File.ReadAllLines(file), this));
                return true;
            }
            return false;
        }

        public bool MakeThread(string[] lines)
        {
            processes.Add(new ScriptProcess(lines, this));
            return true;
        }

        public void MemoryDump()
        {
            foreach (ScriptProcess t in processes)
            {
                t.Dump();
            }
        }
    }
}
