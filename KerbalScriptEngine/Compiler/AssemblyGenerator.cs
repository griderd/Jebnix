using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.Types;

namespace KerboScriptEngine.Compiler
{
    partial class Parser
    {
        public string[] GenerateAssembly()
        {
            List<string> asm = new List<string>();
            string[] lbl = labelPointers.Keys.ToArray();
            int[] lblp = labelPointers.Values.ToArray();

            asm.Add("SEGMENT CODE");
            for (int i = 0; i < segments[0].Count; i++)
            {
                for (int j = 0; i < lbl.Length; i++)
                {
                    if ((lblp[j] == i) && (lbl[j].StartsWith("CODE")))
                        asm.Add(lbl[j] + ":");
                }

                StringBuilder line = new StringBuilder();
                line.Append("\t");
                line.Append(segments[0][i].ToString());
                if (segments[0][i + 1].ObjectType != Instruction.TYPENAME)
                {
                    i++;
                    line.Append(" ");
                    line.Append(segments[0][i].ToString());
                }
                asm.Add(line.ToString());
                line.Clear();
            }

            asm.Add("");

            asm.Add("SEGMENT LOCKS");
            for (int i = 0; i < segments[1].Count; i++)
            {
                for (int j = 0; i < lbl.Length; i++)
                {
                    if ((lblp[j] == i) && (lbl[j].StartsWith("LOCKS")))
                        asm.Add(lbl[j] + ":");
                }

                StringBuilder line = new StringBuilder();
                line.Append("\t");
                line.Append(segments[1][i].ToString());
                if (segments[1][i + 1].ObjectType != Instruction.TYPENAME)
                {
                    i++;
                    line.Append(" ");
                    line.Append(segments[1][i].ToString());
                }
                asm.Add(line.ToString());
                line.Clear();
            }

            asm.Add("");
            asm.Add("SEGMENT DATA");
            for (int i = 0; i < variableNames.Count; i++)
            {
                asm.Add("def " + variableNames[i]);
            }

            return asm.ToArray();
        }
    }
}
