using System;

namespace MuCell.Model.Solver
{

	public class Euler : SolverBase
	{
		private int n;
		private Solver.ModelFunction modelFunction;
		private double[] y;
		private double time;
	
		public Euler(int n, double[] initialValues, Solver.ModelFunction modelFun)
		{
			this.n = n;
			this.modelFunction = modelFun;
			this.y = new double[n];
			
			// Set up initial values
			for(int i=0;i<n;i++){
				this.y[i] = initialValues[i];
			}
			
			this.time = 0;
		}

        /// <summary>
        /// Set the vector value at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="amount"></param>
        public override void SetVectorIndex(int index, double amount)
        {
            this.y[index] = amount;
        }
		
		/// <summary>
		/// Get a value from the model with index.
		/// </summary>
		/// <param name="index">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Double"/>
		/// </returns>
		public override double GetValue(int index)
		{
			if (index<this.n)
			{
				return y[index];
			}
			else
			{
				return -1.0d;
			}
		}
				
		/// <summary>
		/// Computes one time step of the model
		/// </summary>
		/// <param name="time">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <param name="timeStep">
		/// A <see cref="System.Double"/>
		/// </param>
		public override double OneStep(double time, double timeStep)
		{
			if (this.modelFunction!=null)
			{
				double[] deltaVector = this.modelFunction(time, this.y);
				
				// Add the delta vector * timeStep
				for(int i=0;i<this.n;i++)
				{
					this.y[i] = this.y[i] + deltaVector[i]*timeStep;
				}
				
				this.time = time+timeStep;
			}
			return this.time;
		}
		
		/// <summary>
		/// Restart a model at a point
		/// </summary>
		/// <param name="timeStart">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <param name="newY">
		/// A <see cref="System.Double"/>
		/// </param>
		public override void Restart(double timeStart, double[] newY)
		{
			// Set up initial values
			for(int i=0;i<n;i++){
				this.y[i] = newY[i];
			}
			
			// Reset time
			this.time = timeStart;
		}
	
		public override void Release()
		{
			// Nothing to release
		}
	}
}