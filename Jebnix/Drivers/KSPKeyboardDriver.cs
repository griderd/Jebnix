using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Jebnix.Drivers
{
    public class KSPKeyboardDriver : IKeyboardDriver<KeyCode>
    {

        public char ProcessKeyboard(KeyCode keycode, bool alt, bool ctrl, bool shift, bool capsLock)
        {
            throw new NotImplementedException();
        }
    }
}
