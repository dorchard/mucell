using System;
using MuCell;
using System.Collections.Generic;
using MuCell.Model;
using System.Threading;
using System.Windows.Forms;


namespace MuCell.Controller
{
    /// <summary>
    /// Singleton Simulator Component that runs the threaded simulations
    /// </summary>
    /// <owner>Duncan</owner>

    

    public class Simulator
    {
        protected class SimulatorThread
        {
            public Simulation simulation;
            public Thread runningThread;

            public SimulatorThread(Simulation simulation)
            {
                this.simulation = simulation;
            }
            public void run()
            {
                Console.WriteLine("Running simulation thread");
                Console.WriteLine("="+simulation.Parameters.SimulationLength);
                Console.WriteLine("=" + simulation.Parameters.SolverMethod);
                simulation.StartSimulation();
                Console.WriteLine("Simulation done");
                getSimulator().simulationFinished(this);
                Console.WriteLine("Cleaned up");
            }
        }

        private static Simulator singleton;
        private bool finishing;

        public static Simulator getSimulator()
        {
            if (singleton == null)
            {
                singleton = new Simulator();
            }
            return singleton;
        }


        private LinkedList<SimulatorThread> runningSimulations;

        public Simulator()
        {
            runningSimulations = new LinkedList<SimulatorThread>();
            finishing = false;
        }
        /// <summary>
        /// End all the running simulations, returning true if it will call back
        /// </summary>
        public bool stopAllSimulations()
        {
            lock (runningSimulations)
            {
                if (runningSimulations.Count > 0)
                {
                    finishing = true;
                    foreach (SimulatorThread s in runningSimulations)
                    {
                        Console.WriteLine("Ending simulation " + s);
                        s.simulation.EndSimulation();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        protected void simulationFinished(SimulatorThread simulation)
        {
            lock (runningSimulations)
            {
                runningSimulations.Remove(simulation);
                if (finishing && runningSimulations.Count == 0)
                {
                    //call for the program to exit
                    Application.Exit();
                }
            }
        }
        public void runThreadedSimulation(Simulation simulation, ISimulationListener listener)
        {
            lock (runningSimulations)
            {
                bool contains = false;
                foreach (SimulatorThread s in runningSimulations)
                {
                    if (s.simulation == simulation)
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    
                    simulation.setSimulationListener(listener);

                    //start a thread for the simulation and return
                    SimulatorThread threadTarget = new SimulatorThread(simulation);
                    Thread newThread = new Thread(new ThreadStart(threadTarget.run));
                    threadTarget.runningThread = newThread;
                    runningSimulations.AddLast(threadTarget);
                    newThread.Start();
                }
            }
        }
        public bool isSimulationRunning(Simulation simulation)
        {
            lock (runningSimulations)
            {
                foreach (SimulatorThread s in runningSimulations)
                {
                    if (s.simulation == simulation)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

    }
}
