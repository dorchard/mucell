namespace MuCell.View
{
    partial class TimeSeriesEditorPanelUI
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
            this.listOfTimeSeries = new System.Windows.Forms.ListBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtFormula = new System.Windows.Forms.TextBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btnAddTimeSeries = new System.Windows.Forms.Button();
            this.listOfContextCellDefs = new System.Windows.Forms.ListBox();
            this.listOfContextGroups = new System.Windows.Forms.ListBox();
            this.listOfContextSpecies = new System.Windows.Forms.ListBox();
            this.btnClearSelection = new System.Windows.Forms.Button();
            this.btnAddToFormula = new System.Windows.Forms.Button();
            this.btnEvaluateFormula = new System.Windows.Forms.Button();
            this.btnRemoveTimeSeries = new System.Windows.Forms.Button();
            this.groupBoxFormula = new System.Windows.Forms.GroupBox();
            this.txtFormulaHelper = new System.Windows.Forms.TextBox();
            this.btnDivide = new System.Windows.Forms.Button();
            this.btnMultiply = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.lblAvailableSpecies = new System.Windows.Forms.Label();
            this.lblFormula = new System.Windows.Forms.Label();
            this.lblAvailableGroups = new System.Windows.Forms.Label();
            this.lblAvailableCellDefs = new System.Windows.Forms.Label();
            this.lblListOfTimeSeries = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblTimeInterval = new System.Windows.Forms.Label();
            this.groupBoxCurrent = new System.Windows.Forms.GroupBox();
            this.txtTimeIntervalHelper = new System.Windows.Forms.TextBox();
            this.lblTimeInterval2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBoxFormula.SuspendLayout();
            this.groupBoxCurrent.SuspendLayout();
            this.SuspendLayout();
            // 
            // listOfTimeSeries
            // 
            this.listOfTimeSeries.FormattingEnabled = true;
            this.listOfTimeSeries.Location = new System.Drawing.Point(6, 22);
            this.listOfTimeSeries.Name = "listOfTimeSeries";
            this.listOfTimeSeries.Size = new System.Drawing.Size(248, 134);
            this.listOfTimeSeries.TabIndex = 0;
            this.listOfTimeSeries.SelectedValueChanged += new System.EventHandler(this.listOfTimeSeries_SelectedValueChanged);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(47, 19);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(126, 20);
            this.txtName.TabIndex = 1;
            this.txtName.Validating += new System.ComponentModel.CancelEventHandler(this.txtName_Validating);
            // 
            // txtFormula
            // 
            this.txtFormula.Location = new System.Drawing.Point(6, 32);
            this.txtFormula.Name = "txtFormula";
            this.txtFormula.Size = new System.Drawing.Size(291, 20);
            this.txtFormula.TabIndex = 2;
            this.txtFormula.TextChanged += new System.EventHandler(this.txtFormula_TextChanged);
            this.txtFormula.Click += new System.EventHandler(this.txtFormula_Click);
            this.txtFormula.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtFormula_KeyUp);
            this.txtFormula.Validating += new System.ComponentModel.CancelEventHandler(this.txtFormula_Validating);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(83, 45);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(49, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // btnAddTimeSeries
            // 
            this.btnAddTimeSeries.Location = new System.Drawing.Point(98, 162);
            this.btnAddTimeSeries.Name = "btnAddTimeSeries";
            this.btnAddTimeSeries.Size = new System.Drawing.Size(75, 23);
            this.btnAddTimeSeries.TabIndex = 4;
            this.btnAddTimeSeries.Text = "Add new";
            this.btnAddTimeSeries.UseVisualStyleBackColor = true;
            this.btnAddTimeSeries.Click += new System.EventHandler(this.btnAddTimeSeries_Click);
            // 
            // listOfContextCellDefs
            // 
            this.listOfContextCellDefs.FormattingEnabled = true;
            this.listOfContextCellDefs.Location = new System.Drawing.Point(6, 88);
            this.listOfContextCellDefs.Name = "listOfContextCellDefs";
            this.listOfContextCellDefs.Size = new System.Drawing.Size(120, 95);
            this.listOfContextCellDefs.TabIndex = 5;
            this.listOfContextCellDefs.SelectedIndexChanged += new System.EventHandler(this.listOfContextCellDefs_SelectedIndexChanged);
            // 
            // listOfContextGroups
            // 
            this.listOfContextGroups.FormattingEnabled = true;
            this.listOfContextGroups.Location = new System.Drawing.Point(132, 88);
            this.listOfContextGroups.Name = "listOfContextGroups";
            this.listOfContextGroups.Size = new System.Drawing.Size(120, 95);
            this.listOfContextGroups.TabIndex = 6;
            this.listOfContextGroups.SelectedIndexChanged += new System.EventHandler(this.listOfContextGroups_SelectedIndexChanged);
            // 
            // listOfContextSpecies
            // 
            this.listOfContextSpecies.FormattingEnabled = true;
            this.listOfContextSpecies.Location = new System.Drawing.Point(258, 88);
            this.listOfContextSpecies.Name = "listOfContextSpecies";
            this.listOfContextSpecies.Size = new System.Drawing.Size(120, 95);
            this.listOfContextSpecies.TabIndex = 7;
            this.listOfContextSpecies.SelectedIndexChanged += new System.EventHandler(this.listOfContextSpecies_SelectedIndexChanged);
            // 
            // btnClearSelection
            // 
            this.btnClearSelection.Location = new System.Drawing.Point(102, 235);
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.Size = new System.Drawing.Size(91, 23);
            this.btnClearSelection.TabIndex = 10;
            this.btnClearSelection.Text = "Clear selection";
            this.btnClearSelection.UseVisualStyleBackColor = true;
            this.btnClearSelection.Click += new System.EventHandler(this.btnClearSelection_Click);
            // 
            // btnAddToFormula
            // 
            this.btnAddToFormula.Location = new System.Drawing.Point(3, 235);
            this.btnAddToFormula.Name = "btnAddToFormula";
            this.btnAddToFormula.Size = new System.Drawing.Size(93, 23);
            this.btnAddToFormula.TabIndex = 11;
            this.btnAddToFormula.Text = "Add to formula";
            this.btnAddToFormula.UseVisualStyleBackColor = true;
            this.btnAddToFormula.Click += new System.EventHandler(this.btnAddToFormula_Click);
            // 
            // btnEvaluateFormula
            // 
            this.btnEvaluateFormula.Location = new System.Drawing.Point(303, 30);
            this.btnEvaluateFormula.Name = "btnEvaluateFormula";
            this.btnEvaluateFormula.Size = new System.Drawing.Size(75, 23);
            this.btnEvaluateFormula.TabIndex = 12;
            this.btnEvaluateFormula.Text = "Validate";
            this.btnEvaluateFormula.UseVisualStyleBackColor = true;
            this.btnEvaluateFormula.Click += new System.EventHandler(this.btnEvaluateFormula_Click);
            // 
            // btnRemoveTimeSeries
            // 
            this.btnRemoveTimeSeries.Location = new System.Drawing.Point(179, 162);
            this.btnRemoveTimeSeries.Name = "btnRemoveTimeSeries";
            this.btnRemoveTimeSeries.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveTimeSeries.TabIndex = 13;
            this.btnRemoveTimeSeries.Text = "Remove";
            this.btnRemoveTimeSeries.UseVisualStyleBackColor = true;
            this.btnRemoveTimeSeries.Click += new System.EventHandler(this.btnRemoveTimeSeries_Click);
            // 
            // groupBoxFormula
            // 
            this.groupBoxFormula.Controls.Add(this.txtFormulaHelper);
            this.groupBoxFormula.Controls.Add(this.btnDivide);
            this.groupBoxFormula.Controls.Add(this.btnMultiply);
            this.groupBoxFormula.Controls.Add(this.btnMinus);
            this.groupBoxFormula.Controls.Add(this.btnPlus);
            this.groupBoxFormula.Controls.Add(this.lblAvailableSpecies);
            this.groupBoxFormula.Controls.Add(this.lblFormula);
            this.groupBoxFormula.Controls.Add(this.lblAvailableGroups);
            this.groupBoxFormula.Controls.Add(this.lblAvailableCellDefs);
            this.groupBoxFormula.Controls.Add(this.btnClearSelection);
            this.groupBoxFormula.Controls.Add(this.btnEvaluateFormula);
            this.groupBoxFormula.Controls.Add(this.btnAddToFormula);
            this.groupBoxFormula.Controls.Add(this.listOfContextCellDefs);
            this.groupBoxFormula.Controls.Add(this.listOfContextGroups);
            this.groupBoxFormula.Controls.Add(this.txtFormula);
            this.groupBoxFormula.Controls.Add(this.listOfContextSpecies);
            this.groupBoxFormula.Location = new System.Drawing.Point(6, 191);
            this.groupBoxFormula.Name = "groupBoxFormula";
            this.groupBoxFormula.Size = new System.Drawing.Size(386, 266);
            this.groupBoxFormula.TabIndex = 14;
            this.groupBoxFormula.TabStop = false;
            this.groupBoxFormula.Text = "Formula editor";
            // 
            // txtFormulaHelper
            // 
            this.txtFormulaHelper.BackColor = System.Drawing.Color.White;
            this.txtFormulaHelper.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFormulaHelper.Location = new System.Drawing.Point(6, 189);
            this.txtFormulaHelper.Multiline = true;
            this.txtFormulaHelper.Name = "txtFormulaHelper";
            this.txtFormulaHelper.ReadOnly = true;
            this.txtFormulaHelper.Size = new System.Drawing.Size(372, 40);
            this.txtFormulaHelper.TabIndex = 22;
            this.txtFormulaHelper.Text = "formula helper";
            // 
            // btnDivide
            // 
            this.btnDivide.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDivide.Location = new System.Drawing.Point(355, 235);
            this.btnDivide.Name = "btnDivide";
            this.btnDivide.Size = new System.Drawing.Size(23, 23);
            this.btnDivide.TabIndex = 21;
            this.btnDivide.Text = "/";
            this.btnDivide.UseVisualStyleBackColor = true;
            this.btnDivide.Click += new System.EventHandler(this.btnDivide_Click);
            // 
            // btnMultiply
            // 
            this.btnMultiply.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMultiply.Location = new System.Drawing.Point(325, 235);
            this.btnMultiply.Name = "btnMultiply";
            this.btnMultiply.Size = new System.Drawing.Size(23, 23);
            this.btnMultiply.TabIndex = 20;
            this.btnMultiply.Text = "*";
            this.btnMultiply.UseVisualStyleBackColor = true;
            this.btnMultiply.Click += new System.EventHandler(this.btnMultiply_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinus.Location = new System.Drawing.Point(296, 235);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(23, 23);
            this.btnMinus.TabIndex = 19;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlus.Location = new System.Drawing.Point(266, 235);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(23, 23);
            this.btnPlus.TabIndex = 18;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // lblAvailableSpecies
            // 
            this.lblAvailableSpecies.AutoSize = true;
            this.lblAvailableSpecies.Location = new System.Drawing.Point(255, 72);
            this.lblAvailableSpecies.Name = "lblAvailableSpecies";
            this.lblAvailableSpecies.Size = new System.Drawing.Size(48, 13);
            this.lblAvailableSpecies.TabIndex = 2;
            this.lblAvailableSpecies.Text = "Species:";
            // 
            // lblFormula
            // 
            this.lblFormula.AutoSize = true;
            this.lblFormula.Location = new System.Drawing.Point(6, 16);
            this.lblFormula.Name = "lblFormula";
            this.lblFormula.Size = new System.Drawing.Size(173, 13);
            this.lblFormula.TabIndex = 17;
            this.lblFormula.Text = "Record the value of this expression";
            // 
            // lblAvailableGroups
            // 
            this.lblAvailableGroups.AutoSize = true;
            this.lblAvailableGroups.Location = new System.Drawing.Point(129, 72);
            this.lblAvailableGroups.Name = "lblAvailableGroups";
            this.lblAvailableGroups.Size = new System.Drawing.Size(44, 13);
            this.lblAvailableGroups.TabIndex = 1;
            this.lblAvailableGroups.Text = "Groups:";
            // 
            // lblAvailableCellDefs
            // 
            this.lblAvailableCellDefs.AutoSize = true;
            this.lblAvailableCellDefs.Location = new System.Drawing.Point(3, 72);
            this.lblAvailableCellDefs.Name = "lblAvailableCellDefs";
            this.lblAvailableCellDefs.Size = new System.Drawing.Size(77, 13);
            this.lblAvailableCellDefs.TabIndex = 0;
            this.lblAvailableCellDefs.Text = "Cell definitions:";
            // 
            // lblListOfTimeSeries
            // 
            this.lblListOfTimeSeries.AutoSize = true;
            this.lblListOfTimeSeries.Location = new System.Drawing.Point(6, 6);
            this.lblListOfTimeSeries.Name = "lblListOfTimeSeries";
            this.lblListOfTimeSeries.Size = new System.Drawing.Size(207, 13);
            this.lblListOfTimeSeries.TabIndex = 15;
            this.lblListOfTimeSeries.Text = "Time series to record during the simulation:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(6, 22);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 16;
            this.lblName.Text = "Name";
            // 
            // lblTimeInterval
            // 
            this.lblTimeInterval.AutoSize = true;
            this.lblTimeInterval.Location = new System.Drawing.Point(6, 47);
            this.lblTimeInterval.Name = "lblTimeInterval";
            this.lblTimeInterval.Size = new System.Drawing.Size(71, 13);
            this.lblTimeInterval.TabIndex = 18;
            this.lblTimeInterval.Text = "Record every";
            // 
            // groupBoxCurrent
            // 
            this.groupBoxCurrent.Controls.Add(this.txtTimeIntervalHelper);
            this.groupBoxCurrent.Controls.Add(this.lblTimeInterval2);
            this.groupBoxCurrent.Controls.Add(this.lblName);
            this.groupBoxCurrent.Controls.Add(this.lblTimeInterval);
            this.groupBoxCurrent.Controls.Add(this.txtName);
            this.groupBoxCurrent.Controls.Add(this.numericUpDown1);
            this.groupBoxCurrent.Location = new System.Drawing.Point(260, 6);
            this.groupBoxCurrent.Name = "groupBoxCurrent";
            this.groupBoxCurrent.Size = new System.Drawing.Size(182, 149);
            this.groupBoxCurrent.TabIndex = 19;
            this.groupBoxCurrent.TabStop = false;
            this.groupBoxCurrent.Text = "Edit settings";
            // 
            // txtTimeIntervalHelper
            // 
            this.txtTimeIntervalHelper.BackColor = System.Drawing.Color.White;
            this.txtTimeIntervalHelper.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTimeIntervalHelper.Location = new System.Drawing.Point(6, 71);
            this.txtTimeIntervalHelper.Multiline = true;
            this.txtTimeIntervalHelper.Name = "txtTimeIntervalHelper";
            this.txtTimeIntervalHelper.ReadOnly = true;
            this.txtTimeIntervalHelper.Size = new System.Drawing.Size(167, 72);
            this.txtTimeIntervalHelper.TabIndex = 20;
            this.txtTimeIntervalHelper.Text = "Time interval helper";
            // 
            // lblTimeInterval2
            // 
            this.lblTimeInterval2.AutoSize = true;
            this.lblTimeInterval2.Location = new System.Drawing.Point(138, 47);
            this.lblTimeInterval2.Name = "lblTimeInterval2";
            this.lblTimeInterval2.Size = new System.Drawing.Size(35, 13);
            this.lblTimeInterval2.TabIndex = 19;
            this.lblTimeInterval2.Text = "steps.";
            // 
            // TimeSeriesEditorPanelUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxCurrent);
            this.Controls.Add(this.lblListOfTimeSeries);
            this.Controls.Add(this.groupBoxFormula);
            this.Controls.Add(this.btnRemoveTimeSeries);
            this.Controls.Add(this.btnAddTimeSeries);
            this.Controls.Add(this.listOfTimeSeries);
            this.Name = "TimeSeriesEditorPanelUI";
            this.Size = new System.Drawing.Size(600, 498);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBoxFormula.ResumeLayout(false);
            this.groupBoxFormula.PerformLayout();
            this.groupBoxCurrent.ResumeLayout(false);
            this.groupBoxCurrent.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listOfTimeSeries;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtFormula;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btnAddTimeSeries;
        private System.Windows.Forms.ListBox listOfContextCellDefs;
        private System.Windows.Forms.ListBox listOfContextGroups;
        private System.Windows.Forms.ListBox listOfContextSpecies;
        private System.Windows.Forms.Button btnClearSelection;
        private System.Windows.Forms.Button btnAddToFormula;
        private System.Windows.Forms.Button btnEvaluateFormula;
        private System.Windows.Forms.Button btnRemoveTimeSeries;
        private System.Windows.Forms.GroupBox groupBoxFormula;
        private System.Windows.Forms.Label lblAvailableSpecies;
        private System.Windows.Forms.Label lblAvailableGroups;
        private System.Windows.Forms.Label lblAvailableCellDefs;
        private System.Windows.Forms.Label lblListOfTimeSeries;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblFormula;
        private System.Windows.Forms.Label lblTimeInterval;
        private System.Windows.Forms.GroupBox groupBoxCurrent;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Label lblTimeInterval2;
        private System.Windows.Forms.Button btnDivide;
        private System.Windows.Forms.Button btnMultiply;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.TextBox txtTimeIntervalHelper;
        private System.Windows.Forms.TextBox txtFormulaHelper;

    }
}
