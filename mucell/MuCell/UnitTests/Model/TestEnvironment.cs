using System;
using NUnit.Framework;
using MuCell.Model;

namespace MuCell.UnitTests.Model
{
    [TestFixture] public class TestEnvironment
    {

        [Test] public void TestGroupOperators()
		{

            //create an environemnt, and an array of 100 cells
            MuCell.Model.Environment env = new MuCell.Model.Environment(new Vector3(1, 1, 1));
            CellInstance[] cells = new CellInstance[100];
            CellDefinition celDef = new CellDefinition("testCellDef");
            for (int i = 0; i < 100; i++)
            {
                CellInstance cell = new CellInstance(celDef);
                cells[i] = cell;
            }

            //assert that there are no groups yet formed in the env
            Assert.That( env.GetGroups().Count == 0 );


            //obtain an unused group index, and create a group with 25 cells in it
            int newGroupIndex1 = env.GetUnusedGroupIndex();
            for (int i = 0; i < 25; i++)
            {
                env.AddCellToGroup(newGroupIndex1, cells[i]);
            }


            //assert that there is now one group
            Assert.That(env.GetGroups().Count == 1);

            //assert that it is the correct group
            Assert.That(env.ContainsGroup(newGroupIndex1));

            //assert that there are 25 cells in the group
            Assert.That(env.CellsFromGroup(newGroupIndex1).Count == 25);

            //obtain another unused group index
            int newGroupIndex2 = env.GetUnusedGroupIndex();

            //group indexes should differ
            Assert.That(newGroupIndex1 != newGroupIndex2);

            //add some cells to the new group index
            for (int i = 25; i < 50; i++)
            {
                env.AddCellToGroup(newGroupIndex2, cells[i]);
            }

            //assert that there is now one group
            Assert.That(env.GetGroups().Count == 2);


        }
    }
}



