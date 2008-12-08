// AggregateReference.cs created with MonoDevelop
// User: riftor at 12:02 31/03/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Collections.Generic;

using MuCell;
using MuCell.Model;

namespace MuCell.Model.SBML
{
    public class AggregateReferenceNode : LeafNode
    {
    
    		private Experiment experiment;
    		private Simulation simulation;
    
    		/// <value>
    		/// CellDefinition
    		/// </value>
    		private CellDefinition cellDefinition;
    		public CellDefinition CellDefinition
    		{
    			get { return cellDefinition; }
    			set { cellDefinition = value; }
    		}
    		
    		/// <summary>
    		/// Group 
    		/// </summary>
    		private Group group;
    		public Group Group
    		{
    			get { return group; }
    			set { group = value; }
    		}
    		
    		/// <summary>
    		/// Species
    		/// </summary>
    		private Species species;
    		public Species Species
    		{
    			get { return species; }
    			set { species = value; }
    		}
    
    		/// <summary>
    		/// Base constructor
    		/// </summary>
		public AggregateReferenceNode(Experiment experiment, Simulation simulation)
		{
			this.cellDefinition = null;
			this.group = null;
			this.species = null;
			this.experiment = experiment;
			this.simulation = simulation;
		}
		
		public override AggregateEvaluationFunction ToAggregateEvaluationFunction()
        {
        		return delegate(StateSnapshot s)
        		{
        			if (this.species == null && this.group == null && this.cellDefinition == null)
        			{
        				// Somehow we have a blank node
        				System.Console.WriteLine("Error in creating aggregate functions");
        				return 0.0d;
        			}
        			else
        			{       				
					// List of cells
        				List<CellInstance> cells = new List<CellInstance>();
        				StateSnapshot state = this.simulation.GetCurrentState();
        			
        				// If there is a group, restrict cells to cells from that group
        				if (this.group != null)
        				{
        					cells = state.SimulationEnvironment.CellsFromGroup(Int32.Parse(this.group.ID));  			
        				}
        				// Else set cells to all
        				else
        				{
	        				cells = state.Cells;
    		    			}
        			
        				// Test celldefinitions
    		    			if(this.cellDefinition != null)
					{
						List<CellInstance> tempCells = new List<CellInstance>();
						// Foreach cell
						foreach(CellInstance cell in cells)
						{
							// If it has the correct celldefinition
							if (cell.CellInstanceDefinition.Name == this.cellDefinition.Name)
							{
								// Add to the list
								 tempCells.Add(cell);
							}
						}
						// Set the new list to the old list
						cells = tempCells;
					}
					
					// Now test Species
					if (this.species != null)
					{
						double speciesAmount = 0.0d;
						// Sum the species amount in the cells
						foreach(CellInstance cell in cells)
						{
							// Use getSpeciesAmount to get the value out of the cell not the simulation
							speciesAmount+=cell.getSpeciesAmount(this.species.ID);
						}
						return speciesAmount;
					}
					else
					{
						// No species ref so just return the population amount
						return (double)cells.Count;
					}
				}
        		};
		}

        /// <summary>
        /// Returns the approximate string representation of the units of the formula
        /// </summary>
        /// <returns>A string</returns>
        public override string ApproximateUnits()
        {
            if (this.cellDefinition != null)
            {
                if (this.group != null)
                {
                    if (this.species != null)
                    {
                        return "Concentration";
                    }
                    else
                    {
                        return "Cells";
                    }
                }
                else
                {
                    if (this.species != null)
                    {
                        return "Concentration";
                    }
                    else
                    {
                        return "Cells";
                    }
                }
            }
            else
            {
                if (this.group != null)
                {
                    if (this.species != null)
                    {
                        return "Concentration";
                    }
                    else
                    {
                        return "Cells";
                    }
                }
                else
                {
                    if (this.species != null)
                    {
                        return "Concentration";
                    }
                    else
                    {
                        return "No units";
                    }
                }
            }
        }
		
		/// <summary>
		/// ToString function
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public override string ToString ()
		{
			string output = "";
			if (this.cellDefinition!=null)
			{
				output += this.cellDefinition.Name;
			}
			if (this.group!=null)
			{
				// If we already had a cell definition add the period
				if (output!=""){
					output+=".";
				}
				output += "group"+this.group.ID;
			}
			if (this.species!=null)
			{
				// If we already had a cell definition/group add the period
				if (output!=""){
					output+=".";
				}
				output += this.species.ID;
			}
			return output;
		}

	}
}
