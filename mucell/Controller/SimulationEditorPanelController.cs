using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Controller
{
    public class SimulationEditorPanelController : IControllable<ApplicationController>
    {
        ApplicationController applicationController;
        ISimulationEditorPanelUI simulationEditorPanelUI;

        public SimulationEditorPanelController(ISimulationEditorPanelUI simulationEditorPanelUI)
        {
            this.simulationEditorPanelUI = simulationEditorPanelUI;
        }

        #region IControllable<ApplicationController> Members

        public void setController(ApplicationController controller)
        {
            applicationController = controller;
        }

        #endregion

        public void showParameters(Model.SimulationParameters simulationParameters)
        {
            simulationEditorPanelUI.setSimulationParameters(simulationParameters);
        }

        public void applyChanges(Model.SimulationParameters simulationParameters)
        {
            simulationEditorPanelUI.getSimulationParameters(simulationParameters);
        }

        public void refreshCurrentSimulationData()
        {
            applicationController.refreshCurrentSimulationData();
        }
    }
}
