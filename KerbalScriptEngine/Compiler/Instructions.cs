using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerboScriptEngine.Compiler
{
    enum Instructions
    {
        /// <summary>
        /// Pushes the given local variable onto the data stack
        /// </summary>
        pushv,
        /// <summary>
        /// Pushes the given global variable onto the data stack
        /// </summary>
        pushg,
        /// <summary>
        /// Pushes the given literal onto the data stack
        /// </summary>
        pushl,
        /// <summary>
        /// Pops the top value off the data stack and assigns it to the given global variable.
        /// </summary>
        popg,
        /// <summary>
        /// Pops the top value off the data stack and assigns it to the given local variable.
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
        /// Pops the top two values off the data stack, performs bitwise or logical AND on them, and pushes the result.
        /// </summary>
        and,
        /// <summary>
        /// Pops the top two values off the data stack, performs bitwise or logical OR on them, and pushes the result.
        /// </summary>
        or,
        /// <summary>
        /// Pops the top value off the data stack, performs bitwise or logical NOT on it, and pushes the result.
        /// </summary>
        not,
        /// <summary>
        /// Pops the top value off the data stack, performs the positive operator on it, and pushes the result.
        /// </summary>
        pos,
        /// <summary>
        /// Pops the top value off the data stack, peforms the negative operator on it, and pushes the result.
        /// </summary>
        neg,
        /// <summary>
        /// Pops the top two values off the stack, calculates the second to the power of the first, and pushes the result.
        /// </summary>
        pow,
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
        scr,
        /// <summary>
        /// Halt and wait for input.
        /// </summary>
        inp
    }
}
