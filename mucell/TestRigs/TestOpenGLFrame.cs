using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UnitTests.TestRigs
{
    public static class TestOpenGL1
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Begin()
        {
            
            Application.Run(new TestRigs.OpenGLForm2());
        }
    }
}