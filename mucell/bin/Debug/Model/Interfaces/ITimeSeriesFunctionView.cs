using System;

namespace MuCell.Model
{
	
	/// <summary>
	/// View interface for the TimeSeries object
	/// </summary>
	public interface ITimeSeriesFunctionView
	{
	
		string Name
		{
			get;
		}

		double TimeInterval
		{
			get;
		}
			
		string FunctionString
        {
            get;
        }
	
	}
}
