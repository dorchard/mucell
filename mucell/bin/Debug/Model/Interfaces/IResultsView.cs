using System;
using System.Collections.Generic;

namespace MuCell.Model
{
	
	/// <summary>
	/// View interface for the Results object
	/// </summary>
	public interface IResultsView
	{
	
	
		// View has full access to File Path
		string FilePath
		{
			get;
		}
		
		// View has read only access to State Snapshots
		List<StateSnapshot> StateSnapshots
		{
			get;
		}
		
		// View has read only access to Time Series
		List<TimeSeries> TimeSeries
		{
			get;
		}
	
	}
}
