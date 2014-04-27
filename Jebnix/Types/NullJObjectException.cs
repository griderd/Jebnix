using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Types
{
    /// <summary>
    /// Thrown when a JObject.IsNull value is true when it is expected not to be.
    /// </summary>
    public class NullJObjectException : Exception
    {
        public NullJObjectException()
            : base("A JObject has an IsNull value of True when it is expected to be false.")
        {

        }
    }
}
