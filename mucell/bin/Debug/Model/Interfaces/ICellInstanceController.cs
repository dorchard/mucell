using System;

namespace MuCell.Model
{

	/// <summary>
	/// Controller interface for the CellInstance data object
	/// </summary>
	public interface ICellInstanceController
	{
		
		CellDefinition cellDefinition
		{
			get;
			set;
		}
		
		SpatialContext spatialContext
		{
			get;
			set;
		}
	
	}
}
