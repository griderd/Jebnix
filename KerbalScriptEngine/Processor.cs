using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Jebnix;
using System.IO;
using KerboScriptEngine.Compiler;
using Jebnix.stdlib;
using Jebnix.Types;
using Jebnix.Types.Structures;

namespace KerboScriptEngine
{
    public class Processor : Interpreter
    {
        Queue<Process> processes;
        Queue<Process> inputQueue;

        internal List<JObject> globalHeap;
        internal List<string> globalSymbols;

        public Process CurrentProcess
        {
            get;
            private set;
        }

        public Processor(Vessel vessel, DirectoryInfo archiveFolder)
            : base(vessel, archiveFolder)
        {
            processes = new Queue<Process>();
            UtilityModules.SystemAPI.Initialize(this);

            InitializeHeap();
        }

        public Processor(DirectoryInfo archiveFolder)
            : base(archiveFolder)
        {
            processes = new Queue<Process>();
            UtilityModules.SystemAPI.Initialize(this);

            InitializeHeap();
        }

        private void InitializeHeap()
        {
            globalHeap = new List<JObject>();
            globalSymbols = new List<string>();

            try
            {
                if (vessel != null)
                {
                    AddToHeap("vesselname", new JString(vessel.vesselName));
                    AddToHeap("altitude", new JFloat(vessel.altitude));
                    // TODO: Add ALT structure
                    // TODO: Add BODY structure
                    AddToHeap("missiontime", new JFloat(vessel.missionTime));
                    AddToHeap("velocity", new Vector(FlightGlobals.ship_obtVelocity));
                    AddToHeap("verticalspeed", new JFloat(vessel.verticalSpeed));
                    AddToHeap("surfacespeed", new JFloat(vessel.horizontalSrfSpeed));
                    AddToHeap("latitude", new JFloat(vessel.latitude));     // TODO: fix latitude/longitude bug
                    AddToHeap("longitude", new JFloat(vessel.longitude));
                    // TODO: Add STATUS enum
                    // TODO: Add INLIGHT
                    // TODO: Add INCOMMRANGE
                    // TODO: Add COMMRANGE
                    AddToHeap("mass", new JFloat(vessel.GetTotalMass()));
                    // TODO: Add MAXTHRUST
                    // TODO: Add TIME structure
                    // TODO: Add PROGRADE vector
                    // TODO: Add RETROGRADE vector
                    // TODO: Add UP vector

                }
            }
            catch (System.Security.SecurityException)
            {
                stdio.PrintLine("Instance running outside of KSP.");
            }
        }

        internal void AddToHeap(string symbol, JObject value)
        {
            globalHeap.Add(value);
            globalSymbols.Add(symbol);
        }

        internal void AddToHeap(string symbol)
        {
            globalSymbols.Add(symbol);
            globalHeap.Add(new JValue());
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
                    processes.Enqueue(s.ToProcess(this));
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
                processes.Enqueue(s.ToProcess(this));
            }
            return 0;
        }

        public override void ExecuteProcess()
        {
            if (processes.Count > 0)
            {
                CurrentProcess = processes.Dequeue();
                CurrentProcess.RunCycle();
                //if (!inputQueue.Contains(CurrentProcess))
                //    processes.Enqueue(CurrentProcess);
                if (CurrentProcess.Running)
                    processes.Enqueue(CurrentProcess);
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
            System.Version v = Assembly.GetExecutingAssembly().GetName().Version;
            return "KerboScript++ " + v.Major + "." + v.Minor + " Build " + v.Revision;
        }

        internal void WaitForInput(Process p)
        {
            inputQueue.Enqueue(p);
        }

        public void SendInput(Jebnix.Types.JObject value)
        {
            if (inputQueue.Count > 0)
            {
                Process p = inputQueue.Dequeue();
                p.PushToDataStack(value);
                processes.Enqueue(p);
            }
        }
    }
}
