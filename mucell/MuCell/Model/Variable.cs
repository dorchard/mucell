using System;

namespace MuCell.Model
{
	
	/// <summary>
	/// Struct for a Variable, associates a string name with a double value
	/// like a Tuple of String and Value
	/// </summary>
	public struct Variable
	{
		public string name;
		public double value;
	
		public Variable(string variableName, double variableValue)
		{
			this.name = variableName;
			this.value = variableValue;
		}
	}
}
