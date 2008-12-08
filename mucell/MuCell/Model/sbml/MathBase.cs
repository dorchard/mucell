/* Cathy Young
 * 
 * Classes representing a whole SBML model
 * Latest SBML spec: http://belnet.dl.sourceforge.net/sourceforge/sbml/sbml-level-2-version-3-rel-1.pdf
 */

using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML
{

	/// <summary>
    /// Abstract parent class for any element containing a Maths expression
    /// </summary>
    [XmlInclude(typeof(LeafNode)),XmlInclude(typeof(InnerNode))]
    public abstract class MathBase : SBase
    {
        [XmlIgnore]
        public MathTree math;

        public void AddProperties(MathTree math)
        {
            this.math = math;
        }
        
        public CellEvaluationFunction ToCellEvaluationFunction()
        {
			return this.math.ToCellEvaluationFunction();
		}
		
		/// <summary>
		/// Check equality of MathBases
		/// </summary>
		/// <param name="o">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public new bool SBMLEquals (object o)
		{
			if (this.math!=null)
			{
				return this.math.SBMLEquals(o);
			}
			else
			{
				return false;
			}
		}

    }
}
