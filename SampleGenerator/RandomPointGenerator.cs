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
    }
}
