using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Types
{
    public class JString : JObject<string>
    {
        public new const string TYPENAME = "String";

        public JString(string value)
            : base(value, (value == null), TYPENAME) { }

        public override string Value
        {
            get { return value; }
            set
            {
                this.value = value;
            }
        }

        public JString ToLower()
        {
            return value.ToLower();
        }

        public JString ToUpper()
        {
            return value.ToUpper();
        }

        public JString Substring(int startIndex)
        {
            return value.Substring(startIndex);
        }

        public JString Substring(int startIndex, int length)
        {
            return value.Substring(startIndex, length);
        }

        public JInteger Length
        {
            get
            {
                return value.Length;
            }
        }

        public static implicit operator JString(string s)
        {
            return new JString(s);
        }

        public static implicit operator string(JString s)
        {
            return s.value;
        }

        public static explicit operator JString(JInteger x)
        {
            return new JString(x.ToString());
        }

        public static explicit operator JString(JFloat x)
        {
            return new JString(x.ToString());
        }

        public override string ToString()
        {
            return value;
        }

        public static JString operator +(JString a, JObject b)
        {
            return a.value + b.ToString();
        }

        public static bool operator ==(JString a, JString b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(JString a, JString b)
        {
            return a.Value != b.Value;
        }

        protected override bool IsEqual(JObject a)
        {
            if (!IsSameType(a))
                return false;

            JString s = (JString)a;
            return this == s;
        }

        protected override bool IsNotEqual(JObject a)
        {
            if (!IsSameType(a))
                return false;

            JString s = (JString)a;
            return this != s;
        }

        protected override JObject IsLessThan(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject IsLessThanOrEqual(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject IsGreaterThan(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject IsGreaterThanOrEqual(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Addition(JObject a)
        {
            return this + a;
        }

        protected override JObject Subtract(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Multiply(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Divide(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Modulus(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Pow(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject And(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Or(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Positive()
        {
            throw new InvalidOperationException();
        }

        protected override JObject Negative()
        {
            throw new InvalidOperationException();
        }

        protected override JObject Not()
        {
            throw new InvalidOperationException();
        }

        protected override JObject Increment()
        {
            throw new InvalidOperationException();
        }

        protected override JObject Decrement()
        {
            throw new InvalidOperationException();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
