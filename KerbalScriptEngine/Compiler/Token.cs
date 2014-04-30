using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerboScriptEngine.Compiler
{
    struct Token
    {
        public string FileName { get; private set; }
        public string Text { get; private set; }
        public int LineNumber { get; private set; }

        public Token(string text, string filename, int lineNumber)
            : this()
        {
            FileName = filename;
            Text = text;
            LineNumber = lineNumber;
        }
    }
}
