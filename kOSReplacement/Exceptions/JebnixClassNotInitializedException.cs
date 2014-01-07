using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Exceptions
{
    public class JebnixClassNotInitializedException : Exception
    {
        public JebnixClassNotInitializedException(string className)
            : base(className + " class not initialized. Try calling " + className + ".Initialize().")
        {

        }
    }
}
