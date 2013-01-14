using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleGen
{
    class MatrixCalculator
    {
        public List<Point> prepareVectors(Point p)
        {
            RandomPointGenerator gen = new RandomPointGenerator();

            List<Point> pointList = new List<Point>();

            for (int i = 0; i < p.Dimensions-1; i++)
            {
              pointList.Add(gen.generatePoint(p.Dimensions));
            }

            for (int i =0; i< pointList.Count;i++)
            {
                Point point = pointList.ElementAt(i);
                pointList[i] = p.subtract(point);
            }

            return pointList;
        }

        private Point projection(Point u, Point v)
        {
            Point result = null;

            result = u.multiply(u.dotProduct(v)/u.dotProduct(u));

            return result;
        }

        public List<Point> gramSchmidt(List<Point> pointList)
        {
            for (int i = 0; i < pointList.Count; i++)
            {
                //normalize?

                for (int j = i + 1; j < pointList.Count; j++)
                {
                    Point u = pointList.ElementAt(i);
                    Point v = pointList.ElementAt(j);
                    pointList[j] = v.subtract(projection(u, v));
                }
            }

            return pointList;
        }

        public List<Point> gramSchmidt(Point x, List<Point> pointList)
        {
            List<Point> fullList = new List<Point>();
            fullList.Add(x);
            fullList.AddRange(pointList);

            return gramSchmidt(fullList);
        }

        public double[][] createMatrix(Point p, Point x, List<Point> pointList)
        {
            int n = p.Dimensions;
            int k = pointList.Count;
   
            double [][] matrix = new double[n][];
            for (int i = 0; i < n; i++)
            {
                matrix[i] = new double[k+2];

                for (int j = 0; j < k; j++)
                {
                    matrix[i][j] = pointList[j].p[i];
                }

                matrix[i][k] = x.p[i];
                matrix[i][k+1] = p.p[i];
            }

            return matrix;

        }

        public double[][] createInverse(Point p, Point x, List<Point> pointList)
        {
            int n = pointList.Count+1;
            int k = p.Dimensions;

            pointList.Add(x);

            double[][] inverse = new double[n][];
            for (int i = 0; i < n; i++)
            {
                inverse[i] = new double[k+1];

                for (int j = 0; j < k; j++)
                {
                    inverse[i][j] = pointList[i].p[j];
                }

                inverse[i][k] = -1*pointList[i].dotProduct(p);
            }

            return inverse;
        }
        
    }
}
