using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace UnitTests.Model
{
	
	[TestFixture] public class TestEvaluationFunctions
	{
		
		/// <summary>
		/// Performs a test on using species, reactions and cells 
		/// in conjunction with string formulas and math trees
		/// using the EffectReactionEvaluation technique for effecing the state
		/// </summary>
		[Test] public void TestWithSpecies()
		{
			// Create a model 		
			MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
			MuCell.Model.SBML.Species species1 = new MuCell.Model.SBML.Species();
			MuCell.Model.SBML.Species species2 = new MuCell.Model.SBML.Species();
			
			model.listOfSpecies = new List<MuCell.Model.SBML.Species>();
			model.listOfSpecies.Add(species1);
			model.listOfSpecies.Add(species2);
			
			// Set some values for species1
			species1.ID = "s1";
			species1.InitialAmount = 4.0d;
			
			// Set some values for species2
			species2.ID = "s2";
			species2.InitialAmount = 0.1d;
			
			model.AddId("s1", species1);
			model.AddId("s2", species2);
			
			// Set up the reaction
			MuCell.Model.SBML.Reaction reaction1 = new MuCell.Model.SBML.Reaction("reaction1");
			
			model.listOfReactions = new List<MuCell.Model.SBML.Reaction>();
			model.listOfReactions.Add(reaction1);
			
			// Set up the kinetic law
			reaction1.KineticLaw = new MuCell.Model.SBML.KineticLaw(model);
			reaction1.KineticLaw.Formula = "(s1)*2";
			
			// set up the species reference for the reactants
			MuCell.Model.SBML.SpeciesReference ref1 = new MuCell.Model.SBML.SpeciesReference(species1, 1);
			// set up the species references for the products
			MuCell.Model.SBML.SpeciesReference ref2 = new MuCell.Model.SBML.SpeciesReference(species1, 0.75);
			MuCell.Model.SBML.SpeciesReference ref3 = new MuCell.Model.SBML.SpeciesReference(species2, 2);
			
			// Add the references
			reaction1.Reactants.Add(ref1);
			reaction1.Products.Add(ref2);
			reaction1.Products.Add(ref3);
						
			// set up the cell definition
			MuCell.Model.CellDefinition celldef1 = new MuCell.Model.CellDefinition("cell1");
			celldef1.addSBMLModel(model);
			
			// set up a cell
			MuCell.Model.CellInstance cell1 = celldef1.createCell();
			
			cell1.localSpeciesDelta.Add("s1", 0.0d);
			cell1.localSpeciesDelta.Add("s2", 0.0d);
			
			cell1.localSpeciesVariables.Add("s1", 4.0d);
			cell1.localSpeciesVariables.Add("s2", 0.1d);
			
			// set up the statesnaphost
			MuCell.Model.StateSnapshot state1 = new MuCell.Model.StateSnapshot();
			state1.Cells.Add(cell1);
			
			// get the eval function
			MuCell.Model.EffectReactionEvaluationFunction fun1 = reaction1.ToEffectReactionEvaluationFunction();
			
			// Evaluate
			fun1(state1, cell1);
			
			cell1.setSpeciesAmount("s1", cell1.UnitTestGetSpeciesAmountsAndDeltas("s1"));
			cell1.setSpeciesAmount("s2", cell1.UnitTestGetSpeciesAmountsAndDeltas("s2"));
			
			cell1.UnitTestClearDeltas();
			
			Assert.AreEqual(2.0d, cell1.getLocalSimulationSpeciesAmount("s1"));
			Assert.AreEqual(16.1d, cell1.getLocalSimulationSpeciesAmount("s2"));
			
			// Evaluate again
			fun1(state1, cell1);
			
			cell1.setSpeciesAmount("s1", cell1.UnitTestGetSpeciesAmountsAndDeltas("s1"));
			cell1.setSpeciesAmount("s2", cell1.UnitTestGetSpeciesAmountsAndDeltas("s2"));
			
			cell1.UnitTestClearDeltas();
			
			
			Assert.AreEqual(1.0d, cell1.getLocalSimulationSpeciesAmount("s1"));
			Assert.AreEqual(24.1d, cell1.getLocalSimulationSpeciesAmount("s2"));			
		}

		[Test] public void TestWithParameters()
		{
			// Create a model 		
			MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
			MuCell.Model.SBML.Species species1 = new MuCell.Model.SBML.Species();
			MuCell.Model.SBML.Species species2 = new MuCell.Model.SBML.Species();
			MuCell.Model.SBML.Parameter parameter = new MuCell.Model.SBML.Parameter();
			
			parameter.ID = "k_1";
			parameter.Value = 0.07d;
						
			model.listOfParameters = new List<MuCell.Model.SBML.Parameter>();
			model.listOfParameters.Add(parameter);
			
			model.listOfSpecies = new List<MuCell.Model.SBML.Species>();
			model.listOfSpecies.Add(species1);
			model.listOfSpecies.Add(species2);
			
			// Set some values for species1
			species1.ID = "s1";
			species1.InitialAmount = 1.0d;
			
			// Set some values for species2
			species2.ID = "s2";
			species2.InitialAmount = 12.0d;
			
			model.AddId("s1", species1);
			model.AddId("s2", species2);
			model.AddId("k_1", parameter);
			
			// Set up the reaction
			MuCell.Model.SBML.Reaction reaction1 = new MuCell.Model.SBML.Reaction("reaction1");
			
			// Set up the kinetic law
			reaction1.KineticLaw = new MuCell.Model.SBML.KineticLaw(model);
			reaction1.KineticLaw.Formula = "s1*s2*k_1";
			
			// set up the species reference for the reactants
			MuCell.Model.SBML.SpeciesReference ref1 = new MuCell.Model.SBML.SpeciesReference(species1, 1);
			// set up the species references for the products
			MuCell.Model.SBML.SpeciesReference ref2 = new MuCell.Model.SBML.SpeciesReference(species2, 3);
						
			// Add the references
			reaction1.Reactants.Add(ref1);
			reaction1.Products.Add(ref2);
									
			// set up the cell definition
			MuCell.Model.CellDefinition celldef1 = new MuCell.Model.CellDefinition("cell1");
			celldef1.addSBMLModel(model);
			
			// set up a cell
			MuCell.Model.CellInstance cell1 = celldef1.createCell();
			
			cell1.localSpeciesDelta.Add("s1", 0.0d);
			cell1.localSpeciesDelta.Add("s2", 0.0d);
			
			cell1.localSpeciesVariables.Add("s1", 1.0d);
			cell1.localSpeciesVariables.Add("s2", 12.0d);
			
			// set up the statesnaphost
			MuCell.Model.StateSnapshot state1 = new MuCell.Model.StateSnapshot();
			state1.Cells.Add(cell1);
			
			// get the eval function
			MuCell.Model.EffectReactionEvaluationFunction fun1 = reaction1.ToEffectReactionEvaluationFunction();
			
			// Evaluate
			fun1(state1, cell1);
			
			
			cell1.setSpeciesAmount("s1", cell1.UnitTestGetSpeciesAmountsAndDeltas("s1"));
			cell1.setSpeciesAmount("s2", cell1.UnitTestGetSpeciesAmountsAndDeltas("s2"));
			
			cell1.UnitTestClearDeltas();
			
			AssertDouble.AreEqual(1.0d-0.84d, cell1.getLocalSimulationSpeciesAmount("s1"));
			AssertDouble.AreEqual(12.0d+0.84d*3.0d, cell1.getLocalSimulationSpeciesAmount("s2"));

			
			System.Console.WriteLine("about to eval");
			
			// Evaluate again
			fun1(state1, cell1);

			cell1.setSpeciesAmount("s1", cell1.UnitTestGetSpeciesAmountsAndDeltas("s1"));
			cell1.setSpeciesAmount("s2", cell1.UnitTestGetSpeciesAmountsAndDeltas("s2"));
			
			cell1.UnitTestClearDeltas();
						
			AssertDouble.AreEqual(0.16d-0.162624d, cell1.getLocalSimulationSpeciesAmount("s1"));
			AssertDouble.AreEqual(14.52d+0.162624d*3.0d, cell1.getLocalSimulationSpeciesAmount("s2"));
			
			// Check the param is the same
			Assert.AreEqual(0.07m, parameter.Value);
		}
	}
}
