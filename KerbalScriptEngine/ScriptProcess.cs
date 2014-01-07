using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerbalScriptEngine
{
    class ScriptProcess
    {
        List<LineInfo> lines;
        int nextLine;
        int nextToken;

        bool CanAddParameters { get; set; }
        Stack<Dictionary<string, Value>> scopeStack;
        Stack<Tuple<int, int>> executionStack;

        Dictionary<string, Value> localScope;
        Dictionary<string, Value> parameters;

        /// <summary>
        /// Contains conditions and code of when/then blocks. Key is condition, Value is code to execute.
        /// </summary>
        Dictionary<LineInfo, LineInfo[]> WhenBlocks;
        /// <summary>
        /// Contains conditions of when/then blocks, and a value determining if they persist. To persist, write WHEN... THEN PERSIST { }.
        /// </summary>
        Dictionary<LineInfo, bool> WhenThenPersist;

        Stack<Status> stateStack;

        

        public bool Running
        {
            get
            {
                return (nextLine + 1 < lines.Count);
            }
        }

        public Interpreter Parent { get; private set; }

        public ScriptProcess(LineInfo[] lines, Interpreter parent)
        {
            this.lines = new List<LineInfo>(lines);
            nextLine = 0;
            nextToken = 0;
            CanAddParameters = false;
            scopeStack = new Stack<Dictionary<string, Value>>();
            localScope = new Dictionary<string, Value>();
            scopeStack.Push(localScope);
            parameters = new Dictionary<string, Value>();
            stateStack = new Stack<Status>();
            stateStack.Push(Status.Normal);
            executionStack = new Stack<Tuple<int, int>>();
            Parent = parent;
        }

        public void ExecuteNext()
        {
            string[] err;
            Execute(nextLine, nextToken, lines.ToArray(), out err);

        }

        public void Interrupt(int nextLine, int nextToken)
        {
            executionStack.Push(new Tuple<int, int>(nextLine, nextToken));
        }

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

            for (int i = tokenIndex; i < line.Tokens.Length; i++)
            {
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
                                    if (!parameters.ContainsKey(token))
                                        parameters.Add(token, var);
                                    else
                                        
                                }
                            }
                            else
                            {
                                ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.SyntaxError, "Identifier cannot be reserved word.", ref errors);
                            }
                        }
                        break;
                }
            }
        }

        public void CheckWhenConditions()
        {
            string[] err;
            foreach (LineInfo when in WhenBlocks.Keys)
            {
                if (Evaluator.Evaluate(when, out err).BooleanValue && err.Length == 0)
                {
                    Interrupt(WhenBlocks[when]); 
                    WhenBlocks.Remove(when);
                }
                if (err.Length > 0)
                {
                    // TODO: print errors
                }
            }
        }

        public void AddLines(LineInfo[] lines)
        {
            this.lines.AddRange(lines);
        }

        public bool HasVariable(string name)
        {
            return parameters.ContainsKey(name) | localScope.ContainsKey(name);
        }

        public void SetVariable(string name, Value val)
        {
            if (localScope.ContainsKey(name))
                localScope[name] = val;
            else
                localScope.Add(name, val);
        }

        public bool TryGetVariable(string name, out Value value)
        {
            if (parameters.ContainsKey(name))
            {
                value = parameters[name];
                return true;
            }
            else if (Parent.HasVariable(name))
            {
                return Parent.TryGetVariable(name, out value);
            }
            else
            {
                // Go through the scope stack looking for the variable
                foreach (Dictionary<string, Value> scope in scopeStack)
                {
                    if (scope.ContainsKey(name))
                    {
                        value = scope[name];
                        return true;
                    }
                }
            }

            value = Value.NullValue;
            return false;
        }

        internal void Dump()
        {
            throw new NotImplementedException();
        }
    }
}
