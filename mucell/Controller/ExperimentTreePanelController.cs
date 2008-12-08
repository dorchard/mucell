using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MuCell.Controller
{
    /// <summary>
    /// Controller for the experiment panel
    /// </summary> 
    public class ExperimentTreePanelController : IControllable<ApplicationController>
    {
        ApplicationController applicationController;
        IExperimentTreePanelUI experimentTreePanelUI;

        public ExperimentTreePanelController(IExperimentTreePanelUI experimentTreePanelUI)
        {
            this.experimentTreePanelUI = experimentTreePanelUI;
        }
        public void refreshTreeElements(List<Model.IExperimentView> openExperiments)
        {
            this.experimentTreePanelUI.setOpenExperiments(openExperiments);
        }
        public void switchTab(int tab)
        {
            this.experimentTreePanelUI.switchTabs(tab);
        }
        public void setSelectedSimulation(string simulationName)
        {
            experimentTreePanelUI.setSelectedSimulation(simulationName);
        }
        public void setSelectedCellDefinition(string cellDefinitionName)
        {
            experimentTreePanelUI.setSelectedCellDefinition(cellDefinitionName);
        }


        /// <summary>
        /// Switches to the specified experiment
        /// </summary> 
        public void switchToExperiment(int ID)
        {
            applicationController.switchToExperiment(ID);
        }

        public void refreshCurrentSimulationData()
        {
            applicationController.refreshCurrentSimulationData();
        }

        /// <summary>
        /// Switches to the specified simulation in the
        /// current experiment
        /// </summary> 
        public void switchToSimulation(string name)
        {
            applicationController.switchToSimulation(name);
        }
        public bool renameExperiment(int id, String newName)
        {
            return applicationController.renameExperiment(id, newName);
        }
        public bool renameSimulation(int experimentID, String oldName, String newName)
        {
            return applicationController.renameSimulation(experimentID, oldName, newName);
        }
        public bool renameCellDefinition(int experimentID, String oldName, String newName)
        {
            return applicationController.renameCellDefinition(experimentID, oldName, newName);
        }
        public void deleteExperiment(int experimentID)
        {
            applicationController.deleteExperiment(experimentID);
        }
        public void deleteSimulation(int experimentID, String simulationName)
        {
            applicationController.deleteSimulation(experimentID, simulationName);
        }
        public void deleteCellDefinition(int experimentID, String cellDefinitionName)
        {
            applicationController.deleteCellDefinition(experimentID, cellDefinitionName);
        }

        /// <summary>
        /// Switches to the specified cell definition in the 
        /// current experiment
        /// </summary> 
        public void switchToCellDefinition(string name)
        {
            applicationController.switchToCellDefinition(name);
        }


        /// <summary>
        /// Adds a new simulation to the current experiment
        /// with default parameters
        /// </summary> 
        public void addNewSimulation()
        {

        }

        /// <summary>
        /// Adds a new cell definition to the current experiment
        /// </summary> 
        public void addNewCellDefinition()
        {

        }



        #region IControllable<ApplicationController> Members

        public void setController(ApplicationController controller)
        {
            applicationController = controller;
        }

        #endregion
    }
}
