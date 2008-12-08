/* Cathy Young
 * 
 * Classes representing a whole SBML model
 * Latest SBML spec: http://belnet.dl.sourceforge.net/sourceforge/sbml/sbml-level-2-version-3-rel-1.pdf
 */

using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MuCell.Model.SBML.Model
{
    public enum MathOperators
    {
        Plus, Minus, Times, Divide, Power, Log, Root, Abs,
        Exp, Ln, Floor, Ceiling, Factorial
    };

    public enum LogicOperators
    {
        And, Or, Xor, Not
    };

    public enum TrigOperators
    {
        Sin, Cos, Tan, Sec, Csc, Cot,
        Sinh, Cosh, Tanh, Sech, Csch, Coth,
        Arcsin, Arccos, Arctan, Arcsec, Arccsc, Arccot,
        Arcsinh, Arccosh, Arctanh, Arcsech, Arccsch, Arccoth
    };
    
    public enum MathConstants
    {
        True, False, Pi, Exponentiale, Notanumber, Infinity
    };

    public enum CSymbol
    {
        Time, Delay
    };

    public enum RelationOperators
    {
        Lt, Gt, Eq, Neq, Leq, Geq
    };


    public abstract class SBase
    {
        // notes and annotation are actually (respectively) XHTML (for humans)
        // and arbitrary XML (for machines)
        protected String notes;
        protected String annotation;
        protected int sboTerm;
        public String id;
        public String name;
        public Model model;

        public String getElementName()
        {
            String name = this.GetType().Name;
            return name.Substring(0,1).ToLower() + 
                name.Substring(1, name.Length - 1);
        }
    }

    public class Model: SBase
    {
        public List<FunctionDefinition> listOfFunctionDefinitions;
        public List<UnitDefinition> listOfUnitDefinitions;
        public List<CompartmentType> listOfCompartmentTypes;
        public List<SpeciesType> listOfSpeciesTypes;
        public List<Compartment> listOfCompartments;
        public List<Species> listOfSpecies;
        public List<Parameter> listOfParameters;
        public List<InitialAssignment> listOfInitialAssignments;
        public List<Rule> listOfRules;
        public List<Constraint> listOfConstraints;
        public List<Reaction> listOfReactions;
        public List<Event> listOfEvents;
        // should redo these as proper properties

        public Hashtable UnitTable;
        public Hashtable IdTable;

        public Model()
        {
            this.UnitTable = new Hashtable();
            this.initializeUnits();
            this.IdTable = new Hashtable();
        }

        public Model(String id)
        {
            this.UnitTable = new Hashtable();
            this.initializeUnits();
            this.IdTable = new Hashtable();

            this.id = id;
            this.name = id;
            this.AddId(id, this);
        }

        public Model(String id, String name)
        {
            this.UnitTable = new Hashtable();
            this.initializeUnits();
            this.IdTable = new Hashtable();

            this.id = id;
            this.AddId(id, this);
            this.name = name;
        }

        private void initializeUnits()
        {
            AddUnit("ampere");
            AddUnit("gram");
            AddUnit("katal");
            AddUnit("metre");
            AddUnit("second");
            AddUnit("watt");
            AddUnit("becquerel");
            AddUnit("gray");
            AddUnit("kelvin");
            AddUnit("mole");
            AddUnit("siemens");
            AddUnit("weber");
            AddUnit("candela");
            AddUnit("henry");
            AddUnit("kilogram");
            AddUnit("newton");
            AddUnit("sievert");
            AddUnit("coulomb");
            AddUnit("hertz");
            AddUnit("litre");
            AddUnit("ohm");
            AddUnit("steradian");
            AddUnit("dimensionless");
            AddUnit("item");
            AddUnit("lumen");
            AddUnit("pascal");
            AddUnit("tesla");
            AddUnit("farad");
            AddUnit("joule");
            AddUnit("lux");
            AddUnit("radian");
            AddUnit("volt");
            AddUnit("length");
            AddUnit("area");
            AddUnit("volume");
        }

        public void AddUnit(String unitName)
        {
            this.UnitTable.Add(unitName, null);
        }

        public void AddUnit(String unitName, Object reference)
        {
            this.UnitTable.Add(unitName, reference);
        }

        public Boolean IsUnits(String unitName)
        {
            if (unitName != null)
            {
                if (this.UnitTable.Contains(unitName))
                {
                    return true;
                }
            }
            return false;
        }

        public void AddId(String id, Object reference)
        {
            this.IdTable.Add(id, reference);
        }

        public Object findObject(String id)
        {
            if (this.IdTable.Contains(id))
            {
                return this.IdTable[id];
            }
            return null;
        }

    }

    public class FunctionDefinition : MathBase
    {
        public FunctionDefinition(Model m)
        {
            this.model = model;
        }

        public FunctionDefinition(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public FunctionDefinition(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public FunctionDefinition(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }
    }

    public class UnitDefinition : SBase
    {
        public List<Unit> listOfUnits;

        public UnitDefinition(Model m)
        {
            this.model = model;
        }

        public UnitDefinition(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddUnit(this.id, this);
            this.model.AddId(id, this);            
        }

        public UnitDefinition(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddUnit(this.id, this);
            this.model.AddId(id, this);
            this.name = name;
        }

        public UnitDefinition(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }

        public void createUnitsList()
        {
            this.listOfUnits = new List<Unit>();
        }
    }

    public class Unit : SBase
    {
        public String kind; 
        public int exponent;
        public int scale;
        public double multiplier;

        public Unit(Model m, String kind, int exponent, int scale, double multiplier)
        {
            this.model = model;
            if (this.model.IsUnits(kind) == true)
            {
                this.kind = kind;
            }
            // else throw exception - not a valid unit

            this.exponent = exponent;
            this.scale = scale;
            this.multiplier = multiplier;
        }

    }

    public class CompartmentType : SBase
    {
        public CompartmentType(Model m)
        {
            this.model = model;
        }

        public CompartmentType(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public CompartmentType(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public CompartmentType(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }
    }

    public class SpeciesType : SBase
    {
        public SpeciesType(Model m)
        {
            this.model = model;
        }

        public SpeciesType(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public SpeciesType(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public SpeciesType(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }
    }

    public class Compartment : SBase
    {
        public CompartmentType compartmentType;
        public int spatialDimensions;
        public double size;
        public String units; // unit enum or user-defined unit
        public Compartment outside;
        public Boolean constant;

        public Compartment()
        {
        }

        public Compartment(Model m)
        {
            this.model = model;
        }

        public Compartment(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public Compartment(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public Compartment(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }

        public void AddProperties(CompartmentType cType, int sD, double size,
            String units, Compartment outside, Boolean constant)
        {
            this.compartmentType = cType;
            this.spatialDimensions = sD;
            this.size = size;
            this.outside = outside;
            this.constant = constant;

            if (this.model.IsUnits(units) == true)
            {
                this.units = units;
            }
            // else throw exception for disallowed units;
        }

    }

    public class Species : SBase
    {
        public SpeciesType speciesType;
        public Compartment compartment;
        public double initialAmount;
        public double initialConcentration;
        public String substanceUnits; // unit enum
        public Boolean hasOnlySubstanceUnits;
        public Boolean boundaryCondition;
        public Boolean constant;

        public Species(Model m)
        {
            this.model = model;
        }

        public Species(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public Species(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public Species(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }

        public void AddProperties(SpeciesType st, Compartment c, double iA,
            double iC, String sU, Boolean hOSU, Boolean bC, Boolean con)
        {
            this.speciesType = st;
            this.compartment = c;
            this.initialAmount = iA;
            this.initialConcentration = iC;
            this.substanceUnits = sU;
            this.hasOnlySubstanceUnits = hOSU;
            this.boundaryCondition = bC;
            this.constant = con;
        }
    }

    public class Parameter : SBase
    {
        public double value;
        public String units; // unit enum or unit definition
        public Boolean constant;

        public Parameter(Model m)
        {
            this.model = model;
        }

        public Parameter(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public Parameter(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public Parameter(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }

        public void AddProperties(double value, String units, Boolean constant)
        {
            this.value = value;
            this.constant = constant;

            if (this.model.IsUnits(units) == true)
            {
                this.units = units;
            }
            // else throw exception - not valid units

        }
    }

    public class InitialAssignment : MathBase
    {
        public SBase variable; // can be Species, Compartment or Parameter

        public InitialAssignment(Model m)
        {
            this.model = model;
        }

        public InitialAssignment(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public InitialAssignment(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public InitialAssignment(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }

        public InitialAssignment(Model model, SBase variable)
        {
            this.model = model;
            this.variable = variable;
        }
    }

    public abstract class Rule : MathBase
    {
    }

    public class AlgebraicRule : Rule
    {
        public AlgebraicRule(Model m)
        {
            this.model = model;
        }

        public AlgebraicRule(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public AlgebraicRule(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public AlgebraicRule(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }

    }
    
    public class AssignmentRule : Rule
    {
        public SBase variable; // can be Species, Compartment or Parameter

        public AssignmentRule(Model model)
        {
            this.model = model;
        }

        public AssignmentRule(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public AssignmentRule(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public AssignmentRule(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }

        public AssignmentRule(Model model, SBase variable)
        {
            this.model = model;
            this.variable = variable; // check for null
        }
    }

    public class RateRule : Rule
    {
        public SBase variable; // can be Species, Compartment or Parameter

        public RateRule(Model model)
        {
            this.model = model;
        }

        public RateRule(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public RateRule(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public RateRule(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }

        public RateRule(Model model, SBase variable)
        {
            this.model = model;
            this.variable = variable; // check for null
        }
    }

    public class Constraint : MathBase
    {
        public String message; // actually XHTML, for human readers

        public Constraint(Model model)
        {
            this.model = model;
        }

        public Constraint(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public Constraint(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public Constraint(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }

        public void AddProperties(String message)
        {
            this.message = message;
        }

    }

    public class Reaction : SBase
    {
        public Boolean fast;
        public Boolean reversible;
        public List<SpeciesReference> listOfReactants;
        public List<SpeciesReference> listOfProducts;
        public List<ModifierSpeciesReference> listOfModifiers;
        public KineticLaw kineticLaw;

        public Reaction(Model model)
        {
            this.model = model;
        }

        public Reaction(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public Reaction(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public Reaction(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }

        public void AddProperties(Boolean fast, Boolean reversible)
        {
            this.fast = fast;
            this.reversible = reversible;
        }
    }

    public abstract class SimpleSpeciesReference : MathBase
    {
        public Species species;
    }

    public class SpeciesReference : SimpleSpeciesReference
    {
        public double stoichiometry;

        public SpeciesReference(Model model)
        {
            this.model = model;
        }

        public SpeciesReference(Model model, Species species)
        {
            this.model = model;
            this.species = species;
            this.stoichiometry = 1;
        }

        public SpeciesReference(Model model, Species species, double stoichiometry)
        {
            this.model = model;
            this.species = species;
            this.stoichiometry = stoichiometry;
        }

        public new void AddProperties(MathTree stoichiometryMath)
        {
            this.math = stoichiometryMath;
        }
    }

    public class ModifierSpeciesReference : SimpleSpeciesReference
    {
        public ModifierSpeciesReference(Model model)
        {
            this.model = model;
        }

        public ModifierSpeciesReference(Model model, Species species)
        {
            this.model = model;
            this.species = species;
        }
    }

    public abstract class MathBase : SBase
    {
        public MathTree math;

        public void AddProperties(MathTree math)
        {
            this.math = math;
        }
    }

    public class MathTree : SBase
    {
        public Stack nodeStack;
        public OperatorNode root;

        public MathTree()
        {
            this.nodeStack = new Stack();
        }

        public new String getElementName()
        {
            return this.name;
        }

        public MathNode getCurrentNode()
        {
            try
            {
                return (MathNode)nodeStack.Peek();
            }
            catch 
            {
                // empty stack, create empty root node
                this.root = new OperatorNode();
                this.nodeStack.Push(this.root);
                return this.root;
            }
        }
    }

    public class MathNode
    {
        public Object data; // can be operator, constant, ID ref, csymbol, number
        public MathNode parent;
        public Model model;

        public MathNode()
        {
          
        }

        public MathNode(MathNode parent)
        {
            this.parent = parent;
        }

    }

    public class LeafNode : MathNode
    {
        String type;

        public LeafNode(Model m, MathNode parent)
        {
            this.model = model;
            this.parent = parent;
        }

        public LeafNode(Model m, MathNode parent, Enum constant)
        {
            this.model = m;
            this.parent = parent;
            this.data = constant;
        }

        public LeafNode(Model m, MathNode parent, String type)
        {
            this.model = m;
            this.parent = parent;

            if (type == "integer" || type == "real" ||
                type == "rational" || type == "e-notation")
            {
                this.type = type;
            }
            else if (type == null)
            {
                this.type = "real";
            }
            else // not a number, it's a string id
            {
                this.type = "string";
            }
        }

        public void AddData(String text)
        {
            // parse according to real, integer, rational, e-notation
            // or a String id;
            switch (this.type)
            {
                case "integer":
                    this.data = Int32.Parse(text);
                    break;
                case "real":
                case "rational":
                    this.data = Double.Parse(text);
                    break;
                case "e-notation":
                    // gak
                    break;
                case "string":
                    // search for id in rest of document
                    // can be:
                    //    Species (amount/concentration of species)
                    //    Compartment (size of compartment)
                    //    Parameter (value of parameter)
                    //    FunctionDefinition (call to that function)
                    //    Reaction (rate of that reaction as defined by the KineticLaw)
                    Object reference = this.model.findObject(text);
                    if (reference != null)
                    {
                        this.data = reference;
                    }
                    break;
            }
        }


    }

    public class OperatorNode : MathNode
    {
        public List<MathNode> subtree;

        public OperatorNode()
        {
            this.subtree = new List<MathNode>();
        }

        public OperatorNode(MathNode parent)
        {
            this.parent = parent;
            this.subtree = new List<MathNode>();
        }

        public OperatorNode(MathNode parent, Enum op)
        {
            this.parent = parent;
            this.subtree = new List<MathNode>();
            this.data = op;
        }

        public void AddData(Enum op)
        {
            this.data = op;
        }

        public void AddNode(MathNode node)
        {
            this.subtree.Add(node);
        }
    }

    public class KineticLaw : MathBase
    {
        public List<Parameter> listOfParameters;

        public KineticLaw(Model model)
        {
            this.model = model;
        }

        public KineticLaw(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public KineticLaw(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public KineticLaw(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }
    }

    public class Event : MathBase
    {
        public Trigger trigger;
        public Delay delay;
        public List<EventAssignment> listOfEventAssignments;
        // each eventAssignment must use a unique variable (no overlap) per event
        
        public Event(Model model)
        {
            this.model = model;
        }

        public Event(Model m, String id)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);            
        }

        public Event(Model m, String id, String name)
        {
            this.model = model;
            this.id = id;
            this.model.AddId(id, this);
            this.name = name;
        }

        public Event(Model model, Hashtable attrs)
        {
            this.model = model;
            if (attrs.Contains("id"))
            {
                this.id = (String)attrs["id"];
                this.model.AddId(this.id, this);
                this.model.AddUnit(this.id, this);
            }
            if (attrs.Contains("name"))
                this.name = name;
        }
    }

    public class Trigger : MathBase
    {
        public Event parentEvent;

        public Trigger(Event parent)
        {
            this.parentEvent = parent;
        }
    }

    public class Delay : MathBase
    {
        public Event parentEvent;

        public Delay(Event parent)
        {
            this.parentEvent = parent;
        }
    }

    public class EventAssignment : MathBase
    {
        public SBase variable; // can be Compartment, Species or Parameter

        public EventAssignment(Model model)
        {
            this.model = model;
        }

        public EventAssignment(Model model, SBase variable)
        {
            this.model = model;
            this.variable = variable;
            // check variable exists in model.IdTable
            // and that it's not set to constant
        }

    }

}
