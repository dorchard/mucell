using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using MuCell.Controller;

namespace MuCell.Model
{
    public class Simulation : ISimulationView
    {
        class SnapshotCache
        {
            private Queue<StateSnapshot> buffer;
            private StateSnapshot renderingFrame;
            private int maxCacheSize;

            public SnapshotCache(int maxCacheSize)
            {
                this.maxCacheSize = maxCacheSize;
                renderingFrame = null;
                buffer = new Queue<StateSnapshot>();
            }

            public void pushState(StateSnapshot currentStateClone)
            {
                lock (buffer)
                {
                    //to prevent buffer overrun
                    if (buffer.Count == maxCacheSize)
                    {
                        buffer.Dequeue();
                    }
                    buffer.Enqueue(currentStateClone);
                }
            }
            public StateSnapshot getStateToRender()
            {
                lock (buffer)
                {
                    //to prevent buffer underrun
                    if (buffer.Count > 0)
                    {
                        renderingFrame = buffer.Dequeue();
                    }
                    return renderingFrame;
                }
            }
        }
        // Name
        private string name;
        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        // SimulationResults
        private Results simulationResults;
        public Results SimulationResults
        {
            get { return simulationResults; }
            set { simulationResults = value; }
        }

        // Parameters
        private SimulationParameters parameters;
        public SimulationParameters Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        private bool runningStatus = false;
        private double stoppedTime = 0;
        private double time = 0;

        private StateSnapshot currentState;

        private ISimulationListener listener;

        // Random object used for random behaviours of components

        private System.Random randomObject;

        private SnapshotCache snapshotCache;

        /// <summary>
        /// Base constructor
        /// </summary>
        public Simulation()
        {
            int seed = Math.Abs((int)(DateTime.Now.Ticks));
            System.Console.WriteLine("Seeding simulation random object - seed = "+seed.ToString());
            this.randomObject = new Random(seed);
            this.snapshotCache = new SnapshotCache(10);
            this.currentState = null;
        }

        /// <summary>
        /// Constructor for the Simulator
        /// </summary>
        /// <param name="name">
        /// A <see cref="String"/>
        /// </param>
        public Simulation(String name)
        {
            this.name = name;
            this.parameters = new SimulationParameters();
            this.SimulationResults = new Results();
            this.listener = null;

            // Seeds the random object based on the time
            int seed = Math.Abs((int)(DateTime.Now.Ticks));
            System.Console.WriteLine("Seeding simulation random object - seed = "+seed.ToString());
            this.randomObject = new Random(seed);
            this.snapshotCache = new SnapshotCache(10);
            this.currentState = null;
        }

        /// <summary>
        /// Returns the current state of the simulation.  Not thread safe while simulating.
        /// </summary>
        /// <returns>
        /// A <see cref="StateSnapshot"/>
        /// </returns>
        public StateSnapshot GetCurrentState()
        {
            return currentState;
        }

        /// <summary>
        /// A thread safe method for returning buffered state snapshots generated by the simulation
        /// </summary>
        /// <returns></returns>
        public StateSnapshot getStateToRender()
        {
            return snapshotCache.getStateToRender();
        }

        /// <summary>
        /// Start the simulation running
        /// </summary>                        
        public void StartSimulation()
        {
            this.runningStatus = true;
            // Clone the initial state into the current state
            this.currentState = (StateSnapshot)this.parameters.InitialState.Clone();
            //this.currentState = this.parameters.InitialState;
            // init cell models
            this.InitializeCells();
            this.StartMainSimulationLoop(0);
        }
        public void setSimulationListener(ISimulationListener listener)
        {
            this.listener = listener;
        }

        /// <summary>
        /// Must be called at the end of a simulation to release the CVODE cell models
        /// </summary>
        public void EndSimulation()
        {
            this.runningStatus = false;
            listener = null;
            this.ReleaseCells();
        }

        /// <summary>
        /// Initialize the cell models
        /// </summary>
        private void InitializeCells()
        {
            // For all cells
            foreach (CellInstance cell in this.currentState.Cells)
            {
                cell.ResetCellConcentrations(this.parameters.InitialState);
                cell.InitializeCellModel(this.parameters.SolverMethod, this.parameters);
                // set the random object in the cell seeded by the simulations random object
                int random = this.randomObject.Next();
                System.Console.WriteLine("Generating new random with seed - "+random.ToString());
                cell.SetRandomObject(new Random(this.randomObject.Next(random)));
            }

        }

        private void ReleaseCells()
        {
            foreach (CellInstance cell in this.currentState.Cells)
            {
                cell.ReleaseCellModel();
            }
        }

        /// <summary>
        /// Starts the simulation main loop
        /// </summary>
        /// <param name="startTime">
        /// A <see cref="System.Double"/>
        /// </param>
        private void StartMainSimulationLoop(double startTime)
        {

            int[] timeSeriesIntervalMultipliers = new int[this.parameters.TimeSeries.Count];
            int i = 0;

            int mainCounter = 0;



            // *** NUTRIENT FIELDS INITIAL ***
            foreach (NutrientField field in this.currentState.SimulationEnvironment.GetNutrientObjects())
            {
                field.InitField(this.currentState.SimulationEnvironment.Boundary);
            }


            // *** COMPONENT PER CELL INITIAL ***
            foreach (CellInstance cell in this.currentState.Cells )
            {

                foreach(SBML.ExtracellularComponents.ComponentWorldStateBase componentStateBase in cell.Components)
                {
                    componentStateBase.InitializeInEnvironment(cell, this.currentState);
                }

            }


            // *** TIME SERIES INITIAL ***
            foreach (TimeSeries timeSeries in this.parameters.TimeSeries)
            {
                timeSeries.ClearTimeSeries();
                //timeSeries.evaluate(this.currentState);

                if (this.parameters.StepTime != 0)
                {
                    timeSeriesIntervalMultipliers[i] = (int)Math.Floor(timeSeries.parameters.TimeInterval / this.parameters.StepTime);
                }

                i++;
            }

            // *** TIME LOOP ***
            for (this.time = startTime; (this.time < this.parameters.SimulationLength && this.runningStatus); this.time += this.parameters.StepTime)
            {
                //System.Console.WriteLine(this.time);
                // *** MAIN COMPUTATION LOOP ***

                // For each cell instance in the environment, run the cell instances equations
                foreach (CellInstance cell in this.currentState.Cells)
                {
                    // Evaluate the cell over the current time step with the current state
                    cell.DoTimeStep(this.currentState, this.time, this.parameters.StepTime);

                    // Perform spatial calculations
                    if (cell.Components != null)
                    {
                        this.SpatialSimulator(cell, this.currentState, this.time, this.parameters.StepTime);
                    }
                }

                // *** NUTRIENT FIELDS ***
                foreach (NutrientField field in this.currentState.SimulationEnvironment.GetNutrientObjects())
                {
                    field.DoTimeStep(this.time, this.parameters.StepTime);
                }

                // If the time is at a snapshot interval then clone it and add to results list

                StateSnapshot currentStateClone = (StateSnapshot)this.currentState.Clone();
                snapshotCache.pushState(currentStateClone);
                if (this.time % this.parameters.SnapshotInterval == 0)
                {
                    this.simulationResults.StateSnapshots.Add(currentStateClone);
                }


                // increment the main counter
                mainCounter++;

                // *** TIME SERIES ***
                i = 0;
                if (time >= parameters.TimeSeriesStartOfsset)
                {
                    // [assumption1] That the ordering of the List of TimeSeries is constant
                    foreach (TimeSeries timeSeries in this.parameters.TimeSeries)
                    {
                        // If we have reached a multplier for the current time series
                        if (timeSeriesIntervalMultipliers[i] != 0 && (mainCounter % timeSeriesIntervalMultipliers[i]) == 0)
                        {
                            timeSeries.evaluate(this.currentState);
                        }
                        i++;
                    }
                }

                // *** DIRECT VALUE MODIFICATION FOR TESTER
                foreach (CellInstance cell in this.currentState.Cells)
                {
                    if (TestRigs.GlobalData.useHack)
                    {
                        cell.setSpeciesAmountInSimulation(TestRigs.GlobalData.hackSpeciesName, TestRigs.GlobalData.hackSpeciesValue);
                    }
                }

                

                // Run the listener call back
                if (listener != null)
                {
                    listener.simulationIterationComplete(this);
                }
            }
            // Set the stop time at the end of the iteration
            this.stoppedTime = this.time;
            this.runningStatus = false;
            if (listener != null)
            {
                listener.simulationStopped(this);
            }
        }

        /// <summary>
        /// Pause the simulation
        /// </summary>
        public void PauseSimulation()
        {
            this.runningStatus = false;
        }

        /// <summary>
        /// UnPause the simulation
        /// </summary>
        public void UnPauseSimulation()
        {
            this.runningStatus = true;
            // Restart the simulation at the point it was stopped
            this.StartMainSimulationLoop(this.stoppedTime);
        }
        public bool isRunning()
        {
            return runningStatus;
        }

        /// <summary>
        /// Returns a percentage value representing the progress of the simulation
        /// </summary>
        /// <returns>
        /// A <see cref="System.double"/>
        /// </returns>
        public double SimulationProgress()
        {
            return (this.time / this.Parameters.SimulationLength);
        }

        public static void SpatialSimulatorZZZ(CellInstance cell, StateSnapshot currentState, double time, double StepTime)
        {

        }

        /// <summary>
        /// Private method for performing the spatial simulation.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="currentState"></param>
        /// <param name="time"></param>
        /// <param name="StepTime"></param>
        private void SpatialSimulator(CellInstance cell, StateSnapshot currentState, double time, double StepTime)
        {
            Vector3 p = cell.CellInstanceSpatialContext.Position;
            Vector3 v = cell.CellInstanceSpatialContext.Velocity;
            Vector3 ang = cell.CellInstanceSpatialContext.Orientation;
            float visc = currentState.SimulationEnvironment.Viscosity;
            float h = (float)StepTime;

            //viscous drag (Stokes's drag)
            v.x += -visc * h * v.x * 1.5f;
            v.y += -visc * h * v.y * 1.5f;
            v.z += -visc * h * v.z * 1.5f;

            //cell collisions
            if (currentState.SimulationEnvironment.EnableCellCollisions)
            {
                foreach (CellInstance cellB in currentState.Cells)
                {
                    if (cell != cellB) //avoid collision with self
                    {
                        //the vector from the centre of this cell to cellB
                        Vector3 dist = new Vector3(p.x - cellB.CellInstanceSpatialContext.Position.x,
                                                   p.y - cellB.CellInstanceSpatialContext.Position.y,
                                                   p.z - cellB.CellInstanceSpatialContext.Position.z);
                        float distMag = dist.magnitude();

                       
                        if (distMag < cell.CellInstanceSpatialContext.Radius*0.001 + cellB.CellInstanceSpatialContext.Radius*0.001)
                        {
                            /*
                             * Collision occured: accelerate this cell away from the centre of the cell it is colliding with:
                             */

                            dist.unitVect(); //normalize
                            dist.x *= -h*0.1f;
                            dist.y *= -h*0.1f;
                            dist.z *= -h*0.1f;

                            //accelerate this cell and the one it collided with in oppose directions:
                            v.x -= dist.x;
                            v.y -= dist.y;
                            v.z -= dist.z;
                            cellB.CellInstanceSpatialContext.Accelerate(dist);

                        }

                             
                    }
                }
                
            }
 


            //positional change
            p.x += v.x * h;
            p.y += v.y * h;
            p.z += v.z * h;


            //check that boundary conditions are maintained (most of the time this will be true, and will be the only test needed)
            if (!currentState.SimulationEnvironment.Boundary.InsideBoundary(p))
            {
                /*
                 * If the cell is not inside the environment boundary, we move it back
                 * and attempt to move the cell by individual components (this allows 
                 * the cell to "slide" along the edges of the boundary without 
                 * breaking the boundary conditions). In most cases these three boundary
                 * checks should not be necessary
                 */

                //begin by checking x component, so move back y and z components
                p.y -= v.y * h;
                p.z -= v.z * h;
                if (!currentState.SimulationEnvironment.Boundary.InsideBoundary(p))
                {
                    //x failed, so move back x
                    p.x -= v.x * h;
                }

                //check y component
                p.y += v.y * h;
                if (!currentState.SimulationEnvironment.Boundary.InsideBoundary(p))
                {
                    //y failed
                    p.y -= v.y * h;
                }

                //check z component
                p.z += v.z * h;
                if (!currentState.SimulationEnvironment.Boundary.InsideBoundary(p))
                {
                    //z failed
                    p.z -= v.z * h;
                }

            }





            cell.CellInstanceSpatialContext.Position = p;
            cell.CellInstanceSpatialContext.Velocity = v;
            cell.CellInstanceSpatialContext.Orientation = ang;


        }

        public bool exptEquals(Simulation other)
        {
            if (this.Name != other.Name)
            {
                Console.Write("Simulation objects not equal: ");
                Console.WriteLine("this.Name='" + this.Name + "'; other.Name='" + other.Name);
                return false;
            }
            if (this.SimulationResults.exptEquals(other.SimulationResults) == false)
            {
                Console.Write("Simulation objects not equal: ");
                Console.WriteLine("this.SimulationResults != other.SimulationResults");
                return false;
            }
            if (this.Parameters.exptEquals(other.Parameters) == false)
            {
                Console.Write("Simulation objects not equal: ");
                Console.WriteLine("this.Parameters != other.Parameters");
                return false;
            }
            return true;
        }
    }
}
