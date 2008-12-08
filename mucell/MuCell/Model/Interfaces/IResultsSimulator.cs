using System;
using System.Collections.Generic;

namespace MuCell.Model
{
	
	public interface IResultsSimulator
	{
	
		// Controller/Analyser can only get the file path
		// <todo> Possibly revise this to not have it at all? </todo>
		string FilePath
		{
			get;
		}
		
		// Controller/Analyser can update state snapshots
		List<StateSnapshot> StateSnapshots
		{
			get;
			set;
		}
	
		// Controller/Analyser has full access to current state
		StateSnapshot CurrentState
		{
			get;
			set;
		}
		
		// Controller/Analyser has full access to time series
		List<TimeSeries> TimeSeries
		{
			get;
			set;
		}
	
	}
}
