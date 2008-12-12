/* Cathy Young
 * 
 * Classes representing a whole SBML model
 * Latest SBML spec: http://belnet.dl.sourceforge.net/sourceforge/sbml/sbml-level-2-version-3-rel-1.pdf
 */

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MuCell.Model.SBML
{

	/// <summary>
	/// DuplicateSBMLObjectId Exception is raised when objects with duplicate IDs are found in a model
	/// that also have non-identical data, therefore are not "data-duplicates" which are usually ignored.
	/// </summary>
	class DuplicateSBMLObjectIdException : Exception
	{
		public DuplicateSBMLObjectIdException(string duplicateId)
		{
			System.Console.WriteLine("An SBML object with a duplicate id - "+duplicateId+" - and non-identical data was found in the model");
		}
	}

    /// <summary>
    /// Top-level container object for the model
    /// </summary>
    [XmlRoot("model")]
    public class Model : SBase
    {
        [XmlArray()]
        [XmlArrayItem("functionDefinition")]
        public List<FunctionDefinition> listOfFunctionDefinitions;
        [XmlArray()]
        [XmlArrayItem("unitDefinition")]
        public List<UnitDefinition> listOfUnitDefinitions;
        [XmlArray()]
        [XmlArrayItem("compartmentType")]
        public List<CompartmentType> listOfCompartmentTypes;
        [XmlArray()]
        [XmlArrayItem("speciesType")]
        public List<SpeciesType> listOfSpeciesTypes;
        [XmlArray()]
        [XmlArrayItem("compartment")]
        public List<Compartment> listOfCompartments;
        [XmlArray()]
        [XmlArrayItem("species")]
        public List<Species> listOfSpecies;
        [XmlArray()]
        [XmlArrayItem("parameter")]
        public List<Parameter> listOfParameters;
        [XmlArray()]
        [XmlArrayItem("initialAssignment")]
        public List<InitialAssignment> listOfInitialAssignments;
        [XmlArray()]
        public List<Rule> listOfRules;
        [XmlArray()]
        [XmlArrayItem("constraint")]
        public List<Constraint> listOfConstraints;
        [XmlArray()]
        [XmlArrayItem("reaction")]
        public List<Reaction> listOfReactions;
        [XmlArray()]
        [XmlArrayItem("event")]
        public List<Event> listOfEvents;

        
        [XmlArray()]
        [XmlArrayItem("extraCellularComponent")]
        public List<ExtracellularComponents.ComponentBase> listOfComponents;


        [XmlIgnore]
        public int level = 2;
        [XmlIgnore]
        public int version = 3;

        /// <summary>
        /// Maintains a list of valid Units (includes built-in units
        /// and also user-defined units)
        /// </summary>
        [XmlIgnore]
        public Hashtable UnitTable;
        
        /// <summary>
        /// Maintains a list of the current IDs found in the SBML document,
        /// and the object they refer to.
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, SBase> IdTable;

        public Model()
        {
            this.UnitTable = new Hashtable();
            this.initializeUnits();
            this.IdTable = new Dictionary<string, SBase>();

            this.listOfSpecies = new List<Species>();
            this.listOfReactions = new List<Reaction>();
            this.listOfParameters = new List<Parameter>();
            this.listOfComponents = new List<MuCell.Model.SBML.ExtracellularComponents.ComponentBase>();
        }

        public Model(string id)
        {
            this.UnitTable = new Hashtable();
            this.initializeUnits();
            this.IdTable = new Dictionary<string,SBase>();

            this.ID = id;
            this.AddId(id, this);

            this.listOfSpecies = new List<Species>();
            this.listOfReactions = new List<Reaction>();
            this.listOfParameters = new List<Parameter>();
            this.listOfComponents = new List<MuCell.Model.SBML.ExtracellularComponents.ComponentBase>();
        }

        private void initializeUnits()
        {
            AddUnit("ampere");
            AddUnit("gram");
            AddUnit("katal");
            AddUnit("metre");
            AddUnit("second");
            AddUnit("watt");
            AddUnit("becquerel");
            AddUnit("gray");
            AddUnit("kelvin");
            AddUnit("mole");
            AddUnit("siemens");
            AddUnit("weber");
            AddUnit("candela");
            AddUnit("henry");
            AddUnit("kilogram");
            AddUnit("newton");
            AddUnit("sievert");
            AddUnit("coulomb");
            AddUnit("hertz");
            AddUnit("litre");
            AddUnit("ohm");
            AddUnit("steradian");
            AddUnit("dimensionless");
            AddUnit("item");
            AddUnit("lumen");
            AddUnit("pascal");
            AddUnit("tesla");
            AddUnit("farad");
            AddUnit("joule");
            AddUnit("lux");
            AddUnit("radian");
            AddUnit("volt");
            AddUnit("length");
            AddUnit("area");
            AddUnit("volume");
        }

        private void AddUnit(string unitName)
        {
            this.UnitTable.Add(unitName, null);
        }

        /// <summary>
        /// Used for adding user-defined units to the global UnitTable
        /// </summary>
        /// <param name="unitName">The name given to the unit</param>
        /// <param name="reference">The UnitDefinition object defining the unit</param>
        public void AddUnit(string unitName, object reference)
        {
            this.UnitTable.Add(unitName, reference);
        }

        /// <summary>
        /// Checks whether a unit referred to is valid in the current document
        /// </summary>
        /// <param name="unitName">The unit name to check</param>
        /// <returns>Boolean</returns>
        public Boolean IsUnits(string unitName)
        {
            if (unitName != null)
            {
                if (this.UnitTable.Contains(unitName))
                {
                    return true;
                }
            }
            return false;
        }

 		/// <summary>
		/// Used when a new ID is found in the SBML document to add it
        /// and its object reference to the global IdTable 
		/// </summary>
		/// <param name="id">The id of the object</param>
		/// <param name="reference">
		/// The SBase <see cref="SBase"/> object with the corresponding id.
		/// </param>        
        public void AddId(string id, SBase reference)
        {
        		// Need to make sure that we don't add a duplicate refernce but add with a slightly different id
        		// and flag as a possible duplicate for pass at the end
        		if (this.IdTable.ContainsKey(id)) 
        		{
        			// Duplicate found
        			// Get the original
        			SBase original = this.IdTable[id];
        			original.duplicateCount++;
        			// Modify duplicates id
        			reference.ID = reference.ID+"_"+original.duplicateCount;
        			// Set the original
        			reference.setHasADuplicateId(original);
        			// Add to the idtable
        			this.IdTable.Add(reference.ID, reference);
        			
        		} else {
				this.IdTable.Add(id, reference);
			}
        }
        
        public void RemoveId(string id)
        {
        		if (this.idExists(id))
        		{
            		this.IdTable.Remove(id);
            	}
        }

        /// <summary>
        /// Returns the object that has the given id.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>The object that has the given id, or null if no such
        /// object exists.</returns>
        public Object findObject(string id)
        {
            // replace spaces for underscores
            id = id.Replace(' ', '_');

            foreach(KeyValuePair<string, SBase> kvp in this.IdTable)
            {
                if (kvp.Key.Replace(' ', '_') == id)
                {
                    return this.IdTable[kvp.Key];
                }
            }
            return null;
        }

		/// <summary>
		/// Determines whether a variable given as a string exsists in the model
		/// </summary>
		/// <param name="id">
		/// A <see cref="String"/>
		/// </param>
		/// <returns>
		/// A <see cref="Boolean"/>
		/// </returns>
        public Boolean idExists(string id)
        {
            if (this.IdTable.ContainsKey(id))
            {
                return true;
            }
            else
            {
                id = id.Replace(' ', '_');

                // Check for underscore version
                foreach (KeyValuePair<string, SBase> kvp in this.IdTable)
                {
                    if (kvp.Key.Replace(' ', '_') == id)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void InterrogateModelForMissingIDs()
        {
            List<Species> speciesToList = new List<Species>(); 
            List<Parameter> parameterToList = new List<Parameter>();

            // Check all species in the list of species
            foreach(Species s in this.listOfSpecies)
            {
                // If the IDtable doesn't contain the ID or its space replaced version
                if (!(this.IdTable.ContainsKey(s.ID) || this.IdTable.ContainsKey(s.ID.Replace(' ', '_'))))
                {
                    // Add the space replaced version
                    this.AddId(s.ID.Replace(' ', '_'), s);
                    speciesToList.Add(s);
                }
            }
            // Check the list of reactions
            foreach(Reaction r in this.listOfReactions)
            {
                // Check paramters
                foreach(Parameter p in r.Parameters)
                {
                    // If not in ID table add
                    if (!this.IdTable.ContainsKey(p.ID))
                    {
                        this.AddId(p.ID, p);
                        parameterToList.Add(p);
                    }
                }
                // Check reactants
                foreach(SpeciesReference sr in r.Reactants)
                {
                    if (!(this.IdTable.ContainsKey(sr.SpeciesID) || this.IdTable.ContainsKey(sr.SpeciesID.Replace(' ', '_'))))
                    {
                        this.AddId(sr.SpeciesID.Replace(' ', '_'), sr.species);
                        speciesToList.Add(sr.species);
                    }
                }
                /// Check products
                foreach (SpeciesReference sr in r.Products)
                {
                    if (!(this.IdTable.ContainsKey(sr.SpeciesID) || this.IdTable.ContainsKey(sr.SpeciesID.Replace(' ', '_'))))
                    {
                        this.AddId(sr.SpeciesID.Replace(' ', '_'), sr.species);
                        speciesToList.Add(sr.species);
                    }
                }
            }

            //this.listOfSpecies;
        }
        
        /// <summary>
        /// Parses the entire model data structure and removes any duplicates that have the same internal data
        /// (these are likely errors created by SBML transcription programs).
        /// Any non-data duplicates, e.g. objects with an id the same as another object, whose internal
        /// data is different raise an exception.
        /// </summary>
        public void processDuplicates() 
        {
        		Dictionary<string, SBase>.ValueCollection values = this.IdTable.Values;
        	    List<string> idsToRemove = new List<string>();
        	        
        		foreach (SBase entity in values)
        		{
        			// If the object is a duplicate
        			if  (entity.hadDuplicateId())
        			{
        				SBase original = entity.getOriginal();
        				// If the data is a original
        				if (original.SBMLEquals(entity))
        				{
        					// Remove the object based on its temporary id that was assigned earlier
        					// Do this by adding it to a list (otherwise we get dictionary out of sync problems)
        					idsToRemove.Add(entity.ID);
        				}
        				else 
        				{
        					// Else they are not data-duplicates, throw exception
       					    throw new DuplicateSBMLObjectIdException(entity.ID);
        				}
        			}
        		}
        		
        		// remove the ids
        		foreach(string ID in idsToRemove)
        		{
        			SBase entity = this.IdTable[ID];
        			
        			// Remove the id from the idtable
        			this.RemoveId(ID);
        			
        			// See if the entity belongs to any externally managed lists in the model and remove
        			if(entity is FunctionDefinition && this.listOfFunctionDefinitions.Contains((FunctionDefinition)entity))
        			{
        				this.listOfFunctionDefinitions.Remove((FunctionDefinition)entity);
				}
				else if (entity is UnitDefinition && this.listOfUnitDefinitions.Contains((UnitDefinition)entity))
				{
					this.listOfUnitDefinitions.Remove((UnitDefinition)entity);
				}
				else if (entity is CompartmentType && this.listOfCompartmentTypes.Contains((CompartmentType)entity))
				{
					this.listOfCompartmentTypes.Remove((CompartmentType)entity);
				}
				else if (entity is SpeciesType && this.listOfSpeciesTypes.Contains((SpeciesType)entity))
				{
					this.listOfSpeciesTypes.Remove((SpeciesType)entity);
				}
				else if (entity is Compartment && this.listOfCompartments.Contains((Compartment)entity))
				{
					this.listOfCompartments.Remove((Compartment)entity);
				}
				else if (entity is Species && this.listOfSpecies.Contains((Species)entity))
				{
					this.listOfSpecies.Remove((Species)entity);
				}
				else if (entity is Parameter && this.listOfParameters.Contains((Parameter)entity))
				{
					this.listOfParameters.Remove((Parameter)entity);
				}
				else if (entity is InitialAssignment && this.listOfInitialAssignments.Contains((InitialAssignment)entity))
				{
					this.listOfInitialAssignments.Remove((InitialAssignment)entity);
				}
				else if (entity is Rule && this.listOfRules.Contains((Rule)entity))
				{
					this.listOfRules.Remove((Rule)entity);
				}
				else if (entity is Constraint && this.listOfConstraints.Contains((Constraint)entity))
				{
					this.listOfConstraints.Remove((Constraint)entity);
				}
				else if (entity is Reaction && this.listOfReactions.Contains((Reaction)entity))
				{
					this.listOfReactions.Remove((Reaction)entity);
				}
				else if (entity is Event && this.listOfEvents.Contains((Event)entity))
				{
					this.listOfEvents.Remove((Event)entity);
				}
			}
        }

		/// <summary>
		/// Updates the ID of an SBML object in the model, for renaming of SBML objects
		/// </summary>
		/// <param name="entity">
		/// A <see cref="SBase"/>
		/// </param>
		public void updateID(SBase entity)
		{
			// If we have an old ID implies it has actully been renamed
			// If the model already contains that ID
			// and if the new ID is different to the old one
			if (entity.getOldID()!=null && this.IdTable.ContainsKey(entity.getOldID()) && entity.ID!=entity.getOldID())
			{
                if (this.IdTable.ContainsKey(entity.ID))
                {
                    throw new DuplicateSBMLObjectIdException(entity.ID);
                }
                else
                {
                    this.IdTable.Remove(entity.getOldID());
                    this.IdTable.Add(entity.ID, entity);
                }
			}
		}


        public bool saveToXml(String filename)
        {
            try
            {
                SBMLroot sbml = new SBMLroot(this);
                XmlSerializer s = new XmlSerializer(typeof(SBMLroot));
                TextWriter w = new StreamWriter(filename);
                s.Serialize(w, sbml);
                w.Close();
                return true;
            }
            catch (Exception e)
            {
                throw e;
                return false;
            }
        }

    }

}
