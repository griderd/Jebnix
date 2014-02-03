using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerboScriptEngine.ScriptAPI
{
    class KerboMath
    {
        /// <summary>
        /// The constant PI
        /// </summary>
        public static Value Pi
        {
            get
            {
                return Math.PI;
            }
        }

        /// <summary>
        /// The constant natural number E
        /// </summary>
        public static Value E
        {
            get
            {
                return Math.E;
            }
        }

        /// <summary>
        /// Gets the absolute value of a number
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Value Abs(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            if (x.Type == Value.ValueTypes.Integer)
                return x.IntegerValue < 0 ? x.IntegerValue * -1 : x.IntegerValue;
            else
                return x.FloatValue < 0 ? x.FloatValue * -1 : x.FloatValue;
        }

        /// <summary>
        /// Gets the modulus of two numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the floor of a number
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Value Floor(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return Math.Floor(x.FloatValue);
        }

        /// <summary>
        /// Gets the ceiling of a number
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Value Ceiling(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return Math.Ceiling(x.FloatValue);
        }

        /// <summary>
        /// Rounds a number
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Value Round(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return Math.Round(x.FloatValue);
        }

        /// <summary>
        /// Rounds a number to the given number of places after the decimal
        /// </summary>
        /// <param name="x"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        public static Value Round(Value x, Value place)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return Math.Round(x.FloatValue, place.IntegerValue);
        }

        /// <summary>
        /// Takes the square root of a number
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Value Sqrt(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return Math.Sqrt(x.FloatValue);
        }

        /// <summary>
        /// Converts from radians to degrees
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Value RadToDeg(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return (180.0 / Math.PI) * x.FloatValue;
        }

        /// <summary>
        /// Converts from degrees to radians
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Value DegToRad(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return x.FloatValue * (Math.PI / 180.0);
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

            return Math.Sin(DegToRad(x).FloatValue);
        }

        /// <summary>
        /// Gets the cosine of x, where x is an angle measured in degrees.
        /// </summary>
        /// <param name="x">An angle measured in degrees.</param>
        /// <returns></returns>
        public static Value Cos(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return Math.Cos(DegToRad(x).FloatValue);
        }

        /// <summary>
        /// Gets the tangent of x, where x is an angle measured in degrees.
        /// </summary>
        /// <param name="x">An angle measured in degrees.</param>
        /// <returns></returns>
        public static Value Tan(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return Math.Tan(DegToRad(x).FloatValue);
        }

        /// <summary>
        /// Gets the sine of x, where x is an angle measured in radians.
        /// </summary>
        /// <param name="x">An angle measured in radians.</param>
        /// <returns></returns>
        public static Value SinR(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return Math.Sin(x.FloatValue);
        }

        /// <summary>
        /// Gets the cosine of x, where x is an angle measured in radians.
        /// </summary>
        /// <param name="x">An angle measured in radians.</param>
        /// <returns></returns>
        public static Value CosR(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return Math.Cos(x.FloatValue);
        }

        /// <summary>
        /// Gets the tangent of x, where x is an angle measured in radians.
        /// </summary>
        /// <param name="x">An angle measured in radians.</param>
        /// <returns></returns>
        public static Value TanR(Value x)
        {
            if (Value.IsNull(x))
                throw new ArgumentNullException();

            return new Value(Math.Tan(x.FloatValue));
        }

        public static Value ASin(Value x)
        {
            return RadToDeg(Math.Asin(x.FloatValue));
        }

        public static Value ACos(Value x)
        {
            return RadToDeg(Math.Acos(x.FloatValue));
        }

        public static Value ATan(Value x)
        {
            return RadToDeg(Math.Atan(x.FloatValue));
        }

        public static Value ATan2(Value y, Value x)
        {
            return RadToDeg(Math.Atan2(y.FloatValue, x.FloatValue));
        }

        public static Value ASinR(Value x)
        {
            return Math.Asin(x.FloatValue);
        }

        public static Value ACosR(Value x)
        {
            return Math.Acos(x.FloatValue);
        }

        public static Value ATanR(Value x)
        {
            return Math.Atan(x.FloatValue);
        }

        public static Value ATan2R(Value y, Value x)
        {
            return Math.Atan2(y.FloatValue, x.FloatValue);
        }

        public static Value Log(Value base10)
        {
            return Math.Log10(base10.FloatValue);
        }

        public static Value Log(Value val, Value baseN)
        {
            return Math.Log(val.FloatValue, baseN.FloatValue);
        }

        public static Value Ln(Value baseE)
        {
            return Math.Log(baseE.FloatValue);
        }
    }
}
