using System;

namespace MuCell.Model
{

	/// <summary>
	/// Controller interface for the CellDefinition data object
	/// </summary>	
	public interface ICellDefinitionController : ICellDefinitionView
	{

		string name
    		{
    			get;
    			set;
    		}
    		
		// Allow adding of SBML model
		bool addSBMLModel(string filePath);
	
	}
}
