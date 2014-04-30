using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.Types;

namespace KerboScriptEngine.Compiler
{
    class Instruction : JObject<Instructions>
    {
        public new const string TYPENAME = "Instruction";

        public Instruction(Instructions instr)
            : base(instr, false, "Instruction")
        {
        }

        public override Instructions Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        protected override bool IsEqual(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override bool IsNotEqual(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject IsLessThan(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject IsLessThanOrEqual(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject IsGreaterThan(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject IsGreaterThanOrEqual(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Addition(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Subtract(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Multiply(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Divide(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Modulus(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Pow(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject And(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Or(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Positive()
        {
            throw new NotImplementedException();
        }

        protected override JObject Negative()
        {
            throw new NotImplementedException();
        }

        protected override JObject Not()
        {
            throw new NotImplementedException();
        }

        protected override JObject Increment()
        {
            throw new NotImplementedException();
        }

        protected override JObject Decrement()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Enum.GetName(typeof(Instructions), value);
        }
    }
}
