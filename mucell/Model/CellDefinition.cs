using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.SBML;

namespace MuCell.Model
{
    /// <summary>
    /// CellDefinition outlines the properties of a CellInstance
    /// </summary>
    /// <owner>Cathy</owner>

    public class CellDefinition
    {

        // private List<SBML.Model> sbmlModels;
        private SBML.Model sbmlModel;

        private string name;
        [XmlAttribute("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string lastSBMLPath;
        public string LastSBMLPath
        {
            get { return lastSBMLPath; }
            set { lastSBMLPath = value; addSBMLModel(lastSBMLPath); }
        }

        [XmlIgnore]
        public string setLastSBMLPath
        {
            set { lastSBMLPath = value; }
        }
        //never used - not really sure it would be the right way to go anyway, no way of getting unique name...unless...
        public void autoSave(String experimentName)
        {
            if (sbmlModel != null)
            {
                if (lastSBMLPath != null && lastSBMLPath.Length > 0)
                {
                    sbmlModel.saveToXml(lastSBMLPath);
                }
                else
                {
                    try
                    {
                        String path = System.AppDomain.CurrentDomain.BaseDirectory + "/Models/autosave_" + experimentName + "_" + Name + "_" + DateTime.Now.Ticks + ".xml";

                        bool saved=sbmlModel.saveToXml(path);
                        if (saved)
                        {
                            this.lastSBMLPath = path;
                        }
                    }
                    catch (Exception e)
                    {

                    }
                    
                }
            }
        }

        /// <summary>
        /// Override ToString method
        /// </summary>
        /// <returns>name of the cell definition</returns>
        public override String ToString()
        {
            return name;
        }

        /// <summary>
        /// Base constructor
        /// </summary>
        public CellDefinition()
        {
            sbmlModel = new MuCell.Model.SBML.Model();
        }

        /// <summary>
        /// Constructor with argument to set the name
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/>
        /// </param>
        public CellDefinition(String name)
        {
            //this.sbmlModels = new List<SBML.Model>();
            this.name = name;
        }

        /// <summary>
        ///  	Given a filePath parse and add the SBML model defined
        ///	by the file at the filepath and  sets the SBML model
        /// </summary>
        /// <param name="filePath">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="bool"/> denoting success of failure as a boolean
        /// </returns>



        /// <summary>
        /// Adds an SBML model from the object
        /// </summary>
        /// <param name="model">
        /// A <see cref="Model.SBML.Model"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool addSBMLModel(Model.SBML.Model model)
        {
            this.sbmlModel = model;
            return true;
        }

        /// <summary>
        /// Adds an SBML model from a file
        /// </summary>
        /// <param name="filePath">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool addSBMLModel(string filePath)
        {
            SBML.Reader.SBMLReader reader = new SBML.Reader.SBMLReader();
            if (reader.Parse(filePath))
            {
                // Add the model
                // sbmlModels.Add(read.model);
                this.sbmlModel = reader.model;
                return true;
            }
            else
            {
                // Else fail
                return false;
            }
        }


        /// <summary>
        /// 	Constructs a cell instance object using this as the cell definition
        /// </summary>
        /// <returns>
        /// A newly instantiated <see cref="CellInstance" />
        /// </returns>
        public CellInstance createCell()
        {

            // Create a new instance
            CellInstance newInstance = new CellInstance(this);

            if (this.sbmlModel.listOfReactions != null)
            {
                // Fold all relevant reactions into a list of functions 
                List<EffectReactionEvaluationFunction> reactions = new List<EffectReactionEvaluationFunction>();
                foreach (SBML.Reaction reaction in this.sbmlModel.listOfReactions)
                {
                    reactions.Add(reaction.ToEffectReactionEvaluationFunction());
                }

                // Add the reaction function list
                newInstance.setReactions(reactions);
            }

            // Add the compnents from the model
            if (this.sbmlModel.listOfComponents != null)
            {
                foreach (SBML.ExtracellularComponents.ComponentBase component in this.sbmlModel.listOfComponents)
                {
                   
                    newInstance.Components.Add(component.CreateWorldStateObject());
                }
            }

            return newInstance;
        }

        /// <summary>
        /// Checks which nutrient fields required by this cell definition are missing
        /// from the given environment. Returns a list of the names of the missing
        /// nutrient fields. If none are missing, it returns an empty list.
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public List<String> MissingNutriendFields(Environment env)
        {

            TestRigs.ErrorLog.LogError("Checking missing nutrient fields for this celldef: " );

            List<String> missing = new List<String>();

            if (this.sbmlModel.listOfComponents != null)
            {
                foreach (SBML.ExtracellularComponents.ComponentBase component in this.sbmlModel.listOfComponents)
                {
                    TestRigs.ErrorLog.LogError("Here's a component in the list");

                    foreach (String nutName in component.GetRequiredNutrientFieldNames())
                    {

                        TestRigs.ErrorLog.LogError("Missing : " + nutName);

                        if (env.GetNutrientByName(nutName) == null)
                        {
                            missing.Add(nutName);
                        }

                    }
                }
            }
            else
            {
                TestRigs.ErrorLog.LogError("Component list null");
            }

            return missing;
        }

  




        /// <summary>
        /// Gets the cell definitions SBML model
        /// </summary>
        /// <returns>
        /// A <see cref="SBML.Model"/>
        /// </returns>
        public Model.SBML.Model getModel()
        {
            return sbmlModel;
        }

        public bool exptEquals(CellDefinition other)
        {
            if (this.Name != other.Name)
            {
                Console.Write("CellDefinition objects not equal: ");
                Console.WriteLine("this.Name='" + this.Name + "'; other.Name='" + other.Name);
                return false;
            }
           return true;
        }

    }
}
