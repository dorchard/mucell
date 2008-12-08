using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model
{

    /// <summary>
    /// 
    /// </summary>
    /// <owner>Jonathan</owner>
    public class TimeSeriesFunction
    {

        private AggregateEvaluationFunction function;

        /// <summary>
        /// Name of the function
        /// </summary>
        /// <param name="f">
        /// A <see cref="StateEvaluationFunction"/>
        /// </param>
        private string name;
        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Time interval when to record the data
        /// </summary>
        /// <param name="f">
        /// A <see cref="StateEvaluationFunction"/>
        /// </param>
        private double timeInterval;
        [XmlAttribute]
        public double TimeInterval
        {
            get { return timeInterval; }
            set { timeInterval = value; }
        }

        /// <summary>
        /// String that represents the function - must call InitialiseTimeSeriesFunction after setting this
        /// </summary>
        /// <param name="f">
        /// A <see cref="StateEvaluationFunction"/>
        /// </param>
        private string functionString;
        public string FunctionString
        {
            get { return functionString; }
            set { functionString = value; }
        }

        private string units = "No units";
        /// <summary>
        /// A string representing the units of the data held in the time series
        /// </summary>
        public string Units
        {
            get { return units; }
        }

        /// <summary>
        /// Initalizes the time series function - must be called after setting the function string
        /// </summary>
        /// <param name="models">
        /// A <see cref="List`1"/>
        /// </param>
        /// <param name="experiment">
        /// A <see cref="Experiment"/>
        /// </param>
        /// <param name="simulation">
        /// A <see cref="Simulation"/>
        /// </param>
        public void InitializeTimeSeriesFunction(List<Model.SBML.Model> models, Experiment experiment, Simulation simulation)
        {
        		if (this.functionString!=null)
        		{
        			// create a reader
        			MuCell.Model.SBML.Reader.SBMLReader reader = new MuCell.Model.SBML.Reader.SBMLReader();
        			// parse the formular
				SBML.FormulaParser fp = new SBML.FormulaParser(reader, this.functionString, models, experiment, simulation);
				// Get the formula tree
            		SBML.MathTree formulaTree = fp.getFormulaTree();
            		// Convert to a function
				this.function = formulaTree.ToAggregateEvaluationFunction();
                    units = formulaTree.ApproximateUnits();
			}
        }

        /// <summary>
        /// Applies the function to a state
        /// </summary>
        /// <param name="state">
        /// A <see cref="StateSnapshot"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.double"/>
        /// </returns>
        public double evaluateNext(StateSnapshot state)
        {
            if (this.function != null)
            {
                return this.function(state);
            }
            else
            {
                return 0.0d;
            }
        }

        public TimeSeriesFunction()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="timeInterval">
        /// A <see cref="System.Double"/>
        /// </param>
        public TimeSeriesFunction(string name, double timeInterval)
        {
            this.name = name;
            this.timeInterval = timeInterval;
            //Console.Write("timeserifn = " + this.timeInterval);
        }

        public bool exptEquals(TimeSeriesFunction other)
        {
            if (this.Name != other.Name)
            {
                Console.Write("TimeSeriesFunction objects not equal: ");
                Console.WriteLine("this.Name='" + this.Name + "'; other.Name='" + other.Name);
                return false;
            }
            if (this.TimeInterval != other.TimeInterval)
            {
                Console.Write("TimeSeriesFunction objects not equal: ");
                Console.WriteLine("this.TimeInterval=" + this.TimeInterval + "; other.TimeInterval=" + other.TimeInterval);
                return false;
            }
            if (this.FunctionString != other.FunctionString)
            {
                Console.Write("TimeSeriesFunction objects not equal: ");
                Console.WriteLine("this.FunctionString='" + this.FunctionString + "'; other.FunctionString='" + other.FunctionString);
                return false;
            }
            return true;
        }

    }
}
