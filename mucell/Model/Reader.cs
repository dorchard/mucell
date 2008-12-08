using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.Model;

namespace MuCell.Model.SBML.Reader
{
    public class SBMLReader
{
    Stack elementStack = new Stack();
    StringBuilder value = new StringBuilder();
    Model model;
    Boolean inMathNode = false;

    Hashtable OperatorsTbl = new Hashtable();
    Hashtable LogicOperatorsTbl = new Hashtable();
    Hashtable TrigOperatorsTbl = new Hashtable();
    Hashtable MathConstantsTbl = new Hashtable();
    Hashtable RelationOperatorsTbl = new Hashtable();

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
        OperatorsTbl.Add("plus", MathOperators.Plus);
        OperatorsTbl.Add("minus", MathOperators.Minus);
        OperatorsTbl.Add("times", MathOperators.Times);
        OperatorsTbl.Add("divide", MathOperators.Divide);
        OperatorsTbl.Add("power", MathOperators.Power);
        OperatorsTbl.Add("root", MathOperators.Root);
        OperatorsTbl.Add("abs", MathOperators.Abs);
        OperatorsTbl.Add("exp", MathOperators.Exp);
        OperatorsTbl.Add("ln", MathOperators.Ln);
        OperatorsTbl.Add("log", MathOperators.Log);
        OperatorsTbl.Add("floor", MathOperators.Floor);
        OperatorsTbl.Add("ceiling", MathOperators.Ceiling);
        OperatorsTbl.Add("factorial", MathOperators.Factorial);

        LogicOperatorsTbl.Add("and", LogicOperators.And);
        LogicOperatorsTbl.Add("or", LogicOperators.Or);
        LogicOperatorsTbl.Add("not", LogicOperators.Not);
        LogicOperatorsTbl.Add("xor", LogicOperators.Xor);

        TrigOperatorsTbl.Add("sin", TrigOperators.Sin);
        TrigOperatorsTbl.Add("cos", TrigOperators.Cos);
        TrigOperatorsTbl.Add("tan", TrigOperators.Tan);
        TrigOperatorsTbl.Add("sec", TrigOperators.Sec);
        TrigOperatorsTbl.Add("csc", TrigOperators.Csc);
        TrigOperatorsTbl.Add("cot", TrigOperators.Cot);
        TrigOperatorsTbl.Add("sinh", TrigOperators.Sinh);
        TrigOperatorsTbl.Add("cosh", TrigOperators.Cosh);
        TrigOperatorsTbl.Add("tanh", TrigOperators.Tanh);
        TrigOperatorsTbl.Add("sech", TrigOperators.Sech);
        TrigOperatorsTbl.Add("csch", TrigOperators.Csch);
        TrigOperatorsTbl.Add("coth", TrigOperators.Coth);
        TrigOperatorsTbl.Add("arcsin", TrigOperators.Arcsin);
        TrigOperatorsTbl.Add("arccos", TrigOperators.Arccos);
        TrigOperatorsTbl.Add("arctan", TrigOperators.Arctan);
        TrigOperatorsTbl.Add("arcsec", TrigOperators.Arcsec);
        TrigOperatorsTbl.Add("arccsc", TrigOperators.Arccsc);
        TrigOperatorsTbl.Add("arccot", TrigOperators.Arccot);
        TrigOperatorsTbl.Add("arcsinh", TrigOperators.Arcsinh);
        TrigOperatorsTbl.Add("arccosh", TrigOperators.Arccosh);
        TrigOperatorsTbl.Add("arctanh", TrigOperators.Arctanh);
        TrigOperatorsTbl.Add("arcsech", TrigOperators.Arcsech);
        TrigOperatorsTbl.Add("arccsch", TrigOperators.Arccsch);
        TrigOperatorsTbl.Add("arccoth", TrigOperators.Arccoth);

        MathConstantsTbl.Add("true", MathConstants.True);
        MathConstantsTbl.Add("false", MathConstants.False);
        MathConstantsTbl.Add("pi", MathConstants.Pi);
        MathConstantsTbl.Add("exponentiale", MathConstants.Exponentiale);
        MathConstantsTbl.Add("notanumber", MathConstants.Notanumber);
        MathConstantsTbl.Add("infinity", MathConstants.Infinity);

        RelationOperatorsTbl.Add("lt", RelationOperators.Lt);
        RelationOperatorsTbl.Add("gt", RelationOperators.Gt);
        RelationOperatorsTbl.Add("eq", RelationOperators.Eq);
        RelationOperatorsTbl.Add("neq", RelationOperators.Neq);
        RelationOperatorsTbl.Add("leq", RelationOperators.Leq);
        RelationOperatorsTbl.Add("geq", RelationOperators.Geq);
    }

    public void Parse(string filePath)
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

                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Prefix != String.Empty)
                        {
                            // namespaced element not part of SBML
                            reader.Skip();
                        }
                        String elementName = reader.LocalName.ToLower();

                        if (elementName == "notes" || 
                            elementName == "annotation")
                        {
                            // not part of the SBML model
                            //reader.Skip(); // need to skip then move back one node
                            reader.Read();
                        }

                        Hashtable attributes = new Hashtable();
                        if (reader.HasAttributes)
                        {
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                if (reader.Name == "xmlns" && elementName != "math"
                                    && elementName != "sbml")
                                {
                                    // namespaced node/subtree not part of SBML...unless MathML
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
                        EndElement(reader.LocalName.ToLower());
                        break;
                    // There are many other types of nodes, but
                    // we are not interested in them
                }
            }
        }
        catch (XmlException e)
        {
            Console.WriteLine(e.Message);
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
                MathCiElement();
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
        //    MessageBox.Show(elementName + "\n\nMath node stack: " + node.ToString());
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
        //
        //if (mathElement.nodeStack.Count > 0)
        //{
        //    MathNode node = mathElement.getCurrentNode();
        //    MessageBox.Show("</"+elementName + ">\n\nMath node stack: " + node.ToString());
        //}
        //else
        //{
        //    MessageBox.Show(elementName + "\n\nMath node stack is empty");
        //}

        if (elementName == "apply" || elementName == "ci" || elementName == "cn")
        {
            // pop from MathTree element.nodeStack            
            mathElement.nodeStack.Pop();
        }
        if (elementName == "math")
        {
            this.inMathNode = false;
            elementStack.Pop();
        }
    }

    public void MathCharacters(String text)
    {
        // grab current leafnode from mathtree.nodeStack
        MathTree mathElement = (MathTree)elementStack.Peek();
        LeafNode currentLeaf = (LeafNode)mathElement.getCurrentNode();

        currentLeaf.AddData(text);
    }

    public void MathCnElement(Hashtable attrs)
    {
        String type = null;
        if (attrs.Contains("type"))
            type = (String)attrs["type"];

        MathTree mathElement = (MathTree)elementStack.Peek();
        OperatorNode currentNode = (OperatorNode)mathElement.getCurrentNode();

        LeafNode leaf = new LeafNode(this.model, currentNode, type);
        currentNode.AddNode(leaf);
        mathElement.nodeStack.Push(leaf);
    }

    public void MathCiElement()
    {
        MathTree mathElement = (MathTree)elementStack.Peek();
        OperatorNode currentNode = (OperatorNode)mathElement.getCurrentNode();

        LeafNode leaf = new LeafNode(this.model, currentNode, "string");
        currentNode.AddNode(leaf);
        mathElement.nodeStack.Push(leaf);
    }

    public void CheckMathOperators(String elementName)
    {
        Enum validOperator = null;
        // check in all Operator Hashtables
        // send to MathOperatorElement

        if (OperatorsTbl.Contains(elementName))
        {
            validOperator = (MathOperators)OperatorsTbl[elementName];
        }
        else if (LogicOperatorsTbl.Contains(elementName))
        {
            validOperator = (LogicOperators)LogicOperatorsTbl[elementName];
        }
        else if (TrigOperatorsTbl.Contains(elementName))
        {
            validOperator = (TrigOperators)TrigOperatorsTbl[elementName];
        }
        else if (RelationOperatorsTbl.Contains(elementName))
        {
            validOperator = (RelationOperators)RelationOperatorsTbl[elementName];
        }
        else if (MathConstantsTbl.Contains(elementName))
        {
            validOperator = (MathConstants)MathConstantsTbl[elementName];
        }

        if (validOperator != null)
            MathOperatorElement(validOperator);
    }

    public void MathOperatorElement(Enum argument)
    {
        // get current MathTree node
        // currentNode.data = (Operator)Hashtable[elementName]
        MathTree mathElement = (MathTree)elementStack.Peek();
        OperatorNode currentNode = (OperatorNode)mathElement.getCurrentNode();
        currentNode.data = argument;
    }

    public void MathApplyElement()
    {
        MathTree mathElement = (MathTree)elementStack.Peek();
        OperatorNode currentNode = (OperatorNode)mathElement.getCurrentNode();

        OperatorNode newNode = new OperatorNode(currentNode);
        // add as new node to MathTree
        currentNode.AddNode(newNode);
        // push to tree stack as current node
        mathElement.nodeStack.Push(newNode);
    }


    public void MathCsymbolElement(Hashtable attrs)
    {
        // look for definitionUrl
        // should be either Time or Delay
        // add to current Node
        String url = null;
        CSymbol symbol;

        if (attrs.Contains("definitionUrl"))
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
        OperatorNode currentNode = (OperatorNode)mathElement.getCurrentNode();


        LeafNode leaf = new LeafNode(this.model, currentNode, symbol);
        currentNode.AddNode(leaf);

    }


    public void StartElement(String elementName, Hashtable attrs)
    {
        if (elementStack.Count > 0)
        {
            MessageBox.Show(elementName + "\n\nStack: " + elementStack.Peek().ToString());
        }
        else
        {
            MessageBox.Show(elementName + "\n\nStack is empty");
        }
        
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

            default:
                break;
        }

    }

    public void EndElement(String name)
    {
        MessageBox.Show("</"+name+">");

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
            elementStack.Pop(); 
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

        if (name == "model")
        {
            MessageBox.Show("end of model");
        }

    }

    public void ModelElement(Hashtable attrs)
    {
        if (attrs.ContainsKey("name"))
        {
            this.model = new Model((String)attrs["name"]);
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
        FunctionDefinition functionDef = new FunctionDefinition(this.model, attrs);
        this.model.listOfFunctionDefinitions.Add(functionDef);

        elementStack.Push(functionDef);
    }

    public void ListOfUnitDefinitionsElement(Hashtable attrs)
    {
        this.model.listOfUnitDefinitions = new List<UnitDefinition>();
    }

    public void UnitDefinitionElement(Hashtable attrs)
    {
        UnitDefinition unitDef = new UnitDefinition(this.model, attrs);
        unitDef.createUnitsList();
        this.model.listOfUnitDefinitions.Add(unitDef);
        elementStack.Push(unitDef);        
    }

    public void UnitElement(Hashtable attrs)
    {
        UnitDefinition unitDef = (UnitDefinition) elementStack.Peek();
        String kind = null;
        int exponent = 1;
        int scale = 0;
        double multiplier = 1;

        if (attrs.Contains("kind")) 
            kind = (String) attrs["kind"];
        if (attrs.Contains("exponent")) 
            exponent = (int) (Int32.Parse((String)attrs["exponent"]));
        if (attrs.Contains("scale"))
            scale = (int)(Int32.Parse((String)attrs["scale"]));
        if (attrs.Contains("multiplier"))
            multiplier = (double)(Double.Parse((String)attrs["multiplier"]));

        Unit unit = new Unit(this.model, kind, exponent, scale, multiplier);
        unitDef.listOfUnits.Add(unit);
    }

    public void ListOfCompartmentTypesElement(Hashtable attrs)
    {
        this.model.listOfCompartmentTypes = new List<CompartmentType>();   
    }

    public void CompartmentTypeElement(Hashtable attrs)
    {
        CompartmentType compType = new CompartmentType(this.model, attrs);
        this.model.listOfCompartmentTypes.Add(compType);
    }

    public void ListOfSpeciesTypesElement(Hashtable attrs)
    {
        this.model.listOfSpeciesTypes = new List<SpeciesType>(); 
    }

    public void SpeciesTypeElement(Hashtable attrs)
    {
        SpeciesType specType = new SpeciesType(this.model, attrs);
        this.model.listOfSpeciesTypes.Add(specType);
    }

    public void ListOfCompartmentsElement(Hashtable attrs)
    {
        this.model.listOfCompartments = new List<Compartment>();
    }

    public void CompartmentElement(Hashtable attrs)
    {
        Compartment compartment = new Compartment(this.model, attrs);

        CompartmentType compartmentType = null;
        int spatialDimensions = 3;
        double size = 1;
        String units = null; // unit enum or user-defined unit
        Compartment outside = null;
        Boolean constant = true;

        if (attrs.Contains("constant")) 
            constant = Boolean.Parse((String)attrs["units"]);
        if (attrs.Contains("spatialdimensions")) 
            spatialDimensions = (int)(Int32.Parse((String)attrs["spatialdimensions"]));
        if (attrs.Contains("size"))
            size = (double)(Double.Parse((String)attrs["size"]));
        else if (attrs.Contains("volume")) // from SBML Level 1
            size = (double)(Double.Parse((String)attrs["volume"])); 
        if (attrs.Contains("units") && this.model.IsUnits((String)attrs["units"])) 
            units = (String)attrs["units"];
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
        
        if (attrs.Contains("compartmenttype"))
        {
            String cTypeId = (String)attrs["compartmenttype"];
            compartmentType = (CompartmentType)this.model.findObject(cTypeId);
        }
        if (attrs.Contains("outside"))
        {
            String cId = (String)attrs["outside"];
            outside = (Compartment)this.model.findObject(cId);
        }

        compartment.AddProperties(compartmentType, spatialDimensions, size,
            units, outside, constant);

        this.model.listOfCompartments.Add(compartment);
    }

    public void ListOfSpeciesElement(Hashtable attrs)
    {
        this.model.listOfSpecies = new List<Species>();
    }

    public void SpeciesElement(Hashtable attrs)
    {
        Species species = new Species(this.model, attrs);

        SpeciesType speciesType = null;
        Compartment compartment = null;
        double initialAmount = 0;
        double initialConcentration = 0;
        String substanceUnits = null;
        Boolean hasOnlySubstanceUnits = false;
        Boolean boundaryCondition = false;
        Boolean constant = false;

        if (attrs.Contains("initialamount")) 
            initialAmount = (double)(Double.Parse((String)attrs["initialamount"]));
        if (attrs.Contains("initialconcentration"))
            initialConcentration = (double)(Double.Parse((String)attrs["initialconcentration"]));
        if (attrs.Contains("substanceunits"))
            substanceUnits = (String)attrs["substanceunits"];
        if (attrs.Contains("hasonlysubstanceunits"))
            hasOnlySubstanceUnits = Boolean.Parse((String)attrs["hasonlysubstanceunits"]);
        if (attrs.Contains("boundarycondition"))
            boundaryCondition = Boolean.Parse((String)attrs["boundarycondition"]);
        if (attrs.Contains("constant"))
            constant = Boolean.Parse((String)attrs["constant"]);

        if (attrs.Contains("speciestype"))
        {
            String sTypeId = (String)attrs["speciesType"];
            speciesType = (SpeciesType)this.model.findObject(sTypeId);
        }

        if (attrs.Contains("compartment"))
        {
            String cId = (String)attrs["compartment"];
            compartment = (Compartment)this.model.findObject(cId);
        }

        if (attrs.Contains("substanceunits"))
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
        Parameter parameter = new Parameter(this.model, attrs);

        Double value = 1;
        String units = null;
        Boolean constant = true;

        if (attrs.Contains("value"))
            value = (double)(Double.Parse((String)attrs["value"]));
        if (attrs.Contains("units"))
            units = (String)attrs["units"];
        if (attrs.Contains("constant"))
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

        if (attrs.Contains("symbol"))
        {
            symbol = (String)attrs["symbol"];

            if (this.model.findObject(symbol)!=null)
                variable = (SBase)this.model.findObject(symbol);

            InitialAssignment initialAssignment = new InitialAssignment(this.model, variable);
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
        AlgebraicRule algebraicRule = new AlgebraicRule(this.model);
        this.model.listOfRules.Add(algebraicRule);

        elementStack.Push(algebraicRule);
    }

    public void AssignmentRuleElement(Hashtable attrs)
    {
        String variable;
        SBase parameter = null;

        if (attrs.Contains("variable"))
        {
            variable = (String)attrs["variable"];

            if (this.model.findObject(variable) != null)
                parameter = (SBase)this.model.findObject(variable);
        }
        // else throw exception on required attribute  

        AssignmentRule assignmentRule = new AssignmentRule(this.model, parameter);
        this.model.listOfRules.Add(assignmentRule);

        elementStack.Push(assignmentRule);
    }

    public void RateRuleElement(Hashtable attrs)
    {
        String variable;
        SBase parameter = null;

        if (attrs.Contains("variable"))
        {
            variable = (String)attrs["variable"];

            if (this.model.findObject(variable) != null)
                parameter = (SBase)this.model.findObject(variable);
        }
        // else throw exception on required attribute  

        RateRule rateRule = new RateRule(this.model, parameter);
        this.model.listOfRules.Add(rateRule);

        elementStack.Push(rateRule);
    }

    public void ListOfConstraintsElement(Hashtable attrs)
    {
        this.model.listOfConstraints = new List<Constraint>();
    }

    public void ConstraintElement(Hashtable attrs)
    {
        Constraint constraint = new Constraint(this.model);
        this.model.listOfConstraints.Add(constraint);

        elementStack.Push(constraint);
    }

    public void ListOfReactionsElement(Hashtable attrs)
    {
        this.model.listOfReactions = new List<Reaction>();
    }

    public void ReactionElement(Hashtable attrs)
    {
        Reaction reaction = new Reaction(this.model, attrs);
        Boolean fast = false;
        Boolean reversible = true;

        if (attrs.Contains("fast"))
            fast = Boolean.Parse((String)attrs["fast"]);
        if (attrs.Contains("reversible"))
            reversible = Boolean.Parse((String)attrs["reversible"]);
        
        reaction.AddProperties(fast, reversible);
        this.model.listOfReactions.Add(reaction);

        elementStack.Push(reaction);
    }

    public void ListOfReactantsElement(Hashtable attrs)
    {
        Reaction reaction = (Reaction)elementStack.Peek();
        reaction.listOfReactants = new List<SpeciesReference>();
        elementStack.Push(reaction.listOfReactants);
        elementStack.Push("listOfReactants");
    }

    public void ListOfProductsElement(Hashtable attrs)
    {
        Reaction reaction = (Reaction)elementStack.Peek();
        reaction.listOfProducts = new List<SpeciesReference>();
        elementStack.Push(reaction.listOfProducts);
        elementStack.Push("listOfProducts");
    }

    public void ListOfModifiersElement(Hashtable attrs)
    {
        Reaction reaction = (Reaction)elementStack.Peek();
        reaction.listOfModifiers = new List<ModifierSpeciesReference>();
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

                if (attrs.Contains("stoichiometry"))
                {
                    speciesRef = new SpeciesReference(this.model, species,
                        (double)(Double.Parse((String)attrs["stoichiometry"])));
                }
                else
                {
                    speciesRef = new SpeciesReference(this.model, species);
                }

                reactantsList.Add(speciesRef);
                elementStack.Push("listOfReactants");
            }

            else
                // list of products instead
            {
                SpeciesReference speciesRef;                
                List<SpeciesReference> productsList = (List<SpeciesReference>)elementStack.Peek();

                if (attrs.Contains("stoichiometry"))
                {
                    speciesRef = new SpeciesReference(this.model, species,
                        (double)(Double.Parse((String)attrs["stoichiometry"])));
                }
                else
                {
                    speciesRef = new SpeciesReference(this.model, species);
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
            
            ModifierSpeciesReference modifierRef;
            List<ModifierSpeciesReference> modifierList;

            Reaction reaction = (Reaction)elementStack.Peek();
            modifierList = reaction.listOfModifiers;
            modifierRef = new ModifierSpeciesReference(this.model, species);
            modifierList.Add(modifierRef);
            
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
        KineticLaw kLaw = new KineticLaw(this.model);
        reaction.kineticLaw = kLaw;

        elementStack.Push(kLaw);
    }

    public void ListOfEventsElement(Hashtable attrs)
    {
        this.model.listOfEvents = new List<Event>();
    }

    public void EventElement(Hashtable attrs)
    {
        Event eventNode = new Event(this.model, attrs);
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
        String variable;
        SBase parameter = null;

        if (attrs.Contains("variable"))
        {
            variable = (String)attrs["variable"];

            if (this.model.findObject(variable) != null)
                parameter = (SBase)this.model.findObject(variable);
        }

        EventAssignment eventAssignment = new EventAssignment(this.model, parameter);

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
        SpeciesReference lastRef = listOfRefs[listOfRefs.Count-1];

        elementStack.Push(lastRef);

    }

}
    
}
