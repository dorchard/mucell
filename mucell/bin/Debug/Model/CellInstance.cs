using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

using MuCell.Model.SBML;

namespace MuCell.Model
{

    public delegate double CellEvaluationFunction(StateSnapshot snapshot, CellInstance cell);
    public delegate void EffectReactionEvaluationFunction(StateSnapshot snapshot, CellInstance cell);
    public delegate double AggregateEvaluationFunction(StateSnapshot snapshot);

    public class CellInstance : ICloneable
    {
        //hack for testing chemo
        public SBML.ExtracellularComponents.FlagellaComponent flaggy;

        // Dictionaries to map species to their model indices
        // These exist to be faster than doing inverse dictionary lookups
        private Dictionary<string, int> speciesToIndexMap;
        private Dictionary<int, string> indexToSpeciesMap;
        private int maxIndex;

        private Solver.SolverBase solver;
        private StateSnapshot currentState;

        private List<SBML.Species> species;
        private Dictionary<string, double> speciesVariables;

        [XmlIgnore]
        public Dictionary<string, double> localSpeciesVariables;
        [XmlIgnore]
        public Dictionary<string, double> localSpeciesDelta;

        [XmlIgnore]
        private List<SBML.ExtracellularComponents.ComponentWorldStateBase> components;
        public List<SBML.ExtracellularComponents.ComponentWorldStateBase> Components
        {
            get { return components; }
            set { components = value; }
        }

        private List<EffectReactionEvaluationFunction> reactions;

        private int N;

        // Used for random behaviours in components
        private System.Random randomObject;

        /// <summary>
        /// Identifies a cell to a group
        /// </summary>
        /// <param name="cellDefinition">
        /// A <see cref="CellDefinition"/>
        /// </param>
        private int groupID;
        [XmlAttribute]
        public int GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }

        private CellDefinition cellInstanceDefinition;
        public CellDefinition CellInstanceDefinition
        {
            get { return cellInstanceDefinition; }
            set { cellInstanceDefinition = value; }
        }

        private SpatialContext cellInstanceSpatialContext;
        public SpatialContext CellInstanceSpatialContext
        {
            get { return cellInstanceSpatialContext; }
            set { cellInstanceSpatialContext = value; }
        }

        public CellInstance()
        {
            this.components = new List<MuCell.Model.SBML.ExtracellularComponents.ComponentWorldStateBase>();

        }

        /// <summary>
        /// Constructor for creating a cell instance with a cell definition
        /// </summary>
        /// <param name="cellDefinition">
        /// A <see cref="CellDefinition"/>
        /// </param>
        public CellInstance(CellDefinition cellDefinition)
        {
            this.cellInstanceDefinition = cellDefinition;
            SBML.Model model = cellDefinition.getModel();
            if (model != null)
            {
                this.species = cellDefinition.getModel().listOfSpecies;
            }
            // Initialize the specials Dictionary
            this.speciesVariables = new Dictionary<string, double>();

            this.localSpeciesVariables = new Dictionary<string, double>();
            this.localSpeciesDelta = new Dictionary<string, double>();

            this.speciesToIndexMap = new Dictionary<string, int>();
            this.indexToSpeciesMap = new Dictionary<int, string>();

            // set up the list
            this.convertSpeciesListToVariables();

            this.components = new List<MuCell.Model.SBML.ExtracellularComponents.ComponentWorldStateBase>();


            this.flaggy = new MuCell.Model.SBML.ExtracellularComponents.FlagellaComponent();

            cellInstanceSpatialContext = new SpatialContext();
        }


        /// <summary>
        /// Clones a CellInstance
        /// </summary>
        /// <returns>
        /// A <see cref="Object"/>
        /// </returns>
        public Object Clone()
        {
            // Clone the component world states
            List<SBML.ExtracellularComponents.ComponentWorldStateBase> clonedComponents = new List<SBML.ExtracellularComponents.ComponentWorldStateBase>();
            foreach (SBML.ExtracellularComponents.ComponentWorldStateBase componentWorldState in this.components)
            {
                clonedComponents.Add((SBML.ExtracellularComponents.ComponentWorldStateBase)componentWorldState.Clone());
            }

            // create the objects
            CellInstance cloned = new CellInstance(this.cellInstanceDefinition, (SpatialContext)this.cellInstanceSpatialContext.Clone(),
                this.species, this.speciesVariables, this.indexToSpeciesMap, this.speciesToIndexMap,
                this.solver, clonedComponents, this.randomObject);
            // Set reactions
            cloned.reactions = this.reactions;
            return cloned;
        }

        /// <summary>
        /// Secondary constructor that takes a cell definition,
        /// a spatial context, a list of species and a species variables dictionary
        /// </summary>
        /// <param name="cellDefinition">
        /// A <see cref="CellDefinition"/>
        /// </param>
        /// <param name="spatialContext">
        /// A <see cref="SpatialContext"/>
        /// </param>
        /// <param name="species">
        /// A <see cref="List`1"/>
        /// </param>
        /// <param name="speciesVariables">
        /// A <see cref="Dictionary`2"/>
        /// </param>
        public CellInstance(CellDefinition cellDefinition, SpatialContext spatialContext,
                List<SBML.Species> species, Dictionary<string, double> speciesVariables,
                Dictionary<int, string> indexToSpeciesMap, Dictionary<string, int> speciesToIndexMap,
                Solver.SolverBase solver, List<SBML.ExtracellularComponents.ComponentWorldStateBase> components,
                System.Random random)
        {
            this.cellInstanceDefinition = cellDefinition;
            this.species = species;
            this.speciesVariables = speciesVariables;
            this.cellInstanceSpatialContext = spatialContext;
            this.indexToSpeciesMap = indexToSpeciesMap;
            this.speciesToIndexMap = speciesToIndexMap;
            this.solver = solver;
            this.components = components;
            this.randomObject = random;

            this.localSpeciesVariables = new Dictionary<string, double>();
            this.localSpeciesDelta = new Dictionary<string, double>();

        }

        /// <summary>
        /// Sets the random object for a cell instance
        /// </summary>
        /// <param name="random"></param>
        public void SetRandomObject(System.Random random)
        {
            this.randomObject = random;
        }

        /// <summary>
        /// Return the random object for a cell instance
        /// </summary>
        /// <returns></returns>
        public System.Random GetRandomObject()
        {
            return this.randomObject;
        }

        /// <summary>
        /// Resets the concentrations in the cells to their initial values
        /// </summary>
        public void ResetCellConcentrations(StateSnapshot initialState)
        {
            foreach (Species s in this.species)
            {
                this.speciesVariables[s.ID] = s.initialValue;
            }


        }

        /// <summary>
        /// CellModelling funcion
        /// </summary>
        /// <param name="time">
        /// A <see cref="System.Double"/>
        /// </param>
        /// <param name="y">
        /// A <see cref="System.Double"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Double"/>
        /// </returns>
        public double[] ModelDeltaFunction(double time, double[] y)
        {
            int i;
            for (i = 0; i < this.N; i++)
            {
                // Set local species variable
                this.localSpeciesVariables[indexToSpeciesMap[i]] = y[i];
                // Set delta to 0
                this.localSpeciesDelta[indexToSpeciesMap[i]] = 0.0;
            }

            // Evaluate every EffectReactionEvaluationFunctionFunction
            foreach (EffectReactionEvaluationFunction fun in this.reactions)
            {
                fun(this.currentState, this);
            }
            
            double[] ydot = new double[this.N];

            // Get the values out
            for (i = 0; i < this.N; i++)
            {
                // Get delta out
                ydot[i] = this.localSpeciesDelta[indexToSpeciesMap[i]];
            }

            return ydot;
        }


        /// <summary>
        /// Converts a list of SBML Species to a more efficient dictionary of
        /// string->doubles internally
        /// </summary>
        private void convertSpeciesListToVariables()
        {
            // set up the dictionary
            this.speciesVariables = new Dictionary<string, double>();

            // If we have species
            if (this.species != null)
            {

                this.maxIndex = 0;

                // Then add these to the more efficient dictionary format
                foreach (SBML.Species s in this.species)
                {
                    if (!s.BoundaryCondition)
                    {
                        // Update index<->species maps
                        indexToSpeciesMap[maxIndex] = s.ID;
                        speciesToIndexMap[s.ID] = maxIndex;
                        maxIndex++;

                    }

                    this.speciesVariables.Add(s.ID, s.initialValue);

                }
            }
        }

        /// <summary>
        /// Gets an amount for a special variable belong to the cell
        /// </summary>
        /// <param name="species">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.double"/>
        /// </returns>
        public double getSpeciesAmount(string species)
        {
            if (this.speciesVariables.ContainsKey(species))
            {
                return this.speciesVariables[species];
            }
            else
            {
                return 0.0d;
            }
        }

        /// <summary>
        /// Gets the local simulation species amount
        /// </summary>
        /// <param name="species">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Double"/>
        /// </returns>
        public double getLocalSimulationSpeciesAmount(string species)
        {
            if (this.localSpeciesVariables.ContainsKey(species))
            {
                return this.localSpeciesVariables[species];
            }
            else
            {
                return 0.0d;
            }
        }

        /// <summary>
        /// Given a species sums up the species amounts from the local sim variables
        /// This is used mainly for testing
        /// </summary>
        /// <param name="species">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Double"/>
        /// </returns>
        public double UnitTestGetSpeciesAmountsAndDeltas(string species)
        {
            if (this.localSpeciesVariables.ContainsKey(species))
            {
                return this.localSpeciesVariables[species] + this.localSpeciesDelta[species];
            }
            else
            {
                return 0.0d;
            }
        }

        /// <summary>
        /// Used fo testing
        /// </summary>
        public void UnitTestClearDeltas()
        {
            foreach (Species s in this.species)
            {
                if (!s.BoundaryCondition)
                {
                    // Set delta to 0
                    this.localSpeciesDelta[s.ID] = 0.0;
                }
            }
        }

        /// <summary>
        /// Sets an amount for a species variable belonging to the cell
        /// </summary>
        /// <param name="species">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="amount">
        /// A <see cref="System.double"/>
        /// </param>
        public void setSpeciesAmount(string species, double amount)
        {
            if (this.speciesVariables.ContainsKey(species))
            {
                this.speciesVariables[species] = amount;
                this.localSpeciesVariables[species] = amount;
            }
            else
            {
                // <todo>Confirm if we want this behaviour, if so this can be refactored</todo>
                // If we don't have the species variable, then add it
                this.speciesVariables[species] = amount;
                this.localSpeciesVariables[species] = amount;
            }
        }

        /// <summary>
        /// Allows one to set the value of a species in the middle of a simulation - use by the components
        /// </summary>
        /// <param name="species"></param>
        /// <param name="amount"></param>
        public void setSpeciesAmountInSimulation(string species, double amount)
        {
            if (this.speciesVariables.ContainsKey(species))
            {
                this.solver.SetVectorIndex(speciesToIndexMap[species], amount);
            }
        }

        /// <summary>
        /// Set the list of rections within the cell as a list EffectReactionEvaluationFunction
        /// usually set by the CellDefinition.createCell method.
        /// </summary>
        /// <param name="reactions">
        /// A <see cref="List`1"/>
        /// </param>
        public void setReactions(List<EffectReactionEvaluationFunction> reactions)
        {
            this.reactions = reactions;
        }

        /// <summary>
        /// Initializes a cell model for simulation solving
        /// </summary>
        /// <param name="solverMethod">
        /// A <see cref="Solver.SolverMethods"/>
        /// </param>
        /// <param name="parameters">
        /// A <see cref="SimulationParameters"/>
        /// </param>
        public unsafe void InitializeCellModel(Solver.SolverMethods solverMethod, SimulationParameters parameters)
        {

            // Find out number of variable species
            int n = 0;
            foreach (Species s in this.species)
            {
                if (!s.BoundaryCondition)
                {
                    n++;
                }
            }

            this.N = n;

            // Cvode method
            if (solverMethod == Solver.SolverMethods.Cvode)
            {

                double[] values = new double[n];
                double[] absoluteTolerances = new double[n];

                // Set initial values
                foreach (Species s in this.species)
                {
                    if (!s.BoundaryCondition)
                    {
                        // collect the initial values
                        values[speciesToIndexMap[s.ID]] = s.initialValue;
                        // collect the absolute tolerances
                        absoluteTolerances[speciesToIndexMap[s.ID]] = s.AbsoluteTolerance;
                    }
                }

                // Create CVODE interface object
                this.solver = new Solver.CVode(n, values,
                    new Solver.CVode.ModelFunction(delegate(double time, IntPtr y, IntPtr ydot, IntPtr fdata)
                    {
                        double* yp = (double*)y.ToPointer();
                        double* ydotp = (double*)ydot.ToPointer();
                        int i;

                        double[] data = new double[this.N];

                        for (i = 0; i < this.N; i++)
                        {
                            data[i] = yp[i];
                        }

                        double[] result = this.ModelDeltaFunction(time, data);

                        // Get the values out
                        for (i = 0; i < this.N; i++)
                        {
                            ydotp[i] = result[i];
                        }

                        return 0;
                    }),
                        absoluteTolerances,
                        parameters.RelativeTolerance,
                        parameters.CvodeType);
            }
            else if (solverMethod == Solver.SolverMethods.Euler)
            {
                double[] values = new double[n];

                // Set initial values
                foreach (Species s in this.species)
                {
                    if (!s.BoundaryCondition)
                    {
                        // collect the initial values
                        values[speciesToIndexMap[s.ID]] = s.initialValue;
                    }
                }

                this.solver = new Solver.Euler(n, values, this.ModelDeltaFunction);
            }
            else if (solverMethod == Solver.SolverMethods.RungeKutta)
            {

                double[] values = new double[n];

                // Set initial values
                foreach (Species s in this.species)
                {
                    if (!s.BoundaryCondition)
                    {
                        // collect the initial values
                        values[speciesToIndexMap[s.ID]] = s.initialValue;
                    }
                }

                this.solver = new Solver.RungeKutta(n, values, this.ModelDeltaFunction);

            }
        }

        /// <summary>
        /// Release the CVODE model
        /// </summary>
        public void ReleaseCellModel()
        {
            if (this.solver != null)
            {
                this.solver.Release();
            }
        }

        /// <summary>
        /// Allows all reactions within the cell to run for the current timestep
        /// </summary>
        /// <param name="currentState">
        /// A <see cref="StateSnapshot"/>
        /// </param>
        /// <param name="time">
        /// A <see cref="System.Double"/>
        /// </param>
        /// <param name="stepTime">
        /// A <see cref="System.Double"/>
        /// </param>
        public void DoTimeStep(StateSnapshot currentState, double time, double stepTime)
        {
            // set the current state
            this.currentState = currentState;
            
            // Do the component simulations
            foreach (SBML.ExtracellularComponents.ComponentWorldStateBase component in this.components)
            {
                component.DoTimeStep(this, currentState, time, stepTime);
            }

            // Step the cvode module
            double endTime = this.solver.OneStep(time, stepTime);

            // get values off vector
            for (int i = 0; i < this.speciesToIndexMap.Count; i++)
            {
                this.speciesVariables[indexToSpeciesMap[i]] = this.solver.GetValue(i);
            }

        }

        public bool exptEquals(CellInstance other)
        {
            if (this.GroupID != other.GroupID)
            {
                Console.Write("CellInstance objects not equal: ");
                Console.WriteLine("this.GroupID='" + this.GroupID + "'; other.GroupID='" + other.GroupID);
                return false;
            }
            if (this.CellInstanceDefinition.exptEquals(other.CellInstanceDefinition) == false)
            {
                Console.Write("CellInstance objects not equal: ");
                Console.Write("this.CellInstanceDefinition.Name='" + this.CellInstanceDefinition.Name);
                Console.WriteLine("'; other.CellInstanceDefinition.Name'" + other.CellInstanceDefinition.Name);
                return false;
            }
            if (this.CellInstanceSpatialContext.exptEquals(other.CellInstanceSpatialContext) == false)
            {
                Console.Write("CellInstance objects not equal: ");
                Console.WriteLine("this.CellInstanceSpatialContext != other.CellInstanceSpatialContext");
                return false;
            }

            return true;
        }
    }
}
