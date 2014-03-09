using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerboScriptEngine
{
    /// <summary>
    /// Tokenizer helper class. Tokenizes Kerboscript++.
    /// </summary>
    class Tokenizer
    {
        /// <summary>
        /// Splits the line into parsable tokens.
        /// </summary>
        /// <param name="line">Line to tokenize</param>
        /// <param name="err">A list of errors that occured while tokenizing.</param>
        /// <returns>Returns a string array containing extracted tokens.</returns>
        public static string[] Tokenize(LineInfo line, out string[] err)
        {
            List<string> errors = new List<string>();
            List<string> tokens = new List<string>();
            StringBuilder token = new StringBuilder();

            bool inString = false;
            bool ctrlChar = false;

            string s = line.Line;

            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToLower()[i];
                char nextc;
                if (i + 1 < s.Length)
                    nextc = s[i + 1];
                else
                    nextc = '\0';

                switch (c)
                {
                    case '\"':
                        inString = !inString;
                        token.Append(c);
                        continue;

                    case ' ':
                        if (!inString)
                        {
                            tokens.Add(token.ToString());
                            token.Clear();
                        }
                        else
                            token.Append(c);
                        continue;

                    case '.':
                        if ((inString) | (token.Length > 0 && char.IsDigit(token[token.Length - 1]) && char.IsDigit(nextc)))
                            token.Append(c);
                        else
                        {
                            tokens.Add(token.ToString());
                            token.Clear();
                            tokens.Add(c.ToString());
                        }
                        continue;

                    case '*':
                    case '/':
                    case '%':
                    case '^':
                    case '|':
                    case '&':
                    case '{':
                    case '}':
                    case '(':
                    case ')':
                    case '[':
                    case ']':
                        if (inString)
                            token.Append(c);
                        else
                        {
                            tokens.Add(token.ToString());
                            token.Clear();
                            tokens.Add(c.ToString());
                        
                        }
                        continue;

                    case '+':
                    case '-':
                    case '=':
                        if (inString)
                            token.Append(c);
                        else if (c == nextc)
                        {
                            if (token.Length > 0)
                            {
                                tokens.Add(token.ToString());
                                token.Clear();
                            }
                            token.Append(c);
                            token.Append(c);
                            tokens.Add(token.ToString());
                            token.Clear();
                            i++;
                        }
                        else
                        {
                            if (token.Length > 0)
                            {
                                tokens.Add(token.ToString());
                                token.Clear();
                            }
                            token.Append(c);
                            tokens.Add(token.ToString());
                            token.Clear();
                        }
                        continue;

                    case '!':
                    case '>':
                    case '<':
                        if (inString)
                            token.Append(c);
                        else if (nextc == '=')
                        {
                            if (token.Length > 0)
                            {
                                tokens.Add(token.ToString());
                                token.Clear();
                            }
                            token.Append(c);
                            token.Append(nextc);
                            tokens.Add(token.ToString());
                            token.Clear();
                            i++;
                        }
                        else
                        {
                            if (token.Length > 0)
                            {
                                tokens.Add(token.ToString());
                                token.Clear();
                            }
                            token.Append(c);
                            tokens.Add(token.ToString());
                            token.Clear();
                        }
                        continue;

                    case '\\':
                        if (inString)
                        {
                            if (!ctrlChar)
                                ctrlChar = true;
                            else
                            {
                                token.Append(c);
                                ctrlChar = false;
                            }
                        }
                        else
                        {
                            errors.Add("Syntax error (" + line.Filename + ":" + line.LineNumber.ToString() + "," + (i + 1 + line.ColumnOffset).ToString() + ") - Escape character '\\' invalid outside string literal."); 
                        }
                        continue;

                    default:
                        if (inString)
                        {
                            char seq = c;
                            if ((ctrlChar) && (InterpretEscapeSequence(c, out seq)))
                                ctrlChar = false;
                            token.Append(seq);
                        }
                        else
                        {
                            token.Append(c);
                        }
                        continue;
                }
            }

            if (token.Length > 0)
            {
                tokens.Add(token.ToString());
            }

            List<string> finalTokens = new List<string>();
            for (int i = 0; i < tokens.Count; i++)
            {
                string t = tokens[i].Trim();
                if (t != "")
                    finalTokens.Add(t);
            }

            err = errors.ToArray();
            return finalTokens.ToArray();
        }

        /// <summary>
        /// Interprets the given escape sequence.
        /// </summary>
        /// <param name="c">Character after the escape character</param>
        /// <param name="sequence">Translated sequence.</param>
        /// <returns>Returns true if the character given created a real escape sequence. Otherwise, returns false.</returns>
        static bool InterpretEscapeSequence(char c, out char sequence)
        {
            // NOTE: case '\\' is not included because it is handled by the Tokenize() function.
            switch (c)
            {
                case '0':
                    sequence = '\0';
                    return true;

                case 'n':
                    sequence = '\n';
                    return true;

                default:
                    sequence = c;
                    return false;
            }
        }
    }
}
