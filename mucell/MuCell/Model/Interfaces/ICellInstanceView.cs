using System;

namespace MuCell.Model
{
	/// <summary>
	/// View interface for the CellInstance data object
	/// </summary>
	public interface ICellInstanceView
	{
		
		CellDefinition cellDefinition
		{
			get;
		}
		
		SpatialContext spatialContext
		{
			get;
		}
		
	}
}
