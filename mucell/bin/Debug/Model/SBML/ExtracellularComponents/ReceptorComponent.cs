using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML.ExtracellularComponents
{
    public class ReceptorComponent : ComponentBase
    {

        //Nutrient fields
        private int attractantIndex, repellentIndex;


        public ReceptorComponent():base()
        {
            this.reactants = new SpeciesReference[2];
            this.products = new SpeciesReference[1];
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
            NutrientField attractant = state.SimulationEnvironment.GetNutrientFieldObject(attractantIndex);


            if (speciesRef == null)
            {
                TestRigs.ErrorLog.LogError("Receptor not connected properly! ");
                return;

            }

            float amount = attractant.GetNutrientLevel(cell.CellInstanceSpatialContext.Position);

           // TestRigs.ErrorLog.LogError("setting " + speciesRef.species.ID + " to: " + amount);
            cell.setSpeciesAmountInSimulation(speciesRef.species.ID,amount );
            cell.setSpeciesAmount(speciesRef.species.ID, amount);
   
        }


        public override void InitializeInEnvironment(CellInstance cell, ComponentWorldStateBase compData, StateSnapshot state)
        {
            //TestRigs.ErrorLog.LogError(" In InitialInENv: " );
      
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
            attractantIndex = -1;
            repellentIndex = -1;

            attractantIndex = env.GetNutrientByName("nutrient0").Index;
            //    repellent = env.GetNutrientByName("repellent");


        }


        /// <summary>
        /// Returns a list of all the nutrient field names required to
        /// exist in the environment for this component
        /// </summary>
        /// <returns></returns>
        public override String[] GetRequiredNutrientFieldNames()
        {
            List<String> names = new List<String>();

            //attractant
            names.Add("nutrient0");
            //repellant
            //  names.Add("repellent");

            return names.ToArray();
        }




        public override String[] getReactantNames()
        {
            return new String[] { "Methylate","Demethylate" };
        }
        public override String[] getProductNames()
        {
            return new String[] {"Phosphorylation\nRate Modifier"};
        }
    }
}
