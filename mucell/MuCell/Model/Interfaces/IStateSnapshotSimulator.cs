using System;
using System.Collections.Generic;

namespace MuCell.Model
{
	
	public interface IStateSnapshotSimulator
	{

		List<CellInstance> Cells
		{
			get;
			set;
		}
		
		Environment SimulationEnvironment
		{
			get;
			set;
		}
	
	}
}
