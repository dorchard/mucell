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
    public class Unit : SBase
    {
        [XmlAttribute]
        public String kind;
        [XmlAttribute]
        public int exponent;
        [XmlAttribute]
        public int scale;
        [XmlAttribute]
        public double multiplier;

        public Unit()
        {
        }

        public Unit(String kind, int exponent, int scale, double multiplier)
        {
            this.kind = kind;
            this.exponent = exponent;
            this.scale = scale;
            this.multiplier = multiplier;
        }

		/// <summary>
		/// Test the equality of two SBML units
		/// </summary>
		/// <param name="o">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="boolean"/>
		/// </returns>
        public new bool SBMLEquals(object o)
        {
        		if (o is Unit)
        		{
        			Unit ob = (Unit)o;
        			
        			return ((SBase)o).SBOTerm == this.SBOTerm
        				&& this.kind == ob.kind
        				&& this.exponent == ob.exponent
        				&& this.scale == ob.scale
        				&& this.multiplier == ob.multiplier;
        		}
        		else
			{
				return false;
			}
        }

    }
}
