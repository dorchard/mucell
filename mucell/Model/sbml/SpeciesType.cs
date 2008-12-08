/* Cathy Young
 * 
 * Classes representing a whole SBML model
 * Latest SBML spec: http://belnet.dl.sourceforge.net/sourceforge/sbml/sbml-level-2-version-3-rel-1.pdf
 */

using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MuCell.Model.SBML
{
    public class SpeciesType : SBase
    {
        public SpeciesType()
        {
        }

        public SpeciesType(Hashtable attrs)
        {
            this.setId(attrs);
        }
    }
}
