using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerbalScriptEngine
{
    public struct Value
    {
        public bool IsNull { get; private set; }

        string s_value;
        int i_value;
        double f_value;
        bool b_value;

        public enum ValueTypes : int
        {
            String,
            Integer,
            Float,
            Boolean
        }

        public static Value NullValue
        {
            get
            {
                Value v = new Value();
                v.IsNull = true;
                return v;
            }
        }

        ValueTypes Type { get; set; }

        public string StringValue
        {
            get 
            {
                if (!IsNull)
                    return s_value;
                else
                    return null;
            }
            set 
            { 
                SetValue(value);
            }
        }

        public int IntegerValue
        {
            get 
            {
                if (!IsNull)
                    return i_value;
                else
                    return 0;
            }
            set 
            { 
                SetValue(value);
            }
        }

        public double FloatValue
        {
            get 
            {
                if (!IsNull)
                    return f_value;
                else
                    return double.NaN;
            }
            set 
            { 
                SetValue(value);
            }
        }

        public bool BooleanValue
        {
            get 
            {
                if (!IsNull)
                    return b_value;
                else
                    return false;
            }
            set
            {
                SetValue(value);
            }
        }

        public Value(string value)
            : this()
        {
            SetValue(value);
        }

        public Value(int value)
            : this()
        {
            SetValue(value);
        }

        public Value(double value)
            : this()
        {
            SetValue(value);
        }

        public Value(bool value)
            : this()
        {
            SetValue(value);
        }

        public void SetValue(string value)
        {
            IsNull = false;
            Type = ValueTypes.String;
            s_value = value;
            if (!int.TryParse(value, out i_value)) i_value = 0;
            if (!double.TryParse(value, out f_value)) f_value = double.NaN;
            if (!bool.TryParse(value, out b_value)) b_value = false;
        }

        public void SetValue(int value)
        {
            IsNull = false;
            Type = ValueTypes.Integer;
            i_value = value;
            s_value = value.ToString();
            f_value = (double)i_value;
            b_value = i_value != 0;
        }

        public void SetValue(double value)
        {
            IsNull = false;
            Type = ValueTypes.Float;
            f_value = value;
            i_value = (int)value;
            b_value = i_value != 0;
            s_value = value.ToString();
        }

        public void SetValue(bool value)
        {
            IsNull = false;
            Type = ValueTypes.Boolean;
            b_value = value;
            s_value = value.ToString();
            i_value = value ? 1 : 0;
            f_value = (double)i_value;
        }

        public void CastToType(ValueTypes t)
        {
            switch (Type)
            {
                case ValueTypes.Boolean:
                    SetValue(BooleanValue);
                    break;

                case ValueTypes.Float:
                    SetValue(FloatValue);
                    break;

                case ValueTypes.Integer:
                    SetValue(IntegerValue);
                    break;
                    
                case ValueTypes.String:
                    SetValue(StringValue);
                    break;
            }
        }

        #region Binary Operations
        public static Value operator ==(Value a, Value b)
        {
            if (a.IsNull | b.IsNull) throw new NullReferenceException();

            if ((a.Type == ValueTypes.String) | (b.Type == ValueTypes.String))
                return new Value(a.StringValue == b.StringValue);
            else if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
                return new Value(Math.Abs(a.FloatValue - b.FloatValue) < double.Epsilon * 10);
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return new Value(a.IntegerValue == b.IntegerValue);
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return new Value(a.BooleanValue == b.BooleanValue);
            else
                return new Value();
        }

        public static Value operator !=(Value a, Value b)
        {
            if (a.IsNull | b.IsNull) throw new NullReferenceException();

            return !(a == b);
        }

        public static Value operator +(Value a, Value b)
        {
            if (a.IsNull | b.IsNull) throw new NullReferenceException();

            if ((a.Type == ValueTypes.String) | (b.Type == ValueTypes.String))
                return new Value(a.StringValue + b.StringValue);
            else if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
                return new Value(a.FloatValue + b.FloatValue);
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return new Value(a.IntegerValue + b.IntegerValue);
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return new Value((a.IntegerValue + b.IntegerValue) != 0 ? true : false);
            else
                return new Value();
        }

        public static Value operator -(Value a, Value b)
        {
            if (a.IsNull | b.IsNull) throw new NullReferenceException();

            if ((a.Type == ValueTypes.String) | (b.Type == ValueTypes.String))
                throw new InvalidOperationException("Cannot perform subtraction with a string value.");
            else if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
                return new Value(a.FloatValue - b.FloatValue);
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return new Value(a.IntegerValue - b.IntegerValue);
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return new Value((a.IntegerValue - b.IntegerValue) != 0 ? true : false);
            else
                return new Value();
        }

        public static Value operator *(Value a, Value b)
        {
            if (a.IsNull | b.IsNull) throw new NullReferenceException();

            if ((a.Type == ValueTypes.String) | (b.Type == ValueTypes.String))
                throw new InvalidOperationException("Cannot perform multiplication with a string value.");
            else if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
                return new Value(a.FloatValue * b.FloatValue);
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return new Value(a.IntegerValue * b.IntegerValue);
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return new Value((a.IntegerValue * b.IntegerValue) != 0 ? true : false);
            else
                return new Value();
        }

        public static Value operator /(Value a, Value b)
        {
            if (a.IsNull | b.IsNull) throw new NullReferenceException();

            if ((a.Type == ValueTypes.String) | (b.Type == ValueTypes.String))
                throw new InvalidOperationException("Cannot perform division with a string value.");
            else if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
            {
                if (b.FloatValue.CompareTo(0f) != 0)
                    throw new DivideByZeroException();
                return new Value(a.FloatValue / b.FloatValue);
            }
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
            {
                if (b.IntegerValue == 0)
                    throw new DivideByZeroException();
                return new Value(a.IntegerValue / b.IntegerValue);
            }
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
            {
                if (b.IntegerValue == 0)
                    throw new DivideByZeroException();
                return new Value((a.IntegerValue / b.IntegerValue) != 0 ? true : false);
            }
            else
                return new Value();
        }

        public static Value operator %(Value a, Value b)
        {
            if (a.IsNull | b.IsNull) throw new NullReferenceException();

            if ((a.Type == ValueTypes.String) | (b.Type == ValueTypes.String))
                throw new InvalidOperationException("Cannot perform remainder division with a string value.");
            else if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
            {
                if (b.FloatValue.CompareTo(0f) != 0)
                    throw new DivideByZeroException();
                return new Value(a.FloatValue % b.FloatValue);
            }
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
            {
                if (b.IntegerValue == 0)
                    throw new DivideByZeroException();
                return new Value(a.IntegerValue % b.IntegerValue);
            }
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
            {
                if (b.IntegerValue == 0)
                    throw new DivideByZeroException();
                return new Value((a.IntegerValue % b.IntegerValue) != 0 ? true : false);
            }
            else
                return new Value();
        }

        public static Value RaiseToPower(Value x, Value power)
        {
            if (x.IsNull | power.IsNull) throw new NullReferenceException();

            if ((x.Type == ValueTypes.String) | (power.Type == ValueTypes.String))
                throw new InvalidOperationException("Cannot perform multiplication with a string value.");
            else if ((x.Type == ValueTypes.Float) | (power.Type == ValueTypes.Float))
                return new Value(Math.Pow(x.FloatValue, power.FloatValue));
            else if ((x.Type == ValueTypes.Integer) | (power.Type == ValueTypes.Integer))
                return new Value((int)Math.Pow(x.FloatValue, power.FloatValue));
            else if ((x.Type == ValueTypes.Boolean) | (power.Type == ValueTypes.Boolean))
                return new Value(((int)Math.Pow(x.FloatValue, power.FloatValue)) != 0 ? true : false);
            else
                return new Value();
        }

        public static Value operator &(Value a, Value b)
        {
            if (a.IsNull | b.IsNull) throw new NullReferenceException();

            if ((a.Type == ValueTypes.String) | (b.Type == ValueTypes.String))
                throw new InvalidOperationException("Cannot perform bitwise or logical AND with a string value.");
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return new Value(a.IntegerValue & b.IntegerValue);
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return new Value(a.BooleanValue & b.BooleanValue);
            else if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
                throw new InvalidOperationException("Cannot perform bitwise or logical AND with a floating-point value.");
            else
                return new Value();
        }

        public static Value operator |(Value a, Value b)
        {
            if (a.IsNull | b.IsNull) throw new NullReferenceException();

            if ((a.Type == ValueTypes.String) | (b.Type == ValueTypes.String))
                throw new InvalidOperationException("Cannot perform bitwise or logical AND with a string value.");
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return new Value(a.IntegerValue | b.IntegerValue);
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return new Value(a.BooleanValue | b.BooleanValue);
            else if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
                throw new InvalidOperationException("Cannot perform bitwise or logical AND with a floating-point value.");
            else
                return new Value();
        }

        #endregion

        #region Unary Operations

        public static Value operator +(Value x)
        {
            if (x.IsNull) throw new NullReferenceException();
            return x;
        }

        public static Value operator -(Value x)
        {
            if (x.IsNull) throw new NullReferenceException();

            if (x.Type == ValueTypes.String)
                throw new InvalidOperationException("Cannot perform mathematical negation on a string value.");
            else if (x.Type == ValueTypes.Float)
                return new Value(x.FloatValue * -1);
            else if (x.Type == ValueTypes.Integer)
                return new Value(x.IntegerValue * -1);
            else if (x.Type == ValueTypes.Boolean)
                return new Value((x.IntegerValue * -1) != 0 ? true : false);
            else
                return new Value();
        }

        public static Value operator ~(Value x)
        {
            if (x.IsNull) throw new NullReferenceException();

            if (x.Type == ValueTypes.String)
                throw new InvalidOperationException("Cannot perform binary negation on a string value.");
            else if (x.Type == ValueTypes.Float)
                throw new InvalidOperationException("Cannot perform binary negation on a double-precision floating point value.");
            else if (x.Type == ValueTypes.Integer)
                return new Value(~x.IntegerValue);
            else if (x.Type == ValueTypes.Boolean)
                return new Value((~x.IntegerValue) != 0 ? true : false);
            else
                return new Value();
        }

        public static Value operator !(Value x)
        {
            if (x.IsNull) throw new NullReferenceException();

            if (x.Type == ValueTypes.Boolean)
            {
                return new Value(!x.BooleanValue);
            }
            else
            {
                throw new InvalidOperationException("Cannot perform logical-NOT on non-boolean values.");
            }
        }

        public static Value operator ++(Value x)
        {
            if (x.IsNull) throw new NullReferenceException();

            if (x.Type == ValueTypes.String)
                throw new InvalidOperationException("Cannot perform incrementation with a string value.");
            else if (x.Type == ValueTypes.Float)
                return new Value(x.FloatValue + 1);
            else if (x.Type == ValueTypes.Integer)
                return new Value(x.IntegerValue + 1);
            else if (x.Type == ValueTypes.Boolean)
                return new Value((x.IntegerValue + 1) != 0 ? true : false);
            else
                return new Value();
        }

        public static Value operator --(Value x)
        {
            if (x.IsNull) throw new NullReferenceException();

            if (x.Type == ValueTypes.String)
                throw new InvalidOperationException("Cannot perform decrementation with a string value.");
            else if (x.Type == ValueTypes.Float)
                return new Value(x.FloatValue - 1);
            else if (x.Type == ValueTypes.Integer)
                return new Value(x.IntegerValue - 1);
            else if (x.Type == ValueTypes.Boolean)
                return new Value((x.IntegerValue - 1) != 0 ? true : false);
            else
                return new Value();
        }

        #endregion
    }
}
