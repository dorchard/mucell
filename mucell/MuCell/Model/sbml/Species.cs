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
    [DefaultPropertyAttribute("Name")]
    public class Species : SBase, IModelComponent
    {
        private SpeciesType speciesType;
        private Compartment compartment;
        private Double initialAmount;
        private Double initialConcentration;
        private String substanceUnits; // unit enum
        private Boolean hasOnlySubstanceUnits;
        private Boolean boundaryCondition;
        private Boolean constant;
        [XmlIgnore]
        public float xPosition;
        [XmlIgnore]
        public float yPosition;

		[XmlIgnore]
		public double initialValue;

        [BrowsableAttribute(false)]
        [XmlElement("annotation",Namespace="http://example.com/MuCell")]
        public SpatialParameters spatialParams
        {
            get {
                if (xPosition != 0 && yPosition != 0)
                {
                    SpatialParameters sParams = new SpatialParameters(xPosition, yPosition);
                    return sParams;
                }
                else
                {
                    return null;
                }
            }
            set {  }
        }

        [CategoryAttribute("Species settings"), DescriptionAttribute("The type of species"),BrowsableAttribute(false)]
        [XmlIgnore]
        public SpeciesType SpeciesType
        {
            get { return speciesType; }
            set { speciesType = value; }
        }

        [XmlAttribute("speciesType")]
        [BrowsableAttribute(false)]
        public String SpeciesTypeId
        {
            get
            {
                if (speciesType != null) { return speciesType.ID; }
                else { return null; }
            }
            set { }
        }
        
        
        [XmlIgnore]
        [BrowsableAttribute(false)]
        public Compartment Compartment
        {
            get { return compartment; }
            set { compartment = value; }
        }

        [XmlAttribute("compartment")]
        [CategoryAttribute("Species settings"), DescriptionAttribute("The compartment where the species is located"), BrowsableAttribute(false)]
        public String CompartmentId
        {
            get
            {
                if (compartment != null) { return compartment.ID; }
                else { return null; }
            }
            set {  }
        }

        [CategoryAttribute("Species settings"), DescriptionAttribute("The amount at the start of the simulation")]
        [XmlAttribute("initialAmount")]
        public Double InitialAmount
        {
            get { return initialAmount; }
            set
            {
                initialAmount = value;
                if (this.initialAmount == -1d)
                {
                    this.initialValue = this.initialConcentration;
                }
                else
                {
                    this.initialValue = this.initialAmount;
                }
            }
        }
        [CategoryAttribute("Species settings"), DescriptionAttribute("The concentration at the start of the simulation")]
        [XmlAttribute("initialConcentration")]
        public Double InitialConcentration
        {
            get { return initialConcentration; } 
            set { initialConcentration = value;
                if (this.initialConcentration == -1d)
                {
                    this.initialValue = this.initialAmount;
                }
                else
                {
                    this.initialValue = this.initialConcentration;
                }
            }
        }

        [CategoryAttribute("Species settings"), DescriptionAttribute("The unit used to represent the amount")]
        [XmlAttribute("substanceUnits")]
        public String SubstanceUnits
        {
            get { return substanceUnits; }
            set { substanceUnits = value; }
        }

        [CategoryAttribute("Species settings"), DescriptionAttribute("Is it using only substance units?")]
        [XmlAttribute("hasOnlySubstanceUnits")]
        public bool HasOnlySubstanceUnits
        {
            get { return hasOnlySubstanceUnits; }
            set { hasOnlySubstanceUnits = value; }
        }

        [CategoryAttribute("Species settings"), DescriptionAttribute("Is it a boundary condition (so not affected by reactions)?")]
        [XmlAttribute("boundaryCondition")]
        public bool BoundaryCondition
        {
            get { return boundaryCondition; }
            set { boundaryCondition = value; }
        }

        [CategoryAttribute("Species settings"), DescriptionAttribute("Does the value stay the same?")]
        [XmlAttribute("constant")]
        public bool Constant
        {
            get { return constant; }
            set { constant = value; }
        }
        
        private double absoluteTolerance = 1e-6d;
        [XmlIgnore]
        public double AbsoluteTolerance
        {
        		get { return absoluteTolerance; }
        		set { absoluteTolerance = value; }
        }

        public Species()
        {
        }

        public Species(Hashtable attrs)
        {
            this.setId(attrs);
        }

        public Species(String id)
        {
            this.ID = id;
        }

        public void AddProperties(SpeciesType st, Compartment c, Double iA,
            Double iC, String sU, Boolean hOSU, Boolean bC, Boolean con)
        {
            this.speciesType = st;
            this.compartment = c;
            this.initialAmount = iA;
            this.initialConcentration = iC;
            this.substanceUnits = sU;
            this.hasOnlySubstanceUnits = hOSU;
            this.boundaryCondition = bC;
            this.constant = con;
            
            // Set an inital value based on whether we have a concentration or amount
            // Used as an optimisation in the model for boundaryCondition species
            if (this.initialAmount == -1d)
            {
                this.initialValue = this.initialConcentration;
            }
            else
            {
                this.initialValue = this.initialAmount;
            }
        }

        public override string ToString()
        {
            return this.ID;
        }

        #region IModelComponent Members

        public void setPosition(float x, float y)
        {
            xPosition = x;
            yPosition = y;
        }

        public Vector2 getPosition()
        {
            return new Vector2(xPosition, yPosition);
        }


        public float getWidth()
        {
            return 16;
        }

        public float getHeight()
        {
            return 16;
        }

        public Vector2 getClosestPoint(Vector2 otherPosition)
        {
            //find angle to other point

            double angleTo = Math.Atan2(otherPosition.y - yPosition, otherPosition.x - xPosition);

            //then project along that angle of length radius

            return new Vector2(((float)Math.Cos(angleTo) * getWidth()*0.5f) +xPosition, ((float)Math.Sin(angleTo) * getHeight()*0.5f) + yPosition);
        }
        
        /// <summary>
        /// Test the equality of two SBML Species
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
        		if (o is Species)
        		{
        			bool check = true;
        			Species ob = (Species)o;
        			
        			check &= (ob.SBOTerm == this.SBOTerm);
        			check &= (ob.BoundaryCondition == this.BoundaryCondition);
        			check &= (ob.Compartment == null && this.Compartment==null) ||
        					(ob.Compartment !=null && ob.Compartment.SBMLEquals(this.Compartment));
        			check &= (ob.CompartmentId == this.CompartmentId);
        			check &= (ob.Constant == this.Constant);
        			check &= (ob.HasOnlySubstanceUnits == this.HasOnlySubstanceUnits);
        			check &= (ob.InitialAmount == this.InitialAmount);
        			check &= (ob.InitialConcentration == this.InitialConcentration);
        			check &= (ob.SpeciesType == null && this.SpeciesType == null) ||
        					(ob.SpeciesType!=null && ob.SpeciesType.SBMLEquals(this.SpeciesType));
        			check &= (ob.SpeciesTypeId == this.SpeciesTypeId);
        			check &= (ob.SubstanceUnits == this.SubstanceUnits);       			
        			return check;
        		}
        		else
			{
				return false;
			}
        }

        #endregion
    }

}
