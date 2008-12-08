// created on 22/01/2008 at 15:36
using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnitTests.Controller {

	[TestFixture] public class TestSimulator {
    	
    		// Data generated from test CVODE program
    		double cvode_x_t1 = 2.4925099321963900d;
    		double cvode_y_t1 = 2.4999995933414200d;
    		double cvode_z_t1 = 2.4999423252419400d;
    		
    		double cvode_x_t2 = 2.4850473957127900d;
    		double cvode_y_t2 = 2.4999975627323100d;
    		double cvode_z_t2 = 2.4997947046791800d;

		double cvode_x_t3 = 2.4776073058136900d;
    		double cvode_y_t3 = 2.4999933729171300d;
    		double cvode_z_t3 = 2.4995748099626500d;
    		
		double cvode_x_t4 = 2.4701896649036800d;
    		double cvode_y_t4 = 2.4999863383052300d;
    		double cvode_z_t4 = 2.4992835798738000d;
    		
    		// generated from the test Euler program
    		double euler_x_t1 = 2.4924925000000000d;
    		double euler_y_t1 = 2.5000000000000000d;
    		double euler_z_t1 = 2.5000000000000000d;
    		
    		double euler_x_t2 = 2.4850075450225000d;
    		double euler_y_t2 = 2.5000000000000000d;
    		double euler_z_t2 = 2.4999248499250002d;

		double euler_x_t3 = 2.4775450673647974d;
    		double euler_y_t3 = 2.4999992477477493d;
    		double euler_z_t3 = 2.4997755277029263d;
    		
		double euler_x_t4 = 2.4701050181835269d;
    		double euler_y_t4 = 2.4999925637948741d;
    		double euler_z_t4 = 2.4995530007949416d;
    		
    		// generated from the test Rudge-Kutta program
    		double rk_x_t1 = 2.4925037620179653d;
    		double rk_y_t1 = 2.4999998753462540d;
    		double rk_z_t1 = 2.4999625875143727d;
    		
    		double rk_x_t2 = 2.4850300122778179d;
    		double rk_y_t2 = 2.4999990084739299d;
    		double rk_z_t2 = 2.4998509968491374d;

		double rk_x_t3 = 2.4775787107364966d;
    		double rk_y_t3 = 2.4999966727660135d;
    		double rk_z_t3 = 2.4996661905936306d;
    		
		double rk_x_t4 = 2.4701498349828896d;
    		double rk_y_t4 = 2.4999921583804956d;
    		double rk_z_t4 = 2.4994091214394247d;
    		
//    		// Define a function called test1 that matches the delegate RateEquation
//    		double test1(params double[] ps) {
//    			double result = 0;
//    			// Sum up the arguments adding the index as well sum(ps) + sum(0..ps.length)	
//    			for (int i=0;i<ps.Length;i++){
//    				result = ps[i]+result+i;
//    			}
//    			return result;
//    		}
//    		   		
    
    		/// <summary>
    		/// SBML Version 1 Level 1 Test
    		/// </summary>
    		[Test] public void TestHopfSimulationV1L1_RungeKutta()
    		{
    			// Hopf model
			MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.xml");
			RunHopfSimulationRK(s);
    		}
    		
    		/// <summary>
    		/// SBML Version 1 Level 2 Test
    		/// </summary>
    		[Test] public void TestHopfSimulationV1L2_RungeKutta()
    		{
    			// Hopf model
			MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.level2.xml");
			RunHopfSimulationRK(s);
    		}
    		
    		/// <summary>
    		/// SBML Version 1 Level 2 Test With Euler method
    		/// </summary>
    		[Test] public void TestHopfSimulationV1L2_Euler()
    		{
    			// Hopf model
			MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.level2.xml");
			RunHopfSimulationEuler(s);
    		}
    		
    		/// <summary>
    		/// SBML Version 1 Level 2 Test With CVODE BDF_Newton method
    		/// </summary>
    		[Test] public void TestHopfSimulationV1L2_CVODE()
    		{
    			// Hopf model
			MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.level2.xml");
			RunHopfSimulationCVODE(s);
    		}


		[Test] public void TestGenerateTimeSeries()
		{
			// simulation
			MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
		
			// experiment
			MuCell.Model.Experiment experiment = new MuCell.Model.Experiment("experiment1");
		
			// Hopf model
			MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.level2.xml");
			List<MuCell.Model.SBML.Model> models = new List<MuCell.Model.SBML.Model>();
			models.Add(s.model);
		
			// Cell definition
			MuCell.Model.CellDefinition celldef1 = new MuCell.Model.CellDefinition("celldef1");
			celldef1.addSBMLModel(s.model);
			
			experiment.addCellDefinition(celldef1);
			
			// Cells
			List<MuCell.Model.CellInstance> cells = new List<MuCell.Model.CellInstance>();
			
			for(int i=0;i<5;i++)
			{
				cells.Add(celldef1.createCell());
			}
				
			// StateSnapshot for intial state
			MuCell.Model.StateSnapshot initialState = new MuCell.Model.StateSnapshot(cells);
			MuCell.Model.Vector3 size = new MuCell.Model.Vector3(1, 1, 1);
			initialState.SimulationEnvironment = new MuCell.Model.Environment(size);
			
			// trime series
			MuCell.Model.TimeSeries ts1 = new MuCell.Model.TimeSeries("Average X", "X/celldef1", 0.01001d);
			ts1.Initialize(models, experiment, simulation);
			
			MuCell.Model.TimeSeries ts2 = new MuCell.Model.TimeSeries("Total X", "X", 0.01001d);
			ts2.Initialize(models, experiment, simulation);
			
			// Parameters
			MuCell.Model.SimulationParameters parameters = new MuCell.Model.SimulationParameters();
			
			parameters.TimeSeries.Add(ts1);
			parameters.TimeSeries.Add(ts2);
			
			parameters.InitialState = initialState;
			parameters.SimulationLength = 0.04004d;
			parameters.SnapshotInterval = 1;
			parameters.StepTime = 0.01001d;
			
			parameters.RelativeTolerance = 1E-8;
			parameters.SolverMethod = MuCell.Model.Solver.SolverMethods.RungeKutta;
			
			// Simulation
			simulation.Parameters = parameters;

			// Start simulation
			simulation.StartSimulation();
			
			// Now check the results
			
			Assert.AreEqual(5, ts1.Series.Count);
			Assert.AreEqual(5, ts2.Series.Count);
			
			double[] ts = ts1.Series.ToArray();
			double[] tst = ts2.Series.ToArray();

			Assert.AreEqual(2.5d, ts[0]);
			AssertDouble.AreEqual(rk_x_t1, ts[1]);
			AssertDouble.AreEqual(rk_x_t2, ts[2]);
			AssertDouble.AreEqual(rk_x_t3, ts[3]);
			AssertDouble.AreEqual(rk_x_t4, ts[4]);
			
			Assert.AreEqual(5*2.5d, tst[0]);
			AssertDouble.AreEqual(5*rk_x_t1, tst[1]);
			AssertDouble.AreEqual(5*rk_x_t2, tst[2]);
			AssertDouble.AreEqual(5*rk_x_t3, tst[3]);
			AssertDouble.AreEqual(5*rk_x_t4, tst[4]);
		}

        [Test]
        public void InvalidTimeSeriesEquation()
        {
            // simulation
			MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
		
			// experiment
			MuCell.Model.Experiment experiment = new MuCell.Model.Experiment("experiment1");
		
			// Hopf model
			MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.level2.xml");
			List<MuCell.Model.SBML.Model> models = new List<MuCell.Model.SBML.Model>();
			models.Add(s.model);
			
			// Cell definition
			MuCell.Model.CellDefinition celldef1 = new MuCell.Model.CellDefinition("celldef1");
			celldef1.addSBMLModel(s.model);
			
			experiment.addCellDefinition(celldef1);
			
			// Cells
			List<MuCell.Model.CellInstance> cells = new List<MuCell.Model.CellInstance>();
			
			for(int i=0;i<5;i++)
			{
				cells.Add(celldef1.createCell());
			}
				
			// StateSnapshot for intial state
			MuCell.Model.StateSnapshot initialState = new MuCell.Model.StateSnapshot(cells);
			MuCell.Model.Vector3 size = new MuCell.Model.Vector3(1, 1, 1);
			initialState.SimulationEnvironment = new MuCell.Model.Environment(size);

            // Parameters
            simulation.Parameters.InitialState = initialState;
            simulation.Parameters.SimulationLength = 3;
            simulation.Parameters.SnapshotInterval = 1;
            simulation.Parameters.StepTime = 1;

            // Time series
            MuCell.Model.TimeSeries ts1 = new MuCell.Model.TimeSeries("Average A", "(A)", 1.0);
            ts1.Initialize(models, experiment, simulation);

        }
        
        public void RunHopfSimulationRK(MuCell.Model.SBML.Reader.SBMLReader s) {
			
			// Cell definition
			MuCell.Model.CellDefinition celldef1 = new MuCell.Model.CellDefinition("celldef1");
			celldef1.addSBMLModel(s.model);
			
			// Cells
			List<MuCell.Model.CellInstance> cells = new List<MuCell.Model.CellInstance>();
			MuCell.Model.CellInstance cell1 = celldef1.createCell();
			cells.Add(cell1);
				
			// StateSnapshot for intial state
			MuCell.Model.StateSnapshot initialState = new MuCell.Model.StateSnapshot(cells);
			MuCell.Model.Vector3 size = new MuCell.Model.Vector3(1, 1, 1);
			initialState.SimulationEnvironment = new MuCell.Model.Environment(size);

			// Parameters
			MuCell.Model.SimulationParameters parameters = new MuCell.Model.SimulationParameters();
			parameters.InitialState = initialState;
			parameters.SimulationLength = 0.01001d;
			parameters.SnapshotInterval = 2;
			parameters.StepTime = 0.01001d;

			parameters.SolverMethod = MuCell.Model.Solver.SolverMethods.RungeKutta;
						
			// Simulation
			MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
			simulation.Parameters = parameters;
			
			// Assert that is not complete at all
			Assert.AreEqual(0.0d, simulation.SimulationProgress());

			// 	Test the initial conditions
			Assert.AreEqual(1.0d, cell1.getSpeciesAmount("A"));
			Assert.AreEqual(2.5d, cell1.getSpeciesAmount("X"));
			Assert.AreEqual(2.5d, cell1.getSpeciesAmount("Y"));
			Assert.AreEqual(2.5d, cell1.getSpeciesAmount("Z"));
			Assert.AreEqual(0.0d, cell1.getSpeciesAmount("_void_"));
			
			// Start simulation
			simulation.StartSimulation();
			
			// Assert that is complete
			Assert.AreEqual(1.0d, simulation.SimulationProgress());
			
			// 	Test the results (test generated from the Python model)
			AssertDouble.AreEqual(1.0d, cell1.getSpeciesAmount("A"));
			AssertDouble.AreEqual(rk_x_t1, cell1.getSpeciesAmount("X"));
			AssertDouble.AreEqual(rk_y_t1, cell1.getSpeciesAmount("Y"));
			AssertDouble.AreEqual(rk_z_t1, cell1.getSpeciesAmount("Z"));
			Assert.AreEqual(0.0d, cell1.getSpeciesAmount("_void_"));
			
			// Make the simulation run a bit longer
			parameters.SimulationLength = 0.02002;
			simulation.UnPauseSimulation();
			
			AssertDouble.AreEqual(1.0d, cell1.getSpeciesAmount("A"));
			AssertDouble.AreEqual(rk_x_t2, cell1.getSpeciesAmount("X"));
			AssertDouble.AreEqual(rk_y_t2, cell1.getSpeciesAmount("Y"));
			AssertDouble.AreEqual(rk_z_t2, cell1.getSpeciesAmount("Z"));
			Assert.AreEqual(0.0d, cell1.getSpeciesAmount("_void_"));

			// Make the simulation run a bit longer
			parameters.SimulationLength = 0.03003;
			simulation.UnPauseSimulation();
			
			AssertDouble.AreEqual(1.0d, cell1.getSpeciesAmount("A"));
			AssertDouble.AreEqual(rk_x_t3, cell1.getSpeciesAmount("X"));
			AssertDouble.AreEqual(rk_y_t3, cell1.getSpeciesAmount("Y"));
			AssertDouble.AreEqual(rk_z_t3, cell1.getSpeciesAmount("Z"));
			Assert.AreEqual(0.0d, cell1.getSpeciesAmount("_void_"));
		}
    
    		public void RunHopfSimulationEuler(MuCell.Model.SBML.Reader.SBMLReader s) {
			
			// Cell definition
			MuCell.Model.CellDefinition celldef1 = new MuCell.Model.CellDefinition("celldef1");
			celldef1.addSBMLModel(s.model);
			
			// Cells
			List<MuCell.Model.CellInstance> cells = new List<MuCell.Model.CellInstance>();
			MuCell.Model.CellInstance cell1 = celldef1.createCell();
			cells.Add(cell1);
				
			// StateSnapshot for intial state
			MuCell.Model.StateSnapshot initialState = new MuCell.Model.StateSnapshot(cells);
			MuCell.Model.Vector3 size = new MuCell.Model.Vector3(1, 1, 1);
			initialState.SimulationEnvironment = new MuCell.Model.Environment(size);

			// Parameters
			MuCell.Model.SimulationParameters parameters = new MuCell.Model.SimulationParameters();
			parameters.InitialState = initialState;
			parameters.SimulationLength = 0.01001d;
			parameters.SnapshotInterval = 2;
			parameters.StepTime = 0.01001d;

			parameters.SolverMethod = MuCell.Model.Solver.SolverMethods.Euler;
						
			// Simulation
			MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
			simulation.Parameters = parameters;
			
			// Assert that is not complete at all
			Assert.AreEqual(0.0d, simulation.SimulationProgress());

			// 	Test the initial conditions
			Assert.AreEqual(1.0d, cell1.getSpeciesAmount("A"));
			Assert.AreEqual(2.5d, cell1.getSpeciesAmount("X"));
			Assert.AreEqual(2.5d, cell1.getSpeciesAmount("Y"));
			Assert.AreEqual(2.5d, cell1.getSpeciesAmount("Z"));
			Assert.AreEqual(0.0d, cell1.getSpeciesAmount("_void_"));
			
			// Start simulation
			simulation.StartSimulation();
			
			// Assert that is complete
			Assert.AreEqual(1.0d, simulation.SimulationProgress());
			
			// 	Test the results (test generated from the Python model)
			AssertDouble.AreEqual(1.0d, cell1.getSpeciesAmount("A"));
			AssertDouble.AreEqual(euler_x_t1, cell1.getSpeciesAmount("X"));
			AssertDouble.AreEqual(euler_y_t1, cell1.getSpeciesAmount("Y"));
			AssertDouble.AreEqual(euler_z_t1, cell1.getSpeciesAmount("Z"));
			Assert.AreEqual(0.0d, cell1.getSpeciesAmount("_void_"));
			
			// Make the simulation run a bit longer
			parameters.SimulationLength = 0.02002;
			simulation.UnPauseSimulation();
			
			AssertDouble.AreEqual(1.0d, cell1.getSpeciesAmount("A"));
			AssertDouble.AreEqual(euler_x_t2, cell1.getSpeciesAmount("X"));
			AssertDouble.AreEqual(euler_y_t2, cell1.getSpeciesAmount("Y"));
			AssertDouble.AreEqual(euler_z_t2, cell1.getSpeciesAmount("Z"));
			Assert.AreEqual(0.0d, cell1.getSpeciesAmount("_void_"));

			// Make the simulation run a bit longer
			parameters.SimulationLength = 0.03003;
			simulation.UnPauseSimulation();
			
			AssertDouble.AreEqual(1.0d, cell1.getSpeciesAmount("A"));
			AssertDouble.AreEqual(euler_x_t3, cell1.getSpeciesAmount("X"));
			AssertDouble.AreEqual(euler_y_t3, cell1.getSpeciesAmount("Y"));
			AssertDouble.AreEqual(euler_z_t3, cell1.getSpeciesAmount("Z"));
			Assert.AreEqual(0.0d, cell1.getSpeciesAmount("_void_"));
		}
    
		public void RunHopfSimulationCVODE(MuCell.Model.SBML.Reader.SBMLReader s) {
			
			// Cell definition
			MuCell.Model.CellDefinition celldef1 = new MuCell.Model.CellDefinition("celldef1");
			celldef1.addSBMLModel(s.model);
			
			// Cells
			List<MuCell.Model.CellInstance> cells = new List<MuCell.Model.CellInstance>();
			MuCell.Model.CellInstance cell1 = celldef1.createCell();
			cells.Add(cell1);
				
			// StateSnapshot for intial state
			MuCell.Model.StateSnapshot initialState = new MuCell.Model.StateSnapshot(cells);
			MuCell.Model.Vector3 size = new MuCell.Model.Vector3(1, 1, 1);
			initialState.SimulationEnvironment = new MuCell.Model.Environment(size);

			// Parameters
			MuCell.Model.SimulationParameters parameters = new MuCell.Model.SimulationParameters();
			parameters.InitialState = initialState;
			parameters.SimulationLength = 0.01001d;
			parameters.SnapshotInterval = 2;
			parameters.StepTime = 0.01001d;

			parameters.RelativeTolerance = 1E-8;
			parameters.SolverMethod = MuCell.Model.Solver.SolverMethods.CVode_Adams_Moulton;
			parameters.CvodeType = 1;
			
			// Simulation
			MuCell.Model.Simulation simulation = new MuCell.Model.Simulation("simulation1");
			simulation.Parameters = parameters;
			
			// Assert that is not complete at all
			Assert.AreEqual(0.0d, simulation.SimulationProgress());

			// 	Test the initial conditions
			Assert.AreEqual(1.0d, cell1.getSpeciesAmount("A"));
			Assert.AreEqual(2.5d, cell1.getSpeciesAmount("X"));
			Assert.AreEqual(2.5d, cell1.getSpeciesAmount("Y"));
			Assert.AreEqual(2.5d, cell1.getSpeciesAmount("Z"));
			Assert.AreEqual(0.0d, cell1.getSpeciesAmount("_void_"));
			
			// Start simulation
			simulation.StartSimulation();
			
			// Assert that is complete
			Assert.AreEqual(1.0d, simulation.SimulationProgress());
			
			// 	Test the results (test generated from the Python model)
			AssertDouble.AreEqual(1.0d, cell1.getSpeciesAmount("A"));
			AssertDouble.AreEqual(cvode_x_t1, cell1.getSpeciesAmount("X"));
			AssertDouble.AreEqual(cvode_y_t1, cell1.getSpeciesAmount("Y"));
			AssertDouble.AreEqual(cvode_z_t1, cell1.getSpeciesAmount("Z"));
			Assert.AreEqual(0.0d, cell1.getSpeciesAmount("_void_"));
			
			// Make the simulation run a bit longer
			parameters.SimulationLength = 0.02002;
			simulation.UnPauseSimulation();
			
			AssertDouble.AreEqual(1.0d, cell1.getSpeciesAmount("A"));
			AssertDouble.AreEqual(cvode_x_t2, cell1.getSpeciesAmount("X"));
			AssertDouble.AreEqual(cvode_y_t2, cell1.getSpeciesAmount("Y"));
			AssertDouble.AreEqual(cvode_z_t2, cell1.getSpeciesAmount("Z"));
			Assert.AreEqual(0.0d, cell1.getSpeciesAmount("_void_"));

			// Make the simulation run a bit longer
			parameters.SimulationLength = 0.03003;
			simulation.UnPauseSimulation();
			
			AssertDouble.AreEqual(1.0d, cell1.getSpeciesAmount("A"));
			AssertDouble.AreEqual(cvode_x_t3, cell1.getSpeciesAmount("X"));
			AssertDouble.AreEqual(cvode_y_t3, cell1.getSpeciesAmount("Y"));
			AssertDouble.AreEqual(cvode_z_t3, cell1.getSpeciesAmount("Z"));
			Assert.AreEqual(0.0d, cell1.getSpeciesAmount("_void_"));
		}
		

	}
}