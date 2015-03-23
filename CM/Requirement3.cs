using System;
using System.Diagnostics;
using NUnit.Framework;

namespace CM
{
    [TestFixture]
    public class Requirement3
    {
        /// <summary>Computes the area of a triangle (Equilateral, Isosceles and Scalene)</summary>
        /// <param name="side1">The first side of the triangle.</param>
        /// <param name="side2">The second side of the triangle.</param>
        /// <param name="side3">The third side of the triangle.</param>
        /// <returns>Area of the triangle.</returns>
        private static double CalculateTriangleArea(int side1, int side2, int side3)
        {
            // Check sides are positive integer values
            if (side1 <= 0 || side2 <= 0 || side3 <= 0)
            {
                throw new InvalidTriangleException("A triangle must be composed of sides that have length > 0");
            }

            // Triangle test
            if ((side1 + side2) > side3 == false
                || (side1 + side3) > side2 == false
                || (side2 + side3) > side1 == false)
            {
                throw new InvalidTriangleException("Sides do not form a valid triangle");
            }

            // Check the composition of the sides in order to use a more performant area calculation algorithm
            var side1And2AreEqual = side1 == side2;
            var side2And3AreEqual = side2 == side3;

            // Equiliateral triangle (all sides equal length)
            if (side1And2AreEqual && side2And3AreEqual)
            {
                return CalculateEquilateralTriangleArea(side1);
            }

            // Isoceles triangle (two sides of equal length)
            if (side1And2AreEqual || side2And3AreEqual || side1 == side3)
            {
                return side1And2AreEqual
                    ? CalculateIsocelesTriangleArea(side1, side3)
                    : (side2And3AreEqual ? CalculateIsocelesTriangleArea(side2, side1)
                                         : CalculateIsocelesTriangleArea(side1, side2));
            }

            // I had considered implementing a right-angle check here but decided against it for the following reasons:
            // - Special right-angle triangles are uncommon (http://en.wikipedia.org/wiki/Special_right_triangles#Common_Pythagorean_triples)
            // - The computational cost of checking the right-angledness *likely* outweighs just simply using the scalene algorithm

            // Scalene (any length of sides)
            return CalculateScaleneTriangleArea(side1, side2, side3);
        }

        private static double CalculateIsocelesTriangleArea(int longSide, int shortSide)
        {
            // Use simplified Herons formula
            var d = (longSide * longSide * 4d) - (shortSide * shortSide);
            var area = shortSide * Math.Sqrt(d) / 4d;
            return area;
        }

        private static double CalculateScaleneTriangleArea(int side1, int side2, int side3)
        {
            // Uses Herons formula: http://www.mathopenref.com/heronsformula.html
            var halfPerimeter = (side1 + side2 + side3) / 2d;
            return Math.Sqrt(halfPerimeter * (halfPerimeter - side1) * (halfPerimeter - side2) * (halfPerimeter - side3));
        }

        private static double CalculateEquilateralTriangleArea(int side)
        {
            return (Math.Sqrt(3d) / 4d) * (side * side);
        }

        private static long Time(Action method, int repetitions = 100000000)
        {
            var clock = new Stopwatch();
            clock.Start();
            for (var i = 0; i < repetitions; i++)
            {
                method();
            }

            clock.Stop();
            return clock.ElapsedMilliseconds;
        }

        [Test]
        [TestCase(10, 43.301270189221931d)]
        [TestCase(100, 4330.1270189221932d)]
        public void TestAreaOfEquilateralTriangle(int input, double expected)
        {
            // Comparing doubles isn't a good idea, we should be checking the difference against an epsilon
            Assert.AreEqual(expected, CalculateEquilateralTriangleArea(input));
        }

        [Test]
        [TestCase(2, 3, 1.984313483298443d)]
        [TestCase(3, 2, 2.8284271247461903d)]
        [TestCase(6, 3, 8.7142125289666872d)]
        public void TestAreaOfIsocelesTriangle(int longSide, int shortSide, double expected)
        {
            // Comparing doubles isn't a good idea, we should be checking the difference against an epsilon
            Assert.AreEqual(expected, CalculateIsocelesTriangleArea(longSide, shortSide));
        }

        [Test]
        [TestCase(3, 4, 5, 6d)]
        [TestCase(6, 8, 10, 24d)]
        [TestCase(5, 12, 13, 30d)]
        public void TestAreaOfScaleneTriangle(int side1, int side2, int side3, double expected)
        {
            // Comparing doubles isn't a good idea, we should be checking the difference against an epsilon
            Assert.AreEqual(expected, CalculateScaleneTriangleArea(side1, side2, side3));
        }

        [Test]
        [Ignore("Run locally")]
        public void TestEquilateralTriangleSpecialisationIsFasterThanScalene()
        {
            var specialised = Time(() => CalculateEquilateralTriangleArea(1));
            var generalised = Time(() => CalculateScaleneTriangleArea(1, 1, 1));
            Assert.GreaterOrEqual(generalised, specialised);
        }

        [Test]
        [TestCase(1, 1, 2)]
        [TestCase(2, 2, 4)]
        [TestCase(3, 3, 6)]
        public void TestInvalidTriangle(int side1, int side2, int side3)
        {
            Assert.Throws<InvalidTriangleException>(() => CalculateTriangleArea(side1, side2, side3));
        }

        [Test]
        [Ignore("Run locally")]
        public void TestIsocelesTriangleSpecialisationIsFasterThanScalene()
        {
            var specialised = Time(() => CalculateIsocelesTriangleArea(6, 3));
            var general = Time(() => CalculateScaleneTriangleArea(6, 6, 3));
            Assert.GreaterOrEqual(general, specialised);
        }

        [Test]
        [TestCase(-1, 1, 1)]
        public void TestSidesWithNegativeLength(int side1, int side2, int side3)
        {
            Assert.Throws<InvalidTriangleException>(() => CalculateTriangleArea(side1, side2, side3));
        }

        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(0, 0, 1)]
        [TestCase(0, 1, 1)]
        public void TestSidesWithZeroLength(int side1, int side2, int side3)
        {
            Assert.Throws<InvalidTriangleException>(() => CalculateTriangleArea(side1, side2, side3));
        }

        [Test]
        [TestCase(10, 10, 10, 43.301270189221931d)]
        public void TestEquilateral(int side1, int side2, int side3, double expected)
        {
            // Comparing doubles isn't a good idea, we should be checking the difference against an epsilon
            Assert.AreEqual(expected, CalculateTriangleArea(side1, side2, side3));
        }

        [Test]
        [TestCase(2, 3, 3, 2.8284271247461903d)]
        [TestCase(3, 2, 3, 2.8284271247461903d)]
        [TestCase(3, 3, 2, 2.8284271247461903d)]
        [TestCase(2, 2, 3, 1.984313483298443d)]
        [TestCase(2, 3, 2, 1.984313483298443d)]
        [TestCase(3, 2, 2, 1.984313483298443d)]
        public void TestIsoceles(int side1, int side2, int side3, double expected)
        {
            // Comparing doubles isn't a good idea, we should be checking the difference against an epsilon
            Assert.AreEqual(expected, CalculateTriangleArea(side1, side2, side3));
        }

        [Test]
        [TestCase(3, 4, 5, 6d)]
        [TestCase(5, 4, 3, 6d)]
        [TestCase(3, 5, 4, 6d)]
        public void TestScalene(int side1, int side2, int side3, double expected)
        {
            // Comparing doubles isn't a good idea, we should be checking the difference against an epsilon
            Assert.AreEqual(expected, CalculateTriangleArea(side1, side2, side3));
        }
    }

    public class InvalidTriangleException : Exception
    {
        public InvalidTriangleException(string message) : base(message)
        {
        }
    }
}