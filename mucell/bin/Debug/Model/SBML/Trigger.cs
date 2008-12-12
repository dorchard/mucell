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
    /// Describes the trigger condition that, when it is met, triggers
    /// the eventAssignments defined in the parent Event object.
    /// </summary>
    public class Trigger : MathBase
    {
        public Event parentEvent;

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

        public Trigger()
        {
        }

        public Trigger(Event parent)
        {
            this.parentEvent = parent;
        }
        
        /// <summary>
        /// Test the equality of two SBML Triggers
        /// </summary>
        /// <param name="o">
        /// A <see cref="System.Object"/>
        /// </param>
        /// <returns>
        /// A <see cref="boolean"/>
        /// </returns>
        public new bool SBMLEquals(object o)
        {
        		if (o is Trigger)
        		{
        			// Test equality of parent event
        			return ((SBase)o).SBOTerm == this.SBOTerm
        				&& ((this.parentEvent == null && ((Trigger)o).parentEvent == null) ||
        					(this.parentEvent != null && this.parentEvent.SBMLEquals(((Trigger)o).parentEvent)))
        				&& ((MathBase)o).SBMLEquals((MathBase)this);
        		}
        		else
			{
				return false;
			}
        }
    }
}
