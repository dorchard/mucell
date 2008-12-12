using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML.ExtracellularComponents
{

    /// <summary>
    /// The world state associated with a Receptor component - this effectively a data
    /// object with a reference to the component type to which the data belongs
    /// </summary>
    public class ReceptorWorldState : ComponentWorldStateBase
    {
      
        public ReceptorWorldState(ComponentBase componentType)
            :
            base(componentType)
        {
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
  
            return newInstance;
        }

    }
}
