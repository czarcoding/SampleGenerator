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

        public Point[] makePointsArray(int n, int sampleCount)
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

        //public Point generatePoint(int n)
       // {
            /*Point point = new Point(n);

            for (int i = 0; i < n; i++)
            {
                double value = randomGenerator.NextDouble();

                double rangeSubstract = 0;
                for (int j = 0; j < i; j++)
                {
                    rangeSubstract += point.p[j];
                }
                double maxRange = 1 - rangeSubstract;

                if (i == n - 1)
                {
                    point.p[i] = maxRange;
                }
                else
                {
                    point.p[i] = value * maxRange;
                }
            }*/
           // Point point = this.generateX(n);

            //return point;
            //Console.WriteLine(point.ToString());
            /*
            if (point.isAddingToOne())
            {
                Console.WriteLine("Point is valid, sum is 1");
                return point;
            }
            else
            {
                Console.WriteLine("Point NOT valid!");
                return null;
            }
             */

        //}
    }
}
