using System;
using NUnit;
using NUnit.Core;
using NUnit.Framework;

namespace UnitTests 
{
	class AssertDouble : Assert
	{
	
		/// <summary>
		/// Asserts an effective equality on doubles coming from tests and the simulator
		/// </summary>
		/// <param name="expected">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <param name="actualy">
		/// A <see cref="System.Double"/>
		/// </param>
		public static void AreEqual(double expected, double actual)
		{
			Assert.AreEqual(String.Format("{0:0.00000000000000}", expected).Substring(0, 13), String.Format("{0:0.00000000000000}", actual).ToString().Substring(0, 13));
		}
	
	}
}