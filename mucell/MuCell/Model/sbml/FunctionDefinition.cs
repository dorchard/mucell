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
    /// Describes a function definition given in the SBML document.
    /// </summary>
    public class FunctionDefinition : MathBase
    {
        
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

        public FunctionDefinition()
        {
        }

        public FunctionDefinition(Hashtable attrs)
        {
            this.setId(attrs);
        }
        
        /// <summary>
        /// Tests the equality of two SBML FunctionDefinitions
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
        		if (o is FunctionDefinition)
        		{
        			// Test equality
        			return ((SBase)o).SBOTerm == this.SBOTerm
        				&& ((MathBase)o).SBMLEquals((MathBase)this);
        		}
        		else
			{
				return false;
			}
        }
    }
}
