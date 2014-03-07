using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KerboScriptEngine;
using Jebnix.FileSystem;

namespace JebnixRPM
{
    public class Monitor : InternalModule  
    {
        Interpreter kos;

        public override void OnLoad(ConfigNode node)
        {
            kos = new Interpreter(vessel, new System.IO.DirectoryInfo("PluginData\\Archive"));

            base.OnLoad(node);
        }

        public string UpdateRPM(int width, int height)
        {
            return Jebnix.Graphics.Graphics.Text;
        }
    }
}
