using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.Types;
using Jebnix.FileSystem;

namespace KerboScriptEngine.Compiler
{
    class KSPPCompiler
    {
        public static Script Compile(string[] lines, string filename, out string[] err)
        {
            string[] errors;

            // Tokenize
            Token[] tokens = Tokenizer.Tokenize(lines, filename, out errors);

            // If there are errors, cancel the compilation.
            if (errors.Length > 0)
            {
                err = errors;
                return null;
            }

            Parser p = new Parser(tokens);
            err = p.GetErrors();
            JObject[] objects = p.GetObjectCode();

            Script script = new Script(filename, objects);
            return script;
        }

        public static Script Compile(File file, out string[] err)
        {
            return Compile(file.Lines, file.Name, out err);
        }

        
    }
}
