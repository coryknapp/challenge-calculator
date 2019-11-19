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

        [Test]
        public void TestEmpty()
        {
            Assert.AreEqual(0, ChallengeCalculator.ChallengeCalculator.sumString(""));
            Assert.AreEqual(0, ChallengeCalculator.ChallengeCalculator.sumString(","));
        }

        [Test]
        public void TestSingle()
        {
            Assert.AreEqual(2, ChallengeCalculator.ChallengeCalculator.sumString("2"));
            Assert.AreEqual(0, ChallengeCalculator.ChallengeCalculator.sumString("goop"));
        }

        [Test]
        public void TestAdd()
        {
            Assert.AreEqual(4, ChallengeCalculator.ChallengeCalculator.sumString("2, 2"));
            Assert.AreEqual(2, ChallengeCalculator.ChallengeCalculator.sumString("2, bad"));
            Assert.AreEqual(0, ChallengeCalculator.ChallengeCalculator.sumString("bad, worse"));
        }

        [Test]
        public void TestMany()
        {
            Assert.AreEqual(78, ChallengeCalculator.ChallengeCalculator.sumString("1,2,3,4,5,6,7,8,9,10,11,12"));
            Assert.AreEqual(10, ChallengeCalculator.ChallengeCalculator.sumString("5,tytyt,5"));
        }

        [Test]
        public void TestWithNewLineDelimiter()
        {
            Assert.AreEqual(6, ChallengeCalculator.ChallengeCalculator.sumString("1\n2,3"));
            Assert.AreEqual(6, ChallengeCalculator.ChallengeCalculator.sumString("1\n2,bad\n3"));
        }

        [Test]
        public void TestRejectNegativeNumbers()
        {
            var exception = Assert.Throws<ArgumentException>( () => {
                ChallengeCalculator.ChallengeCalculator.sumString("2, -3, 4, -5");
            });
            Assert.AreEqual("Found these negatives: -3, -5", exception.Message);
        }

        [Test]
        public void TestNumbersGreaterThen1000()
        {
            Assert.AreEqual(6, ChallengeCalculator.ChallengeCalculator.sumString("1\n2,1001,3"));
            Assert.AreEqual(6, ChallengeCalculator.ChallengeCalculator.sumString("1001,1\n2,bad\n3"));
        }

        [Test]
        public void TestCustomDelimiter()
        {
            Console.SetIn( new StringReader( "//?\n1?2,3\n4\n\n") );
            var textWriter = new StringWriter();
            Console.SetOut( textWriter );
            ChallengeCalculator.ChallengeCalculator.Main( new string[0] );
            Assert.AreEqual("10", textWriter.ToString().Trim() );
        }

        [Test]
        public void TestMultiCharCustomDelimiter()
        {
            Console.SetIn(new StringReader("//[test]\n1test2,3\n4test5\n\n"));
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);
            ChallengeCalculator.ChallengeCalculator.Main(new string[0]);
            Assert.AreEqual("15", textWriter.ToString().Trim());
        }

        [Test]
        public void TestMultipleMultiCharCustomDelimiter()
        {
            Console.SetIn(new StringReader("//[*][!!][r9r]\n11r9r22*hh*33!!44\n\n"));
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);
            ChallengeCalculator.ChallengeCalculator.Main(new string[0]);
            Assert.AreEqual("110", textWriter.ToString().Trim());
        }
    }
}