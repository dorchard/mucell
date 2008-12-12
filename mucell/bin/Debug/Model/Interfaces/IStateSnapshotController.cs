using System;
using System.Collections.Generic;

namespace MuCell.Model
{
	
	/// <summary>
	/// Controller interface for the StateSnapshot object
	/// </summary>
	public interface IStateSnapshotController
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
