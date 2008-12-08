using System;

namespace MuCell.Model
{
	
	/// <summary>
	/// Controller interface for the TimeSeriesFunction object
	/// </summary>
	public interface ITimeSeriesFunctionController
	{
	
		string Name
		{
			get;
			set;
		}
		
		double TimeInterval
		{
			get;
			set;
		}
		
        string FunctionString
        {
            get;
            set;
        }
	
	}
}
