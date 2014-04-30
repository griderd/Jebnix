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


        private void Clearscreen()
        {
            Token t = currentToken;
            GetToken();
            if (!HasToken())
            {
                ErrorBuilder.BuildError(t, "Expected '.'", ref errors);
                return;
            }

            if (currentToken.Text == ".")
            {
                Call(new Pseudopointer(Functions.GenerateUniqueName("stdio", "clearscreen", 0)));
            }
        }

        private void SetLock(bool set = true)
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
                ErrorBuilder.BuildError(t, "Expected '=' or 'to'.", ref errors);
                return;
            }

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
            string untilLabel = MakeLabel("until_" + loopStack.Peek().ToString(), Segment.Code);
            string bodyLabel = MakeLabel("untilbody_" + loopStack.Peek().ToString(), Segment.Code);
            string endLabel = MakeLabel("enduntil_" + loopStack.Peek().ToString(), Segment.Code);

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
            string untilLabel = MakeLabel("until_" + loopStack.Peek().ToString(), Segment.Code);
            string endLabel = MakeLabel("enduntil_" + loopStack.Peek().ToString(), Segment.Code);
            Hopf(new Pseudopointer(untilLabel));
            AddLabel(endLabel, Segment.Code);
            Pop();
            loopStack.Pop();
        }

        private void WhileBlock()
        {
            loopStack.Push(loopCount);
            loopCount++;
            string whileLabel = MakeLabel("while_" + loopStack.Peek().ToString(), Segment.Code);
            string bodyLabel = MakeLabel("whilebody_" + loopStack.Peek().ToString(), Segment.Code);
            string endLabel = MakeLabel("endwhile_" + loopStack.Peek().ToString(), Segment.Code);

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
            string whileLabel = MakeLabel("while_" + loopStack.Peek().ToString(), Segment.Code);
            string endLabel = MakeLabel("endwhile_" + loopStack.Peek().ToString(), Segment.Code);
            Hopt(new Pseudopointer(whileLabel));
            AddLabel(endLabel, Segment.Code);
            Pop();
            loopStack.Pop();
        }

        private void DoWhileBlock()
        {
            loopStack.Push(loopCount);
            loopCount++;
            string whileLabel = MakeLabel("while_" + loopStack.Peek().ToString(), Segment.Code);
            string bodyLabel = MakeLabel("whilebody_" + loopStack.Peek().ToString(), Segment.Code);

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
            Pop();

            loopStack.Pop();
        }
    }
}
