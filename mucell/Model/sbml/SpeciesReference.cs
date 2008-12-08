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
    public class SpeciesReference : SimpleSpeciesReference
    {

		[CategoryAttribute("Reference settings"), DescriptionAttribute("The id of the referenced species")]
        [XmlAttribute("species")]
        public string SpeciesID
        {
            get
            {
                if (this.species != null) { return this.species.ID; }
                else { return ""; }
            }
            set {}
        }
        
        private double stoichiometry;
        [CategoryAttribute("Reference settings"), DescriptionAttribute("The multiple involved in the reaction")]
        [XmlAttribute("stoichiometry")]
        public double Stoichiometry
        {
        		get { return stoichiometry; }
            set { stoichiometry = value; }
        }

        public SpeciesReference()
        {
            this.stoichiometry = 1;
        }

        public SpeciesReference(Species species)
        {
            this.species = species;
            this.stoichiometry = 1;
        }

        public SpeciesReference(Species species, double stoichiometry)
        {
            this.species = species;
            this.stoichiometry = stoichiometry;
        }

        public new void AddProperties(MathTree stoichiometryMath)
        {
            this.math = stoichiometryMath;
        }
        
        
        	public new bool SBMLEquals(object o)
        {
        		if (o is SpeciesReference)
        		{
        			return ((SBase)o).SBOTerm == this.SBOTerm
        				&& ((SpeciesReference)o).Stoichiometry == this.Stoichiometry
        				&& ((SimpleSpeciesReference)o).SBMLEquals((SimpleSpeciesReference)this);
        		}
        		else
			{
				return false;
			}
        }
    }
}
