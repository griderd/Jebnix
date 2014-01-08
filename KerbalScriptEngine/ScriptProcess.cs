using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerboScriptEngine
{
    partial class ScriptProcess
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
        }

        public void ExecuteNext()
        {
            string[] err;
            Execute(CurrentState.nextLine, CurrentState.nextToken, CurrentState.lines.ToArray(), out err);

        }

        public void Interrupt(int nextLine, int nextToken)
        {
            CurrentState.PushExecutionFrame();
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
                    // TODO: print errors
                }
            }
        }

        public void AddLines(LineInfo[] lines)
        {
            CurrentState.lines.AddRange(lines);
        }

        public bool HasVariable(string name)
        {
            return CurrentState.parameters.ContainsKey(name) | LocalScope.ContainsKey(name);
        }

        public void SetVariable(string name, Value val)
        {
            if (LocalScope.ContainsKey(name))
                LocalScope[name] = val;
            else
                LocalScope.Add(name, val);
        }

        public bool TryGetVariable(string name, out Value value)
        {
            if (CurrentState.parameters.ContainsKey(name))
            {
                value = CurrentState.parameters[name];
                return true;
            }
            else if (Parent.HasVariable(name))
            {
                return Parent.TryGetVariable(name, out value);
            }
            else
            {
                // Go through the scope stack looking for the variable
                foreach (Dictionary<string, Value> scope in CurrentState.scopeStack)
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
