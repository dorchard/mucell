using System;

namespace MuCell.Model
{
	
	/// <summary>
	/// View interface for the Experiment data objects
	/// </summary>
	public interface IExperimentView
	{
		
		// Ability to get cell definitions and simulations
		CellDefinition[] getCellDefinitions();		
		Simulation[] getSimulations();
		
        int Id
        {
            get;
        }
		
		string Name
		{
			get;
			set;
		}

        String getLastSavedPath();

        bool save(string filePath);
	}
}
