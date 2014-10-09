using NUnit.Framework;

namespace CM
{
    public static class StringExtensionMethods
    {
        public static bool IsNullOrEmptyV2(this string input)
        {
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
