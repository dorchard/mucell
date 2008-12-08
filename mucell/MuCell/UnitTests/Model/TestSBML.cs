// created on 23/01/2008 at 09:25
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace UnitTests.Model
{
    [TestFixture]
    public class TestSBML
    {

        [Test]
        public void loadAndParseTestv1L1()
        {
            // http://www.genome.ad.jp/dbget-bin/get_pathway?org_name=ecc&mapno=00020
            // Citrate Cycle for E. Coli CFT073
            MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.xml");

            // Assert name is equal
            Assert.AreEqual("Smallest_chemical_reaction_system_with_Hopf_bifurcation", s.model.ID);

            MuCell.Model.SBML.Reaction[] rs = s.model.listOfReactions.ToArray();
            Assert.AreEqual(5, rs.Length);

            System.Console.Write(rs[1].KineticLaw.math);

            MuCell.Model.SBML.Reaction[] reactions = s.model.listOfReactions.ToArray();

            // Check there are 3 kinetic laws
            Assert.AreEqual(5, reactions.Length);

            // Assert the correctness of serializes a kinetic law into a string
            Assert.AreEqual("k_1*X*A", reactions[0].KineticLaw.math.ToString());
            Assert.AreEqual("k_2*X*Y", reactions[1].KineticLaw.math.ToString());
            Assert.AreEqual("k_3*X", reactions[2].KineticLaw.math.ToString());
            Assert.AreEqual("k_4*Z", reactions[3].KineticLaw.math.ToString());
            Assert.AreEqual("k_5*Y", reactions[4].KineticLaw.math.ToString());
        }

        [Test]
        public void loadAndParseTestSBMLv1L2()
        {
            // http://www.genome.ad.jp/dbget-bin/get_pathway?org_name=ecc&mapno=00020
            // Citrate Cycle for E. Coli CFT073
            MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.level2.xml");

            // Assert name is equal
            Assert.AreEqual("Smallest_chemical_reaction_system_with_Hopf_bifurcation", s.model.ID);

            MuCell.Model.SBML.Reaction[] rs = s.model.listOfReactions.ToArray();
            Assert.AreEqual(5, rs.Length);

            MuCell.Model.SBML.Reaction[] reactions = s.model.listOfReactions.ToArray();

            // Check there are 3 kinetic laws
            Assert.AreEqual(5, reactions.Length);

            // Assert the correctness of serializes a kinetic law into a string
            Assert.AreEqual("k_1*X*A", reactions[0].KineticLaw.math.ToString());
            Assert.AreEqual("k_2*X*Y", reactions[1].KineticLaw.math.ToString());
            Assert.AreEqual("k_3*X", reactions[2].KineticLaw.math.ToString());
            Assert.AreEqual("k_4*Z", reactions[3].KineticLaw.math.ToString());
            Assert.AreEqual("k_5*Y", reactions[4].KineticLaw.math.ToString());
        }

        [Test]
        public void testDataDuplicate()
        {
            MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
            MuCell.Model.SBML.Species species1 = new MuCell.Model.SBML.Species();
            MuCell.Model.SBML.Species species2 = new MuCell.Model.SBML.Species();

            model.listOfSpecies = new List<MuCell.Model.SBML.Species>();
            model.listOfSpecies.Add(species1);
            model.listOfSpecies.Add(species2);

            // Set some values for species1
            species1.ID = "species1";
            species1.InitialAmount = 4.0d;
            species1.InitialConcentration = 7.0d;
            species1.xPosition = 2.0f;

            // Set some values for species2 that has the same id and same values
            species2.ID = "species1";
            species2.InitialAmount = 4.0d;
            species2.InitialConcentration = 7.0d;
            species2.xPosition = 2.0f;

            // Add to the model
            model.AddId(species1.ID, species1);
            model.AddId(species2.ID, species2);

            // Check that we have two species
            Assert.AreEqual(2, model.listOfSpecies.Count);

            // Should remove species2
            model.processDuplicates();

            // Check there is now only 1 species in the model
            Assert.AreEqual(1, model.listOfSpecies.Count);

            // Check that it is species1
            Assert.AreEqual(species1, model.listOfSpecies.ToArray()[0]);

        }

        [Test]
        public void testNonDataDuplicate()
        {

            MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
            MuCell.Model.SBML.Species species1 = new MuCell.Model.SBML.Species();
            MuCell.Model.SBML.Species species2 = new MuCell.Model.SBML.Species();
            MuCell.Model.SBML.Species species3 = new MuCell.Model.SBML.Species();

            model.listOfSpecies = new List<MuCell.Model.SBML.Species>();
            model.listOfSpecies.Add(species1);
            model.listOfSpecies.Add(species2);
            model.listOfSpecies.Add(species3);

            // Set some values for species1
            species1.ID = "species1";
            species1.InitialAmount = 4.0d;
            species1.InitialConcentration = 7.0d;
            species1.xPosition = 2.0f;

            // Set some values for species2 that has the same id and same values
            species2.ID = "species1";
            species2.InitialAmount = 4.0d;
            species2.InitialConcentration = 7.0d;
            species2.xPosition = 2.0f;

            // Set some values for species3 that has the same id but DIFFERENT values
            species3.ID = "species1";
            // Difference
            species3.InitialAmount = 5.0d;
            species3.InitialConcentration = 7.0d;
            species3.xPosition = 2.0f;

            // Add to the model
            model.AddId(species1.ID, species1);
            model.AddId(species2.ID, species2);
            model.AddId(species3.ID, species3);

            try
            {
                model.processDuplicates();
            }
            catch (MuCell.Model.SBML.DuplicateSBMLObjectIdException e)
            {
                // Assert that an exception has been thrown
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void testInterrogateModelForIDs()
        {
            MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();

            model.listOfReactions = new List<MuCell.Model.SBML.Reaction>();
            model.listOfSpecies = new List<MuCell.Model.SBML.Species>();

            MuCell.Model.SBML.Species species1 = new MuCell.Model.SBML.Species();
            MuCell.Model.SBML.Species species2 = new MuCell.Model.SBML.Species();
            MuCell.Model.SBML.Species species3 = new MuCell.Model.SBML.Species();

            species1.ID = "species1";
            species2.ID = "species2";
            species3.ID = "species 3";

            model.AddId("species1", species1);

            MuCell.Model.SBML.Reaction r1 = new MuCell.Model.SBML.Reaction();
            MuCell.Model.SBML.SpeciesReference sp1r = new MuCell.Model.SBML.SpeciesReference();
            sp1r.species = species1;
            MuCell.Model.SBML.SpeciesReference sp2r = new MuCell.Model.SBML.SpeciesReference();
            sp2r.species = species2;
            MuCell.Model.SBML.SpeciesReference sp3r = new MuCell.Model.SBML.SpeciesReference();
            sp3r.species = species3;

            r1.Reactants.Add(sp1r);
            r1.Reactants.Add(sp2r);
            r1.Products.Add(sp3r);

            MuCell.Model.SBML.KineticLaw kl = new MuCell.Model.SBML.KineticLaw(model);
            r1.KineticLaw = kl;

            MuCell.Model.SBML.Parameter k1 = new MuCell.Model.SBML.Parameter();
            k1.ID = "k1";
            r1.Parameters.Add(k1);
            r1.Formula = "species1*species_3*k1";

            model.listOfSpecies.Add(species1);
            model.listOfReactions.Add(r1);

            // Assert the unknown entities
            Assert.AreEqual(2, r1.KineticLaw.UnknownEntitiesFromFormula().Count);
            Assert.AreEqual("species_3", r1.KineticLaw.UnknownEntitiesFromFormula().ToArray()[0].ID);
            Assert.AreEqual("k1", r1.KineticLaw.UnknownEntitiesFromFormula().ToArray()[1].ID);

            // Assert missing items in model
            Assert.IsTrue(model.idExists("species1"));
            Assert.IsFalse(model.idExists("species2"));
            Assert.IsFalse(model.idExists("species 3"));
            Assert.IsFalse(model.idExists("k1"));

            model.InterrogateModelForMissingIDs();

            // redo formula
            r1.Formula = "species1*species_3*k1";

            // assert no unknown entities
            Assert.AreEqual(0, r1.KineticLaw.UnknownEntitiesFromFormula().Count);

            // Assert everything is in the model
            Assert.IsTrue(model.idExists("species1"));
            Assert.IsTrue(model.idExists("species2"));
            Assert.IsTrue(model.idExists("species 3"));
            Assert.IsTrue(model.idExists("k1"));
        }

        [Test]
        public void testRenaming()
        {
            MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
            MuCell.Model.SBML.Species species1 = new MuCell.Model.SBML.Species();
            MuCell.Model.SBML.Species species2 = new MuCell.Model.SBML.Species();

            model.listOfSpecies = new List<MuCell.Model.SBML.Species>();
            model.listOfSpecies.Add(species1);
            model.listOfSpecies.Add(species2);

            // Set some values for species1
            species1.ID = "species1";
            species1.InitialAmount = 4.0d;
            species1.InitialConcentration = 7.0d;
            species1.xPosition = 2.0f;

            // Set some values for species2 that has the same id and same values
            species2.ID = "species2";
            species2.InitialAmount = 4.0d;
            species2.InitialConcentration = 7.0d;
            species2.xPosition = 2.0f;

            // Add to the model
            model.AddId(species1.ID, species1);
            model.AddId(species2.ID, species2);

            // Rename
            species1.ID = "new_species1";

            // Check the id in the species object
            Assert.AreEqual("new_species1", species1.ID);
            Assert.AreEqual("species1", species1.getOldID());

            model.updateID(species1);

            // Check the ids in the model
            Assert.IsTrue(model.idExists("new_species1"));
            Assert.IsTrue(model.idExists("species2"));
            Assert.IsFalse(model.idExists("species1"));
        }

        [Test]
        public void treeStructureTest()
        {
            MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
            MuCell.Model.SBML.Reaction reaction1 = new MuCell.Model.SBML.Reaction();
            model.listOfReactions = new List<MuCell.Model.SBML.Reaction>();
            model.listOfReactions.Add(reaction1);
            MuCell.Model.SBML.KineticLaw kw1 = new MuCell.Model.SBML.KineticLaw(model);

            kw1.math = new MuCell.Model.SBML.MathTree();
            kw1.math.root = new MuCell.Model.SBML.InnerNode(MuCell.Model.SBML.BinaryMathOperators.Times);

            reaction1.KineticLaw = kw1;
            model.AddId("reaction1", kw1);

            MuCell.Model.SBML.Reaction[] reactions = model.listOfReactions.ToArray();
            Assert.AreEqual(1, reactions.Length);

            Assert.AreEqual(MuCell.Model.SBML.BinaryMathOperators.Times, ((MuCell.Model.SBML.InnerNode)(reactions[0].KineticLaw.math.root)).data);
        }

        /// <summary>
        /// Test serialization of an empty model
        /// </summary>
        [Test]
        public void SerializeEmptyModel()
        {
            // Load SBML
            MuCell.Model.SBML.Reader.SBMLReader sbml = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/data/test1.xml");
            // Assert that it serializes
            Assert.That(sbml.model.saveToXml("../../UnitTests/data/test1.Serialized.xml"));

            // Load the serialized code
            MuCell.Model.SBML.Reader.SBMLReader sbmlSerialized = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/data/test1.Serialized.xml");
            // Assert its SBML object hierarchy is equal to the original
            sbml.model.SBMLEquals(sbmlSerialized.model);
        }

        /// <summary>
        /// Tests serialization of an empty model with notes + annotations
        /// </summary>
        [Test]
        public void SerializeEmptyModelAndNotes()
        {
            // Load SBML
            MuCell.Model.SBML.Reader.SBMLReader sbml = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/data/test2.xml");
            // Assert that it serializes
            Assert.That(sbml.model.saveToXml("../../UnitTests/data/test2.Serialized.xml"));

            // Load the serialized code
            MuCell.Model.SBML.Reader.SBMLReader sbmlSerialized = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/data/test2.Serialized.xml");
            // Assert its SBML object hierarchy is equal to the original
            sbml.model.SBMLEquals(sbmlSerialized.model);
        }

        /// <summary>
        /// Tests serialization of an model with compartments
        /// </summary>
        [Test]
        public void SerializeModelWithCompartments()
        {
            // Load SBML
            MuCell.Model.SBML.Reader.SBMLReader sbml = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/data/test3.xml");
            // Assert that it serializes
            Assert.That(sbml.model.saveToXml("../../UnitTests/data/test3.Serialized.xml"));

            // Load the serialized code
            MuCell.Model.SBML.Reader.SBMLReader sbmlSerialized = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/data/test3.Serialized.xml");
            // Assert its SBML object hierarchy is equal to the original
            sbml.model.SBMLEquals(sbmlSerialized.model);
        }

        /// <summary>
        /// Tests serialization of an model with compartments and species
        /// </summary>
        [Test]
        public void SerializeModelWithCompartmentsAndSpecies()
        {
            // Load SBML
            MuCell.Model.SBML.Reader.SBMLReader sbml = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/data/test4.xml");
            // Assert that it serializes
            Assert.That(sbml.model.saveToXml("../../UnitTests/data/test4.Serialized.xml"));

            // Load the serialized code
            MuCell.Model.SBML.Reader.SBMLReader sbmlSerialized = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/data/test4.Serialized.xml");
            // Assert its SBML object hierarchy is equal to the original
            sbml.model.SBMLEquals(sbmlSerialized.model);
        }

        /// <summary>
        /// Tests serialization of an model with compartments and species and a reaction with no law
        /// </summary>
        [Test]
        public void SerializeModelWithCompartmentsAndSpeciesAndEmptyReaction()
        {
            // Load SBML
            MuCell.Model.SBML.Reader.SBMLReader sbml = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/data/test5.xml");
            // Assert that it serializes
            Assert.That(sbml.model.saveToXml("../../UnitTests/data/test5.Serialized.xml"));

            // Load the serialized code
            MuCell.Model.SBML.Reader.SBMLReader sbmlSerialized = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/data/test5.Serialized.xml");
            // Assert its SBML object hierarchy is equal to the original
            sbml.model.SBMLEquals(sbmlSerialized.model);
        }

        /// <summary>
        /// Tests serialization of an model with compartments and species and a reaction with no law
        /// </summary>
        [Test]
        public void SerializeModelWithCompartmentsAndSpeciesAndReaction()
        {
            // Load SBML
            MuCell.Model.SBML.Reader.SBMLReader sbml = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/data/test6.xml");
            // Assert that it serializes
            Assert.That(sbml.model.saveToXml("../../UnitTests/data/test6.Serialized.xml"));

            // Load the serialized code
            MuCell.Model.SBML.Reader.SBMLReader sbmlSerialized = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/data/test6.Serialized.xml");
            // Assert its SBML object hierarchy is equal to the original
            sbml.model.SBMLEquals(sbmlSerialized.model);
        }

        /// <summary>
        /// Tests serialization of the smallest hopf model - version 1 level 1
        /// </summary>
        [Test]
        public void SerializeHopfModelv1l1()
        {
            MuCell.Model.SBML.Reader.SBMLReader sbml = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.xml");
            Assert.That(sbml.model.saveToXml("../../UnitTests/smallest.Hopf.serialized.xml"));

            // Load the serialized code
            MuCell.Model.SBML.Reader.SBMLReader sbmlSerialized = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.serialized.xml");
            // Assert its SBML object hierarchy is equal to the original
            sbml.model.SBMLEquals(sbmlSerialized.model);
        }

        /// <summary>
        /// Tests serialization of the smallest hopf model
        /// </summary>
        [Test]
        public void SerializeHopfModelv1l2()
        {
            MuCell.Model.SBML.Reader.SBMLReader sbml = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.level2.xml");
            Assert.That(sbml.model.saveToXml("../../UnitTests/smallest.Hopf.level2.serialized.xml"));

            // Load the serialized code
            MuCell.Model.SBML.Reader.SBMLReader sbmlSerialized = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.level2.serialized.xml");
            // Assert its SBML object hierarchy is equal to the original
            sbml.model.SBMLEquals(sbmlSerialized.model);
        }

        [Test]
        public void serializeXandYPositions()
        {
            MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
            MuCell.Model.SBML.Species species1 = new MuCell.Model.SBML.Species();
            MuCell.Model.SBML.Species species2 = new MuCell.Model.SBML.Species();

            model.listOfSpecies = new List<MuCell.Model.SBML.Species>();
            model.listOfSpecies.Add(species1);
            model.listOfSpecies.Add(species2);

            // Set some values for species1
            species1.ID = "species1";
            species1.InitialAmount = 4.0;
            species1.InitialConcentration = 7.0;
            species1.xPosition = 2.0f;
            species1.yPosition = 3.0f;

            // Set some values for species2
            species2.ID = "species2";
            species2.InitialAmount = 1.0;
            species2.InitialConcentration = 3.0;
            species2.xPosition = 5.0f;
            species2.yPosition = 6.0f;

            // Add to the model
            model.AddId(species1.ID, species1);
            model.AddId(species2.ID, species2);

            model.saveToXml("../../UnitTests/xy.serialized.xml");
        }

        [Test]
        public void readXandYpositions()
        {
            MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/xy.serialized.xml");
            MuCell.Model.SBML.Model model = s.model;

            Assert.AreEqual(2, s.model.listOfSpecies[0].xPosition);
            Assert.AreEqual(3, s.model.listOfSpecies[0].yPosition);
            Assert.AreEqual(5, s.model.listOfSpecies[1].xPosition);
            Assert.AreEqual(6, s.model.listOfSpecies[1].yPosition);
        }

        [Test]
        public void SerializeEcc00020()
        {
            MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/ecc00020.xml");

            MuCell.Model.SBML.Model model = s.model;

            model.saveToXml("../../UnitTests/ecc00020.serialized.xml");

            // Load the serialized code
            MuCell.Model.SBML.Reader.SBMLReader sbmlSerialized = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/ecc00020.serialized.xml");
            // Assert its SBML object hierarchy is equal to the original
            s.model.SBMLEquals(sbmlSerialized.model);

        }

        [Test]
        public void ExtraCellularComponents()
        {
            MuCell.Model.SBML.Reader.SBMLReader s = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.serialized.xml");
            MuCell.Model.SBML.Model model = s.model;

            // Fill in dummy data adding extracellular component data to model!
            //
            model.listOfComponents = new List<MuCell.Model.SBML.ExtracellularComponents.ComponentBase>();
            MuCell.Model.SBML.ExtracellularComponents.ComponentBase cb = new MuCell.Model.SBML.ExtracellularComponents.ComponentBase();
            model.listOfComponents.Add(cb);

            model.saveToXml("../../UnitTests/extracellularcomponents.serialized.xml");
        }


    }
}
