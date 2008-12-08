using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MuCell.Controller;
using MuCell.Model;

namespace MuCell.View
{
    public partial class ApplicationUI
        : Form, IApplicationUI, IControllable<ApplicationController>
    {
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem newExperimentToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem simulationToolStripMenuItem;
        private ToolStripMenuItem cellDefinitionToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;

        // child panel
        private IContainer components;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem saveAllExperimentsToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private ExperimentTreePanelUI experimentTreePanelUI;

        private ApplicationController controller;


        #region Constructor

        public ApplicationUI()
        {
            InitializeComponent();
            keysDown = new List<Keys>();
        }

        #endregion

        #region IApplicationUI Members

        public IExperimentTreePanelUI getExperimentTreePanelUI()
        {
            return experimentTreePanelUI;
        }

        #endregion

        #region IControllable<ApplicationController> Members

        public void setController(ApplicationController controller)
        {
            this.controller = controller;
        }

        #endregion

        #region Events

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Form Designer

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newExperimentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllExperimentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cellDefinitionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.experimentTreePanelUI = new MuCell.View.ExperimentTreePanelUI();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.addToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(883, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newExperimentToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveAllExperimentsToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newExperimentToolStripMenuItem
            // 
            this.newExperimentToolStripMenuItem.Name = "newExperimentToolStripMenuItem";
            this.newExperimentToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.newExperimentToolStripMenuItem.Text = "New Experiment";
            this.newExperimentToolStripMenuItem.Click += new System.EventHandler(this.newExperimentToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.openToolStripMenuItem.Text = "Open Experiment";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAllExperimentsToolStripMenuItem
            // 
            this.saveAllExperimentsToolStripMenuItem.Name = "saveAllExperimentsToolStripMenuItem";
            this.saveAllExperimentsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveAllExperimentsToolStripMenuItem.Text = "Save all experiments";
            this.saveAllExperimentsToolStripMenuItem.Click += new System.EventHandler(this.saveAllExperimentsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.simulationToolStripMenuItem,
            this.cellDefinitionToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // simulationToolStripMenuItem
            // 
            this.simulationToolStripMenuItem.Name = "simulationToolStripMenuItem";
            this.simulationToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.simulationToolStripMenuItem.Text = "Simulation";
            this.simulationToolStripMenuItem.Click += new System.EventHandler(this.simulationToolStripMenuItem_Click);
            // 
            // cellDefinitionToolStripMenuItem
            // 
            this.cellDefinitionToolStripMenuItem.Name = "cellDefinitionToolStripMenuItem";
            this.cellDefinitionToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.cellDefinitionToolStripMenuItem.Text = "Cell Definition";
            this.cellDefinitionToolStripMenuItem.Click += new System.EventHandler(this.cellDefinitionToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "XML files (.xml)|*.xml|All files|*.*";
            this.openFileDialog1.Title = "Open Experiment";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "XML files (.xml)|*.xml|All files|*.*";
            this.saveFileDialog1.Title = "Save Experiment";
            // 
            // experimentTreePanelUI
            // 
            this.experimentTreePanelUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.experimentTreePanelUI.Location = new System.Drawing.Point(0, 24);
            this.experimentTreePanelUI.Name = "experimentTreePanelUI";
            this.experimentTreePanelUI.Size = new System.Drawing.Size(883, 571);
            this.experimentTreePanelUI.TabIndex = 1;
            // 
            // ApplicationUI
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(883, 595);
            this.Controls.Add(this.experimentTreePanelUI);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ApplicationUI";
            this.Text = "muCell";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ApplicationUI_FormClosed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ApplicationUI_KeyUp);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ApplicationUI_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ApplicationUI_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private static List<Keys> keysDown;
        public static bool isKeyDown(Keys key)
        {
            return keysDown.Contains(key);
        }
        private static void pressKey(Keys key)
        {
            if (!isKeyDown(key))
            {
                keysDown.Add(key);
            }
        }
        private static void releaseKey(Keys key)
        {
            if (isKeyDown(key))
            {
                keysDown.Remove(key);
            }
        }

        private void ApplicationUI_KeyUp(object sender, KeyEventArgs e)
        {
            //Console.WriteLine("Button released=" + e.KeyCode);
            if (e.Control && e.KeyCode == Keys.Z)
            {
                controller.undo();
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                controller.redo();
            }
            if (e.KeyCode == Keys.Delete)
            {
                controller.delete();
            }
            releaseKey(e.KeyCode);
            
        }
        private void ApplicationUI_KeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine("Button pressed=" + e.KeyCode);
            pressKey(e.KeyCode);
        }

        private void newExperimentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.createNewExperiment();
        }

        private void simulationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.createNewSimulation();
        }

        private void cellDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.createNewCellDefinition();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.redo();
        }

        private void ApplicationUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("Application closing");
            bool notReadyToClose=Simulator.getSimulator().stopAllSimulations() || GraphLayoutThreadManager.getGraphLayoutThreadManager().cleanUpThreads();
            if (notReadyToClose)
            {
                e.Cancel = true;
            }
        }

        private void ApplicationUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.WriteLine("Application closed");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = System.Environment.CurrentDirectory;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                String fileName = openFileDialog1.FileName;
                if (fileName != null && fileName.Length > 0)
                {
                    try
                    {
                        Experiment loaded = Experiment.load(fileName);
                        if (loaded != null)
                        {
                            controller.addExperiment(loaded);
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Invalid experiment file\n"+exception.Message + "\n" + (exception.InnerException!=null?exception.InnerException.Message:""), "Error loading experiment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void saveAllExperimentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<IExperimentView> experiments = controller.getExperiments();

            foreach (IExperimentView experiment in experiments)
            {
                String lastSavePath = experiment.getLastSavedPath();

                bool shouldUseSaveAs = true;

                if (lastSavePath != null && lastSavePath.Length != 0)
                {
                    shouldUseSaveAs = false;
                }

                String actualSavePath = "";

                if (shouldUseSaveAs)
                {
                    //show file save dialog to "save as"
                    saveFileDialog1.InitialDirectory = System.Environment.CurrentDirectory;
                    DialogResult result = saveFileDialog1.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        String fileName = saveFileDialog1.FileName;
                        if (fileName != null && fileName.Length > 0)
                        {
                            actualSavePath = fileName;
                        }
                    }

                }
                else
                {
                    //just save to the old location
                    actualSavePath = lastSavePath;
                }

                if (actualSavePath.Length > 0)
                {
                    experiment.save(actualSavePath);
                }
            }
            controller.refreshTreeElements();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("muCell is a multi-cellular biological modelling tool\n\nWritten by: \n\nDominic Orchard, Duncan Stead, Cathy Young,\nJames Lohr, Jonathan Gover and Lee Herrington", "About...", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        



    }
}
