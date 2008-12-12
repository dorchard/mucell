using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MuCell.Controller;
using System.Collections;


/// <summary>
/// This class extends the Panel class and implements the ISimulationAnalyserPanelUI 
/// interface, providing a concrete graphical Panel for displaying simulation analysis data
/// 
/// This panel is to be a child of ExperimentTreePanel
/// 
/// </summary>

namespace MuCell.View
{
    class SimulationAnalyserPanelUI
        : UserControl, ISimulationAnalyserPanelUI
    {
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TableLayoutPanel tableLayoutPanel1;
        private Button runBtn;
        private Label label1;
        private ProgressBar progressBar1;
        private Button stopBtn;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Analyser3DViewPanelUI analyser3DViewPanelUI;

        private SimulationAnalyserPanelController controller;

        #region IControllable<SimulationAnalyserPanelController> Members

        public void setController(SimulationAnalyserPanelController controller)
        {
            this.controller = controller;
        }

        #endregion

        #region ISimulationAnalyserPanelUI Members

        public void addTimeSeriesGraph(List<MuCell.Model.TimeSeries> timeSeries)
        {
            Hashtable unitTypes = new Hashtable();
            foreach (MuCell.Model.TimeSeries ts in timeSeries)
            {
                if (!unitTypes.ContainsKey(ts.Parameters.Units))
                {
                    unitTypes.Add(ts.Parameters.Units, new List<MuCell.Model.TimeSeries>());
                }
                ((List<MuCell.Model.TimeSeries>)unitTypes[ts.Parameters.Units]).Add(ts);
            }

            foreach (string unitType in unitTypes.Keys)
            {
                MuCell.View.TimeSeriesGraphPanel newGraph = new MuCell.View.TimeSeriesGraphPanel((List<MuCell.Model.TimeSeries>)unitTypes[unitType]);
                newGraph.Dock = DockStyle.Fill;
                //newGraph.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
                tableLayoutPanel1.Controls.Add(newGraph);
            }
        }

        public void removeTimeSeriesGraph(string name)
        {
            tableLayoutPanel1.Controls.RemoveByKey(name);
        }

        public void clearGraphs()
        {
            tableLayoutPanel1.Controls.Clear();
        }

        public delegate void UpdateTimeSeriesGraphCallback(List<MuCell.Model.TimeSeries> timeSeries);
        public void updateTimeSeriesGraph(List<MuCell.Model.TimeSeries> timeSeries)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateTimeSeriesGraphCallback(updateTimeSeriesGraph),timeSeries);
            }
            else
            {
                if (timeSeries == null)
                {
                    foreach (TimeSeriesGraphPanel graph in tableLayoutPanel1.Controls)
                    {
                        graph.updateGraph();
                    }
                }
                else
                {
                    clearGraphs();
                    addTimeSeriesGraph(timeSeries);
                }
            }
        }


        /// <summary>
        /// Thread safe method to set the control button and progress bar states
        /// </summary>
        /// <param name="state"></param>
        public delegate void SetSimulationControlStateCallBack(SimulationControlState state);
        public void setSimulationControlStateCallbackMethod(SimulationControlState state)
        {
            if (state.isRunning)
            {
                runBtn.Enabled = false;
                stopBtn.Enabled = true;
            }
            else
            {
                runBtn.Enabled = true;
                stopBtn.Enabled = false;
            }
            double progressToShow = state.progress;
            if (progressToShow > 1)
            {
                progressToShow = 1;
            }
            else if (progressToShow < 0)
            {
                progressToShow = 0;
            }
            progressBar1.Value = (int)(progressToShow * 100);

            //this.Refresh();
        }
        public void setSimulationControlState(SimulationControlState state)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetSimulationControlStateCallBack(this.setSimulationControlStateCallbackMethod),state);
            }
            else
            {
                setSimulationControlStateCallbackMethod(state);
            }
        }

        #endregion

        public SimulationAnalyserPanelUI()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulationAnalyserPanelUI));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.analyser3DViewPanelUI = new MuCell.View.Analyser3DViewPanelUI();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.runBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.stopBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(525, 552);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.analyser3DViewPanelUI);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(517, 526);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "3d View";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // analyser3DViewPanelUI
            // 
            this.analyser3DViewPanelUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.analyser3DViewPanelUI.Location = new System.Drawing.Point(3, 3);
            this.analyser3DViewPanelUI.Name = "analyser3DViewPanelUI";
            this.analyser3DViewPanelUI.Size = new System.Drawing.Size(511, 520);
            this.analyser3DViewPanelUI.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(517, 526);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Time series";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 255F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 255F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(511, 520);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // runBtn
            // 
            this.runBtn.AutoSize = true;
            this.runBtn.Image = ((System.Drawing.Image)(resources.GetObject("runBtn.Image")));
            this.runBtn.Location = new System.Drawing.Point(110, 8);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(61, 30);
            this.runBtn.TabIndex = 1;
            this.runBtn.Text = "Run";
            this.runBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.runBtn.UseVisualStyleBackColor = true;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Simulation controls:";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(246, 8);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(270, 30);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 3;
            // 
            // stopBtn
            // 
            this.stopBtn.AutoSize = true;
            this.stopBtn.Image = ((System.Drawing.Image)(resources.GetObject("stopBtn.Image")));
            this.stopBtn.Location = new System.Drawing.Point(177, 8);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(63, 30);
            this.stopBtn.TabIndex = 4;
            this.stopBtn.Text = "Stop";
            this.stopBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.stopBtn);
            this.panel1.Controls.Add(this.runBtn);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 558);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(531, 46);
            this.panel1.TabIndex = 5;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(531, 604);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // SimulationAnalyserPanelUI
            // 
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "SimulationAnalyserPanelUI";
            this.Size = new System.Drawing.Size(531, 604);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

                                      
        public IAnalyzer3DViewPanelUI getAnalyzer3DViewPanelUI()
        {
            return (IAnalyzer3DViewPanelUI )analyser3DViewPanelUI;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.Controls.RemoveByKey("Concentration of X");
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            controller.tryRunningSimulation();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            controller.tryStoppingSimulation();
        }
    }
}
