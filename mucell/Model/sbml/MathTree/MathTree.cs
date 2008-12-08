/* Cathy Young
 * 
 * Classes representing a whole SBML model
 * Latest SBML spec: http://belnet.dl.sourceforge.net/sourceforge/sbml/sbml-level-2-version-3-rel-1.pdf
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace MuCell.Model.SBML
{
    /// <summary>
    /// Abstract syntax tree denoting a mathematical expression or equation
    /// </summary>
    public class MathTree : SBase
    {
        [XmlIgnore]
        public Stack nodeStack;

        [XmlElement("root",Type=typeof(InnerNode))]
        public MathNode root;

		/// <summary>
		/// Constructor creates an empty node stack
		/// </summary>
        public MathTree()
        {
            this.nodeStack = new Stack();
        }

        public MathTree(InnerNode root, MathNode child)
        {
            this.nodeStack = new Stack();
            this.root = root;
            
            ((InnerNode) this.root).AddNode(child);
        }

        public MathTree(InnerNode root, MathNode left, MathNode right)
        {
            this.nodeStack = new Stack();
            this.root = root;

            ((InnerNode)this.root).AddNode(left);
            ((InnerNode)this.root).AddNode(right);            
        }

		/// <summary>
		/// Return the XML name of the object
		/// </summary>
		/// <returns>
		/// A <see cref="String"/>
		/// </returns>
        public new String getElementName()
        {
            return this.ID;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns>
		/// A <see cref="MathNode"/>
		/// </returns
        [XmlInclude(typeof(LeafNode)), XmlInclude(typeof(InnerNode))]
        public MathNode getCurrentNode()
        {
            try
            {
                return (MathNode)nodeStack.Peek();
            }
            catch
            {
                // empty stack, create empty root node
                this.root = new InnerNode();
                this.nodeStack.Push(this.root);
                return this.root;
            }
        }
        
        /// <summary>
        /// Builds a function of type StateSnapshot x CellInstance -> double
        /// </summary>
        /// <returns>
        /// A <see cref="CellEvaluationFunction"/>
        /// </returns>
        public CellEvaluationFunction ToCellEvaluationFunction()
        {
         	return this.root.ToCellEvaluationFunction();
        }
        
        /// <summary>
        /// Builds a function of type SnapsShot -> decimal for calculating functions with aggregate variables
        /// </summary>
        /// <returns>
        /// A <see cref="AggregateEvaluationFunction"/>
        /// </returns>
        public AggregateEvaluationFunction ToAggregateEvaluationFunction()
        {
        		return this.root.ToAggregateEvaluationFunction();
        }

        /// <summary>
        /// Returns a string of the approximate units of the function
        /// </summary>
        /// <returns>A string</returns>
        public string ApproximateUnits()
        {
            return this.root.ApproximateUnits();
        }
        
        /// <summary>
        /// Pretty prints the MathTree
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public override string ToString()
        {
        		return this.root.ToString();
        }
        
        /// <summary>
        /// Checks the equality of MathTrees
        /// </summary>
        /// <param name="o">
        /// A <see cref="System.Object"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public new bool SBMLEquals (object o)
        {
			if (o is MathTree)
			{
				/// Test equality from the root
				MathTree ob = (MathTree)o;
				return this.root.SBMLEquals(ob.root);
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
		public void SetSBaseReferences(Model model)
		{
			root.SetSBaseReferences(model);
		}

    }

}
