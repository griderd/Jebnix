using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Jebnix.Types.Structures
{
    public class OrderedPair : JObject
    {
        public new const string TYPENAME = "OrderedPair";

        public JInteger X { get; set; }
        public JInteger Y { get; set; }

        public OrderedPair()
            : base(false, TYPENAME)
        {
            X = 0;
            Y = 0;
        }

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

        public static OrderedPair operator +(OrderedPair a, OrderedPair b)
        {
            return new OrderedPair(a.X + b.X, a.Y + b.Y);
        }

        public static OrderedPair operator -(OrderedPair a, OrderedPair b)
        {
            return new OrderedPair(a.X - b.X, a.Y - b.Y);
        }

        public static OrderedPair operator *(OrderedPair a, OrderedPair b)
        {
            return new OrderedPair(a.X * b.X, a.Y - b.Y);
        }

        public static OrderedPair operator /(OrderedPair a, OrderedPair b)
        {
            if (b.X != 0 & b.Y != 0)
                return new OrderedPair(a.X / b.X, a.Y / b.Y);
            else
                throw new DivideByZeroException();
        }

        public static OrderedPair operator %(OrderedPair a, OrderedPair b)
        {
            if (b.X != 0 & b.Y != 0)
                return new OrderedPair(a.X % b.X, a.Y % b.Y);
            else
                throw new DivideByZeroException();
        }

        public static bool operator ==(OrderedPair a, OrderedPair b)
        {
            return a.X == b.X & a.Y == b.Y;
        }

        public static bool operator !=(OrderedPair a, OrderedPair b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        protected override bool IsEqual(JObject a)
        {
            if (IsSameType(a))
                return this == (OrderedPair)a;
            else
                return false;
        }

        protected override bool IsNotEqual(JObject a)
        {
            if (IsSameType(a))
                return this != (OrderedPair)a;
            else
                return false;
        }

        protected override JObject IsLessThan(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject IsLessThanOrEqual(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject IsGreaterThan(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject IsGreaterThanOrEqual(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Addition(JObject a)
        {
            if (IsSameType(a))
                return this + (OrderedPair)a;

            switch (a.ObjectType)
            {
                case JString.TYPENAME:
                    return (JString)(this.ToString() + (JString)a);
                    
                case JBoolean.TYPENAME:
                case JInteger.TYPENAME:
                case JFloat.TYPENAME:
                    return new OrderedPair(X + (JInteger)a, Y);

                default:
                    return new OrderedPair();
            }
                
        }

        protected override JObject Subtract(JObject a)
        {
            if (IsSameType(a))
                return this - (OrderedPair)a;

            switch (a.ObjectType)
            {
                case JBoolean.TYPENAME:
                case JInteger.TYPENAME:
                case JFloat.TYPENAME:
                    return new OrderedPair(X - (JInteger)a, Y);

                default:
                    throw new InvalidOperationException();
            }
        }

        protected override JObject Multiply(JObject a)
        {
            if (IsSameType(a))
                return this * (OrderedPair)a;

            switch (a.ObjectType)
            {
                case JBoolean.TYPENAME:
                case JInteger.TYPENAME:
                case JFloat.TYPENAME:
                    return new OrderedPair(X * (JInteger)a, Y);

                default:
                    throw new InvalidOperationException();
            }
        }

        protected override JObject Divide(JObject a)
        {
            if (IsSameType(a))
            {
                try
                {
                    return this / (OrderedPair)a;
                }
                catch
                {
                    throw;
                }
            }

            switch (a.ObjectType)
            {
                case JBoolean.TYPENAME:
                case JInteger.TYPENAME:
                case JFloat.TYPENAME:
                    try
                    {
                        return new OrderedPair(X / (JInteger)a, Y);
                    }
                    catch
                    {
                        throw;
                    }

                default:
                    throw new InvalidOperationException();
            }
        }

        protected override JObject Modulus(JObject a)
        {
            if (IsSameType(a))
            {
                try
                {
                    return this % (OrderedPair)a;
                }
                catch
                {
                    throw;
                }
            }

            switch (a.ObjectType)
            {
                case JBoolean.TYPENAME:
                case JInteger.TYPENAME:
                case JFloat.TYPENAME:
                    try
                    {
                        return new OrderedPair(X % (JInteger)a, Y);
                    }
                    catch
                    {
                        throw;
                    }

                default:
                    throw new InvalidOperationException();
            }
        }

        protected override JObject Pow(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject And(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Or(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Positive()
        {
            throw new InvalidOperationException();
        }

        protected override JObject Negative()
        {
            throw new InvalidOperationException();
        }

        protected override JObject Not()
        {
            throw new InvalidOperationException();
        }

        protected override JObject Increment()
        {
            throw new InvalidOperationException();
        }

        protected override JObject Decrement()
        {
            throw new InvalidOperationException();
        }
    }
}
