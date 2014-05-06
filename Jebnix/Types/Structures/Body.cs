using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.stdlib;

namespace Jebnix.Types.Structures
{
    public class Body : JObject 
    {
        CelestialBody body;
        Vessel vessel;
        JString name;
        JFloat radius;
        Body parent;
        JFloat mass;
        ValueArray satellites;

        public const string TYPENAME = "Body";

        public Body()
            : base(true, TYPENAME)
        {
            name = new JString(null);
            radius = 0;
            parent = new Body();
            mass = 0;
            satellites = new ValueArray();
            body = null;
        }

        public Body(CelestialBody body, Body parent, Vessel vessel)
            : base(false, TYPENAME)
        {
            this.body = body;
            name = new JString(body.bodyName);
            radius = body.Radius;
            this.parent = parent;
            mass = body.Mass;
            this.vessel = vessel;

            foreach (CelestialBody b in body.orbitingBodies)
            {
                satellites.Add(new Body(b, this, vessel));
            }
        }

        public JString Name
        {
            get
            {
                return name;
            }
        }

        public JFloat Radius
        {
            get
            {
                return radius;
            }
        }

        public JFloat Altitude
        {
            get
            {
                if (IsNull)
                    return 0;

                return new JFloat(Vector3d.Distance(vessel.GetWorldPos3D(), Position.ToVector3d()));
            }
        }

        public Vector Position
        {
            get
            {
                if (IsNull)
                    return new Vector();

                return new Vector(body.getPositionAtUT(stdtime.Now));
            }
        }

        public Vector GetPositionAtTime(JFloat time)
        {
            if (IsNull)
                return new Vector();

            return new Vector(body.getPositionAtUT(time));
        }

        protected override bool IsEqual(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override bool IsNotEqual(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject IsLessThan(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject IsLessThanOrEqual(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject IsGreaterThan(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject IsGreaterThanOrEqual(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Addition(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Subtract(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Multiply(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Divide(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Modulus(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Pow(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject And(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Or(JObject a)
        {
            throw new NotImplementedException();
        }

        protected override JObject Positive()
        {
            throw new NotImplementedException();
        }

        protected override JObject Negative()
        {
            throw new NotImplementedException();
        }

        protected override JObject Not()
        {
            throw new NotImplementedException();
        }

        protected override JObject Increment()
        {
            throw new NotImplementedException();
        }

        protected override JObject Decrement()
        {
            throw new NotImplementedException();
        }
    }
}
