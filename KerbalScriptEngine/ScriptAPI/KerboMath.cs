using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerboScriptEngine.ScriptAPI
{
    class KerboMath
    {
        public static Value Abs(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            if (x.Type == Value.ValueTypes.Integer)
                return new Value(x.IntegerValue < 0 ? x.IntegerValue * -1 : x.IntegerValue);
            else
                return new Value(x.FloatValue < 0 ? x.FloatValue * -1 : x.FloatValue);
        }

        public static Value Mod(Value a, Value b)
        {
            if (Value.IsNull(a) | Value.IsNull(b))
                throw new ArgumentNullException();

            try
            {
                return a % b;
            }
            catch
            {
                throw;
            }
        }

        public static Value Floor(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value(x.IntegerValue);
        }

        public static Value Ceiling(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value(Math.Ceiling(x.FloatValue));
        }

        public static Value Round(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value(Math.Round(x.FloatValue));
        }

        public static Value Round(Value x, Value place)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value(Math.Round(x.FloatValue, place.IntegerValue));
        }

        public static Value Sqrt(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value(Math.Sqrt(x.FloatValue));
        }

        public static Value RadToDeg(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value((180.0 / Math.PI) * x.FloatValue);
        }

        public static Value DegToRad(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value(x.FloatValue * (Math.PI / 180.0));
        }

        /// <summary>
        /// Gets the sine of x, where x is an angle measured in degrees.
        /// </summary>
        /// <param name="x">An angle measured in degrees</param>
        /// <returns></returns>
        public static Value Sin(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value(Math.Sin(DegToRad(x).FloatValue));
        }

        public static Value Cos(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value(Math.Cos(DegToRad(x).FloatValue));
        }

        public static Value Tan(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value(Math.Tan(DegToRad(x).FloatValue));
        }

        public static Value SinR(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value(Math.Sin(x.FloatValue));
        }

        public static Value CosR(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value(Math.Cos(x.FloatValue));
        }

        public static Value TanR(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value(Math.Tan(x.FloatValue));
        }
    }
}
