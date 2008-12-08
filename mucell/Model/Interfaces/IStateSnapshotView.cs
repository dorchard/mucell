using System;
using System.Collections.Generic;

namespace MuCell.Model
{
	
	/// <summary>
	/// View interface for the StateSnapshot object
	/// </summary>
	public interface IStateSnapshotView
	{
	
		List<CellInstance> Cells
		{
			get;
		}
		
		Environment SimulationEnvironment
		{
			get;
		}
	
	}
}
