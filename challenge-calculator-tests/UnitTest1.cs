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
        public void TestThrows()
        {
            Assert.Throws<FormatException>(
                delegate {
                    ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString("2, 2, 2");
                });
            Assert.Throws<FormatException>(
                delegate {
                    ChallengeCalculator.ChallengeCalculator.sumCommaDelimitedString("bad, worse, ohno");
                });

        }
    }
}