using System;
using System.Collections.Generic;

namespace MuCell.Model
{
	
	/// <summary>
	/// View interface for the TimeSeries object
	/// </summary>
	public interface ITimeSeriesView
	{
	
		TimeSeriesFunction SeriesFunction
		{
			get;
		}
		
		List<double> Series
		{
			get;
		}
	
	}
}
