using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleGen;

namespace SampleGenUnitTests
{
    [TestClass]
    public class PointTest
    {
        [TestMethod]
        public void pointSubstractTest()
        {
            double a1 = 4.0;
            double a2 = 3.5;

            double b1 = 2.0;
            double b2 = 4.0;

            Point a = new Point(2);

            a.p[0] = a1;
            a.p[1] = a2;

            Point b = new Point(2);
            b.p[0] = b1;
            b.p[1] = b2;

            Point result = a.subtract(b);

            Assert.AreEqual(a1 - b1, result.p[0]);
            Assert.AreEqual(a2 - b2, result.p[1]);
        }
        [TestMethod]
        public void pointDotProductTest()
        {
            double expected = 3;

            double a1 = 1.0;
            double a2 = 3.0;
            double a3 = -5.0;

            double b1 = 4.0;
            double b2 = -2.0;
            double b3 = -1.0;

            Point a = new Point(3);
            a.p[0] = a1;
            a.p[1] = a2;
            a.p[2] = a3;

            Point b = new Point(3);
            b.p[0] = b1;
            b.p[1] = b2;
            b.p[2] = b3;

            Assert.AreEqual(expected, a.dotProduct(b));

        }
    }
}
