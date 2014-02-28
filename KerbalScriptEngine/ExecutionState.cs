using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// Stack of internal call frames.
        /// </summary>
        public Stack<ExecutionFrame> executionStack;

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
            executionStack = new Stack<ExecutionFrame>();
            statusStack = new Stack<Status>();
            whenBlocks = new Dictionary<LineInfo, Tuple<int,int>>();
            whenThenPersist = new Dictionary<LineInfo, bool>();
            parameters = new Dictionary<string, Value>();
            scopeStack.Push(new Dictionary<string, Value>());
            lockedVariables = new Dictionary<string, string>();
            onBlocks = new Dictionary<LineInfo, Tuple<int, int>>();
        }

        public void PushExecutionFrame()
        {
            executionStack.Push(new ExecutionFrame(nextLine, nextToken));
        }

        public void PopExecutionFrame()
        {
            ExecutionFrame f = executionStack.Pop();
            nextLine = f.LineNumber;
            nextToken = f.TokenOffset;
        }
    }
}
