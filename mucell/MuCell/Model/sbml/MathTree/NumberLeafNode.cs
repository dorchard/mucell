using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML
{
    /// <summary>
    /// Subtype of LeafNode denoting a real value
    /// </summary>
    public class NumberLeafNode : LeafNode
    {
        //[XmlElement("NumberLeaf")]
        [XmlIgnore]
        public double data;

        [XmlElement]
        public String Number
        {
            get
            {
                return this.data.ToString();
            }
            set { }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public NumberLeafNode() {}

		/// <summary>
		/// Adds data to the NumberLeafNode
		/// </summary>
		/// <param name="text">
		/// A <see cref="System.String"/>
		/// </param>
        public override void AddData(Object o)
        {
        		if (o is string)
        		{
        			this.data = double.Parse((string)o);
            	}
        }
	
		/// <summary>
		/// Add a leaf node that is a MathConstant
		/// </summary>
		/// <param name="constant">
		/// A <see cref="MathConstants"/>
		/// </param>
		new public void AddData(MathConstants constant)
		{
			if (constant == MathConstants.Pi)
			{
				this.data = Math.PI;
			}
			else if (constant == MathConstants.Exponential)
			{
				this.data = Math.E;
			}
			else if (constant == MathConstants.Notanumber)
			{
				this.data = double.NaN;
			}
			else if (constant == MathConstants.Infinity)
			{
				this.data = Double.PositiveInfinity; 
			}
			// <todo>What do we do with True, False?
		}


        /// <summary>
        /// Returns the appxorimate units of the formula
        /// </summary>
        /// <returns>A string</returns>
        public override string ApproximateUnits()
        {
            return "No units";
        }

        /// <summary>
        /// ToString function
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public override string ToString()
        {
        		return this.data.ToString();
        	}
        	
        /// <summary>
        /// Builds a function of type StateSnapshot -> double from the Node
        /// </summary>
        /// <returns>
        /// A <see cref="CellEvaluationFunction"/>
        /// </returns>
        public override AggregateEvaluationFunction ToAggregateEvaluationFunction()
        {
			return delegate(StateSnapshot s){ return this.data; };
		}

        /// <summary>
        /// Builds a function of type StateSnapshot x CellInstance -> double from the Node
        /// </summary>
        /// <returns>
        /// A <see cref="CellEvaluationFunction"/>
        /// </returns>
        public override CellEvaluationFunction ToCellEvaluationFunction()
        {
			return delegate(StateSnapshot s, CellInstance c){ return this.data; };
		}
		
		/// <summary>
		/// Test the equality of two NumberLeafNodes
		/// </summary>
		/// <param name="o">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public override bool SBMLEquals (object o)
        {
        		if (o is NumberLeafNode)
        		{
        			NumberLeafNode ob = (NumberLeafNode)o;
        			// Check data
        			return (ob.data == this.data);        			
        		}
        		else
        		{
        			return false;
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
			// Do nothing
		}
    }
}
