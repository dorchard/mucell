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

        /// <summary>
        /// The type of component to which this data belongs
        /// </summary>
        private ComponentBase componentType;
        public ComponentBase ComponentType
        {
            get { return componentType; }
            set { componentType = value; }
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="compType"></param>
        public ComponentWorldStateBase(ComponentBase compType)
        {
            this.componentType = compType;
        }

        /// <summary>
        /// Execute a timestep, manipulating data accordingly
        /// </summary>
        /// <param name="cell">The cell to which this component belongs</param>
        /// <param name="state">The current environment state</param>
        /// <param name="time">Simulation time</param>
        /// <param name="timeStep">Simulation time step</param>
        public virtual void DoTimeStep(CellInstance cell, StateSnapshot state, double time, double timeStep)
        {
            ComponentType.DoTimeStep(cell, this, state, time, timeStep);
        }

        /// <summary>
        /// Initializes this component before simulation begins, allowing it to
        /// enter smoothly into the environment
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="state"></param>
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
