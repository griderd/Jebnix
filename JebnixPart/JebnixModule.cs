using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;
using UnityEngine;
using Jebnix;

namespace JebnixPart
{
    public class JebnixModule : PartModule 
    {
        JebnixGui gui;

        public override void OnActive()
        {
            gui = new JebnixGui(Screen.width / 2, Screen.height / 2, 100, 100);

            base.OnActive();
        }
    }
}
