using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model
{

   

    public class SimulationParameters
    {

       
        private bool changed;
        [XmlIgnore]
        public bool Changed
        {
            get { return changed; }
        }
        
        // StepTime
        private double stepTime;
        [XmlAttribute]
        public double StepTime
        {
        		get { return stepTime; }
            set { if (stepTime != value) { stepTime = value; changed = true; } }
        	}
        
        // SimulationLength
        private double simulationLength;
        [XmlAttribute]
        public double SimulationLength 
        {
        		get { return simulationLength; }
            set { if (simulationLength != value) { simulationLength = value; changed = true; } }
        }
        
        // SnapshotInterval time
        private double snapshotInterval;
        [XmlAttribute]
        public double SnapshotInterval
        {
        		get { return snapshotInterval; }
            set { if (snapshotInterval != value) { snapshotInterval = value; changed = true; } }
        }
        
        // InitialState
        private StateSnapshot initialState;
        public StateSnapshot InitialState
        {
        		get { return initialState; }
        		set { initialState = value; }
        }
        
        // TimeSeries
        private List<TimeSeries> timeSeries;
        public List<TimeSeries> TimeSeries
        {
        		get { return timeSeries; }
        		set { timeSeries = value; }
        }
        
        // The overall relative tolerance for ODE solving
        private double relativeTolerance;
        [XmlAttribute]
        public double RelativeTolerance
        {
        		get { return relativeTolerance; }
            set { if (relativeTolerance != value) { relativeTolerance = value; changed = true; } }
        }
        
        private Solver.SolverMethods solverMethod;
        [XmlAttribute]
        public Solver.SolverMethods SolverMethod
        {
        		get { return solverMethod; }
            set { if (solverMethod != value) { solverMethod = value; changed = true; } }
        }
        



        // the ode method to use if Cvode is selected
        // 0 - ADAMS_FUNCTIONAL
        // 1 - BDF_NEWTON
        private int cvodeType;
        [XmlAttribute]
        public int CvodeType
        {
        		 get { return cvodeType; }
        		 set { cvodeType = value; }
        }




        // set of parameters determining the view of the environment (eg zoom / offset / pointer position / selected group etc)
        private MuCell.View.OpenGL.SpatialViewState environmentViewState;
        [XmlIgnore]
        public MuCell.View.OpenGL.SpatialViewState EnvironmentViewState
        {
            get { return environmentViewState; }
            set { environmentViewState = value; }
        }

        private double timeSeriesStartOffset = 0;
        [XmlAttribute]
        public double TimeSeriesStartOfsset
        {
            get { return timeSeriesStartOffset; }
            set { timeSeriesStartOffset = value; }
        }

        public SimulationParameters()
        {
        	// Defaults
            this.environmentViewState = new MuCell.View.OpenGL.SpatialViewState();
            this.stepTime = 0.01d;
            this.simulationLength = 5d;
            this.snapshotInterval = 0d;
            this.InitialState = new StateSnapshot();
            this.timeSeries = new List<TimeSeries>();
            this.relativeTolerance = 1e-8d;
            this.solverMethod = Solver.SolverMethods.RungeKutta;
            this.cvodeType = 0;
        }
        public void resetChangedValue()
        {
            changed = false;
        }

        public bool exptEquals(SimulationParameters other)
        {
            if (this.StepTime != other.StepTime)
            {
                Console.Write("SimulationParameters objects not equal: ");
                Console.WriteLine("this.StepTime='" + this.StepTime + "'; other.StepTime='" + other.StepTime);
                return false;
            }
            if (this.SimulationLength != other.SimulationLength)
            {
                Console.Write("SimulationParameters objects not equal: ");
                Console.WriteLine("this.SimulationLength='" + this.SimulationLength + "'; other.SimulationLength='" + other.SimulationLength);
                return false;
            }
            if (this.SnapshotInterval != other.SnapshotInterval)
            {
                Console.Write("SimulationParameters objects not equal: ");
                Console.WriteLine("this.SnapshotInterval='" + this.SnapshotInterval + "'; other.SnapshotInterval='" + other.SnapshotInterval);
                return false;
            }
            if (this.RelativeTolerance != other.RelativeTolerance)
            {
                Console.Write("SimulationParameters objects not equal: ");
                Console.WriteLine("this.RelativeTolerance='" + this.RelativeTolerance + "'; other.RelativeTolerance='" + other.RelativeTolerance);
                return false;
            }
            if (this.CvodeType != other.CvodeType)
            {
                Console.Write("SimulationParameters objects not equal: ");
                Console.WriteLine("this.CvodeType='" + this.CvodeType + "'; other.CvodeType='" + other.CvodeType);
                return false;
            }
            if (this.SolverMethod != other.SolverMethod)
            {
                Console.Write("SimulationParameters objects not equal: ");
                Console.WriteLine("this.SolverMethod='" + this.SolverMethod + "'; other.SolverMethod='" + other.SolverMethod);
                return false;
            }

            if (this.InitialState.exptEquals(other.InitialState) == false)
            {
                Console.Write("SimulationParameters objects not equal: ");
                Console.WriteLine("this.InitialState != other.InitialState");
                return false;
            }
            try
            {
                for (int i = 0; i < this.TimeSeries.Count; i++)
                {
                    if (this.TimeSeries[i].exptEquals(other.TimeSeries[i]) == false)
                    {
                        Console.Write("SimulationParameters objects not equal: ");
                        Console.WriteLine("this.TimeSeries[" + i + "] != other.TimeSeries[" + i + "]");
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                // Array out of bounds; lists are unequal
                Console.Write("SimulationParameters objects not equal: ");
                Console.WriteLine("List lengths differed");
                return false;
            }
           
            return true;
        }
    }
}
