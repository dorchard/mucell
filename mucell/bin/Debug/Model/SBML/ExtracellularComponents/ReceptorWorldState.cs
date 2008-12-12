using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML.ExtracellularComponents
{
    public class ReceptorWorldState : ComponentWorldStateBase
    {

        public float SomeInternalValue;

        //whether or not the cell should be tumbling
        public Boolean TumbleState;

        public ReceptorWorldState(ComponentBase componentType)
            :
            base(componentType)
        {
            this.SomeInternalValue = 0.0f;
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
            ReceptorWorldState newInstance = new ReceptorWorldState(this.ComponentType);
            // Add the properties
            newInstance.SomeInternalValue = this.SomeInternalValue;
  
            return newInstance;
        }

    }
}
