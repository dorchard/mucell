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
    public class RateRule : Rule
    {
        [XmlIgnore]
        private SBase variable; // can be Species, Compartment or Parameter

        [XmlAttribute("variable")]
        public String variableID
        {
            get
            {
                if (variable != null) { return variable.ID; }
                else { return null; }
            }
        }

        public RateRule()
        {
        }

        public RateRule(SBase variable)
        {
            this.variable = variable;
        }

        public RateRule(Hashtable attrs)
        {
            this.setId(attrs);
        }
        
        /// <summary>
        /// Tests the equality of two SBML RateRules
        /// </summary>
        /// <param name="o">
        /// A <see cref="System.Object"/>
        /// </param>
        /// <returns>
        /// A <see cref="boolean"/>
        /// </returns>
         public new bool SBMLEquals(object o)
        {
        		// Type check
        		if (o is RateRule)
        		{
        			RateRule ob = (RateRule)o;
        			// Test equality of variable and maths
        			return (ob.SBOTerm == this.SBOTerm)
        				&& ((this.variable == null && ob.variable == null) || (this.variable != null && this.variable.SBMLEquals(ob.variableID)))
        				&& ((Rule)o).SBMLEquals((Rule)this);
        		}
        		else
			{
				return false;
			}
        }
    }
}
