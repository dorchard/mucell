using System;
using MuCell.Model;

namespace MuCell
{
	
	public interface IExperimentSimulator
	{
	
		string Name
		{
			get;
		}
		
		int Id {
			get;
		}
		
		CellDefinition[] getCellDefinitions();
		Simulation[] getSimulations();
	
	}
}
