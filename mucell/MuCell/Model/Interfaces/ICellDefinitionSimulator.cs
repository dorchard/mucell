using System;

namespace MuCell.Model
{

	public interface ICellDefinitionSimulator
	{

		string name
    		{
    			get;
    			set;
    		}
    		
    		CellInstance createCell();

		SBML.Model getSBMLModel();

	}
}