using NUnit.Framework;
using ChallengeCalculator;

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
            Assert.AreEqual(0, ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString(""));
            Assert.AreEqual(0, ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString(","));
        }

        [Test]
        public void TestSingle()
        {
            Assert.AreEqual(2, ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString("2"));
            Assert.AreEqual(0, ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString("goop"));
        }

        [Test]
        public void TestAdd()
        {
            Assert.AreEqual(4, ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString("2, 2"));
            Assert.AreEqual(-1, ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString("2, -3"));
            Assert.AreEqual(2, ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString("2, bad"));
            Assert.AreEqual(0, ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString("bad, worse"));
        }

        [Test]
        public void TestMany()
        {
            Assert.AreEqual(78, ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString("1,2,3,4,5,6,7,8,9,10,11,12"));
            Assert.AreEqual(10, ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString("5,tytyt,5"));
        }

        [Test]
        public void TestWithNewLineDelimiter()
        {
            Assert.AreEqual(6, ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString("1\n2,3"));
            Assert.AreEqual(6, ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString("1\n2,bad\n3"));
        }
    }
}