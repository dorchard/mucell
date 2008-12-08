using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Controller
{

    /// <summary>
    /// Interface defining the methods the SimulationAnalyserPanelUI must provide
    /// the system controller with.
    /// </summary>
    /// <owner>Jonathan</owner>

    public struct SimulationControlState
    {
        public double progress;
        public bool isRunning;
    }

    public interface ISimulationAnalyserPanelUI : IControllable<SimulationAnalyserPanelController>
    {
        /// <summary>
        /// Adds a time series (containing uniformly sampled data against time)
        /// to the list of graphs to be drawn on the panel
        /// The panel will handle the location of the graph on the screen, depending
        /// on the order in which time series are passed with calls to this method.
        /// </summary>
        /// <param name="timeSeries">The additional graph to be drawn on the panel</param>

        void addTimeSeriesGraph(List<Model.TimeSeries> timeSeries);

        /// <summary>
        /// If a time series with the specified name is being drawn, remove it from the panel.
        /// </summary>
        /// <param name="name">The internal name of the time series,
        /// contained in the TimeSeriesParameters object</param>

        void removeTimeSeriesGraph(string name);

        /// <summary>
        /// Empty the panel of graphs
        /// </summary>
        void clearGraphs();


        /// <summary>
        /// Update the graphs so they render new data (thread safe)
        /// </summary>
        /// <param name="timeSeries"></param>
        void updateTimeSeriesGraph(List<MuCell.Model.TimeSeries> timeSeries);

        /// <summary>
        /// Sets the simulation control state (thread safe)
        /// </summary>
        /// <param name="progress"></param>
        void setSimulationControlState(SimulationControlState state);

        /// <summary>
        /// Returns the child Analyzer3DViewPanel UI
        /// </summary>
        /// <returns></returns>
        IAnalyzer3DViewPanelUI getAnalyzer3DViewPanelUI();
    }
}
