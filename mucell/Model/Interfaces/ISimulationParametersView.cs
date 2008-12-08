using System;
using System.Collections.Generic;

namespace MuCell.Model
{
	
	/// <summary>
	/// View interface for the Simulation Parameters data object
	/// </summary>
	public interface ISimulationParametersView
	{
	
		double StepTime
		{
			get;
		}
		
		double SimulationLength 
        {
			get;
        }
        
        double SnapshotInterval
        {
        		get;
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