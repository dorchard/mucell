using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model;

namespace MuCell.Controller
{

    /// <summary>
    /// Controller class for the Analyzer 3D view
    /// </summary>
    public class Analyzer3DViewPanelController : IControllable<ApplicationController>
    {

        //reference to parent controller
        ApplicationController applicationController;
        //reference to panel to control
        IAnalyzer3DViewPanelUI analyzer3DViewPanelUI;
        //reference to the currently selected simulation
        Simulation selectedSimulation;
       
       
        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="analyzer3DViewPanelUI"></param>
        public Analyzer3DViewPanelController(IAnalyzer3DViewPanelUI analyzer3DViewPanelUI)
        {
            this.analyzer3DViewPanelUI = analyzer3DViewPanelUI;
            this.selectedSimulation = null;
           // init();
 
        }

        #region IControllable<ApplicationController> Members

        /// <summary>
        /// Set the class responsible for controlling this controller
        /// </summary>
        /// <param name="controller"></param>
        public void setController(ApplicationController controller)
        {
            applicationController = controller;
        }

        #endregion

        /// <summary>
        /// Set the currently selected simulation
        /// </summary>
        /// <param name="sim"></param>
        public void setSelectedSimulation(Simulation sim)
        {
            selectedSimulation = sim;
            analyzer3DViewPanelUI.setSimulation(sim);

        }

        /// <summary>
        /// Get the currently selected simulation
        /// </summary>
        /// <returns></returns>
        public Simulation GetSelectedSimulation()
        {
            return selectedSimulation;
        }


        /// <summary>
        /// Register a timer tick
        /// </summary>
        public void timer1Tick()
        {
            analyzer3DViewPanelUI.timer1Tick();
        }


    }
}
