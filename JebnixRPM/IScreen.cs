using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JebnixRPM
{
    interface IScreen
    {
        string Update();

        void OnButtonClick(int button);
    }
}
