using System;
using System.Collections.Generic;
using System.Text;
using MuCell;

namespace MuCell.Model
{
    public class StateSnapshot : ICloneable
    {
        
        /// <summary>
        /// List of cells in snapshot
        /// </summary>
        private List<CellInstance> cells;
        public List<CellInstance> Cells
        {
        		get { return cells; }
        		set { cells = value; }
        }
        
        /// <summary>
        /// Simulation environment
        /// </summary>
        private Environment simulationEnvironment;
        public Environment SimulationEnvironment
        {
        		get { return simulationEnvironment; }
        		set { simulationEnvironment = value; }
        }

		/// <summary>
		/// Constructor
		/// </summary>
        public StateSnapshot()
        {
  
            this.SimulationEnvironment = new MuCell.Model.Environment(new MuCell.Model.Vector3(1, 1, 1));
            this.cells = new List<CellInstance>();
        }

        /// <summary>
        /// Constructor that takes a list of cells as the argument.
        /// </summary>
        /// <param name="cells">
        /// A <see cref="List`1"/>
        /// </param>
        public StateSnapshot(List<CellInstance> cells)
        {
                this.SimulationEnvironment = new MuCell.Model.Environment(new MuCell.Model.Vector3(1, 1, 1));
        		this.cells = cells;
		}
		
		/// <summary>
		/// Clone method for StateSnapshots
		/// </summary>
		/// <returns>
		/// A <see cref="Object"/>
		/// </returns>
		public Object Clone()
		{
			StateSnapshot cloned = new StateSnapshot();
			// Clone the cell instances within
			foreach(CellInstance cell in this.cells)
			{
				cloned.cells.Add((CellInstance)cell.Clone());
			}
			// Clone the environment
			cloned.simulationEnvironment = (Environment)this.simulationEnvironment.Clone();


			return cloned;
		}

        public bool exptEquals(StateSnapshot other)
        {
            if (this.SimulationEnvironment.exptEquals(other.SimulationEnvironment) == false)
            {
                Console.Write("StateSnapshot objects not equal: ");
                Console.Write("this.SimulationEnvironment.volume=" + this.SimulationEnvironment.volume);
                Console.WriteLine("; other.SimulationEnvironment.volume=" + other.SimulationEnvironment.volume);
                return false;
            }
            // check CellInstances
            try
            {
                for (int i = 0; i < this.Cells.Count; i++)
                {
                    if (this.Cells[i].exptEquals(other.Cells[i]) == false)
                    {
                        Console.Write("StateSnapshot objects not equal: ");
                        Console.WriteLine("this.Cells[" + i + "] != other.Cells[" + i + "]");
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                // Array out of bounds; lists are unequal
                Console.Write("SimulationParameters objects not equal: ");
                Console.WriteLine("List lengths differed");
                return false;
            }
            return true;
        }

    }
}
