using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerbalScriptEngine
{
    struct LineInfo
    {
        /// <summary>
        /// Line of script.
        /// </summary>
        public string Line { get; private set; }

        /// <summary>
        /// The file the line is from.
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// The line in the file.
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// The column that the line starts on.
        /// </summary>
        public int ColumnOffset { get; private set; }

        /// <summary>
        /// Errors that occured while tokenizing.
        /// </summary>
        public string[] Errors { get; private set; }

        /// <summary>
        /// Tokenized version of the line.
        /// </summary>
        public string[] Tokens { get; private set; }

        /// <summary>
        /// Process the script is running on.
        /// </summary>
        public ScriptProcess Process { get; private set; }

        /// <summary>
        /// Instantiates the LineInfo structure.
        /// </summary>
        /// <param name="line">String containing the actual code.</param>
        /// <param name="filename">File the code came from.</param>
        /// <param name="lineNumber">Line number of the code.</param>
        /// <param name="columnOffset"></param>
        /// <param name="process"></param>
        public LineInfo(string line, string filename, int lineNumber, int columnOffset, ScriptProcess process)
            : this()
        {
            Line = line;
            Filename = filename;
            LineNumber = lineNumber;
            ColumnOffset = columnOffset;

            string[] err;
            Tokens = Tokenizer.Tokenize(this, out err);
            Errors = err;
        }
    }
}
