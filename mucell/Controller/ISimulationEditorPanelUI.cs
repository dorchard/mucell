using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Controller
{

  

    /// <summary>
    /// Interface defining the methods the SimulationEditorPanelUI must provide
    /// the system controller with.
    /// </summary> 
    public interface ISimulationEditorPanelUI : IControllable<SimulationEditorPanelController>

    {
        /*
         * note: will probably decompose these into individual simulation parameters, so
         * that the UI need only store base types such as int/String instead of an entire
         * SimulationParameters object.
         * 
         */

        /// <summary>
        /// Configures UI to display the provided simulation parameters
        /// </summary> 
        void setSimulationParameters(Model.SimulationParameters simulationParameters);

        /// <summary>
        /// Pulls the simulation parameters from the UI.
        /// </summary> 
        void getSimulationParameters(Model.SimulationParameters simulationParameters);

    }
}
