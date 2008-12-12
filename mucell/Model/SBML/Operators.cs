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

namespace MuCell.Model.SBML
{

    	/// <summary>
	/// Enum of the binary Maths operators available in SBML rate equations
	/// </summary>
    public enum BinaryMathOperators
    {
        Plus, Minus, Times, Divide, Power
    };
    
    /// <summary>
    /// Enum of the unary Maths operations available in SBML rate equations
    /// </summary>
    public enum UnaryMathOperators
    {
    		Log, Root, Abs, Exp, Ln, Floor, Ceiling, Factorial, Sqr, Sqrt,
    		Sin, Cos, Tan, Sec, Csc, Cot,
        Sinh, Cosh, Tanh, Sech, Csch, Coth,
        Arcsin, Arccos, Arctan, Arcsec, Arccsc, Arccot,
        Arcsinh, Arccosh, Arctanh, Arcsech, Arccsch, Arccoth
    };
    
    	/// <summary>
	/// Enum of the binary Logic operators available in SBML rate equations
	/// </summary>
    public enum BinaryLogicOperators
    {
        And, Or, Xor
    };
    
    public enum UnaryLogicOperators
    {
    		Not
    };

	// <todo> Phase this out, its redundant to have two enums that are both unary maths functions</todo>
	/// <summary>
	/// Enum of the Trigonometric function available in SBML rate equations
	/// </summary>
    public enum TrigOperators
    {
        
    };
    
    /// <summary>
    /// Enum of the mathematical constants available in SBML rate equations
    /// </summary>
    public enum MathConstants
    {
        True, False, Pi, Exponential, Notanumber, Infinity
    };

	/// <summary>
	/// Symbols available
	/// </summary>
    public enum CSymbol
    {
        Time, Delay
    };

	/// <summary>
	/// Enum of relational operators available in SBML rate equations
	/// </summary>
    public enum RelationOperators
    {
        Lt, Gt, Eq, Neq, Leq, Geq
    };

}
