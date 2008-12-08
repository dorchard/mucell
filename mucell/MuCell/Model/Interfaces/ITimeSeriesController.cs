using System;
using System.Collections.Generic;

namespace MuCell.Model
{
	
	/// <summary>
	/// Controller interface for the TimeSeries object
	/// </summary>
	public interface ITimeSeriesController
	{
	
		TimeSeriesFunction SeriesFunction
		{
			get;
			set;
		}
		
		List<double> Series
		{
			get;
		}

	}
}
