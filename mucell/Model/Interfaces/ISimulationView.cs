using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model
{
    
    /// <summary>
    /// View interface for the Simulation Object
    /// </summary>
    public interface ISimulationView
    {
    
        string Name
        {
            get;
        }
        
        SimulationParameters Parameters
        {
        		get;
        }
        
        Results SimulationResults 
        {
        		get;
        }
        
        
        
    }
}
