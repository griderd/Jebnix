using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Types.Structures
{
    public class Vector : JObject 
    {
        JFloat x, y, z;

        public const string TYPENAME = "Vector";

        public Vector()
            : base(false, TYPENAME)
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Vector(JFloat x, JFloat y, JFloat z)
            : this()
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(Vector3d vector)
            : this()
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public JFloat X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public JFloat Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public JFloat Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
            }
        }

        public JFloat Magnitude
        {
            get
            {
                return stdlib.stdmath.Sqrt(x * x + y * y + z * z);
            }
        }

        public Vector Normalized
        {
            get
            {
                return new Vector(ToVector3d().normalized);
            }
        }

        public JFloat DotProduct(Vector v)
        {
            return x * v.x + y * v.y + z * v.z;
        }

        public JFloat AngleBetween(Vector v)
        {
            return stdlib.stdmath.ACos(DotProduct(v) / (Magnitude * v.Magnitude));
        }

        public Vector3d ToVector3d()
        {
            return new Vector3d(x, y, z);
        }

        protected override bool IsEqual(JObject a)
        {
            if (!IsSameType(a))
                return false;

            Vector v = (Vector)a;
            return x == v.x &
                   y == v.y &
                   z == v.z;
        }

        protected override bool IsNotEqual(JObject a)
        {
            return !IsEqual(a);
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
            {
                Vector v = (Vector)a;
                return new Vector(x + v.x, y + v.y, z + v.z);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        protected override JObject Subtract(JObject a)
        {
            if (IsSameType(a))
            {
                Vector v = (Vector)a;
                return new Vector(x - v.x, y - v.y, z - v.z);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        protected override JObject Multiply(JObject a)
        {
            if (IsSameType(a))
            {
                Vector v = (Vector)a;
                return new Vector(x * v.x, y * v.y, z * v.z);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        protected override JObject Divide(JObject a)
        {
            throw new InvalidOperationException();
        }

        protected override JObject Modulus(JObject a)
        {
            throw new InvalidOperationException();
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
