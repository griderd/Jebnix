using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerboScriptEngine.ScriptAPI
{
    class APIInfo
    {
        static string[] functionNames = null;
        static Dictionary<string, Delegate> functionDelegates = new Dictionary<string, Delegate>();
        static Dictionary<string, int> functionSignature = new Dictionary<string, int>();

        #region Delegates

        public delegate Value SingleArgDelegate(Value x);
        public delegate Value TwoArgDelegate(Value x, Value y);

        #endregion

        public static bool InvokeFunction(string name, out Value returnValue, params Value[] val)
        {
            BuildFunctions();

            if (!functionNames.Contains(name))
            {
                returnValue = null;
                return false;
            }

            returnValue = (Value)functionDelegates[name].DynamicInvoke(val);
            return true;
        }

        public static void BuildFunctions()
        {
            BuildSignatures();
            if (functionDelegates.Count == 0)
            {
                functionDelegates.Add("abs", new SingleArgDelegate(KerboMath.Abs));
                functionDelegates.Add("mod", new TwoArgDelegate(KerboMath.Mod));
                functionDelegates.Add("floor", new SingleArgDelegate(KerboMath.Floor));
                functionDelegates.Add("ceiling", new SingleArgDelegate(KerboMath.Ceiling));
                functionDelegates.Add("round", new SingleArgDelegate(KerboMath.Round));
                functionDelegates.Add("roundto", new TwoArgDelegate(KerboMath.RoundTo));
                functionDelegates.Add("sqrt", new SingleArgDelegate(KerboMath.Sqrt));
                functionDelegates.Add("radtodeg", new SingleArgDelegate(KerboMath.RadToDeg));
                functionDelegates.Add("degtorad", new SingleArgDelegate(KerboMath.DegToRad));
                functionDelegates.Add("sin", new SingleArgDelegate(KerboMath.Sin));
                functionDelegates.Add("sinr", new SingleArgDelegate(KerboMath.SinR));
                functionDelegates.Add("asin", new SingleArgDelegate(KerboMath.ASin));
                functionDelegates.Add("asinr", new SingleArgDelegate(KerboMath.ASinR));
                functionDelegates.Add("cos", new SingleArgDelegate(KerboMath.Cos));
                functionDelegates.Add("cosr", new SingleArgDelegate(KerboMath.CosR));
                functionDelegates.Add("acos", new SingleArgDelegate(KerboMath.ACos));
                functionDelegates.Add("acosr", new SingleArgDelegate(KerboMath.ACosR));
                functionDelegates.Add("tan", new SingleArgDelegate(KerboMath.Tan));
                functionDelegates.Add("tanr", new SingleArgDelegate(KerboMath.TanR));
                functionDelegates.Add("atan", new SingleArgDelegate(KerboMath.ATan));
                functionDelegates.Add("atanr", new SingleArgDelegate(KerboMath.ATanR));
                functionDelegates.Add("atan2", new TwoArgDelegate(KerboMath.ATan2));
                functionDelegates.Add("atan2r", new TwoArgDelegate(KerboMath.ATan2R));
                functionDelegates.Add("log", new SingleArgDelegate(KerboMath.Log));
                functionDelegates.Add("logx", new SingleArgDelegate(KerboMath.Log));
                functionDelegates.Add("ln", new SingleArgDelegate(KerboMath.Ln));
            }
        }

        private static void BuildSignatures()
        {
            if (functionSignature.Count == 0)
            {
                if (functionSignature.Count == 0)
                {
                    functionSignature.Add("abs", 1);
                    functionSignature.Add("mod", 2);
                    functionSignature.Add("floor", 1);
                    functionSignature.Add("round", 1);
                    functionSignature.Add("roundto", 2);
                    functionSignature.Add("sqrt", 1);
                    functionSignature.Add("radtodeg", 1);
                    functionSignature.Add("degtorad", 1);
                    functionSignature.Add("sin", 1);
                    functionSignature.Add("cos", 1);
                    functionSignature.Add("tan", 1);
                    functionSignature.Add("sinr", 1);
                    functionSignature.Add("cosr", 1);
                    functionSignature.Add("tanr", 1);
                    functionSignature.Add("asin", 1);
                    functionSignature.Add("acos", 1);
                    functionSignature.Add("atan", 1);
                    functionSignature.Add("atan2", 2);
                    functionSignature.Add("asinr", 1);
                    functionSignature.Add("acosr", 1);
                    functionSignature.Add("atanr", 1);
                    functionSignature.Add("atan2r", 2);
                    functionSignature.Add("log", 1);
                    functionSignature.Add("logx", 2);
                    functionSignature.Add("ln", 1);
                }
            }
        }

        public static string[] FunctionNames
        {
            get
            {
                if (functionNames == null)
                {
                    BuildFunctions();

                    functionNames = functionDelegates.Keys.ToArray();
                }
                return functionNames;
            }
        }

        public static int ArgumentCount(string functionName)
        {
            BuildFunctions();

            if (FunctionNames.Contains(functionName))
                return functionSignature[functionName];
            else
                return -1;
        }

        public static bool IsSignatureMatch(string name, int argCount)
        {
            BuildFunctions();

            if (FunctionNames.Contains(name))
                return functionSignature[name] == argCount;
            else
                return false;
        }
    }
}
