using NUnit.Framework;
using System;
using BulletMLLib;
using Sprache;
using System.Collections.Generic;

namespace BulletMLTests
{
    [TestFixture()]
    public class BulletMLEquationTest
    {
        BulletMLEquation eq;

        [SetUp()]
        public void setupHarness()
        {
            eq = new BulletMLEquation();
        }

        [Test()]
        public void ReturnsZeroIfNoAst()
        {
            Assert.AreEqual(0, eq.Eval());
        }

        [Test()]
        public void ReturnsZeroIfEmptyString()
        {
            eq.Astify("");
            Assert.AreEqual(0, eq.Eval());
        }

        [Test()]
        public void ReturnsZeroIfWhitespaceString()
        {
            eq.Astify(" \t\r\n");
            Assert.AreEqual(0, eq.Eval());
        }

        [Test()]
        public void ThrowsParseExceptionOnString()
        {
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("1 + aoeuhn - 2");
            });
        }

        [Test()]
        public void ThrowsParseExceptionOnUnknownOperator()
        {
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("1 = 2");
            });
        }

        [Test()]
        public void ThrowsParseExceptionOnMultipleOperators()
        {
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("1 * + 2");
            });
        }

        [Test()]
        public void ThrowsParseExceptionOnMultipleNumbers()
        {
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("1 2 3");
            });
        }

        [Test()]
        public void ParsesInteger()
        {
            eq.Astify("42");
            Assert.AreEqual(42, eq.Eval());
        }

        [Test()]
        public void ParsesDecimal()
        {
            eq.Astify(Math.PI.ToString());
            Assert.AreEqual((float)Math.PI, eq.Eval());
        }

        [Test()]
        public void ParsesNegative()
        {
            eq.Astify("-1.23");
            Assert.AreEqual(-1.23f, eq.Eval());
        }

        [Test()]
        public void ParsesNumberWithWhitespace()
        {
            eq.Astify("\n\t123\n   ");
            Assert.AreEqual(123, eq.Eval());
        }

        [Test()]
        public void ParsesExpressionWithWhitespace()
        {
            eq.Astify("\n\t2 + 5.5\n   ");
            Assert.AreEqual(7.5f, eq.Eval());
        }

        [Test()]
        public void AppliesAddition()
        {
            eq.Astify("2 + 5.5");
            Assert.AreEqual(7.5f, eq.Eval());
        }

        [Test()]
        public void AppliesMinus()
        {
            eq.Astify("2 - 5.5");
            Assert.AreEqual(-3.5f, eq.Eval());
        }

        [Test()]
        public void AppliesTimes()
        {
            eq.Astify("2 * 5.5");
            Assert.AreEqual(11f, eq.Eval());
        }

        [Test()]
        public void AppliesDivide()
        {
            eq.Astify("5.5 / 2");
            Assert.AreEqual(2.75f, eq.Eval());
        }

        [Test()]
        public void AppliesModulo()
        {
            eq.Astify("5.5 % 2");
            Assert.AreEqual(1.5f, eq.Eval());
        }

        [Test()]
        public void AppliesMultipleOperations()
        {
            eq.Astify("1 + 2 + 3");
            Assert.AreEqual(6, eq.Eval());
        }

        [Test()]
        public void AppliesOrderOfOperations()
        {
            eq.Astify("-7 + 3 * -4 - 10 % 3 + 13 / 2 - -9");
            Assert.AreEqual(-4.5f, eq.Eval());
        }

        [Test()]
        public void AppliesParens()
        {
            eq.Astify("2 * (3 + 5)");
            Assert.AreEqual(16, eq.Eval());
        }

        [Test()]
        public void AppliesNestedParens()
        {
            eq.Astify("2 * ((3 + 5) / 4)");
            Assert.AreEqual(4, eq.Eval());
        }

        [Test()]
        public void AppliesJustParens()
        {
            eq.Astify("(3.5 + 5)");
            Assert.AreEqual(8.5f, eq.Eval());
        }

        [Test()]
        public void AppliesJustNestedParens()
        {
            eq.Astify("((3.5 + 5))");
            Assert.AreEqual(8.5f, eq.Eval());
        }

        [Test()]
        public void AppliesJustParensWithWhitespace()
        {
            eq.Astify("\n\t(3.5 + 5)\n  ");
            Assert.AreEqual(8.5f, eq.Eval());
        }

        [Test()]
        public void ThrowsParseExceptionOnMismatchedLeftParens()
        {
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("2 * (3 + 5)(");
            });
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("2 * ((3 + 5)");
            });
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("2 (3 + 5)");
            });
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("(2 * (3 + 5)");
            });
        }

        [Test()]
        public void ThrowsParseExceptionOnMismatchedRightParens()
        {
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("2 * (3 + 5))");
            });
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("2 * ()3 + 5)");
            });
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify(")2 * (3 + 5)");
            });
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("2) * (3 + 5)");
            });
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("(3 + 5) 2");
            });
        }

        [Test()]
        public void ParsesParam()
        {
            eq.Astify("$1 + 1");
            Assert.AreEqual(3.5f, eq.Eval(i =>
            {
                if (i == 1)
                    return 2.5f;
                throw new AssertionException("expected param $1");
            }));
        }

        [Test()]
        public void ParsesJustParam()
        {
            eq.Astify("$2");
            Assert.AreEqual(2.5f, eq.Eval(i =>
            {
                if (i == 2) return 2.5f;
                throw new AssertionException("expected param $2");
            }));
        }

        [Test()]
        public void ParsesMultipleParams()
        {
            eq.Astify("$1 + 3 * ($2 - $3)");
            Assert.AreEqual(-1.1f, eq.Eval(i =>
            {
                if (i == 1) return 2.5f;
                if (i == 2) return 3.7f;
                if (i == 3) return 4.9f;
                throw new AssertionException("expected param $1, $2 or $3");
            }), delta: 0.0001f);
        }

        [Test()]
        public void ThrowsInvalidOperationExceptionIfParamEvalFails()
        {
            eq.Astify("$1");
            Assert.Throws<InvalidOperationException>(delegate
            {
                eq.Eval(i => new float[]{}[0]);
            });
        }

        [Test()]
        public void ThrowsParseExceptionIfParamIsNegative()
        {
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("$-1 + $2");
            });
        }

        [Test()]
        public void ThrowsParseExceptionIfParam0()
        {
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("$0 + $1");
            });
        }

        [Test()]
        public void ThrowsParseExceptionIfParamStartsWith0()
        {
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("$01 + $2");
            });
        }

        [Test()]
        public void ThrowsParseExceptionIfParamBiggerThanMaxInt()
        {
            Assert.Throws<ParseException>(delegate
            {
                eq.Astify("$3000000000");
            });
        }

        [Test()]
        public void ParsesRank()
        {
            eq.Astify("2 + $rank * 3 - 4");
            Assert.AreEqual(-2f, eq.Eval());
        }

        [Test()]
        public void ParsesComplexExpression()
        {
            eq.Astify("( ($rank))+2 *-3+ 2  /(\n4+ $rand )+1");
            Assert.AreEqual(-4.5f, eq.Eval());
        }
    }
}
