using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix
{
    class Processor
    {
        public static void ProcessInput(string input, VirtualMachine vm)
        {
            switch (input)
            {
                case "clearscreen.":
                    stdlib.stdio.ClearScreen(); break;

                case "reset.":
                    vm.Stop();
                    vm.Start();
                    break;

                case "shutdown.":
                    vm.Stop();
                    break;
            }
        }
    }
}
