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
        IBulletManager defaultManager = new TestManager(
            rank: () => 0.3246f,
            rand: () => 0.1337f
        );

        float Eval(float[] paramValues = null, IBulletManager m = null)
        {
            return eq.Eval(m ?? defaultManager, i => paramValues[i-1]);
        }

        [SetUp()]
        public void setupHarness()
        {
            eq = new BulletMLEquation();
        }

        [Test()]
        public void ReturnsZeroIfNoAst()
        {
            Assert.AreEqual(0, Eval());
        }

        [Test()]
        public void ReturnsZeroIfEmptyString()
        {
            eq.Astify("");
            Assert.AreEqual(0, Eval());
        }

        [Test()]
        public void ReturnsZeroIfWhitespaceString()
        {
            eq.Astify(" \t\r\n");
            Assert.AreEqual(0, Eval());
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
            Assert.AreEqual(42, Eval());
        }

        [Test()]
        public void ParsesDecimal()
        {
            eq.Astify(Math.PI.ToString());
            Assert.AreEqual((float)Math.PI, Eval());
        }

        [Test()]
        public void ParsesNegative()
        {
            eq.Astify("-1.23");
            Assert.AreEqual(-1.23f, Eval());
        }

        [Test()]
        public void ParsesNumberWithWhitespace()
        {
            eq.Astify("\n\t123\n   ");
            Assert.AreEqual(123, Eval());
        }

        [Test()]
        public void ParsesExpressionWithWhitespace()
        {
            eq.Astify("\n\t2 + 5.5\n   ");
            Assert.AreEqual(7.5f, Eval());
        }

        [Test()]
        public void AppliesAddition()
        {
            eq.Astify("2 + 5.5");
            Assert.AreEqual(7.5f, Eval());
        }

        [Test()]
        public void AppliesMinus()
        {
            eq.Astify("2 - 5.5");
            Assert.AreEqual(-3.5f, Eval());
        }

        [Test()]
        public void AppliesTimes()
        {
            eq.Astify("2 * 5.5");
            Assert.AreEqual(11f, Eval());
        }

        [Test()]
        public void AppliesDivide()
        {
            eq.Astify("5.5 / 2");
            Assert.AreEqual(2.75f, Eval());
        }

        [Test()]
        public void AppliesModulo()
        {
            eq.Astify("5.5 % 2");
            Assert.AreEqual(1.5f, Eval());
        }

        [Test()]
        public void AppliesMultipleOperations()
        {
            eq.Astify("1 + 2 + 3");
            Assert.AreEqual(6, Eval());
        }

        [Test()]
        public void AppliesOrderOfOperations()
        {
            eq.Astify("-7 + 3 * -4 - 10 % 3 + 13 / 2 - -9");
            Assert.AreEqual(-4.5f, Eval());
        }

        [Test()]
        public void AppliesParens()
        {
            eq.Astify("2 * (3 + 5)");
            Assert.AreEqual(16, Eval());
        }

        [Test()]
        public void AppliesNestedParens()
        {
            eq.Astify("2 * ((3 + 5) / 4)");
            Assert.AreEqual(4, Eval());
        }

        [Test()]
        public void AppliesJustParens()
        {
            eq.Astify("(3.5 + 5)");
            Assert.AreEqual(8.5f, Eval());
        }

        [Test()]
        public void AppliesJustNestedParens()
        {
            eq.Astify("((3.5 + 5))");
            Assert.AreEqual(8.5f, Eval());
        }

        [Test()]
        public void AppliesJustParensWithWhitespace()
        {
            eq.Astify("\n\t(3.5 + 5)\n  ");
            Assert.AreEqual(8.5f, Eval());
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
            Assert.AreEqual(3.5f, Eval(new[] { 2.5f }));
        }

        [Test()]
        public void ParsesJustParam()
        {
            eq.Astify("$2");
            Assert.AreEqual(2.5f, Eval(new[] { 3.2f, 2.5f }));
        }

        [Test()]
        public void ParsesMultipleParams()
        {
            eq.Astify("$1 + 3 * ($2 - $3)");
            Assert.AreEqual(-1.1f, Eval(new[] { 2.5f, 3.7f, 4.9f }), delta: 0.0001f);
        }

        [Test()]
        public void ThrowsInvalidOperationExceptionIfParamEvalFails()
        {
            eq.Astify("$1");
            Assert.Throws<InvalidOperationException>(delegate
            {
                Eval();
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
            Assert.AreEqual(-1.0262f, Eval(), delta: 0.0001f);
        }

        [Test()]
        public void ParsesRand()
        {
            eq.Astify("2 + $rand * 3 - 4");
            Assert.AreEqual(-1.5989f, Eval(), delta: 0.0001f);
        }

        [Test()]
        public void RecomputesRandEveryTime()
        {
            eq.Astify("$rand + $rand");
            int i = 0;
            var randManager = new TestManager(
                rank: () => 0f,
                rand: () =>
                {
                    if (i == 0)
                    {
                        i++;
                        return 2;
                    }
                    else
                        return 3;
                }
            );
            Assert.AreEqual(5f, Eval(m: randManager));
        }

        [Test()]
        public void ParsesComplexExpression()
        {
            eq.Astify("( ($rank))+2 *-3+ 2  /(\n4+ $rand )+1");
            Assert.AreEqual(-4.19157f, Eval(), delta: 0.0001f);
        }

        private class TestManager : IBulletManager
        {
            private Func<float> Rank;
            private Func<float> Rand;

            public TestManager(Func<float> rank, Func<float> rand)
            {
                Rank = rank;
                Rand = rand;
            }

            public float GameDifficulty
            {
                get { return Rank(); }
            }

            public float Random
            {
                get { return Rand(); }
            }

            public float PlayerX
            {
                get { throw new NotImplementedException(); }
            }

            public float PlayerY
            {
                get { throw new NotImplementedException(); }
            }

            public void RemoveBullet(Bullet deadBullet)
            {
                throw new NotImplementedException();
            }

            public Bullet CreateBullet()
            {
                throw new NotImplementedException();
            }
        }

    }
}
