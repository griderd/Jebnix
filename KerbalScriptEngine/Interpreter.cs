using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using UnityEngine;
using BIOS.FileSystem;
using KerboScriptEngine.ScriptAPI;

namespace KerboScriptEngine
{
    public class Interpreter : BIOS.Interpreter
    {
        List<ScriptProcess> processes;
        int currentProcess;

        List<string> filesAsFunctions;

        public string[] GlobalFunctions
        {
            get
            {
                List<string> funcs = new List<string>(filesAsFunctions);
                funcs.AddRange(APIInfo.FunctionNames);
                return funcs.ToArray();
            }
        }

        private void GetFilesAsFunctions()
        {
            filesAsFunctions.Clear();

            foreach (BIOS.FileSystem.File file in CurrentFolder.Files)
            {
                filesAsFunctions.Add(file.Name);
            }
        }

        public override bool HasVariable(string name)
        {
            throw new NotImplementedException();
        }

        public Interpreter(DirectoryInfo archive)
            :  base(archive)
        {
            processes = new List<ScriptProcess>();
            filesAsFunctions = new List<string>();
            currentProcess = 0;
        }

        public Interpreter(Vessel vessel, DirectoryInfo archive)
            : base(vessel, archive)
        {
            processes = new List<ScriptProcess>();
            currentProcess = 0;
        }

        public override string GetInterpreterVersion()
        {
            System.Version v =  Assembly.GetExecutingAssembly().GetName().Version;
            return "KerboScript++ Interpreter " + v.Major + "." + v.Minor + " Build " + v.Revision;
        }

        public override void ExecuteProcess()
        {
            if (processes.Count > currentProcess)
            {
                processes[currentProcess].ExecuteNext();
                currentProcess++;
                if (currentProcess == processes.Count)
                    currentProcess = 0;
            }
            else
            {
                currentProcess = 0;
            }
        }

        public override int CreateProcess(string file)
        {
            if (System.IO.File.Exists(file))
            {
                processes.Add(new ScriptProcess(ConvertToLineInfo(System.IO.File.ReadAllLines(file), file), this));
                return processes.Count - 1;
            }
            return -1;
        }

        public override int CreateProcess(string[] lines, string filename)
        {
            processes.Add(new ScriptProcess(ConvertToLineInfo(lines, filename), this));
            return processes.Count - 1;
        }

        public bool AddCodeToProcess(int processId, string[] lines, string filename)
        {
            if ((processId >= 0) && (processes.Count > processId))
            {
                processes[processId].AddLines(ConvertToLineInfo(lines, filename));
                return true;
            }
            return false;
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

        public override void MemoryDump()
        {
            foreach (ScriptProcess t in processes)
            {
                t.Dump();
            }
        }
    }
}
