using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model
{
    public class Experiment : IExperimentController
    {

        // List of cell definitions associated to the experiment
        private List<CellDefinition> cellDefinitions;
        [XmlArray]
        public List<CellDefinition> CellDefinitions { get { return cellDefinitions; } set { cellDefinitions = value; } }

        // List of simulations association with the experiment
        [XmlArray]
        public List<Simulation> simulations;

        // A string uniquely identifying the experiment
        private string name;
        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        private int id;
        [XmlAttribute]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        // A string denoting the file path of the experiment
        private string filePath;

        public Experiment()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/>
        /// </param>
        public Experiment(string name)
        {
            this.name = name;
            // Initialize the definitions and simulations
            this.cellDefinitions = new List<CellDefinition>();
            this.simulations = new List<Simulation>();
        }

        /// <summary>
        /// Saves an experiment
        /// </summary>
        /// <param name="filePath">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool save(string filePath)
        {
            try
            {
                //try autosaving the cell definitions first

                foreach (CellDefinition cell in cellDefinitions)
                {
                    cell.autoSave(this.Name);
                }

                XmlSerializer s = new XmlSerializer(typeof(MuCell.Model.Experiment));
                TextWriter w = new StreamWriter(filePath);
                s.Serialize(w, this);
                w.Close();
                this.filePath = filePath;

                //reset the changed value on all the simulation parameters
                resetAllSimulationChangedValues();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
                throw e;
            }
        }
        void resetAllSimulationChangedValues()
        {
            foreach (Simulation s in simulations)
            {
                if (s.Parameters != null)
                {
                    s.Parameters.resetChangedValue();
                }
            }
        }
        public String getLastSavedPath()
        {
            return filePath;
        }

        public static Experiment load(string filePath)
        {
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(MuCell.Model.Experiment));
                TextReader tr = new StreamReader(filePath);
                Experiment deserializedExpt = (Experiment)s.Deserialize(tr);
                tr.Close();

                deserializedExpt.filePath = filePath;
                deserializedExpt.resetAllSimulationChangedValues();
                return deserializedExpt;
            }
            catch (Exception e)
            {
                Console.WriteLine("Experiment with filename " + filePath + " could not be loaded.");

                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
                throw e;             
            }       
        }
        

        /// <summary>
        /// Add a cell definition to the experiment
        /// </summary>
        /// <param name="cellDefinition">
        /// A <see cref="CellDefinition"/>
        /// </param>
        public void addCellDefinition(CellDefinition cellDefinition)
        {
            this.cellDefinitions.Add(cellDefinition);
        }

        /// <summary>
        /// Removes a cell definition from the experiment
        /// </summary>
        /// <param name="cellDefinition">
        /// A <see cref="CellDefinition"/>
        /// </param>
        public void removeCellDefinition(CellDefinition cellDefinition)
        {
            this.cellDefinitions.Remove(cellDefinition);
        }
		
		/// <summary>
		/// Returns an array of the cell definitions
		/// </summary>
		/// <returns>
		/// A <see cref="CellDefinition"/> array
		/// </returns>
		public CellDefinition[] getCellDefinitions()
		{
			return this.cellDefinitions.ToArray();
		}
		
		/// <summary>
		/// Tests whether an experiment contains a certain CellDefinition from a name
		/// </summary>
		/// <param name="id">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool ContainsCellDefinition(string name)
		{
            // replace any spaces in the lookup with underscores
            name = name.Replace(' ', '_');

			// Foreach celldefinition
			foreach(CellDefinition celldef in this.cellDefinitions)
			{
				// If we have a matching id, then return true, replace spaces with underscores
				if (celldef.Name.Replace(' ', '_') == name)
				{
					return true;
				}
			}
			return false;
		}
		
		/// <summary>
		/// Gets a certain CellDefinition from an experiment
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="CellDefinition"/>
		/// </returns>
		public CellDefinition GetCellDefinition(string name)
		{
            name = name.Replace(' ', '_');

			// Foreach celldefinition
			foreach(CellDefinition celldef in this.cellDefinitions)
			{
				// If we have a matching id, then return true
				if (celldef.Name.Replace(' ', '_') == name)
				{
					return celldef;
				}
			}
			return null;
		}
		
		/// <summary>
		/// Add a simulation to the experiment
		/// </summary>
		/// <param name="simulation">
		/// A <see cref="Simulation"/>
		/// </param>
		public void addSimulation(Simulation simulation)
		{
			this.simulations.Add(simulation);		
		}

        public void removeSimulation(Simulation simulation)
        {
            this.simulations.Remove(simulation);
        }

        /// <summary>
        /// Returns an array of the simulations
        /// </summary>
        /// <returns>
        /// A <see cref="Simulation"/> array
        /// </returns>
        public Simulation[] getSimulations()
        {
            return this.simulations.ToArray();
        }

        public bool exptEquals(Experiment other)
        {
            // check Name and Id
            if (this.Name != other.Name)
            {
                Console.Write("Experiment objects not equal: ");
                Console.WriteLine("this.Name='" + this.Name + "'; other.Name='" + other.Name);
                return false;
            }
            if (this.Id != other.Id) 
            {
                Console.Write("Experiment objects not equal: ");
                Console.WriteLine("this.id='" + this.Id + "'; other.Id='" + other.Id);
                return false; 
            }
            // check CellDefinitions
            try
            {
                for (int i = 0; i < this.cellDefinitions.Count; i++)
                {
                    if (this.cellDefinitions[i].exptEquals(other.cellDefinitions[i]) == false)
                    {
                        Console.Write("Experiment objects not equal: ");
                        Console.Write("this.cellDefinitions[" + i + "].Name='" + this.cellDefinitions[i].Name);
                        Console.WriteLine("'; other.cellDefinitions[" + i + "].Name'" + other.cellDefinitions[i].Name);
                        return false;
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                // Array out of bounds; lists are unequal
                Console.Write("Experiment objects not equal: ");
                Console.Write("this.cellDefinitions.Count = "+this.cellDefinitions.Count);
                Console.WriteLine("; other.cellDefinitions.Count = " + other.cellDefinitions.Count);
                return false;
            }
            try
            {
                // check Simulations
                for (int i = 0; i < this.simulations.Count; i++)
                {
                    if (this.simulations[i].exptEquals(other.simulations[i]) == false)
                    {
                        Console.Write("Experiment objects not equal: ");
                        Console.WriteLine("this.simulations[" + i + "] != other.simulations[" + i + "]");
                        return false;
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                // Array out of bounds; lists are unequal
                Console.Write("Experiment objects not equal: ");
                Console.Write("this.simulations.Count = " + this.simulations.Count);
                Console.WriteLine("; other.simulations.Count = " + other.simulations.Count);
                return false;
            }
            
            return true;
        }
    }
}
