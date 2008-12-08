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
    public class InitialAssignment : MathBase
    {
        [XmlIgnore]
        public SBase variable; // can be Species, Compartment or Parameter

        [XmlAttribute("symbol")]
        public String variableId
        {
            get
            {
                if (variable != null) { return variable.ID; }
                else { return null; }
            }
        }

        [XmlAttribute("formula")]
        public String XmlMath
        {
            get
            {
                if (this.math != null)
                {
                    return this.math.ToString();
                }
                else
                {
                    return null;
                }
            }
            set { }
        }

        public InitialAssignment()
        {
        }

        public InitialAssignment(SBase variable)
        {
            this.variable = variable;
        }

        public InitialAssignment(SBase variable, Hashtable attrs)
        {
            this.variable = variable;
            this.setId(attrs);
        }
        
        public InitialAssignment(Hashtable attrs)
        {
            this.setId(attrs);
        }
        
        /// <summary>
        /// Test the equality of two Initial Assignments
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
        		if (o is InitialAssignment)
        		{
        			// Test maths equality
        			return 	((SBase)o).SBOTerm == this.SBOTerm
        				&& ((MathBase)o).SBMLEquals((MathBase)this);
        		}
        		else
			{
				return false;
			}
        }
        
        
    }
}
