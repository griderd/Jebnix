using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.Types;

namespace Jebnix.stdlib
{
    class stdmath
    {
        /// <summary>
        /// The constant PI
        /// </summary>
        public static JFloat Pi
        {
            get
            {
                return Math.PI;
            }
        }

        /// <summary>
        /// The constant natural number E
        /// </summary>
        public static JFloat E
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
        public static JFloat Abs(JFloat x)
        {
            return Math.Abs(x);
        }

        /// <summary>
        /// Gets the modulus of two numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static JInteger Mod(JInteger a, JInteger b)
        {
            return a % b;
        }

        /// <summary>
        /// Gets the floor of a number
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static JFloat Floor(JFloat x)
        {
            return Math.Floor(x);
        }

        /// <summary>
        /// Gets the ceiling of a number
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static JFloat Ceiling(JFloat x)
        {
            return Math.Ceiling(x);
        }

        /// <summary>
        /// Rounds a number
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static JFloat Round(JFloat x)
        {
            return Math.Round(x);
        }

        /// <summary>
        /// Rounds a number to the given number of places after the decimal
        /// </summary>
        /// <param name="x"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        public static JFloat Round(JFloat x, JInteger place)
        {
            return Math.Round(x, place);
        }

        /// <summary>
        /// Takes the square root of a number
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static JFloat Sqrt(JFloat x)
        {
            return Math.Sqrt(x);
        }

        /// <summary>
        /// Converts from radians to degrees
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static JFloat RadToDeg(JFloat x)
        {
            return (180.0 / Math.PI) * x;
        }

        /// <summary>
        /// Converts from degrees to radians
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static JFloat DegToRad(JFloat x)
        {
            return x * (Math.PI / 180.0);
        }

        /// <summary>
        /// Gets the sine of x, where x is an angle measured in degrees.
        /// </summary>
        /// <param name="x">An angle measured in degrees</param>
        /// <returns></returns>
        public static JFloat Sin(JFloat x)
        {
            return Math.Sin(DegToRad(x));
        }

        /// <summary>
        /// Gets the cosine of x, where x is an angle measured in degrees.
        /// </summary>
        /// <param name="x">An angle measured in degrees.</param>
        /// <returns></returns>
        public static JFloat Cos(JFloat x)
        {
            return Math.Cos(DegToRad(x));
        }

        /// <summary>
        /// Gets the tangent of x, where x is an angle measured in degrees.
        /// </summary>
        /// <param name="x">An angle measured in degrees.</param>
        /// <returns></returns>
        public static JFloat Tan(JFloat x)
        {
            return Math.Tan(DegToRad(x));
        }

        /// <summary>
        /// Gets the sine of x, where x is an angle measured in radians.
        /// </summary>
        /// <param name="x">An angle measured in radians.</param>
        /// <returns></returns>
        public static JFloat SinR(JFloat x)
        {
            return Math.Sin(x);
        }

        /// <summary>
        /// Gets the cosine of x, where x is an angle measured in radians.
        /// </summary>
        /// <param name="x">An angle measured in radians.</param>
        /// <returns></returns>
        public static JFloat CosR(JFloat x)
        {
            return Math.Cos(x);
        }

        /// <summary>
        /// Gets the tangent of x, where x is an angle measured in radians.
        /// </summary>
        /// <param name="x">An angle measured in radians.</param>
        /// <returns></returns>
        public static JFloat TanR(JFloat x)
        {
            return new JFloat(Math.Tan(x));
        }

        public static JFloat ASin(JFloat x)
        {
            return RadToDeg(Math.Asin(x));
        }

        public static JFloat ACos(JFloat x)
        {
            return RadToDeg(Math.Acos(x));
        }

        public static JFloat ATan(JFloat x)
        {
            return RadToDeg(Math.Atan(x));
        }

        public static JFloat ATan2(JFloat y, JFloat x)
        {
            return RadToDeg(Math.Atan2(y, x));
        }

        public static JFloat ASinR(JFloat x)
        {
            return Math.Asin(x);
        }

        public static JFloat ACosR(JFloat x)
        {
            return Math.Acos(x);
        }

        public static JFloat ATanR(JFloat x)
        {
            return Math.Atan(x);
        }

        public static JFloat ATan2R(JFloat y, JFloat x)
        {
            return Math.Atan2(y, x);
        }

        public static JFloat Log(JFloat base10)
        {
            return Math.Log10(base10);
        }

        public static JFloat Log(JFloat val, JFloat baseN)
        {
            return Math.Log(val, baseN);
        }

        public static JFloat Ln(JFloat baseE)
        {
            return Math.Log(baseE);
        }
    }
}
