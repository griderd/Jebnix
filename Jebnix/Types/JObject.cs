using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Jebnix.Types.BasicTypes;
using Jebnix.Types.Structures;

namespace Jebnix.Types
{
    /// <summary>
    /// Represents a Jebnix object. 
    /// </summary>
    public abstract class JObject
    {
        public const string TYPENAME = "Object";

        public JObject(bool isNull, string objectType)
        {
            IsNull = isNull;
            ObjectType = objectType;
        }

        public bool IsSameType(JObject value)
        {
            return (value.ObjectType == ObjectType);
        }

        public string ObjectType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value determining if the value of the JObject is NULL.
        /// </summary>
        public virtual bool IsNull
        {
            get;
            protected set;
        }

        /// <summary>
        /// Determines if a JObject has a particular function.
        /// </summary>
        /// <param name="obj">JObject to check.</param>
        /// <param name="name">Function name</param>
        /// <returns>Returns true if the function exists. Otherwise returns false.</returns>
        public static bool HasFunction(JObject obj, string name)
        {
            if (obj == null)
                return false;

            MethodInfo func = obj.GetType().GetMethod(name);
            return func != null;
        }

        /// <summary>
        /// Invokes the function of the given name in the given JObject.
        /// </summary>
        /// <param name="obj">JObject to use</param>
        /// <param name="name">Function name</param>
        /// <param name="parameters">Function parameters</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool InvokeFunction(JObject obj, string name, JObject[] parameters, out JObject result)
        {
            if (obj == null)
            {
                result = null;
                return false;
            }

            MethodInfo func = obj.GetType().GetMethod(name);
            if (func != null)
            {
                result = (JObject)func.Invoke(obj, parameters);
                return true;
            }
            result = null;
            return false;
        }

        public static bool GetProperty(JObject obj, string name, out JObject result)
        {
            if (obj == null)
            {
                result = null;
                return false;
            }

            PropertyInfo property = obj.GetType().GetProperty(name);
            if (property != null)
            {
                if (property.CanRead)
                {
                    result = (JObject)property.GetValue(obj, null);
                    return true;
                }
            }
            result = null;
            return false;
        }

        public static bool SetProperty(JObject obj, string name, JObject v)
        {
            if (obj == null)
            {
                return false;
            }

            PropertyInfo property = obj.GetType().GetProperty(name);
            if (property != null)
            {
                if (property.CanWrite)
                {
                    property.SetValue(obj, v, null);
                    return true;
                }
            }
            return false; 
        }

        public static bool HasProperty(JObject obj, string name)
        {
            if (obj == null)
                return false;

            PropertyInfo property = obj.GetType().GetProperty(name);
            return property != null;
        }

        #region Abstract Operators

        protected abstract bool IsEqual(JObject a);
        protected abstract bool IsNotEqual(JObject a);
        protected abstract JObject IsLessThan(JObject a);
        protected abstract JObject IsLessThanOrEqual(JObject a);
        protected abstract JObject IsGreaterThan(JObject a);
        protected abstract JObject IsGreaterThanOrEqual(JObject a);
        protected abstract JObject Add(JObject a);
        protected abstract JObject Subtract(JObject a);
        protected abstract JObject Multiply(JObject a);
        protected abstract JObject Divide(JObject a);
        protected abstract JObject Modulus(JObject a);
        protected abstract JObject Pow(JObject a);
        protected abstract JObject And(JObject a);
        protected abstract JObject Or(JObject a);
        protected abstract JObject Positive();
        protected abstract JObject Negative();
        protected abstract JObject Not();
        protected abstract JObject Increment();
        protected abstract JObject Decrement();
        
        #endregion

        #region Binary Operations

        public static bool operator ==(JObject a, JObject b)
        {
            try
            {
                return a.IsEqual(b);
            }
            catch
            {
                throw;
            }
        }


        public static bool operator !=(JObject a, JObject b)
        {
            try
            {
                return a.IsNotEqual(b);
            }
            catch
            {
                throw;
            }
        }

        public static JObject operator >(JObject a, JObject b)
        {
            try
            {
                return a.IsGreaterThan(b);
            }
            catch
            {
                throw;
            }
        }

        public static JObject operator >=(JObject a, JObject b)
        {
            try
            {
                return a.IsGreaterThanOrEqual(b);
            }
            catch
            {
                throw;
            }
        }

        public static JObject operator <=(JObject a, JObject b)
        {
            try
            {
                return a.IsLessThanOrEqual(b);
            }
            catch
            {
                throw;
            }
        }

        public static JObject operator <(JObject a, JObject b)
        {
            try
            {
                return a.IsLessThan(b);
            }
            catch
            {
                throw;
            }
        }

        public static JObject operator +(JObject a, JObject b)
        {
            try
            {
                return a.Add(b);
            }
            catch
            {
                throw;
            }
        }

        public static JObject operator -(JObject a, JObject b)
        {
            try
            {
                return a.Subtract(b);
            }
            catch
            {
                throw;
            }
        }

        public static JObject operator *(JObject a, JObject b)
        {
            return a.Multiply(b);
        }

        public static JObject operator /(JObject a, JObject b)
        {
            return a.Divide(b);
        }

        public static JObject operator %(JObject a, JObject b)
        {
            return a.Modulus(b);
        }

        public static JObject RaiseToPower(JObject x, JObject power)
        {
            return x.Pow(power);
        }

        public static JObject operator &(JObject a, JObject b)
        {
            return a.And(b);
        }

        public static JObject operator |(JObject a, JObject b)
        {
            return a.Or(b);
        }

        #endregion

        #region Unary Operations

        public static JObject operator +(JObject x)
        {
            return x.Positive();
        }

        public static JObject operator -(JObject x)
        {
            return x.Negative();
        }

        public static JObject operator ~(JObject x)
        {
            return x.Not();
        }

        public static JObject operator !(JObject x)
        {
            return x.Not();
        }

        public static JObject operator ++(JObject x)
        {
            return x.Increment();
        }

        public static JObject operator --(JObject x)
        {
            return x.Decrement();
        }

        #endregion

        
    }
}
