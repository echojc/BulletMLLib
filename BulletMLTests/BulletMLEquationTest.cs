using NUnit.Framework;
using System;
using BulletMLLib;
using Sprache;

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
        public void AppliesJustParens()
        {
            eq.Astify("(3.5 + 5)");
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
    }
}
