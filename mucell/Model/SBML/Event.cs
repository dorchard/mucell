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
    /// The list of event assignments take place once the "trigger" condition
    /// is met (with an optional delay described by the Delay field).
    /// </summary>
    public class Event : MathBase
    {
        [XmlElement]
        public Trigger trigger;
        [XmlElement]
        public Delay delay;
        [XmlArray]
        [XmlArrayItem("eventAssignment")]
        public List<EventAssignment> listOfEventAssignments;
        // each eventAssignment must use a unique variable 
        // (no overlap) per event

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

        public Event()
        {
        }

        public Event(Hashtable attrs)
        {
            this.setId(attrs);
        }
        
        /// <summary>
        /// Tests the equality of two SBML Events.
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
        		if (o is Event)
        		{
        			// Test equality of delay, event and test equality of the maths
        			return ((SBase)o).SBOTerm == this.SBOTerm 
        				&& ((this.trigger == null && ((Event)o).trigger == null) ||
        					(this.trigger != null && this.trigger.SBMLEquals(((Event)o).trigger)))
        				&& ((this.delay == null && ((Event)o).delay == null) ||
        					(this.delay != null && this.delay.SBMLEquals(((Event)o).delay)))
        				&& ((MathBase)o).SBMLEquals((MathBase)this);
        		}
        		else
			{
				return false;
			}
        }
        
    }
}
