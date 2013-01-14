using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleGen
{
    /// <summary>
    /// Class representing a point in n-dimensional space.
    /// </summary>
    public class Point
    {
        public double[] p;
        private int dimensions;

        public int Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }
        public Point(int dim)
        {
            dimensions = dim;
            p = new double[dimensions];
        }

        public override string ToString()
        {
            string output = "";

            output += "\n[";
            for (int i = 0; i < dimensions; i++)
            {
                output += p[i];
                if (i < dimensions - 1)
                {
                    output += ", ";
                }
            }
            output += " ]";
            return output;
        }
        /// <summary>
        /// Method subtracts argument point from this point.
        /// </summary>
        /// <param name="arg">Point being subtrahend</param>
        /// <returns>Point being result of subtraction operation. 
        /// If dimensions of both points don't match, returns <code>null</code></returns>
        public Point subtract(Point arg)
        {
            if (this.dimensions == arg.dimensions)
            {
                Point result = new Point(this.dimensions);

                for (int i = 0; i < dimensions; i++)
                {
                    result.p[i] = this.p[i] - arg.p[i];
                }

                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Method adds argument point to this point.
        /// </summary>
        /// <param name="arg">Point being addition argument</param>
        /// <returns>Point being result of addition operation. 
        /// If dimensions of both points don't match, returns <code>null</code></returns>
        public Point add(Point arg)
        {
            if (this.dimensions == arg.dimensions)
            {
                Point result = new Point(this.dimensions);

                for (int i = 0; i < dimensions; i++)
                {
                    result.p[i] = this.p[i] + arg.p[i];
                }

                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Method multiplies this point by a given scalar number. 
        /// Each of the point coordinates is multiplied by argument
        /// </summary>
        /// <param name="arg">A scalar argument of multiplication</param>
        /// <returns>Point being result of multiplication.</returns>
        public Point multiply(double arg)
        {
            Point result = new Point(this.dimensions);

            for (int i = 0; i < this.dimensions; i++)
            {
                result.p[i] = this.p[i] * arg;
            }
            return result;
        }

        /// <summary>
        /// Methods performs dot product operation with this point and it's argument.
        /// </summary>
        /// <param name="arg">Argument of operation</param>
        /// <returns>A scalar value being result of operation.
        /// If dimensions of both points don't match, returns <code>null</code></returns>
        public double dotProduct(Point arg)
        {
            if (this.dimensions == arg.dimensions)
            {
                double result = 0;
                for (int i = 0; i < this.dimensions; i++)
                {
                    result += this.p[i] * arg.p[i];
                }
                return result;
            }
            else
            {
                return 0;
            }
        }      
    }
}
