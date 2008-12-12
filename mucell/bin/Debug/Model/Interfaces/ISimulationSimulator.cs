using System;

namespace MuCell.Model
{
	
	public interface ISimulationSimulator
	{
	    	string Name
        {
            get;
        }
        
        Results SimulationResults 
        {
        		get;
        		set;
        }
        
        SimulationParameters Parameters
        {
        		get;
        }
	}
}
