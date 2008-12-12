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
    /// Describes an optional delay between the Trigger condition being met
    /// and the EventAssignments being executed.
    /// </summary>
    public class Delay : MathBase
    {
        [XmlIgnore]
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

        public Delay()
        {
        }

        public Delay(Event parent)
        {
            this.parentEvent = parent;
        }
        
        /// <summary>
        /// Tests equality of two SBML Delays.
        /// </summary>
        /// <param name="o">
        /// A <see cref="System.Object"/>
        /// </param>
        /// <returns>
        /// A <see cref="boolean"/>
        /// </returns>
        public new bool SBMLEquals(object o)
        {
        		if (o is Delay)
        		{
        			// Test equality of parent events, and test equality of the maths
        			return ((SBase)o).SBOTerm == this.SBOTerm
        				&& ((this.parentEvent == null && ((Delay)o).parentEvent == null) ||  
        					(this.parentEvent != null && this.parentEvent.SBMLEquals(((Delay)o).parentEvent)))
        				&& ((MathBase)o).SBMLEquals((MathBase)this);
        		}
        		else
			{
				return false;
			}
        }
    }
}
