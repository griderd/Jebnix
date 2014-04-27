using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Types.BasicTypes
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

        public static implicit operator JFloat(JObject value)
        {
            return (JFloat)value;
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
