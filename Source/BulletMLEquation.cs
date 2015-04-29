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

            rpn = Shunt(Parsers.Complete().End().Parse(text));
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
                else // number or param
                {
                    output.Add(next);
                }
            }

            // append remaining operators
            while (ops.Count > 0)
                output.Add(ops.Pop());

            // there should always be an odd number of items, and
            // count(numbers) should be count(ops) + 1
            if (output.Count % 2 == 0 || output.Count(a => a is WithPrecedence) != output.Count / 2)
                throw new ParseException("Expression resulted in an unbalanced stack.");

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
                else if (next is Param)
                {
                    int index = ((Param)next).Index;
                    float result;
                    try
                    {
                        result = paramResolver(index);
                    } catch (Exception e)
                    {
                        throw new InvalidOperationException("Evaluating parameter caused an exception.", e);
                    }
                    stack.Push(result);
                }
                else if (next is Rank)
                {
                    // TODO
                    stack.Push(0f);
                }
                else if (next is Rand)
                {
                    // TODO
                    stack.Push(0f);
                }
                else if (next is Operator)
                {
                    if (stack.Count < 2)
                        throw new InvalidOperationException("Not enough values left on stack to apply operation.");
                    float rhs = stack.Pop();
                    float lhs = stack.Pop();
                    stack.Push(((Operator)next).Apply(lhs, rhs));
                }
                else
                {
                    throw new InvalidOperationException("Unknown value in stack [" + next + "].");
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

    }

    internal static class Parsers
    {
        internal static Parser<IEnumerable<Ast>> Complete()
        {
            return expression().End();
        }

        private static Parser<IEnumerable<Ast>> expression()
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

        private static Parser<IEnumerable<Ast>> term()
        {
            return parens().Or(
                       from ast in param().Or<Ast>(number()).Or(rank()).Or(rand())
                       select new[] { ast }
                   );
        }

        private static Parser<Param> param()
        {
            return from q in Parse.Char('$')
                   from first in Parse.Chars("123456789")
                   from rest in Parse.Optional(Parse.Number)
                   select new Param(first + rest.GetOrElse(""));
        }

        private static Parser<Rank> rank()
        {
            return from q in Parse.String("$rank").Token()
                   select new Rank();
        }

        private static Parser<Rand> rand()
        {
            return from q in Parse.String("$rand").Token()
                   select new Rand();
        }

        private static Parser<IEnumerable<Ast>> parens()
        {
            return from lp in Parse.Char('(').Token()
                   from expr in expression()
                   from rp in Parse.Char(')').Token()
                   select new[] { new LeftParens() }.Concat(expr).Concat(new[] { new RightParens() });
        }

        private static Parser<Number> number()
        {
            return from negative in Parse.Optional(Parse.Char('-'))
                   from value in Parse.Decimal
                   select new Number((float)(decimal.Parse(value) * (negative.IsDefined ? -1 : 1)));
        }

        private static Parser<Operator> op()
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

    internal class Param : Ast
    {
        internal Param(string index)
        {
            int result;
            if (!int.TryParse(index, out result))
                throw new ParseException("Could not parse parameter index.");
            Index = result;
        }
        internal int Index { get; private set; }
    }

    internal class Rank : Ast { }
    internal class Rand : Ast { }

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

