using System.Linq;
using NUnit.Framework;

namespace CM
{
    [TestFixture]
    public class Requirement4
    {
        public static int[] GetCommonNumbers(int[] input)
        {
            // If only one input then return it
            if (input.Length == 1)
            {
                return input;
            }

            // Group input by value, transform to dictionary with value and count
            var grouped = input.GroupBy(i => i).ToDictionary(i => i.Key, i => i.Count());
            if (grouped.Count == input.Length)
            {
                // All items are common
                return input;
            }

            // What is the common item count?
            var ordered = grouped.OrderByDescending(g => g.Value);
            var commonCount = ordered.First().Value;

            // Select from the group while the common count is the same
            return ordered.TakeWhile(g => g.Value == commonCount)
                          .Select(g => g.Key)
                          .ToArray();
        }

        [Test]
        [TestCase(new[] { 2, 4, 3, 5, 4, 5, 1, 6, 1, 2, 5, 4 }, new[] { 4, 5 })]
        [TestCase(new[] { 5, 4, 3, 2, 4, 5, 1, 6, 1, 2, 5, 4 }, new[] { 5, 4 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 1, 6, 7 }, new[] { 1 })]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7 }, new[] { 1, 2, 3, 4, 5, 6, 7 })]
        [TestCase(new[] { 1 }, new[] { 1 })]
        public void Test(int[] input, int[] expected)
        {
            CollectionAssert.AreEqual(expected, GetCommonNumbers(input));
        }
    }
}