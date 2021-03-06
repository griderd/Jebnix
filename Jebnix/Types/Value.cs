﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Types
{
    public class Value
    {
        string s_value;
        int i_value;
        double f_value;
        bool b_value;
        OrderedPair op_value;

        public enum ValueTypes : int
        {
            String,
            Integer,
            Float,
            Boolean,
            OrderedPair,
            Pointer,
            Operator
        }

        public ValueTypes Type { get; private set; }

        public static bool IsNull(Value obj)
        {
            return object.ReferenceEquals(obj, null);
        }

        public static Value IsNull_Property(Value obj)
        {
            return new Value(IsNull(obj));
        }

        public override string ToString()
        {
            Value x = Copy();
            x.CastToType(ValueTypes.String);
            return x.StringValue;
        }

        public Value Copy()
        {
            switch (Type)
            {
                case ValueTypes.Boolean:
                    return new Value(BooleanValue);
                     
                case ValueTypes.Float:
                    return new Value(FloatValue);

                case ValueTypes.Integer:
                    return new Value(IntegerValue);

                case ValueTypes.OrderedPair:
                    return new Value(OrderedPairValue);

                case ValueTypes.Pointer:
                    return Value.GetPointer(StringValue);

                case ValueTypes.String:
                    return new Value(StringValue);

                default:
                    return null;
            }
        }

        public string PointerValue
        {
            get
            {
                return s_value;
            }
            set
            {
                SetValue(value);
            }
        }

        public string StringValue
        {
            get 
            {
                return s_value;
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
                return i_value;
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
                return f_value;
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
                return b_value;
            }
            set
            {
                SetValue(value);
            }
        }

        public OrderedPair OrderedPairValue
        {
            get
            {
                return op_value;
            }
        }

        public Value()
        {
            SetValue(0);
        }

        public Value(string value)
        {
            SetValue(value);
        }

        public Value(int value)
        {
            SetValue(value);
        }

        public Value(double value)
        {
            SetValue(value);
        }

        public Value(bool value)
        {
            SetValue(value);
        }

        public Value(OrderedPair value)
        {
            SetValue(value);
        }

        public void SetValue(string value, bool isPointer = false)
        {
            Type = isPointer ? ValueTypes.Pointer : ValueTypes.String;
            s_value = value;
            if (!int.TryParse(value, out i_value)) i_value = 0;
            if (!double.TryParse(value, out f_value)) f_value = double.NaN;
            if (!bool.TryParse(value, out b_value)) b_value = false;
            if (!OrderedPair.TryParse(value, out op_value)) op_value = new OrderedPair();
        }

        public void SetValue(int value)
        {
            Type = ValueTypes.Integer;
            i_value = value;
            s_value = value.ToString();
            f_value = (double)i_value;
            b_value = i_value != 0;
            op_value = new OrderedPair(value, 0);
        }

        public void SetValue(double value)
        {
            Type = ValueTypes.Float;
            f_value = value;
            i_value = (int)value;
            b_value = i_value != 0;
            s_value = value.ToString();
            op_value = new OrderedPair(i_value, 0);
        }

        public void SetValue(bool value)
        {
            Type = ValueTypes.Boolean;
            b_value = value;
            s_value = value.ToString();
            i_value = value ? 1 : 0;
            f_value = (double)i_value;
            op_value = new OrderedPair(i_value, 0);
        }

        public void SetValue(OrderedPair value)
        {
            Type = ValueTypes.OrderedPair;
            op_value = value;
            i_value = value.X;
            f_value = (double)value.X;
            b_value = value.X == 0 ? false : true;
            s_value = value.ToString();
        }

        public static Value GetPointer(string pointsTo)
        {
            Value v = new Value(pointsTo);
            v.CastToType(ValueTypes.Pointer);
            return v;
        }

        public static Value Parse(string value)
        {
            OrderedPair op_temp;
            int i_temp;
            double f_temp;
            bool b_temp;

            if (OrderedPair.TryParse(value, out op_temp))
                return new Value(op_temp);
            else if (int.TryParse(value, out i_temp))
                return i_temp;
            else if (double.TryParse(value, out f_temp))
                return f_temp;
            else if (bool.TryParse(value, out b_temp))
                return b_temp;
            else
                return value;
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

                case ValueTypes.OrderedPair:
                    SetValue(OrderedPairValue);
                    break;

                case ValueTypes.Pointer:
                    SetValue(StringValue);
                    Type = ValueTypes.Pointer;
                    break;
            }
        }

        #region Binary Operations
        public static Value operator ==(Value a, Value b)
        {
            if (IsNull(a) | IsNull(b)) throw new NullReferenceException();

            if ((a.Type == ValueTypes.String) | (b.Type == ValueTypes.String))
                return new Value(a.StringValue == b.StringValue);
            else if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
                return new Value(Math.Abs(a.FloatValue - b.FloatValue) < double.Epsilon * 10);
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return new Value(a.IntegerValue == b.IntegerValue);
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return new Value(a.BooleanValue == b.BooleanValue);
            else if ((a.Type == ValueTypes.OrderedPair) | (b.Type == ValueTypes.OrderedPair))
                return new Value((a.OrderedPairValue.X == b.OrderedPairValue.X) & (a.OrderedPairValue.Y == b.OrderedPairValue.Y));  
            else
                return new Value();
        }

        public static Value operator !=(Value a, Value b)
        {
            if (IsNull(a) | IsNull(b)) throw new NullReferenceException();

            return !(a == b);
        }

        public static Value operator >(Value a, Value b)
        {
            if (IsNull(a) | IsNull(b)) throw new NullReferenceException();

            if ((a.Type == ValueTypes.String) | (b.Type == ValueTypes.String))
                return a.StringValue.CompareTo(b.StringValue) > 0;
            else if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
                return a.FloatValue > b.FloatValue;
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return a.IntegerValue > b.IntegerValue;
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return a.IntegerValue > b.IntegerValue;
            else if ((a.Type == ValueTypes.OrderedPair) | (b.Type == ValueTypes.OrderedPair))
                return a.OrderedPairValue.CompareTo(b.OrderedPairValue) > 0;
            else
                return new Value();
        }

        public static Value operator >=(Value a, Value b)
        {
            if (IsNull(a) | IsNull(b)) throw new NullReferenceException();

            return (a == b) | (a > b);
        }

        public static Value operator <=(Value a, Value b)
        {
            if (IsNull(a) | IsNull(b)) throw new NullReferenceException();

            return (a == b) | (a < b);
        }

        public static Value operator <(Value a, Value b)
        {
            if (IsNull(a) | IsNull(b)) throw new NullReferenceException();

            if ((a.Type == ValueTypes.String) | (b.Type == ValueTypes.String))
                return a.StringValue.CompareTo(b.StringValue) < 0;
            else if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
                return a.FloatValue < b.FloatValue;
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return a.IntegerValue < b.IntegerValue;
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return a.IntegerValue < b.IntegerValue;
            else if ((a.Type == ValueTypes.OrderedPair) | (b.Type == ValueTypes.OrderedPair))
                return a.OrderedPairValue.CompareTo(b.OrderedPairValue) < 0;
            else
                return new Value();
        }

        public static Value operator +(Value a, Value b)
        {
            if (IsNull(a) | IsNull(b)) throw new NullReferenceException();

            if ((a.Type == ValueTypes.String) | (b.Type == ValueTypes.String))
                return new Value(a.StringValue + b.StringValue);
            else if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
                return new Value(a.FloatValue + b.FloatValue);
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return new Value(a.IntegerValue + b.IntegerValue);
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return new Value((a.IntegerValue + b.IntegerValue) != 0 ? true : false);
            else if ((a.Type == ValueTypes.OrderedPair) | (b.Type == ValueTypes.OrderedPair))
                return new Value(new OrderedPair(a.OrderedPairValue.X + b.OrderedPairValue.X, a.OrderedPairValue.Y + b.OrderedPairValue.Y));
            else
                return new Value();
        }

        public static Value operator -(Value a, Value b)
        {
            if (IsNull(a) | IsNull(b)) throw new NullReferenceException();

            if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
                return new Value(a.FloatValue - b.FloatValue);
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return new Value(a.IntegerValue - b.IntegerValue);
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return new Value((a.IntegerValue - b.IntegerValue) != 0 ? true : false);
            else if ((a.Type == ValueTypes.OrderedPair) | (b.Type == ValueTypes.OrderedPair))
                return new Value(new OrderedPair(a.OrderedPairValue.X - b.OrderedPairValue.X, a.OrderedPairValue.Y - b.OrderedPairValue.Y));
            else
                return new Value(a.FloatValue - b.FloatValue);
        }

        public static Value operator *(Value a, Value b)
        {
            if (IsNull(a) | IsNull(b)) throw new NullReferenceException();

            if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
                return new Value(a.FloatValue * b.FloatValue);
            else if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return new Value(a.IntegerValue * b.IntegerValue);
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return new Value((a.IntegerValue * b.IntegerValue) != 0 ? true : false);
            else if ((a.Type == ValueTypes.OrderedPair) && (b.Type == ValueTypes.OrderedPair))
                return new Value(new OrderedPair(a.OrderedPairValue.X * b.OrderedPairValue.X, a.OrderedPairValue.Y * b.OrderedPairValue.Y));
            else if (a.Type == ValueTypes.OrderedPair)
                return new Value(new OrderedPair(a.OrderedPairValue.X * b.IntegerValue, a.OrderedPairValue.Y * b.IntegerValue));
            else if (b.Type == ValueTypes.OrderedPair)
                return new Value(new OrderedPair(b.OrderedPairValue.X * a.IntegerValue, b.OrderedPairValue.Y * a.IntegerValue));
            else
                return new Value(a.FloatValue * b.FloatValue);
        }

        public static Value operator /(Value a, Value b)
        {
            if (IsNull(a) | IsNull(b)) throw new NullReferenceException();

            if ((a.Type == ValueTypes.Float) | (b.Type == ValueTypes.Float))
            {
                if (b.FloatValue.CompareTo(0f) == 0)
                    throw new DivideByZeroException();
                return new Value(a.FloatValue / b.FloatValue);
            }
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
            {
                if (b.IntegerValue == 0)
                    throw new DivideByZeroException();
                return new Value((a.IntegerValue / b.IntegerValue) != 0 ? true : false);
            }
            else if ((a.Type == ValueTypes.OrderedPair) && (b.Type == ValueTypes.OrderedPair))
            {
                if ((b.OrderedPairValue.Y == 0) | (b.OrderedPairValue.X == 0))
                    throw new DivideByZeroException();
                return new Value(new OrderedPair(a.OrderedPairValue.X / b.OrderedPairValue.X, a.OrderedPairValue.Y / b.OrderedPairValue.Y));
            }
            else if (a.Type == ValueTypes.OrderedPair)
            {
                if (b.IntegerValue == 0)
                    throw new DivideByZeroException();
                return new Value(new OrderedPair(a.OrderedPairValue.X / b.IntegerValue, a.OrderedPairValue.Y / b.IntegerValue));
            }
            else
            {
                if (b.FloatValue.CompareTo(0f) == 0)
                    throw new DivideByZeroException();
                return new Value(a.FloatValue / b.FloatValue);
            }
        }

        public static Value operator %(Value a, Value b)
        {
            if (IsNull(a) | IsNull(b)) throw new NullReferenceException();

            if ((a.Type == ValueTypes.OrderedPair) | (b.Type == ValueTypes.OrderedPair))
            {
                if ((b.OrderedPairValue.X == 0) | (b.OrderedPairValue.Y == 0))
                    throw new DivideByZeroException();
                return new Value(new OrderedPair(a.OrderedPairValue.X % b.OrderedPairValue.X, a.OrderedPairValue.Y % b.OrderedPairValue.Y));
            }
            else if (a.Type == ValueTypes.OrderedPair)
            {
                if (b.IntegerValue == 0)
                    throw new DivideByZeroException();
                return new Value(new OrderedPair(a.OrderedPairValue.X % b.IntegerValue, a.OrderedPairValue.Y % b.IntegerValue));
            }
            else
            {
                if (b.IntegerValue == 0)
                    throw new DivideByZeroException();
                return new Value(a.IntegerValue % b.IntegerValue);
            }
        }

        public static Value RaiseToPower(Value x, Value power)
        {
            if (IsNull(x) | IsNull(power)) throw new NullReferenceException();

            if ((x.Type == ValueTypes.Float) | (power.Type == ValueTypes.Float))
                return new Value(Math.Pow(x.FloatValue, power.FloatValue));
            else if ((x.Type == ValueTypes.Integer) | (power.Type == ValueTypes.Integer))
                return new Value((int)Math.Pow(x.FloatValue, power.FloatValue));
            else if ((x.Type == ValueTypes.Boolean) | (power.Type == ValueTypes.Boolean))
                return new Value(((int)Math.Pow(x.FloatValue, power.FloatValue)) != 0 ? true : false);
            else if ((x.Type == ValueTypes.OrderedPair) && (power.Type == ValueTypes.OrderedPair))
                return new Value(new OrderedPair((int)Math.Pow(x.OrderedPairValue.X, power.OrderedPairValue.X),
                    (int)Math.Pow(x.OrderedPairValue.Y, power.OrderedPairValue.Y)));
            else if (x.Type == ValueTypes.OrderedPair)
                return new Value(new OrderedPair((int)Math.Pow(x.OrderedPairValue.X, power.FloatValue),
                    (int)Math.Pow(x.OrderedPairValue.Y, power.FloatValue)));
            else
                return new Value(Math.Pow(x.FloatValue, power.FloatValue));
        }

        public static Value operator &(Value a, Value b)
        {
            if (IsNull(a) | IsNull(b)) throw new NullReferenceException();

            if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return new Value(a.IntegerValue & b.IntegerValue);
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return new Value(a.BooleanValue & b.BooleanValue);
            else if ((a.Type == ValueTypes.OrderedPair) && (b.Type == ValueTypes.OrderedPair))
                return new Value(new OrderedPair(a.OrderedPairValue.X & b.OrderedPairValue.X, a.OrderedPairValue.Y & b.OrderedPairValue.Y));
            else if (a.Type == ValueTypes.OrderedPair)
                return new Value(new OrderedPair(a.OrderedPairValue.Y & b.IntegerValue, a.OrderedPairValue.Y & b.IntegerValue));
            else
                return new Value(a.IntegerValue & b.IntegerValue);
        }

        public static Value operator |(Value a, Value b)
        {
            if (IsNull(a) | IsNull(b)) throw new NullReferenceException();

            if ((a.Type == ValueTypes.Integer) | (b.Type == ValueTypes.Integer))
                return new Value(a.IntegerValue | b.IntegerValue);
            else if ((a.Type == ValueTypes.Boolean) | (b.Type == ValueTypes.Boolean))
                return new Value(a.BooleanValue | b.BooleanValue);
            else if ((a.Type == ValueTypes.OrderedPair) && (b.Type == ValueTypes.OrderedPair))
                return new Value(new OrderedPair(a.OrderedPairValue.X | b.OrderedPairValue.X, a.OrderedPairValue.Y | b.OrderedPairValue.Y));
            else if (a.Type == ValueTypes.OrderedPair)
                return new Value(new OrderedPair(a.OrderedPairValue.Y | b.IntegerValue, a.OrderedPairValue.Y | b.IntegerValue));
            else
                return new Value(a.IntegerValue | b.IntegerValue);
        }

        #endregion

        #region Unary Operations

        public static Value operator +(Value x)
        {
            if (IsNull(x)) throw new NullReferenceException();
            return x;
        }

        public static Value operator -(Value x)
        {
            if (IsNull(x)) throw new NullReferenceException();

            if (x.Type == ValueTypes.String)
                return x;
            else if (x.Type == ValueTypes.Float)
                return new Value(x.FloatValue * -1);
            else if (x.Type == ValueTypes.Integer)
                return new Value(x.IntegerValue * -1);
            else if (x.Type == ValueTypes.Boolean)
                return new Value((x.IntegerValue * -1) != 0 ? true : false);
            else if (x.Type == ValueTypes.OrderedPair)
                return new Value(new OrderedPair(x.OrderedPairValue.X * -1, x.OrderedPairValue.Y * -1));
            else
                return new Value();
        }

        public static Value operator ~(Value x)
        {
            if (IsNull(x)) throw new NullReferenceException();

            if (x.Type == ValueTypes.Integer)
                return new Value(~x.IntegerValue);
            else if (x.Type == ValueTypes.Boolean)
                return new Value((~x.IntegerValue) != 0 ? true : false);
            else if (x.Type == ValueTypes.OrderedPair)
                return new Value(new OrderedPair(~x.OrderedPairValue.X, ~x.OrderedPairValue.Y));
            else
                return new Value(~x.IntegerValue);
        }

        public static Value operator !(Value x)
        {
            if (IsNull(x)) throw new NullReferenceException();

            return new Value(!x.BooleanValue);
        }

        public static Value operator ++(Value x)
        {
            if (IsNull(x)) throw new NullReferenceException();

            if (x.Type == ValueTypes.Float)
                return new Value(x.FloatValue + 1);
            else if (x.Type == ValueTypes.Integer)
                return new Value(x.IntegerValue + 1);
            else if (x.Type == ValueTypes.Boolean)
                return new Value((x.IntegerValue + 1) != 0 ? true : false);
            else if (x.Type == ValueTypes.OrderedPair)
                return new Value(new OrderedPair(x.OrderedPairValue.X + 1, x.OrderedPairValue.Y + 1));
            else
                return new Value(x.FloatValue + 1);
        }

        public static Value operator --(Value x)
        {
            if (IsNull(x)) throw new NullReferenceException();

            if (x.Type == ValueTypes.String)
                throw new InvalidOperationException("Cannot perform decrementation with a string value.");
            else if (x.Type == ValueTypes.Float)
                return new Value(x.FloatValue - 1);
            else if (x.Type == ValueTypes.Integer)
                return new Value(x.IntegerValue - 1);
            else if (x.Type == ValueTypes.Boolean)
                return new Value((x.IntegerValue - 1) != 0 ? true : false);
            else if (x.Type == ValueTypes.OrderedPair)
                return new Value(new OrderedPair(x.OrderedPairValue.X - 1, x.OrderedPairValue.Y - 1));
            else
                return new Value();
        }

        #endregion

        #region Casting

        public static implicit operator Value(double x)
        {
            return new Value(x);
        }

        public static implicit operator Value(int x)
        {
            return new Value(x);
        }

        public static implicit operator Value(string x)
        {
            return new Value(x);
        }

        public static implicit operator Value(bool x)
        {
            return new Value(x);
        }

        #endregion

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
