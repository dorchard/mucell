using System;
using System.Collections.Generic;

namespace MuCell.Model
{
	
	/// <summary>
	/// Controller interface for the Simulation Parameters data object
	/// </summary>
	public interface ISimulationParametersController
	{
	
		double StepTime
		{
			get;
			set;
		}
		
		double SimulationLength 
        {
			get;
			set;
        }
        
        double SnapshotInterval
        {
        		get;
        		set;
        }
        StateSnapshot InitialState
        {
        		get;
        }
         
       	List<TimeSeries> TimeSeries
        {
       		get;
        }		
	
	}
}
