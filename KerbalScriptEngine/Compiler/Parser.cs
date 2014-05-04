using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.Types;
using Jebnix.Types.Structures;

namespace KerboScriptEngine.Compiler
{
    partial class Parser
    {
        Dictionary<string, int> labelPointers;
        List<string> variableNames;
        /// <summary>
        /// Contains variable names with the lock label associated with them. Variable names are the keys. The current lock label is the value.
        /// </summary>
        Dictionary<string, string> lockLabels;

        List<Token> tokens;
        List<string> errors;

        Token currentToken;
        int currentTokenIndex;

        Stack<int> branchStack;
        Stack<int> loopStack;
        Stack<Blocks> blockStack;

        List<string> asm;

        enum Blocks
        {
            General,
            IfElse,
            If,
            Else,
            Until,
            While,
            DoWhile,
            For,
            ForEach
        }


        public Parser(Token[] tokens)
        {
            labelPointers = new Dictionary<string, int>();
            variableNames = new List<string>();
            lockLabels = new Dictionary<string, string>();
            this.tokens = new List<Token>();
            errors = new List<string>();
            InitializeSegments();
            currentTokenIndex = 0;

            this.tokens.AddRange(tokens);
            currentToken = new Token();

            lockCount = 0;
            loopCount = 0;
            branchCount = 0;

            branchStack = new Stack<int>();
            loopStack = new Stack<int>();
            blockStack = new Stack<Blocks>();
            asm = new List<string>();

            CreateLabel("_main", Segment.Code);

            Parse();
        }

        public JObject[] GetObjectCode()
        {
            List<JObject> objects = new List<JObject>();
            objects.AddRange(segments[0]);
            objects.AddRange(segments[1]);
            objects.AddRange(segments[2]);
            return objects.ToArray();
        }

        public string[] GetErrors()
        {
            return errors.ToArray();
        }

        private void Parse()
        {
            GetToken();
            if (!HasToken())
                return;

            switch (currentToken.Text)
            {
                case "clearscreen":
                    Clearscreen();
                    break;

                case "set":
                    SetLock();
                    break;

                case "lock":
                    SetLock(false);
                    break;

                case "unlock":
                    Unlock();
                    break;
                    
                case "if":
                    IfBlock();
                    break;

                case "else":
                    blockStack.Push(Blocks.Else);
                    break;

                case "do":
                    DoWhileBlock();
                    break;

                case "until":
                    if ((blockStack.Count > 0) && (blockStack.Peek() == Blocks.DoWhile))
                        EndDoWhileBlock();
                    else
                        UntilBlock();
                    break;

                case "break":
                    Break();
                    break;

                case "run":
                    Run();
                    break;

                case "copy":
                    Copy();
                    break;

                case "delete":


                case "rename":

                case "switch":

                case "}":
                    if (blockStack.Count == 0)
                        ErrorBuilder.BuildError(currentToken, "'}' without '{'.", ref errors);
                    else
                    {
                        Blocks b = blockStack.Pop();
                        if (b == Blocks.Else)
                        {
                            blockStack.Pop();
                            Ret();
                        }
                        else if (b == Blocks.If)
                        {
                            Ret();
                            ElseBlock();
                        }
                        else if (b == Blocks.DoWhile)
                        {
                            blockStack.Push(b);
                        }
                    }
                    break;

                case "{":
                    blockStack.Push(Blocks.General);
                    break;
            }
        }

        private bool ContainsVariable(string name)
        {
            return variableNames.Contains(name);
        }

        private bool HasToken()
        {
            return ((currentTokenIndex >= 0) & (currentTokenIndex < tokens.Count));
        }

        private Token GetToken()
        {
            currentToken = tokens[currentTokenIndex];
            return tokens[currentTokenIndex++];
        }

        private bool IsFloat()
        {
            return IsFloat(currentToken);
        }

        private bool IsFloat(Token t)
        {
            double temp;
            return double.TryParse(t.Text, out temp);
        }

        private bool IsInteger()
        {
            return IsInteger(currentToken);
        }

        private bool IsInteger(Token t)
        {
            int temp;
            return int.TryParse(t.Text, out temp);
        }

        private bool IsString()
        {
            return IsString(currentToken);
        }

        private bool IsString(Token t)
        {
            return (t.Text.StartsWith("\"") & t.Text.EndsWith("\""));
        }

        private bool IsBoolean()
        {
            return IsBoolean(currentToken);
        }

        private bool IsBoolean(Token t)
        {
            return (t.Text == "true") | (t.Text == "false");
        }

        private bool IsOrderedPair()
        {
            return IsOrderedPair(currentToken);
        }

        private bool IsOrderedPair(Token t)
        {
            OrderedPair op;
            return OrderedPair.TryParse(t.Text, out op);
        }

        private void ParseExpression(Segment segment, params string[] endTokens)
        {
            Stack<Token> s = new Stack<Token>();
            Queue<Token> output = new Queue<Token>();

            Func<bool> CanPush = new Func<bool>(delegate()
                {
                    return IsInteger() | IsFloat() | IsBoolean() | IsString() | IsOrderedPair() | ContainsVariable(currentToken.Text);
                });

            Func<Token, bool> IsFunction = new Func<Token, bool>(t =>
                {
                    return !IsInteger(t) & !IsFloat(t) & !IsBoolean(t) & !IsString(t) & !IsOrderedPair(t) & !ContainsVariable(t.Text);
                });


            Action<Token> PushToken = new Action<Token>(t => 
                {
                    if (IsInteger(t))
                    {
                        Pushl(JInteger.Parse(t.Text), segment);
                    }
                    else if (IsFloat(t))
                    {
                        Pushl(JFloat.Parse(t.Text), segment);
                    }
                    else if (IsBoolean(t))
                    {
                        Pushl(JBoolean.Parse(t.Text), segment);
                    }
                    else if (IsString(t))
                    {
                        Pushl(new JString(t.Text.Trim('\"')), segment);
                    }
                    else if (IsOrderedPair(t))
                    {
                        Pushl(OrderedPair.Parse(t.Text), segment);
                    }
                    else if (ContainsVariable(t.Text))
                    {
                        Push(new Pseudopointer(t.Text), segment);
                    }
                });

            Token token = new Token();
            while (HasToken())
            {
                token = GetToken();
                foreach (string t in endTokens)
                {
                    // If we've hit an end token
                    if (t == token.Text)
                    {
                        // Check that there aren't values on the stack. If there are, the expression is invalid.
                        if (s.Count > 0)
                            ErrorBuilder.BuildError(token, "Invalid expression.", ref errors);

                        // Then exit the function
                        return;
                    }
                }

                if (CanPush())
                {
                    PushToken(currentToken);
                }
                else if (token.Text == ",")
                {
                    while (s.Count > 0 && s.Peek().Text == "(")
                    {
                        PushToken(s.Pop());
                    }
                    if (s.Count == 0)
                    {
                        ErrorBuilder.BuildError(token, "Parentheses mismatch.", ref errors);
                        return;
                    }
                }
                else if (token.Text == "(")
                {
                    s.Push(token);
                }
                else if (token.Text == ")")
                {
                    while (s.Count > 0 && s.Peek().Text == "(")
                    {
                        PushToken(s.Pop());
                    }
                    if (s.Count == 0)
                    {
                        ErrorBuilder.BuildError(token, "Parentheses mismatch. ')' without '('.", ref errors);
                        return;
                    }
                    s.Pop();
                    if (s.Count > 0 && IsFunction(s.Peek()))
                        Call(new Pseudopointer(s.Pop().Text), segment);
                }
                else
                {
                    s.Push(token);
                }
            }

            while (s.Count > 0)
            {
                if (s.Peek().Text == "(")
                {
                    ErrorBuilder.BuildError(token, "Parentheses mismatch. '(' without ')'.", ref errors);
                    return;
                }

                PushToken(s.Pop());
            }
        }
    }
}
