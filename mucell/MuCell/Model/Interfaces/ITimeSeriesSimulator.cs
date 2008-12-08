using System;

namespace MuCell.Model
{
	
	public interface ITimeSeriesSimulator
	{
	
		string Name
		{
			get;
		}

		double TimeInterval
		{
			get;
		}
	
		// Can perform the evaluate function
		void evaluate(StateSnapshot state);
	
	}
}
