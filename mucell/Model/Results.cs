using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model
{
    public class Results
    {
        // Path to datafile of results
        private string filePath;
        [XmlAttribute]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        // List of snapshots
        private List<StateSnapshot> stateSnapshots;
        public List<StateSnapshot> StateSnapshots
        {
            get { return stateSnapshots; }
            set { stateSnapshots = value; }
        }

        // Current state
        private StateSnapshot currentState;
        public StateSnapshot CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        // List of time series
        private List<TimeSeries> timeSeries;
        [XmlArray]
        [XmlArrayItem("series")]
        public List<TimeSeries> TimeSeries
        {
            get { return timeSeries; }
            set { timeSeries = value; }
        }

        public Results()
        {
            this.timeSeries = new List<TimeSeries>();
            this.stateSnapshots = new List<StateSnapshot>();
            this.currentState = new StateSnapshot();
        }

        public bool exptEquals(Results other)
        {
            if (this.FilePath != other.FilePath)
            {
                Console.Write("Results objects not equal: ");
                Console.WriteLine("this.FilePath='" + this.FilePath + "'; other.FilePath='" + other.FilePath);
                return false;
            }
            if (this.CurrentState.exptEquals(other.CurrentState) == false)
            {
                Console.Write("Results objects not equal: ");
                Console.WriteLine("this.CurrentState != other.CurrentState");
                return false;
            }
            try
            {
                for (int i = 0; i < this.StateSnapshots.Count; i++)
                {
                    if (this.StateSnapshots[i].exptEquals(other.StateSnapshots[i]) == false)
                    {
                        Console.Write("Results objects not equal: ");
                        Console.WriteLine("this.StateSnapshots[" + i + "] != other.StateSnapshots[" + i + "]");
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Results objects not equal: ");
                Console.Write("this.StateSnapshots.Count = " + this.StateSnapshots.Count);
                Console.WriteLine("; other.StateSnapshots.Count = " + other.StateSnapshots.Count);
                return false;
            }
            try 
            {
                for (int i = 0; i < this.TimeSeries.Count; i++)
                {
                    if (this.TimeSeries[i].exptEquals(other.TimeSeries[i]) == false)
                    {
                        Console.Write("Results objects not equal: ");
                        Console.WriteLine("this.TimeSeries[" + i + "] != other.TimeSeries[" + i + "]");
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                // Array out of bounds; lists are unequal
                Console.Write("Results objects not equal: ");
                Console.Write("this.TimeSeries.Count = " + this.TimeSeries.Count);
                Console.WriteLine("; other.TimeSeries.Count = " + other.TimeSeries.Count);

                Console.WriteLine("\n" + e.InnerException);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
                return false;
            }
            return true;
        }
    }
}