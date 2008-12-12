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
    public class TimeSeries
    {
        public TimeSeriesFunction parameters;
        [XmlElement]
        public TimeSeriesFunction Parameters { get { return parameters; } }

        /// <summary>
        /// Stores the data for the series
        /// </summary>
        private List<double> series;
        [XmlArray("data")]
        [XmlArrayItem("dp")]
        public List<double> Series
        {
            get { return series; }
        }

        /// <summary>
        /// Evaluates the time series to add the next data point
        /// </summary>
        /// <param name="state">
        /// A <see cref="StateSnapshot"/>
        /// </param>
        public void evaluate(StateSnapshot state)
        {
            // Evaluate the inner function with the state and add to the series
            addDataPoint(parameters.evaluateNext(state));
        }

        private double maxValue;
        [XmlIgnore]
        public double MaxValue
        {
            get { return maxValue; }
        }

        private double minValue;
        [XmlIgnore]
        public double MinValue
        {
            get { return minValue; }
        }

        /// <summary>
        /// Primary constructor
        /// </summary>
        public TimeSeries()
        {
            this.series = new List<double>();
        }

        /// <summary>
        /// Constructor with a name and time interval
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="timeInterval">
        /// A <see cref="System.Double"/>
        /// </param>
        public TimeSeries(string name, double timeInterval)
        {
            parameters = new TimeSeriesFunction(name, timeInterval);
            series = new List<double>();
            maxValue = double.MinValue;
            minValue = double.MaxValue;
        }

        /// <summary>
        /// Different constructor that fills in the equation into the TimeSeries parameters
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="equation">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="timeInterval">
        /// A <see cref="System.Double"/>
        /// </param>
        public TimeSeries(string name, string equation, double timeInterval)
        {
            parameters = new TimeSeriesFunction(name, timeInterval);
            parameters.FunctionString = equation;
            series = new List<double>();
            maxValue = double.MinValue;
            minValue = double.MaxValue;
        }

        public void ClearTimeSeries()
        {
            maxValue = double.MinValue;
            minValue = double.MaxValue;
            this.Series.Clear();
        }

        /// <summary>
        /// Initialize 
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
        public void Initialize(List<Model.SBML.Model> models, Experiment experiment, Simulation simulation)
        {
            if (this.parameters != null)
            {
                this.parameters.InitializeTimeSeriesFunction(models, experiment, simulation);
            }
        }

        /// <summary>
        /// Add a double data point to the time series, also remembers the max and min values
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.Decimal"/>
        /// </param>
        public void addDataPoint(decimal data)
        {
            this.addDataPoint((double)data);
        }

        /// <summary>
        /// Add a data point to the time series, also remembers the max and min values
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.Double"/>
        /// </param>
        public void addDataPoint(double data)
        {
            series.Add(data);
            if (data > maxValue)
            {
                maxValue = data;
            }
            if (data < minValue)
            {
                minValue = data;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", parameters.Name, parameters.FunctionString);
        }

        public bool exptEquals(TimeSeries other)
        {
            //if (this.MaxValue != other.MaxValue)
            //{
            //    Console.Write("TimeSeries objects not equal: ");
            //    Console.WriteLine("this.MaxValue=" + this.MaxValue + "; other.MaxValue=" + other.MaxValue);
            //    return false;
            //}

            //if (this.MinValue != other.MinValue)
            //{
            //    Console.Write("TimeSeries objects not equal: ");
            //    Console.WriteLine("this.MinValue=" + this.MinValue + "; other.MinValue=" + other.MinValue);
            //    return false;
            //}
            if (this.Parameters.exptEquals(other.Parameters) == false)
            {
                Console.Write("TimeSeries objects not equal: ");
                Console.WriteLine("this.Parameters != other.Parameters");
                return false;
            }
            try
            {
                for (int i = 0; i < this.Series.Count; i++)
                {
                    if (this.Series[i] != other.Series[i])
                    {
                        Console.Write("TimeSeries objects not equal: ");
                        Console.Write("this.Series[" + i + "]=" + this.Series[i]);
                        Console.WriteLine("; other.Series[" + i + "]" + other.Series[i]);
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                // Array out of bounds; lists are unequal
                Console.Write("TimeSeries objects not equal: ");
                Console.WriteLine("List lengths differed");
                return false;
            }

            return true;
        }
    }
}
