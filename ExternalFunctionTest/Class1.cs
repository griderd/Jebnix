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
            Functions.RegisterFunction("librarytest", 1, new Action<JString>(LibraryPrintTest));
            Functions.RegisterFunction("getnumber", 0, new Func<double>(GetNumber));
            Functions.RegisterFunction("externaltest", 0, new Action(ExternalCallTest));
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
        public void LibraryPrintTest(JString value)
        {
            stdio.PrintLine("This is a library test. " + value);
        }

        /// <summary>
        /// Prints "This is an invocation test" to the screen via the registered function "stdio_println_1".
        /// </summary>
        public void ExternalCallTest()
        {
            Functions.InvokeMethod(Functions.STDIO, "println", "This is an invocation test.");
        }

        /// <summary>
        /// Returns the number 10.
        /// </summary>
        /// <returns></returns>
        public double GetNumber()
        {
            return 10;
        }
    }
}
