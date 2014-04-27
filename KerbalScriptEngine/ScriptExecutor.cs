using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Jebnix.stdlib;
using Jebnix.Types;
using KSP;

namespace KerboScriptEngine
{
    partial class ScriptProcess
    {
        private void ExecuteLine(LineInfo line, out string[] err)
        {
            ExecuteBlock(new LineInfo[] { line }, out err);
        }

        private void ExecuteBlock(LineInfo[] lines, out string[] err)
        {
            Tuple<int, int> position = new Tuple<int, int>(0, 0);
            err = new string[0];
            while ((position.Item1 != lines.Length - 1) && (position.Item2 != lines.Last().Tokens.Length - 1))
            {
                position = Execute(position.Item1, position.Item2, lines, out err);
                if (err.Length > 0)
                    break;
            }
        }

        /// <summary>
        /// Executes a single statement in the given lines starting at the given index, and returns the location it stopped at.
        /// </summary>
        /// <param name="lineIndex"></param>
        /// <param name="lines"></param>
        /// <param name="err"></param>
        /// <returns>Returns a tuple containing the lineIndex stopped at and the token stopped at.</returns>
        private Tuple<int, int> Execute(int lineIndex, int tokenIndex, LineInfo[] lines, out string[]  err)
        {
            LineInfo line = lines[lineIndex];
#if DEBUG
            Debug.Print("Executing line: " + line.Line);
#endif

            string token = "";
            List<string> errors = new List<string>();

            int i = tokenIndex;

            // Build and throw an error
            Action<ErrorBuilder.ErrorType, string> ThrowError = delegate(ErrorBuilder.ErrorType t, string message)
            {
                ErrorBuilder.BuildError(line, t, message, ref errors);
            };

            // Increments to the next line, if it exists.
            Func<bool> GetNextLine = delegate()
            {
                lineIndex++;
                if (lineIndex < lines.Length)
                {
                    line = lines[lineIndex];
                    i = 0;
                    return true;
                }
                else
                {
                    lineIndex--;
                    return false;
                }
            };

            // Increments to the next token, if it exists.
            Func<bool> GetNextToken = delegate()
            {
                i++;
                if (i < line.Tokens.Length)
                {
                    token = line.Tokens[i];
                }
                else
                {
                    if (!GetNextLine())
                        return false;
                    else
                        token = line.Tokens[i];
                }
                return true;
            };

            Func<string> ReadNextToken = delegate()
            {
                if (i + 1 < line.Tokens.Length)
                {
                    return line.Tokens[i + 1];
                }
                else if (lineIndex + 1 < lines.Length)
                {
                    return lines[lineIndex + 1].Tokens[0];
                }
                else
                {
                    return null;
                }
            };

            // Gets the tokens of the next expression.
            // Returns empty string when the expression is empty.
            // Returns null when the an expressionTerminator is missing.
            Func<string[], string> GetNextExpression = delegate(string[] expressionTerminators) 
            {
                StringBuilder s = new StringBuilder();

                if (!GetNextToken())
                    return null;

                if (expressionTerminators.Contains(token))
                    return "";
                else
                {
                    while (!expressionTerminators.Contains(token))
                    {
                        s.Append(token);
                        if (!GetNextToken())
                            return null;
                    }

                    return s.ToString();
                }
            };

            // Exits the current scope, ignoring trailing WHILE.
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

            if (((i > 0) || (lineIndex > 0)) && (!GetNextToken()))
            {
                err = errors.ToArray();
                return new Tuple<int, int>(lineIndex, tokenIndex);
            }
            else
            {
                token = line.Tokens[i];
            }

            switch (token)
            {
                case "{":
                    {
                        NewScope(ExecutionState.Status.GenericBlock);
                        break;
                    }

                case "}":
                    {
                        string[] e;
                        Tuple<int, int> newPosition;
                        if (!ExitScope(out e, out newPosition))
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "'}' without matching '{'.");
                        if (newPosition != null)
                        {
                            lineIndex = newPosition.Item1;
                            i = newPosition.Item2;
                        }
                        break;
                    }

                case "set":
                    {
                        CanAddParameters = false;
                        string nextToken = ReadNextToken();
                        bool local = (nextToken == "local");
                        if (local) GetNextToken();
                        
                        string name = GetNextExpression(new string[] { "to", "=" });
                        string[] ex1 = new string[0];

                        Value nameptr;
                        if (name.Contains('[') && name.Contains(']'))
                            nameptr = Evaluator.Evaluate(name, line, out ex1);
                        else
                            nameptr = name;

                        if (ex1.Length > 0)
                        {
                            errors.AddRange(ex1);
                            break;
                        }
                        if (!ReservedWords.IsValidIdentifier(nameptr.StringValue))
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Identifier not valid.");
                            break;
                        }

                        string expression = GetNextExpression(new string[] { "." });
                        string[] ex;
                        Value var = Evaluator.Evaluate(expression, line, out ex);
                        errors.AddRange(ex);

                        if (ex.Length == 0)
                        {
                            if (HasVariable(nameptr.StringValue))
                            {
                                SetVariable(nameptr.StringValue, var);
                            }
                            else if (local)
                            {
                                SetLocalVariable(nameptr.StringValue, var);
                            }
                            else
                            {
                                SetGlobalVariable(nameptr.StringValue, var);
                            }
                        }
                        else
                        {
                            errors.AddRange(ex);
                            break;
                        }
                    }
                    break;

                case "lock":
                    {
                        CanAddParameters = false;
                        GetNextToken();
                        if (!ReservedWords.IsReserved(token))
                        {
                            string name = token;
                            GetNextToken();
                            if (token == "to" | token == "=")
                            {
                                string expression = GetNextExpression(new string[] { "." });

                                if (expression == "")
                                {
                                    ThrowError(ErrorBuilder.ErrorType.SyntaxError, "No expression provided.");
                                    break;
                                }
                                if (expression == null)
                                {
                                    ThrowError(ErrorBuilder.ErrorType.SyntaxError, "\".\" expected.");
                                    break;
                                }

                                if (CurrentState.lockedVariables.ContainsKey(name))
                                {
                                    CurrentState.lockedVariables[name] = expression;
                                }
                                else
                                {
                                    CurrentState.lockedVariables.Add(name, expression);
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
                            Value var = null;
                            if (local)
                                SetVariable(token, var);
                            else if (parameter)
                            {
                                if ((!CurrentState.parameters.ContainsKey(token)) & (CanAddParameters))
                                    CurrentState.parameters.Add(token, var);
                                else if (!CanAddParameters)
                                    ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Parameters must be added ahead of any other code.");
                                else
                                    ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Parameter with name \"" + token + "\" already exists.");
                            }
                        }
                        else
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Identifier cannot be reserved word.");
                        }
                    }
                    break;

                case "call":
                    {
                        string[] ex;
                        string s = GetNextExpression(new string[] { "." });
                        if (s == null)
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Missing expression terminator \".\"");
                        }
                        if (s == "")
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "No expression provided. Cannot run CALL.");
                            break;
                        }

                        Evaluator.Evaluate(s, line, out ex);
                        errors.AddRange(ex);

                        break;
                    }

                case "print":
                    {
                        string[] ex;
                        string s = GetNextExpression(new string[] { "at", "." });
                        if (s == null)
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Missing expression terminator \"at\" or \".\".");
                            break;
                        }
                        if (s == "")
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "No expression provided. Cannot run PRINT.");
                            break;
                        }
                        
                        // Evaluate expression
                        Value output = Evaluator.Evaluate(s, line, out ex);
                        errors.AddRange(ex);
                        if (ex.Length > 0)
                            break;

                        if (token == "at")
                        {
                            s = GetNextExpression(new string[] { "." });
                            if (s == "")
                            {
                                ThrowError(ErrorBuilder.ErrorType.SyntaxError, "No ordered pair provided. Cannot run PRINT.");
                                break;
                            }
                            if (s == null)
                            {
                                ThrowError(ErrorBuilder.ErrorType.SyntaxError, "\".\" expected.");
                                break;
                            }
                            Value location = Evaluator.Evaluate(s, line, out ex);
                            errors.AddRange(ex);
                            if (ex.Length > 0)
                                break;

                            // TODO: print to output
                        }
                        else
                        {
                            stdio.PrintLine(output.StringValue);        
                        }
                        break;
                    }

                case "clearscreen":
                    GetNextToken();
                    if (token == ".")
                    {
                        stdio.ClearScreen();
                    }
                    else
                    {
                        ThrowError(ErrorBuilder.ErrorType.SyntaxError, "'.' expected.");
                    }
                    break;

                case "if":
                    {
                        string s = GetNextExpression(new string[] { "{" });
                        if (s == null)
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "\"{\" expected after IF.");
                            break;
                        }
                        else if (s == "")
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Expression expected.");
                            break;
                        } 
                        else
                        {
                            string[] ex;
                            Value v = Evaluator.Evaluate(s, line, out ex);
                            errors.AddRange(ex);
                            if (ex.Length > 0)
                                break;

                            if (v.BooleanValue)
                            {
                                NewScope(ExecutionState.Status.IfStatement);
                            }
                            else
                            {
                                AdvanceToEndOfScope();
                                if (token == "else")
                                {
                                    GetNextToken();
                                    if (token == "{")
                                        NewScope(ExecutionState.Status.ElseStatement);
                                    else
                                        ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Else requires \"{...}\" block.");
                                }
                            }
                        }
                    }
                    break;

                case "unlock":
                    {
                        GetNextToken();
                        string name = token;
                        GetNextToken();
                        if (token == ".")
                        {
                            if (!ReservedWords.IsReserved(name))
                            {
                                if (CurrentState.lockedVariables.ContainsKey(name))
                                {
                                    CurrentState.lockedVariables.Remove(name);
                                }
                                else
                                {
                                    //ThrowError(ErrorBuilder.ErrorType.RuntimeError, "Variable \"" + name + "\" not locked.");
                                }
                            }
                            else
                            {
                                ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Variable name \"" + name + "\" is a reserved word.");
                            }
                        }
                        else
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "'.' expected.");
                        }
                    }
                    break;

                case "list":
                    {
                        GetNextToken();
                        string lst = token;
                        GetNextToken();
                        if (token != ".")
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "'.' expected.");
                            break;
                        }
                        switch (lst)
                        {
                            case "volumes":
                                stdio.PrintLine("0 - Archive\t" + Parent.Archive.BytesUsed + "/" +
                                    (Parent.Archive.MaxCapacity > 0 ? Parent.Archive.MaxCapacity.ToString() : "Infinity") + " (" +
                                    (!double.IsInfinity(Parent.Archive.UsagePercent) ? Parent.Archive.UsagePercent.ToString("F2") : "Infinity") + "%)");
                                stdio.PrintLine("1 - Root   \t" + Parent.RootFolder.BytesUsed + "/" + 
                                    (Parent.RootFolder.MaxCapacity > 0 ? Parent.RootFolder.MaxCapacity.ToString() : "Infinity") + " (" +
                                    (!double.IsInfinity(Parent.RootFolder.UsagePercent) ? Parent.RootFolder.UsagePercent.ToString("F2") : "Infinity") + "%)");
                                stdio.PrintLine();
                                break;
                                
                            case "files":
                                stdio.PrintLine("~/" + Parent.CurrentFolder.Name);
                                stdio.PrintLine(Parent.CurrentFolder.Files.Length.ToString() + " files in directory.");
                                stdio.PrintLine();
                                foreach (Jebnix.FileSystem.File file in Parent.CurrentFolder.Files)
                                {
                                    stdio.PrintLine(file.Name + "\t" + file.Size + " bytes");
                                }
                                if (Parent.CurrentFolder.Files.Length > 0) stdio.PrintLine();
                                break;

                            case "parts":
                                if (Parent.vessel == null)
                                {
                                    ThrowError(ErrorBuilder.ErrorType.InternalError, "Vessel is currently NULL. Are you running outside KSP?");
                                    break;
                                }

                                stdio.PrintLine("Vessel: " + Parent.vessel.vesselName);
                                stdio.PrintLine(Parent.vessel.Parts.Count.ToString() + " parts in vessel.");
                                stdio.PrintLine();
                                foreach (Part p in Parent.vessel.Parts)
                                {
                                    stdio.PrintLine(p.partName);
                                }
                                if (Parent.vessel.Parts.Count > 0) stdio.PrintLine();
                                break;

                            case "resources":
                                if (Parent.vessel == null)
                                {
                                    ThrowError(ErrorBuilder.ErrorType.InternalError, "Vessel is currently NULL. Are you running outside KSP?");
                                    break;
                                }

                                stdio.PrintLine("Vessel: " + Parent.vessel.vesselName);
                                PartResource[] res = stdvessel.GetResources(Parent.vessel);

                                stdio.PrintLine(res.Length.ToString() + " resources found.");
                                stdio.PrintLine();
                                foreach (PartResource r in res)
                                {
                                    stdio.PrintLine(r.resourceName + "\t" + r.amount + "/" + r.maxAmount);
                                }
                                if (res.Length > 0) stdio.PrintLine();

                                break;
                        }
                        break;
                    }

                case "run":
                    {
                        string nextToken = ReadNextToken();
                        if (nextToken == null)
                        {
                            ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.SyntaxError, "No file name or expression provided.", ref errors);
                        }
                        else if (Parent.CurrentFolder.ContainsFile(nextToken + ".txt"))
                        {
                            GetNextToken();
                            GetNextToken();
                            if (token == ".")
                                Parent.CreateProcess(Parent.CurrentFolder.GetFile(nextToken + ".txt").Lines, token);
                            else
                                ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Expected \".\"");
                        }
                        else
                        {
                            string[] e = new string[0];
                            string s = GetNextExpression(new string[] { "." });
                            if (s == null)
                            {
                                ThrowError(ErrorBuilder.ErrorType.SyntaxError, "\"{\" expected after IF.");
                                break;
                            }
                            else if (s == "")
                            {
                                ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Expression expected.");
                                break;
                            }
                            Value v = Evaluator.Evaluate(s, line, out e);
                            if (e.Length > 0)
                            {
                                errors.AddRange(e);
                                break;
                            }
                            else if (Parent.CurrentFolder.ContainsFile(v.StringValue + ".txt"))
                            {
                                Parent.CreateProcess(Parent.CurrentFolder.GetFile(v.StringValue + ".txt").Lines, v.StringValue);
                            }
                            else
                            {
                                ThrowError(ErrorBuilder.ErrorType.SyntaxError, "File \"" + v.StringValue + "\" does not exist.");
                            }
                        }
                        break;
                    }

                case "while":
                    {
                        string s = GetNextExpression(new string[] { "{" });
                        if (s == null)
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "\"{\" expected after WHILE.");
                            break;
                        }
                        else if (s == "")
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Expression expected.");
                            break;
                        }
                        
                        string[] e;
                        Value v = Evaluator.Evaluate(s, line, out e);
                        if ((v.BooleanValue) && (e.Length == 0))
                        {
                            CurrentState.PushCall(s, line.Filename, this);
                            NewScope(ExecutionState.Status.WhileLoop);
                        }
                        else
                        {
                            errors.AddRange(e);
                            AdvanceToEndOfScope();
                        }
                    }
                    break;

                case "until":
                    {
                        string s = GetNextExpression(new string[] { "{" });
                        if (s == null)
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "\"{\" expected after UNTIL.");
                            break;
                        }
                        else if (s == "")
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Expression expected.");
                            break;
                        }

                        string[] e;
                        Value v = Evaluator.Evaluate(s, line, out e);
                        if ((!v.BooleanValue) && (e.Length == 0))
                        {
                            CurrentState.PushCall(s, line.Filename, this);
                            NewScope(ExecutionState.Status.UntilLoop);
                        }
                        else
                        {
                            errors.AddRange(e);
                            AdvanceToEndOfScope();
                        }
                    }
                    break;

                case "input":
                    {
                        string[] ex;
                        string s = GetNextExpression(new string[] { "to" });
                        if (s == null)
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Missing expression terminator \"at\" or \".\".");
                            break;
                        }
                        if (s == "")
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "No expression provided. Cannot run PRINT.");
                            break;
                        }

                        // Evaluate expression
                        Value output = Evaluator.Evaluate(s, line, out ex);
                        errors.AddRange(ex);
                        if (ex.Length > 0)
                            break;

                        s = GetNextExpression(new string[] { "." });
                        if (s == "")
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "No input variable provided. Cannot INPUT.");
                            break;
                        }
                        if (s == null)
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "\".\" expected.");
                            break;
                        }
                        Value location = Evaluator.Evaluate(s, line, out ex);
                        errors.AddRange(ex);
                        if (ex.Length > 0)
                            break;
                        else
                        {
                            Halt();
                        }
                    }
                    break;

                //case "for":
                //    {
                //        string a = GetNextExpression(new string[] { "from", "." });
                //        GetNextToken();
                //        if (token == ".") 
                            
                //        string b = GetNextExpression(new string[] { "to" });
                //        GetNextToken();
                //        string c = GetNextExpression(new string[] { "step", "{" });
                //        GetNextToken();
                //        string d;
                //        if (token == "step")
                //        {
                //            d = "set " + a + " to " + GetNextExpression(new string[] { "{" }) + ".";
                //        }
                //        else
                //        {
                //            string[] e, f;
                //            Value bval = Evaluator.Evaluate(b, line, out e);
                //            Value cval = Evaluator.Evaluate(c, line, out f);
                //            if ((e.Length > 0) || (f.Length > 0))
                //            {
                //                errors.AddRange(e);
                //                errors.AddRange(f);
                //                break;
                //            }

                //            d = "set " + a + " to " + a + " + " + ((cval > bval).BooleanValue ? "1" : "-1") + ".";
                //        }

                        
                //    }
            }


            err = errors.ToArray();
            return new Tuple<int, int>(lineIndex, i);
        }
    }
}
