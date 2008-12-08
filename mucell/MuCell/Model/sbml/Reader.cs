using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using MuCell.Model.SBML;

/*
  * Classes representing a whole SBML model
 * Latest SBML spec: http://belnet.dl.sourceforge.net/sourceforge/sbml/sbml-level-2-version-3-rel-1.pdf
 */
/// <summary>
/// 
/// </summary>
/// <owner>
/// Cathy
/// </owner>

// Usage:
// SBMLReader s = new SBMLReader(filePath);
// s.model etc.

namespace MuCell.Model.SBML.Reader
{
    public class SBMLReader
    {
        private Stack elementStack = new Stack();
        private StringBuilder value = new StringBuilder();
        private Boolean inMathNode = false;
        private Boolean skipNode = false;

        public Model model;

        // Lookup tables for all the operators, from their string representations to the enumerations]
        public Dictionary<string, BinaryMathOperators> binaryMathOperatorsLookup = new Dictionary<string, BinaryMathOperators>();
        public Dictionary<string, UnaryMathOperators> unaryMathOperatorsLookup = new Dictionary<string, UnaryMathOperators>();
        public Dictionary<string, BinaryLogicOperators> binaryLogicOperatorsLookup = new Dictionary<string, BinaryLogicOperators>();
        public Dictionary<string, UnaryLogicOperators> unaryLogicOperatorsLookup = new Dictionary<string, UnaryLogicOperators>();
        public Dictionary<string, MathConstants> mathConstantsLookup = new Dictionary<string, MathConstants>();
        public Dictionary<string, RelationOperators> relationOperatorsLookup = new Dictionary<string, RelationOperators>();

        /// <summary>
        /// From an operator Enum get a string representing that operator in an equation.
        /// </summary>
        /// <param name="op">
        /// A <see cref="Enum"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public string GetOperator(Enum op)
        {

            // Build a list of lookups
            if (Enum.IsDefined(typeof(BinaryMathOperators), op))
            {
                string shortest = "aaaaaaaaa";
                foreach (KeyValuePair<string, BinaryMathOperators> res in binaryMathOperatorsLookup)
                {
                    if (Enum.Equals(res.Value, op) && res.Key.ToString().Length <= shortest.Length)
                    {
                        shortest = res.Key.ToString();
                    }
                }
                if (shortest != "aaaaaaaaa")
                {
                    return shortest;
                }
            }
            else if (System.Enum.IsDefined(typeof(UnaryMathOperators), op))
            {
                foreach (KeyValuePair<string, UnaryMathOperators> res in unaryMathOperatorsLookup)
                {
                    if (Enum.Equals(res.Value, op))
                    {
                        return res.Key.ToString();
                    }
                }
            }
            else if (System.Enum.IsDefined(typeof(BinaryLogicOperators), op))
            {
                foreach (KeyValuePair<string, BinaryLogicOperators> res in binaryLogicOperatorsLookup)
                {
                    if (Enum.Equals(res.Value, op))
                    {
                        return res.Key.ToString();
                    }
                }
            }
            else if (System.Enum.IsDefined(typeof(UnaryLogicOperators), op))
            {
                foreach (KeyValuePair<string, UnaryLogicOperators> res in unaryLogicOperatorsLookup)
                {
                    if (Enum.Equals(res.Value, op))
                    {
                        return res.Key.ToString();
                    }
                }
            }
            else if (System.Enum.IsDefined(typeof(MathConstants), op))
            {
                foreach (KeyValuePair<string, MathConstants> res in mathConstantsLookup)
                {
                    if (Enum.Equals(res.Value, op))
                    {
                        return res.Key.ToString();
                    }
                }
            }
            else if (System.Enum.IsDefined(typeof(RelationOperators), op))
            {
                foreach (KeyValuePair<string, RelationOperators> res in relationOperatorsLookup)
                {
                    if (Enum.Equals(res.Value, op))
                    {
                        return res.Key.ToString();
                    }
                }
            }

            return op.ToString();
        }


        public SBMLReader()
        {
            this.InitializeOperators();
        }

        public SBMLReader(String filePath)
        {
            this.InitializeOperators();
            Parse(filePath);
        }

        private void InitializeOperators()
        {
            binaryMathOperatorsLookup.Add("+", BinaryMathOperators.Plus);
            binaryMathOperatorsLookup.Add("-", BinaryMathOperators.Minus);
            binaryMathOperatorsLookup.Add("*", BinaryMathOperators.Times);
            binaryMathOperatorsLookup.Add("/", BinaryMathOperators.Divide);
            binaryMathOperatorsLookup.Add("pow", BinaryMathOperators.Power);

            binaryMathOperatorsLookup.Add("plus", BinaryMathOperators.Plus);
            binaryMathOperatorsLookup.Add("minus", BinaryMathOperators.Minus);
            binaryMathOperatorsLookup.Add("times", BinaryMathOperators.Times);
            binaryMathOperatorsLookup.Add("divide", BinaryMathOperators.Divide);
            binaryMathOperatorsLookup.Add("power", BinaryMathOperators.Power);

            unaryMathOperatorsLookup.Add("root", UnaryMathOperators.Root);
            unaryMathOperatorsLookup.Add("abs", UnaryMathOperators.Abs);
            unaryMathOperatorsLookup.Add("exp", UnaryMathOperators.Exp);
            unaryMathOperatorsLookup.Add("ln", UnaryMathOperators.Ln);
            unaryMathOperatorsLookup.Add("log", UnaryMathOperators.Log);
            unaryMathOperatorsLookup.Add("log10", UnaryMathOperators.Log);
            unaryMathOperatorsLookup.Add("floor", UnaryMathOperators.Floor);
            unaryMathOperatorsLookup.Add("ceiling", UnaryMathOperators.Ceiling);
            unaryMathOperatorsLookup.Add("factorial", UnaryMathOperators.Factorial);

            binaryLogicOperatorsLookup.Add("and", BinaryLogicOperators.And);
            binaryLogicOperatorsLookup.Add("or", BinaryLogicOperators.Or);
            binaryLogicOperatorsLookup.Add("xor", BinaryLogicOperators.Xor);

            unaryLogicOperatorsLookup.Add("not", UnaryLogicOperators.Not);

            unaryMathOperatorsLookup.Add("sin", UnaryMathOperators.Sin);
            unaryMathOperatorsLookup.Add("cos", UnaryMathOperators.Cos);
            unaryMathOperatorsLookup.Add("tan", UnaryMathOperators.Tan);
            unaryMathOperatorsLookup.Add("sec", UnaryMathOperators.Sec);
            unaryMathOperatorsLookup.Add("csc", UnaryMathOperators.Csc);
            unaryMathOperatorsLookup.Add("cot", UnaryMathOperators.Cot);
            unaryMathOperatorsLookup.Add("sinh", UnaryMathOperators.Sinh);
            unaryMathOperatorsLookup.Add("cosh", UnaryMathOperators.Cosh);
            unaryMathOperatorsLookup.Add("tanh", UnaryMathOperators.Tanh);
            unaryMathOperatorsLookup.Add("sech", UnaryMathOperators.Sech);
            unaryMathOperatorsLookup.Add("csch", UnaryMathOperators.Csch);
            unaryMathOperatorsLookup.Add("coth", UnaryMathOperators.Coth);
            unaryMathOperatorsLookup.Add("arcsin", UnaryMathOperators.Arcsin);
            unaryMathOperatorsLookup.Add("arccos", UnaryMathOperators.Arccos);
            unaryMathOperatorsLookup.Add("arctan", UnaryMathOperators.Arctan);
            unaryMathOperatorsLookup.Add("arcsec", UnaryMathOperators.Arcsec);
            unaryMathOperatorsLookup.Add("arccsc", UnaryMathOperators.Arccsc);
            unaryMathOperatorsLookup.Add("arccot", UnaryMathOperators.Arccot);
            unaryMathOperatorsLookup.Add("arcsinh", UnaryMathOperators.Arcsinh);
            unaryMathOperatorsLookup.Add("arccosh", UnaryMathOperators.Arccosh);
            unaryMathOperatorsLookup.Add("arctanh", UnaryMathOperators.Arctanh);
            unaryMathOperatorsLookup.Add("arcsech", UnaryMathOperators.Arcsech);
            unaryMathOperatorsLookup.Add("arccsch", UnaryMathOperators.Arccsch);
            unaryMathOperatorsLookup.Add("arccoth", UnaryMathOperators.Arccoth);

            mathConstantsLookup.Add("true", MathConstants.True);
            mathConstantsLookup.Add("false", MathConstants.False);
            mathConstantsLookup.Add("pi", MathConstants.Pi);
            mathConstantsLookup.Add("exponential", MathConstants.Exponential);
            mathConstantsLookup.Add("notanumber", MathConstants.Notanumber);
            mathConstantsLookup.Add("infinity", MathConstants.Infinity);

            relationOperatorsLookup.Add("lt", RelationOperators.Lt);
            relationOperatorsLookup.Add("gt", RelationOperators.Gt);
            relationOperatorsLookup.Add("eq", RelationOperators.Eq);
            relationOperatorsLookup.Add("neq", RelationOperators.Neq);
            relationOperatorsLookup.Add("leq", RelationOperators.Leq);
            relationOperatorsLookup.Add("geq", RelationOperators.Geq);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool Parse(string filePath)
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(filePath);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                while (reader.Read())
                {
                    if (this.inMathNode == true)
                    {
                        ParseMath(reader);
                        continue;

                    }

                    if (this.skipNode == true)
                    {
                        if (!(reader.NodeType == XmlNodeType.EndElement &&
                            reader.LocalName.ToLower() == "annotation"))
                        {
                            this.skipNode = false;
                        }
                        continue;
                    }

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            //if (reader.Prefix != String.Empty)
                            //{
                            //    // namespaced element not part of SBML
                            //    reader.Skip();
                            //}
                            String elementName = reader.LocalName.ToLower();

                            if (elementName == "annotation" &&
                                reader.HasAttributes &&
                                reader.MoveToAttribute("xmlns") &&
                                reader.Value == "http://example.com/MuCell")
                            {
                                // continue down, do not fall into else branch
                            }
                            else if (elementName == "notes" ||
                                elementName == "annotation" ||
                                reader.Prefix != String.Empty)
                            {
                                // not part of the SBML model
                                this.skipNode = true;
                                continue;
                            }

                            Hashtable attributes = new Hashtable();
                            if (reader.HasAttributes)
                            {
                                for (int i = 0; i < reader.AttributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);
                                    if (reader.Name == "xmlns" && elementName != "math"
                                        && elementName != "sbml" && reader.Value != "http://example.com/MuCell")
                                    {
                                        // namespaced node/subtree not part of SBML...unless MathML
                                        // todo: could be part of annotation
                                        reader.MoveToElement();
                                        reader.Skip();
                                    }
                                    if (reader.Name == "specie")
                                    {
                                        // horrible leftover from SBML Level 1
                                        attributes.Add("species", reader.Value);
                                    }
                                    else
                                    {
                                        attributes.Add(reader.Name.ToLower(), reader.Value);
                                    }
                                }
                            }
                            StartElement(elementName, attributes);
                            break;

                        case XmlNodeType.EndElement:
                            elementName = reader.LocalName.ToLower();
                            EndElement(elementName);
                            break;
                        // There are many other types of nodes, but
                        // we are not interested in them
                    }
                }
                // Success
                // Process duplicates
                this.model.processDuplicates();
                return true;
            }
            catch (DuplicateSBMLObjectIdException e)
            {
                // Failure
                return false;
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.Message);
                // Failure
                return false;
            }
        }

        public void ParseMath(XmlTextReader reader)
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    String elementName = reader.LocalName.ToLower();

                    Hashtable attributes = new Hashtable();
                    if (reader.HasAttributes)
                    {
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            reader.MoveToAttribute(i);
                            attributes.Add(reader.Name.ToLower(), reader.Value);
                        }
                    }
                    if (elementName == "csymbol")
                    {
                        reader.Skip(); // csymbol will have an unimportant text childNode
                    }

                    StartMathElement(elementName, attributes);
                    break;

                case XmlNodeType.EndElement:
                    EndMathElement(reader.Name.ToLower());
                    break;

                case XmlNodeType.Text: // from cn or ci
                    MathCharacters(reader.Value);
                    break;

            }

        }

        public void StartMathElement(String elementName, Hashtable attrs)
        {
            switch (elementName)
            {
                case "cn":
                    MathCnElement(attrs);
                    break;
                case "ci":
                    MathCiElement(attrs);
                    break;
                case "csymbol":
                    MathCsymbolElement(attrs);
                    break;
                //case "sep":
                //    MathSepElement();
                //    break;
                case "apply":
                    MathApplyElement();
                    break;
                default:
                    CheckMathOperators(elementName);
                    break;
            }

            // debugging!
            // 
            //MathTree mathTree = (MathTree)elementStack.Peek();
            //if (mathTree.nodeStack.Count > 0)
            //{
            //    MathNode node = mathTree.getCurrentNode();
            //   MessageBox.Show(elementName + "\n\nMath node stack: " + node.ToString());
            //}
            //else
            //{
            //    MessageBox.Show(elementName + "\n\nMath node stack is empty");
            //}
        }

        public void EndMathElement(String elementName)
        {
            MathTree mathElement = (MathTree)elementStack.Peek();

            // debugging!

            // System.Console.WriteLine(mathElement.nodeStack.Count); 

            //if (mathElement.nodeStack.Count > 0)
            //{
            //    MathNode node = mathElement.getCurrentNode();
            //    MessageBox.Show("</"+elementName + ">\n\nMath node stack: " + node.ToString());
            //}
            //else
            //{
            //    MessageBox.Show(elementName + "\n\nMath node stack is empty");
            //}
            //if (elementName == "apply" || elementName == "ci" || elementName == "cn" || elementName == "csymbol")

            // Don't peek an apply
            if (elementName == "ci" || elementName == "cn" || elementName == "csymbol")
            {
                // pop from MathTree element.nodeStack            
                mathElement.nodeStack.Pop();
            }
            else if (elementName == "math")
            {
                this.inMathNode = false;
                elementStack.Pop();
            }
            else
            {
                // Case of an operation?


            }
        }

        public void MathCharacters(String text)
        {
            // grab current leafnode from mathtree.nodeStack
            MathTree mathElement = (MathTree)elementStack.Peek();
            LeafNode currentLeaf = (LeafNode)mathElement.getCurrentNode();

            currentLeaf.AddData(text.Trim(' '));
        }

        public void MathCnElement(Hashtable attrs)
        {
            String type = null;
            if (attrs.ContainsKey("type"))
                type = (String)attrs["type"];

            MathTree mathElement = (MathTree)elementStack.Peek();
            InnerNode currentNode = (InnerNode)mathElement.getCurrentNode();

            NumberLeafNode leaf = new NumberLeafNode();

            currentNode.AddNode(leaf);
            mathElement.nodeStack.Push(leaf);
        }

        public void MathCiElement(Hashtable attrs)
        {
            MathTree mathElement = (MathTree)elementStack.Peek();
            InnerNode currentNode = (InnerNode)mathElement.getCurrentNode();

            ReferenceLeafNode leaf = new ReferenceLeafNode();

            currentNode.AddNode(leaf);
            mathElement.nodeStack.Push(leaf);
        }

        public void CheckMathOperators(String elementName)
        {
            Enum validOperator = null;
            // check in all Operator Hashtables
            // send to MathOperatorElement

            if (binaryMathOperatorsLookup.ContainsKey(elementName))
            {
                validOperator = (BinaryMathOperators)binaryMathOperatorsLookup[elementName];
            }
            else if (unaryMathOperatorsLookup.ContainsKey(elementName))
            {
                validOperator = (UnaryMathOperators)unaryMathOperatorsLookup[elementName];
            }
            else if (binaryLogicOperatorsLookup.ContainsKey(elementName))
            {
                validOperator = (BinaryLogicOperators)binaryLogicOperatorsLookup[elementName];
            }
            else if (unaryLogicOperatorsLookup.ContainsKey(elementName))
            {
                validOperator = (UnaryLogicOperators)unaryLogicOperatorsLookup[elementName];
            }
            else if (relationOperatorsLookup.ContainsKey(elementName))
            {
                validOperator = (RelationOperators)relationOperatorsLookup[elementName];
            }
            else if (mathConstantsLookup.ContainsKey(elementName))
            {
                validOperator = (MathConstants)mathConstantsLookup[elementName];
            }

            if (validOperator != null)
                MathOperatorElement(validOperator);
        }

        public void MathOperatorElement(Enum argument)
        {
            // get current MathTree node
            MathTree mathElement = (MathTree)elementStack.Peek();
            InnerNode currentNode = (InnerNode)mathElement.getCurrentNode();
            currentNode.data = argument;
        }

        public void MathApplyElement()
        {
            //MathTree mathElement = (MathTree)elementStack.Peek();
            //InnerNode currentNode = (InnerNode)mathElement.getCurrentNode();

            //InnerNode newNode = new InnerNode();
            // add as new node to MathTree
            //currentNode.AddNode(newNode);
            // push to tree stack as current node
            //mathElement.nodeStack.Push(newNode);
        }

        public void MathCsymbolElement(Hashtable attrs)
        {
            // look for definitionUrl
            // should be either Time or Delay
            // add to current Node
            String url = null;
            CSymbol symbol;

            if (attrs.ContainsKey("definitionUrl"))
                url = (String)attrs["definitionUrl"];

            if (url == "http://www.sbml.org/sbml/symbols/delay")
            {
                symbol = CSymbol.Delay;
            }
            else
            {
                symbol = CSymbol.Time;
            }

            MathTree mathElement = (MathTree)elementStack.Peek();
            InnerNode currentNode = (InnerNode)mathElement.getCurrentNode();


            LeafNode leaf = new LeafNode();
            leaf.AddData(symbol);
            currentNode.AddNode(leaf);

        }

        public void StartElement(String elementName, Hashtable attrs)
        {

            switch (elementName.ToLower())
            {
                case "model":
                    ModelElement(attrs);
                    break;
                case "listoffunctiondefinitions":
                    ListOfFunctionDefinitionsElement(attrs);
                    break;
                case "functiondefinition":
                    FunctionDefinitionElement(attrs);
                    break;
                case "listofunitdefinitions":
                    ListOfUnitDefinitionsElement(attrs);
                    break;
                case "unitdefinition":
                    UnitDefinitionElement(attrs);
                    break;
                case "unit":
                    UnitElement(attrs);
                    break;
                case "listofcompartmenttypes":
                    ListOfCompartmentTypesElement(attrs);
                    break;
                case "compartmenttype":
                    CompartmentTypeElement(attrs);
                    break;
                case "listofspeciestypes":
                    ListOfSpeciesTypesElement(attrs);
                    break;
                case "specietype":
                case "speciestype":
                    SpeciesTypeElement(attrs);
                    break;
                case "listofcompartments":
                    ListOfCompartmentsElement(attrs);
                    break;
                case "compartment":
                    CompartmentElement(attrs);
                    break;
                case "listofspecies":
                    ListOfSpeciesElement(attrs);
                    break;
                case "specie": // defined this way in SBML 1(!)
                case "species":
                    SpeciesElement(attrs);
                    break;
                case "listofparameters":
                    ListOfParametersElement(attrs);
                    break;
                case "parameter":
                    ParameterElement(attrs);
                    break;
                case "listofinitialassignments":
                    ListOfInitialAssignmentsElement(attrs);
                    break;
                case "initialassignment":
                    InitialAssignmentElement(attrs);
                    break;
                case "listofrules":
                    ListOfRulesElement(attrs);
                    break;
                case "algebraicrule":
                    AlgebraicRuleElement(attrs);
                    break;
                case "assignmentrule":
                    AssignmentRuleElement(attrs);
                    break;
                case "raterule":
                    RateRuleElement(attrs);
                    break;
                case "listofconstraints":
                    ListOfConstraintsElement(attrs);
                    break;
                case "constraint":
                    ConstraintElement(attrs);
                    break;
                case "listofreactions":
                    ListOfReactionsElement(attrs);
                    break;
                case "reaction":
                    ReactionElement(attrs);
                    break;
                case "listofreactants":
                    ListOfReactantsElement(attrs);
                    break;
                case "listofproducts":
                    ListOfProductsElement(attrs);
                    break;
                case "listofmodifiers":
                    ListOfModifiersElement(attrs);
                    break;
                case "speciereference": // defined this way in SBML 1(!)
                case "speciesreference":
                    SpeciesReferenceElement(attrs);
                    break;
                case "modifierspeciesreference":
                    ModifierSpeciesReferenceElement(attrs);
                    break;
                case "kineticlaw":
                    KineticLawElement(attrs);
                    break;
                case "listofevents":
                    ListOfEventsElement(attrs);
                    break;
                case "event":
                    EventElement(attrs);
                    break;
                case "trigger":
                    TriggerElement(attrs);
                    break;
                case "delay":
                    DelayElement(attrs);
                    break;
                case "listofeventassignments":
                    ListOfEventAssignmentsElement(attrs);
                    break;
                case "eventassignment":
                    EventAssignmentElement(attrs);
                    break;
                case "math":
                    MathElement(attrs);
                    break;
                case "stoichiometrymath":
                    StoichiometryMathElement(attrs);
                    break;
                case "xposition":
                    XPositionElement(attrs);
                    break;
                case "yposition":
                    YPositionElement(attrs);
                    break;

                default:
                    break;
            }

        }

        public void EndElement(String name)
        {
            //MessageBox.Show("</"+name+">");

            if (name == "kineticlaw")
            {
                // If are ending a kineticlaw
                // evaluate its formula now that the parameters have been passed
                KineticLaw kw = (KineticLaw)elementStack.Peek();
                kw.evaluateFormula();
            }

            if (name == "unitdefinition" ||
                name == "algebraicrule" ||
                name == "assignmentrule" ||
                name == "raterule" ||
                name == "functiondefinition" ||
                name == "constraint" ||
                name == "reaction" ||
                name == "listofreactants" ||
                name == "listofproducts" ||
                name == "event" ||
                name == "trigger" ||
                name == "delay" ||
                name == "eventassignment" ||
                name == "kineticlaw" ||
                name == "math")
            {
                Console.WriteLine(elementStack.Pop());
            }

            if (name == "listofreactants" ||
                name == "listofproducts")
            {
                elementStack.Pop();
            }

            if (name == "math")
            {
                this.inMathNode = false;
            }

        }

        public void ModelElement(Hashtable attrs)
        {
            if (attrs.ContainsKey("name"))
            {
                this.model = new Model((String)attrs["name"]);
            }
            else if (attrs.ContainsKey("id"))
            {
                this.model = new Model((string)attrs["id"]);
            }
            else
                this.model = new Model();
        }

        public void ListOfFunctionDefinitionsElement(Hashtable attrs)
        {
            this.model.listOfFunctionDefinitions = new List<FunctionDefinition>();
        }

        public void FunctionDefinitionElement(Hashtable attrs)
        {
            FunctionDefinition functionDef = new FunctionDefinition(attrs);
            if (functionDef.ID != null)
            {
                this.model.AddId(functionDef.ID, functionDef);
            }
            this.model.listOfFunctionDefinitions.Add(functionDef);

            elementStack.Push(functionDef);
        }

        public void ListOfUnitDefinitionsElement(Hashtable attrs)
        {
            this.model.listOfUnitDefinitions = new List<UnitDefinition>();
        }

        public void UnitDefinitionElement(Hashtable attrs)
        {
            UnitDefinition unitDef = new UnitDefinition(attrs);
            if (unitDef.ID != null)
            {
                this.model.AddId(unitDef.ID, unitDef);
            }
            this.model.listOfUnitDefinitions.Add(unitDef);
            elementStack.Push(unitDef);
        }

        public void UnitElement(Hashtable attrs)
        {
            UnitDefinition unitDef = (UnitDefinition)elementStack.Peek();
            String kind = null;
            int exponent = 1;
            int scale = 0;
            double multiplier = 1;

            if (attrs.ContainsKey("kind"))
                kind = (String)attrs["kind"];
            if (attrs.ContainsKey("exponent"))
                exponent = (int)(Int32.Parse((String)attrs["exponent"]));
            if (attrs.ContainsKey("scale"))
                scale = (int)(Int32.Parse((String)attrs["scale"]));
            if (attrs.ContainsKey("multiplier"))
                multiplier = (double)(double.Parse((String)attrs["multiplier"]));

            if (this.model.IsUnits(kind) == true)
            {
                Unit unit = new Unit(kind, exponent, scale, multiplier);
                unitDef.listOfUnits.Add(unit);
            }



        }

        public void ListOfCompartmentTypesElement(Hashtable attrs)
        {
            this.model.listOfCompartmentTypes = new List<CompartmentType>();
        }

        public void CompartmentTypeElement(Hashtable attrs)
        {
            CompartmentType compType = new CompartmentType(attrs);
            if (compType.ID != null)
            {
                this.model.AddId(compType.ID, compType);
            }

            this.model.listOfCompartmentTypes.Add(compType);
        }

        public void ListOfSpeciesTypesElement(Hashtable attrs)
        {
            this.model.listOfSpeciesTypes = new List<SpeciesType>();
        }

        public void SpeciesTypeElement(Hashtable attrs)
        {
            SpeciesType specType = new SpeciesType(attrs);
            if (specType.ID != null)
            {
                this.model.AddId(specType.ID, specType);
            }
            this.model.listOfSpeciesTypes.Add(specType);
        }

        public void ListOfCompartmentsElement(Hashtable attrs)
        {
            this.model.listOfCompartments = new List<Compartment>();
        }

        public void CompartmentElement(Hashtable attrs)
        {
            Compartment compartment = new Compartment(attrs);

            CompartmentType compartmentType = null;
            int spatialDimensions = 3;
            double size = 1;
            String units = null; // unit enum or user-defined unit
            Compartment outside = null;
            Boolean constant = true;

            if (attrs.ContainsKey("constant"))
            {
                constant = Boolean.Parse((String)attrs["constant"]);
			}
			
            if (attrs.ContainsKey("spatialdimensions"))
            {
                spatialDimensions = (int)(Int32.Parse((String)attrs["spatialdimensions"]));
			}
			
            if (attrs.ContainsKey("size"))
            {
                size = (double)(double.Parse((String)attrs["size"]));
			}
            else if (attrs.ContainsKey("volume")) // from SBML Level 1
            {
                size = (double)(double.Parse((String)attrs["volume"]));
			}
			
            if (attrs.ContainsKey("units") && this.model.IsUnits((String)attrs["units"]))
            {
                units = (String)attrs["units"];
			}
            else
            {
                switch (spatialDimensions)
                {
                    case 1:
                        units = "length";
                        break;
                    case 2:
                        units = "area";
                        break;
                    case 3:
                        units = "volume";
                        break;
                }
            }

            if (attrs.ContainsKey("compartmenttype"))
            {
                String cTypeId = (String)attrs["compartmenttype"];
                compartmentType = (CompartmentType)this.model.findObject(cTypeId);
            }
            if (attrs.ContainsKey("outside"))
            {
                String cId = (String)attrs["outside"];
                outside = (Compartment)this.model.findObject(cId);
            }

            compartment.AddProperties(compartmentType, spatialDimensions, size,
                units, outside, constant);
            if (compartment.ID != null)
            {
                this.model.AddId(compartment.ID, compartment);
            }

            this.model.listOfCompartments.Add(compartment);
        }

        public void ListOfSpeciesElement(Hashtable attrs)
        {
            this.model.listOfSpecies = new List<Species>();
        }

        public void SpeciesElement(Hashtable attrs)
        {
            Species species = new Species(attrs);
            if (species.ID != null)
            {
                this.model.AddId(species.ID, species);
            }

            SpeciesType speciesType = null;
            Compartment compartment = null;
            Double initialAmount = -1d;
            Double initialConcentration = -1d;
            String substanceUnits = null;
            Boolean hasOnlySubstanceUnits = false;
            Boolean boundaryCondition = false;
            Boolean constant = false;

            if (attrs.ContainsKey("initialamount"))
                initialAmount = Double.Parse((String)attrs["initialamount"]);
            if (attrs.ContainsKey("initialconcentration"))
                initialConcentration = Double.Parse((String)attrs["initialconcentration"]);
            if (attrs.ContainsKey("substanceunits"))
                substanceUnits = (String)attrs["substanceunits"];
            if (attrs.ContainsKey("hasonlysubstanceunits"))
                hasOnlySubstanceUnits = Boolean.Parse((String)attrs["hasonlysubstanceunits"]);
            if (attrs.ContainsKey("boundarycondition"))
                boundaryCondition = Boolean.Parse((String)attrs["boundarycondition"]);
            if (attrs.ContainsKey("constant"))
                constant = Boolean.Parse((String)attrs["constant"]);

            if (attrs.ContainsKey("speciestype"))
            {
                String sTypeId = (String)attrs["speciesType"];
                speciesType = (SpeciesType)this.model.findObject(sTypeId);
            }

            if (attrs.ContainsKey("compartment"))
            {
                String cId = (String)attrs["compartment"];
                compartment = (Compartment)this.model.findObject(cId);
            }

            if (attrs.ContainsKey("substanceunits"))
            {
                substanceUnits = (String)attrs["substanceunits"];
            }
            else
            {
                // look it up based on units of compartment;
            }

            species.AddProperties(speciesType, compartment, initialAmount,
                initialConcentration, substanceUnits, hasOnlySubstanceUnits, boundaryCondition,
                constant);

            this.model.listOfSpecies.Add(species);
        }

        public void XPositionElement(Hashtable attrs)
        {
            Species species = this.model.listOfSpecies[this.model.listOfSpecies.Count-1];
            species.xPosition = float.Parse((String)attrs["value"]);

        }

        public void YPositionElement(Hashtable attrs)
        {
            Species species = this.model.listOfSpecies[this.model.listOfSpecies.Count - 1];
            species.yPosition = float.Parse((String)attrs["value"]);
        }



        public void ListOfParametersElement(Hashtable attrs)
        {
            if (elementStack.Count == 0)
            // top-level element under <model>
            {
                this.model.listOfParameters = new List<Parameter>();
            }
            else
            // <listOfParameters> under <kineticLaw> under <reaction>
            {
                KineticLaw kineticLaw = (KineticLaw)elementStack.Peek();
                kineticLaw.listOfParameters = new List<Parameter>();
            }
        }

        public void ParameterElement(Hashtable attrs)
        {
            Parameter parameter = new Parameter(attrs);
            if (parameter.ID != null)
            {
                this.model.AddId(parameter.ID, parameter);
            }

            double value = 1d;
            String units = null;
            Boolean constant = true;

            if (attrs.ContainsKey("value"))
                value = Double.Parse((String)attrs["value"]);
            if (attrs.ContainsKey("units"))
                units = (String)attrs["units"];
            if (attrs.ContainsKey("constant"))
                constant = Boolean.Parse((String)attrs["constant"]);

            parameter.AddProperties(value, units, constant);

            if (elementStack.Count == 0)
            // from top-level element <listOfParameters> under <model>
            {
                this.model.listOfParameters.Add(parameter);
            }
            else
            // from <listOfParameters> under <kineticLaw> under <reaction>
            {
                KineticLaw kineticLaw = (KineticLaw)elementStack.Peek();
                kineticLaw.listOfParameters.Add(parameter);
            }

        }

        public void ListOfInitialAssignmentsElement(Hashtable attrs)
        {
            this.model.listOfInitialAssignments = new List<InitialAssignment>();
        }

        public void InitialAssignmentElement(Hashtable attrs)
        {
            String symbol;
            SBase variable = null;

            if (attrs.ContainsKey("symbol"))
            {
                symbol = (String)attrs["symbol"];

                if (this.model.findObject(symbol) != null)
                    variable = (SBase)this.model.findObject(symbol);

                InitialAssignment initialAssignment = new InitialAssignment(variable);
                //                if (initialAssignment.ID != null)
                //                {
                //                    this.model.AddId(initialAssignment.ID, initialAssignment);
                //                }
                this.model.listOfInitialAssignments.Add(initialAssignment);

                elementStack.Push(initialAssignment);
            }
            // else throw exception on required attribute        
        }

        public void ListOfRulesElement(Hashtable attrs)
        {
            this.model.listOfRules = new List<Rule>();
        }

        public void AlgebraicRuleElement(Hashtable attrs)
        {
            AlgebraicRule algebraicRule = new AlgebraicRule();
            if (algebraicRule.ID != null)
            {
                this.model.AddId(algebraicRule.ID, algebraicRule);
            }

            this.model.listOfRules.Add(algebraicRule);

            elementStack.Push(algebraicRule);
        }

        public void AssignmentRuleElement(Hashtable attrs)
        {
            String varID;
            SBase variable = null;

            if (attrs.ContainsKey("variable"))
            {
                varID = (String)attrs["variable"];

                if (this.model.findObject(varID) != null)
                    variable = (SBase)this.model.findObject(varID);
            }
            // else throw exception on required attribute  

            AssignmentRule assignmentRule = new AssignmentRule(variable);
            if (assignmentRule.ID != null)
            {
                this.model.AddId(assignmentRule.ID, assignmentRule);
            }

            this.model.listOfRules.Add(assignmentRule);

            elementStack.Push(assignmentRule);
        }

        public void RateRuleElement(Hashtable attrs)
        {
            String varID;
            SBase variable = null;

            if (attrs.ContainsKey("variable"))
            {
                varID = (String)attrs["variable"];

                if (this.model.findObject(varID) != null)
                    variable = (SBase)this.model.findObject(varID);
            }
            // else throw exception on required attribute  

            RateRule rateRule = new RateRule(variable);
            this.model.listOfRules.Add(rateRule);

            elementStack.Push(rateRule);
        }

        public void ListOfConstraintsElement(Hashtable attrs)
        {
            this.model.listOfConstraints = new List<Constraint>();
        }

        public void ConstraintElement(Hashtable attrs)
        {
            Constraint constraint = new Constraint();
            this.model.listOfConstraints.Add(constraint);

            elementStack.Push(constraint);
        }

        public void ListOfReactionsElement(Hashtable attrs)
        {
            this.model.listOfReactions = new List<Reaction>();
        }

        public void ReactionElement(Hashtable attrs)
        {
            Reaction reaction = new Reaction(attrs);
            if (reaction.ID != null)
            {
                this.model.AddId(reaction.ID, reaction);
            }
            Boolean fast = false;
            Boolean reversible = true;

            if (attrs.ContainsKey("fast"))
                fast = Boolean.Parse((String)attrs["fast"]);
            if (attrs.ContainsKey("reversible"))
                reversible = Boolean.Parse((String)attrs["reversible"]);

            reaction.AddProperties(fast, reversible);
            this.model.listOfReactions.Add(reaction);

            elementStack.Push(reaction);
        }

        public void ListOfReactantsElement(Hashtable attrs)
        {
            Reaction reaction = (Reaction)elementStack.Peek();
            reaction.Reactants = new List<SpeciesReference>();
            elementStack.Push(reaction.Reactants);
            elementStack.Push("listOfReactants");
        }

        public void ListOfProductsElement(Hashtable attrs)
        {
            Reaction reaction = (Reaction)elementStack.Peek();
            reaction.Products = new List<SpeciesReference>();
            elementStack.Push(reaction.Products);
            elementStack.Push("listOfProducts");
        }

        public void ListOfModifiersElement(Hashtable attrs)
        {
            Reaction reaction = (Reaction)elementStack.Peek();
            reaction.Modifiers = new List<ModifierSpeciesReference>();
        }

        public void SpeciesReferenceElement(Hashtable attrs)
        {
            String speciesId;
            Species species;

            try
            {
                speciesId = (String)attrs["species"];
                species = (Species)this.model.findObject(speciesId);

                // find out if it is a list of reactants or products
                if (((String)elementStack.Pop()) == "listOfReactants")
                {
                    SpeciesReference speciesRef;
                    List<SpeciesReference> reactantsList = (List<SpeciesReference>)elementStack.Peek();

                    if (attrs.ContainsKey("stoichiometry"))
                    {
                        speciesRef = new SpeciesReference(species,
                            (double)(double.Parse((String)attrs["stoichiometry"])));
                    }
                    else
                    {
                        speciesRef = new SpeciesReference(species);
                    }

                    reactantsList.Add(speciesRef);
                    elementStack.Push("listOfReactants");
                }

                else
                // list of products instead
                {
                    SpeciesReference speciesRef;
                    List<SpeciesReference> productsList = (List<SpeciesReference>)elementStack.Peek();

                    if (attrs.ContainsKey("stoichiometry"))
                    {
                        speciesRef = new SpeciesReference(species,
                            (double)(double.Parse((String)attrs["stoichiometry"])));
                    }
                    else
                    {
                        speciesRef = new SpeciesReference(species);
                    }

                    productsList.Add(speciesRef);
                    elementStack.Push("listOfProducts");
                }

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("speciesReference element missing required attribute 'species'");
                throw (e);
            }

        }

        public void ModifierSpeciesReferenceElement(Hashtable attrs)
        {
            String speciesId;
            Species species;

            try
            {
                speciesId = (String)attrs["species"];
                species = (Species)this.model.findObject(speciesId);

                Reaction reaction = (Reaction)elementStack.Peek();
                ModifierSpeciesReference modifierRef = new ModifierSpeciesReference(species);
                reaction.Modifiers.Add(modifierRef);
            }
            catch (Exception e)
            {
                Console.WriteLine("modifierSpeciesReference element missing required attribute 'species'");
                throw (e);
            }
        }

        public void KineticLawElement(Hashtable attrs)
        {
            Reaction reaction = (Reaction)elementStack.Peek();

            KineticLaw kLaw = new KineticLaw(this.model, attrs);
            if (kLaw.ID != null)
            {
                this.model.AddId(kLaw.ID, kLaw);
            }
            reaction.KineticLaw = kLaw;

            elementStack.Push(kLaw);
        }

        public void ListOfEventsElement(Hashtable attrs)
        {
            this.model.listOfEvents = new List<Event>();
        }

        public void EventElement(Hashtable attrs)
        {
            Event eventNode = new Event(attrs);
            if (eventNode.ID != null)
            {
                this.model.AddId(eventNode.ID, eventNode);
            }
            this.model.listOfEvents.Add(eventNode);

            elementStack.Push(eventNode);
        }

        public void TriggerElement(Hashtable attrs)
        {
            Event eventNode = (Event)elementStack.Peek();
            eventNode.trigger = new Trigger(eventNode);
            elementStack.Push(eventNode.trigger);
        }

        public void DelayElement(Hashtable attrs)
        {
            Event eventNode = (Event)elementStack.Peek();
            eventNode.delay = new Delay(eventNode);
            elementStack.Push(eventNode.delay);
        }

        public void ListOfEventAssignmentsElement(Hashtable attrs)
        {
            Event eventNode = (Event)elementStack.Peek();
            eventNode.listOfEventAssignments = new List<EventAssignment>();
        }

        public void EventAssignmentElement(Hashtable attrs)
        {
            String varID;
            SBase variable = null;

            if (attrs.ContainsKey("variable"))
            {
                varID = (String)attrs["variable"];

                if (this.model.findObject(varID) != null)
                    variable = (SBase)this.model.findObject(varID);
            }

            EventAssignment eventAssignment = new EventAssignment(variable);

            Event eventNode = (Event)elementStack.Peek();
            eventNode.listOfEventAssignments.Add(eventAssignment);

            elementStack.Push(eventAssignment);
        }

        public void MathElement(Hashtable attrs)
        {
            this.inMathNode = true;

            // find what this <math> element belongs to
            MathBase currentElement = (MathBase)elementStack.Peek();
            // create MathTree, push to stack
            currentElement.math = new MathTree();
            elementStack.Push(currentElement.math);
        }

        public void StoichiometryMathElement(Hashtable attrs)
        {
            // must be from a speciesReference 
            //
            // cannot guarantee speciesReference removal from stack
            // on EndElement since it may be an empty element
            // Therefore we cannot put it on the stack to begin with
            // But we can put the list of reactants/products on the stack
            // and grab the last one.

            String listType = (String)elementStack.Pop();
            List<SpeciesReference> listOfRefs = (List<SpeciesReference>)elementStack.Peek();
            elementStack.Push(listType);
            SpeciesReference lastRef = listOfRefs[listOfRefs.Count - 1];

            elementStack.Push(lastRef);

        }

    }

}
