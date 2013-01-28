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

        private AnnealingSettingsProvider annealingSettings;

        internal AnnealingSettingsProvider AnnealingSettings
        {
            get { return annealingSettings; }
            set
            {
                if (value != null)
                {
                    annealingSettings = value;
                }
            }
        }

        public PointProcessor()
        {
            annealingSettings = new AnnealingSettingsProvider();
            annealingSettings.MaxAnnealingIterations = 100;
            annealingSettings.InnerAnnealingIterations = 1000;
            annealingSettings.StartingTemperature = 10;
            annealingSettings.CoolingFactor = 0.90;
        }

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
            int maxIter = annealingSettings.MaxAnnealingIterations;
            int j = 0;
            Point values = generator.generateTValues(p.Dimensions-2);
            Point bestValues = generator.generateTValues(p.Dimensions - 2);
            Point last = values;
            double temperature = annealingSettings.StartingTemperature;
            double tempFactor = annealingSettings.CoolingFactor;
            double innerIterations = annealingSettings.InnerAnnealingIterations;
            double condition = 0;

      //      double avgCost = annealingSettings.StartingTemperature;
    //        double P1 = 0.95;

  //          double G = annealingSettings.MaxAnnealingIterations-annealingSettings.InnerAnnealingIterations;

//            double temperature = (-avgCost)/(Math.Log(P1));

            //double P2 = annealingSettings.CoolingFactor;


            //double tempFactor = Math.Pow((-avgCost/(temperature * Math.Log(P2))),(1.0 / (G*innerIterations)));
            
            do
            {
                for (int i = 0; i < innerIterations; i++)
                {
                    //Point testValues = generator.generateTValues(p.Dimensions - 2);
                    Point testValues = generator.generateClosePoint(values, temperature);
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
                        double val = generator.generateDouble();
                        double threshold = Math.Exp((-r) / temperature);
                        if (val < threshold)
                        {
                            values = testValues;
                        }
                    }
                }
               // if ((Math.Abs(targetFunction(p, values) - targetFunction(p, last))) <= 0.000001)
                //{
                 //   break;
                //}
                last = values;
                Console.WriteLine("End of era");

                    temperature = temperature * tempFactor;

                Console.WriteLine(temperature);
                condition = targetFunction(p, values);
                Console.WriteLine("Target value is: "+condition);
                if (condition <= 1)
                {
                    break;
                }
                j++;
            } while (j<maxIter);

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
