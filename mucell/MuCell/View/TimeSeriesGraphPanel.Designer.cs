namespace MuCell.View
{
    partial class TimeSeriesGraphPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listOfTimeSeries = new System.Windows.Forms.ListBox();
            this.picGraphBox = new System.Windows.Forms.PictureBox();
            this.graphScrollBar = new System.Windows.Forms.HScrollBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControls = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxCoordinates = new System.Windows.Forms.GroupBox();
            this.lblYCoordinate = new System.Windows.Forms.Label();
            this.lblXCoordinate = new System.Windows.Forms.Label();
            this.btnShowData = new System.Windows.Forms.Button();
            this.txtGraphUnits = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.lblZoomAmount = new System.Windows.Forms.Label();
            this.panelGraph = new System.Windows.Forms.Panel();
            this.menuSetColour = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setColourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTipNameTooLong = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picGraphBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBoxCoordinates.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panelGraph.SuspendLayout();
            this.menuSetColour.SuspendLayout();
            this.SuspendLayout();
            // 
            // listOfTimeSeries
            // 
            this.listOfTimeSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listOfTimeSeries.FormattingEnabled = true;
            this.listOfTimeSeries.Location = new System.Drawing.Point(3, 126);
            this.listOfTimeSeries.Name = "listOfTimeSeries";
            this.listOfTimeSeries.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listOfTimeSeries.Size = new System.Drawing.Size(138, 82);
            this.listOfTimeSeries.TabIndex = 4;
            this.listOfTimeSeries.MouseHover += new System.EventHandler(this.listOfTimeSeries_MouseHover);
            this.listOfTimeSeries.SelectedIndexChanged += new System.EventHandler(this.listOfTimeSeries_SelectedIndexChanged);
            this.listOfTimeSeries.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listOfTimeSeries_MouseMove);
            this.listOfTimeSeries.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listOfTimeSeries_MouseDown);
            // 
            // picGraphBox
            // 
            this.picGraphBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picGraphBox.BackColor = System.Drawing.Color.White;
            this.picGraphBox.Location = new System.Drawing.Point(3, 3);
            this.picGraphBox.Name = "picGraphBox";
            this.picGraphBox.Size = new System.Drawing.Size(498, 223);
            this.picGraphBox.TabIndex = 0;
            this.picGraphBox.TabStop = false;
            this.picGraphBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picGraphBox_MouseMove);
            this.picGraphBox.Resize += new System.EventHandler(this.picGraphBox_Resize);
            this.picGraphBox.Paint += new System.Windows.Forms.PaintEventHandler(this.picGraphBox_Paint);
            // 
            // graphScrollBar
            // 
            this.graphScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.graphScrollBar.Location = new System.Drawing.Point(0, 229);
            this.graphScrollBar.Name = "graphScrollBar";
            this.graphScrollBar.Size = new System.Drawing.Size(504, 20);
            this.graphScrollBar.TabIndex = 1;
            this.graphScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.graphScrollBar_Scroll);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelControls, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelGraph, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(660, 255);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // panelControls
            // 
            this.panelControls.BackColor = System.Drawing.SystemColors.Control;
            this.panelControls.Controls.Add(this.tableLayoutPanel2);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControls.Location = new System.Drawing.Point(3, 3);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(144, 249);
            this.panelControls.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBoxCoordinates, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnShowData, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.listOfTimeSeries, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtGraphUnits, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(144, 249);
            this.tableLayoutPanel2.TabIndex = 13;
            // 
            // groupBoxCoordinates
            // 
            this.groupBoxCoordinates.Controls.Add(this.lblYCoordinate);
            this.groupBoxCoordinates.Controls.Add(this.lblXCoordinate);
            this.groupBoxCoordinates.Location = new System.Drawing.Point(3, 76);
            this.groupBoxCoordinates.Name = "groupBoxCoordinates";
            this.groupBoxCoordinates.Size = new System.Drawing.Size(138, 44);
            this.groupBoxCoordinates.TabIndex = 11;
            this.groupBoxCoordinates.TabStop = false;
            this.groupBoxCoordinates.Text = "Coordinates";
            // 
            // lblYCoordinate
            // 
            this.lblYCoordinate.AutoSize = true;
            this.lblYCoordinate.Location = new System.Drawing.Point(6, 29);
            this.lblYCoordinate.Name = "lblYCoordinate";
            this.lblYCoordinate.Size = new System.Drawing.Size(47, 13);
            this.lblYCoordinate.TabIndex = 9;
            this.lblYCoordinate.Text = "Y: 5.324";
            // 
            // lblXCoordinate
            // 
            this.lblXCoordinate.AutoSize = true;
            this.lblXCoordinate.Location = new System.Drawing.Point(6, 16);
            this.lblXCoordinate.Name = "lblXCoordinate";
            this.lblXCoordinate.Size = new System.Drawing.Size(47, 13);
            this.lblXCoordinate.TabIndex = 8;
            this.lblXCoordinate.Text = "X: 2.161";
            // 
            // btnShowData
            // 
            this.btnShowData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowData.Location = new System.Drawing.Point(3, 223);
            this.btnShowData.Name = "btnShowData";
            this.btnShowData.Size = new System.Drawing.Size(75, 23);
            this.btnShowData.TabIndex = 7;
            this.btnShowData.Text = "Show data...";
            this.btnShowData.UseVisualStyleBackColor = true;
            this.btnShowData.Click += new System.EventHandler(this.btnShowData_Click);
            // 
            // txtGraphUnits
            // 
            this.txtGraphUnits.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGraphUnits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGraphUnits.Location = new System.Drawing.Point(3, 3);
            this.txtGraphUnits.Multiline = true;
            this.txtGraphUnits.Name = "txtGraphUnits";
            this.txtGraphUnits.ReadOnly = true;
            this.txtGraphUnits.Size = new System.Drawing.Size(138, 24);
            this.txtGraphUnits.TabIndex = 10;
            this.txtGraphUnits.Text = "Units";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel3.Controls.Add(this.btnZoomOut, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnZoomIn, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblZoomAmount, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 33);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(138, 37);
            this.tableLayoutPanel3.TabIndex = 12;
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomOut.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnZoomOut.Location = new System.Drawing.Point(104, 3);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(31, 31);
            this.btnZoomOut.TabIndex = 1;
            this.btnZoomOut.Text = "-";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomIn.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnZoomIn.Location = new System.Drawing.Point(3, 3);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(31, 31);
            this.btnZoomIn.TabIndex = 1;
            this.btnZoomIn.Text = "+";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // lblZoomAmount
            // 
            this.lblZoomAmount.AutoSize = true;
            this.lblZoomAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblZoomAmount.Location = new System.Drawing.Point(40, 0);
            this.lblZoomAmount.Name = "lblZoomAmount";
            this.lblZoomAmount.Size = new System.Drawing.Size(58, 37);
            this.lblZoomAmount.TabIndex = 2;
            this.lblZoomAmount.Text = "12.5x";
            this.lblZoomAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelGraph
            // 
            this.panelGraph.BackColor = System.Drawing.Color.White;
            this.panelGraph.Controls.Add(this.picGraphBox);
            this.panelGraph.Controls.Add(this.graphScrollBar);
            this.panelGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraph.Location = new System.Drawing.Point(153, 3);
            this.panelGraph.Name = "panelGraph";
            this.panelGraph.Size = new System.Drawing.Size(504, 249);
            this.panelGraph.TabIndex = 0;
            // 
            // menuSetColour
            // 
            this.menuSetColour.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setColourToolStripMenuItem});
            this.menuSetColour.Name = "menuSetColour";
            this.menuSetColour.Size = new System.Drawing.Size(146, 26);
            this.menuSetColour.Opening += new System.ComponentModel.CancelEventHandler(this.menuSetColour_Opening);
            // 
            // setColourToolStripMenuItem
            // 
            this.setColourToolStripMenuItem.Name = "setColourToolStripMenuItem";
            this.setColourToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.setColourToolStripMenuItem.Text = "Set colour...";
            this.setColourToolStripMenuItem.Click += new System.EventHandler(this.setColourToolStripMenuItem_Click);
            // 
            // TimeSeriesGraphPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TimeSeriesGraphPanel";
            this.Size = new System.Drawing.Size(660, 255);
            ((System.ComponentModel.ISupportInitialize)(this.picGraphBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelControls.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBoxCoordinates.ResumeLayout(false);
            this.groupBoxCoordinates.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.panelGraph.ResumeLayout(false);
            this.menuSetColour.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listOfTimeSeries;
        private System.Windows.Forms.PictureBox picGraphBox;
        private System.Windows.Forms.HScrollBar graphScrollBar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelGraph;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.ContextMenuStrip menuSetColour;
        private System.Windows.Forms.ToolStripMenuItem setColourToolStripMenuItem;
        private System.Windows.Forms.Button btnShowData;
        private System.Windows.Forms.Label lblYCoordinate;
        private System.Windows.Forms.Label lblXCoordinate;
        private System.Windows.Forms.TextBox txtGraphUnits;
        private System.Windows.Forms.GroupBox groupBoxCoordinates;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Label lblZoomAmount;
        private System.Windows.Forms.ToolTip toolTipNameTooLong;
    }
}
