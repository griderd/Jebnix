using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIOS.Types;

namespace KerboScriptEngine
{
    /// <summary>
    /// Provides a parser engine for interactive code and code syntax checking. Ensures that code is (mostly) free of syntax errors before being run.
    /// </summary>
    class Parser
    {
        Stack<ExecutionState> stateStack;

        ExecutionState CurrentState
        {
            get
            {
                return stateStack.Pop();
            }
        }

        Dictionary<string, Value> LocalScope
        {
            get
            {
                return CurrentState.scopeStack.Pop();
            }
        }

        void NewScope(ExecutionState.Status status)
        {
            CurrentState.scopeStack.Push(new Dictionary<string, Value>());
            CurrentState.statusStack.Push(status);
        }

        bool ExitScope()
        {
            if (CurrentState.scopeStack.Count > 1)
            {
                CurrentState.scopeStack.Pop();
                CurrentState.statusStack.Pop();
                return true;
            }
            return false;
        }

        public Parser(LineInfo[] lines)
        {
            stateStack = new Stack<ExecutionState>();
            stateStack.Push(new ExecutionState(lines));
        }

        public void AddLine(LineInfo line)
        {
            CurrentState.lines.Add(line);
        }

        public void AddLines(LineInfo[] lines)
        {
            CurrentState.lines.AddRange(lines);
        }

        public Tuple<int, int> Parse(int lineIndex, int tokenIndex, LineInfo[] lines, out string[] err)
        {
            LineInfo line = lines[lineIndex];

            string token;
            List<string> errors = new List<string>();

            int i = tokenIndex;
            token = line.Tokens[i];

            Action<ErrorBuilder.ErrorType, string> ThrowError = delegate(ErrorBuilder.ErrorType t, string message)
            {
                ErrorBuilder.BuildError(line, t, message, ref errors);
            };

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

            Func<string[], string> GetNextExpression = delegate(string[] expressionTerminators)
            {
                StringBuilder s = new StringBuilder();

                GetNextToken();
                if (expressionTerminators.Contains(token))
                    return "";
                else
                {
                    while (!expressionTerminators.Contains(token))
                    {
                        s.Append(token);
                        GetNextToken();
                    }

                    return s.ToString();
                }
            };

            Action AdvanceToEndOfScope = delegate()
            {
                Stack<object> scope = new Stack<object>();

                if (token == "{")
                {
                    scope.Push(null);

                    while (scope.Count > 0)
                    {
                        GetNextToken();
                        if (token == "{")
                            scope.Push(null);
                        else if (token == "}")
                        {
                            if (scope.Count > 0)
                                scope.Pop();
                            else
                            {
                                ThrowError(ErrorBuilder.ErrorType.SyntaxError, "'}' without matching '{'.");
                                break;
                            }
                        }
                    }
                    GetNextToken();
                }
            };

            switch (token)
            {
                case "{":
                    {
                        NewScope(ExecutionState.Status.GenericBlock);
                        break;
                    }

                case "}":
                    {
                        if (!ExitScope())
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "'}' without matching '{'.");
                        break;
                    }

                case "set":
                    {
                        GetNextToken();
                        bool local = (token == "local");
                        if (local) GetNextToken();
                        if (!ReservedWords.IsReserved(token))
                        {
                            string name = token;
                            GetNextToken();
                            if ((token != "to") & (token != "="))
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
                        bool parameter = false;
                        parameter = (token == "parameter");
                        GetNextToken();
                        if (!ReservedWords.IsReserved(token))
                        {
                            if (parameter)
                            {
                                if (CurrentState.parameters.ContainsKey(token))
                                    ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Parameter with name \"" + token + "\" already exists.");
                            }
                        }
                        else
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Identifier cannot be reserved word.");
                        }
                    }
                    break;

                case "print":
                    {
                        string s = GetNextExpression(new string[] { "at", "." });
                        if (s == "")
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "No expression provided. Cannot run PRINT.");
                            break;
                        }

                        if (token == "at")
                        {
                            s = GetNextExpression(new string[] { "." });
                            if (s == "")
                            {
                                ThrowError(ErrorBuilder.ErrorType.SyntaxError, "No ordered pair provided. Cannot run PRINT.");
                                break;
                            }
                        }
                        break;
                    }

                case "clearscreen":
                    GetNextToken();
                    if (token != ".")
                        ThrowError(ErrorBuilder.ErrorType.SyntaxError, "'.' expected.");
                    break;

                case "if":
                    {
                        string s = GetNextExpression(new string[] { "{" });
                        if (s == "")
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Expression expected.");
                            break;
                        }
                        else
                        {
                            NewScope(ExecutionState.Status.IfStatement);
                        }
                    }
                    break;

                case "else":
                    {
                        // TODO: determine if "IF" preceeded "ELSE"
                        NewScope(ExecutionState.Status.ElseStatement);
                    }
                    break;
            }


            err = errors.ToArray();
            return new Tuple<int, int>(lineIndex, i);
        }
    }
}
