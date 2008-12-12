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
    public class Parameter : SBase
    {
    		    
        private double value;
        private String units; // unit enum or unit definition
        private Boolean constant;

        [CategoryAttribute("Parameter settings"), DescriptionAttribute("Parameter value")]
        [XmlAttribute("value")]
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }
        [CategoryAttribute("Parameter settings"), DescriptionAttribute("Parameter units")]
        [XmlAttribute("units")]
        public String Units
        {
            get { return units; }
            set { units = value; }
        }
        [CategoryAttribute("Parameter settings"), DescriptionAttribute("Is the parameter a constant?")]
        [XmlAttribute("constant")]
        public bool Constant
        {
            get { return constant; }
            set { constant = value; }
        }

        public Parameter()
        {
        }

        public Parameter(Hashtable attrs)
        {
            this.setId(attrs);
        }

        public void AddProperties(double value, String units, Boolean constant)
        {
            this.value = value;
            this.constant = constant;
            this.units = units;
        }
        public override String ToString()
        {
            return this.ID;
        }

		/// <summary>
		/// Test the equality of two SBML Parameters.
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
        		if (o is Parameter)
        		{
        			Parameter ob = (Parameter)o;
        			
        			// Data check
        			return (ob.SBOTerm == this.SBOTerm)
        				&& (ob.Units == this.units)
        				&& (ob.Value == this.Value)
        				&& (ob.Constant == this.Constant);
        		}
        		else
			{
				return false;
			}
        }


    }
}
