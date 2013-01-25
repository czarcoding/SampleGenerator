using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleGen
{
    class AnnealingSettingsProvider
    {
        private int maxIterations;
        private int innerIterations;
        private double initTemperature;
        private double coolingFactor;

        public int MaxAnnealingIterations
        {
            set { maxIterations = value; }
            get { return maxIterations; }
        }

        public int InnerAnnealingIterations
        {
            set { innerIterations = value; }
            get { return innerIterations; }
        }

        public double StartingTemperature
        {
            set { initTemperature = value; }
            get { return initTemperature; }
        }

        public double CoolingFactor
        {
            set { coolingFactor = value; }
            get { return coolingFactor; }
        }

        public override string ToString()
        {
            string result = "";

            result += "Max iterations = " + maxIterations + "\n";
            result += "Inner iterations = " + innerIterations + "\n";
            result += "Starting temperature = " + StartingTemperature + "\n";
            result += "Cooling factor = " + coolingFactor + "\n";

            return result;
        }

    }
}
