using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIOS
{
    public class ExternalFunctions
    {
        static Dictionary<string, Delegate> externalFunctions = new Dictionary<string, Delegate>();

        /// <summary>
        /// String representing the global namespace.
        /// </summary>
        public const string GLOBAL_NAMESPACE = "global";

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

            return GenerateUniqueName(GLOBAL_NAMESPACE, name, paramCount);
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

        /// <summary>
        /// Registers the function with the given name into the global namespace.
        /// </summary>
        /// <param name="name">Function name</param>
        /// <param name="paramCount">Function parameter count</param>
        /// <param name="ptr">Pointer to a function</param>
        /// <returns>Returns true if the function can be added. Returns false if the function name is not unique.</returns>
        public static bool RegisterFunction(string name, int paramCount, Delegate ptr)
        {
            return RegisterFunction(GLOBAL_NAMESPACE, name, paramCount, ptr);
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
            if (!externalFunctions.ContainsKey(n))
            {
                externalFunctions.Add(n, ptr);
                return true;
            }
            return false;
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
            return InvokeFunction(GLOBAL_NAMESPACE, name, out returnVal, paramList);
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
            if (externalFunctions.ContainsKey(n))
            {
                returnVal = externalFunctions[n].DynamicInvoke(paramList);
                return true;
            }
            returnVal = null;
            return false;
        }
    }
}
