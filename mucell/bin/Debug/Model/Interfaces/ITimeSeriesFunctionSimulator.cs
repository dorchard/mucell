using System;
using System.Collections.Generic;

namespace MuCell.Model
{
	
	public interface ITimeSeriesFunctionSimulator
	{
	
	// Can call evaluate next
	decimal evaluateNext(StateSnapshot state);
	
	void InitializeTimeSeriesFunction(List<Model.SBML.Model> models, Experiment experiment, Simulation simulation);
	
	}
}
