using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.stdlib;
using Jebnix.Types;

namespace Jebnix
{
    public class Functions
    {
        static Dictionary<string, Delegate> functions = new Dictionary<string, Delegate>();
        static bool stdlibRegistered = false;

        /// <summary>
        /// String representing the global namespace.
        /// </summary>
        public const string GLOBAL = "global";

        /// <summary>
        /// String representing the standard math library namespace.
        /// </summary>
        public const string STDMATH = "stdmath";

        /// <summary>
        /// String representing the standard I/O library namespace.
        /// </summary>
        public const string STDIO = "stdio";

        /// <summary>
        /// An empty parameter list.
        /// </summary>
        public static object[] EmptyParams
        {
            get
            {
                return new object[0];
            }
        }

        /// <summary>
        /// Generates a unique name for a function based on global namespace, function name, and parameter count.
        /// </summary>
        /// <param name="name">Function name</param>
        /// <param name="paramCount">Number of function parameters</param>
        /// <returns>Returns a unique name</returns>
        public static string GenerateUniqueName(string name, int paramCount)
        {
            if ((name == null))
                throw new ArgumentNullException();
            if ((name == "") | (paramCount < 0))
                throw new ArgumentOutOfRangeException();

            return GenerateUniqueName(GLOBAL, name, paramCount);
        }

        /// <summary>
        /// Generates a unique name for a function based on namespace, function name, and parameter count.
        /// </summary>
        /// <param name="namespc">Namspace</param>
        /// <param name="name">Function name</param>
        /// <param name="paramCount">Number of function parameters</param>
        /// <returns>Returns a unique name</returns>
        public static string GenerateUniqueName(string namespc, string name, int paramCount)
        {
            if ((namespc == null) | (name == null))
                throw new ArgumentNullException();
            if ((namespc == "") | (name == "") | (paramCount < 0))
                throw new ArgumentOutOfRangeException();

            return namespc + "_" + name + "_" + paramCount.ToString();
        }

        public static string ResolveNamespace(string[] namespaces)
        {
            if (namespaces == null)
                throw new ArgumentNullException();
            if (namespaces.Length == 0)
                throw new ArgumentOutOfRangeException();

            StringBuilder s = new StringBuilder();
            for (int i = 0; i < namespaces.Length; i++)
            {
                string n = namespaces[i];
                if (n == null)
                    throw new ArgumentNullException();
                else if (n == "")
                    throw new ArgumentOutOfRangeException();
                else
                {
                    s.Append(n);
                    if (i < namespaces.Length - 1) s.Append("_");
                }
            }

            return s.ToString();
        }

        public static string GenerateUniqueName(string[] namespaces, string name, int paramCount)
        {
            if ((namespaces == null) | (name == null))
                throw new ArgumentNullException();
            if ((namespaces.Length == 0) | (name == "") | (paramCount < 0))
                throw new ArgumentOutOfRangeException();

            StringBuilder s = new StringBuilder();
            foreach (string n in namespaces)
            {
                if (n == null)
                    throw new ArgumentNullException();
                else if (n == "")
                    throw new ArgumentOutOfRangeException();
                else
                {
                    s.Append(n);
                    s.Append("_");
                }
            }

            s.Append(name);
            s.Append("_");
            s.Append(paramCount);

            return s.ToString();
        }

        public static bool ContainsFunction(string functionName, int paramCount)
        {
            return ContainsFunction(GLOBAL, functionName, paramCount);
        }

        public static bool ContainsFunction(string namespc, string functionName, int paramCount)
        {
            return functions.ContainsKey(GenerateUniqueName(namespc, functionName, paramCount));
        }

        public static bool ContainsFunction(string[] namespaces, string functionName, int paramCount)
        {
            return functions.ContainsKey(GenerateUniqueName(namespaces, functionName, paramCount));
        }

        /// <summary>
        /// Registers the function with the given name into the global namespace.
        /// </summary>
        /// <param name="name">Function name</param>
        /// <param name="paramCount">Function parameter count</param>
        /// <param name="ptr">Pointer to a function</param>
        /// <returns>Returns true if the function can be added. Returns false if the function name is not unique.</returns>
        public static bool RegisterFunction(string name, int paramCount, Delegate ptr)
        {
            return RegisterFunction(GLOBAL, name, paramCount, ptr);
        }

        /// <summary>
        /// Registers the function with the given name into the given namespace.
        /// </summary>
        /// <param name="namespc">Namespace name</param>
        /// <param name="name">Function name</param>
        /// <param name="paramCount">Function parameter count</param>
        /// <param name="ptr">Pointer to function</param>
        /// <returns>Returns true if the function can be added. Returns false if the function name is not unique.</returns>
        public static bool RegisterFunction(string namespc, string name, int paramCount, Delegate ptr)
        {
            string n = GenerateUniqueName(namespc, name, paramCount);
            if (!functions.ContainsKey(n))
            {
                functions.Add(n, ptr);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Registers a given function under a given alias.
        /// </summary>
        /// <param name="alias">Alias to register</param>
        /// <param name="functionName">Original function name</param>
        /// <param name="paramCount">Parameter count</param>
        /// <returns>Returns true on success, otherwise returns false</returns>
        public static bool RegisterAlias(string alias, string functionName, int paramCount)
        {
            return RegisterAlias(GLOBAL, alias, GLOBAL, functionName, paramCount);
        }

        /// <summary>
        /// Registers a given function under a given alias
        /// </summary>
        /// <param name="alias">Alias to register</param>
        /// <param name="functionNamespace">Original function namespace</param>
        /// <param name="functionName">Original function name</param>
        /// <param name="paramCount">Parameter count</param>
        /// <returns>Returns true on success, otherwise returns false</returns>
        public static bool RegisterAlias(string alias, string functionNamespace, string functionName, int paramCount)
        {
            return RegisterAlias(GLOBAL, alias, functionNamespace, functionName, paramCount);
        }

        /// <summary>
        /// Registers a function under a given alias
        /// </summary>
        /// <param name="aliasNamespace">Alias namespace</param>
        /// <param name="alias">Alias name</param>
        /// <param name="functionNamespace">Original function namespace</param>
        /// <param name="functionName">Original function name</param>
        /// <param name="paramCount">Parameter count</param>
        /// <returns>Returns true on success, otherwise returns false.</returns>
        public static bool RegisterAlias(string aliasNamespace, string alias, string functionNamespace, string functionName, int paramCount)
        {
            string a = GenerateUniqueName(aliasNamespace, alias, paramCount);
            string n = GenerateUniqueName(functionNamespace, functionName, paramCount);

            if (functions.ContainsKey(n))
            {
                if (!functions.ContainsKey(a))
                {
                    functions.Add(a, functions[n]);
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Registers a function under the given name and with the given alias, both in the global namespace.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="alias"></param>
        /// <param name="paramCount"></param>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static bool RegisterWithAlias(string functionName, string alias, int paramCount, Delegate ptr)
        {
            return RegisterWithAlias(GLOBAL, functionName, GLOBAL, alias, paramCount, ptr);
        }

        /// <summary>
        /// Registers a function under the given name, and registers the given alias in global namespace.
        /// </summary>
        /// <param name="functionNamespace"></param>
        /// <param name="functionName"></param>
        /// <param name="alias"></param>
        /// <param name="paramCount"></param>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static bool RegisterWithAlias(string functionNamespace, string functionName, string alias, int paramCount, Delegate ptr)
        {
            return RegisterWithAlias(functionNamespace, functionName, GLOBAL, alias, paramCount, ptr);
        }

        /// <summary>
        /// Registers a function under the given name and with the given alias
        /// </summary>
        /// <param name="functionNamespace"></param>
        /// <param name="functionName"></param>
        /// <param name="aliasNamespace"></param>
        /// <param name="alias"></param>
        /// <param name="paramCount"></param>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static bool RegisterWithAlias(string functionNamespace, string functionName, string aliasNamespace, string alias, int paramCount, Delegate ptr)
        {
            return RegisterFunction(functionNamespace, functionName, paramCount, ptr) && RegisterAlias(aliasNamespace, alias, functionNamespace, functionName, paramCount);
        }

        /// <summary>
        /// Invokes the function with the given name in the global namespace and returns the result.
        /// </summary>
        /// <param name="name">Function name</param>
        /// <param name="returnVal">Variable to store the result.</param>
        /// <param name="paramList">Parameter list</param>
        /// <returns>Returns true if the function was invoked. Otherwise returns false.</returns>
        public static bool InvokeFunction(string name, out object returnVal, params object[] paramList)
        {
            return InvokeFunction(GLOBAL, name, out returnVal, paramList);
        }


        /// <summary>
        /// Invokes a function with the given name in the global namespace. This DOES NOT return a result.
        /// </summary>
        /// <param name="name">Function name.</param>
        /// <param name="paramList">Parameter list</param>
        /// <returns>Returns true if the function was invoked. Otherwise returns false.</returns>
        public static bool InvokeMethod(string name, params object[] paramList)
        {
            object o;
            return InvokeFunction(name, out o, paramList);
        }

        /// <summary>
        /// Invokes a function with the given name in the given namespace. This DOES NOT return a result.
        /// </summary>
        /// <param name="namespc"></param>
        /// <param name="name"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static bool InvokeMethod(string namespc, string name, params object[] paramList)
        {
            object o;
            return InvokeFunction(namespc, name, out o, paramList);
        }

        /// <summary>
        /// Invokes a function with the given name in the given namespace. Returns the result.
        /// </summary>
        /// <param name="namespc">Namespace</param>
        /// <param name="name">Function name</param>
        /// <param name="returnVal">Variable to store the result</param>
        /// <param name="paramList">Parameter list</param>
        /// <returns>Returns true if the function was invoked. Otherwise returns false.</returns>
        public static bool InvokeFunction(string namespc, string name, out object returnVal, params object[] paramList)
        {
            string n = GenerateUniqueName(namespc, name, paramList.Length);
            if (functions.ContainsKey(n))
            {
                try
                {
                    returnVal = functions[n].DynamicInvoke(paramList);
                    return true;
                }
                catch
                {
                    throw;
                }
            }
            returnVal = null;
            return false;
        }

        #region Standard Library Registry
        
        /// <summary>
        /// Registers standard library functions
        /// </summary>
        public static void RegisterStandardLibrary()
        {
            if (!stdlibRegistered)
            {
                RegisterMathFunctions();
                RegisterIOFunctions();

                stdlibRegistered = true;
            }
        }

        private static void RegisterIOFunctions()
        {
            RegisterFunction(STDIO, "print", 1, new Action<Value>(stdio.Print));
            RegisterFunction(STDIO, "println", 1, new Action<Value>(stdio.PrintLine));
            RegisterFunction(STDIO, "println", 0, new Action(stdio.PrintLine));
            RegisterFunction(STDIO, "clearscreen", 0, new Action(stdio.ClearScreen));
            RegisterFunction(STDIO, "getjebnixversion", 0, new Func<string>(stdio.GetJebnixVersion));
        }

        private static void RegisterMathFunctions()
        {
            string[] func = new string[] { "abs", "mod", "floor", "ceiling", "round", "round", "sqrt", "radtodeg", "degtorad", "sin", 
                                           "sinr", "asin", "asinr", "cos", "cosr", "acos", "acosr", "tan", "tanr", "atan", "atanr",
                                           "atan2", "atan2r", "log", "log", "ln" };
            int[] param = new int[] { 1, 2, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 2, 1 };
            Delegate[] dels = new Delegate[] { new Func<Value, Value>(stdmath.Abs), new Func<Value, Value, Value>(stdmath.Mod), 
                                               new Func<Value, Value>(stdmath.Floor), new Func<Value, Value>(stdmath.Ceiling), 
                                               new Func<Value, Value>(stdmath.Round), new Func<Value, Value, Value>(stdmath.Round),
                                               new Func<Value, Value>(stdmath.Sqrt), new Func<Value, Value>(stdmath.RadToDeg), 
                                               new Func<Value, Value>(stdmath.DegToRad), new Func<Value, Value>(stdmath.RadToDeg),
                                               new Func<Value, Value>(stdmath.DegToRad), new Func<Value, Value>(stdmath.Sin),
                                               new Func<Value, Value>(stdmath.SinR), new Func<Value, Value>(stdmath.ASin), 
                                               new Func<Value, Value>(stdmath.ASinR), new Func<Value, Value>(stdmath.Cos), 
                                               new Func<Value, Value>(stdmath.CosR), new Func<Value, Value>(stdmath.ACos),
                                               new Func<Value, Value>(stdmath.ACosR), new Func<Value, Value>(stdmath.Tan), 
                                               new Func<Value, Value>(stdmath.TanR), new Func<Value, Value>(stdmath.ATan), 
                                               new Func<Value, Value>(stdmath.ATanR), new Func<Value, Value, Value>(stdmath.ATan2),
                                               new Func<Value, Value, Value>(stdmath.ATan2R), new Func<Value, Value>(stdmath.Log), 
                                               new Func<Value, Value, Value>(stdmath.Log), new Func<Value, Value>(stdmath.Ln) };

            for (int i = 0; i < func.Length; i++)
            {
                RegisterWithAlias(STDMATH, func[i], func[i], param[i], dels[i]);
            }
        }

        #endregion
    }
}
