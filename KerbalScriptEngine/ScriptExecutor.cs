using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIOS.stdlib;
using KSP;

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
        private Tuple<int, int> Execute(int lineIndex, int tokenIndex, LineInfo[] lines, out string[]  err)
        {
            LineInfo line = lines[lineIndex];

            string token;
            List<string> errors = new List<string>();

            int i = tokenIndex;
            token = line.Tokens[i];
                

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
                }
                return true;
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

            // Exits the current scope. 
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
                                string expression = GetNextExpression(new string[] { "." });
                                string[] ex;
                                Value var = Evaluator.Evaluate(expression, line, ResolvedScope, Parent.GlobalFunctions, out ex);
                                errors.AddRange(ex);

                                if (ex.Length == 0)
                                {
                                    if (HasVariable(name))
                                    {
                                        SetVariable(name, var);
                                    }
                                    else if (local)
                                    {
                                        SetLocalVariable(name, var);
                                    }
                                    else
                                    {
                                        SetGlobalVariable(name, var);
                                    }
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

                case "print":
                    {
                        string[] ex;
                        string s = GetNextExpression(new string[] { "at", "." });
                        if (s == "")
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "No expression provided. Cannot run PRINT.");
                            break;
                        }
                        if (s == null)
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Missing expression terminator \"at\" or \".\".");
                            break;
                        }
                        
                        // Evaluate expression
                        Value output = Evaluator.Evaluate(s, line, ResolvedScope, Parent.GlobalFunctions, out ex);
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
                            Value location = Evaluator.Evaluate(s, line, ResolvedScope, Parent.GlobalFunctions, out ex);
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
                        if (s == "")
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "Expression expected.");
                            break;
                        }
                        else if (s == null)
                        {
                            ThrowError(ErrorBuilder.ErrorType.SyntaxError, "\"{\" expected after IF.");
                            break;
                        }
                        else
                        {
                            string[] ex;
                            Value v = Evaluator.Evaluate(s, line, ResolvedScope, Parent.GlobalFunctions, out ex);
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
                                foreach (BIOS.FileSystem.File file in Parent.CurrentFolder.Files)
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
                                List<PartResource> res = new List<PartResource>();

                                Func<PartResource, bool> HasResource = delegate(PartResource x) { 
                                    foreach (PartResource r in res) { 
                                        if (r.resourceName == x.resourceName) 
                                            return true; 
                                    } 
                                    return false; 
                                };

                                foreach (Part p in Parent.vessel.Parts)
                                {
                                    foreach (PartResource r in p.Resources)
                                    {
                                        if (!HasResource(r))
                                            res.Add(r);
                                    }
                                }

                                stdio.PrintLine(res.Count.ToString() + " resources found.");
                                stdio.PrintLine();
                                foreach (PartResource r in res)
                                {
                                    stdio.PrintLine(r.resourceName + "\t" + r.amount + "/" + r.maxAmount);
                                }
                                if (res.Count > 0) stdio.PrintLine();

                                break;
                        }
                        break;
                    }
            }


            err = errors.ToArray();
            return new Tuple<int, int>(lineIndex, i);
        }
    }
}
