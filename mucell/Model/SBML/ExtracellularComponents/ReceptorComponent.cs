using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace MuCell.Model.SBML.ExtracellularComponents
{

    /// <summary>
    /// A receptor component responsible for allowing a cell to sample 
    /// the nutrient concentration in the environment
    /// </summary>
    public class ReceptorComponent : ComponentBase
    {

        //Nutrient fields
        private int nutrientIndex;



        //which nutrient the receptor is bound to
        private String nutrientName;
        [CategoryAttribute("Nutrient Field Properties"), DescriptionAttribute("The name of the nutrient field to which this receptor is linked")]
        [XmlAttribute]
        public String NutrientName
        {
            get { return nutrientName; }
            set { nutrientName = value; }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public ReceptorComponent():base()
        {
            this.reactants = new SpeciesReference[0];
            this.products = new SpeciesReference[1];
            this.nutrientName = "attractant";
        }


        /// <summary>
        /// Returns an object assosiated with this component containing world
        /// state information (specific to an individual cell)
        /// </summary>
        /// <returns></returns>
        public override ComponentWorldStateBase CreateWorldStateObject()
        {
            return new ReceptorWorldState(this);
        }


        //update output (ie set values in simulator)
        private void updateOutput(CellInstance cell, StateSnapshot state)
        {
            SpeciesReference speciesRef = this.getSpeciesReference(0, ComponentLinkType.Output);
            NutrientField attractant = state.SimulationEnvironment.GetNutrientFieldObject(nutrientIndex);

            float amount = attractant.GetNutrientLevel(cell.CellInstanceSpatialContext.Position);

			cell.setSpeciesAmountInSimulation(speciesRef.species.ID,amount );
            cell.setSpeciesAmount(speciesRef.species.ID, amount );
   
        }


        public override void InitializeInEnvironment(CellInstance cell, ComponentWorldStateBase compData, StateSnapshot state)
        {
                     updateOutput(cell, state);
        }

        public override void DoTimeStep(CellInstance cell, ComponentWorldStateBase compData, StateSnapshot state, double time, double timeStep)
        {

            updateOutput(cell, state);
        }

        /// <summary>
        /// Links the nutrient fields in the given environment to 
        /// this component
        /// </summary>
        /// <param name="env"></param>
        public override void LinkToNutrientFields(Environment env)
        {
            nutrientIndex = -1;
            nutrientIndex = env.GetNutrientByName(nutrientName).Index;
        }

        /// <summary>
        /// Returns a list of all the nutrient field names required to
        /// exist in the environment for this component
        /// </summary>
        /// <returns></returns>
        public override String[] GetRequiredNutrientFieldNames()
        {
            List<String> names = new List<String>();

            //nutrient
            names.Add(nutrientName);
            return names.ToArray();
        }

        public override String[] getReactantNames()
        {
           // return new String[] { "Methylate","Demethylate" };
            return new String[] {  };
        }
        public override String[] getProductNames()
        {
            return new String[] { "Field Concentration \r\noutput" };
        }
    }
}
