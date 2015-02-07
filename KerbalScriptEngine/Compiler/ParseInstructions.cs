using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.Types;

namespace KerboScriptEngine.Compiler
{
    partial class Parser
    {
        private void Pushg(Pseudopointer variable, Segment segment = Segment.Code)
        {
            AddTo(Segment.Code, Instructions.pushg);
            // TODO: Check that variable exists in scope
            AddTo(Segment.Code, variable);
        }

        /// <summary>
        /// Pushes the given literal onto the stack.
        /// </summary>
        /// <param name="literal"></param>
        private void Pushl(JObject literal, Segment segment = Segment.Code)
        {
            AddTo(Segment.Code, Instructions.pushl);
            AddTo(Segment.Code, literal);
        }

        private void Call(Pseudopointer pointer, Segment segment = Segment.Code)
        {
            AddTo(Segment.Code, Instructions.call);
            AddTo(Segment.Code, pointer);
        }

        private void Popv(Pseudopointer pointer, Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.popv);
            AddTo(segment, pointer);
        }

        private void Pop(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.pop);
        }

        private void Ret(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.ret);
        }

        private void Lok(Pseudopointer pointer, Segment segment = Segment.Code)
        {
            if (lockLabels.ContainsKey(pointer.Value))
            {
                ULok(pointer, segment);
            }
            AddTo(segment, Instructions.lok);
            string lbl = CreateLockLabel(pointer.Value);
            AddTo(segment, new Pseudopointer(lbl));
        }

        private void ULok(Pseudopointer pointer, Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.ulok);
            AddTo(segment, pointer);
            lockLabels.Remove(pointer.Value);
        }

        private void Jump(Pseudopointer pointer, Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.jmp);
            AddTo(segment, pointer);
        }

        private void Jumpt(Pseudopointer pointer, Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.jmpt);
            AddTo(segment, pointer);
        }

        private void Jumpf(Pseudopointer pointer, Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.jmpf);
            AddTo(segment, pointer);
        }

        private void Hop(Pseudopointer pointer, Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.hop);
            AddTo(segment, pointer);
        }

        private void Hopt(Pseudopointer pointer, Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.hopt);
            AddTo(segment, pointer);
        }

        private void Hopf(Pseudopointer pointer, Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.hopf);
            AddTo(segment, pointer);
        }

        private void Add(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.add);
        }

        private void Subtract(Segment segment = Segment.Code)
        {
            AddTo(Segment.Code, Instructions.sub);
        }

        private void Multiply(Segment segment = Segment.Code)
        {
            AddTo(Segment.Code, Instructions.mult);
        }

        private void Divide(Segment segment = Segment.Code)
        {
            AddTo(Segment.Code, Instructions.div);
        }

        private void Mod(Segment segment = Segment.Code)
        {
            AddTo(Segment.Code, Instructions.mod);
        }

        private void And(Segment segment = Segment.Code)
        {
            AddTo(Segment.Code, Instructions.and);
        }

        private void Or(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.or);
        }

        private void Not(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.not);
        }

        private void Equality(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.eq);
        }

        private void Inequality(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.neq);
        }

        private void GreaterThan(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.gt);
        }

        private void LessThan(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.lt);
        }

        private void GreaterThanEqualTo(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.gte);
        }

        private void LessThanEqualTo(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.lte);
        }

        private void Positive(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.pos);
        }

        private void Negative(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.neg);
        }

        private void Pow(Segment segment = Segment.Code)
        {
            AddTo(segment, Instructions.pow);
        }

        /// <summary>
        /// Generates a label and returns it.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        private string MakeLabel(string label, Segment segment)
        {
            StringBuilder lbl = new StringBuilder();
            if (segment == Segment.Code)
                lbl.Append("CODE_");
            else if (segment == Segment.Data)
                lbl.Append("DATA_");
            else if (segment == Segment.Locks)
                lbl.Append("LOCKS_");

            lbl.Append(label);
            return lbl.ToString();
        }

        /// <summary>
        /// Generates a label and adds the label to the pointer dictionary.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        private string CreateLabel(string label, Segment segment)
        {
            string lbl = MakeLabel(label, segment);
            AddLabel(lbl, segment);
            return lbl;
        }

        /// <summary>
        /// Adds the given label to the pointer dictionary.
        /// </summary>
        /// <param name="label"></param>
        private void AddLabel(string label, Segment segment)
        {
            labelPointers.Add(label, segments[(int)segment].Count);
        }
    }
}
