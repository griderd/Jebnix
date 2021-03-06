﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using UnityEngine;
using Jebnix.FileSystem;
using KerboScriptEngine.ScriptAPI;

namespace KerboScriptEngine
{
    public class Interpreter : Jebnix.Interpreter
    {
        List<ScriptProcess> processes;
        int currentProcess;

        List<string> filesAsFunctions;

        public bool Busy
        {
            get;
            private set;
        }

        public string[] GlobalFunctions
        {
            get
            {
                List<string> funcs = new List<string>(filesAsFunctions);
                return funcs.ToArray();
            }
        }

        private void GetFilesAsFunctions()
        {
            filesAsFunctions.Clear();

            foreach (Jebnix.FileSystem.File file in CurrentFolder.Files)
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
            if ((!Busy) && (processes.Count > currentProcess))
            {
                Busy = true;
#if DEBUG
                System.Diagnostics.Debug.Print("Executing process " + currentProcess.ToString());
#endif
                processes[currentProcess].ExecuteNext();
                currentProcess++;
                if (currentProcess == processes.Count)
                    currentProcess = 0;
                Busy = false;
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
                processes[processId].AddLines(ConvertToLineInfo(lines, filename, processes[processId]));
                return true;
            }
            return false;
        }

        private LineInfo[] ConvertToLineInfo(string[] lines, string filename, ScriptProcess p = null)
        {
            List<LineInfo> l = new List<LineInfo>();

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
                if (lines[i] != "")
                    l.Add(new LineInfo(lines[i], filename, i + 1, 0, p));
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
