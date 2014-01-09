using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerboScriptEngine
{
    class Evaluator
    {
        public static Value Evaluate(string expression, LineInfo source, out string[] errors)
        {
            return Evaluate(new LineInfo(expression, source.Filename, source.LineNumber, source.ColumnOffset, source.Process), out errors);
        }

        public static Value Evaluate(LineInfo line, out string[] errors)
        {
            Stack<Value> rpn = ConvertToRPN(line.Tokens);
            List<string> err = new List<string>();

            while (rpn.Count > 1)
            {
                string operation;
                bool unary = false;
                Value b = rpn.Pop();
                string bname = "";
                string aname = "";

                if (b.Type == Value.ValueTypes.Pointer)
                {
                    bname = b.StringValue;
                    if (!line.Process.TryGetVariable(b.StringValue, out b))
                    {
                        ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.RuntimeError, "Variable \"" + b.StringValue + "\" undeclared.", ref err);
                        errors = err.ToArray();
                        return Value.NullValue;
                    }
                }

                Value a = rpn.Pop();
                if (ReservedWords.Operators.Contains(a.StringValue))
                {
                    operation = a.StringValue;
                    unary = true;
                }
                else if (ReservedWords.Operators.Contains(rpn.Peek().StringValue))
                {
                    operation = rpn.Pop().StringValue;

                    if (a.Type == Value.ValueTypes.Pointer)
                    {
                        aname = a.StringValue;
                        if (!line.Process.TryGetVariable(a.StringValue, out a))
                        {
                            ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.RuntimeError, "Variable \"" + a.StringValue + "\" undeclared.", ref err);
                            errors = err.ToArray();
                            return Value.NullValue;
                        }
                    }
                }
                else
                {
                    ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.SyntaxError, "Invalid number of operands.", ref err);
                    break;
                }

                if (a.IsNull) ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.RuntimeError, "NullReferenceException. Operand \"" + aname + "\" is NULL.", ref err);
                if (b.IsNull) ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.RuntimeError, "NullReferenceException. Operand \"" + bname + "\" is NULL.", ref err);

                Value c;

                try
                {
                    switch (operation)
                    {
                        case "+":
                            if (unary)
                                c = +a;
                            else
                                c = a + b;
                            break;

                        case "-":
                            if (unary)
                                c = -a;
                            else
                                c = a - b;
                            break;

                        case "*":
                            c = a * b;
                            break;

                        case "/":
                            c = a / b;
                            break;

                        case "%":
                            c = a % b;
                            break;

                        case "^":
                            c = Value.RaiseToPower(a, b);
                            break;

                        case "&":
                        case "and":
                            c = a & b;
                            break;

                        case "|":
                        case "or":
                            c = a | b;
                            break;

                        case "!":
                        case "not":
                            c = !a;
                            break;

                        case "~":
                            c = ~a;
                            break;

                        case "++":
                            c = a++;
                            break;

                        case "--":
                            c = a--;
                            break;

                    }
                }
                catch (InvalidOperationException ex)
                {
                    ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.RuntimeError, ex, ref err);
                    break;
                }
            }

            errors = err.ToArray();
            return rpn.Pop();
        }

        private static Stack<Value> ConvertToRPN(string[] tokens)
        {
            Stack<Value> s = new Stack<Value>();

            foreach (string token in tokens)
            {
                
            }

            return s;
        }
    }
}
