using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerboScriptEngine
{
    partial class ScriptProcess
    {
        /// <summary>
        /// Executes a single statement in the given lines starting at the given index, and returns the location it stopped at.
        /// </summary>
        /// <param name="lineIndex"></param>
        /// <param name="lines"></param>
        /// <param name="err"></param>
        /// <returns>Returns a tuple containing the lineIndex stopped at and the token stopped at.</returns>
        private Tuple<int, int> Execute(int lineIndex, int tokenIndex, LineInfo[] lines, out string[] err)
        {
            LineInfo line = lines[lineIndex];

            string token;
            List<string> errors = new List<string>();

            int i = tokenIndex;
            token = line.Tokens[i];

            Action GetNextLine = delegate()
            {
                lineIndex++;
                if (lineIndex < lines.Length)
                {
                    line = lines[lineIndex];
                    i = 0;
                }
                else
                    lineIndex--;
            };

            Action GetNextToken = delegate()
            {
                i++;
                if (i < line.Tokens.Length)
                    token = line.Tokens[i];
                else
                    GetNextLine();
                //ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.SyntaxError, "Line ended unexpectedly. Are you missing a '.'?", ref errors);
            };

            switch (token)
            {
                case "set":
                    {
                        CanAddParameters = false;
                        GetNextToken();
                        bool local = (token == "local");
                        if (local) GetNextToken();
                        if (!ReservedWords.IsReserved(token))
                        {
                            string name = token;
                            GetNextToken();
                            if (token == "to" | token == "=")
                            {
                                StringBuilder s = new StringBuilder();
                                GetNextToken();
                                while (token != ".")
                                {
                                    s.Append(token);
                                    s.Append(" ");
                                    GetNextToken();
                                }
                                string[] ex;
                                Value var = Evaluator.Evaluate(new LineInfo(s.ToString(), line.Filename, line.LineNumber, line.ColumnOffset, this), out ex);
                                errors.AddRange(ex);

                                if (ex.Length == 0)
                                {
                                    if (local)
                                        SetVariable(name, var);
                                    else
                                        Parent.SetVariable(name, var);
                                }
                            }
                            else
                            {
                                ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.SyntaxError, "Expected keyword 'TO' or '='.", ref errors);
                            }
                        }
                        else
                        {
                            ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.SyntaxError, "Identifier cannot be reserved word.", ref errors);
                        }
                    }
                    break;

                case "declare":
                    {
                        GetNextToken();
                        bool local = false;
                        bool parameter = false;
                        local = (token == "local");
                        parameter = (token == "parameter");
                        GetNextToken();
                        if (!ReservedWords.IsReserved(token))
                        {
                            Value var = Value.NullValue;
                            if (local)
                                SetVariable(token, var);
                            else if (parameter)
                            {
                                if ((!CurrentState.parameters.ContainsKey(token)) & (CanAddParameters))
                                    CurrentState.parameters.Add(token, var);
                                else if (!CanAddParameters)
                                    ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.SyntaxError, "Parameters must be added ahead of any other code.", ref errors);
                                else
                                    ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.SyntaxError, "Parameter with the name \"" + token + "\" already exists.", ref errors);
                            }
                        }
                        else
                        {
                            ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.SyntaxError, "Identifier cannot be reserved word.", ref errors);
                        }
                    }
                    break;

                case "print":
                    {
                        StringBuilder s = new StringBuilder();
                        GetNextToken();
                        if ((token == "at") | (token == "."))
                            ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.SyntaxError, "No expression provided. Cannot PRINT,", ref errors);
                        while ((token != "at") & (token != "."))
                        {
                            s.Append(token);
                        }
                    }
            }


            err = errors.ToArray();
            return new Tuple<int, int>(lineIndex, i);
        }
    }
}
