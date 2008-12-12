/// <summary>
/// Modified from the Systems Biology Workbench
/// C# Foreign Function Interface to the CVode library DLL
/// </summary>

using System;
using System.Runtime.InteropServices;

namespace MuCell.Model.Solver
{
	public class CVode : SolverBase
	{

		private int n = 0;
		private IntPtr y;
		private IntPtr absoluteTolerance;
		private IntPtr cvodeMem;
		private int errCode;
		private double relativeTolerance;

		// Begin the FFI DDL imports
		
		public delegate int ModelFunction (double time, IntPtr y, IntPtr ydot, IntPtr fdata);
		//void (*TCallBackFcn)(int n, double Time, double *y, double *ydot, void *f_data);

		public ModelFunction modelFunction;

		[DllImport ("sundials_nvecserial.dll", EntryPoint="N_VNew_Serial", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		static extern IntPtr NewCvode_Vector(int n);

		[DllImport ("sundials_nvecserial.dll", EntryPoint="N_VDestroy_Serial", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		static extern void FreeCvode_Vector (IntPtr vect);

		[DllImport ("sundials_cvode.dll", EntryPoint="CVodeFree", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		static extern void FreeCvode_Mem (IntPtr p);  // void *p

		[DllImport ("sundials_nvecserial.dll", EntryPoint="N_VSet", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		public static extern void Cvode_SetVector (IntPtr v, int Index, double Value);
		
		[DllImport ("sundials_nvecserial.dll", EntryPoint="N_VGet", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		public static extern double Cvode_GetVector (IntPtr v, int Index);

		[DllImport ("sundials_cvode.dll", EntryPoint="CVodeCreate", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		static extern IntPtr CVodeCreate(int lmm, int iter);

		[DllImport ("sundials_cvode.dll", EntryPoint="CVodeMalloc", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		static extern int AllocateCvodeMem(IntPtr cvode_mum, ModelFunction fcn, double t0, IntPtr y, int itol, ref double reltol, IntPtr abstol);
		
		[DllImport ("sundials_cvode.dll", EntryPoint="CVDense", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		static extern int CvDense(IntPtr cvode_mem, int N);  // int = size of systems

		[DllImport ("sundials_cvode.dll", EntryPoint="CVReInit", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		static extern int  CVReInit(IntPtr cvode_mem, double t0, IntPtr y0, double reltol, IntPtr abstol);
		
		[DllImport ("sundials_cvode.dll", EntryPoint="CVode", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		static extern int  CVode_Run(IntPtr cvode_mem, double tout, IntPtr y, ref double t, int itask);  // t = double *
		
		[DllImport ("sundials_cvode.dll", EntryPoint="CVodeSetStopTime", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			CharSet=CharSet.Unicode, SetLastError=true)]
		static extern int CVodeSetStopTime(IntPtr cvode_mem, double tstop);

		[DllImport ("sundials_cvode.dll", EntryPoint="CVReInit", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		static extern int  SetMaxNumSteps(IntPtr cvode_mem, int mxsteps);

		[DllImport ("sundials_cvode.dll", EntryPoint="SetMaxOrder", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		static extern int SetMaxOrder(IntPtr cvode_mem, int mxorder);

		[DllImport ("sundials_cvode.dll", EntryPoint="CVSetFData", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		static extern int CVSetFData (IntPtr cvode_mem, IntPtr f_data);
 
		[DllImport ("sundials_cvode.dll", EntryPoint="SetMaxErrTestFails", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		static extern int SetMaxErrTestFails(IntPtr cvode_mem, int maxnef);

		[DllImport ("sundials_cvode.dll", EntryPoint="SetMaxConvFails", CallingConvention=CallingConvention.Cdecl, ExactSpelling=false,
			 CharSet=CharSet.Unicode, SetLastError=true)]
		static extern int SetMaxConvFails(IntPtr cvode_mem, int maxncf);


		public override double OneStep (double timeStart, double hstep) 
		{
			double timeEnd = 0.0;
			double tout = timeStart + hstep;
			CVodeSetStopTime(this.cvodeMem, tout);
			int errCode = CVode_Run(this.cvodeMem, tout, y, ref timeEnd, 3);  // t = double *
			if (timeEnd!=(timeStart+hstep))
			{
				System.Console.WriteLine("timeEnd = "+timeEnd+" should be "+(timeStart+hstep));
			}
			return (timeEnd);
		}

		/// <summary>
		/// Restart the process with the old starting points defined by newY
		/// </summary>
		/// <param name="timeStart">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <param name="newY">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Int32"/>
		/// </returns>
		public override void Restart(double timeStart, double[] newY) {

			for (int i=0; i<this.n; i++)
			{
                Cvode_SetVector (this.y, i, newY[i]);
			}

			CVReInit(this.cvodeMem, timeStart, this.y, this.relativeTolerance, this.absoluteTolerance);
		}

		/// <summary>
		/// Get a value for a variable from the Cvode.
		/// </summary>
		/// <param name="index">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Double"/>
		/// </returns>
		public override double GetValue(int index) 
		{
			return Cvode_GetVector(this.y, index);
		}

		/// <summary>
		/// Default constructor, defaults to BDF Newton method.
		/// </summary>
		/// <param name="n">
		/// A <see cref="System.Int32"/>
		/// </param>
		public CVode(int n, ModelFunction modelFun)
		{
			this.n = n;
			this.modelFunction = modelFun;

			this.cvodeMem = CVodeCreate(2, 2);

			this.y = NewCvode_Vector(this.n);
			this.absoluteTolerance = NewCvode_Vector(this.n);
			
			for (int i=0; i<this.n; i++)
			{
				Cvode_SetVector (this.absoluteTolerance, i, 1.0e-6);
				Cvode_SetVector (this.y, i, 0.0);
			}
			
			this.relativeTolerance = 1.0e-8;

			this.errCode = AllocateCvodeMem(this.cvodeMem, this.modelFunction, 0.0, this.y, 1, ref this.relativeTolerance, this.absoluteTolerance);
			this.errCode = CvDense (this.cvodeMem, n);  // int = size of systems
		}

		/// <summary>
		/// Another constructor that allows greater specification of parameteres
		/// </summary>
		/// <param name="n">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="modelFun">
		/// A <see cref="ModelFcn"/>
		/// </param>
		/// <param name="abstol">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <param name="reltol">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <param name="type">
		/// A <see cref="System.Int32"/>
		/// </param>
		public CVode(int n, double[] initialValues, ModelFunction modelFun, double[] abstol, double reltol, int type)
		{
			this.n = n;
			this.modelFunction = modelFun;
			
			if (type==0)
			{
				this.cvodeMem = CVodeCreate(1, 1);
			}
			else
			{
				this.cvodeMem = CVodeCreate(2, 2);
			}

			this.y = NewCvode_Vector(this.n);
			this.absoluteTolerance = NewCvode_Vector(this.n);
			
			for (int i=0; i<this.n; i++)
			{
				System.Console.WriteLine("abstol = "+abstol[i]);
				Cvode_SetVector (this.absoluteTolerance, i, abstol[i]);
				Cvode_SetVector (this.y, i, initialValues[i]);
			}
			
			this.relativeTolerance = reltol;
			System.Console.WriteLine("relative tolerance = "+reltol);

			this.errCode = AllocateCvodeMem(this.cvodeMem, this.modelFunction, 0.0, this.y, 1, ref this.relativeTolerance, this.absoluteTolerance);
			this.errCode = CvDense (this.cvodeMem, n);  // int = size of systems
		}

		public override void Release() 
		{
			FreeCvode_Mem (this.cvodeMem);  // void *p
			FreeCvode_Vector(this.y);
		}

        /// <summary>
        /// Set the vector value at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="amount"></param>
        public override void SetVectorIndex(int index, double amount)
        {
            Cvode_SetVector(this.y, index, amount);
        }

	}
}
