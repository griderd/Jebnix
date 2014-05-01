using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.Types;
using Jebnix;

namespace KerboScriptEngine.Compiler
{
    partial class Parser
    {
        private void SetLock(bool set = true)
        {
            if (!ExpectToken("variable name."))
                return;

            Pseudopointer var = new Pseudopointer(currentToken.Text);
            if (!variableNames.Contains(currentToken.Text))
            {
                variableNames.Add(currentToken.Text);
            }

            if (!ExpectToken("'=' or 'to'."))
                return;

            if (currentToken.Text == "=" | currentToken.Text == "to")
            {
                Segment s;
                if (set)
                    s = Segment.Code;
                else
                    s = Segment.Locks;

                if (!set)
                {
                    Lok(var);
                }

                ParseExpression(s, ".");
                Popv(var, s);
                if (!set)
                {
                    Ret(Segment.Locks);
                }
            }
        }

        private void Unlock()
        {
            Token t = currentToken;
            GetToken();
            if (!HasToken())
            {
                ErrorBuilder.BuildError(t, "Expected variable.", ref errors);
                return;
            }
            Pseudopointer var = new Pseudopointer(currentToken.Text);
            GetToken();
            if (!HasToken())
            {
                ErrorBuilder.BuildError(t, "Expected '.'", ref errors);
                return;
            }
            if (var.Value != ".")
            {
                ErrorBuilder.BuildError(t, "Expected '.'", ref errors);
                return;
            }

            ULok(var);
        }

        private void IfBlock()
        {
            ParseExpression(Segment.Code, "{");

            branchStack.Push(branchCount);
            branchCount++;
            string ifLabel = MakeLabel("if_" + branchStack.Peek().ToString(), Segment.Code);
            string elseLabel = MakeLabel("else_" + branchStack.Peek().ToString(), Segment.Code);
            string endIfLabel = MakeLabel("endif_" + branchStack.Peek().ToString(), Segment.Code);

            Jumpt(new Pseudopointer(ifLabel));
            Jumpf(new Pseudopointer(elseLabel));
            Pop();
            Hop(new Pseudopointer(endIfLabel));

            AddLabel(ifLabel, Segment.Code);
            blockStack.Push(Blocks.IfElse);
            blockStack.Push(Blocks.If);
        }

        private void ElseBlock()
        {
            string elseLabel = MakeLabel("else_" + branchStack.Peek().ToString(), Segment.Code);

            Ret();
            AddLabel(elseLabel, Segment.Code);
            blockStack.Push(Blocks.Else);            
        }

        private void EndIfBlock()
        {
            Ret();
            string endIfLabel = MakeLabel("endif_" + branchStack.Peek().ToString(), Segment.Code);
            AddLabel(endIfLabel, Segment.Code);
        }

        private void UntilBlock()
        {
            loopStack.Push(loopCount);
            loopCount++;
            string untilLabel = MakeLabel("loop_" + loopStack.Peek().ToString(), Segment.Code);
            string bodyLabel = MakeLabel("loopbody_" + loopStack.Peek().ToString(), Segment.Code);
            string endLabel = MakeLabel("endloop_" + loopStack.Peek().ToString(), Segment.Code);

            Pseudopointer until = new Pseudopointer(untilLabel);
            Pseudopointer body = new Pseudopointer(bodyLabel);
            Pseudopointer end = new Pseudopointer(endLabel);

            Hop(body);

            AddLabel(untilLabel, Segment.Code);
            Pop();

            AddLabel(bodyLabel, Segment.Code);
            ParseExpression(Segment.Code, "{");
            Hopt(end);

            blockStack.Push(Blocks.Until);
        }

        private void EndUntilBlock()
        {
            string untilLabel = MakeLabel("loop_" + loopStack.Peek().ToString(), Segment.Code);
            string endLabel = MakeLabel("endloop_" + loopStack.Peek().ToString(), Segment.Code);
            Hopf(new Pseudopointer(untilLabel));
            AddLabel(endLabel, Segment.Code);
            Pop();
            loopStack.Pop();
        }

        private void WhileBlock()
        {
            loopStack.Push(loopCount);
            loopCount++;
            string whileLabel = MakeLabel("loop_" + loopStack.Peek().ToString(), Segment.Code);
            string bodyLabel = MakeLabel("loopbody_" + loopStack.Peek().ToString(), Segment.Code);
            string endLabel = MakeLabel("endloop_" + loopStack.Peek().ToString(), Segment.Code);

            Pseudopointer whilel = new Pseudopointer(whileLabel);
            Pseudopointer body = new Pseudopointer(bodyLabel);
            Pseudopointer end = new Pseudopointer(endLabel);

            Hop(body);

            AddLabel(whileLabel, Segment.Code);
            Pop();

            AddLabel(bodyLabel, Segment.Code);
            ParseExpression(Segment.Code, "{");
            Hopf(end);

            blockStack.Push(Blocks.Until);
        }

        private void EndWhileBlock()
        {
            string whileLabel = MakeLabel("loop_" + loopStack.Peek().ToString(), Segment.Code);
            string endLabel = MakeLabel("endloop_" + loopStack.Peek().ToString(), Segment.Code);
            Hopt(new Pseudopointer(whileLabel));
            AddLabel(endLabel, Segment.Code);
            Pop();
            loopStack.Pop();
        }

        private void DoWhileBlock()
        {
            loopStack.Push(loopCount);
            loopCount++;
            string whileLabel = MakeLabel("loop_" + loopStack.Peek().ToString(), Segment.Code);
            string bodyLabel = MakeLabel("loopbody_" + loopStack.Peek().ToString(), Segment.Code);

            Hop(new Pseudopointer(bodyLabel));
            AddLabel(whileLabel, Segment.Code);
            Pop();
            AddLabel(bodyLabel, Segment.Code);
        }

        private void EndDoWhileBlock()
        {
            ParseExpression(Segment.Code, ".");
            string whileLabel = MakeLabel("while_" + loopStack.Peek().ToString(), Segment.Code);
            Hopt(new Pseudopointer(whileLabel));
            string endLabel = MakeLabel("endloop_" + loopStack.Peek().ToString(), Segment.Code);
            AddLabel(endLabel, Segment.Code);
            Pop();

            loopStack.Pop();
        }

        private void Break()
        {
            Token t = currentToken;
            GetToken();
            if (!HasToken())
            {
                ErrorBuilder.BuildError(t, "Expected '.'", ref errors);
                return;
            }
            if (currentToken.Text != ".")
            {
                ErrorBuilder.BuildError(t, "Expected '.'", ref errors);
                return;
            }

            if (loopStack.Count > 0)
            {
                string endLabel = MakeLabel("endloop_" + loopStack.Peek().ToString(), Segment.Code);
                Hop(new Pseudopointer(endLabel), Segment.Code);
            }
            else
            {
                ErrorBuilder.BuildError(currentToken, "Warning - No loops to break out of!", ref errors);
            }
        }


    }
}
