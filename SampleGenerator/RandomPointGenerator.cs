using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleGen
{
    public class RandomPointGenerator
    {
        int n = 3;
        int sampleCount = 0;
        Random randomGenerator = new Random();

        public Point[] generatePointsArray(int n, int sampleCount)
        {
            Point[] points = new Point[sampleCount];

            this.n = n;
            Console.Write("\n");
            Console.WriteLine("n = " + n);
            this.sampleCount = sampleCount;
            for (int i = 0; i < sampleCount; i++)
            {
                Console.WriteLine("\nPoint no "+ (i+1));
                points[i] =  generatePoint(this.n);
            }
            return points;
            
        }

        public Point generatePoint(int n)
        {
            Point point = new Point(n);

            for (int i = 0; i < n; i++)
            {
                double value = randomGenerator.NextDouble();
                point.p[i] = value;
            }
            return point;
        }

        public Point generateTValues(int n)
        {
            Point point = new Point(n);
            double range = 10;

            for (int i = 0; i < n; i++)
            {
                double value = randomGenerator.NextDouble();
                point.p[i] = (value*range)-range/2;
            }
            return point;
            
        }
        public double generateDouble()
        {
            return randomGenerator.NextDouble();
        }
        public Point generateClosePoint(Point values, double temp)
        {
            Point point = new Point(values.Dimensions);
            double range = 5*temp;
            double max=range/2;
            double min=-(range/2);

            for (int i = 0; i < values.Dimensions; i++)
            {
                double value = randomGenerator.NextDouble()*(max-min)+min;
                point.p[i] = values.p[i] + value;
            }
            return point;
        }
    }
}
