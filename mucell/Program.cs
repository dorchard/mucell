using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MuCell.Controller;

namespace MuCell
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary> 
        [STAThread]
        static void Main()
        {
        		// A test
        		//Model.SBML.Reader.SBMLReader s = new Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.xml");

            //MuCell.Model.SBML.Reader.SBMLReader sbml = new MuCell.Model.SBML.Reader.SBMLReader("../../UnitTests/smallest.Hopf.xml");

            //sbml.model.saveToXml("../../UnitTests/smallest.Hopf.serialized.xml");


            TestRigs.ErrorLog.InitErrorLog();
            TestRigs.ErrorLog.LogError("Error Log Begin. ");


            //MuCell.TestRigs.TestOpenGL1.Begin();
            MuCell.UnitTests.Model.TestNutrientField test = new MuCell.UnitTests.Model.TestNutrientField();
            test.TestOffsets();

            ApplicationController app = new ApplicationController();
            app.run();

            View.OpenGL.GLTextures.UnloadTextures();

        }
    }
}
