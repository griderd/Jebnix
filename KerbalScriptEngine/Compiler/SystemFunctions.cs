using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix;
using Jebnix.Types;

namespace KerboScriptEngine.Compiler
{
    partial class Parser
    {
        /// <summary>
        /// Determines if there is a token. If there is no token, an error is reported to the ErrorBuilder with the provided string.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        //private bool ExpectToken(string expected)
        //{
        //    Token t = currentToken;
        //    GetToken();
        //    if (!HasToken())
        //    {
        //        ErrorBuilder.BuildError(t, "Expected " + expected, ref errors);
        //        return false;
        //    }
        //    return true;
        //}
        private bool ExpectToken(string expected)
        {
            if (currentTokenIndex + 1 == tokens.Count)
            {
                ErrorBuilder.BuildError(currentToken, "Expected " + expected, ref errors);
                return false;
            }
            return true;
        }

        private void Clearscreen()
        {
            if (!ExpectToken("'.'"))
                return;

            if (currentToken.Text == ".")
            {
                Call(new Pseudopointer(Functions.GenerateUniqueName("stdio", "clearscreen", 0)));
            }
        }

        private void Print()
        {
            if (!ExpectToken("string."))
                return;

            ParseExpression(Segment.Code, ".");
            Call(new Pseudopointer(Functions.GenerateUniqueName("stdio", "println", 1)));
        }

        private void Run()
        {
            if (!ExpectToken("script name."))
                return;
            string scriptName = currentToken.Text;

            if (!ExpectToken("'(' or '.'"))
                return;

            if (currentToken.Text == "(")
            {
                currentTokenIndex--;
                ParseExpression(Segment.Code, ".");
            }
            else if (currentToken.Text != ".")
            {
                ErrorBuilder.BuildError(currentToken, "Expected '(' or '.'", ref errors);
                return;
            }

            Pushl(new JString(scriptName));
            Call(new Pseudopointer("system.run"));
        }

        public void Copy()
        {
            if (!ExpectToken("file name"))
                return;
            string filename = currentToken.Text;
            string folder;
            bool to = false;

            if (!ExpectToken("'TO' or 'FROM'"))
                return;

            if (currentToken.Text == "to")
                to = true;
            else if (currentToken.Text == "from")
                to = false;
            else
            {
                ErrorBuilder.BuildError(currentToken, "Expected 'to' or 'from'.", ref errors);
                return;
            }

            if (!ExpectToken("folder name"))
                return;
            folder = currentToken.Text;
            Pushl(new JString(filename));
            Pushl(new JString(folder));

            if (!ExpectToken("'.'"))
                return;

            if (to)
                Call(new Pseudopointer("system.copyto"));
            else
                Call(new Pseudopointer("system.copyfrom"));
                
        }

        public void Delete()
        {
            if (!ExpectToken("file name"))
                return;

            string filename = currentToken.Text;

            if (!ExpectToken("'.'"))
                return;

            Pushl(new JString(filename));
            Call(new Pseudopointer("system.delete"));
        }
    }
}
