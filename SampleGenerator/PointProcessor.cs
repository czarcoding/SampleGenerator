using System;
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
        public Point tryAnnealing(Point point, Point X)
        {
            MatrixCalculator matrixCalculator = new MatrixCalculator();

            List<Point> pointList = matrixCalculator.prepareVectors(point);

            pointList = matrixCalculator.gramSchmidt(X, pointList);

            pointList.RemoveAt(0);

            mMatrix = matrixCalculator.createMatrix(point, X, pointList);
            nMatrix = matrixCalculator.createInverse(point, X, pointList);

            //Point local = this.toLocal(point);
            //Console.WriteLine("=== TO LOCAL: " + local.ToString());

            //solver and modyfying local
            //double[,] m = Solver.prepareEquations(mMatrix);
            //Solver.Solve(m);

            //for (int i = 0; i < point.Dimensions - 1; i++)
            //{
            //    local.p[i] = m[i, point.Dimensions - 1];
            //}

            //Point global = this.toGlobal(local);
            //Console.WriteLine("=== TO GLOBAL: " + global.ToString());
            Point global = annealing(point);
            return global;
        }
        private Point makeK(Point p, Point vals)
        {
            double []t = new double[vals.Dimensions+2];
            for (int i = 0; i < vals.Dimensions; i++)
            {
                t[i] = vals.p[i];
            }
            double[] eq = makeMainEq(this.mMatrix);
            double lastT = getLastT(eq, vals.p);
            t[t.Length-2]=lastT;
            Point l = toLocal(p);

            for (int i = 0; i < p.Dimensions - 1; i++)
            {
                l.p[i] = t[i];
            }
            Point k = toGlobal(l);

            return k;
        }
        private double targetFunction(Point p, Point vals)
        {
            Point k = makeK(p, vals);
            double result = 0;

            for (int i = 0; i < p.Dimensions; i++)
            {
                result += Math.Abs(k.p[i]);
            }
            return result;
        }
        private double[] makeMainEq(double[][] mat)
        {
            int k = mat.Length;
            double[] elements = new double[k];

            for (int i = 0; i < k-1; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    elements[i] += mat[j][i];
                }
            }

            for (int j = 0; j < k; j++)
            {
                elements[k-1] += mat[j][k];
            }

            double[] elem = new double[elements.Length - 1];
            int resultIntex = elements.Length-2;

            for (int i = 0; i < elem.Length-1; i++)
            {
                elem[i] = -1*(elements[i] / elements[resultIntex]);
            }
            elem[elem.Length - 1] = (1 / elements[resultIntex])-(elements[elements.Length - 1] / elements[resultIntex]);


            return elem;
        }
        private double getLastT(double[] eq, double[] t){

            double result = 0;
            if (eq.Length -1 == t.Length)
            {
                for (int i = 0; i < t.Length; i++)
                {
                    result += eq[i] * t[i];
                }
                result += eq[eq.Length - 1];
            }
            return result;
        }
        public Point annealing(Point p)
        {
            Point result = null;
            RandomPointGenerator generator = new RandomPointGenerator();
            int maxIter = 10;
            int j = 0;
            Point values = generator.generatePoint(p.Dimensions-2);
            Point bestValues = generator.generatePoint(p.Dimensions - 2);
            double temperature = 500;
            double tempFactor = 0.90;
            double condition = 0;
            do
            {
                for (int i = 0; i < 1000; i++)
                {
                    Point testValues = generator.generatePoint(p.Dimensions - 2);

                    if (targetFunction(p, testValues) < targetFunction(p, bestValues))
                    {
                        bestValues = testValues;
                    }

                    double r = targetFunction(p, testValues) - targetFunction(p, values);
                    if (r < 0)
                    {
                        values = testValues;
                    }
                    else
                    {
                        double val = generator.generatePoint(1).p[0];
                        if (val < Math.Exp((-r) / temperature))
                        {
                            values = testValues;
                        }
                    }
                }
                Console.WriteLine("End of era");
                temperature = temperature * tempFactor;
                condition = targetFunction(p, bestValues);
                Console.WriteLine("Target value is: "+condition);
                j++;
            } while (condition>1&&j<maxIter);

            result = makeK(p,bestValues);
            Console.WriteLine(result.ToString());

            return result;
        }

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
                local.p[i] = m[i, point.Dimensions - 1];
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

            //point = tryFitSum(point, X);
            point = tryAnnealing(point, X);
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
