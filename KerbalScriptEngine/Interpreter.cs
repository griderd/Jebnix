using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace KerboScriptEngine
{
    public class Interpreter : Jebnix.IInterpreter
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

        public string GetInterpreterVersion()
        {
            System.Version v =  Assembly.GetExecutingAssembly().GetName().Version;
            return "KerboScript++ Interpreter v" + v.Major + "." + v.Minor + "." + v.Revision + " Build " + v.Build;
        }

        public void ExecuteProcess()
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

        public bool CreateProcess(string file)
        {
            if (File.Exists(file))
            {
                processes.Add(new ScriptProcess(ConvertToLineInfo(File.ReadAllLines(file), file), this));
                return true;
            }
            return false;
        }

        public bool CreateProcess(string[] lines, string filename)
        {
            processes.Add(new ScriptProcess(ConvertToLineInfo(lines, filename), this));
            return true;
        }

        private LineInfo[] ConvertToLineInfo(string[] lines, string filename)
        {
            List<LineInfo> l = new List<LineInfo>();

            for (int i = 0; i < lines.Length; i++)
            {
                l.Add(new LineInfo(lines[i], filename, i + 1, 0, null));
            }
            return l.ToArray();
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
