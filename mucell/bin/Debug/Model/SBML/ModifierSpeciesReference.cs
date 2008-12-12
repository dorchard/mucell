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
    public class ModifierSpeciesReference : SimpleSpeciesReference
    {
        [XmlAttribute("species")]
        public String SpeciesID
        {
            get
            {
                if (species != null) { return species.ID; }
                else { return null; }
            }
            set { }
        }

        public ModifierSpeciesReference()
        {
        }

        public ModifierSpeciesReference(Species species)
        {
            this.species = species;
        }
    }
}
