using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML.ExtracellularComponents
{
    public class FlagellaWorldState : ComponentWorldStateBase
    {

        public float TumbleUpdateCounter;
        public float TumbleCounter;
        public float SampleCounter;
        public float FirstSample;
        public bool FirstSampleTaken;

        public float TumbleLikelihood;
      
        //whether or not the cell should be tumbling
        public Boolean TumbleState;

        public FlagellaWorldState(ComponentBase componentType):
            base(componentType) 
        {
            //the current likelihood that the cell will tumble when next a decision phase is entered ( 1 => certain, 0 => certainly not)
            TumbleLikelihood = 1.0f;

            TumbleUpdateCounter = 0.0f;
            TumbleState = true;
            TumbleCounter = 0.0f;
            SampleCounter = 0.0f;
            FirstSample = 0.0f;
            FirstSampleTaken = false;
        }

        public override void DoTimeStep(CellInstance cell, StateSnapshot state, double time, double timeStep)
        {
            ComponentType.DoTimeStep(cell, this, state, time, timeStep);
        }

        public override void InitializeInEnvironment(CellInstance cell, StateSnapshot state)
        {
            ComponentType.InitializeInEnvironment(cell, this, state);
        }




        /// <summary>
        /// CLones the FlagellaWorldState object
        /// </summary>
        /// <returns></returns>
        public override Object Clone()
        {
            FlagellaWorldState newInstance = new FlagellaWorldState(this.ComponentType);
            // Add the properties
            newInstance.TumbleLikelihood = this.TumbleLikelihood;
            newInstance.TumbleUpdateCounter = this.TumbleUpdateCounter;
            newInstance.TumbleState = this.TumbleState;
            newInstance.TumbleCounter = this.TumbleCounter;
            newInstance.SampleCounter = this.SampleCounter;
            newInstance.FirstSample = this.FirstSample;
            newInstance.FirstSampleTaken = this.FirstSampleTaken;

            return newInstance;
        }

    }
}
