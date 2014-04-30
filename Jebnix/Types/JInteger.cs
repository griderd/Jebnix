using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Types
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

        public static JInteger Parse(string value)
        {
            int x;
            if (!int.TryParse(value, out x))
                throw new InvalidCastException();
            return x;
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
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME))
                return (this == ((JInteger)a));

            return false;
        }

        protected override bool IsNotEqual(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME))
                return (this != ((JInteger)a));

            return false;
        }

        protected override JObject IsLessThan(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME))
                return (this < ((JInteger)a));

            return JBoolean.False;
        }

        protected override JObject IsLessThanOrEqual(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME))
                return (this <= ((JInteger)a));

            return JBoolean.False;
        }

        protected override JObject IsGreaterThan(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME))
                return (this > ((JInteger)a));

            return JBoolean.False;
        }

        protected override JObject IsGreaterThanOrEqual(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME))
                return (this >= ((JInteger)a));

            return JBoolean.False;
        }

        protected override JObject Addition(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME) | (a.ObjectType == JBoolean.TYPENAME))
                return (this + ((JInteger)a));
            else if (a.ObjectType == JString.TYPENAME)
                return new JString(this.ToString() + a.ToString());

            throw new InvalidOperationException();
        }

        protected override JObject Subtract(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME))
                return (this - ((JInteger)a));

            throw new InvalidOperationException();
        }

        protected override JObject Multiply(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME))
                return (this * ((JInteger)a));

            throw new InvalidOperationException();
        }

        protected override JObject Divide(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME))
                return (this / ((JInteger)a));

            throw new InvalidOperationException();
        }

        protected override JObject Modulus(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME))
                return (this % ((JInteger)a));

            throw new InvalidOperationException();
        }

        protected override JObject Pow(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME))
                return (JInteger)(Math.Pow((JFloat)this, (JFloat)a));

            throw new InvalidOperationException();
        }

        protected override JObject And(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME))
                return (this & ((JInteger)a));

            throw new InvalidOperationException();
        }

        protected override JObject Or(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JFloat.TYPENAME))
                return (this | ((JInteger)a));

            throw new InvalidOperationException();
        }

        protected override JObject Positive()
        {
            return this;
        }

        protected override JObject Negative()
        {
            return -this;
        }

        protected override JObject Not()
        {
            return !this;
        }

        protected override JObject Increment()
        {
            return this + 1;
        }

        protected override JObject Decrement()
        {
            return this - 1;
        }
    }
}
