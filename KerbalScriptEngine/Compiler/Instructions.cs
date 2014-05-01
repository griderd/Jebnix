using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerboScriptEngine.Compiler
{
    enum Instructions
    {
        /// <summary>
        /// Pushes the given variable onto the data stack
        /// </summary>
        push,
        /// <summary>
        /// Pushes the given literal onto the data stack
        /// </summary>
        pushl,
        /// <summary>
        /// Pops the top value off the data stack and assigns it to the given variable.
        /// </summary>
        popv,
        /// <summary>
        /// Pops the top value off the data stack. Does not get assigned.
        /// </summary>
        pop,
        /// <summary>
        /// Pops the top two values off the data stack, adds them, and pushes the result.
        /// </summary>
        add,
        /// <summary>
        /// Pops the top two values off the data stack, subtracts them, and pushes the result.
        /// </summary>
        sub,
        /// <summary>
        /// Pops the top two values off the data stack, multiplies them, and pushes the result.
        /// </summary>
        mult,
        /// <summary>
        /// Pops the top two values off the data stack, divides them, and pushes the result.
        /// </summary>
        div,
        /// <summary>
        /// Pops the top two values off the data stack, divides and gets the remainder of them, and pushes the result.
        /// </summary>
        mod,
        /// <summary>
        /// Unconditional jump. Jumps to the given location, leaving a return location in the call stack.
        /// </summary>
        jmp,
        /// <summary>
        /// Conditional jump. Jumps only if the value at the top of the data stack is true. Jumps to the given location, leaving a return location in the call stack. DOES NOT consume the data stackframe.
        /// </summary>
        jmpt,
        /// <summary>
        /// Conditional jump. Jumps only if the value at the top of the data stack is false. Jumps to the given location, leaving a return location in the call stack. DOES NOT consume the data stackframe.
        /// </summary>
        jmpf,
        /// <summary>
        /// Unconditional hop. Moves to the given location if the value at the top of the data stack is true. DOES NOT leave a return address.
        /// </summary>
        hop,
        /// <summary>
        /// Conditional hop. Moves to the given location if the value at the top of the data stack is true. DOES NOT leave a return address. DOES NOT consume the data stack frame.
        /// </summary>
        hopt,
        /// <summary>
        /// Conditional hop. Moves to the given location if the value at the top of the data stack is false. DOES NOT leave a return address. DOES NOT consume the data stack frame.
        /// </summary>
        hopf,
        /// <summary>
        /// Returns to the location at the top of the call stack.
        /// </summary>
        ret,
        /// <summary>
        /// Calls an external function
        /// </summary>
        call,
        /// <summary>
        /// Adds the given pointer to the lock list
        /// </summary>
        lok,
        /// <summary>
        /// Removes the given pointer from the lock list
        /// </summary>
        ulok,
        /// <summary>
        /// Calls the given script.
        /// </summary>
        scr
    }
}
