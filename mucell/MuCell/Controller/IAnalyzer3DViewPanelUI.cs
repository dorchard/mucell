using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model;

namespace MuCell.Controller
{
    /// <summary>
    /// Interface defining the methods the Analyzer3DViewPanelUI must provide
    /// the system controller with.
    /// </summary>
    public interface IAnalyzer3DViewPanelUI : IControllable<Analyzer3DViewPanelController>
    {
        /// <summary>
        /// Registers a timer tick
        /// </summary>
        void timer1Tick();

        /// <summary>
        /// sets which simulation is being viewed
        /// </summary>
        /// <param name="sim"></param>
        void setSimulation(Simulation sim);
    }
    
}
