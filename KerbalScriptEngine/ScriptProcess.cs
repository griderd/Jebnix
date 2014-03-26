using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Jebnix.stdlib;
using Jebnix.Types;

namespace KerboScriptEngine
{
    partial class ScriptProcess
    {
        Dictionary<string, Value> globalScope;
        Dictionary<string, string> globalLocked;
        Stack<ExecutionState> stateStack;

        ExecutionState CurrentState
        {
            get
            {
                return stateStack.Peek();
            }
        }

        Dictionary<string, Value> LocalScope
        {
            get
            {
                return CurrentState.scopeStack.Peek();
            }
        }

        /// <summary>
        /// Gets the variables that exist in the local scope, as well as the global scope. Local variables override global variables with the same name.
        /// </summary>
        Dictionary<string, Value> ResolvedScope
        {
            get
            {
                Dictionary<string, Value> vars = new Dictionary<string, Value>();

                foreach (KeyValuePair<string, Value> kvp in CurrentState.parameters)
                {
                    vars.Add(kvp.Key, kvp.Value);
                }

                foreach (Dictionary<string, Value> dict in CurrentState.scopeStack)
                {
                    foreach (KeyValuePair<string, Value> kvp in dict)
                    {
                        if (!vars.ContainsKey(kvp.Key))
                            vars.Add(kvp.Key, kvp.Value);
                    }
                }

                foreach (KeyValuePair<string, Value> kvp in globalScope)
                {
                    if (!vars.ContainsKey(kvp.Key))
                        vars.Add(kvp.Key, kvp.Value);
                }

                return vars;
            }
        }

        void NewScope(ExecutionState.Status status)
        {
#if DEBUG
            Debug.Print("Entering scope " + status.ToString());
#endif
            CurrentState.scopeStack.Push(new Dictionary<string, Value>());
            CurrentState.statusStack.Push(status);
        }

        bool ExitScope(out string[] errors, out Tuple<int, int> newPosition)
        {
            if (CurrentState.scopeStack.Count > 1)
            {
                CurrentState.scopeStack.Pop();
                ExecutionState.Status status = CurrentState.statusStack.Pop();

#if DEBUG
                Debug.Print("Exiting scope " + status.ToString());
#endif

                newPosition = null;
                if (status == ExecutionState.Status.WhileLoop)
                {
                    LineInfo call;
                    if (CurrentState.PopCall(out errors, out call))
                    {
                        NewScope(ExecutionState.Status.WhileLoop);
                        CurrentState.PushCall(call);
                        newPosition = new Tuple<int, int>(call.LineNumber, call.ColumnOffset);
                    }
                }
                else if (status == ExecutionState.Status.UntilLoop)
                {
                    LineInfo call;
                    if (!CurrentState.PopCall(out errors, out call))
                    {
                        NewScope(ExecutionState.Status.UntilLoop);
                        CurrentState.PushCall(call);
                        newPosition = new Tuple<int, int>(call.LineNumber, call.ColumnOffset);
                    }
                }
                else
                {
                    errors = new string[0];
                    newPosition = null;
                }
                return true;
            }
            errors = new string[0];
            newPosition = null;
            return false;
        }

        Stack<Value> argumentStack;

        bool CanAddParameters { get; set; }

        public bool Running
        {
            get
            {
                return (CurrentState.nextLine + 1 < CurrentState.lines.Count);
            }
        }

        public Interpreter Parent { get; private set; }

        public ScriptProcess(LineInfo[] lines, Interpreter parent)
        {
            Parent = parent;
            stateStack = new Stack<ExecutionState>();
            LineInfo[] l = lines;
            for (int i = 0; i < l.Length; i++)
            {
                l[i].Process = this;
            }
            stateStack.Push(new ExecutionState(lines));
            argumentStack = new Stack<Value>();
            globalLocked = new Dictionary<string, string>();
            globalScope = new Dictionary<string, Value>();
        }

        public void ExecuteNext()
        {
            string[] err = new string[0];
            if (CurrentState.nextLine < CurrentState.lines.Count)
            {
#if DEBUG
                Debug.Print("Executing at " + CurrentState.nextLine + ", " + CurrentState.nextToken);
#endif
                Tuple<int, int> t = Execute(CurrentState.nextLine, CurrentState.nextToken, CurrentState.lines.ToArray(), out err);
                //if (CurrentState.nextToken >= CurrentState.lines[CurrentState.nextLine].Tokens.Length)
                //    CurrentState.nextLine++;
                CurrentState.nextLine = t.Item1;
                CurrentState.nextToken = t.Item2;
            }

            if (err.Length > 0)
            {
                foreach (string e in err)
                {
                    stdio.PrintLine(e);
                }
            }
        }

        public void AddLines(LineInfo[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].Process = this;
                CurrentState.lines.Add(lines[i]);
            }
        }

        public void Interrupt(int nextLine, int nextToken)
        {
            CurrentState.PushCall("true", "", this);
            CurrentState.nextLine = nextLine;
            CurrentState.nextToken = nextToken;
        }

        public void CheckWhenConditions()
        {
            string[] err;
            foreach (LineInfo when in CurrentState.whenBlocks.Keys)
            {
                if (Evaluator.Evaluate(when, out err).BooleanValue && err.Length == 0)
                {
                    Interrupt(CurrentState.whenBlocks[when].Item1, CurrentState.whenBlocks[when].Item2); 
                    CurrentState.whenBlocks.Remove(when);
                }
                if (err.Length > 0)
                {
                    foreach (string e in err)
                    {
                        stdio.PrintLine(e);
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether a variable with the given name exists in the resolved scope.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasVariable(string name)
        {
            return ResolvedScope.ContainsKey(name);
        }

        /// <summary>
        /// Sets a variable with the given name in the resolved scope. Returns a value determining if the variable exists.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool SetVariable(string name, Value val)
        {
            if (CurrentState.parameters.ContainsKey(name))
            {
                CurrentState.parameters[name] = val;
                return true;
            }
            else
            {
                // Go through the scope stack looking for the variable
                foreach (Dictionary<string, Value> scope in CurrentState.scopeStack)
                {
                    if (scope.ContainsKey(name))
                    {
                        scope[name] = val;
                        return true;
                    }
                }
            }

            if (globalScope.ContainsKey(name))
            {
                globalScope[name] = val;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets or creates a variable in the global scope.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void SetGlobalVariable(string name, Value val)
        {
            if (globalScope.ContainsKey(name))
                globalScope[name] = val;
            else
                globalScope.Add(name, val);
        }

        /// <summary>
        /// Sets or creates a variable in the local scope.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void SetLocalVariable(string name, Value val)
        {
            if (LocalScope.ContainsKey(name))
                LocalScope[name] = val;
            else
                LocalScope.Add(name, val);
        }

        public bool TryGetVariable(string name, out Value value)
        {
            if (ResolvedScope.ContainsKey(name))
            {
                value = ResolvedScope[name];
                return true;
            }
            value = null;
            return false;
        }

        //public bool TryGetVariable(string name, out Value value)
        //{
        //    if (CurrentState.parameters.ContainsKey(name))
        //    {
        //        value = CurrentState.parameters[name];
        //        return true;
        //    }
        //    else if (globalScope.ContainsKey(name))
        //    {
        //        value = globalScope[name];
        //        return true;
        //    }
        //    else
        //    {
        //        // Go through the scope stack looking for the variable
        //        foreach (Dictionary<string, Value> scope in CurrentState.scopeStack)
        //        {
        //            if (scope.ContainsKey(name))
        //            {
        //                value = scope[name];
        //                return true;
        //            }
        //        }
        //    }

        //    value = null;
        //    return false;
        //}

        internal void Dump()
        {
            throw new NotImplementedException();
        }
    }
}
