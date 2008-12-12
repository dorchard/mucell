using System;

namespace MuCell.Model
{

	/// <summary>
	/// Controller interface for the Experiment data object
	/// </summary>
	public interface IExperimentController : IExperimentView
	{
		
		string Name
		{
			get;
			set;
		}
		
		int Id {
			get;
			set;
		}
		
		bool save(string filePath);
		
		void addCellDefinition(CellDefinition cellDefinition);
		CellDefinition[] getCellDefinitions();
		
		void addSimulation(Simulation simulation);
		Simulation[] getSimulations();
	}
}
