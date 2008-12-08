using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace MuCell.Model.SBML.ExtracellularComponents
{

    /// <summary>
    /// A CellBody component responsible for allowing a cell to sample 
    /// the nutrient concentration in the environment
    /// </summary>
    public class CellBodyComponent : ComponentBase
    {



        //cell radius
        private float radius;
        [CategoryAttribute("Physical Properties"), DescriptionAttribute("The radius of the cell in mirco meters")]
        [XmlAttribute]
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        //cell radius
        private float mass;
        [CategoryAttribute("Physical Properties"), DescriptionAttribute("The mass of the cell in nano grams")]
        [XmlAttribute]
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public CellBodyComponent()
            : base()
        {
            this.reactants = new SpeciesReference[0];
            this.products = new SpeciesReference[0];
            this.mass = 1.0f;
            this.radius = 1.0f;
        }


        /// <summary>
        /// Returns an object assosiated with this component containing world
        /// state information (specific to an individual cell)
        /// </summary>
        /// <returns></returns>
        public override ComponentWorldStateBase CreateWorldStateObject()
        {
            return new CellBodyWorldState(this);
        }




        public override void InitializeInEnvironment(CellInstance cell, ComponentWorldStateBase compData, StateSnapshot state)
        {
            cell.CellInstanceSpatialContext.Mass = mass;
            cell.CellInstanceSpatialContext.Radius = radius;
        }

        public override void DoTimeStep(CellInstance cell, ComponentWorldStateBase compData, StateSnapshot state, double time, double timeStep)
        {

           //nothing to do there
        }

        /// <summary>
        /// Links the nutrient fields in the given environment to 
        /// this component
        /// </summary>
        /// <param name="env"></param>
        public override void LinkToNutrientFields(Environment env)
        {
            //nothing to do
        }

        /// <summary>
        /// Returns a list of all the nutrient field names required to
        /// exist in the environment for this component
        /// </summary>
        /// <returns></returns>
        public override String[] GetRequiredNutrientFieldNames()
        {
            List<String> names = new List<String>();
            return names.ToArray();
        }

        public override String[] getReactantNames()
        {
            return new String[] {};
        }
        public override String[] getProductNames()
        {
            return new String[] {};
        }
    }
}
