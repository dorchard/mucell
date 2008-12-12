using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model
{

	/// <summary>
	/// Controller interface for the Simulation object
	/// </summary>
    interface ISimulationController : ISimulationView
    {
    
    	     string Name
        {
            get;
            set;
        }
        
        Results SimulationResults 
        {
        		get;
        		set;
        }
        
        SimulationParameters Parameters
        {
        		get;
        		set;
        }
    
    }
}
