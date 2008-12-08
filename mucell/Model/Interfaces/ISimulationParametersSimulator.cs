using System;
using System.Collections.Generic;

namespace MuCell.Model
{
	
	public interface ISimulationParametersSimulator
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
