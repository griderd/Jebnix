﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KerboScriptEngine.InternalTypes;

namespace KerboScriptEngine
{
    /// <summary>
    /// This class contains a postfix expression evaluator, as well as a infix-to-postfix converter.
    /// </summary>
    class Evaluator
    {
        /// <summary>
        /// Evaluates the given expression from the given source, and evaluates it.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="source"></param>
        /// <param name="vars"></param>
        /// <param name="functions"></param>
        /// <param name="errors"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public static Value Evaluate(string expression, LineInfo source, out string[] errors, ScriptProcess process)
        {
            return Evaluate(new LineInfo(expression, source.Filename, source.LineNumber, source.ColumnOffset, source.Process), out errors, process);
        }

        public static Value Evaluate(LineInfo line, out string[] errors, ScriptProcess process)
        {
            List<string> err = new List<string>();
            Stack<Value> result = new Stack<Value>();
            string[] e;
            Queue<string> postfix = ConvertToPostfix(line.Tokens, process, out e);
            err.AddRange(e);

            string[] unaryOperators = new string[] { "!", "~", "not", "++", "--" };

            while (postfix.Count > 0)
            {
                string token = postfix.Dequeue();
                if (ReservedWords.Operators.Contains(token))
                {
                    Value a, b;
                    if (result.Count > 0)
                        b = result.Pop();
                    else
                    {
                        ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.SyntaxError, "Invalid expression.", ref err);
                        errors = err.ToArray();
                        return 0;
                    }

                    try
                    {
                        if (unaryOperators.Contains(token))
                        {
                            switch (token)
                            {
                                case "!":
                                case "not":
                                    result.Push(!b);
                                    break;

                                case "~":
                                    result.Push(~b);
                                    break;

                                case "++":
                                    result.Push(b++);
                                    break;

                                case "--":
                                    result.Push(b--);
                                    break;
                            }
                        }
                        else if (result.Count >= 1)
                        {
                            a = result.Pop();

                            switch (token)
                            {
                                case "+":
                                    result.Push(a + b);
                                    break;

                                case "-":
                                    result.Push(a - b);
                                    break;

                                case "*":
                                    result.Push(a * b);
                                    break;

                                case "/":
                                    result.Push(a / b);
                                    break;

                                case "%":
                                    result.Push(a % b);
                                    break;

                                case "^":
                                    result.Push(Value.RaiseToPower(a, b));
                                    break;

                                case "|":
                                case "or":
                                    result.Push(a | b);
                                    break;

                                case "&":
                                case "and":
                                    result.Push(a & b);
                                    break;

                                case "=":
                                case "==":
                                    result.Push(a == b);
                                    break;

                                case "!=":
                                    result.Push(a != b);
                                    break;

                                case "<":
                                    result.Push(a < b);
                                    break;

                                case ">":
                                    result.Push(a > b);
                                    break;

                                case "<=":
                                    result.Push(a <= b);
                                    break;

                                case ">=":
                                    result.Push(a >= b);
                                    break;
                            }
                        }
                        else if (token == "+")
                        {
                            result.Push(+b);
                        }
                        else if (token == "-")
                        {
                            result.Push(-b);
                        }
                        else
                        {
                            ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.SyntaxError, "Invalid expression.", ref err);
                            errors = err.ToArray();
                            return 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        err.Add(ex.Message);
                        errors = err.ToArray();
                        return 0;
                    }
                }
                else if (process.Parent.GlobalFunctions.Contains(token))
                {
                    Value returnVal = null;
                    Stack<Value> paramList = new Stack<Value>();

                    if (ScriptAPI.APIInfo.FunctionNames.Contains(token))
                    {
                        if (ScriptAPI.APIInfo.IsSignatureMatch(token, result.Count))
                        {
                            while (result.Count > 0)
                            {
                                paramList.Push(result.Pop());
                            }
                            if (!ScriptAPI.APIInfo.InvokeFunction(token, out returnVal, paramList.ToArray()))
                                ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.RuntimeError, "An error occured while invoking the API function \"" + token + "\". Ensure the function exists and takes the number of arguments provided.", ref err);
                        }
                        else
                        {
                            ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.RuntimeError, "The function \"" + token + "\" does not take " + result.Count.ToString() + " arguments. The function requires " + ScriptAPI.APIInfo.ArgumentCount(token).ToString() + " arguments.", ref err);   
                        }
                    }

                    if (!Value.IsNull(returnVal))
                        result.Push(returnVal);
                    else
                    {
                        errors = err.ToArray();
                        return 0;
                    }
                }
                else
                {
                    result.Push(ConvertToValue(token, process));
                }
            }

            if (result.Count == 1)
            {
                errors = err.ToArray();
                return result.Pop();
            }
            else
            {
                ErrorBuilder.BuildError(line, ErrorBuilder.ErrorType.SyntaxError, "Invalid expression.", ref err);
                errors = err.ToArray();
                return 0;
            }
        }

        private static Value ConvertToValue(string token, ScriptProcess process)
        {
            int itemp;
            double ftemp;
            bool btemp;
            OrderedPair optemp; 
            Value v = null;

            if (process.TryGetVariable(token, out v))
            {
                return v;
            }
            else if (ReservedWords.IsString(token))
            {
                return token.Trim('\"');
            }
            else if (int.TryParse(token, out itemp))
            {
                return itemp;
            }
            else if (double.TryParse(token, out ftemp))
            {
                return ftemp;
            }
            else if (bool.TryParse(token, out btemp))
            {
                return btemp;
            }
            else if (OrderedPair.TryParse(token, out optemp))
            {
                return new Value(optemp);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts the given infix expression (as a token array) into an postfix expression (as a queue string)
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="vars"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static Queue<string> ConvertToPostfix(string[] tokens, ScriptProcess process, out string[] errors)
        {
            Queue<string> output = new Queue<string>();
            Stack<string> s = new Stack<string>();

            List<string> err = new List<string>();

            double dtemp;
            bool btemp;
            OrderedPair otemp;

            foreach (string token in tokens)
            {
                if (ReservedWords.Operators.Contains(token))
                {
                    string[] rightAssc = new string[] { "!", "~", "not", ",", "^"};
                    while ((s.Count > 0) && (ReservedWords.Operators.Contains(s.Peek())) &&
                        (((!rightAssc.Contains(token)) && (ReservedWords.GetPrecedence(token) == ReservedWords.GetPrecedence(s.Peek()))) ||
                        (ReservedWords.GetPrecedence(token) < ReservedWords.GetPrecedence(s.Peek()))))
                    {
                        output.Enqueue(s.Pop());
                    }
                    s.Push(token);
                }
                else if (double.TryParse(token, out dtemp) || bool.TryParse(token, out btemp) || OrderedPair.TryParse(token, out otemp))
                {
                    output.Enqueue(token);
                }
                else if (ReservedWords.IsString(token))
                {
                    output.Enqueue(token);
                }
                else if (process.HasVariable(token))
                {
                    output.Enqueue(token);
                }
                else if (token == ")")
                {
                    while ((s.Count > 0) && (s.Peek() != "(")) 
                    {
                        output.Enqueue(s.Pop());
                        if (s.Count == 0)
                            err.Add("Parenthesis mismatch. Missing '('");
                    }
                    if ((s.Count > 0) && (s.Peek() == "("))
                        s.Pop();
                }
                else if (token == "]")
                {
                    while ((s.Count > 0) && (s.Peek() != "["))
                    {
                        output.Enqueue(s.Pop());
                        if (s.Count == 0)
                            err.Add("Square bracket mismatch. Missing '['");
                    }
                    if ((s.Count > 0) && (s.Peek() == "["))
                        output.Enqueue(s.Pop());
                }
                else if (token == "(" | token == "[")
                {
                    s.Push(token);
                }
                else
                {
                    s.Push(token);
                }
            }

            while (s.Count > 0)
            {
                if ((s.Peek() == "(") | (s.Peek() == ")"))
                    err.Add("Parenthesis mismatch.");
                else if ((s.Peek() == "]") | (s.Peek() == "["))
                    err.Add("Square bracket mismatch.");
                else
                    output.Enqueue(s.Pop());
            }

            errors = err.ToArray();
            return output;
        }
    }
}
