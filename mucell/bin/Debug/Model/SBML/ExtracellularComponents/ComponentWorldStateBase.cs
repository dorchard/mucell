using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML.ExtracellularComponents
{

  
    /// <summary>
    /// Base class for world state data associated with extracellular components
    /// </summary>
    public class ComponentWorldStateBase : ICloneable
    {

        private ComponentBase componentType;
        public ComponentBase ComponentType
        {
            get { return componentType; }
            set { componentType = value; }
        }

        public ComponentWorldStateBase(ComponentBase compType)
        {
            this.componentType = compType;
        }

        public virtual void DoTimeStep(CellInstance cell, StateSnapshot state, double time, double timeStep)
        {
            ComponentType.DoTimeStep(cell, this, state, time, timeStep);
        }


        public virtual void InitializeInEnvironment(CellInstance cell, StateSnapshot state)
        {
            ComponentType.InitializeInEnvironment(cell, this, state);
        }

        /// <summary>
        /// Clones the CompnentWorldStateBase object
        /// </summary>
        /// <returns></returns>
        public virtual Object Clone()
        {
            ComponentWorldStateBase newInstance = new ComponentWorldStateBase(this.componentType);
            return newInstance;
        }

    }
}
