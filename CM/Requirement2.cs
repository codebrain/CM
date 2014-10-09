using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CM
{
    [TestFixture]
    public class Requirement2
    {
        [Test]
        [TestCase(60, new[] { 1, 2, 3, 4, 5, 6, 10, 12, 15, 20, 30, 60 })]
        [TestCase(42, new[] { 1, 2, 3, 6, 7, 14, 21, 42 })]
        public void Test(int input, int[] expected)
        {
            CollectionAssert.AreEqual(expected, BruteForcePositiveDivisors(input));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void TestThrowsAndException(int input)
        {
            Assert.Throws<ArgumentException>(() => BruteForcePositiveDivisors(input).ToList());
        }

        /// <summary>
        /// Naive implementation that uses a brute-force approach.
        /// </summary>
        /// <param name="input">The number to check for positive divisors</param>
        /// <returns>The list of positive divisors</returns>
        public static IEnumerable<int> BruteForcePositiveDivisors(int input)
        {
            if (input <= 1)
            {
                throw new ArgumentException("Input must be a positive integer greater than 1", "input");
            }

            yield return 1;

            foreach (var divisor in Enumerable.Range(2, input / 2).Where(a => input % a == 0))
            {
                yield return divisor;
            }

            yield return input;
        }
    }
}