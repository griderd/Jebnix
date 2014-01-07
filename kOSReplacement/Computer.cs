using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;

namespace Jebnix
{
    public class Computer : PartModule 
    {
        VirtualMachine vm;

        public override void OnStart(StartState state)
        {
            vm = new VirtualMachine(10);
            vm.ScreenRefresh += new EventHandler(vm_ScreenRefresh);

            if (state != StartState.Editor)
            {

            }
            
            base.OnStart(state);
        }

        void vm_ScreenRefresh(object sender, EventArgs e)
        {
            stdlib.stdio.UpdateScreen();
        }
    }
}
