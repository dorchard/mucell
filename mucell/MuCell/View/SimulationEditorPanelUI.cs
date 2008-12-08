using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MuCell.Controller;
using MuCell.Model;
using MuCell.Model.Solver;
using System.Text.RegularExpressions;


/// <summary>
/// This class extends the Panel class and implements the ISimulationEditorPanelUI 
/// interface, providing a concrete Panel for editing simulation parameters
/// 
/// This panel is to be a child of ExperimentTreePanel
/// 
/// </summary>

namespace MuCell.View
{
    class SimulationEditorPanelUI : UserControl, ISimulationEditorPanelUI
    {
        private Label label2;
        private Label label3;
        private NumericUpDown timeStep;
        private Label label4;
        private NumericUpDown snapshotInterval;
        private NumericUpDown simulationLength;

        private SimulationEditorPanelController controller;
        private ComboBox solverSelect;
        private Label solverSelectTxt;
        private NumericUpDown relativeTolerance;
        private GroupBox groupBoxTimeSettings;
        private Button btnAdvancedSettings;
        private GroupBox groupBoxEngineSettings;
        private Label relativeToleranceTxt;

        public SimulationEditorPanelUI()
        {
            InitializeComponent();
            solverSelect.DataSource = System.Enum.GetValues(typeof(Model.Solver.SolverMethods));
        }

        private void InitializeComponent()
        {
            this.simulationLength = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timeStep = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.snapshotInterval = new System.Windows.Forms.NumericUpDown();
            this.solverSelect = new System.Windows.Forms.ComboBox();
            this.solverSelectTxt = new System.Windows.Forms.Label();
            this.relativeTolerance = new System.Windows.Forms.NumericUpDown();
            this.relativeToleranceTxt = new System.Windows.Forms.Label();
            this.groupBoxTimeSettings = new System.Windows.Forms.GroupBox();
            this.btnAdvancedSettings = new System.Windows.Forms.Button();
            this.groupBoxEngineSettings = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.simulationLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.snapshotInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.relativeTolerance)).BeginInit();
            this.groupBoxTimeSettings.SuspendLayout();
            this.groupBoxEngineSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // simulationLength
            // 
            this.simulationLength.DecimalPlaces = 2;
            this.simulationLength.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.simulationLength.Location = new System.Drawing.Point(167, 19);
            this.simulationLength.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.simulationLength.Name = "simulationLength";
            this.simulationLength.Size = new System.Drawing.Size(120, 20);
            this.simulationLength.TabIndex = 2;
            this.simulationLength.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.simulationLength.ValueChanged += new System.EventHandler(this.simulationLength_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Length of Simulation (seconds):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Time step (seconds):";
            // 
            // timeStep
            // 
            this.timeStep.DecimalPlaces = 2;
            this.timeStep.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.timeStep.Location = new System.Drawing.Point(167, 49);
            this.timeStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.timeStep.Name = "timeStep";
            this.timeStep.Size = new System.Drawing.Size(120, 20);
            this.timeStep.TabIndex = 5;
            this.timeStep.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.timeStep.ValueChanged += new System.EventHandler(this.timeStep_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Snapshot interval (timesteps):";
            // 
            // snapshotInterval
            // 
            this.snapshotInterval.Location = new System.Drawing.Point(167, 76);
            this.snapshotInterval.Name = "snapshotInterval";
            this.snapshotInterval.Size = new System.Drawing.Size(120, 20);
            this.snapshotInterval.TabIndex = 7;
            this.snapshotInterval.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.snapshotInterval.ValueChanged += new System.EventHandler(this.snapshotInterval_ValueChanged);
            // 
            // solverSelect
            // 
            this.solverSelect.FormattingEnabled = true;
            this.solverSelect.Location = new System.Drawing.Point(108, 19);
            this.solverSelect.Name = "solverSelect";
            this.solverSelect.Size = new System.Drawing.Size(121, 21);
            this.solverSelect.TabIndex = 18;
            this.solverSelect.SelectedIndexChanged += new System.EventHandler(this.solverSelect_SelectedIndexChanged);
            // 
            // solverSelectTxt
            // 
            this.solverSelectTxt.AutoSize = true;
            this.solverSelectTxt.Location = new System.Drawing.Point(6, 22);
            this.solverSelectTxt.Name = "solverSelectTxt";
            this.solverSelectTxt.Size = new System.Drawing.Size(64, 13);
            this.solverSelectTxt.TabIndex = 19;
            this.solverSelectTxt.Text = "ODE solver:";
            // 
            // relativeTolerance
            // 
            this.relativeTolerance.DecimalPlaces = 10;
            this.relativeTolerance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            589824});
            this.relativeTolerance.Location = new System.Drawing.Point(108, 49);
            this.relativeTolerance.Name = "relativeTolerance";
            this.relativeTolerance.Size = new System.Drawing.Size(120, 20);
            this.relativeTolerance.TabIndex = 20;
            this.relativeTolerance.ValueChanged += new System.EventHandler(this.relativeTolerance_ValueChanged);
            // 
            // relativeToleranceTxt
            // 
            this.relativeToleranceTxt.AutoSize = true;
            this.relativeToleranceTxt.Location = new System.Drawing.Point(6, 49);
            this.relativeToleranceTxt.Name = "relativeToleranceTxt";
            this.relativeToleranceTxt.Size = new System.Drawing.Size(96, 13);
            this.relativeToleranceTxt.TabIndex = 21;
            this.relativeToleranceTxt.Text = "Relative tolerance:";
            // 
            // groupBoxTimeSettings
            // 
            this.groupBoxTimeSettings.Controls.Add(this.simulationLength);
            this.groupBoxTimeSettings.Controls.Add(this.label2);
            this.groupBoxTimeSettings.Controls.Add(this.label3);
            this.groupBoxTimeSettings.Controls.Add(this.timeStep);
            this.groupBoxTimeSettings.Controls.Add(this.label4);
            this.groupBoxTimeSettings.Controls.Add(this.snapshotInterval);
            this.groupBoxTimeSettings.Location = new System.Drawing.Point(3, 3);
            this.groupBoxTimeSettings.Name = "groupBoxTimeSettings";
            this.groupBoxTimeSettings.Size = new System.Drawing.Size(295, 109);
            this.groupBoxTimeSettings.TabIndex = 22;
            this.groupBoxTimeSettings.TabStop = false;
            this.groupBoxTimeSettings.Text = "Simulation time settings";
            // 
            // btnAdvancedSettings
            // 
            this.btnAdvancedSettings.Location = new System.Drawing.Point(3, 118);
            this.btnAdvancedSettings.Name = "btnAdvancedSettings";
            this.btnAdvancedSettings.Size = new System.Drawing.Size(162, 23);
            this.btnAdvancedSettings.TabIndex = 23;
            this.btnAdvancedSettings.Text = "Show advanced settings >>>";
            this.btnAdvancedSettings.UseVisualStyleBackColor = true;
            this.btnAdvancedSettings.Click += new System.EventHandler(this.btnAdvancedSettings_Click);
            // 
            // groupBoxEngineSettings
            // 
            this.groupBoxEngineSettings.Controls.Add(this.solverSelect);
            this.groupBoxEngineSettings.Controls.Add(this.solverSelectTxt);
            this.groupBoxEngineSettings.Controls.Add(this.relativeTolerance);
            this.groupBoxEngineSettings.Controls.Add(this.relativeToleranceTxt);
            this.groupBoxEngineSettings.Location = new System.Drawing.Point(3, 147);
            this.groupBoxEngineSettings.Name = "groupBoxEngineSettings";
            this.groupBoxEngineSettings.Size = new System.Drawing.Size(239, 79);
            this.groupBoxEngineSettings.TabIndex = 24;
            this.groupBoxEngineSettings.TabStop = false;
            this.groupBoxEngineSettings.Text = "Simulation engine settings";
            this.groupBoxEngineSettings.Visible = false;
            // 
            // SimulationEditorPanelUI
            // 
            this.Controls.Add(this.groupBoxEngineSettings);
            this.Controls.Add(this.btnAdvancedSettings);
            this.Controls.Add(this.groupBoxTimeSettings);
            this.Name = "SimulationEditorPanelUI";
            this.Size = new System.Drawing.Size(595, 435);
            ((System.ComponentModel.ISupportInitialize)(this.simulationLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.snapshotInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.relativeTolerance)).EndInit();
            this.groupBoxTimeSettings.ResumeLayout(false);
            this.groupBoxTimeSettings.PerformLayout();
            this.groupBoxEngineSettings.ResumeLayout(false);
            this.groupBoxEngineSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #region IControllable<SimulationEditorPanelController> Members

        public void setController(SimulationEditorPanelController controller)
        {
            this.controller = controller;
        }

        #endregion

        #region ISimulationEditorPanelUI Members

        public void setSimulationParameters(SimulationParameters simulationParameters)
        {
            Console.WriteLine("Setting UI from simulation "+simulationParameters);
            try
            {
                timeStep.Value = (decimal)simulationParameters.StepTime;
            }
            catch
            {
                MessageBox.Show("Invalid number (negative)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            try
            {
                snapshotInterval.Value = (decimal)simulationParameters.SnapshotInterval;
            }
            catch
            {
                MessageBox.Show("Invalid number (negative)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            try
            {
                simulationLength.Value = (decimal)simulationParameters.SimulationLength;
            }
            catch
            {
                MessageBox.Show("Invalid number (negative)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            solverSelect.SelectedItem = simulationParameters.SolverMethod;
            try
            {
                relativeTolerance.Value = (decimal)simulationParameters.RelativeTolerance;
            }
            catch
            {
                MessageBox.Show("Invalid number (negative)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void getSimulationParameters(SimulationParameters simulationParameters)
        {
            Console.WriteLine("Getting simulation parameters from UI");
            //fill in the values here
            simulationParameters.StepTime = (double)timeStep.Value;
            //Console.WriteLine("my step time is =" + timeStep.Value);
            simulationParameters.SnapshotInterval = (double)snapshotInterval.Value;
            simulationParameters.SimulationLength = (double)simulationLength.Value;
            simulationParameters.SolverMethod = (SolverMethods)solverSelect.SelectedValue;
            simulationParameters.RelativeTolerance = (double)relativeTolerance.Value;
            //Console.WriteLine("steptime=" + simulationParameters.StepTime);
            //Console.WriteLine("snapshot=" + simulationParameters.SnapshotInterval);
            //Console.WriteLine("simlength=" + simulationParameters.SimulationLength);
            //Console.WriteLine("solver=" + simulationParameters.SolverMethod);
        }



        #endregion

        /// <summary>
        /// Cause the controller to update the simulation from the entered data, and to show changes in the treepanel
        /// </summary>
        private void commitToSimulation()
        {
            //not actually used, just makes sure to save data when moving to analysis tab
            //Console.WriteLine("Committing data to simulation object");

        }



        private void simulationLength_ValueChanged(object sender, EventArgs e)
        {
            commitToSimulation();
        }
        private void timeStep_ValueChanged(object sender, EventArgs e)
        {
            commitToSimulation();
        }
        private void snapshotInterval_ValueChanged(object sender, EventArgs e)
        {
            commitToSimulation();
        }
        private void solverSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            commitToSimulation();
        }

        private void relativeTolerance_ValueChanged(object sender, EventArgs e)
        {
            commitToSimulation();
        }

        private void btnAdvancedSettings_Click(object sender, EventArgs e)
        {
            if (groupBoxEngineSettings.Visible)
            {
                groupBoxEngineSettings.Visible = false;
                btnAdvancedSettings.Text = "Show advanced settings >>>";
            }
            else
            {
                groupBoxEngineSettings.Visible = true;
                btnAdvancedSettings.Text = "Hide advanced settings <<<";
            }
        }


    }
}
