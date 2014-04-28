using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Types
{
    public class JBoolean : JInteger
    {
        public new const string TYPENAME = "Boolean";

        public JBoolean(bool value)
            : base(value ? 1 : 0)
        {
        }

        public JBoolean(int value)
            : base(value)
        {
            
        }

        public static JBoolean True
        {
            get
            {
                return new JBoolean(true);
            }
        }

        public static JBoolean False
        {
            get
            {
                return new JBoolean(false);
            }
        }

        public new bool Value
        {
            get
            {
                return (value != 0);
            }
            set
            {
                this.value = value ? 1 : 0;
            }
        }

        public static implicit operator bool(JBoolean value)
        {
            return value.Value;
        }

        public static implicit operator JBoolean(bool value)
        {
            return new JBoolean(value);
        }

        public static JBoolean operator !(JBoolean a)
        {
            return !a.Value;
        }

        public override bool FromString(JString value)
        {
            if (value == null)
                throw new ArgumentNullException();
            if (value.IsNull)
                throw new NullJObjectException();

            bool result, returnVal;
            returnVal = bool.TryParse(value, out result);
            if (returnVal) Value = result;
            return returnVal;
        }

        public static bool TryParse(string value, out JBoolean result)
        {
            bool x;
            bool b = bool.TryParse(value, out x);

            result = b ? x : false;
            return b;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
