using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;

namespace Jebnix.stdlib
{
    public class stdvessel
    {
        public static PartResource[] GetResources(Vessel vessel)
        {
            List<PartResource> resources = new List<PartResource>();

            Func<string, bool> ContainsResource = delegate(string name)
            {
                foreach (PartResource r in resources)
                {
                    if (r.resourceName == name)
                        return true;
                }
                return false;
            };

            foreach (Part p in vessel.Parts)
            {
                foreach (PartResource r in p.Resources)
                {
                    if (!ContainsResource(r.resourceName))
                        resources.Add(r);
                }
            }

            return resources.ToArray();
        }

        public static PartResource[] GetResources(Interpreter interpreter)
        {
            return GetResources(interpreter.vessel);
        }
    }
}
