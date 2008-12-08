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
    public class Compartment : SBase
    {
        [XmlIgnore]
        public CompartmentType compartmentType = null;
        [XmlAttribute]
        public int spatialDimensions;
        [XmlAttribute]
        public double size;
        [XmlAttribute]
        public String units; // unit enum or user-defined unit
        [XmlIgnore]
        public Compartment outside = null;
        [XmlAttribute]
        public Boolean constant;

        [XmlAttribute("compartmentType")]
        public String CompTypeId
        {
            get
            {
                if (compartmentType != null) { return compartmentType.ID; }
                else { return null;  }
            }
            set {  }
        }

        [XmlAttribute("outside")]
        public String CompOutsideId
        {
            get
            {
                if (outside != null) { return outside.ID; }
                else { return null; }
            }
            set {  }
        }

        public Compartment()
        {
        }

        public Compartment(Hashtable attrs)
        {
            this.setId(attrs);
        }

        public Compartment(String id)
        {
            this.ID = id;
        }

        public void AddProperties(CompartmentType cType, int sD, double size,
            String units, Compartment outside, Boolean constant)
        {
            this.compartmentType = cType;
            this.spatialDimensions = sD;
            this.size = size;
            this.outside = outside;
            this.constant = constant;
        }
        
        /// <summary>
        /// Tests equality of two SBML Compartments.
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
        		if (o!=null && o is Compartment)
        		{
        			Compartment ob = (Compartment)o;
        			// Equality test
        			return (ob.CompTypeId == this.CompTypeId
        				&& ob.CompOutsideId == this.CompOutsideId
        				&& ob.SBOTerm == this.SBOTerm
        				&& ob.size == this.size
        				&& ob.spatialDimensions == this.spatialDimensions
        				&& ob.constant == this.constant
        				&& ob.units == this.units);
        		} 
        		else 
        		{
        			return false;
        		}
        }

        //public override string ToString()
        //{
        //    return this.ID;
        //}

    }
}
