using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MuCell.Controller;
using System.Drawing;

/// <summary>
/// This class extends the Panel class and implements the IExperimentTreePanel 
/// interface, providing a concrete Panel for the tree view of experiments in
/// both the simulation view and the cell definitions view.
/// 
/// This panel is a child of the ApplicationUI form
/// 
/// </summary>

namespace MuCell.View
{



    public class ExperimentTreePanelUI
        : UserControl, IExperimentTreePanelUI
    {

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TreeView simulationsTreeView;
        private TreeView cellsTreeView;
        private TabControl tabControl2;
        private TabPage tabPage4;
        private TabPage tabPage5;

        //child panels
        private CellDefinitionsPanelUI cellDefinitionsPanelUI1;
        private SimulationAnalyserPanelUI simulationAnalyserPanelUI1;
        private TabControl tabControl3;
        private TabPage tabPage3;
        private TabPage tabPage6;
        private SpatialConfigurationPanelUI spatialConfigurationPanelUI1;
        private SimulationEditorPanelUI simulationEditorPanelUI1;
        private TabPage tabPage7;
        private TimeSeriesEditorPanelUI timeSeriesEditorPanelUI1;
        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem deleteToolStripMenuItem1;
        private ImageList imageList1;
        private Panel panel1;

        private ExperimentTreePanelController controller;

        #region IExperimentTreePanelUI Members

        public ISimulationEditorPanelUI getSimulationEditorPanelUI()
        {
            return simulationEditorPanelUI1;
        }

        public ISimulationAnalyserPanelUI getSimulationAnalyserPanelUI()
        {
            return simulationAnalyserPanelUI1;

        }

        public ICellDefinitionsPanelUI getCellDefinitionsPanelUI()
        {
            return cellDefinitionsPanelUI1;

        }

        public ISpatialConfigurationPanelUI getSpatialConfigurationPanelUI()
        {
            return spatialConfigurationPanelUI1;

        }


        public ITimeSeriesEditorPanelUI getTimeSeriesEditorPanelUI()
        {
            return timeSeriesEditorPanelUI1;
        }


        //extends the TreeNode class adding an ID member
        class IDTreeNode
            : TreeNode
        {
            public int ID;
        }

        /// <summary>
        /// Used when merging trees, comparing one of the new nodes with the old ones to see if it needs to be expanded or selected
        /// </summary>
        private void compareWithOld(TreeView newTree, TreeNode node, TreeNode[] oldTree)
        {
            if (node != null)
            {
                for (int i = 0; i < oldTree.Length - 1; i++)
                {
                    TreeNode oldnode = oldTree[i];
                    if (((string)node.Tag).Equals((string)oldnode.Tag))
                    {
                        if (oldnode.IsExpanded)
                        {
                            node.Expand();
                        }
                        if (oldnode.IsSelected)
                        {
                            newTree.SelectedNode = node;
                        }
                    }
                }
                if (oldTree[oldTree.Length - 1] != null)
                {
                    if (((string)node.Tag).Equals((string)oldTree[oldTree.Length - 1].Tag))
                    {
                        newTree.SelectedNode = node;
                    }
                }
            }
        }

        /// <summary>
        /// When updating the tree with the new experiments, want to keep the same nodes expanded or selected as before.
        /// </summary>
        /// <param name="newTree">The new tree that wants the old expansions/selection</param>
        /// <param name="oldTree">All the old tree nodes in an array, with an extra element for the old selected node</param>
        private void mergeTreeStates(TreeView newTree, TreeNode[] oldTree)
        {
            foreach (TreeNode node in newTree.Nodes)
            {
                if (node != null)
                {
                    compareWithOld(newTree, node, oldTree);
                    foreach (TreeNode innernode in node.Nodes)
                    {
                        if (innernode != null)
                        {
                            compareWithOld(newTree, innernode, oldTree);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update the experiment tree panel with the latest experiment structure
        /// </summary>
        /// <param name="openExperiments">The currently open experiments, each containing simulations and cell definitions</param>
        public void setOpenExperiments(List<MuCell.Model.IExperimentView> openExperiments)
        {
            TreeNode[] oldTree = new TreeNode[simulationsTreeView.Nodes.Count + 1];
            oldTree[oldTree.Length - 1] = simulationsTreeView.SelectedNode;

            simulationsTreeView.Nodes.CopyTo(oldTree, 0);
            simulationsTreeView.Nodes.Clear();

            foreach (Model.IExperimentView experiment in openExperiments)
            {
                TreeNode expNode = new TreeNode(experiment.Name);
                expNode.Tag = string.Format("{0}", experiment.Id);
                foreach (Model.Simulation sim in experiment.getSimulations())
                {
                    TreeNode simNode = new TreeNode(sim.Name + (sim.Parameters.Changed ? "*" : ""));
                    simNode.Tag = sim.Name;
                    expNode.Nodes.Add(simNode);

                }
                simulationsTreeView.Nodes.Add(expNode);
            }
            mergeTreeStates(simulationsTreeView, oldTree);

            oldTree = new TreeNode[cellsTreeView.Nodes.Count + 1];
            oldTree[oldTree.Length - 1] = cellsTreeView.SelectedNode;

            cellsTreeView.Nodes.CopyTo(oldTree, 0);
            cellsTreeView.Nodes.Clear();
            foreach (Model.IExperimentView experiment in openExperiments)
            {
                TreeNode expNode = new TreeNode(experiment.Name);
                expNode.Tag = experiment.Id + "";
                foreach (Model.CellDefinition cellDef in experiment.getCellDefinitions())
                {
                    TreeNode cellDefNode = new TreeNode(cellDef.Name);
                    cellDefNode.Tag = cellDef.Name;
                    expNode.Nodes.Add(cellDefNode);
                }
                cellsTreeView.Nodes.Add(expNode);
            }
            mergeTreeStates(cellsTreeView, oldTree);

            /* dom's code

            //clear both node trees
            simulationsTreeView.Nodes.Clear();
            simulationsTreeView.ItemHeight += 8;

            cellsTreeView.Nodes.Clear();
            cellsTreeView.ItemHeight += 8;

            foreach (MuCell.Model.IExperimentView exp in openExperiments)
            {
                //create a node for each experiment  
                IDTreeNode expNode = new IDTreeNode();
                expNode.Text = exp.name;
                expNode.ID = exp.ID;
                expNode.Expand();

                Font font1 = new Font("Arial", 10.0f);
                font1 = new Font(font1, FontStyle.Bold);
                expNode.NodeFont = font1;
                
  
                foreach (MuCell.Model.Simulation sim in exp.getSimulations())
                {
                    //create a node for each simulation in the experiment
                    TreeNode simNode = new TreeNode();
                    simNode.Text = sim.name;

                    expNode.Nodes.Add(simNode);

                }
                
                simulationsTreeView.Nodes.Add(expNode);

      
            }


            foreach (MuCell.Model.IExperimentView exp in openExperiments)
            {
                //create a node for each experiment  
                IDTreeNode expNode = new IDTreeNode();
                expNode.Text = exp.name;
                expNode.ID = exp.ID;
                expNode.Expand();

                Font font1 = new Font("Arial", 10.0f);
                font1 = new Font(font1, FontStyle.Bold);
                expNode.NodeFont = font1;

                foreach (MuCell.Model.CellDefinition cellDef in exp.getCellDefinitions())
                {
                    //create a node for each simulation in the experiment
                    TreeNode cellDefNode = new TreeNode();
                    cellDefNode.Text = cellDef.name;

                    expNode.Nodes.Add(cellDefNode);

                }
               
                cellsTreeView.Nodes.Add(expNode);

            }

            */
        }

        public void setSelectedExperiment(int experimentID)
        {
            foreach (TreeNode node in simulationsTreeView.Nodes)
            {
                if (((string)node.Tag).Equals(experimentID + ""))
                {
                    simulationsTreeView.SelectedNode = node;
                }
            }
            foreach (TreeNode node in cellsTreeView.Nodes)
            {
                if (((string)node.Tag).Equals(experimentID + ""))
                {
                    cellsTreeView.SelectedNode = node;
                }
            }

        }

        public void setSelectedSimulation(string simulationName)
        {
            if (simulationsTreeView.SelectedNode != null)
            {
                foreach (TreeNode node in simulationsTreeView.SelectedNode.Nodes)
                {
                    if (((string)node.Tag).Equals(simulationName))
                    {
                        simulationsTreeView.SelectedNode = node;
                        if (!simulationsTreeView.SelectedNode.IsExpanded)
                        {
                            simulationsTreeView.SelectedNode.Expand();
                        }
                    }
                }
                switchTabs(0);
            }
        }

        public void setSelectedCellDefinition(string cellDefinitionName)
        {
            if (cellsTreeView.SelectedNode != null)
            {
                foreach (TreeNode node in cellsTreeView.SelectedNode.Nodes)
                {
                    if (((string)node.Tag).Equals(cellDefinitionName))
                    {
                        cellsTreeView.SelectedNode = node;
                        if (!cellsTreeView.SelectedNode.IsExpanded)
                        {
                            cellsTreeView.SelectedNode.Expand();
                        }
                    }
                }
                switchTabs(1);
            }
        }

        public void showSimulationView()
        {
            tabControl1.SelectedTab = tabControl1.TabPages[0];
        }

        public void showCellDefinitionsView()
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];

        }

        #endregion


        public ExperimentTreePanelUI()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.simulationEditorPanelUI1 = new MuCell.View.SimulationEditorPanelUI();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.spatialConfigurationPanelUI1 = new MuCell.View.SpatialConfigurationPanelUI();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.timeSeriesEditorPanelUI1 = new MuCell.View.TimeSeriesEditorPanelUI();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.simulationAnalyserPanelUI1 = new MuCell.View.SimulationAnalyserPanelUI();
            this.simulationsTreeView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cellDefinitionsPanelUI1 = new MuCell.View.CellDefinitionsPanelUI();
            this.cellsTreeView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(852, 543);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabControl2);
            this.tabPage1.Controls.Add(this.simulationsTreeView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(844, 517);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Simulations";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Location = new System.Drawing.Point(133, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(708, 511);
            this.tabControl2.TabIndex = 1;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tabControl3);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(700, 485);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Editor";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage3);
            this.tabControl3.Controls.Add(this.tabPage6);
            this.tabControl3.Controls.Add(this.tabPage7);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(3, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(694, 479);
            this.tabControl3.TabIndex = 0;
            this.tabControl3.SelectedIndexChanged += new System.EventHandler(this.tabControl3_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.simulationEditorPanelUI1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(686, 453);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Simulation Parameters";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // simulationEditorPanelUI1
            // 
            this.simulationEditorPanelUI1.Location = new System.Drawing.Point(0, 0);
            this.simulationEditorPanelUI1.Name = "simulationEditorPanelUI1";
            this.simulationEditorPanelUI1.Size = new System.Drawing.Size(595, 373);
            this.simulationEditorPanelUI1.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.spatialConfigurationPanelUI1);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(686, 453);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "Spatial Setup";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // spatialConfigurationPanelUI1
            // 
            this.spatialConfigurationPanelUI1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spatialConfigurationPanelUI1.Location = new System.Drawing.Point(3, 3);
            this.spatialConfigurationPanelUI1.Name = "spatialConfigurationPanelUI1";
            this.spatialConfigurationPanelUI1.Size = new System.Drawing.Size(680, 447);
            this.spatialConfigurationPanelUI1.TabIndex = 0;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.timeSeriesEditorPanelUI1);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(686, 453);
            this.tabPage7.TabIndex = 2;
            this.tabPage7.Text = "Time Series Setup";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // timeSeriesEditorPanelUI1
            // 
            this.timeSeriesEditorPanelUI1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeSeriesEditorPanelUI1.Location = new System.Drawing.Point(0, 0);
            this.timeSeriesEditorPanelUI1.Name = "timeSeriesEditorPanelUI1";
            this.timeSeriesEditorPanelUI1.Size = new System.Drawing.Size(686, 453);
            this.timeSeriesEditorPanelUI1.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.simulationAnalyserPanelUI1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(700, 485);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Analyser";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // simulationAnalyserPanelUI1
            // 
            this.simulationAnalyserPanelUI1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simulationAnalyserPanelUI1.Location = new System.Drawing.Point(0, 0);
            this.simulationAnalyserPanelUI1.Name = "simulationAnalyserPanelUI1";
            this.simulationAnalyserPanelUI1.Size = new System.Drawing.Size(700, 485);
            this.simulationAnalyserPanelUI1.TabIndex = 0;
            // 
            // simulationsTreeView
            // 
            this.simulationsTreeView.ContextMenuStrip = this.contextMenuStrip1;
            this.simulationsTreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.simulationsTreeView.HideSelection = false;
            this.simulationsTreeView.LabelEdit = true;
            this.simulationsTreeView.Location = new System.Drawing.Point(3, 3);
            this.simulationsTreeView.Name = "simulationsTreeView";
            this.simulationsTreeView.Size = new System.Drawing.Size(119, 511);
            this.simulationsTreeView.TabIndex = 0;
            this.simulationsTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.simulationsTreeView_AfterLabelEdit);
            this.simulationsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.simulationsTreeView_AfterSelect);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(117, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.cellsTreeView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Cell Definitions";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.cellDefinitionsPanelUI1);
            this.panel1.Location = new System.Drawing.Point(131, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(55, 62);
            this.panel1.TabIndex = 3;
            // 
            // cellDefinitionsPanelUI1
            // 
            this.cellDefinitionsPanelUI1.AutoSize = true;
            this.cellDefinitionsPanelUI1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cellDefinitionsPanelUI1.Location = new System.Drawing.Point(0, 0);
            this.cellDefinitionsPanelUI1.Name = "cellDefinitionsPanelUI1";
            this.cellDefinitionsPanelUI1.Size = new System.Drawing.Size(55, 62);
            this.cellDefinitionsPanelUI1.TabIndex = 2;
            // 
            // cellsTreeView
            // 
            this.cellsTreeView.ContextMenuStrip = this.contextMenuStrip2;
            this.cellsTreeView.HideSelection = false;
            this.cellsTreeView.LabelEdit = true;
            this.cellsTreeView.Location = new System.Drawing.Point(6, 6);
            this.cellsTreeView.Name = "cellsTreeView";
            this.cellsTreeView.Size = new System.Drawing.Size(119, 463);
            this.cellsTreeView.TabIndex = 1;
            this.cellsTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.cellsTreeView_AfterLabelEdit);
            this.cellsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.cellsTreeView_AfterSelect);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(117, 26);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(116, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ExperimentTreePanelUI
            // 
            this.Controls.Add(this.tabControl1);
            this.Name = "ExperimentTreePanelUI";
            this.Size = new System.Drawing.Size(852, 543);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public void switchTabs(int tab)
        {
            tabControl1.SelectedIndex = tab;
        }

        private void simulationsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (controller != null)
            {
                if (e.Node.Level == 0)
                {
                    // experiments are level 0 in the tree
                    controller.switchToExperiment(int.Parse((string)e.Node.Tag));
                }
                else if (e.Node.Level == 1)
                {
                    // simulations are level 1 in the tree
                    controller.switchToExperiment(int.Parse((string)e.Node.Parent.Tag));
                    controller.switchToSimulation((string)e.Node.Tag);
                }
            }
        }

        private void cellsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (controller != null)
            {
                if (e.Node.Level == 0)
                {
                    // experiments are level 0 in the tree
                    controller.switchToExperiment(int.Parse((string)e.Node.Tag));
                }
                else if (e.Node.Level == 1)
                {
                    // cell definitions are level 1 in the tree
                    controller.switchToExperiment(int.Parse((string)e.Node.Parent.Tag));
                    controller.switchToCellDefinition((string)e.Node.Tag);
                }
            }
        }

        #region IControllable<ExperimentTreePanelController> Members

        public void setController(ExperimentTreePanelController controller)
        {
            this.controller = controller;
        }

        #endregion

        private void renameExperimentNode(int experimentID, String newName, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (int.Parse((string)node.Tag) == experimentID)
                {
                    node.Text = newName;
                }
            }
        }
        private String getNewNodeName(NodeLabelEditEventArgs e)
        {
            String newName = e.Label;

            //ignore the final "*" special character
            if (newName != null)
            {
                newName = newName.Replace("*", "");
            }

            return newName;
        }
        private void simulationsTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            String newName = getNewNodeName(e);
            if (newName == null || newName.Length == 0)
            {
                e.CancelEdit = true;
                return;
            }
            bool renamed = false;
            if (e.Node.Level == 0)
            {
                int experimentID = int.Parse((string)e.Node.Tag);
                renamed = controller.renameExperiment(experimentID, newName);
                if (renamed)
                {
                    //find experiment in other list and rename it too
                    TreeNodeCollection nodes = cellsTreeView.Nodes;
                    renameExperimentNode(experimentID, newName, nodes);
                }
            }
            else if (e.Node.Level == 1)
            {
                int experimentID = int.Parse((string)e.Node.Parent.Tag);
                String oldName = (string)e.Node.Tag;
                renamed = controller.renameSimulation(experimentID, oldName, newName);
                if (renamed)
                {
                    e.Node.Tag = newName;
                }
            }

            if (!renamed)
            {
                e.CancelEdit = true;
            }
        }
        private void cellsTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            String newName = getNewNodeName(e);

            if (newName == null || newName.Length == 0)
            {
                e.CancelEdit = true;
                return;
            }

            bool renamed = false;
            if (e.Node.Level == 0)
            {
                int experimentID = int.Parse((string)e.Node.Tag);
                renamed = controller.renameExperiment(experimentID, newName);
                if (renamed)
                {
                    //find experiment in other list and rename it too
                    TreeNodeCollection nodes = simulationsTreeView.Nodes;
                    renameExperimentNode(experimentID, newName, nodes);
                }
            }
            else if (e.Node.Level == 1)
            {
                int experimentID = int.Parse((string)e.Node.Parent.Tag);
                String oldName = (string)e.Node.Tag;
                renamed = controller.renameCellDefinition(experimentID, oldName, newName);
                if (renamed)
                {
                    e.Node.Tag = newName;
                }
            }

            if (!renamed)
            {
                e.CancelEdit = true;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = simulationsTreeView.SelectedNode;
            if (node != null)
            {
                if (node.Level == 0)
                {
                    int experimentID = int.Parse((string)node.Tag);
                    controller.deleteExperiment(experimentID);
                }
                else if (node.Level == 1)
                {
                    int experimentID = int.Parse((string)node.Parent.Tag);
                    String simulationName = (string)node.Tag;
                    controller.deleteSimulation(experimentID, simulationName);
                }
            }
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode node = cellsTreeView.SelectedNode;
            if (node != null)
            {
                if (node.Level == 0)
                {
                    int experimentID = int.Parse((string)node.Tag);
                    controller.deleteExperiment(experimentID);
                }
                else if (node.Level == 1)
                {
                    int experimentID = int.Parse((string)node.Parent.Tag);
                    String cellDefinitionName = (string)node.Tag;
                    controller.deleteCellDefinition(experimentID, cellDefinitionName);
                }
            }
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Simulation tab changed to " + tabControl2.SelectedIndex);

            //update simulation parameters from the editor panes
            controller.refreshCurrentSimulationData();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //set the current experiment and simulation/cell definition from tree panel

            if (controller != null)
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    //simulations tree

                    TreeNode selectedNode=simulationsTreeView.SelectedNode;
                    if (selectedNode != null)
                    {

                        if (selectedNode.Level == 0)
                        {
                            // experiments are level 0 in the tree
                            controller.switchToExperiment(int.Parse((string)selectedNode.Tag));
                        }
                        else if (selectedNode.Level == 1)
                        {
                            // simulations are level 1 in the tree
                            controller.switchToExperiment(int.Parse((string)selectedNode.Parent.Tag));
                            controller.switchToSimulation((string)selectedNode.Tag);
                        }
                    }
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    //cells tree
                    
                    TreeNode selectedNode = cellsTreeView.SelectedNode;
                    if (selectedNode != null)
                    {
                        if (selectedNode.Level == 0)
                        {
                            // experiments are level 0 in the tree
                            controller.switchToExperiment(int.Parse((string)selectedNode.Tag));
                        }
                        else if (selectedNode.Level == 1)
                        {
                            // cell definitions are level 1 in the tree
                            controller.switchToExperiment(int.Parse((string)selectedNode.Parent.Tag));
                            controller.switchToCellDefinition((string)selectedNode.Tag);
                        }
                    }
                }
                
                
            }
        }

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            controller.refreshCurrentSimulationData();
        }
    }
}
