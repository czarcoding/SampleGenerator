using SampleGen;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SampleGenUnitTests
{
    
    [TestClass]
    public class SolverTest
    {
        /// <summary>
        ///A test for Solve
        ///</summary>
        [TestMethod]
        public void SolveTest()
        {
            double[,] M = new double[2, 3];
            
            M[0, 0] = 1.0;
            M[0, 1] = -1.923076923;
            M[0, 2] = -6.025641026;
            M[1, 0] = -0.520000000;
            M[1, 1] = 1.0;
            M[1, 2] = 3.133333333;

            bool expected = false;
            bool actual;
            actual = Solver.Solve(M);

            String message = "";
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for(int j = 0;j<M.GetLength(1); j++){
                    message += " , "+M[i,j].ToString();
                }
            }            
            Assert.AreEqual(expected, actual, message);
        }

        [TestMethod]
        public void prepareEquationsTest()
        {
            double [][]table = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                table[i] = new double[4];
            }

            table[0][0] = -0.131951986866700011;
            table[0][1] = 0.143945467277114019;
            table[0][2] = 1.233;
            table[0][3] = -0.0449748431849999950;

            table[1][0] = 0.984365639819553273;
            table[1][1] = -0.0222786702424330185;
            table[1][2] = 1.233;
            table[1][3] = -0.0692981071790000069;

            table[2][0] = -0.891751719998634229;
            table[2][1] = -0.0458919758855523830;
            table[2][2] = 1.233;
            table[2][3] = 0.879881165771999928;

            double [,] M = Solver.prepareEquations(table);

            String message = "";
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    message += " , " + M[i, j].ToString();
                }
                message += "|";
            }     

            //Assert.AreEqual(5, M.Length, message);

            bool res = Solver.Solve(M);
            message += "|solutions = ";
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    message += " , " + M[i, j].ToString();
                }
                message += "|";
            }

            Assert.AreEqual(true, res, message);

        }
    }
}
