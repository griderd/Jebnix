using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix
{
    public class Settings
    {
        /// <summary>
        /// Changes settings to default.
        /// </summary>
        public static void Initialize()
        {
            UsePrompts = false;
            ShowPath = true;
            PromptCharacter = '>';
        }

        /// <summary>
        /// Gets or sets a value that determines whether to use prompts.
        /// </summary>
        public static bool UsePrompts { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether to show the current path with prompts.
        /// </summary>
        public static bool ShowPath { get; set; }

        /// <summary>
        /// Gets or sets the character to use as a prompt.
        /// </summary>
        public static char PromptCharacter { get; set; }
    }
}
