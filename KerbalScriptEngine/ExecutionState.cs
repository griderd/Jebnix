using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Jebnix.Types;

namespace KerboScriptEngine
{
    /// <summary>
    /// Represents the execution state of a script.
    /// </summary>
    class ExecutionState
    {
        public enum Status
        {
            FileLevel,
            IfStatement,
            ElseStatement,
            UntilLoop,
            WhileLoop,
            WhenBlock,
            GenericBlock
        }

        /// <summary>
        /// Lines of the currently-executing script.
        /// </summary>
        public List<LineInfo> lines;

        /// <summary>
        /// Next line to execute.
        /// </summary>
        public int nextLine;

        /// <summary>
        /// Next token to execute.
        /// </summary>
        public int nextToken;

        /// <summary>
        /// Stack of scope-based local values.
        /// </summary>
        public Stack<Dictionary<string, Value>> scopeStack;

        /// <summary>
        /// Locked variables.
        /// </summary>
        public Dictionary<string, string> lockedVariables;

        /// <summary>
        /// Stack of internal call frames. The line number/token offset is where the frame starts. The condition to jump is located in the line text.
        /// </summary>
        public Stack<LineInfo> callStack;

        /// <summary>
        /// Stack of execution statuses
        /// </summary>
        public Stack<Status> statusStack;

        /// <summary>
        /// When events by condition, where value is a Tuple containing the line number and token ID.
        /// </summary>
        public Dictionary<LineInfo, Tuple<int, int>> whenBlocks;

        /// <summary>
        /// When events by condition, where value is a boolean determining if the event persists.
        /// </summary>
        public Dictionary<LineInfo, bool> whenThenPersist;

        /// <summary>
        /// ON events
        /// </summary>
        public Dictionary<LineInfo, Tuple<int, int>> onBlocks;

        /// <summary>
        /// Function parameters
        /// </summary>
        public Dictionary<string, Value> parameters;

        public ExecutionState(LineInfo[] lines)
        {
            this.lines = new List<LineInfo>(lines);
            nextLine = 0;
            nextToken = 0;
            scopeStack = new Stack<Dictionary<string, Value>>();
            callStack = new Stack<LineInfo>();
            statusStack = new Stack<Status>();
            whenBlocks = new Dictionary<LineInfo, Tuple<int,int>>();
            whenThenPersist = new Dictionary<LineInfo, bool>();
            parameters = new Dictionary<string, Value>();
            scopeStack.Push(new Dictionary<string, Value>());
            lockedVariables = new Dictionary<string, string>();
            onBlocks = new Dictionary<LineInfo, Tuple<int, int>>();
        }

        public void PushCall(LineInfo call)
        {
#if DEBUG
            Debug.Print("Pushing to call stack: \"" + call.Line + "\" at " + call.LineNumber + ", " + call.ColumnOffset);
#endif
            callStack.Push(call);
        }

        public void PushCall(string condition, string filename, ScriptProcess process)
        {
#if DEBUG
            Debug.Print("Pushing to call stack: \"" + condition + "\" at " + nextLine + ", " + nextToken);
#endif
            callStack.Push(new LineInfo(condition, filename, nextLine, nextToken, process));
        }

        public bool PopCall(out string[] errors, out LineInfo call)
        {
            if (callStack.Count > 0)
            {
                call = callStack.Pop();
#if DEBUG
                Debug.Print("Popped from call stack: \"" + call.Line + "\" at " + call.LineNumber + ", " + call.ColumnOffset);
#endif
                if (Evaluator.Evaluate(call, out errors).BooleanValue)
                {
#if DEBUG
                    Debug.Print("Condition TRUE");
#endif
                    return true;
                }
                else
                {
#if DEBUG
                    Debug.Print("Condition TRUE");
#endif
                    return false;
                }
            }

#if DEBUG
            Debug.Print("Call stack empty!");
#endif
            call = new LineInfo();
            errors = new string[0];
            return false;
        }
    }
}
