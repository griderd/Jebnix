using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerboScriptEngine.Compiler
{
    public enum OperatorAssociativity
    {
        LeftAssociative,
        RightAssociative
    }

    public enum OperatorType
    {
        Power,
        Multiply,
        Divide,
        Modulus,
        And,
        Add,
        Subtract,
        Or,
        Equality,
        Inequality,
        GreaterThan,
        LessThan,
        GreaterThanEqualTo,
        LessThanEqualTo,
        Not
    }

    class Operator
    {
        public string Token { get; private set; }
        public int Presidence { get; private set; }
        public OperatorAssociativity Associativity { get; private set; }
        public OperatorType OperatorType { get; private set; }
        public Instructions OperatorInstruction { get; private set; }

        public Operator(string op)
        {
            Token = op;
            switch (op)
            {
                case "!":
                case "not":
                    OperatorType = Compiler.OperatorType.Not;
                    OperatorInstruction = Instructions.not;
                    Associativity = OperatorAssociativity.RightAssociative;
                    Presidence = 5;
                    break;

                case "^":
                    OperatorType = OperatorType.Power;
                    OperatorInstruction = Instructions.pow;
                    Associativity = OperatorAssociativity.RightAssociative;
                    Presidence = 4; 
                    break;

                case "*":
                    OperatorType = OperatorType.Multiply;
                    OperatorInstruction = Instructions.mult;
                    Associativity = OperatorAssociativity.LeftAssociative;
                    Presidence = 3;
                    break;

                case "/":
                    OperatorType = Compiler.OperatorType.Divide;
                    OperatorInstruction = Instructions.div;
                    Associativity = OperatorAssociativity.LeftAssociative;
                    Presidence = 3;
                    break;

                case "%":
                    OperatorType = Compiler.OperatorType.Modulus;
                    OperatorInstruction = Instructions.mod;
                    Associativity = OperatorAssociativity.LeftAssociative;
                    Presidence = 3;
                    break;

                case "+":
                    OperatorType = Compiler.OperatorType.Add;
                    OperatorInstruction = Instructions.add;
                    Associativity = OperatorAssociativity.LeftAssociative;
                    Presidence = 2;
                    break;

                case "-":
                    OperatorType = Compiler.OperatorType.Subtract;
                    OperatorInstruction = Instructions.or;
                    Associativity = OperatorAssociativity.LeftAssociative;
                    Presidence = 2;
                    break;

                case "=":
                case "==":
                    OperatorType = Compiler.OperatorType.Equality;
                    OperatorInstruction = Instructions.eq;
                    Associativity = OperatorAssociativity.LeftAssociative;
                    Presidence = 1;
                    break;

                case "!=":
                case "<>":
                    OperatorType = Compiler.OperatorType.Inequality;
                    OperatorInstruction = Instructions.neq;
                    Associativity = OperatorAssociativity.LeftAssociative;
                    Presidence = 2;
                    break;

                case ">":
                    OperatorType = Compiler.OperatorType.GreaterThan;
                    OperatorInstruction = Instructions.gt;
                    Associativity = OperatorAssociativity.LeftAssociative;
                    Presidence = 2;
                    break;

                case "<":
                    OperatorType = Compiler.OperatorType.LessThan;
                    OperatorInstruction = Instructions.lt;
                    Associativity = OperatorAssociativity.LeftAssociative;
                    Presidence = 2;
                    break;

                case ">=":
                    OperatorType = Compiler.OperatorType.GreaterThanEqualTo;
                    OperatorInstruction = Instructions.gte;
                    Associativity = OperatorAssociativity.LeftAssociative;
                    Presidence = 2;
                    break;

                case "<=":
                    OperatorType = Compiler.OperatorType.LessThanEqualTo;
                    OperatorInstruction = Instructions.lte;
                    Associativity = OperatorAssociativity.LeftAssociative;
                    Presidence = 2;
                    break;

                case "&":
                case "and":
                    OperatorType = Compiler.OperatorType.And;
                    OperatorInstruction = Instructions.and;
                    Associativity = OperatorAssociativity.LeftAssociative;
                    Presidence = 1;
                    break;

                case "|":
                case "or":
                    OperatorType = Compiler.OperatorType.Or;
                    OperatorInstruction = Instructions.or;
                    Associativity = OperatorAssociativity.LeftAssociative;
                    Presidence = 1;
                    break;

            }
        }

    }
}
