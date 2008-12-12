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
	public abstract class SBase
    {
        /// <summary>
        /// Abstract base class for all SBML elements
        /// </summary>
        protected string notes;
        protected string annotation;
        private int sboTerm;
        
        // Used for managing duplicate tags
        private bool duplicate = false;
        private SBase original = null;
        // counts the number of duplicates for relabelling purposes
        [XmlIgnore]
        public int duplicateCount = 0;
        
        private string id = null;
        private string oldid = null;
        
        [CategoryAttribute("Component settings"), DescriptionAttribute("ID of the component")]
        [XmlAttribute("id")]
        public String ID
        {
            get { return id; }
            set
            {
            		// Save the oldID
            		oldid = id;
            		id = value;
            	}
        }

        [CategoryAttribute("Component settings"), DescriptionAttribute("Associated Notes")]
        [XmlElement]
        public String Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        [CategoryAttribute("Component settings"), DescriptionAttribute("Associated Annotation")]
        [XmlElement]
        public String Annotation
        {
            get { return annotation; }
            set { annotation = value; }
        }
        [CategoryAttribute("Component settings"), DescriptionAttribute("SBO term"), BrowsableAttribute(false)]
        [XmlIgnore]
        public int SBOTerm
        {
            get { return sboTerm; }
            set { sboTerm = value; }
        }

        /// <summary>
        /// Returns the XML element name of the object
        /// </summary>
        /// <returns>The XML tagname of the element</returns>
        public String getElementName()
        {
            String name = this.GetType().Name;
            return name.Substring(0,1).ToLower() + 
                name.Substring(1, name.Length - 1);
        }
        
        /// <summary>
        /// Set the Id for an SBML element from the attributes.
        /// Adds the element to the Id table
        /// </summary>
        public void setId(Hashtable attrs)
        {
            // If the attributes contains id
            if (attrs.Contains("id"))
            {
            		
                this.id = (String)attrs["id"];
               // this.model.AddId(this.id, 
                //Console.WriteLine("Has ID="+this.id);
            }
            else
            {
            		// If there is no id attribute, but a name attribute
            		// use the name attribute
            		if (attrs.Contains("name"))
            		{
					this.id = (String)attrs["name"];
					//this.model.AddId(this.id, this);
            		} 
            	}
        } 
        
        /// <summary>
        /// Sets an object as having a duplicate ID to an object already in the model
        /// </summary>
        /// <param name="original">
        /// A <see cref="SBase"/>
        /// </param>
        public void setHasADuplicateId(SBase original) 
        {
        		this.duplicate = true;
        		this.original = original;
        }
        
        /// <summary>
        /// Returns true if the object's Id was a duplicate when inserted
        /// into the model, false if it was unique.
        /// Note: When a duplicate id object is added, its id is changed
        /// </summary>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool hadDuplicateId()
        {
        		return this.duplicate;
        }
        
        /// <summary>
        /// If the object is a duplicate, returns the original
        /// </summary>
        /// <returns>
        /// A <see cref="SBase"/>
        /// </returns>
        public SBase getOriginal()
        {
        		return this.original;
        }
        
        /// <summary>
        /// Returns the old ID of the entity if it has been renamed
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public string getOldID()
        {
        		return this.oldid;
        }
        
        /// <summary>
        /// Test the equality of two SBases
        /// </summary>
        /// <param name="o">
        /// A <see cref="System.Object"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool SBMLEquals(object o)
        {
        		if (o is SBase)
        		{
        			return (((SBase)o).SBOTerm == this.SBOTerm);
        		}
        		else
        		{
        			return false;
        		}
        }

    }

}

