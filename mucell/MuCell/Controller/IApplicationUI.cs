using System;
using System.Collections.Generic;
using System.Text;




namespace MuCell.Controller
{

    /// <summary>
    /// Interface defining the methods the applicationUI must provide
    /// the system controller with.
    /// </summary> 
    public interface IApplicationUI 
    {


        /// <summary>
        /// Return the ExperimentTree child panel
        /// </summary> 
        IExperimentTreePanelUI getExperimentTreePanelUI();


    }
}
