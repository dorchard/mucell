using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using MuCell.Model.SBML;
using MuCell.Model;

namespace UnitTests.Model
{

    [TestFixture] public class TestFormulas
    {

        [Test] public void simpleTest()
        {
            MuCell.Model.SBML.Reader.SBMLReader reader = new MuCell.Model.SBML.Reader.SBMLReader();
            FormulaParser fp = new FormulaParser(reader, "3.4+4/(abs(2-4))");
            MathTree formulaTree = fp.getFormulaTree();
            
			MuCell.Model.SBML.InnerNode root = (InnerNode)formulaTree.root;

            Assert.AreEqual(BinaryMathOperators.Plus, root.data);
            
            /// Assert that the left node is 3.4
            Assert.AreEqual(3.4, ((NumberLeafNode)(root.subtree.ToArray())[0]).data);
            
            // Assert that the right is divide
            
            MuCell.Model.SBML.InnerNode right = (InnerNode)(root.subtree.ToArray())[1];
            Assert.AreEqual(BinaryMathOperators.Divide, right.data);
                        
            // Assert that the right's child is 4 and abs
            Assert.AreEqual(4, ((NumberLeafNode)right.subtree[0]).data);
            Assert.AreEqual(UnaryMathOperators.Abs, ((InnerNode)right.subtree[1]).data);
            // assert that this is a minus
            
            InnerNode minus_child = (InnerNode)((InnerNode)(right.subtree[1])).subtree[0];
            Assert.AreEqual(BinaryMathOperators.Minus, minus_child.data);
            
            Assert.AreEqual(2, ((NumberLeafNode)minus_child.subtree[0]).data);
            Assert.AreEqual(4, ((NumberLeafNode)minus_child.subtree[1]).data);
            
            // Finally evaluate and check the result
            
            // Fold the tree into a cell function
            CellEvaluationFunction fun = formulaTree.ToCellEvaluationFunction();
            Assert.AreEqual(5.4, fun(new StateSnapshot(), new CellInstance(new CellDefinition())));
        }

		[Test] public void testWithVariables()
		{
			// create a formula
			MuCell.Model.SBML.Reader.SBMLReader reader = new MuCell.Model.SBML.Reader.SBMLReader();
			MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
			Species s = new Species("X");
			s.BoundaryCondition = false;
            model.AddId(s.ID, s); // else FormulaParser can't find what X is
			reader.model = model;
			
            FormulaParser fp = new FormulaParser(reader, "10+sin(X)", model);
            MathTree formulaTree = fp.getFormulaTree();
            
            // fold the function
            CellEvaluationFunction fun = formulaTree.ToCellEvaluationFunction();
            
            // create a new CellInstance with a variable X
            CellInstance cell = new CellInstance(new CellDefinition());
            cell.setSpeciesAmount("X", 2.3d);
            
            // evaluate and test that this gets the right result
            Assert.AreEqual(10+Math.Sin(2.3), fun(new StateSnapshot(), cell));
		}
	
		/// <summary>
		/// Test with a celldefinition aggregate
		/// </summary>
		[Test] public void testWithCellDef()
		{
			MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
			MuCell.Model.Experiment experiment = new MuCell.Model.Experiment("experimen1");
			MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
			setupExperiment(model, experiment, simulation);
			
			
			// Test parsing cell def references, group references etc.
			MuCell.Model.SBML.Reader.SBMLReader reader = new MuCell.Model.SBML.Reader.SBMLReader();
            FormulaParser fp = new FormulaParser(reader, "celldef1", model, experiment, simulation);
            MathTree formulaTree = fp.getFormulaTree();
            
            MuCell.Model.SBML.AggregateReferenceNode root = (MuCell.Model.SBML.AggregateReferenceNode)formulaTree.root;
            
            Assert.AreEqual("celldef1", root.ToString());
            
            Assert.AreEqual(experiment.getCellDefinitions()[0], root.CellDefinition);
            Assert.AreEqual(null, root.Group);
            Assert.AreEqual(null, root.Species);
		}

        /// <summary>
        /// Test with a celldefinition aggregate
        /// </summary>
        [Test]
        public void testWithCellDefWithSpaces()
        {
            MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
            MuCell.Model.Experiment experiment = new MuCell.Model.Experiment("experimen1");
            MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
            setupExperiment(model, experiment, simulation);
            experiment.GetCellDefinition("celldef1").Name = "cell def1";

            Assert.That(experiment.ContainsCellDefinition("cell_def1"));


            // Test parsing cell def references, group references etc.
            MuCell.Model.SBML.Reader.SBMLReader reader = new MuCell.Model.SBML.Reader.SBMLReader();
            FormulaParser fp = new FormulaParser(reader, "cell_def1", model, experiment, simulation);
            MathTree formulaTree = fp.getFormulaTree();

            MuCell.Model.SBML.AggregateReferenceNode root = (MuCell.Model.SBML.AggregateReferenceNode)formulaTree.root;

            Assert.AreEqual("cell def1", root.ToString());

            Assert.AreEqual(experiment.getCellDefinitions()[0], root.CellDefinition);
            Assert.AreEqual(null, root.Group);
            Assert.AreEqual(null, root.Species);
        }

		/// <summary>
		/// Test just a group ref
		/// </summary>
		[Test] public void testWithGroup()
		{
			MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
			MuCell.Model.Experiment experiment = new MuCell.Model.Experiment("experimen1");
			MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
			setupExperiment(model, experiment, simulation);
			
			// Test parsing cell def references, group references etc.
			MuCell.Model.SBML.Reader.SBMLReader reader = new MuCell.Model.SBML.Reader.SBMLReader();
            FormulaParser fp = new FormulaParser(reader, "group1", model, experiment, simulation);
            MathTree formulaTree = fp.getFormulaTree();
            
            MuCell.Model.SBML.AggregateReferenceNode root = (MuCell.Model.SBML.AggregateReferenceNode)formulaTree.root;
            
            Assert.AreEqual("group1", root.ToString());
            
            Assert.AreEqual(null, root.CellDefinition);
            Assert.AreEqual("1", root.Group.ID);
            Assert.AreEqual(null, root.Species);
		}
		
		/// <summary>
		/// Test celldef and group
		/// </summary>
		[Test] public void testWithCellDefAndGroup()
		{
			MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
			MuCell.Model.Experiment experiment = new MuCell.Model.Experiment("experimen1");
			MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
			setupExperiment(model, experiment, simulation);
			
			// Test parsing cell def references, group references etc.
			MuCell.Model.SBML.Reader.SBMLReader reader = new MuCell.Model.SBML.Reader.SBMLReader();
            FormulaParser fp = new FormulaParser(reader, "celldef2.group2", model, experiment, simulation);
            MathTree formulaTree = fp.getFormulaTree();
            
            MuCell.Model.SBML.AggregateReferenceNode root = (MuCell.Model.SBML.AggregateReferenceNode)formulaTree.root;
            
            Assert.AreEqual("celldef2.group2", root.ToString());
            
            Assert.AreEqual(experiment.getCellDefinitions()[1], root.CellDefinition);
            Assert.AreEqual("2", root.Group.ID);
            Assert.AreEqual(null, root.Species);
		}
	
		/// <summary>
		/// Tests a formula with a celldef and species reference
		/// </summary>
		[Test] public void testWithCellDefAndSpecies()
		{
			MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
			MuCell.Model.Experiment experiment = new MuCell.Model.Experiment("experimen1");
			MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
			setupExperiment(model, experiment, simulation);
			
			// Test parsing cell def references, group references etc.
			MuCell.Model.SBML.Reader.SBMLReader reader = new MuCell.Model.SBML.Reader.SBMLReader();
            FormulaParser fp = new FormulaParser(reader, "celldef1.s1", model, experiment, simulation);
            MathTree formulaTree = fp.getFormulaTree();
            
            MuCell.Model.SBML.AggregateReferenceNode root = (MuCell.Model.SBML.AggregateReferenceNode)formulaTree.root;
            
            Assert.AreEqual("celldef1.s1", root.ToString());
            
            Assert.AreEqual(experiment.getCellDefinitions()[0], root.CellDefinition);
            Assert.AreEqual(null, root.Group);
            Assert.AreEqual("s1", root.Species.ID);
		}
	
		/// <summary>
		/// Test refernec to a celldef, group and species
		/// </summary>
		[Test] public void testWithCellDefAndGroupAndSpecies()
		{
			MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
			MuCell.Model.Experiment experiment = new MuCell.Model.Experiment("experimen1");
			MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
			setupExperiment(model, experiment, simulation);
			
			// Test parsing cell def references, group references etc.
			MuCell.Model.SBML.Reader.SBMLReader reader = new MuCell.Model.SBML.Reader.SBMLReader();
            FormulaParser fp = new FormulaParser(reader, "5.2+celldef1.group2.s1", model, experiment, simulation);
            MathTree formulaTree = fp.getFormulaTree();
            
            MuCell.Model.SBML.InnerNode root = (InnerNode)formulaTree.root;
            Assert.AreEqual(BinaryMathOperators.Plus, root.data);
            
            Assert.AreEqual(5.2, ((MuCell.Model.SBML.NumberLeafNode)(root.subtree.ToArray()[0])).data);
            
            Assert.AreEqual("5.2+celldef1.group2.s1", root.ToString());
            
            MuCell.Model.SBML.AggregateReferenceNode right = (MuCell.Model.SBML.AggregateReferenceNode)(root.subtree.ToArray()[1]);
            
            Assert.AreEqual("celldef1.group2.s1", right.ToString());
            
            Assert.AreEqual(experiment.getCellDefinitions()[0], right.CellDefinition);
            Assert.AreEqual("2", right.Group.ID);
            Assert.AreEqual("s1", right.Species.ID);
		}
		
		/// <summary>
		/// Tests a reference to just a species
		/// </summary>
		[Test] public void testWithSpecies()
		{
			MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
			MuCell.Model.Experiment experiment = new MuCell.Model.Experiment("experimen1");
			MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
			setupExperiment(model, experiment, simulation);
			
			// Test parsing cell def references, group references etc.
			MuCell.Model.SBML.Reader.SBMLReader reader = new MuCell.Model.SBML.Reader.SBMLReader();
            FormulaParser fp = new FormulaParser(reader, "5.2+s2", model, experiment, simulation);
            MathTree formulaTree = fp.getFormulaTree();
            
            MuCell.Model.SBML.InnerNode root = (InnerNode)formulaTree.root;
            Assert.AreEqual(BinaryMathOperators.Plus, root.data);
            
            Assert.AreEqual(5.2, ((MuCell.Model.SBML.NumberLeafNode)(root.subtree.ToArray()[0])).data);
            
            Assert.AreEqual("5.2+s2", root.ToString());
            
            MuCell.Model.SBML.AggregateReferenceNode right = (MuCell.Model.SBML.AggregateReferenceNode)(root.subtree.ToArray()[1]);
            
            Assert.AreEqual("s2", right.ToString());
            
            Assert.AreEqual(null, right.CellDefinition);
            Assert.AreEqual(null, right.Group);
            Assert.AreEqual("s2", right.Species.ID);
            
		}
		
		/// <summary>
		/// Test a reference to a group and species
		/// </summary>
		[Test] public void testWithGroupAndSpecies()
		{
			MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
			MuCell.Model.Experiment experiment = new MuCell.Model.Experiment("experimen1");
			MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
			setupExperiment(model, experiment, simulation);
			
			// Test parsing cell def references, group references etc.
			MuCell.Model.SBML.Reader.SBMLReader reader = new MuCell.Model.SBML.Reader.SBMLReader();
            FormulaParser fp = new FormulaParser(reader, "5.2*group1.s1", model, experiment, simulation);
            MathTree formulaTree = fp.getFormulaTree();
            
            MuCell.Model.SBML.InnerNode root = (InnerNode)formulaTree.root;
            Assert.AreEqual(BinaryMathOperators.Times, root.data);
            
            Assert.AreEqual(5.2, ((MuCell.Model.SBML.NumberLeafNode)(root.subtree.ToArray()[0])).data);
            
            Assert.AreEqual("5.2*group1.s1", root.ToString());
            
            MuCell.Model.SBML.AggregateReferenceNode right = (MuCell.Model.SBML.AggregateReferenceNode)(root.subtree.ToArray()[1]);
            
            Assert.AreEqual("group1.s1", right.ToString());
            
            Assert.AreEqual(null, right.CellDefinition);
            Assert.AreEqual("1", right.Group.ID);
            Assert.AreEqual("s1", right.Species.ID);
		}

        [Test]
        public void testIdentifiersLists()
        {
            MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.xml");
            MuCell.Model.SBML.Reader.SBMLReader reader = new MuCell.Model.SBML.Reader.SBMLReader();
            FormulaParser fp = new FormulaParser(reader, "X*A+k_1*B_A", s.model);
            MathTree formulaTree = fp.getFormulaTree();
            
            List<Species> formulaSpecies = fp.SpeciesFromFormula();
            Assert.AreEqual(2, formulaSpecies.Count);
            Assert.AreEqual("X", formulaSpecies.ToArray()[0].ID);
            Assert.AreEqual("A", formulaSpecies.ToArray()[1].ID);

            List<Parameter> parameters = fp.ParametersFromFormula();
            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual("k_1", parameters.ToArray()[0].ID);

            List<UnknownEntity> identifiers = fp.UnknownEntitiesFromFormula();
            Assert.AreEqual(1, identifiers.Count);
            Assert.AreEqual("B_A", identifiers.ToArray()[0].ID);
        }

        [Test]
        public void testApproximateUnits()
        {
            MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
            MuCell.Model.Experiment experiment = new MuCell.Model.Experiment("experimen1");
            MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
            setupExperiment(model, experiment, simulation);

            // Test parsing cell def references, group references etc.
            MuCell.Model.SBML.Reader.SBMLReader reader = new MuCell.Model.SBML.Reader.SBMLReader();
            FormulaParser fp = new FormulaParser(reader, "5.2", model, experiment, simulation);
            MathTree formulaTree = fp.getFormulaTree();
            Assert.AreEqual("No units", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "5.2+10", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("No units", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "5.2-10", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("No units", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "5.2*10", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("No units", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "5.2/10", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("No units", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1+celldef2", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1-celldef1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1*celldef2", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells*Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1/celldef2", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells/Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1*celldef2*celldef1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells*Cells*Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1+10", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1*10", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1 / 34.2", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "4/celldef1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("1/Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "24/(celldef1*celldef1)", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("1/Cells*Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1.group1/(celldef1*celldef1)", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells/Cells*Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1/celldef1+10", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells/Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1.group1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1.group2 + celldef1.group1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1.group1/celldef1.group2", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells/Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "143/celldef1.group1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("1/Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "s1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Concentration", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "s1*4", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Concentration", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "s1*s2", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Concentration*Concentration", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "s1*s2*celldef1.s1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Concentration*Concentration*Concentration", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "s1*celldef1.group1.s1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Concentration*Concentration", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "group2.s2*celldef1.group1.s1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Concentration*Concentration", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "s1/s2", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Concentration/Concentration", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "4.345/s1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("1/Concentration", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "s1+celldef1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("No units", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "4.345/s1+celldef1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("No units", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1*celldef1.group1.s1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells*Concentration", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "s2/celldef1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Concentration/Cells", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "s2/celldef1+s1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("No units", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "s2/celldef1+celldef2", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("No units", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "celldef1/celldef1.group1.s1", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Cells/Concentration", formulaTree.ApproximateUnits());

            fp = new FormulaParser(reader, "Abs(s1)", model, experiment, simulation);
            formulaTree = fp.getFormulaTree();
            Assert.AreEqual("Concentration", formulaTree.ApproximateUnits());

        }
  
		public static void setupExperiment(MuCell.Model.SBML.Model model, MuCell.Model.Experiment experiment, MuCell.Model.Simulation simulation)
		{
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
			
			// set up the cell definition
			MuCell.Model.CellDefinition celldef1 = new MuCell.Model.CellDefinition("celldef1");
			celldef1.addSBMLModel(model);
			
			MuCell.Model.CellDefinition celldef2 = new MuCell.Model.CellDefinition("celldef2");
			celldef2.addSBMLModel(model);
			
			MuCell.Model.CellInstance cell1 = celldef1.createCell();
			MuCell.Model.CellInstance cell2 = celldef2.createCell();
			
			List<MuCell.Model.CellInstance> cells = new List<CellInstance>();
			cells.Add(cell1);
			cells.Add(cell2);
			
			// StateSnapshot for intial state
			MuCell.Model.StateSnapshot initialState = new MuCell.Model.StateSnapshot(cells);
			MuCell.Model.Vector3 size = new MuCell.Model.Vector3(1, 1, 1);
			initialState.SimulationEnvironment = new MuCell.Model.Environment(size);
			// Create two groups with one cell of each type in each
			initialState.SimulationEnvironment.AddCellToGroup(1, celldef1.createCell());
			initialState.SimulationEnvironment.AddCellToGroup(2, celldef2.createCell());
			
			// Parameters
			MuCell.Model.SimulationParameters parameters = new MuCell.Model.SimulationParameters();
			parameters.InitialState = initialState;
					
			// Simulation
			simulation.Parameters = parameters;
			
			// Experiment

			experiment.addCellDefinition(celldef1);
			experiment.addCellDefinition(celldef2);
			experiment.addSimulation(simulation);
		}

    }
}
