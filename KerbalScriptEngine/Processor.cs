using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix;
using System.IO;
using KerboScriptEngine.Compiler;
using Jebnix.stdlib;

namespace KerboScriptEngine
{
    class Processor : Interpreter
    {
        Queue<Process> processes;


        public Processor(DirectoryInfo archiveFolder)
            : base(archiveFolder)
        {
            processes = new Queue<Process>();
            UtilityModules.SystemAPI.Initialize(this);
        }

        public override int CreateProcess(string file)
        {
            string[] err;
            if (CurrentFolder.ContainsFile(file))
            {
                Script s = Compiler.KSPPCompiler.Compile(CurrentFolder.GetFile(file), out err);
                if (err.Length != 0)
                {
                    foreach (string e in err)
                    {
                        stdio.PrintLine(e);
                    }
                }
                else
                {
                    processes.Enqueue(s.ToProcess());
                }
            }
            else
            {
                stdio.PrintLine("File \"" + file + "\" not found.");
            }
            return 0;
        }

        public override int CreateProcess(string[] lines, string filename)
        {
            string[] err;
            Script s = Compiler.KSPPCompiler.Compile(lines, filename, out err);
            if (err.Length != 0)
            {
                foreach (string e in err)
                {
                    stdio.PrintLine(e);
                }
            }
            else
            {
                processes.Enqueue(s.ToProcess());
            }
            return 0;
        }

        public override void ExecuteProcess()
        {
            if (processes.Count > 0)
            {
                Process currentProcess = processes.Dequeue();
                currentProcess.RunCycle();
                processes.Enqueue(currentProcess);
            }
        }

        public override bool HasVariable(string name)
        {
            throw new NotImplementedException();
        }

        public override void MemoryDump()
        {
            throw new NotImplementedException();
        }

        public override string GetInterpreterVersion()
        {
            throw new NotImplementedException();
        }
    }
}
