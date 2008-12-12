using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace MuCell.Model.SBML.ExtracellularComponents
{
    public class FlagellaComponent : ComponentBase
    {


        //Nutrient fields
        private int attractantIndex, repellentIndex;


        private float tumbleDuration;
        [CategoryAttribute("Flagella Attributes"), DescriptionAttribute("Tumble duration (seconds)")]
        public float TumbleDuration
        {
            get { return tumbleDuration; }
            set { tumbleDuration = value; }
        }

        private float motiveStength;
        [CategoryAttribute("Flagella Attributes"), DescriptionAttribute("Force with which the flagellar propel the cell forward (nano newtons)")]
        public float MotiveStength
        {
            get { return motiveStength; }
            set { motiveStength = value; }
        }


        private float tumbleUpdateFrequency;
        [CategoryAttribute("Flagella Attributes"), DescriptionAttribute("The frequency with which the cell decides whether or not to enter a tumbling state (for example 0.5 => once every 2 seconds)")]
        public float TumbleUpdateFrequency
        {
            get { return tumbleUpdateFrequency; }
            set { tumbleUpdateFrequency = value; }
        }



        public FlagellaComponent():base()
        {
            TumbleDuration = 3f;
            MotiveStength = 0.8f;
            TumbleUpdateFrequency = 0.25f;

            attractantIndex = -1;
            repellentIndex = -1;

            this.reactants = new SpeciesReference[1];
            this.products = new SpeciesReference[0];

        
        }


        /// <summary>
        /// Returns an object assosiated with this component containing world
        /// state information (specific to an individual cell)
        /// </summary>
        /// <returns></returns>
        public override ComponentWorldStateBase CreateWorldStateObject()
        {
            return new FlagellaWorldState(this);
        }

        public override void InitializeInEnvironment(CellInstance cell, ComponentWorldStateBase compData, StateSnapshot state)
        {

        }




        public override void DoTimeStep(CellInstance cell, ComponentWorldStateBase compData,StateSnapshot state, double time, double timeStep)
        {
            FlagellaWorldState flage = (FlagellaWorldState)compData;

            NutrientField attractant = state.SimulationEnvironment.GetNutrientFieldObject(attractantIndex);
            NutrientField repellent = state.SimulationEnvironment.GetNutrientFieldObject(repellentIndex);



            if (flage.TumbleState)
            {
                flage.TumbleCounter += (float)timeStep;



                //end of tumble
                if (flage.TumbleCounter > tumbleDuration)
                {
                    cell.CellInstanceSpatialContext.Reorientate(new Vector3((float)(cell.GetRandomObject().NextDouble() * 2 - 1),
                                                             (float)(cell.GetRandomObject().NextDouble() * 2 - 1),
                                                             (float)(cell.GetRandomObject().NextDouble() * 2 - 1)));
             
                    flage.TumbleState = false;
                    //up to 50% varience in next tumble duration
                    flage.TumbleCounter = 0.0f + tumbleDuration*0.5f*(float)cell.GetRandomObject().NextDouble();
               
                }

              
            }
            else
            {


                flage.TumbleUpdateCounter += (float)timeStep;

                //Randomly decide whether or not to tumble according to some likelihood
                if (flage.TumbleUpdateCounter > 1.0f / TumbleUpdateFrequency)
                {
                    float val = flage.TumbleLikelihood - (float)cell.GetRandomObject().NextDouble();

                 
                    if (flage.TumbleLikelihood - (float)cell.GetRandomObject().NextDouble() > 0)
                    {
                        //tumble
                        flage.TumbleState = true;
                    }

                    flage.TumbleUpdateCounter = 0.0f;
                }


                flage.SampleCounter += (float)timeStep;






                //propel cell forward
                cell.CellInstanceSpatialContext.Accelerate(new Vector3(
                    cell.CellInstanceSpatialContext.Orientation.x * motiveStength * (float)timeStep,
                    cell.CellInstanceSpatialContext.Orientation.y * motiveStength * (float)timeStep,
                    cell.CellInstanceSpatialContext.Orientation.z * motiveStength * (float)timeStep));

                if (flage.SampleCounter > 0.5f)
                {
                    if (!flage.FirstSampleTaken)
                    {
                        flage.FirstSample = attractant.GetNutrientLevel(cell.CellInstanceSpatialContext.Position);
                        flage.FirstSampleTaken = true;
                    }
                }

              //  TestRigs.ErrorLog.LogError(" coutner: " + flage.SampleCounter);
                if (flage.SampleCounter > 1.0f)
                {

                    float SecondSample = attractant.GetNutrientLevel(cell.CellInstanceSpatialContext.Position);

                    if (SecondSample > flage.FirstSample)
                    {
                        //continue going straight
                        flage.TumbleLikelihood = 0.001f;
                        
                      
                    }
                    else
                    {
                        //tumble
                        flage.TumbleLikelihood = 0.995f;

                    }
                    flage.SampleCounter = 0.0f;
                    flage.FirstSampleTaken = false;
                    
                    
                }

            }


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
            return new String[] { "Tumble Frequency\nSensitivity" };
        }
        public override String[] getProductNames()
        {
            return new String[] {};
        }

    }
}
