using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MuCell.Model.SBML
{
    public class FormulaParser
    {
        public String formula;
        public Model model = null;
        private List<Model> models;
        public MathTree formulaTree;
                
        // Parent SBMLReader
        // Used for getting the mapping from strings to enums for operators
        private Reader.SBMLReader reader;

        private Stack operatorStack = new Stack();
        private Stack operandStack = new Stack();
        private Hashtable opPrecedence = new Hashtable();
        
        private Experiment experiment;
        private Simulation simulation;

        private List<Species> formulaSpecies;
        private List<Parameter> formulaParameters;
        private List<UnknownEntity> formulaUnknownEntities;

        public FormulaParser(Reader.SBMLReader reader, String formula)
        {
            this.formula = formula;
            this.reader = reader;
            
            // Init the collection list
            this.formulaSpecies = new List<Species>();
            this.formulaParameters = new List<Parameter>();
            this.formulaUnknownEntities = new List<UnknownEntity>();

            // Init the operators
            this.init();
            
            // Make the formula tree
            this.formulaTree = this.makeFormulaTree(formula, false);
            
            // no Model, so can't look up ID references
            // (mainly for testing purposes)
        }

        public FormulaParser(Reader.SBMLReader reader, String formula, Model model)
        {
            this.formula = formula;
            this.reader = reader;
            this.model = model;

            model.InterrogateModelForMissingIDs();

            // Init the collection list
            this.formulaSpecies = new List<Species>();
            this.formulaParameters = new List<Parameter>();
            this.formulaUnknownEntities = new List<UnknownEntity>();

            // Init the operators
            this.init();
            this.formulaTree = this.makeFormulaTree(formula, false);
        }

		/// <summary>
		/// Constructor that includes an experiment and simulation reference which is used for formulas with aggregate functions
		/// that use references to cell definitions, groups and species
		/// </summary>
		/// <param name="reader">
		/// A <see cref="Reader.SBMLReader"/>
		/// </param>
		/// <param name="formula">
		/// A <see cref="String"/>
		/// </param>
		/// <param name="model">
		/// A <see cref="Model"/>
		/// </param>
		/// <param name="experiment">
		/// A <see cref="Experiment"/>
		/// </param>
		/// <param name="simulation">
		/// A <see cref="Simulation"/>
		/// </param>
		public FormulaParser(Reader.SBMLReader reader, String formula, Model model, Experiment experiment, Simulation simulation)
        {
            this.formula = formula;
            this.reader = reader;

            model.InterrogateModelForMissingIDs();

            //create model list
            this.models = new List<Model>();
            models.Add(model);

            // Init the collection list
            this.formulaSpecies = new List<Species>();
            this.formulaParameters = new List<Parameter>();
            this.formulaUnknownEntities = new List<UnknownEntity>();

            // Init the operators
            this.init();
            this.experiment = experiment;
            this.simulation = simulation;
            this.formulaTree = this.makeFormulaTree(formula, true);
        }
		
		/// <summary>
		/// Constructor that includes an experiment and simulation reference which is used for formulas with aggregate functions
		/// that use references to cell definitions, groups and species and also a list of all models used
		/// </summary>
		/// <param name="reader">
		/// A <see cref="Reader.SBMLReader"/>
		/// </param>
		/// <param name="formula">
		/// A <see cref="String"/>
		/// </param>
		/// <param name="models">
		/// A <see cref="List`1"/>
		/// </param>
		/// <param name="experiment">
		/// A <see cref="Experiment"/>
		/// </param>
		/// <param name="simulation">
		/// A <see cref="Simulation"/>
		/// </param>
		public FormulaParser(Reader.SBMLReader reader, String formula, List<Model> models, Experiment experiment, Simulation simulation)
        {
            this.formula = formula;
            this.reader = reader;
            this.models = models;

            foreach (Model model in models)
            {
                model.InterrogateModelForMissingIDs();
            }

            // Init the operators
            this.init();
            this.experiment = experiment;
            this.simulation = simulation;
            this.formulaTree = this.makeFormulaTree(formula, true);
        }

        private void init()
        {
            opPrecedence.Add("^", 4);
            opPrecedence.Add("/", 3);
            opPrecedence.Add("*", 3);
            opPrecedence.Add("+", 2);
            opPrecedence.Add("-", 2);
            
            opPrecedence.Add(BinaryMathOperators.Power, 4);
            opPrecedence.Add(BinaryMathOperators.Divide, 3);
           	opPrecedence.Add(BinaryMathOperators.Times, 3);
            opPrecedence.Add(BinaryMathOperators.Plus, 2);
            opPrecedence.Add(BinaryMathOperators.Minus, 2);
        }
        
        private MathTree makeFormulaTree(String formula, bool aggregateVariables)
        {


            // Approximate regexp of tokens
            // Fix - 06/03/2008 - Dorchard
            // First clause from: [a-zA-Z_]+
            // to: [a-zA-Z][a-zA-Z0-9_]*
            // Allows variables with numbers but which must start with at least one letter
            Regex formulaRegexp;
            if (aggregateVariables)
			{
				formulaRegexp = new Regex("([a-zA-Z_][a-zA-Z0-9_.]*)|([0-9]+([.][0-9]+)?)|([0-9]+)|[+-/*^| \t]|[()]");
			}
			else
			{
            	formulaRegexp = new Regex("([a-zA-Z_][a-zA-Z0-9_]*)|([0-9]+([.][0-9]+)?)|([0-9]+)|[+-/*^| \t]|[()]");
            }

            foreach(Match match in formulaRegexp.Matches(formula))
            {           
            		// Get the matched token
            		String token = match.ToString();
            		//System.Console.WriteLine(token);

            		// Look at idtable
            		//foreach (Object o in model.IdTable.Keys)
            		//{
            		//	System.Console.WriteLine(o.ToString()+" - "+model.IdTable[o].ToString());
            		//}
                if (token == " ")
                {
                    // just whitespace, ignore
                    continue;
                }
                // If we are parsing a formula that takes aggregate variables
                if (aggregateVariables && !token.Contains("."))
				{
					// Create aggreaget node first
                		AggregateReferenceNode operand = new AggregateReferenceNode(this.experiment, this.simulation);
                			
                		if (!TryParseAggregateSpecies(operand, token))
                		{
            				if (!TryParseAggregateGroup(operand, token))
            				{
            					if(TryParseAggregateCellDefinition(operand, token))
            					{
            						// Just a cell def
            						operandStack.Push(operand);
            						continue;
            					}
            				}
            				else
            				{
            					// Just a group
            					operandStack.Push(operand);
            					continue;
            				}
            			}
            			else
            			{
            				// Just species
            				operandStack.Push(operand);
            				continue;
            			}
            		}
                
                // if token is a number
                // or a name that will evaluate to a number (in ID table)
                // push to operand stack
                double d = 0.0d;
                if (double.TryParse(token, out d)) {
                    NumberLeafNode operand = new NumberLeafNode();
                    operand.AddData(token);
                    operandStack.Push(operand);
                }
                // some kind of split reference
                	else if (aggregateVariables && token.Contains(".")) 
                	{
                		// Create aggreaget node first
					AggregateReferenceNode operand = new AggregateReferenceNode(this.experiment, this.simulation);
                	
                		string[] tokens = token.Split('.');
                		if(tokens.Length==1)
                		{
                			System.Console.WriteLine("Parse error in formula - "+formula+" at token - "+token); 
                		}
                		else if (tokens.Length==2)
                		{
                			// Try a group
                			if (!TryParseAggregateGroup(operand, tokens[0]))
                			{
                				// Try a celldef
                				if (!TryParseAggregateCellDefinition(operand, tokens[0]))
                				{
                					// If this failed then error
                					System.Console.WriteLine("Parse error in formula - CellDefinition/Group not found: "+tokens[0]);
                				}
                				else
                				{
                					// Starts with a celldef, could be a group of celldef
                					if (!TryParseAggregateGroup(operand, tokens[1]))
                					{
                						// must be a species
									if (TryParseAggregateSpecies(operand, tokens[1]))
			                			{
			                				// CellDef.Species
			                				operandStack.Push(operand);
			                			}
			                			else
			                			{
			                				// Fail
			                				System.Console.WriteLine("Parse error in formula - Species not found: "+tokens[1]);
			                			}
                					}
                					else
                					{
                						// Was a group, ok
                						// CellDef.Group
                						operandStack.Push(operand);
                					}
                				}
                			}
                			else
                			{
                				// Starts with a group
                				// Must therefore be a species
                				if (TryParseAggregateSpecies(operand, tokens[1]))
                				{
                					// Group.Species
                					operandStack.Push(operand);
                				}
                				else
                				{
                					// Fail
                					System.Console.WriteLine("Parse error in formula - Species not found: "+tokens[1]);
                				}
                			}
                		}
                		else if (tokens.Length==3)
                		{
                			// should be CellDef.Group.Species
                			if (TryParseAggregateCellDefinition(operand, tokens[0]))
                			{
                				if (TryParseAggregateGroup(operand, tokens[1]))
                				{
                					if (TryParseAggregateSpecies(operand, tokens[2]))
                					{
                						operandStack.Push(operand);		
                					}
                					else
                					{
                						System.Console.WriteLine("Parse error in formula - Species not found: "+tokens[2]);
                					}
                				}
                				else
                				{
                					System.Console.WriteLine("Parse error in formula - Group not found: "+tokens[1]);
                				}
                			}
                			else
                			{
                				System.Console.WriteLine("Parse error in formula - CellDef not found: "+tokens[0]);
                			}
                		}
                		else
                		{
                			// Too many periods
                			System.Console.WriteLine("Parse error in formula - "+formula+" at token - "+token);
                		}
                	}
                else if ((model != null) && (model.idExists(token)))
                {
                    		ReferenceLeafNode operand = new ReferenceLeafNode();                    
                    		operand.AddData(token, model);
                    		operandStack.Push(operand);

                            if (operand.data is Parameter)
                            {
                                this.formulaParameters.Add((Parameter)operand.data);
                            }
                            if (operand.data is Species)
                            {
                                this.formulaSpecies.Add((Species)operand.data);
                            }
                }
                else if (reader.unaryMathOperatorsLookup.ContainsKey(token))
                {
                    // function call, push to stack
                    InnerNode functionNode = new InnerNode(reader.unaryMathOperatorsLookup[token]);
                    operatorStack.Push(functionNode);

                    // Cannot be user-defined FunctionDefinition, because
                    // string formulae and FunctionDefinitions are in 
                    // mutually exclusive SBML Levels.
                }
                 else if (reader.binaryMathOperatorsLookup.ContainsKey(token) && !opPrecedence.ContainsKey(token))
                {
                    // function call, push to stack
                    InnerNode functionNode = new InnerNode(reader.binaryMathOperatorsLookup[token]);
                    operatorStack.Push(functionNode);
                }
                else if (opPrecedence.ContainsKey(token))
                {
                    // While there is an operator on the top of the stack
                    while (operatorStack.Count>0 &&	operatorStack.Peek() is InnerNode)
                    {
                        InnerNode op2 = (InnerNode)operatorStack.Peek();
                        int op2Precedence = (int)opPrecedence[op2.data];
                        int tokenPrecedence = (int)opPrecedence[token];

                        if (op2Precedence > tokenPrecedence)
                        {
                            // pop, build new subtree, push to operand stack
                            operatorStack.Pop();
                            buildNewSubtree(op2);
                        }
                        else
                        {
                            break;
                        }
                    }
                    // create token as new OperatorNode
                    if (reader.binaryMathOperatorsLookup.ContainsKey(token))
                    {
                    		BinaryMathOperators op = reader.binaryMathOperatorsLookup[token];
                    		InnerNode opToken = new InnerNode(op);
                    		// push token to operator stack
                    		operatorStack.Push(opToken);
                    	}
                    	else if (reader.unaryMathOperatorsLookup.ContainsKey(token))
                    	{
                    	 	UnaryMathOperators op = reader.unaryMathOperatorsLookup[token];
                    		InnerNode opToken = new InnerNode(op);
                    		// push token to operator stack
                    		operatorStack.Push(opToken);
                    	}
                    	else
                    	{
                    		// <todo> raise parse exception </todo>
                    	}
                }
                else if (token == "(")
                {
                    operatorStack.Push(token);
                }
                else if (token == ")")
                {
                    while (operatorStack.Count>0 && !((operatorStack.Peek() is string) && ((string)operatorStack.Peek())=="("))
                    {
                        // pop operator, build new subtree, push to operand stack
                        InnerNode op = (InnerNode)operatorStack.Pop();
                        buildNewSubtree(op);
                    }
                    // remove the "(" from the stack
                    Object o = operatorStack.Pop();
                    
                    // check if top of stack is a function call
                    // if so, pop, build new subtree, push to operand stack
                    if (operatorStack.Count>0 && (reader.unaryMathOperatorsLookup.ContainsValue((UnaryMathOperators)(((InnerNode)(operatorStack.Peek())).data)) ||
                    		reader.binaryMathOperatorsLookup.ContainsValue((BinaryMathOperators)(((InnerNode)(operatorStack.Peek())).data))))
                    {
                        InnerNode op = (InnerNode)operatorStack.Pop();
                        buildNewSubtree(op);
                    }
                }
                else
                {
                    Regex checkAlpha = new Regex("[a-zA-Z_][a-zA-Z0-9_.]*");
                        Match matchAlpha = checkAlpha.Match(token);
                        // Approximate test of unknown token
                        if (!matchAlpha.Success)
                        {
                            // Likely some unknown or badly formatted symbols
                            // No recognition - parse error
                            // <todo>Raise exception?</todo>
                            System.Console.WriteLine("Parse error in formula - " + formula + " at token - " + token);
                        }
                        else
                        {

                            // Potentially an unknown ID try and add as an unknown identifier and try to continue parsing
                            ReferenceLeafNode operand = new ReferenceLeafNode();
                            operand.data = new UnknownEntity();
                            operand.data.ID = token;
                            operandStack.Push(operand);

                            this.formulaUnknownEntities.Add((UnknownEntity)operand.data);
                            System.Console.WriteLine("Unknown entity in formula - " + token);
                        }
                		
                }
            }

            // We have now reached the end of the tokens.
            // Pop the rest of the operators.
            while (operatorStack.Count>0)
            {
                InnerNode op = (InnerNode)operatorStack.Pop();
                buildNewSubtree(op);
            }
            // No more operators.

			MathTree math;
			// if we just have a leaf node on the stack convert it to a mathtree
			if (operandStack.Peek() is LeafNode)
			{
				math = new MathTree();
				math.root = (LeafNode)operandStack.Pop();
			}
			else
			{
            		math = (MathTree)operandStack.Pop();
            	}
            	
            //	debug();
            	
            return math;
        }
		
        /// <summary>
        /// Try and parse the token as a cell definition reference, updating the AggregateReferenceNode if the token is a CellDefinition
        /// </summary>
        /// <param name="node">
        /// A <see cref="AggregateReferenceNode"/>
        /// </param>
        /// <param name="token">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>      
        public bool TryParseAggregateCellDefinition(AggregateReferenceNode node, string token)
        {
        	MuCell.Model.CellDefinition[] celldefs = this.experiment.getCellDefinitions();
			if (this.experiment.ContainsCellDefinition(token))
            	{
            		node.CellDefinition = experiment.GetCellDefinition(token);
            		return true;
            	}
            	else
            	{
            		return false;
            	}
        }
        
        /// <summary>
        /// Try and parse the token as a group reference, updating the AggregateReferenceNode if the token is a group
        /// </summary>
        /// <param name="node">
        /// A <see cref="AggregateReferenceNode"/>
        /// </param>
        /// <param name="token">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool TryParseAggregateGroup(AggregateReferenceNode node, string token)
        {
        		if (token.StartsWith("group"))
        		{
        			try
        			{
        				int parsed = Int32.Parse(token.Remove(0, 5));
        				if ((parsed.ToString() == token.Remove(0, 5)) &&
        					(this.simulation.Parameters!=null) &&
        					(this.simulation.Parameters.InitialState!=null) &&
        					(this.simulation.Parameters.InitialState.SimulationEnvironment!=null) &&
            				(this.simulation.Parameters.InitialState.SimulationEnvironment.ContainsGroup(Int32.Parse(token.Remove(0, 5)))))
            			{
						node.Group = new Group(parsed.ToString());
						return true;
					}
					else
					{
						return false;
					}
        			}
        			catch (Exception e)
        			{
        				return false;
        			}
        		}
			else
			{
				return false;
			}
        }

        /// <summary>
        /// Try and parse the token as a species reference, updating the AggregateReferenceNode if the token is a species
        /// </summary>
        /// <param name="node">
        /// A <see cref="AggregateReferenceNode"/>
        /// </param>
        /// <param name="token">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>      
        public bool TryParseAggregateSpecies(AggregateReferenceNode node, string token)
        {
        	SBase reference = null;
        		
        	// See if we have a species in all the models
        	foreach (Model model in this.models)
			{
            	reference = (SBase)model.findObject(token);
            	if(reference!=null)
            	{
            		break;
            	}
            }
            
            if (reference != null)
            {
            	node.Species = (Species)reference;
            	return true;
            }
            else
            {
            	if (token.Equals("twiddleTime"))
            	{
            		node.FlagellaTracker="twiddleTime";
            		return true;
            	}
            	else if (token.Equals("runTime"))
            	{
            		node.FlagellaTracker="runTime";
            		return true;
            	}
            	else
            	{
            		return false;
            	}
            }
        }

        /// <summary>
        /// Returns a list of the species from a parsed formula
        /// </summary>
        /// <returns></returns>
        public List<Species> SpeciesFromFormula()
        {
            return this.formulaSpecies;
        }

        /// <summary>
        /// Returns a list of the parameters used in a parsed formula
        /// </summary>
        /// <returns></returns>
        public List<Parameter> ParametersFromFormula()
        {
            return this.formulaParameters;
        }

        /// <summary>
        /// Returns a list of the unknown identifies in a parse formula
        /// </summary>
        /// <returns></returns>
        public List<UnknownEntity> UnknownEntitiesFromFormula()
        {
            return this.formulaUnknownEntities;
        }

		private void debug() {
			// DEBUG
            // Print op stack
            System.Console.Write("operandStack - ");
            foreach (Object o in operandStack)
            {
                System.Console.Write(o.ToString() + " ");
            }
            System.Console.Write("\n");
            System.Console.Write("operatorStack - ");
            foreach (Object o in operatorStack)
            {
                System.Console.Write(o.ToString() + " ");
            }
            System.Console.Write("\n");
		}


        // Takes an operator, pops appropriate number of operands from 
        // operand stack, builds them into a new subtree and pushes the 
        // subtree back to the operand stack.
        private void buildNewSubtree(InnerNode op)
        {
           
            // take op2, build new subtree and push to operand stack
            Object topOperand = operandStack.Pop();
			
            // can be LeafNode or MathTree (subtree)
            if (topOperand is LeafNode)
            {
                LeafNode leafNode1 = (LeafNode)topOperand;

                if (op.numArgs() == 2) // binary operator
                {
                    Object nextOperand = operandStack.Pop();
                    if (nextOperand is LeafNode)
                    {
                        LeafNode leafNode2 = (LeafNode)nextOperand;
                        MathTree newSubtree = this.joinTrees(op, leafNode1, leafNode2);
                        operandStack.Push(newSubtree);
                    }
                    else
                    {
                        MathTree subtree2 = (MathTree)nextOperand;
                        MathTree newSubtree = this.joinTrees(op, leafNode1, subtree2);
                        operandStack.Push(newSubtree);
                    }
                }
                else // unary operator
                {
                    MathTree newSubtree = this.joinTrees(op, leafNode1);
                    operandStack.Push(newSubtree);
                }
            }
            else
            {
                MathTree subtree1 = (MathTree)topOperand;

                if (op.numArgs() == 2)  // binary operator
                {
                    Object nextOperand = operandStack.Pop();
                    if (nextOperand is LeafNode)
                    {
                        LeafNode leafNode2 = (LeafNode)nextOperand;
                        MathTree newSubtree = this.joinTrees(op, subtree1, leafNode2);
                        operandStack.Push(newSubtree);
                    }
                    else
                    {
                        MathTree subtree2 = (MathTree)nextOperand;
                        MathTree newSubtree = this.joinTrees(op, subtree1, subtree2);
                        operandStack.Push(newSubtree);
                    }

                }
                else // unary operator
                {
                    MathTree newSubtree = this.joinTrees(op, subtree1);
                    operandStack.Push(newSubtree);
                }
            }

            
        }

        private MathTree joinTrees(InnerNode op, MathTree subtree1, MathTree subtree2)
        {
            MathNode root1 = subtree1.root;
            MathNode root2 = subtree2.root;

            // create new tree with op as the root, and root1, root2
            // as the two child nodes
            MathTree math = new MathTree(op, root2, root1);

            return math;
        }

        private MathTree joinTrees(InnerNode op, MathTree subtree1, LeafNode leaf1)
        {
            MathNode root1 = subtree1.root;
            MathTree math = new MathTree(op, leaf1, root1);
            return math;
        }

        private MathTree joinTrees(InnerNode op, LeafNode leaf1, MathTree subtree1)
        {
            MathNode root1 = subtree1.root;
            MathTree math = new MathTree(op, root1, leaf1);
            return math;
        }

        private MathTree joinTrees(InnerNode op, LeafNode leaf1, LeafNode leaf2)
        {
            MathTree math = new MathTree(op, leaf2, leaf1);
            return math;
        }

        private MathTree joinTrees(InnerNode op, MathTree subtree1)
        {
            MathNode root1 = subtree1.root;
            MathTree math = new MathTree(op, root1);
            return math;
        }

        private MathTree joinTrees(InnerNode op, LeafNode leaf1)
        {
            MathTree math = new MathTree(op, leaf1);
            return math;
        }

        public MathTree getFormulaTree()
        {
            return this.formulaTree;
        }


    }
}
