// TestNutrientField.cs created with MonoDevelop
// User: dao29 at 12:03 27/03/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using NUnit.Framework;
using MuCell.Model;

namespace MuCell.UnitTests.Model
{
    [TestFixture] public class TestNutrientField
    {

        [Test] public void TestOffsets()
		{
			//Bounds
			Boundary bounds = new Boundary();
			bounds.Shape = MuCell.Model.BoundaryShapes.Cuboid;
			bounds.Width = 10.0f;
			bounds.Height = 10.0f;
			bounds.Depth = 10.0f;			
			
			NutrientField nf = new NutrientField();
			nf.InitialDistribution = InitialNutrientDistribution.DenselyCentredSphere;
			nf.InitialQuantity = 1.0f;
			nf.InitialRadius = 5.0f;
			nf.InitialPosition = new Vector3(0, 0, 0);
			nf.Resolution = 0.5f;
			nf.DiffusionRate = 0f;
			nf.InitField(bounds);
			nf.PrintCubes();
		}
		
	}
}
