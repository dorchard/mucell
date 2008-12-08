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
    /// A mathematical expression or equation that must always be true
    /// at every point of the simulation.
    /// </summary>
    public class Constraint : MathBase
    {
        /// <summary>
        /// XHTML explanation of Constraint for the end-user.
        /// Can be shown if the constaint is violated.
        /// </summary>
        [XmlElement]
        public String message;

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

        public Constraint()
        {
        }

        public Constraint(String id)
        {
            this.ID = id;
        }

        public Constraint(Hashtable attrs)
        {
            this.setId(attrs);
        }

        public void AddProperties(String message)
        {
            this.message = message;
        }

    }
}
