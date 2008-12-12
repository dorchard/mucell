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
using System.ComponentModel;

namespace MuCell.Model.SBML
{
    /// <summary>
    /// Abstract parent class for SpeciesReference and ModifierSpeciesReference
    /// </summary>
    public abstract class SimpleSpeciesReference : MathBase
    {
        [XmlIgnore]
        public Species species;
        
        [CategoryAttribute("Reference settings"), DescriptionAttribute("The id of the linked species")]
        [XmlAttribute("species")]
        public string SpeciesID
        {
            get
            {
                if (this.species != null) { return this.species.ID; }
                else { return null; }
            }
            set { }
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

        public override String ToString()
        {
            return SpeciesID;
        }
        
        /// <summary>
        /// Test the equality of two SBML SimpleSpeciesReferences.
        /// </summary>
        /// <param name="o">
        /// A <see cref="System.Object"/>
        /// </param>
        /// <returns>
        /// A <see cref="boolean"/>
        /// </returns>
		public new bool SBMLEquals(object o)
        {
        		if (o is SimpleSpeciesReference)
        		{
        			return ((SBase)o).SBOTerm == this.SBOTerm
        				&& ((SimpleSpeciesReference)o).SpeciesID == this.SpeciesID 
        				&& ((MathBase)o).SBMLEquals((MathBase)this);
        		}
        		else
			{
				return false;
			}
        }
        
    }
}
