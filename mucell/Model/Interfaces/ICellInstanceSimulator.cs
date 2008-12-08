using System;
using MuCell.Model;

namespace MuCell
{
	
	public interface ICellInstanceSimulator
	{
	
		CellDefinition cellDefinition
		{
			get;
		}
		
		SpatialContext spatialContext
		{
			get;
			set;
		}
	
	}
}
