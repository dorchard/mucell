using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace MuCell.Model.SBML
{
    public class ReferenceLeafNode : LeafNode
    {
        [XmlIgnore]
       	public SBase data;
       
        //[XmlIgnore]
        public String idReference;

        /// <summary>
        /// Subtype of LeafNode denoting an object
        /// </summary>
        public ReferenceLeafNode() {}

		/// <summary>
		/// Default AddData method that sets just the idReference.
		/// The data element can be set later with FixSBaseReference.
		/// </summary>
		/// <param name="o">
		/// A <see cref="Object"/>
		/// </param>
		public override void AddData(Object o)
		{
			if (o is string)
			{
				this.idReference = (string)o;
			}
		}
		
		/// <summary>
		/// Checks all ReferenceLeafNodes so that they point to their SBase object.
		/// Used to complete a MathTree generated from MathML after it has been parsed and
		/// more about the model is known (such as paramters).
		/// </summary>
		/// <param name="model">
		/// A <see cref="Model"/>
		/// </param>
		public override void SetSBaseReferences(Model model)
		{
			if (this.idReference != null)
			{
				SBase reference = (SBase)model.findObject(this.idReference);
            		if (reference != null)
            		{
            		    this.data = reference;
            		}
			}
		}

		/// <summary>
		/// Add data to the ReferenceLeafNode
		/// </summary>
		/// <param name="text">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="model">
		/// A <see cref="Model"/>
		/// </param>
        public void AddData(string text, Model model)
        {
        	// Retrieve the object from the model
            SBase reference = (SBase)model.findObject(text);
            if (reference != null)
            {
            	this.idReference = text;
                this.data = reference;
            }
        }

        /// <summary>
        /// Returns the approximate units of the formula
        /// </summary>
        /// <returns>A string</returns>
        public override string ApproximateUnits()
        {
            return "Concentration";
        }
        
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public override string ToString	()
        {
        	return this.idReference;
        }
        	
        	/// <summary>
        	/// In theory this should never be called as there should be no ReferenceLeafNodes in a AggregateFunction
        	/// </summary>
        	/// <returns>
        	/// A <see cref="AggregateEvaluationFunction"/>
        	/// </returns>
        public override AggregateEvaluationFunction ToAggregateEvaluationFunction()
		{
			System.Console.WriteLine("Fail shouldn't ever be calling this\n");
			return null;
		}
        	
        	/// <summary>
        /// Builds a function of type StateSnapshot x CellInstance -> double from the Node
        /// </summary>
        /// <returns>
        /// A <see cref="CellEvaluationFunction"/>
        /// </returns>
        public override CellEvaluationFunction ToCellEvaluationFunction()
        {
			 if (this.data != null)
			{
				// Get a species amount from the cell instance and wrap in a function
				if (this.data is Species)
				{
					// Boundary conditions species therefore fixed
					if (((Species)this.data).BoundaryCondition)
					{
						return delegate(StateSnapshot s, CellInstance c) { return ((Species)this.data).initialValue; };
					}
					else
					{
						return delegate(StateSnapshot s, CellInstance c){ return c.getLocalSimulationSpeciesAmount(((Species)this.data).ID); };
					}
				}
				// Get a parameter from the tree and wrap in a function
				else if (this.data is Parameter)
				{
					return delegate(StateSnapshot s, CellInstance c){ return ((Parameter)this.data).Value; };
				}
				// Get the cell function from a function definition
				else if (this.data is FunctionDefinition)
				{
					return 	((FunctionDefinition)this.data).ToCellEvaluationFunction();
				}
				else
				{
					// <todo> Do we want to do anything with reaction or compartments?
				}					
            }
			// As a fail-safe return a 0 result function
			return delegate(StateSnapshot s, CellInstance c){ return 0.0d; };
		}

		/// <summary>
		/// Test the equality of two reference leaf nodes
		/// </summary>
		/// <param name="o">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
        public override bool SBMLEquals (object o)
        {
        		if (o is ReferenceLeafNode)
        		{
        			ReferenceLeafNode ob = (ReferenceLeafNode)o;
        			// Check data and types
        			return (ob.idReference == this.idReference && ob.data == this.data);        			
        		}
        		else
        		{
        			return false;
        		}
        }

    }
}
