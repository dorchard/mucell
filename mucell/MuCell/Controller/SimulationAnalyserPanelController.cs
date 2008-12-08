using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model;

namespace MuCell.Controller
{

    /// <summary>
    /// Provides methods for the events triggered in the SimulationAnalyserPanelUI to call
    /// and passes data to be drawn as graphs
    /// </summary>
    /// <owner>Jonathan</owner>

    public class SimulationAnalyserPanelController : IControllable<ApplicationController>, ISimulationListener
    {
        ApplicationController applicationController;
        ISimulationAnalyserPanelUI simulationAnalyserPanelUI;

        Simulation currentSimulation;

        public SimulationAnalyserPanelController(ISimulationAnalyserPanelUI simulationAnalyserPanelUI)
        {
            this.simulationAnalyserPanelUI = simulationAnalyserPanelUI;

            List<TimeSeries> allTimeSeries = new List<TimeSeries>();
            // test code
            Model.TimeSeries ts = new MuCell.Model.TimeSeries("Concentration of X", 1);
            for (int i = 0; i < 40; i++)
            {
                ts.addDataPoint(Math.Sin(i));
            }
            allTimeSeries.Add(ts);

            Model.TimeSeries ts2 = new MuCell.Model.TimeSeries("Concentration of Y", 4);
            for (int i = 0; i < 40; i++)
            {
                ts2.addDataPoint(Math.Cos((i / 36.0f) * Math.PI * 2) * 3);
            }
            allTimeSeries.Add(ts2);

            Model.TimeSeries ts3 = new MuCell.Model.TimeSeries("Concentration of Z", 4);
            for (int i = 0; i < 10; i++)
            {
                ts3.addDataPoint(Math.Cos((i / 18.0f) * Math.PI * 2) * 2);
            }

            allTimeSeries.Add(ts3);

            simulationAnalyserPanelUI.addTimeSeriesGraph(allTimeSeries);
        }


        /// <summary>
        /// Find out all the time series that exist in the current results set, so that they can
        /// then be requested by name by the UI
        /// </summary>
        /// <returns>A list of names of time series that exist in the current results set</returns>

        List<string> getAvailableTimeSeries()
        {
            return null;

        }
        /// <summary>
        /// Make a request for a specific time series to be added to the list of graphs
        /// to be drawn - this calls addTimeSeriesGraph() on the UI to push the data back through
        /// </summary>
        /// <param name="name">The name of the time series, as returned by getAvailableTimeSeries()</param>

        void requestTimeSeries(string name){}

        public void showSimulation(Simulation simulation)
        {
            currentSimulation = simulation;
            updateSimulationControlState();

            List<TimeSeries> allTimeSeries = new List<TimeSeries>();

            foreach (TimeSeries timeSeries in currentSimulation.Parameters.TimeSeries)
            {
                Console.WriteLine("count="+timeSeries.Series.Count);
                allTimeSeries.Add(timeSeries);
            }

            simulationAnalyserPanelUI.clearGraphs();
            simulationAnalyserPanelUI.addTimeSeriesGraph(allTimeSeries);
        }
        public void updateSimulationControlState()
        {
            if (currentSimulation != null)
            {
                SimulationControlState controlState = new SimulationControlState();
                controlState.isRunning = currentSimulation.isRunning();
                controlState.progress = currentSimulation.SimulationProgress();

                

                simulationAnalyserPanelUI.setSimulationControlState(controlState);

                simulationAnalyserPanelUI.updateTimeSeriesGraph(null);

                /*List<TimeSeries> allTimeSeries = new List<TimeSeries>();
                foreach (TimeSeries timeSeries in currentSimulation.Parameters.TimeSeries)
                {
                    Console.WriteLine("count=" + timeSeries.Series.Count);
                    if (timeSeries.Series.Count != 0)
                    {
                        allTimeSeries.Add(timeSeries);
                    }
                }

                simulationAnalyserPanelUI.updateTimeSeriesGraph(allTimeSeries);*/
                
            }
        }
        public void tryRunningSimulation()
        {
            if (currentSimulation != null)
            {
                if (!Simulator.getSimulator().isSimulationRunning(currentSimulation))
                {
                    Simulator.getSimulator().runThreadedSimulation(currentSimulation, this);
                    updateSimulationControlState();
                    List<TimeSeries> allTimeSeries = new List<TimeSeries>();

                    foreach (TimeSeries timeSeries in currentSimulation.Parameters.TimeSeries)
                    {
                        Console.WriteLine("count=" + timeSeries.Series.Count);
                        allTimeSeries.Add(timeSeries);
                    }

                    simulationAnalyserPanelUI.updateTimeSeriesGraph(allTimeSeries);
                }
                else
                {
                    Console.WriteLine("Simulation is running already");
                }
            }
        }
        public void tryStoppingSimulation()
        {
            if (currentSimulation != null)
            {
                if (Simulator.getSimulator().isSimulationRunning(currentSimulation))
                {
                    currentSimulation.PauseSimulation();
                    updateSimulationControlState();
                }
                else
                {
                    updateSimulationControlState();
                }
            }
        }





        #region IControllable<ApplicationController> Members

        public void setController(ApplicationController controller)
        {
            applicationController = controller;
        }

        #endregion





        #region ISimulationListener Members

        public void simulationIterationComplete(ISimulationView simulation)
        {
            updateSimulationControlState();
        }
        public void simulationStopped(ISimulationView simulation)
        {
            updateSimulationControlState();
        }

        #endregion
    }
}
