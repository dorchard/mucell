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
using System.Windows.Forms;

namespace MuCell.Model.SBML
{
    public class UnitDefinition : SBase
    {
        [XmlArray]
        public List<Unit> listOfUnits;

        public UnitDefinition()
        {
        }

        public UnitDefinition(Hashtable attrs)
        {
            this.setId(attrs);
            this.listOfUnits = new List<Unit>();
        }

		/// <summary>
		/// Test the equality of two SBML UnitDefinitions
		/// </summary>
		/// <param name="o">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="boolean"/>
		/// </returns>
        public new bool SBMLEquals(object o)
        {
        		if (o is UnitDefinition)
        		{
        			// Test equality of parent events, and test equality of the maths
        			return ((SBase)o).SBOTerm == this.SBOTerm
        				&& this.listOfUnits.Equals(((UnitDefinition)o).listOfUnits);
        		}
        		else
			{
				return false;
			}
        }
    }
}
