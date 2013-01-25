using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleGen
{
    public class PointWrapper
    {
        private Point point;
        private Point X;
        private double m;
        private double errorMargin = 1e-12;
        public Point Point
        {
            get { return point; }
            set { point = value; }
        }

        private double mean;
        public double Mean
        {
            get { return mean; }
        }

        public PointWrapper(Point point, Point X, double m)
        {
            this.point = point;
            this.X = X;
            this.m = m;
        }

        public string PointValues
        {
            get
            {
                string output = "";
                for (int i = 0; i < point.Dimensions; i++)
                {
                    output += " p" + (i + 1) + ": " + point.p[i];
                }
                return output;
            }
        }

        public void calculateMean(Point X)
        {
            double result = 0;
            for (int i = 0; i < point.Dimensions; i++)
            {
                result += point.p[i] * X.p[i];
            }

            mean = result;
        }

        public double Sum
        {
            get
            {
                double sum = 0.0;
                foreach (double val in point.p)
                {
                    sum += val;
                }
                return sum;
            }
        }

        public bool ValuesInRange
        {
            get
            {
                foreach (double val in point.p)
                {
                    if (!(val > 0 && val < 1))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public bool IsValid
        {
            get
            {
                //Make some wider conditions
                bool IsSumValid = (Math.Abs(this.Sum - 1)<=errorMargin);
                //bool IsSumValid = (this.Sum == 1);
                this.calculateMean(this.X);
                bool IsMeanValid = (Math.Abs(this.Mean - m) <= errorMargin);
                bool IsAboveZero = ValuesInRange;
                return (IsSumValid && IsMeanValid && IsAboveZero);
            }
        }
    }
}
