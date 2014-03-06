using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIOS.Types
{
    /// <summary>
    /// Represents a pair of coordinates on the screen.
    /// </summary>
    public struct OrderedPair
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y coordinate
        /// </summary>
        public int Y { get; set; }

        public OrderedPair(int x, int y)
            : this()
        {
            X = x;
            Y = y;
        }

        public static OrderedPair Parse(string value)
        {
            if (value.StartsWith("(") & value.EndsWith(")"))
            {
                string trimmed = value.Substring(1, value.Length - 2);
                string[] parts = trimmed.Split(',');
                if (parts.Length == 2)
                {
                    int x, y;
                    if (int.TryParse(parts[0], out x) && int.TryParse(parts[1], out y))
                    {
                        return new OrderedPair(x, y);
                    }
                    else
                    {
                        throw new FormatException("The X and/or Y components were not integers.");
                    }
                }
                else
                {
                    throw new FormatException("Ordered pair not in the correct format.");
                }
            }
            else
            {
                throw new FormatException("Missing parentheses.");
            }
        }

        public static bool TryParse(string value, out OrderedPair result)
        {
            try
            {
                result = Parse(value);
                return true;
            }
            catch
            {
                result = new OrderedPair();
                return false;
            }
        }

        public override string ToString()
        {
            return "(" + X.ToString() + ", " + Y.ToString() + ")";
        }

        public double CompareTo(OrderedPair other)
        {
            return DistanceFromOrigin() - other.DistanceFromOrigin();
        }

        public double DistanceFromOrigin()
        {
            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
        }
    }
}
