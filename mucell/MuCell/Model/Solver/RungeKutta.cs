using System;

namespace MuCell.Model.Solver
{

	public class RungeKutta : SolverBase
	{
		private int n;
		private Solver.ModelFunction modelFunction;
		private double[] y;
		private double time;
	
		public RungeKutta(int n, double[] initialValues, Solver.ModelFunction modelFun)
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
				// 1
				double[] deltaVector1 = this.modelFunction(time, this.y);
				
				double[] middle1 = new double[n];
				for(int i=0;i<this.n;i++)
				{
					middle1[i] = this.y[i]+deltaVector1[i]*(timeStep/2);
				}
				
				// 2
				double[] deltaVector2 = this.modelFunction(time, middle1);
				
				double[] middle2 = new double[n];
				for(int i=0;i<this.n;i++)
				{
					middle2[i] = this.y[i]+deltaVector2[i]*(timeStep/2);
				}
				
				// 3
				double[] deltaVector3 = this.modelFunction(time, middle2);
				
				double[] middle3 = new double[n];
				for(int i=0;i<this.n;i++)
				{
					middle3[i] = this.y[i]+deltaVector3[i]*timeStep;
				}
				
				// 4
				double[] deltaVector4 = this.modelFunction(time, middle3);
				
				// Calculat the slope vector
				double[] slope = new double[n];
				for(int i=0;i<this.n;i++)
				{
					slope[i] = (timeStep/6)*(deltaVector1[i]+deltaVector2[i]*2+deltaVector3[i]*2+deltaVector4[i]);
				}
				
				// Compute the result
				for(int i=0;i<this.n;i++)
				{
					this.y[i] = this.y[i] + slope[i];
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