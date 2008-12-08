// TestReactions.cs created with MonoDevelop
// User: riftor at 14:29 26/03/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using NUnit.Framework;

namespace UnitTests.Model
{
	
	
	[TestFixture] public class TestReactions
	{
		
		[Test] public void TestHopfParseAndReactionsL1V1()
		{
			// Hopf model
			MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.xml");
			RunHopfParseAndReactions(s);
		}
		
		[Test] public void TestHopfParseAndReactionsL1V2()
		{
			// Hopf model
			MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.level2.xml");
			RunHopfParseAndReactions(s);
		}
		
		
		public void RunHopfParseAndReactions(MuCell.Model.SBML.Reader.SBMLReader s)
		{

			Assert.AreEqual(5, s.model.listOfReactions.Count);
			
			MuCell.Model.SBML.Reaction[] reactions = s.model.listOfReactions.ToArray();
			
			// Reaction 1
			Assert.AreEqual(2, reactions[0].Reactants.Count);
			Assert.AreEqual(1, reactions[0].Reactants.ToArray()[0].Stoichiometry);
			Assert.AreEqual(1, reactions[0].Reactants.ToArray()[1].Stoichiometry);
			Assert.AreEqual("X", reactions[0].Reactants.ToArray()[0].SpeciesID);
			Assert.AreEqual("A", reactions[0].Reactants.ToArray()[1].SpeciesID);
			
			Assert.AreEqual(1, reactions[0].Products.Count);
			Assert.AreEqual(2, reactions[0].Products.ToArray()[0].Stoichiometry);
			Assert.AreEqual("X", reactions[0].Products.ToArray()[0].SpeciesID);
			
			// Reaction 2
			Assert.AreEqual(2, reactions[1].Reactants.Count);
			Assert.AreEqual(1, reactions[1].Reactants.ToArray()[0].Stoichiometry);
			Assert.AreEqual(1, reactions[1].Reactants.ToArray()[1].Stoichiometry);
			Assert.AreEqual("X", reactions[1].Reactants.ToArray()[0].SpeciesID);
			Assert.AreEqual("Y", reactions[1].Reactants.ToArray()[1].SpeciesID);
			
			Assert.AreEqual(2, reactions[1].Products.Count);
			Assert.AreEqual(1, reactions[1].Products.ToArray()[0].Stoichiometry);
			Assert.AreEqual(1, reactions[1].Products.ToArray()[1].Stoichiometry);
			Assert.AreEqual("A", reactions[1].Products.ToArray()[0].SpeciesID);
			Assert.AreEqual("Y", reactions[1].Products.ToArray()[1].SpeciesID);
			
			// Reaction 3
			Assert.AreEqual(1, reactions[2].Reactants.Count);
			Assert.AreEqual(1, reactions[2].Reactants.ToArray()[0].Stoichiometry);
			Assert.AreEqual("X", reactions[2].Reactants.ToArray()[0].SpeciesID);
						
			Assert.AreEqual(1, reactions[2].Products.Count);
			Assert.AreEqual(1, reactions[2].Products.ToArray()[0].Stoichiometry);
			Assert.AreEqual("Z", reactions[2].Products.ToArray()[0].SpeciesID);
			
			// Reaction 4
			Assert.AreEqual(1, reactions[3].Reactants.Count);
			Assert.AreEqual(1, reactions[3].Reactants.ToArray()[0].Stoichiometry);
			Assert.AreEqual("Z", reactions[3].Reactants.ToArray()[0].SpeciesID);
						
			Assert.AreEqual(1, reactions[3].Products.Count);
			Assert.AreEqual(1, reactions[3].Products.ToArray()[0].Stoichiometry);
			Assert.AreEqual("Y", reactions[3].Products.ToArray()[0].SpeciesID);
			
			// Reaction 5
			Assert.AreEqual(1, reactions[4].Reactants.Count);
			Assert.AreEqual(1, reactions[4].Reactants.ToArray()[0].Stoichiometry);
			Assert.AreEqual("Y", reactions[4].Reactants.ToArray()[0].SpeciesID);
			
			Assert.AreEqual(1, reactions[4].Products.Count);
			Assert.AreEqual(1, reactions[4].Products.ToArray()[0].Stoichiometry);
			Assert.AreEqual("_void_", reactions[4].Products.ToArray()[0].SpeciesID);
			
		}
	}
}
