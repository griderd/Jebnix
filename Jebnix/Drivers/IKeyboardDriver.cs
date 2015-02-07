using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Drivers
{
    public interface IKeyboardDriver<KeyCodeType>
    {
        char ProcessKeyboard(KeyCodeType keycode, bool alt, bool ctrl, bool shift, bool capsLock);
    }
}
