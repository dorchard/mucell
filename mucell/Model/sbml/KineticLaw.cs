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
using MuCell.Model.SBML;

namespace MuCell.Model.SBML
{
    /// <summary>
    /// A kinetic law from a Reaction object defining the speed at which 
    /// the process described in the reaction takes place. 
    /// </summary>
    public class KineticLaw : MathBase
    {
        [XmlIgnore]
        public Model model;
        [XmlIgnore]
        public List<Parameter> listOfParameters;
        [XmlIgnore]
        private String formula;

        [XmlAttribute("formula")]
        public String Formula
        {
            get
            {
                if (this.formula != null)
                {
                    return formula;
                }
                else if (this.math != null)
                {
                    return this.math.ToString();
                }
                else
                {
                    return null;
                }
            }
            set { formula = value; this.evaluateFormula(); }
        }

        [XmlArray("listOfParameters")]
        [XmlArrayItem("parameter")]
        public List<Parameter> Parameters
        {
            get { return listOfParameters; }
            set { listOfParameters = value; this.evaluateFormula(); }
        }

        private FormulaParser formulaParser;

        public KineticLaw()
        {
            this.listOfParameters = new List<Parameter>();
            formula = "";
        }

        public KineticLaw(Model model)
        {
            this.model = model;
            this.listOfParameters = new List<Parameter>();
            formula = "";
        }

        public KineticLaw(Model model, Hashtable attrs)
        {
            this.model = model;
            this.listOfParameters = new List<Parameter>();
            this.setId(attrs);
            if (attrs.Contains("formula"))
            {
                this.formula = (String)attrs["formula"];
            }
        }


        /// <summary>
        /// Returns a list of paramteres in the formula for the paramters
        /// </summary>
        public List<Parameter> ParametersFromFormula()
        {
            if (this.formulaParser != null)
            {
                return this.formulaParser.ParametersFromFormula();
            }
            else
            {
                return new List<Parameter>();
            }
        }

        /// <summary>
        /// Returns a list of species in the formula for the kinetic law
        /// </summary>
        public List<Species> SpeciesFromFormula()
        {
            if (this.formulaParser != null)
            {
                return this.formulaParser.SpeciesFromFormula();
            }
            else
            {
                return new List<Species>();
            }
        }

        /// <summary>
        /// Returns a list of the unknown entites in the formula for the kinetic law
        /// </summary>
        public List<UnknownEntity> UnknownEntitiesFromFormula()
        {
            if (this.formulaParser != null)
            {
                return this.formulaParser.UnknownEntitiesFromFormula();
            }
            else
            {
                return new List<UnknownEntity>();
            }
        }

        public void evaluateFormula()
        {
            if (this.formula != null)
            {
                this.formulaParser = new FormulaParser(new Reader.SBMLReader(), this.formula, this.model);
                this.math = formulaParser.getFormulaTree();
            }
            else
            {
                // Likely a MathML rate law => therefore set any SBase references
                this.math.SetSBaseReferences(this.model);
            }
        }

        /// <summary>
        /// Tests the equality of two SBML KineticLaws.
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
            if (o is KineticLaw)
            {
                bool check = true;
                KineticLaw ob = (KineticLaw)o;

                check &= (ob.formula == this.formula) && (ob.SBOTerm == this.SBOTerm);
                check &= ob.listOfParameters.Equals(this.listOfParameters);
                check &= ((MathBase)o).SBMLEquals((MathBase)this);

                return check;
            }
            else
            {
                return false;
            }
        }

    }
}
