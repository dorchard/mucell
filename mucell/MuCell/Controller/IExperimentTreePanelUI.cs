using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Controller
{

    /// <summary>
    /// Interface defining the methods the ExperimentTreePanelUI must provide
    /// the system controller with. 
    /// </summary> 
    public interface IExperimentTreePanelUI : IControllable<ExperimentTreePanelController>
    {

        /// <summary>
        /// Creates the simulation editor child panel
        /// </summary> 
        ISimulationEditorPanelUI getSimulationEditorPanelUI();

        /// <summary>
        /// Creates the simulation analyser child panel
        /// </summary> 
         ISimulationAnalyserPanelUI getSimulationAnalyserPanelUI();

        /// <summary>
        /// Creates the simulation analyser child panel
        /// </summary> 
         ICellDefinitionsPanelUI getCellDefinitionsPanelUI();

        /// <summary>
        /// Creates the spatial configuration child panel
        /// </summary>
        /// <returns></returns>
        ISpatialConfigurationPanelUI getSpatialConfigurationPanelUI();

        /// <summary>
        /// Creates the time series editor child panel
        /// </summary>
        /// <returns></returns>
        ITimeSeriesEditorPanelUI getTimeSeriesEditorPanelUI();


        /// <summary>
        /// Configures UI to display the given list of open experiments and 
        /// their child simulations in the tree exploration view
        /// </summary> 
        void setOpenExperiments(List<Model.IExperimentView> openExperiments);


        /// <summary>
        /// Configures UI to display the experiment given by its ID
        /// as the currently selected experiment
        /// </summary> 
        void setSelectedExperiment(int experimentID);

        /// <summary>
        /// Configures UI to display the simulation given by its name
        /// as the currently selected simulation in the simulation
        /// view
        /// </summary> 
        void setSelectedSimulation(string simulationName);


        /// <summary>
        /// Configures UI to display the cell definition given by its name
        /// as the currently selected cell definition in the cell definitions
        /// view
        /// </summary> 
        void setSelectedCellDefinition(string cellDefinitionName);


        /// <summary>
        /// Display Simulation View
        /// </summary> 
        void showSimulationView();

        /// <summary>
        /// Display Cell Definitions View
        /// </summary> 
        void showCellDefinitionsView();
        void switchTabs(int tab);

    }
}
