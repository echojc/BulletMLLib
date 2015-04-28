using System;
using System.Linq;
using Sprache;
using System.Collections.Generic;

namespace BulletMLLib
{
    /// <summary>
    /// This is an equation used in BulletML nodes.
    /// </summary>
    public class BulletMLEquation
    {
        private IEnumerable<Ast> rpn = null;

        public void Astify(string text)
        {
            if (text == null || (text = text.Trim()) == string.Empty)
                return;

            rpn = Shunt(expression().End().Parse(text));
        }

        private IEnumerable<Ast> Shunt(IEnumerable<Ast> input)
        {
            List<Ast> output = new List<Ast>();
            Stack<WithPrecedence> ops = new Stack<WithPrecedence>();

            foreach (Ast next in input)
            {
                if (next is Operator)
                {
                    Operator op = (Operator)next;

                    while (ops.Count > 0 && ops.Peek().Precedence >= op.Precedence)
                        output.Add(ops.Pop());

                    ops.Push(op);
                }
                else if (next is LeftParens)
                {
                    ops.Push((LeftParens)next);
                }
                else if (next is RightParens)
                {
                    while (!(ops.Peek() is LeftParens))
                        output.Add(ops.Pop());

                    // discard left parens
                    ops.Pop();
                }
                else // number
                {
                    output.Add(next);
                }
            }

            // append remaining operators
            while (ops.Count > 0)
                output.Add(ops.Pop());

            return output;
        }

        public float Eval(Func<int, float> paramResolver)
        {
            if (rpn == null)
                return 0;

            Stack<float> stack = new Stack<float>();

            foreach (Ast next in rpn)
            {
                if (next is Number)
                {
                    stack.Push(((Number)next).Value);
                }
                else
                {
                    if (stack.Count < 2)
                        throw new InvalidOperationException("Expression couldn't be evaluated.");
                    float rhs = stack.Pop();
                    float lhs = stack.Pop();
                    stack.Push(((Operator)next).Apply(lhs, rhs));
                }
            }

            if (stack.Count > 1)
                throw new InvalidOperationException("Expression couldn't be evaluated.");
            else if (stack.Count == 0)
                return 0;
            else
                return stack.Pop();
        }

        public float Eval()
        {
            // TODO: maybe throw exception?
            return Eval(_ => 0f);
        }

        private Parser<IEnumerable<Ast>> expression()
        {
            return from lhs in term()
                   from rest in
                       Parse.Optional(
                           from op1 in op()
                           from rhs in expression()
                           select new[] { op1 }.Concat(rhs)
                       )
                   select rest.IsDefined ? lhs.Concat(rest.Get()) : lhs;
        }

        private Parser<IEnumerable<Ast>> term()
        {
            return parens().Or(number().Select(n => new[] { n }));
        }

        private Parser<IEnumerable<Ast>> parens()
        {
            return from lp in Parse.Char('(').Token()
                   from expr in expression()
                   from rp in Parse.Char(')').Token()
                   select new[] { new LeftParens() }.Concat(expr).Concat(new[] { new RightParens() });
        }

        private Parser<Number> number()
        {
            return from negative in Parse.Optional(Parse.Char('-'))
                   from value in Parse.Decimal
                   select new Number((float)(decimal.Parse(value) * (negative.IsDefined ? -1 : 1)));
        }

        private Parser<Operator> op()
        {
            return from op in Parse.Chars("+-*/%").Token()
                   select Operator.fromSymbol(op);
        }
    }

    internal interface Ast { }

    internal class Number : Ast
    {
        internal Number(float value)
        {
            Value = value;
        }
        internal float Value { get; private set; }
    }

    internal interface WithPrecedence : Ast
    {
        int Precedence { get; }
    }

    internal class Operator : WithPrecedence
    {
        private Operator(Func<float, float, float> fun, int precedence)
        {
            Apply = fun;
            Precedence = precedence;
        }
        internal Func<float, float, float> Apply { get; private set; }
        public int Precedence { get; private set; }
        internal static Operator fromSymbol(char symbol)
        {
            switch (symbol)
            {
                case '+': return new Operator((lhs, rhs) => lhs + rhs, 0);
                case '-': return new Operator((lhs, rhs) => lhs - rhs, 0);
                case '*': return new Operator((lhs, rhs) => lhs * rhs, 1);
                case '/': return new Operator((lhs, rhs) => lhs / rhs, 1);
                case '%': return new Operator((lhs, rhs) => lhs % rhs, 1);
                default: throw new ArgumentException("Invalid operator " + symbol, "symbol");
            }
        }
    }

    internal class LeftParens : WithPrecedence
    {
        public int Precedence { get { return -1; } }
    }

    internal class RightParens : WithPrecedence
    {
        public int Precedence { get { return -1; } }
    }
}

