using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Types.BasicTypes
{
    public class JInteger : JObject<int>
    {
        public new const string TYPENAME = "Integer";

        public JInteger(int value)
            : base(value, false, TYPENAME) { }

        public override bool IsNull
        {
            get { return false; }
        }

        public override int Value
        {
            get { return value; }
            set
            {
                this.value = value;
            }
        }

        public static implicit operator JInteger(int x)
        {
            return new JInteger(x);
        }

        public static implicit operator int(JInteger x)
        {
            return x.value;
        }

        public static explicit operator JInteger(JFloat x)
        {
            return new JInteger((int)x.Value);
        }

        public static implicit operator JInteger(JString x)
        {
            JInteger result = 0;
            if (result.FromString(x))
                return result;
            else
                return x;
        }

        public virtual bool FromString(JString value)
        {
            if (value == null)
                throw new ArgumentNullException();
            if (value.IsNull)
                throw new NullJObjectException();

            return int.TryParse(value, out this.value);
        }

        public static bool TryParse(string value, out JInteger result)
        {
            int x;
            bool b = int.TryParse(value, out x);

            result = b ? x : 0;
            return b;
        }
        
        public override string ToString()
        {
            return value.ToString();
        }

        public string ToString(string format)
        {
            return value.ToString(format);
        }

        public static JInteger operator +(JInteger a, JInteger b)
        {
            return a.value + b.value;
        }

        public static JInteger operator -(JInteger a, JInteger b)
        {
            return a.value - b.value;
        }

        public static JInteger operator *(JInteger a, JInteger b)
        {
            return a.value * b.value;
        }

        public static JInteger operator /(JInteger a, JInteger b)
        {
            if (a != 0 & b != 0)
                return a.value / b.value;
            else
                throw new DivideByZeroException();
        }

        public static JInteger operator %(JInteger a, JInteger b)
        {
            if (a != 0 & b != 0)
                return a.value % b.value;
            else
                throw new DivideByZeroException();
        }

        public static bool operator ==(JInteger a, JInteger b)
        {
            return (a.value == b.value);
        }

        public static bool operator !=(JInteger a, JInteger b)
        {
            return (a.value != b.value);
        }

        public static JInteger operator &(JInteger a, JInteger b)
        {
            return a.value & b.value;
        }

        public static JInteger operator |(JInteger a, JInteger b)
        {
            return a.value | b.value;
        }

        public static JInteger operator ~(JInteger a)
        {
            return ~a.value;
        }

        public static JInteger operator ^(JInteger a, JInteger b)
        {
            return a.value ^ b.value;
        }

        public static JInteger operator ++(JInteger a)
        {
            return a.value + 1;
        }

        public static JInteger operator --(JInteger a)
        {
            return a.value - 1;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
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
    }
}
