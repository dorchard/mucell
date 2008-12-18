using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace MuCell.Model.SBML.ExtracellularComponents
{
    /// <summary>
    /// A flagella component representing a collection of flagella
    /// capable of propelling the cell to which they belong forward
    /// </summary>
    public class FlagellaComponent : ComponentBase
    {


        //How long the cell's average tumble phase lasts
        private float tumbleDuration;
        [CategoryAttribute("Flagella Attributes"), DescriptionAttribute("Tumble duration (seconds)")]
        [XmlAttribute]
        public float TumbleDuration
        {
            get { return tumbleDuration; }
            set { tumbleDuration = value; }
        }

        //The force with which the flagella propell the cell forward
        private float motiveStength;
        [CategoryAttribute("Flagella Attributes"), DescriptionAttribute("Force with which the flagellar propel the cell forward (nano newtons)")]
        [XmlAttribute]
        public float MotiveStength
        {
            get { return motiveStength; }
            set { motiveStength = value; }
        }


        //How frequently the flagella considers updating its current tumble state
        private float tumbleUpdateFrequency;
        [CategoryAttribute("Flagella Attributes"), DescriptionAttribute("The frequency with which the cell decides whether or not to enter a tumbling state (for example 0.5 => once every 2 seconds)")]
        [XmlAttribute]
        public float TumbleUpdateFrequency
        {
            get { return tumbleUpdateFrequency; }
            set { tumbleUpdateFrequency = value; }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public FlagellaComponent():base()
        {
            TumbleDuration = 2f;
            MotiveStength = 0.8f;
            TumbleUpdateFrequency = 0.5f;


            //attractantIndex = -1;
            //repellentIndex = -1;

            this.reactants = new SpeciesReference[1];
            this.products = new SpeciesReference[0];

        
        }
        
        // <note> temporary add-ins for recording running time and twiddle time for a cell </note>
        public double runTime = 0.0d;
        public double twiddleTime = 0.0d;

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
			this.runTime = 0.0d;
			this.twiddleTime = 0.0d;
        }


        /// <summary>
        /// Execute a timestep, manipulating data accordingly
        /// </summary>
        /// <param name="cell">The cell to which this component belongs</param>
        /// <param name="state">The current environment state</param>
        /// <param name="time">Simulation time</param>
        /// <param name="timeStep">Simulation time step</param>
        public override void DoTimeStep(CellInstance cell, ComponentWorldStateBase compData,StateSnapshot state, double time, double timeStep)
        {
            FlagellaWorldState flage = (FlagellaWorldState)compData;

            //NutrientField attractant = state.SimulationEnvironment.GetNutrientFieldObject(attractantIndex);
            //NutrientField repellent = state.SimulationEnvironment.GetNutrientFieldObject(repellentIndex);
            float valueFromCircuit = (float)cell.getLocalSimulationSpeciesAmount(getSpeciesReference(0, ComponentLinkType.Input).species.ID);

			// Tumble
            if (flage.TumbleState)
            {
            	// update twiddle time
            	this.twiddleTime=timeStep;
            	this.runTime=0.0d;
            
                flage.TumbleCounter += (float)timeStep;

				//System.Console.WriteLine("twiddling "+cell.GroupID);

                //end of tumble
                if (flage.TumbleCounter < tumbleDuration)
                {
                	System.Random random = cell.GetRandomObject();
                    cell.CellInstanceSpatialContext.Reorientate(new Vector3((float)(random.NextDouble() * 2 - 1),
                                                             (float)(random.NextDouble() * 2 - 1),
                                                             (float)(random.NextDouble() * 2 - 1)));
             
                    
                    //up to 50% variance in next tumble duration
                    
                    //System.Console.WriteLine("tc - group = "+cell.GroupID+" Tc = "+flage.TumbleCounter.ToString());

                } else {
                	System.Random random = cell.GetRandomObject();
                	flage.TumbleCounter = 0.0f + tumbleDuration*0.5f*(float)random.NextDouble();
                	flage.TumbleState = false;
                
                }

              
            }
            // Run
            else
            {
				this.twiddleTime = 0.0d;
				this.runTime=timeStep;
				
				//System.Console.WriteLine("running "+cell.GroupID);

                flage.TumbleUpdateCounter += (float)timeStep;

                
                if (flage.TumbleUpdateCounter > 1.0f / TumbleUpdateFrequency)
                {
                	//Randomly decide whether or not to tumble according to some likelihood
                	float val = flage.TumbleLikelihood - (float)cell.GetRandomObject().NextDouble();
                	//System.Console.WriteLine("cell - group = "+cell.GroupID+" val = "+val.ToString());
                    if (val > 0)
                    {
                        //tumble
                        flage.TumbleState = true;
                    }

                    flage.TumbleUpdateCounter = 0.0f;
                }

                //propel cell forward
                cell.CellInstanceSpatialContext.Accelerate(new Vector3(
                    cell.CellInstanceSpatialContext.Orientation.x * motiveStength * (float)timeStep,
                    cell.CellInstanceSpatialContext.Orientation.y * motiveStength * (float)timeStep,
                    cell.CellInstanceSpatialContext.Orientation.z * motiveStength * (float)timeStep));

				// Adjust likelihood of twiddle based on chemical value
                if (valueFromCircuit >= 1.0f)
                {
                    flage.TumbleLikelihood = 0.90f;
                }
                else
                {
                    flage.TumbleLikelihood = 0.1f;
                }

            }


        }

//        /// <summary>
//        /// Execute a timestep, manipulating data accordingly
//        /// </summary>
//        /// <param name="cell">The cell to which this component belongs</param>
//        /// <param name="state">The current environment state</param>
//        /// <param name="time">Simulation time</param>
//        /// <param name="timeStep">Simulation time step</param>
//        public override void DoTimeStep(CellInstance cell, ComponentWorldStateBase compData,StateSnapshot state, double time, double timeStep)
//        {
//            FlagellaWorldState flage = (FlagellaWorldState)compData;
//
//            //NutrientField attractant = state.SimulationEnvironment.GetNutrientFieldObject(attractantIndex);
//            //NutrientField repellent = state.SimulationEnvironment.GetNutrientFieldObject(repellentIndex);
//            float valueFromCircuit = (float)cell.getLocalSimulationSpeciesAmount(getSpeciesReference(0, ComponentLinkType.Input).species.ID);
//
//            if (flage.TumbleState)
//            {
//            	// update twiddle time
//            	this.twiddleTime=timeStep;
//            	this.runTime=0.0d;
//            
//                flage.TumbleCounter += (float)timeStep;
//
//
//
//                //end of tumble
//                if (flage.TumbleCounter > tumbleDuration)
//                {
//                	System.Random random = cell.GetRandomObject();
//                    cell.CellInstanceSpatialContext.Reorientate(new Vector3((float)(random.NextDouble() * 2 - 1),
//                                                             (float)(random.NextDouble() * 2 - 1),
//                                                             (float)(random.NextDouble() * 2 - 1)));
//             
//                    flage.TumbleState = false;
//                    //up to 50% variance in next tumble duration
//                    flage.TumbleCounter = 0.0f + tumbleDuration*0.5f*(float)random.NextDouble();
//
//                }
//
//              
//            }
//            else
//            {
//				this.twiddleTime = 0.0d;
//				this.runTime=timeStep;
//
//                flage.TumbleUpdateCounter += (float)timeStep;
//
//                //Randomly decide whether or not to tumble according to some likelihood
//                if (flage.TumbleUpdateCounter > 1.0f / TumbleUpdateFrequency)
//                {
//                    float val = flage.TumbleLikelihood - (float)cell.GetRandomObject().NextDouble();
//
//                 
//                    if (flage.TumbleLikelihood - (float)cell.GetRandomObject().NextDouble() > 0)
//                    {
//                        //tumble
//                        flage.TumbleState = true;
//                    }
//
//                    flage.TumbleUpdateCounter = 0.0f;
//                }
//
//
//                flage.SampleCounter += (float)timeStep;
//
//                //propel cell forward
//                cell.CellInstanceSpatialContext.Accelerate(new Vector3(
//                    cell.CellInstanceSpatialContext.Orientation.x * motiveStength * (float)timeStep,
//                    cell.CellInstanceSpatialContext.Orientation.y * motiveStength * (float)timeStep,
//                    cell.CellInstanceSpatialContext.Orientation.z * motiveStength * (float)timeStep));
//
//                if (flage.SampleCounter > 0.4f)
//                {
//                    /*
//                    if (!flage.FirstSampleTaken)
//                    {
//                        //flage.FirstSample = attractant.GetNutrientLevel(cell.CellInstanceSpatialContext.Position);
//                        flage.FirstSample = valueFromCircuit;
//                        flage.FirstSampleTaken = true;
//                    }*/
//                }
//
//                if (flage.SampleCounter > 0.8f)
//                {
//
//                   // float SecondSample = attractant.GetNutrientLevel(cell.CellInstanceSpatialContext.Position);
//                    /*
//                    float SecondSample = valueFromCircuit;
//
//                    if (SecondSample > flage.FirstSample)
//                    {
//                        //continue going straight
//                        flage.TumbleLikelihood = 0.001f;
//                        
//                      
//                    }
//                    else
//                    {
//                        //tumble
//                        flage.TumbleLikelihood = 0.995f;
//
//                    }*/
//
//                    if (valueFromCircuit > 1.0f)
//                    {
//                        flage.TumbleLikelihood = 0.90f;
//                    }
//                    else
//                    {
//                        flage.TumbleLikelihood = 0.1f;
//                    }
//
//                    flage.SampleCounter = 0.0f;
//                   // flage.FirstSampleTaken = false;
//                    
//                    
//                }
//
//            }
//
//
//        }




        /// <summary>
        /// Links the nutrient fields in the given environment to 
        /// this component
        /// </summary>
        /// <param name="env"></param>
        public override void LinkToNutrientFields(Environment env)
        {
            //attractantIndex = -1;
            //repellentIndex = -1;
            //attractantIndex = env.GetNutrientByName("nutrient0").Index;
        }

        /// <summary>
        /// Returns a list of all the nutrient field names required to
        /// exist in the environment for this component
        /// </summary>
        /// <returns></returns>
        public override String[] GetRequiredNutrientFieldNames()
        {
            List<String> names = new List<String>();
            //names.Add("nutrient0");
            return names.ToArray();
        }


        public override String[] getReactantNames()
        {
            return new String[] { "Tumble Frequency\nInput" };
        }
        public override String[] getProductNames()
        {
            return new String[] {};
        }

    }
}
