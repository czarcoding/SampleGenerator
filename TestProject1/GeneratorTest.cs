using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleGen;

namespace SampleGenUnitTests
{
    [TestClass]
    public class GeneratorTest
    {
        [TestMethod]
        public void generatePointTest()
        {
            RandomPointGenerator gen = new RandomPointGenerator();
            PlaneProjector proj = new PlaneProjector();

            Point p = gen.generatePoint(3);

            //Assert.IsTrue(proj.isAddingToOne(p));
        }
    }
}
