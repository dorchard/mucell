using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model
{
    public delegate double evaluationFunction(StateSnapshot snapshot);

    /// <summary>
    /// 
    /// </summary>
    /// <owner>Jonathan</owner>
    public class TimeSeriesParameters
    {
        private string name;
        public string Name { get { return name; } }

        double timeInterval;
        public double TimeInterval { get { return timeInterval; } }

        evaluationFunction thisEval;
        public evaluationFunction EvaluationFunction { get { return thisEval; } set { thisEval = value; } }

        public TimeSeriesParameters(string name, double timeInterval)
        {
            this.name = name;
            this.timeInterval = timeInterval;
        }
    }
}
