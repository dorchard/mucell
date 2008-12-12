/* Cathy Young and Dominic Orchard
 * 
 * Classes representing a whole SBML model
 * Latest SBML spec: http://belnet.dl.sourceforge.net/sourceforge/sbml/sbml-level-2-version-3-rel-1.pdf
 */

using System;
using System.Xml;
using System.Xml.Serialization;

namespace MuCell.Model.SBML
{
    /// <summary>
    /// Parent class for nodes in a MathTree
    /// </summary>
    [XmlInclude(typeof(LeafNode)),XmlInclude(typeof(InnerNode))]
    public class MathNode
    {
        // Some associated node data
        //[XmlElement(Type=typeof(SBase))]
        //[XmlElement(Type=typeof(double))]
        [XmlIgnore]
        public Object data; // can be operator, constant, ID ref, csymbol, number
                       
//        [XmlArray("apply")]
//        [XmlArrayItem(typeof(ReferenceLeafNode),
//        ElementName = "ci"),
//        XmlArrayItem(typeof(NumberLeafNode),
//        ElementName = "cn"),
//        XmlArrayItem(typeof(OperatorNode),
//        ElementName = "apply"),
//        XmlArrayItem(typeof(LeafNode),
//        ElementName = "cl")]

		/// <summary>
		/// Blank constructor
		/// </summary>
		public MathNode() { }
        
        /// <summary>
        /// Pretty prints a math node
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public override string ToString()
		{
			return data.ToString();
		}
	
		/// <summary>
		/// Virtual ToCellEvaluationFunction to be implemented by LeafNode and OperatorNode
		/// Builds a function of type StateSnapshot x CellInstance -> double
		/// </summary>
		/// <returns>
		/// A <see cref="CellEvaluationFunction"/>
		/// </returns>
		public virtual CellEvaluationFunction ToCellEvaluationFunction()
		{
			System.Console.WriteLine("Fail shouldn't ever be calling this\n");
			return null;
		}
		
		/// <summary>
		/// Virtual ToAggregateEvaluationFunction to be implemented by LeafNode, OperatorNode, and ToAggregateEvaluationFunction
		/// </summary>
		/// <returns>
		/// A <see cref="AggregateEvaluationFunction"/>
		/// </returns>
		public virtual AggregateEvaluationFunction ToAggregateEvaluationFunction()
		{
			System.Console.WriteLine("Fail shouldn't ever be calling this\n");
			return null;
		}
		
		/// <summary>
		/// Check equality of MathNodes
		/// </summary>
		/// <param name="o">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public virtual bool SBMLEquals (object o)
		{
			if (o is MathNode)
			{
				MathNode ob = (MathNode)o;
				// Test data equality
				return (ob.data == this.data);
			}
			else
			{
				return false;
			}
		}

        public virtual string ApproximateUnits()
        {
            return "";
        }
		
		/// <summary>
		/// Checks all ReferenceLeafNodes so that they point to their SBase object.
		/// Used to complete a MathTree generated from MathML after it has been parsed and
		/// more about the model is known (such as paramters).
		/// </summary>
		/// <param name="model">
		/// A <see cref="Model"/>
		/// </param>
		public virtual void SetSBaseReferences(Model model)
		{
			
		}
    }
}
