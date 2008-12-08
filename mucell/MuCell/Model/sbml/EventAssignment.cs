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
    /// <summary>
    /// Contains a mathematical rule that should be evaluated once the parent
    /// event's trigger condition has been met.
    /// </summary>
    public class EventAssignment : MathBase
    {
        [XmlIgnore]
        public SBase variable; // can be Compartment, Species or Parameter

        [XmlAttribute("variable")]
        public String variableId
        {
            get
            {
                if (variable != null) { return variable.ID; }
                else { return null; }
            }
            set {  }
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

        public EventAssignment()
        {
        }

        public EventAssignment(SBase variable)
        {
            this.variable = variable;
        }
        
        /// <summary>
        /// Test the equality of two SBML EventAssignments.
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
        		if (o is EventAssignment)
        		{
        			EventAssignment ob = (EventAssignment)o;
        			// Test equality of variable and maths
        			return ob.SBOTerm == this.SBOTerm
        				&& ((this.variable == null && ob.variable == null) || (this.variable != null && this.variable.SBMLEquals(ob.variable)))
        				&& ((MathBase)o).SBMLEquals((MathBase)this);
        		}
        		else
			{
				return false;
			}
        }

    }
    
}
