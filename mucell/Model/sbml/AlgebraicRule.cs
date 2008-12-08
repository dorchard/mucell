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
	/// Algebraic Rule
	/// </summary>
    public class AlgebraicRule : Rule
    {
    		/// <summary>
    		/// Constructor 
    		/// </summary>
        public AlgebraicRule()
        {
        }

		/// <summary>
		/// Constructor with model association and a hastable of attributes to set the name
		/// </summary>
		/// <param name="model">
		/// A <see cref="Model"/>
		/// </param>
		/// <param name="attrs">
		/// A <see cref="Hashtable"/>
		/// </param>
        public AlgebraicRule(Hashtable attrs)
        {
            this.setId(attrs);
        }

    }
}
