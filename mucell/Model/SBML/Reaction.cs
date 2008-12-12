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
    /// Describes a reaction in the model.
    /// </summary>
    public class Reaction : SBase, IModelComponent
    {
        private Boolean fast;
        private Boolean reversible;
        private List<SpeciesReference> listOfReactants;
        private List<SpeciesReference> listOfProducts;
        private List<ModifierSpeciesReference> listOfModifiers;
        private KineticLaw kineticLaw;
        //position for cell editor
        [XmlIgnore]
        public float xPosition;
        [XmlIgnore]
        public float yPosition;

       
        [CategoryAttribute("Reaction settings"), DescriptionAttribute("Is the reaction accelerated?")]
        [XmlAttribute("fast")]
        public bool Fast
        {
            get { return fast; }
            set { fast = value; }
        }
        [CategoryAttribute("Reaction settings"), DescriptionAttribute("Is the reaction reversible?")]
        [XmlAttribute("reversible")]
        public bool Reversible
        {
            get { return reversible; }
            set { reversible = value; }
        }

        [CategoryAttribute("Reaction settings"), DescriptionAttribute("The list of reactants in the process")]
        [XmlArray("listOfReactants")]
        [XmlArrayItem("speciesReference")]
        public List<SpeciesReference> Reactants
        {
            get { return listOfReactants; }
            set { listOfReactants = value; }
        }
        [CategoryAttribute("Reaction settings"), DescriptionAttribute("The list of products in the process")]
        [XmlArray("listOfProducts")]
        [XmlArrayItem("speciesReference")]
        public List<SpeciesReference> Products
        {
            get { return listOfProducts; }
            set { listOfProducts = value; }
        }
        [CategoryAttribute("Reaction settings"), DescriptionAttribute("The list of modifier species in the process")]
        [XmlArray("listOfModifiers")]
        [XmlArrayItem("modifierSpeciesReference")]
        public List<ModifierSpeciesReference> Modifiers
        {
            get { return listOfModifiers; }
            set { listOfModifiers = value; }
        }

        [CategoryAttribute("Reaction settings"), DescriptionAttribute("The reaction's kinetic law element"), BrowsableAttribute(false)]
        [XmlElement("kineticLaw")]
        public KineticLaw KineticLaw
        {
            get { return kineticLaw; }
            set { kineticLaw = value; }
        }

        [CategoryAttribute("Kinetic Law settings"), DescriptionAttribute("The rate equation")]
        [XmlIgnore]
        public String Formula
        {
            get { return kineticLaw.Formula; }
            set { kineticLaw.Formula = value; }
        }

        [CategoryAttribute("Kinetic Law settings"), DescriptionAttribute("The equation parameters")]
        [XmlIgnore]
        public List<Parameter> Parameters
        {
            get { return kineticLaw.Parameters; }
            set { kineticLaw.Parameters = value; }
        }

        [BrowsableAttribute(false)]
        [XmlElement("annotation", Namespace = "http://example.com/MuCell/Reaction")]
        public SpatialParameters spatialParams
        {
            get
            {
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
            set { }
        }

        public Reaction()
        {
            useInitialValues();
        }

        private void useInitialValues()
        {
            this.listOfReactants = new List<SpeciesReference>();
            this.listOfProducts = new List<SpeciesReference>();
            this.listOfModifiers = new List<ModifierSpeciesReference>();
            this.fast = false;
            this.reversible = false;
        }

        public Reaction(String id)
        {
            this.ID = id;
            useInitialValues();
        }

        public Reaction(Hashtable attrs)
        {
            this.setId(attrs);
            useInitialValues();
        }

        public void AddProperties(Boolean fast, Boolean reversible)
        {
            this.fast = fast;
            this.reversible = reversible;
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
            return 6;
        }

        public float getHeight()
        {
            return 6;
        }

        public Vector2 getClosestPoint(Vector2 otherPosition)
        {
            //find angle to other point

            double angleTo = Math.Atan2(otherPosition.y - yPosition, otherPosition.x - xPosition);

            //then project along that angle of length radius

            return new Vector2(((float)Math.Cos(angleTo) * getWidth() * 0.5f) + xPosition, ((float)Math.Sin(angleTo) * getHeight() * 0.5f) + yPosition);
        }
        
        public CellEvaluationFunction ToCelEvaluationFunction()
        {
        		return this.KineticLaw.ToCellEvaluationFunction();
        }
        
        /// <summary>
        /// Returns a function that encapsulates the intentional functionality of the reaction in a function.
        /// This function has side-effects on the paramtere StateSnapshot and CellInstance
        /// </summary>
        /// <returns>
        /// A <see cref="EffectReactionEvaluationFunction"/>
        /// </returns>
        public MuCell.Model.EffectReactionEvaluationFunction ToEffectReactionEvaluationFunction()
        {
        		// Compute the cell evaluation function for the Reaction
        		MuCell.Model.CellEvaluationFunction cell_eval_function = this.KineticLaw.ToCellEvaluationFunction();
        		return delegate(StateSnapshot s, CellInstance c){
        			// Evaluate the absolute change in concentration of the reactant
        			double value = cell_eval_function(s, c);
        			// Affect the change in the reactants
        			foreach(SpeciesReference speciesReference in this.Reactants)
        			{
        				if (!speciesReference.species.BoundaryCondition)
        				{
        					c.localSpeciesDelta[speciesReference.SpeciesID] -= value*(double)speciesReference.Stoichiometry;
        				}
        			}
        			// Affect the change in the products
        			foreach(SpeciesReference speciesReference in this.Products)
        			{
        				if (!speciesReference.species.BoundaryCondition)
        				{
        					c.localSpeciesDelta[speciesReference.SpeciesID] += value*(double)speciesReference.Stoichiometry;
        				}
        			}};
        }

		public new bool SBMLEquals(object o)
        {
        		// Type check
        		if (o is Reaction)
        		{
        			bool check = true;
        			Reaction ob = (Reaction)o;
        			
        			check &= (ob.SBOTerm == this.SBOTerm) && (ob.Fast == this.Fast) && (ob.Formula == this.Formula);
        			check &= ((ob.KineticLaw == null && this.KineticLaw == null) || (ob.KineticLaw != null && ob.KineticLaw.SBMLEquals(this.KineticLaw)));
        			check &= (ob.Modifiers.Equals(this.Modifiers));
        			check &= (ob.Parameters.Equals(this.Parameters)) && (ob.Products.Equals(this.Products));
        			check &= (ob.Reactants.Equals(this.Reactants)) && (ob.Reversible == this.Reversible);
        			
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
