// TestVector.cs created with MonoDevelop
// User: riftor at 02:07 24/01/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using NUnit.Framework;
using MuCell.Model;

namespace UnitTests.Model
{
	
	
	[TestFixture()]
	public class TestVector
	{
		
		[Test()]
		public void TestEquality()
		{
		
			Vector3 lower = new Vector3(0, 0, 0);
			Vector3 upper = new Vector3(12, 10, 9);
			
			// Test under boundary
			Vector3 under = new Vector3(-1, 0, 0);
			Assert.That(!under.within(lower, upper));
			
			// Test over boundary
			Vector3 over = new Vector3(13, 5, 5);
			Assert.That(!over.within(lower, upper));
			
			// Test in boundary
			Vector3 in1 = new Vector3(5, 5, 5);
			Assert.That(in1.within(lower, upper));
			
			// Tests edges in boundary
			Vector3 edge1 = new Vector3(0, 0, 0);
			Assert.That(edge1.within(lower, upper));
			Vector3 edge2 = new Vector3(12, 0, 9);
			Assert.That(edge2.within(lower, upper));

		}
	}
}
