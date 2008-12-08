/* Cathy Young
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
    /// Denotes a leaf of a MathTree. Is subclases by NumberLeafNode and ReferenceLeafNode.
	/// </summary>
    [XmlInclude(typeof(ReferenceLeafNode)), XmlInclude(typeof(NumberLeafNode))]
    public class LeafNode : MathNode
    {        

		public virtual void AddData(Object o)
		{
			this.data = o;
		}

    }

}
