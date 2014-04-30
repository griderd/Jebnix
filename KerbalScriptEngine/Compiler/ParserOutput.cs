using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.Types;

namespace KerboScriptEngine.Compiler
{
    partial class Parser
    {
        List<JObject>[] segments;
        Dictionary<string, string> debugSymbols;

        int lockCount;
        int loopCount;
        int branchCount;

        private void InitializeSegments()
        {
            segments = new List<JObject>[3];
            segments[0] = new List<JObject>();
            segments[1] = new List<JObject>();
            segments[2] = new List<JObject>();

            debugSymbols = new Dictionary<string, string>();
        }

        enum Segment
        {
            Code,
            Locks,
            Data
        }

        private int GetNextAddress(Segment segment)
        {
            return segments[(int)segment].Count;
        }

        private void AddTo(Segment segment, JObject value)
        {
            segments[(int)segment].Add(value);
        }

        private void AddTo(Segment segment, Instructions instruction)
        {
            AddTo(segment, new Instruction(instruction));
        }

        private void AddDebugSymbol(string internalSymbol, string externalSymbol)
        {
            debugSymbols.Add(internalSymbol, externalSymbol);
        }

        /// <summary>
        /// Generates a new lock label and adds it to the appropriate lists. Returns the full label name.
        /// </summary>
        /// <param name="variableName">Variable associated with the lock.</param>
        /// <returns></returns>
        private string CreateLockLabel(string variableName)
        {
            string lbl = CreateLabel(variableName + "_" + lockCount.ToString(), Segment.Locks);
            if (!lockLabels.ContainsKey(variableName))
                lockLabels.Add(variableName, lbl);
            lockCount++;
            return lbl;
        }
    }
}
