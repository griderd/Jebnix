using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Types
{
    public abstract class JObject<T> : JObject 
    {
        protected T value;

        public abstract T Value
        {
            get;
            set;
        }

        public JObject(T value, bool isNull, string typename)
            : base(isNull, typename)
        {
            this.value = value;
        }
    }
}
