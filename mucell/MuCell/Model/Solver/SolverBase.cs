using System;

namespace MuCell.Model.Solver
{

	/// <summary>
	/// Enumerates the available solvers
	/// </summary>
	public enum SolverMethods
	{
		Euler, RungeKutta, CVode_Adams_Moulton, CVode_BDF_Newton
	};

	/// <summary>
	/// Base Model function takes a time double, a list of variables, y, in the model and returns 
	/// the list of doubles for dy/dt. 
	/// </summary>
	/// <param name="time">
	/// A <see cref="System.Double"/>
	/// </param>
	/// <param name="y">
	/// A <see cref="System.Double"/>
	/// </param>
	/// <returns>
	/// A <see cref="System.Double"/>
	/// </returns>
	public delegate double[] ModelFunction(double time, double[] y);

	public abstract class SolverBase
	{
		
		public abstract double GetValue(int index); 
				
		public abstract double OneStep(double time, double timeStep);
		
		public abstract void Restart(double timeStart, double[] newY);
		
		public abstract void Release();

        public abstract void SetVectorIndex(int index, double amount);
	
	}
}
