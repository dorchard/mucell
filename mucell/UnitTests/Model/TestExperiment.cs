using System;
using System.IO;
using System.Xml.Serialization;
using NUnit.Framework;
using MuCell.Model;

namespace UnitTests.Model
{


    [TestFixture()]
    public class TestExperiment
    {

        [Test()]
        public void TestSerializationEquality()
        {
            String filename = "../../UnitTests/expt1.serialized.xml";

            MuCell.Model.Experiment experiment = new MuCell.Model.Experiment("experiment1");

            Simulation simulation1 = new Simulation("simulation1");
            Results results = new Results();

            MuCell.Model.SBML.Model model = new MuCell.Model.SBML.Model();
            CellDefinition cellDef1 = new CellDefinition("cellDef1");
            cellDef1.addSBMLModel(model);

            for (int i = 0; i < 5; i++)
            {
                StateSnapshot snapshot = new StateSnapshot();
                for (int j = 0; j < 10; j++)
                {
                    CellInstance cell = new CellInstance(cellDef1);
                    cell.GroupID = j;
                    cell.CellInstanceSpatialContext.Position = new Vector3(1*j, 1+i, 2+j);
                    cell.CellInstanceSpatialContext.Velocity = new Vector3(4*i, 5+j, 3+i);
                    cell.CellInstanceSpatialContext.Radius = 0.123f;
                    cell.CellInstanceSpatialContext.Orientation = new Vector3(2+i, 3*j, 4*j);

                    snapshot.Cells.Add(cell);
                }
                MuCell.Model.Environment environment = new MuCell.Model.Environment(new Vector3(10 * i, 14 * i, 15 * i));

                SerializableDictionary<int, MuCell.Model.SBML.Group> dict = new SerializableDictionary<int, MuCell.Model.SBML.Group>();

                // create new groups x3
                MuCell.Model.SBML.Group group1 = new MuCell.Model.SBML.Group(1);
                group1.Col = System.Drawing.Color.Beige;
                MuCell.Model.SBML.Group group2 = new MuCell.Model.SBML.Group(2);
                group2.Col = System.Drawing.Color.Brown;
                MuCell.Model.SBML.Group group3 = new MuCell.Model.SBML.Group(3);
                group3.Col = System.Drawing.Color.Green;
                dict.Add(1, group1); dict.Add(2, group2); dict.Add(3, group3);
                environment.Groups = dict;

                //SerializableDictionary<int, MuCell.Model.NutrientField> nutDict = new SerializableDictionary<int, MuCell.Model.NutrientField>();

                //// create new nutrients x2
                //MuCell.Model.NutrientField nut1 = new MuCell.Model.NutrientField(1);
                //MuCell.Model.NutrientField nut2 = new MuCell.Model.NutrientField(2);
                //nut2.Col = System.Drawing.Color.Fuchsia;
                //nutDict.Add(1,nut1); nutDict.Add(2,nut2);
                //environment.Nutrients = nutDict;

                snapshot.SimulationEnvironment = environment;

                results.StateSnapshots.Add(snapshot);
            }
            results.CurrentState = results.StateSnapshots[4];

            for (int i = 0; i < 3; i++)
            {
                TimeSeries timeSeries = new TimeSeries("Function" + i, 1.2 * i);

                for (int j = 0; j < 20; j++)
                {
                    timeSeries.Series.Add(0.43+i*j+0.031*j);
                }

                results.TimeSeries.Add(timeSeries);
            }
            results.FilePath = "some-file-path";

            simulation1.SimulationResults = results;

            SimulationParameters simulationParams1 = new SimulationParameters();
            simulationParams1.TimeSeries = results.TimeSeries;
            simulationParams1.InitialState = results.StateSnapshots[0];
            // add to simulation
            simulation1.Parameters = simulationParams1;
            // expt.addSimulation
            experiment.addSimulation(simulation1);

            XmlSerializer s = new XmlSerializer(typeof(MuCell.Model.Experiment));
            TextWriter w = new StreamWriter(filename);
            s.Serialize(w, experiment);
            w.Close();

            TextReader tr = new StreamReader(filename);
            Experiment deserializedExpt = (Experiment)s.Deserialize(tr);
            tr.Close();

            Assert.IsTrue(experiment.exptEquals(deserializedExpt));
        }

        [Test]
        public void testLoadMethod()
        {
            String filename = "../../UnitTests/expt1.serialized.xml";
            Experiment e = Experiment.load(filename);
        }

    }



}
