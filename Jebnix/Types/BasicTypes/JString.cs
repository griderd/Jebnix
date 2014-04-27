using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Types.BasicTypes
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

        public override string ToString()
        {
            return value;
        }

        public static JString operator +(JString a, JObject b)
        {
            return a.value + b.ToString();
        }

    }
}
