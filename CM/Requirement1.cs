using NUnit.Framework;

namespace CM
{
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Determines whether the specified input is null or empty.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>True if null or empty, otherwise false.</returns>
        public static bool IsNullOrEmptyV2(this string input)
        {
            // Check null, then length
            return input == null || input.Length == 0;
        }
    }

    [TestFixture]
    public class Requirement1
    {
        [Test]
        [TestCase(null, true)]
        [TestCase("a", false)]
        [TestCase("", true)]
        [TestCase("null", false)]
        public void Test(string input, bool expected)
        {
            Assert.AreEqual(expected, input.IsNullOrEmptyV2());
        }
    }
}
