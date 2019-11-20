using NUnit.Framework;
using System.IO;

using System;

namespace ChallengeCalculatorTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        private int runSumStringWithDefaultOptions(string input)
        {
            return ChallengeCalculator.ChallengeCalculator.sumString(
                input,
                new ChallengeCalculator.ChallengeCalculatorOptions(
                    new string[] { }
                    )
                ).sum;
        }

        [Test]
        public void TestEmpty()
        {
            Assert.AreEqual(0, runSumStringWithDefaultOptions(""));
            Assert.AreEqual(0, runSumStringWithDefaultOptions(","));
        }

        [Test]
        public void TestSingle()
        {
            Assert.AreEqual(2, runSumStringWithDefaultOptions("2"));
            Assert.AreEqual(0, runSumStringWithDefaultOptions("goop"));
        }

        [Test]
        public void TestAdd()
        {
            Assert.AreEqual(4, runSumStringWithDefaultOptions("2, 2"));
            Assert.AreEqual(2, runSumStringWithDefaultOptions("2, bad"));
            Assert.AreEqual(0, runSumStringWithDefaultOptions("bad, worse"));
        }

        [Test]
        public void TestMany()
        {
            Assert.AreEqual(78, runSumStringWithDefaultOptions("1,2,3,4,5,6,7,8,9,10,11,12"));
            Assert.AreEqual(10, runSumStringWithDefaultOptions("5,tytyt,5"));
        }

        [Test]
        public void TestWithNewLineDelimiter()
        {
            Assert.AreEqual(6, runSumStringWithDefaultOptions("1\n2,3"));
            Assert.AreEqual(6, runSumStringWithDefaultOptions("1\n2,bad\n3"));
        }

        [Test]
        public void TestRejectNegativeNumbers()
        {
            var exception = Assert.Throws<ArgumentException>( () => {
                runSumStringWithDefaultOptions("2, -3, 4, -5");
            });
            Assert.AreEqual("Found these negatives: -3, -5", exception.Message);
        }

        [Test]
        public void TestNumbersGreaterThen1000()
        {
            Assert.AreEqual(6, runSumStringWithDefaultOptions("1\n2,1001,3"));
            Assert.AreEqual(6, runSumStringWithDefaultOptions("1001,1\n2,bad\n3"));
        }

        [Test]
        public void TestCustomDelimiter()
        {
            Console.SetIn( new StringReader( "//?\n1?2,3\n4\n\n") );
            var textWriter = new StringWriter();
            Console.SetOut( textWriter );
            ChallengeCalculator.ChallengeCalculator.Main(new string[] { "--run-once" });
            Assert.AreEqual("1+2+3+4 = 10", textWriter.ToString().Trim() );
        }

        [Test]
        public void TestMultiCharCustomDelimiter()
        {
            Console.SetIn(new StringReader("//[test]\n1test2,3\n4test5\n\n"));
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);
            ChallengeCalculator.ChallengeCalculator.Main(new string[] { "--run-once" });
            Assert.AreEqual("1+2+3+4+5 = 15", textWriter.ToString().Trim());
        }

        [Test]
        public void TestMultipleMultiCharCustomDelimiter()
        {
            Console.SetIn(new StringReader("//[*][!!][r9r]\n11r9r22*hh*33!!44\n\n"));
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);
            ChallengeCalculator.ChallengeCalculator.Main(new string[] { "--run-once" } );
            Assert.AreEqual("11+22+0+33+44 = 110", textWriter.ToString().Trim());
        }

        [Test]
        public void TestAlternateDelimiterCommandLineOption()
        {
            Console.SetIn(new StringReader("1?2,3?4\n\n"));
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);
            ChallengeCalculator.ChallengeCalculator.Main(new string[] { "--run-once", "--alternate-delimiter", "?"} );
            Assert.AreEqual("1+2+3+4 = 10", textWriter.ToString().Trim());
        }

        [Test]
        public void TestAlternateDelimiterCommandLineOptionReplacesNewLineDelimiter()
        {
            // If we're defining an alternate delimiter to the one in step #3 then, a new line should
            // no longer work as a delimiter and the `2\n3` should be interpreted as a `0`

            Console.SetIn(new StringReader("1?2\n3,4\n\n"));
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);
            ChallengeCalculator.ChallengeCalculator.Main(new string[] { "--run-once", "--alternate-delimiter", "?"} );
            Assert.AreEqual("1+0+4 = 5", textWriter.ToString().Trim());
        }

        [Test]
        public void TestAllowNegativeNumbersCommandLineOption()
        {
            Console.SetIn(new StringReader("-1,-2,3,4\n\n"));
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);
            ChallengeCalculator.ChallengeCalculator.Main(new string[] { "--run-once", "--allow-negative"} );
            Assert.AreEqual("-1+-2+3+4 = 4", textWriter.ToString().Trim());
        }

        [Test]
        public void TestUpperBoundCommandLineOption()
        {
            Console.SetIn(new StringReader("1,100,101\n\n"));
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);
            ChallengeCalculator.ChallengeCalculator.Main(new string[] { "--run-once", "--upper-bound", "100"} );
            Assert.AreEqual("1+100+0 = 101", textWriter.ToString().Trim());
        }
    }
}