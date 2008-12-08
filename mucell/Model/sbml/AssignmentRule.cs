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
    public class AssignmentRule : Rule
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

        public AssignmentRule()
        {
        }

        
        public AssignmentRule(Hashtable attrs)
        {
            this.setId(attrs);
        }

        public AssignmentRule(SBase variable)
        {
            this.variable = variable;
        }
        
        	public new bool SBMLEquals(object o)
        {
        		// Type check
        		if (o is AssignmentRule)
        		{
        			// Test maths equality
        			return 	((SBase)o).SBOTerm == this.SBOTerm
        				&& ((AssignmentRule)o).variableID == this.variableID
        				&& ((Rule)o).SBMLEquals((Rule)this);
        		}
        		else
			{
				return false;
			}
        }
        
    }
}
