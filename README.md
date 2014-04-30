Jebnix
======

kOS Alternative for KSP

**NOTE:** This is a work in progress, and the code is not complete. If you try running it, it probably won't work.

Compilation instructions:
At the moment, Jebnix compiles, but the only running system is the Jebnix Console Debugger. If you wish to run tests,
do so there. Keep in mind that Jebnix IS NOT feature-complete, and many language features have not been written or
are only partially complete. You can expect that many are still buggy.

Projects:
Jebnix - Includes Interpreter class, file system, graphics, and standard library for interacting with the Jebnix
environment and KSP.

JebnixConsoleDebugger - Displays an interactive Jebnix console in your OS. Useful for basic testing. Uses the terminal.

JebnixDebugger - Displays an interactive Jebnix console in your OS. Useful for basic testing. Uses WinForms. 
THIS DOES NOT WORK RIGHT NOW

JebnixRPM - A plugin for RasterPropMod to give interaction with Jebnix. THIS DOES NOT WORK RIGHT NOW

KerboScriptEngine - The language engine of KerboScript++.

# KerboScript

## Command Reference

### CLEARSCREEN
Clears the screen and places the cursor in the upper left. Example:
    CLEARSCREEN.

### IF, IF-ELSE, IF-ELSE-IF-ELSE
Checks if the expression supplied is true. If it is, the block is executed. Example:
    SET X TO 1.
    IF X = 1
    {
        PRINT "X equals one".
    }
    
You can also use IF-ELSE statements. When the condition associated with the IF is false, the ELSE block executes instead.
    SET X TO 1.
    IF X = 1
    {
        PRINT "X equals one".
    }
    ELSE
    {
        PRINT "X does not equal one".
    }

Alternatively, you can use IF-ELSE-IF-ELSE statements, with as many ELSE-IF statements as you'd like.
    SET X TO 1.
    IF X = 1
    {
        PRINT "X equals one".
    }
    ELSE IF X > 1
    {
        PRINT "X is greater than one".
    }
    ELSE
    {
        PRINT "X is less than one".
    }

### UNTIL
Executes the block until the condition supplied is true.
    SET X TO 1.
    UNTIL X > 10
    {
        PRINT X.
        SET X TO X + 1.
    }
    
### WHILE
Executes the block while the condition supplied is true.
    SET X TO 1.
    WHILE X < 10
    {
        PRINT X.
        SET X TO X + 1.
    }

### DO-WHILE
Executes the block at least once, and continues executing the block while the condition supplied is true. If the condition is false on the first pass, the block still executes once.
    SET X TO 1.
    DO
    {
        PRINT X.
        SET X TO X + 1.
    } WHILE X < 10.
