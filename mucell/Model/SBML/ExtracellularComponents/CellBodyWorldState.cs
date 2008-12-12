using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML.ExtracellularComponents
{

    /// <summary>
    /// The world state associated with a Cell Body component
    /// </summary>
    public class CellBodyWorldState : ComponentWorldStateBase
    {

        public CellBodyWorldState(ComponentBase componentType)
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
            CellBodyWorldState newInstance = new CellBodyWorldState(this.ComponentType);
            // Add the properties

            return newInstance;
        }

    }
}
