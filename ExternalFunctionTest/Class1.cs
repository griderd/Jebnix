using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix;
using Jebnix.stdlib;
using Jebnix.Types;

namespace ExternalFunctionTest
{
    public class Class1
    {
        const string TEST_NAMESPACE = "test";

        public Class1()
        {
            Functions.RegisterFunction("test", 0, new Func<string>(TestFunction));
            Functions.RegisterFunction("librarytest", 1, new Action<Value>(LibraryPrintTest));
            Functions.RegisterFunction("getnumber", 0, new Func<double>(GetNumber));
        }

        /// <summary>
        /// Returns "This is a test function."
        /// </summary>
        /// <returns></returns>
        public string TestFunction()
        {
            return "This is a test function.";
        }
        
        /// <summary>
        /// Prints "This is a library test." to the screen via stdio, along with the string value given.
        /// </summary>
        public void LibraryPrintTest(Value value)
        {
            stdio.PrintLine("This is a library test. " + value.StringValue);
        }

        public double GetNumber()
        {
            return 10;
        }
    }
}
