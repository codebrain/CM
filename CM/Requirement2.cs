using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CM
{
    [TestFixture]
    public class Requirement2
    {
        /// <summary>
        ///   Find the positive divisors of the input integer.
        /// </summary>
        /// <param name="input">The number to check for positive divisors</param>
        /// <returns>The list of positive divisors</returns>
        /// <remarks>
        ///  Naive implementation that uses a simplistic brute-force approach.
        ///  This could be further improved if the input range was known, perhaps using a cached lookup table.
        ///  This implementation doesn't check to see if input is prime (perhaps using a lookup table if the input range was known - http://www.umopit.ru/CompLab/primes32eng.htm).
        ///  Basically, if there were additional requirements, I would have adjusted the solution :)
        /// </remarks>
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

        [Test]
        [TestCase(60, new[] { 1, 2, 3, 4, 5, 6, 10, 12, 15, 20, 30, 60 })]
        [TestCase(42, new[] { 1, 2, 3, 6, 7, 14, 21, 42 })]
        [TestCase(10831, new[] { 1, 10831 })] // Prime
        public void Test(int input, int[] expected)
        {
            CollectionAssert.AreEqual(expected, BruteForcePositiveDivisors(input));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void TestThrowsAnException(int input)
        {
            Assert.Throws<ArgumentException>(() => BruteForcePositiveDivisors(input).ToList());
        }
    }
}