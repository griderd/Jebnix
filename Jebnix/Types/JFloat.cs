using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Types
{
    public class JFloat : JObject<double>
    {
        public new const string TYPENAME = "Float";

        public JFloat(double value)
            : base(value, false, TYPENAME) { }

        public override bool IsNull
        {
            get
            {
                return false;
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override double Value
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

        public static implicit operator JFloat(JString value)
        {
            double d;
            if (double.TryParse(value, out d))
                return d;
            else
                return 0;
        }

        public static implicit operator JFloat(double value)
        {
            return new JFloat(value);
        }

        public static implicit operator JFloat(float value)
        {
            return new JFloat(value);
        }

        public static implicit operator JFloat(int value)
        {
            return new JFloat((double)value);
        }

        public static explicit operator JFloat(JInteger value)
        {
            return new JFloat((double)value.Value);
        }

        public static implicit operator double(JFloat value)
        {
            return value;
        }

        public bool FromString(JString value)
        {
            if (value == null)
                throw new NullReferenceException();
            if (value.IsNull)
                throw new NullJObjectException();

            return double.TryParse(value, out this.value);
        }

        public static JFloat RaiseToPower(JFloat a, JFloat b)
        {
            return Math.Pow(a, b);
        }

        public static bool TryParse(string value, out JFloat result)
        {
            double x;
            bool b = double.TryParse(value, out x);

            result = b ? x : 0;
            return b;
        }

        public static JFloat Parse(string value)
        {
            double x;
            if (!double.TryParse(value, out x))
                throw new InvalidCastException();
            return x;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public static JFloat operator +(JFloat a, JFloat b)
        {
            return a.value + b.value;
        }

        public static JFloat operator -(JFloat a, JFloat b)
        {
            return a.value - b.value;
        }

        public static JFloat operator *(JFloat a, JFloat b)
        {
            return a.value * b.value;
        }

        public static JFloat operator /(JFloat a, JFloat b)
        {
            return a.value / b.value;
        }

        public static bool operator ==(JFloat a, JFloat b)
        {
            return Math.Abs(a - b) < double.Epsilon * 10;
        }

        public static bool operator !=(JFloat a, JFloat b)
        {
            return !(a == b);
        }

        public static bool operator >(JFloat a, JFloat b)
        {
            return a.value > b.value;
        }

        public static bool operator >=(JFloat a, JFloat b)
        {
            return a.value >= b.value;
        }

        public static bool operator <(JFloat a, JFloat b)
        {
            return a.value < b.value;
        }

        public static bool operator <=(JFloat a, JFloat b)
        {
            return a.value <= b.value;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected override bool IsEqual(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JInteger.TYPENAME))
                return new JBoolean(this == (JFloat)a); ;

            return false;
        }

        protected override bool IsNotEqual(JObject a)
        {
            return !IsEqual(a);
        }

        protected override JObject IsLessThan(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JInteger.TYPENAME))
                return new JBoolean(this < (JFloat)a);

            return JBoolean.False;
        }

        protected override JObject IsLessThanOrEqual(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JInteger.TYPENAME))
                return new JBoolean(this <= (JFloat)a);

            return JBoolean.False;
        }

        protected override JObject IsGreaterThan(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JInteger.TYPENAME))
                return new JBoolean(this > (JFloat)a);

            return JBoolean.False;
        }

        protected override JObject IsGreaterThanOrEqual(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JInteger.TYPENAME))
                return new JBoolean(this >= (JFloat)a);

            return JBoolean.False;
        }

        protected override JObject Addition(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JInteger.TYPENAME))
                return new JFloat(this + (JFloat)a);

            throw new InvalidCastException();
        }

        protected override JObject Subtract(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JInteger.TYPENAME))
                return new JFloat(this - (JFloat)a);

            throw new InvalidCastException();
        }

        protected override JObject Multiply(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JInteger.TYPENAME))
                return new JFloat(this * (JFloat)a);

            throw new InvalidCastException();
        }

        protected override JObject Divide(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JInteger.TYPENAME))
                return new JFloat(this / (JFloat)a);

            throw new InvalidCastException();
        }

        protected override JObject Modulus(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Pow(JObject a)
        {
            if ((IsSameType(a)) | (a.ObjectType == JInteger.TYPENAME))
                return new JFloat(Math.Pow(this, (JFloat)a));

            throw new InvalidCastException();
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
            return this;
        }

        protected override JObject Negative()
        {
            return new JFloat(-value);
        }

        protected override JObject Not()
        {
            throw new InvalidOperationException();
        }

        protected override JObject Increment()
        {
            return new JFloat(value + 1);
        }

        protected override JObject Decrement()
        {
            return new JFloat(value - 1);
        }
    }
}
