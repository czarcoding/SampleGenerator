﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SampleGen
{
    class PointProcessor
    {
        double[][] mMatrix;
        double[][] nMatrix;

        public Point tryFitSum(Point point, Point X)
        {
            MatrixCalculator matrixCalculator = new MatrixCalculator();

            List<Point> pointList = matrixCalculator.prepareVectors(point);

            pointList = matrixCalculator.gramSchmidt(X, pointList);
            
            pointList.RemoveAt(0);

            mMatrix = matrixCalculator.createMatrix(point, X, pointList);
            nMatrix = matrixCalculator.createInverse(point, X, pointList);

            Point local = this.toLocal(point);
            Console.WriteLine("=== TO LOCAL: " + local.ToString());

            //solver and modyfying local
            double[,] m = Solver.prepareEquations(mMatrix);
            Solver.Solve(m);
           
            for (int i = 0; i < point.Dimensions - 1; i++)
            {
                local.p[i] = m[i,point.Dimensions-1];
            }

            Point global = this.toGlobal(local);
            Console.WriteLine("=== TO GLOBAL: " + global.ToString());
            return global;
        }

        public Point processPoint(Point p, Point X)
        {
            Console.WriteLine("\nProcessing\n");

            Point point = p;
            Console.WriteLine("Before: " + point.ToString());

            point = tryFitSum(point, X);

            return point;
           
        }

        private Point toLocal(Point p)
        {
            Point result = new Point(p.Dimensions);
            for (int i = 0; i < result.Dimensions; i++)
            {
                double r = 0;
                for (int j = 0; j < p.Dimensions; j++)
                {
                    r += nMatrix[i][j] * p.p[j];
                }
                r += nMatrix[i][p.Dimensions];
                result.p[i] = r;
            }
            return result;
        }

        private Point toGlobal(Point p)
        {
            Point result = new Point(p.Dimensions);
            for (int i = 0; i < result.Dimensions; i++)
            {
                double r = 0;
                for (int j = 0; j < p.Dimensions-1; j++)
                {
                    r += mMatrix[i][j] * p.p[j];
                }
                r += mMatrix[i][p.Dimensions];
                result.p[i] = r;
            }
            return result;
        }
    }
}
