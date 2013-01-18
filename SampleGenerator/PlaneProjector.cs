using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SampleGen
{
    public class PlaneProjector
    {
        private RandomPointGenerator generator;
        private ObservableCollection<Point> samplePArray;

        public ObservableCollection<Point> SamplePArray
        {
            get { return samplePArray; }
            set { samplePArray = value; }
        }

        public PlaneProjector()
        {
            generator = new RandomPointGenerator();
        }

        public Point projectPoint(Point point, Point normal, double d) 
        {
            double t = (point.dotProduct(normal) - d) / normal.dotProduct(normal);

            Point result = point.subtract(normal.multiply(t));

            return result;

        }
        //public bool fitsHyperplane(double c, Point p)
        //{
        //    if (p.Mean == c)
        //    {
        //        Console.WriteLine(p.Mean.ToString(MainWindow.DOUBLE_FORMAT_STRING) + " == " +
        //            c.ToString(MainWindow.DOUBLE_FORMAT_STRING));
        //        return true;
        //    }
        //    else
        //    {
        //        Console.WriteLine(p.Mean.ToString(MainWindow.DOUBLE_FORMAT_STRING) + " != "
        //            + c.ToString(MainWindow.DOUBLE_FORMAT_STRING));
        //        return false;
        //    }
        //}
        public Point makePoint(Point X, double d, int dim)
        {
            if (generator != null)
            {
                Point p = generator.generatePoint(dim);

                Console.WriteLine("Point= " + p.ToString() + "\n");

                Point projection = this.projectPoint(p, X, d);

                Console.WriteLine("Projected= " + projection.ToString() + "\n");

                return projection;
            }
            else
            {
                throw new ArgumentNullException("generator", "Generator is null");
            }
            
        }

        //public bool isAddingToOne(Point p)
        //{
        //    double sum = p.Sum;

        //    Console.WriteLine("Sum= " + sum.ToString(MainWindow.DOUBLE_FORMAT_STRING) + "\n");

        //    return sum == 1.0;
        //}

        //TODO Verify point for console output
    }
}
