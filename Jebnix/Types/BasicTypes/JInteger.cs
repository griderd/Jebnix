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
            return a.value / b.value;
        }

        public static JInteger operator %(JInteger a, JInteger b)
        {
            return a.value % b.value;
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
    }
}
