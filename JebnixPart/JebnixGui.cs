using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JebnixPart
{
    class JebnixGui
    {
        /// <summary>
        /// The position/size of the window
        /// </summary>
        Rect windowPos;
        int x, y, width, height;
        
        /// <summary>
        /// Creates the GUI window and assigns its position and size;
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public JebnixGui(int x, int y, int width, int height)
        {
            GUIStyle windowStyle = new GUIStyle(GUI.skin.window);

        }

        /// <summary>
        /// Draws the window
        /// </summary>
        private void Draw()
        {
        }
    }
}
