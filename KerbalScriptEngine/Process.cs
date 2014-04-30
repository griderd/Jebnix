using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix;
using Jebnix.Types;
using Jebnix.Types.Structures;
using KerboScriptEngine.Compiler;

namespace KerboScriptEngine
{
    class Process
    {
        string name;
        JObject[] memory;
        int programCounter;
        List<int> lockedVars;

        Script script;

        Stack<int> callStack;
        Stack<JObject> dataStack;

        bool running;

        public Script SourceScript
        {
            get
            {
                return script;
            }
        }

        JObject CurrentValue
        {
            get
            {
                return memory[programCounter];
            }
        }

        public Stack<int> CallStack
        {
            get
            {
                return callStack;
            }
        }

        public Process(Script s)
        {
            script = s;
            programCounter = 0;
            memory = s.Bytecode;
            name = s.Name;
            callStack = new Stack<int>();
            dataStack = new Stack<JObject>();
            running = true;
            lockedVars = new List<int>();
        }

        public void RunCycle()
        {
            if (!running)
                return;

            Instruction inst;
            Pointer ptr;
            JObject value = GetValue();
            if (value.ObjectType != Instruction.TYPENAME)
                throw new Exception("Expected instruction.");

            inst = (Instruction)value;

            switch (inst.Value)
            {
                case Instructions.push:
                    ptr = GetPointer();
                    dataStack.Push(Dereference(ptr));
                    break;

                case Instructions.pushl:
                    dataStack.Push(GetValue());
                    break;

                case Instructions.pop:
                    ptr = GetPointer();
                    memory[ptr.Value] = dataStack.Pop();
                    break;

                case Instructions.add:
                    {
                        JObject a, b;
                        b = dataStack.Pop();
                        a = dataStack.Pop();
                        dataStack.Push(a + b);
                    }
                    break;

                case Instructions.sub:
                    {
                        JObject a, b;
                        b = dataStack.Pop();
                        a = dataStack.Pop();
                        dataStack.Push(a - b);
                    }
                    break;

                case Instructions.mult:
                    {
                        JObject a, b;
                        b = dataStack.Pop();
                        a = dataStack.Pop();
                        dataStack.Push(a * b);
                    }
                    break;

                case Instructions.div:
                    {
                        JObject a, b;
                        b = dataStack.Pop();
                        a = dataStack.Pop();
                        dataStack.Push(a / b);
                    }
                    break;

                case Instructions.mod:
                    {
                        JObject a, b;
                        b = dataStack.Pop();
                        a = dataStack.Pop();
                        dataStack.Push(a / b);
                    }
                    break;

                case Instructions.jmp:
                    ptr = GetPointer();
                    Jump(ptr);
                    break;

                case Instructions.hop:
                    ptr = GetPointer();
                    Hop(ptr);
                    break;

                case Instructions.jmpt:
                    ConditionalJumpHop(true);
                    break;

                case Instructions.jmpf:
                    ConditionalJumpHop(false);
                    break;

                case Instructions.hopt:
                    ConditionalJumpHop(true, true);
                    break;

                case Instructions.hopf:
                    ConditionalJumpHop(false, true);
                    break;

                case Instructions.ret:
                    Ret();
                    break;

                case Instructions.call:
                    // TODO: Call function
                    break;

                case Instructions.lok:
                    lockedVars.Add(GetPointer().Value);
                    break;

                case Instructions.ulok:
                    lockedVars.Remove(GetPointer().Value);
                    break;
                        
            }
        }

        /// <summary>
        /// Returns the current value and advances the program counter
        /// </summary>
        /// <returns></returns>
        private JObject GetValue()
        {
            return memory[programCounter++];
        }

        private JObject GetValue(int address)
        {
            if ((address >= 0) & (address <= memory.Length))
                return memory[address];
            else
                throw new IndexOutOfRangeException();
        }

        private JObject Dereference(Pointer ptr)
        {
            try
            {
                return GetValue(ptr.Value);
            }
            catch
            {
                throw;
            }
        }

        private Pointer GetPointer()
        {
            JObject value = GetValue();
            if (!value.IsSameType(Pointer.NullPointer))
                throw new Exception("Expected Pointer after PUSH.");
            return (Pointer)value;
        }

        private void Jump(int location)
        {
            callStack.Push(programCounter);
            programCounter = location;
        }

        private void Jump(Pointer location)
        {
            Jump(location.Value);
        }

        private void Ret()
        {
            if (callStack.Count > 0)
                programCounter = callStack.Pop();
            else
                throw new Exception("Call Stack Underflow");
        }

        private void Hop(int location)
        {
            programCounter = location;
        }

        private void Hop(Pointer location)
        {
            programCounter = location.Value;
        }

        private void ConditionalJumpHop(bool condition, bool hop = false)
        {
            JObject value = dataStack.Peek();
            JBoolean b = false;
            if (value.IsSameType(b) | value.IsSameType(new JInteger(0)))
                b = (JBoolean)value;
            if (b.Value == condition)
            {
                Pointer ptr = GetPointer();
                if (hop)
                    Hop(ptr);
                else
                    Jump(ptr);
            }
        }
    }
}
