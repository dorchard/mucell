using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MuCell.Controller;
using MuCell.View.OpenGL;
using MuCell.Model;
using OpenTK.OpenGL;
using OpenTK.OpenGL.Enums;
using MuCell.View.OpenGL;

namespace MuCell.View
{

    /// <summary>
    /// This class extends the Panel class and implements the ISpatialConfigurationPanelUI
    /// interface, providing a concrete Panel for editing the positions of cells in the 
    /// inital state of the simulation.
    /// 
    /// This panel is a child of the SimulationEditorPanelUI panel.
    /// 
    /// </summary>
    /// 



    class SpatialConfigurationPanelUI
        : UserControl, ISpatialConfigurationPanelUI
    {
        private Label label1;
        private Label label2;
        private Label label3;
        private TabPage tabPage2;
        private TabPage tabPage1;
        private Button addCellsButton;
        private ComboBox CellTypeComboBox;
        private Label label5;
        private Label label4;
        private NumericUpDown PopulationNumericUpDown;
        private TabPage tabPage3;
        private Button button5;
        private Label label10;
        private Label label9;
        private TabControl tabControl1;
        private OpenGLCellPlacementPanel OpenGLPanelXY;
        private OpenGLCellPlacementPanel OpenGLPanelXZ;
        private OpenGLCellPlacementPanel OpenGLPanel3D;

        private SpatialConfigurationPanelController controller;
        private Label label12;
        private ComboBox DistributionComboBox;
        private NumericUpDown RadiusNumericUpDown;
        private Label label13;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel openGLViewsTableLayoutPanel;
        private ListBox groupListBox;


        private bool gogo = false;

        private bool cellsGogo = false;

        //initial mouse position, used for scrolling
        private Point initialPointerPos;

        /*
         * where or not to animate (refresh at regular time intervals)
         * 0 => not animated
         * 1 => animated
         * -1 => complete one more frame of animation, then stop animation
         */
        private int openGLPanelXYAnimationState;
        private int openGLPanelXZAnimationState;
        private GroupBox groupBox1;
        private RadioButton cylinderRadioButton;
        private RadioButton cuboidRadioButton;
        private RadioButton sphereRadioButton;
        private NumericUpDown boundaryHeightNumericUpDown;
        private NumericUpDown boundaryWidthNumericUpDown;
        private Label label8;
        private NumericUpDown boundaryRadiusNumericUpDown;
        private Label label17;
        private NumericUpDown boundaryDepthNumericUpDown;
        private Button removeButton;
        private Button addGroupButton;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel2;
        private Label cursorPositionLabel;
        private GroupBox groupBox3;

        //whether or not the views are currently being dragged (scrolled) by the mouse
        private int openGLPanelViewDraggingState;
        private GroupBox nutrientsGroupBox;
        private Button removeNutrientFieldButton;
        private Button addNutrientFieldButton;
        private ListBox nutrientFieldListBox;
        private GroupBox fieldPropertiesGroupBox;
        private NumericUpDown initialQuantityNumericUpDown;
        private Label label7;
        private NumericUpDown diffusionRateNumericUpDown;
        private Label label11;
        private Label label14;
        private ComboBox initialDistributionComboBox;
        private Label label15;
        private NumericUpDown nutrientsDistributionRadiusNumericUpDown;
        private Label cellCountLabel;
        private Label label6;
        private NumericUpDown resolutoinNumericUpDown;
        private TextBox resolutionHelper;
        private Button viewNutrient;

        //reference to the spatialViewState object for the currently selected simulation
        private SpatialViewState spatialViewState;

        #region IControllable<SpatialConfigurationPanelController> Members

        public void setController(SpatialConfigurationPanelController controller)
        {
            this.controller = controller;
        }

        #endregion

        public SpatialConfigurationPanelUI()
        {

            this.Dock = DockStyle.Fill;
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.OpenGLViewsTableLayoutPanel_MouseWheel);


            this.initialPointerPos = new Point(0, 0);
            this.openGLPanelXYAnimationState = 0;
            this.openGLPanelXZAnimationState = 0;
            this.openGLPanelViewDraggingState = 0;

            InitializeComponent();


        }


        private void InitOpenGLPanels()
        {
            spatialViewState = new SpatialViewState();

            this.OpenGLPanelXY = new OpenGLCellPlacementPanel(spatialViewState, Views.XY);
            this.OpenGLPanelXZ = new OpenGLCellPlacementPanel(spatialViewState, Views.XZ);
            this.OpenGLPanel3D = new OpenGLCellPlacementPanel(spatialViewState, Views.ThreeD);
        }

        private void InitializeComponent()
        {
            spatialViewState = new SpatialViewState();

            this.OpenGLPanelXY = new OpenGLCellPlacementPanel(spatialViewState, Views.XY);
            this.OpenGLPanelXZ = new OpenGLCellPlacementPanel(spatialViewState, Views.XZ);
            this.OpenGLPanel3D = new OpenGLCellPlacementPanel(spatialViewState, Views.ThreeD);

            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.fieldPropertiesGroupBox = new System.Windows.Forms.GroupBox();
            this.viewNutrient = new System.Windows.Forms.Button();
            this.resolutionHelper = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.resolutoinNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.nutrientsDistributionRadiusNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.initialDistributionComboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.diffusionRateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.initialQuantityNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.nutrientsGroupBox = new System.Windows.Forms.GroupBox();
            this.removeNutrientFieldButton = new System.Windows.Forms.Button();
            this.addNutrientFieldButton = new System.Windows.Forms.Button();
            this.nutrientFieldListBox = new System.Windows.Forms.ListBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cursorPositionLabel = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.RadiusNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.DistributionComboBox = new System.Windows.Forms.ComboBox();
            this.addCellsButton = new System.Windows.Forms.Button();
            this.CellTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.PopulationNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cellCountLabel = new System.Windows.Forms.Label();
            this.removeButton = new System.Windows.Forms.Button();
            this.addGroupButton = new System.Windows.Forms.Button();
            this.groupListBox = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.boundaryRadiusNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.boundaryDepthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.boundaryHeightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.boundaryWidthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.sphereRadioButton = new System.Windows.Forms.RadioButton();
            this.cylinderRadioButton = new System.Windows.Forms.RadioButton();
            this.cuboidRadioButton = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.openGLViewsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tabPage2.SuspendLayout();
            this.fieldPropertiesGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resolutoinNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nutrientsDistributionRadiusNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffusionRateNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialQuantityNumericUpDown)).BeginInit();
            this.nutrientsGroupBox.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RadiusNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopulationNumericUpDown)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boundaryRadiusNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boundaryDepthNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boundaryHeightNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boundaryWidthNumericUpDown)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.openGLViewsTableLayoutPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenGLPanelXY
            // 
            this.OpenGLPanelXY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OpenGLPanelXY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OpenGLPanelXY.Location = new System.Drawing.Point(3, 19);
            this.OpenGLPanelXY.Name = "OpenGLPanelXY";
            this.OpenGLPanelXY.TabIndex = 7;
            this.OpenGLPanelXY.View = MuCell.View.OpenGL.Views.XY;
            this.OpenGLPanelXY.ChildMouseDownEvent += new System.Windows.Forms.MouseEventHandler(this.OpenGLPanelXY_ChildMouseDownEvent);
            this.OpenGLPanelXY.Resize += new System.EventHandler(this.OpenGLPanelXY_Resize);
            this.OpenGLPanelXY.ChildMouseUpEvent += new System.Windows.Forms.MouseEventHandler(this.OpenGLPanelXY_ChildMouseUpEvent);
            // 
            // OpenGLPanelXZ
            // 
            this.OpenGLPanelXZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OpenGLPanelXZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OpenGLPanelXZ.Location = new System.Drawing.Point(237, 19);
            this.OpenGLPanelXZ.Name = "OpenGLPanelXZ";
            this.OpenGLPanelXZ.Size = new System.Drawing.Size(228, 208);
            this.OpenGLPanelXZ.TabIndex = 8;
            this.OpenGLPanelXZ.View = MuCell.View.OpenGL.Views.XZ;
            this.OpenGLPanelXZ.ChildMouseDownEvent += new System.Windows.Forms.MouseEventHandler(this.OpenGLPanelXZ_ChildMouseDownEvent);
            this.OpenGLPanelXZ.Resize += new System.EventHandler(this.OpenGLPanelXZ_Resize);
            this.OpenGLPanelXZ.ChildMouseUpEvent += new System.Windows.Forms.MouseEventHandler(this.OpenGLPanelXZ_ChildMouseUpEvent);
            // 
            // OpenGLPanel3D
            // 
            this.OpenGLPanel3D.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OpenGLPanel3D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OpenGLPanel3D.Location = new System.Drawing.Point(471, 19);
            this.OpenGLPanel3D.Name = "OpenGLPanel3D";
            this.OpenGLPanel3D.Size = new System.Drawing.Size(228, 208);
            this.OpenGLPanel3D.TabIndex = 9;
            this.OpenGLPanel3D.View = MuCell.View.OpenGL.Views.ThreeD;
            this.OpenGLPanel3D.Resize += new System.EventHandler(this.OpenGLPanel3D_Resize);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Side (X-Y)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(471, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "3D view";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(237, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Top-down  (X-Z)";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.fieldPropertiesGroupBox);
            this.tabPage2.Controls.Add(this.nutrientsGroupBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(686, 171);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Setup Concentration Fields";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // fieldPropertiesGroupBox
            // 
            this.fieldPropertiesGroupBox.Controls.Add(this.viewNutrient);
            this.fieldPropertiesGroupBox.Controls.Add(this.resolutionHelper);
            this.fieldPropertiesGroupBox.Controls.Add(this.label6);
            this.fieldPropertiesGroupBox.Controls.Add(this.resolutoinNumericUpDown);
            this.fieldPropertiesGroupBox.Controls.Add(this.nutrientsDistributionRadiusNumericUpDown);
            this.fieldPropertiesGroupBox.Controls.Add(this.label15);
            this.fieldPropertiesGroupBox.Controls.Add(this.label14);
            this.fieldPropertiesGroupBox.Controls.Add(this.initialDistributionComboBox);
            this.fieldPropertiesGroupBox.Controls.Add(this.label11);
            this.fieldPropertiesGroupBox.Controls.Add(this.diffusionRateNumericUpDown);
            this.fieldPropertiesGroupBox.Controls.Add(this.label7);
            this.fieldPropertiesGroupBox.Controls.Add(this.initialQuantityNumericUpDown);
            this.fieldPropertiesGroupBox.Location = new System.Drawing.Point(205, 6);
            this.fieldPropertiesGroupBox.Name = "fieldPropertiesGroupBox";
            this.fieldPropertiesGroupBox.Size = new System.Drawing.Size(376, 162);
            this.fieldPropertiesGroupBox.TabIndex = 28;
            this.fieldPropertiesGroupBox.TabStop = false;
            this.fieldPropertiesGroupBox.Text = "Nutrient Field Properties";
            // 
            // viewNutrient
            // 
            this.viewNutrient.Location = new System.Drawing.Point(324, 11);
            this.viewNutrient.Name = "viewNutrient";
            this.viewNutrient.Size = new System.Drawing.Size(43, 25);
            this.viewNutrient.TabIndex = 25;
            this.viewNutrient.Text = "View";
            this.viewNutrient.UseVisualStyleBackColor = true;
            this.viewNutrient.Click += new System.EventHandler(this.viewNutrient_Click);
            // 
            // resolutionHelper
            // 
            this.resolutionHelper.BackColor = System.Drawing.Color.White;
            this.resolutionHelper.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resolutionHelper.Location = new System.Drawing.Point(9, 94);
            this.resolutionHelper.Margin = new System.Windows.Forms.Padding(6);
            this.resolutionHelper.Multiline = true;
            this.resolutionHelper.Name = "resolutionHelper";
            this.resolutionHelper.ReadOnly = true;
            this.resolutionHelper.Size = new System.Drawing.Size(358, 61);
            this.resolutionHelper.TabIndex = 41;
            this.resolutionHelper.Text = "Resolution helper\r\n";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(219, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 40;
            this.label6.Text = "Field Resolution:";
            // 
            // resolutoinNumericUpDown
            // 
            this.resolutoinNumericUpDown.DecimalPlaces = 3;
            this.resolutoinNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.resolutoinNumericUpDown.Location = new System.Drawing.Point(313, 66);
            this.resolutoinNumericUpDown.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.resolutoinNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.resolutoinNumericUpDown.Name = "resolutoinNumericUpDown";
            this.resolutoinNumericUpDown.Size = new System.Drawing.Size(54, 20);
            this.resolutoinNumericUpDown.TabIndex = 39;
            this.resolutoinNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.resolutoinNumericUpDown.ValueChanged += new System.EventHandler(this.resolutoinNumericUpDown_ValueChanged);
            // 
            // nutrientsDistributionRadiusNumericUpDown
            // 
            this.nutrientsDistributionRadiusNumericUpDown.DecimalPlaces = 2;
            this.nutrientsDistributionRadiusNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nutrientsDistributionRadiusNumericUpDown.Location = new System.Drawing.Point(313, 40);
            this.nutrientsDistributionRadiusNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nutrientsDistributionRadiusNumericUpDown.Name = "nutrientsDistributionRadiusNumericUpDown";
            this.nutrientsDistributionRadiusNumericUpDown.Size = new System.Drawing.Size(54, 20);
            this.nutrientsDistributionRadiusNumericUpDown.TabIndex = 37;
            this.nutrientsDistributionRadiusNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nutrientsDistributionRadiusNumericUpDown.ValueChanged += new System.EventHandler(this.nutrientsDistributionRadiusNumericUpDown_ValueChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(184, 42);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(123, 13);
            this.label15.TabIndex = 36;
            this.label15.Text = "Distribution Radius (mm):";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(89, 13);
            this.label14.TabIndex = 35;
            this.label14.Text = "Initial Distribution:";
            // 
            // initialDistributionComboBox
            // 
            this.initialDistributionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.initialDistributionComboBox.Location = new System.Drawing.Point(114, 14);
            this.initialDistributionComboBox.Name = "initialDistributionComboBox";
            this.initialDistributionComboBox.Size = new System.Drawing.Size(201, 21);
            this.initialDistributionComboBox.TabIndex = 38;
            this.initialDistributionComboBox.SelectedIndexChanged += new System.EventHandler(this.initialDistributionComboBox_SelectedIndexChanged);
            this.initialDistributionComboBox.SelectedValueChanged += new System.EventHandler(this.initialDistributionComboBox_SelectedValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(31, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "Diffusion Rate:";
            // 
            // diffusionRateNumericUpDown
            // 
            this.diffusionRateNumericUpDown.DecimalPlaces = 3;
            this.diffusionRateNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.diffusionRateNumericUpDown.Location = new System.Drawing.Point(114, 66);
            this.diffusionRateNumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.diffusionRateNumericUpDown.Name = "diffusionRateNumericUpDown";
            this.diffusionRateNumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.diffusionRateNumericUpDown.TabIndex = 31;
            this.diffusionRateNumericUpDown.ValueChanged += new System.EventHandler(this.diffusionRateNumericUpDown_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(2, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Initial Quatity (moles):";
            // 
            // initialQuantityNumericUpDown
            // 
            this.initialQuantityNumericUpDown.DecimalPlaces = 3;
            this.initialQuantityNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.initialQuantityNumericUpDown.Location = new System.Drawing.Point(114, 40);
            this.initialQuantityNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.initialQuantityNumericUpDown.Name = "initialQuantityNumericUpDown";
            this.initialQuantityNumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.initialQuantityNumericUpDown.TabIndex = 29;
            this.initialQuantityNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.initialQuantityNumericUpDown.ValueChanged += new System.EventHandler(this.initialQuantityNumericUpDown_ValueChanged);
            // 
            // nutrientsGroupBox
            // 
            this.nutrientsGroupBox.Controls.Add(this.removeNutrientFieldButton);
            this.nutrientsGroupBox.Controls.Add(this.addNutrientFieldButton);
            this.nutrientsGroupBox.Controls.Add(this.nutrientFieldListBox);
            this.nutrientsGroupBox.Location = new System.Drawing.Point(6, 6);
            this.nutrientsGroupBox.Name = "nutrientsGroupBox";
            this.nutrientsGroupBox.Size = new System.Drawing.Size(193, 162);
            this.nutrientsGroupBox.TabIndex = 26;
            this.nutrientsGroupBox.TabStop = false;
            this.nutrientsGroupBox.Text = "Nutrient Fields";
            // 
            // removeNutrientFieldButton
            // 
            this.removeNutrientFieldButton.Location = new System.Drawing.Point(122, 113);
            this.removeNutrientFieldButton.Name = "removeNutrientFieldButton";
            this.removeNutrientFieldButton.Size = new System.Drawing.Size(61, 27);
            this.removeNutrientFieldButton.TabIndex = 24;
            this.removeNutrientFieldButton.Text = "Remove";
            this.removeNutrientFieldButton.UseVisualStyleBackColor = true;
            this.removeNutrientFieldButton.Click += new System.EventHandler(this.removeNutrientFieldButton_Click);
            // 
            // addNutrientFieldButton
            // 
            this.addNutrientFieldButton.Location = new System.Drawing.Point(122, 80);
            this.addNutrientFieldButton.Name = "addNutrientFieldButton";
            this.addNutrientFieldButton.Size = new System.Drawing.Size(61, 27);
            this.addNutrientFieldButton.TabIndex = 23;
            this.addNutrientFieldButton.Text = "Add";
            this.addNutrientFieldButton.UseVisualStyleBackColor = true;
            this.addNutrientFieldButton.Click += new System.EventHandler(this.addNutrientFieldButton_Click);
            // 
            // nutrientFieldListBox
            // 
            this.nutrientFieldListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.nutrientFieldListBox.FormattingEnabled = true;
            this.nutrientFieldListBox.Location = new System.Drawing.Point(6, 19);
            this.nutrientFieldListBox.Name = "nutrientFieldListBox";
            this.nutrientFieldListBox.Size = new System.Drawing.Size(110, 121);
            this.nutrientFieldListBox.TabIndex = 20;
            this.nutrientFieldListBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.nutrientFieldListBox_MouseUp);
            this.nutrientFieldListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.nutrientFieldListBox_MouseDoubleClick);
            this.nutrientFieldListBox.SelectedValueChanged += new System.EventHandler(this.nutrientFieldListBox_SelectedValueChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(686, 171);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Setup Cells";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cursorPositionLabel);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.RadiusNumericUpDown);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.DistributionComboBox);
            this.groupBox3.Controls.Add(this.addCellsButton);
            this.groupBox3.Controls.Add(this.CellTypeComboBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.PopulationNumericUpDown);
            this.groupBox3.Location = new System.Drawing.Point(205, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(376, 162);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Cluster generation";
            // 
            // cursorPositionLabel
            // 
            this.cursorPositionLabel.AutoSize = true;
            this.cursorPositionLabel.BackColor = System.Drawing.Color.Gainsboro;
            this.cursorPositionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cursorPositionLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.cursorPositionLabel.Location = new System.Drawing.Point(6, 127);
            this.cursorPositionLabel.Name = "cursorPositionLabel";
            this.cursorPositionLabel.Size = new System.Drawing.Size(167, 13);
            this.cursorPositionLabel.TabIndex = 0;
            this.cursorPositionLabel.Text = "    Crosshair : (0.00, 0.00, 0.00)     ";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(4, 101);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "Radius (mm):";
            // 
            // RadiusNumericUpDown
            // 
            this.RadiusNumericUpDown.DecimalPlaces = 2;
            this.RadiusNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.RadiusNumericUpDown.Location = new System.Drawing.Point(83, 99);
            this.RadiusNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.RadiusNumericUpDown.Name = "RadiusNumericUpDown";
            this.RadiusNumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.RadiusNumericUpDown.TabIndex = 15;
            this.RadiusNumericUpDown.Value = new decimal(new int[] {
            125,
            0,
            0,
            65536});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 51);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Distribution:";
            // 
            // DistributionComboBox
            // 
            this.DistributionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DistributionComboBox.FormattingEnabled = true;
            this.DistributionComboBox.Location = new System.Drawing.Point(83, 47);
            this.DistributionComboBox.Name = "DistributionComboBox";
            this.DistributionComboBox.Size = new System.Drawing.Size(120, 21);
            this.DistributionComboBox.TabIndex = 9;
            // 
            // addCellsButton
            // 
            this.addCellsButton.Location = new System.Drawing.Point(233, 22);
            this.addCellsButton.Name = "addCellsButton";
            this.addCellsButton.Size = new System.Drawing.Size(101, 23);
            this.addCellsButton.TabIndex = 5;
            this.addCellsButton.Text = "Add cells";
            this.addCellsButton.UseVisualStyleBackColor = true;
            this.addCellsButton.Click += new System.EventHandler(this.addCellsButton_Click);
            // 
            // CellTypeComboBox
            // 
            this.CellTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CellTypeComboBox.FormattingEnabled = true;
            this.CellTypeComboBox.Location = new System.Drawing.Point(83, 19);
            this.CellTypeComboBox.Name = "CellTypeComboBox";
            this.CellTypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.CellTypeComboBox.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Count:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Cell Type:";
            // 
            // PopulationNumericUpDown
            // 
            this.PopulationNumericUpDown.Location = new System.Drawing.Point(83, 74);
            this.PopulationNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.PopulationNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PopulationNumericUpDown.Name = "PopulationNumericUpDown";
            this.PopulationNumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.PopulationNumericUpDown.TabIndex = 0;
            this.PopulationNumericUpDown.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cellCountLabel);
            this.groupBox2.Controls.Add(this.removeButton);
            this.groupBox2.Controls.Add(this.addGroupButton);
            this.groupBox2.Controls.Add(this.groupListBox);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(193, 162);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cell Groups";
            // 
            // cellCountLabel
            // 
            this.cellCountLabel.AutoSize = true;
            this.cellCountLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.cellCountLabel.Location = new System.Drawing.Point(6, 146);
            this.cellCountLabel.Name = "cellCountLabel";
            this.cellCountLabel.Size = new System.Drawing.Size(58, 13);
            this.cellCountLabel.TabIndex = 25;
            this.cellCountLabel.Text = "Cell Count:";
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(122, 113);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(61, 27);
            this.removeButton.TabIndex = 24;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // addGroupButton
            // 
            this.addGroupButton.Location = new System.Drawing.Point(122, 80);
            this.addGroupButton.Name = "addGroupButton";
            this.addGroupButton.Size = new System.Drawing.Size(61, 27);
            this.addGroupButton.TabIndex = 23;
            this.addGroupButton.Text = "Add";
            this.addGroupButton.UseVisualStyleBackColor = true;
            this.addGroupButton.Click += new System.EventHandler(this.addGroupButton_Click);
            // 
            // groupListBox
            // 
            this.groupListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupListBox.FormattingEnabled = true;
            this.groupListBox.Location = new System.Drawing.Point(6, 19);
            this.groupListBox.Name = "groupListBox";
            this.groupListBox.Size = new System.Drawing.Size(110, 121);
            this.groupListBox.TabIndex = 20;
            this.groupListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.groupListBox_MouseDoubleClick);
            this.groupListBox.SelectedValueChanged += new System.EventHandler(this.groupListBox_SelectedValueChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.button5);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(686, 171);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Spatial settings";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.boundaryRadiusNumericUpDown);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.boundaryDepthNumericUpDown);
            this.groupBox1.Controls.Add(this.boundaryHeightNumericUpDown);
            this.groupBox1.Controls.Add(this.boundaryWidthNumericUpDown);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.sphereRadioButton);
            this.groupBox1.Controls.Add(this.cylinderRadioButton);
            this.groupBox1.Controls.Add(this.cuboidRadioButton);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(576, 162);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Boundary shape";
            // 
            // boundaryRadiusNumericUpDown
            // 
            this.boundaryRadiusNumericUpDown.Location = new System.Drawing.Point(441, 44);
            this.boundaryRadiusNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.boundaryRadiusNumericUpDown.Name = "boundaryRadiusNumericUpDown";
            this.boundaryRadiusNumericUpDown.Size = new System.Drawing.Size(86, 20);
            this.boundaryRadiusNumericUpDown.TabIndex = 12;
            this.boundaryRadiusNumericUpDown.ValueChanged += new System.EventHandler(this.boundaryRadiusNumericUpDown_ValueChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(367, 46);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(68, 13);
            this.label17.TabIndex = 11;
            this.label17.Text = "Radius (mm):";
            // 
            // boundaryDepthNumericUpDown
            // 
            this.boundaryDepthNumericUpDown.Location = new System.Drawing.Point(245, 103);
            this.boundaryDepthNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.boundaryDepthNumericUpDown.Name = "boundaryDepthNumericUpDown";
            this.boundaryDepthNumericUpDown.Size = new System.Drawing.Size(86, 20);
            this.boundaryDepthNumericUpDown.TabIndex = 10;
            this.boundaryDepthNumericUpDown.ValueChanged += new System.EventHandler(this.boundaryDepthNumericUpDown_ValueChanged);
            // 
            // boundaryHeightNumericUpDown
            // 
            this.boundaryHeightNumericUpDown.Location = new System.Drawing.Point(245, 73);
            this.boundaryHeightNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.boundaryHeightNumericUpDown.Name = "boundaryHeightNumericUpDown";
            this.boundaryHeightNumericUpDown.Size = new System.Drawing.Size(86, 20);
            this.boundaryHeightNumericUpDown.TabIndex = 9;
            this.boundaryHeightNumericUpDown.ValueChanged += new System.EventHandler(this.boundaryHeightNumericUpDown_ValueChanged);
            // 
            // boundaryWidthNumericUpDown
            // 
            this.boundaryWidthNumericUpDown.Location = new System.Drawing.Point(245, 42);
            this.boundaryWidthNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.boundaryWidthNumericUpDown.Name = "boundaryWidthNumericUpDown";
            this.boundaryWidthNumericUpDown.Size = new System.Drawing.Size(86, 20);
            this.boundaryWidthNumericUpDown.TabIndex = 8;
            this.boundaryWidthNumericUpDown.ValueChanged += new System.EventHandler(this.boundaryWidthNumericUpDown_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(165, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Depth Z (mm):";
            // 
            // sphereRadioButton
            // 
            this.sphereRadioButton.AutoSize = true;
            this.sphereRadioButton.Location = new System.Drawing.Point(52, 103);
            this.sphereRadioButton.Name = "sphereRadioButton";
            this.sphereRadioButton.Size = new System.Drawing.Size(59, 17);
            this.sphereRadioButton.TabIndex = 2;
            this.sphereRadioButton.TabStop = true;
            this.sphereRadioButton.Text = "Sphere";
            this.sphereRadioButton.UseVisualStyleBackColor = true;
            this.sphereRadioButton.CheckedChanged += new System.EventHandler(this.sphereRadioButton_CheckedChanged);
            // 
            // cylinderRadioButton
            // 
            this.cylinderRadioButton.AutoSize = true;
            this.cylinderRadioButton.Location = new System.Drawing.Point(52, 71);
            this.cylinderRadioButton.Name = "cylinderRadioButton";
            this.cylinderRadioButton.Size = new System.Drawing.Size(62, 17);
            this.cylinderRadioButton.TabIndex = 1;
            this.cylinderRadioButton.TabStop = true;
            this.cylinderRadioButton.Text = "Cylinder";
            this.cylinderRadioButton.UseVisualStyleBackColor = true;
            this.cylinderRadioButton.CheckedChanged += new System.EventHandler(this.cylinderRadioButton_CheckedChanged);
            // 
            // cuboidRadioButton
            // 
            this.cuboidRadioButton.AutoSize = true;
            this.cuboidRadioButton.Location = new System.Drawing.Point(52, 42);
            this.cuboidRadioButton.Name = "cuboidRadioButton";
            this.cuboidRadioButton.Size = new System.Drawing.Size(58, 17);
            this.cuboidRadioButton.TabIndex = 0;
            this.cuboidRadioButton.TabStop = true;
            this.cuboidRadioButton.Text = "Cuboid";
            this.cuboidRadioButton.UseVisualStyleBackColor = true;
            this.cuboidRadioButton.CheckedChanged += new System.EventHandler(this.cuboidRadioButton_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(163, 75);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Height Y (mm):";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(166, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Width X (mm):";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(385, 80);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(86, 23);
            this.button5.TabIndex = 9;
            this.button5.Text = "Apply changes";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 4);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(694, 197);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.openGLViewsTableLayoutPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(708, 447);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // openGLViewsTableLayoutPanel
            // 
            this.openGLViewsTableLayoutPanel.ColumnCount = 3;
            this.openGLViewsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.openGLViewsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.openGLViewsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.openGLViewsTableLayoutPanel.Controls.Add(this.OpenGLPanelXY, 0, 1);
            this.openGLViewsTableLayoutPanel.Controls.Add(this.label2, 2, 0);
            this.openGLViewsTableLayoutPanel.Controls.Add(this.label3, 1, 0);
            this.openGLViewsTableLayoutPanel.Controls.Add(this.OpenGLPanel3D, 2, 1);
            this.openGLViewsTableLayoutPanel.Controls.Add(this.OpenGLPanelXZ, 1, 1);
            this.openGLViewsTableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.openGLViewsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openGLViewsTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.openGLViewsTableLayoutPanel.Name = "openGLViewsTableLayoutPanel";
            this.openGLViewsTableLayoutPanel.RowCount = 2;
            this.openGLViewsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.openGLViewsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.openGLViewsTableLayoutPanel.Size = new System.Drawing.Size(702, 230);
            this.openGLViewsTableLayoutPanel.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 239);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(702, 205);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // SpatialConfigurationPanelUI
            // 
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SpatialConfigurationPanelUI";
            this.Size = new System.Drawing.Size(708, 447);
            this.VisibleChanged += new System.EventHandler(this.SpatialConfigurationPanelUI_VisibleChanged);
            this.tabPage2.ResumeLayout(false);
            this.fieldPropertiesGroupBox.ResumeLayout(false);
            this.fieldPropertiesGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resolutoinNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nutrientsDistributionRadiusNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffusionRateNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialQuantityNumericUpDown)).EndInit();
            this.nutrientsGroupBox.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RadiusNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopulationNumericUpDown)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boundaryRadiusNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boundaryDepthNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boundaryHeightNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boundaryWidthNumericUpDown)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.openGLViewsTableLayoutPanel.ResumeLayout(false);
            this.openGLViewsTableLayoutPanel.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }



        private void setCursorPositionLabelValues(float x, float y, float z)
        {

            String strX, strY, strZ;

            strX = (x == 0) ? "0.00" : x.ToString("00.00");
            strY = (y == 0) ? "0.00" : y.ToString("00.00");
            strZ = (z == 0) ? "0.00" : z.ToString("00.00");

            cursorPositionLabel.Text = "    Crosshair : (" +
                strX + ",  " +
                strY + ",  " +
                strZ + ")     ";
        }



        private void OpenGLPanelXY_ChildMouseDownEvent(object sender, MouseEventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (e.Button == MouseButtons.Left)
            {


                Point p = PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
                float x = p.X - OpenGLPanelXY.Location.X;
                float y = p.Y - OpenGLPanelXY.Location.Y;

                spatialViewState.setPointerPosition(x, y, this.OpenGLPanelXY, Views.XY);

                this.OpenGLPanelXY.Refresh();
                this.OpenGLPanelXZ.Refresh();
                this.OpenGLPanel3D.Refresh();

                setCursorPositionLabelValues(spatialViewState.CrossHairPosition.x, spatialViewState.CrossHairPosition.y, spatialViewState.CrossHairPosition.z);
            }
            else if (e.Button == MouseButtons.Right)
            {
                initialPointerPos = new Point(Cursor.Position.X, Cursor.Position.Y);
                openGLPanelViewDraggingState = 1;
                openGLPanelXYAnimationState = 1;
            }
        }


        private void OpenGLPanelXZ_ChildMouseDownEvent(object sender, MouseEventArgs e)
        {

            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (e.Button == MouseButtons.Left)
            {
                Point p = PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
                float x = p.X - OpenGLPanelXZ.Location.X;
                float y = p.Y - OpenGLPanelXZ.Location.Y;


                spatialViewState.setPointerPosition(x, y, this.OpenGLPanelXZ, Views.XZ);
                this.OpenGLPanelXY.Refresh();
                this.OpenGLPanelXZ.Refresh();
                this.OpenGLPanel3D.Refresh();

                setCursorPositionLabelValues(spatialViewState.CrossHairPosition.x, spatialViewState.CrossHairPosition.y, spatialViewState.CrossHairPosition.z);
            }
            else if (e.Button == MouseButtons.Right)
            {
                initialPointerPos = new Point(Cursor.Position.X, Cursor.Position.Y);
                openGLPanelViewDraggingState = 2;
                openGLPanelXZAnimationState = 1;
            }

        }

        private void OpenGLPanelXY_ChildMouseUpEvent(object sender, MouseEventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (e.Button == MouseButtons.Right)
            {
                Point finalPointerPos = new Point(Cursor.Position.X, Cursor.Position.Y);

                spatialViewState.XY.UpdateScrollOffset(finalPointerPos.X - initialPointerPos.X, finalPointerPos.Y - initialPointerPos.Y, OpenGLPanelXY);
                openGLPanelViewDraggingState = 0;
                openGLPanelXYAnimationState = -1;
            }
        }

        private void OpenGLPanelXZ_ChildMouseUpEvent(object sender, MouseEventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (e.Button == MouseButtons.Right)
            {
                Point finalPointerPos = new Point(Cursor.Position.X, Cursor.Position.Y);
                spatialViewState.XZ.UpdateScrollOffset(finalPointerPos.X - initialPointerPos.X, finalPointerPos.Y - initialPointerPos.Y, OpenGLPanelXZ);
                openGLPanelViewDraggingState = 0;
                openGLPanelXZAnimationState = -1;
            }
        }



        public void timer1Tick()
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }



            {
                if (GetSelectedNutrientIndex() != -1 && gogo)
                {

                    NutrientField nut = controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrientFieldObject(GetSelectedNutrientIndex());
                    nut.DoTimeStep(1.0, 1);

                }


                if (this.cellsGogo)
                {

                    StateSnapshot state = controller.GetSelectedSimulationParameters().InitialState;

                    foreach (CellInstance cell in controller.GetSelectedSimulationParameters().InitialState.Cells)
                    {
                        Simulation.SpatialSimulatorZZZ(cell, state, 0, 0.1);


                        foreach (MuCell.Model.SBML.ExtracellularComponents.ComponentWorldStateBase component in cell.Components)
                        {

                            component.DoTimeStep(cell, controller.GetSelectedSimulationParameters().InitialState, 0.0, 0.1);
                        }


                    }
                }








            }

            SimulationParameters selectedParams = controller.GetSelectedSimulationParameters();
            if (selectedParams != null)
            {
                selectedParams.EnvironmentViewState.ThreeD.Ang3D += 0.3f;
            }
            this.OpenGLPanel3D.Refresh();


            if (openGLPanelXYAnimationState != 0)
            {
                if (openGLPanelViewDraggingState == 1)
                {
                    Point currentPointerPos = new Point(Cursor.Position.X, Cursor.Position.Y);
                    spatialViewState.XY.SetTempScrollOffset(currentPointerPos.X - initialPointerPos.X, currentPointerPos.Y - initialPointerPos.Y, OpenGLPanelXY);

                }

                this.OpenGLPanelXY.Refresh();
                if (openGLPanelXYAnimationState == -1)
                {
                    openGLPanelXYAnimationState = 0;
                }
            }



            if (openGLPanelXZAnimationState != 0)
            {
                if (openGLPanelViewDraggingState == 2)
                {
                    Point currentPointerPos = new Point(Cursor.Position.X, Cursor.Position.Y);
                    spatialViewState.XZ.SetTempScrollOffset(currentPointerPos.X - initialPointerPos.X, currentPointerPos.Y - initialPointerPos.Y, OpenGLPanelXZ);
                }

                this.OpenGLPanelXZ.Refresh();
                if (openGLPanelXZAnimationState == -1)
                {
                    openGLPanelXZAnimationState = 0;
                }
            }


        }


        private void DisableUnusedBoundaryNumbericUpDowns()
        {

            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            switch (controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.Boundary.Shape)
            {
                case BoundaryShapes.Cuboid:

                    boundaryWidthNumericUpDown.Enabled = true;
                    boundaryHeightNumericUpDown.Enabled = true;
                    boundaryDepthNumericUpDown.Enabled = true;
                    boundaryRadiusNumericUpDown.Enabled = false;
                    break;

                case BoundaryShapes.Cylinder:

                    boundaryWidthNumericUpDown.Enabled = false;
                    boundaryHeightNumericUpDown.Enabled = true;
                    boundaryDepthNumericUpDown.Enabled = false;
                    boundaryRadiusNumericUpDown.Enabled = true;
                    break;

                case BoundaryShapes.Sphere:

                    boundaryWidthNumericUpDown.Enabled = false;
                    boundaryHeightNumericUpDown.Enabled = false;
                    boundaryDepthNumericUpDown.Enabled = false;
                    boundaryRadiusNumericUpDown.Enabled = true;
                    break;
            }
        }


        #region ISpatialConfigurationPanelUI Members


        public void setDistributionFunctions(List<DistributionFunction> funcs)
        {

            foreach (DistributionFunction func in funcs)
            {
                DistributionComboBox.Items.Add(func);
            }

            int index = DistributionComboBox.FindString("Normal");
            DistributionComboBox.SelectedIndex = index;
        }


        public void setSimulationParameters(SimulationParameters simulationParameters)
        {


            //set viewState to display the initial state of the given simulation parameter set
            spatialViewState = simulationParameters.EnvironmentViewState;
            spatialViewState.SimParams = simulationParameters;
            spatialViewState.InitialSimState = simulationParameters.InitialState;
            OpenGLPanelXY.SpatialViewState = spatialViewState;
            OpenGLPanelXZ.SpatialViewState = spatialViewState;
            OpenGLPanel3D.SpatialViewState = spatialViewState;

            setBoundaryShape(simulationParameters.InitialState.SimulationEnvironment.Boundary.Shape);
            setBoundaryWidth(simulationParameters.InitialState.SimulationEnvironment.Boundary.Width);
            setBoundaryHeight(simulationParameters.InitialState.SimulationEnvironment.Boundary.Height);
            setBoundaryDepth(simulationParameters.InitialState.SimulationEnvironment.Boundary.Depth);
            setBoundaryRadius(simulationParameters.InitialState.SimulationEnvironment.Boundary.Radius);

            spatialViewState.UpdateCursorOutOfBounds();

            controller.updateCellGroups();
            controller.updateNutrientList();
            setSelectedGroupByIndex(spatialViewState.SelectedGroupIndex);
            setSelectedNutrientByIndex(spatialViewState.SelectedNutrientIndex);
            updateNutrientFieldProperties();
            updateAddCellsEnabled();
            spatialViewState.UpdateSelectedGroupBox();
            OpenGLPanelXY.Refresh();
            OpenGLPanelXZ.Refresh();
            OpenGLPanel3D.Refresh();

        }


        public void setCellDefinitions(CellDefinition[] cellDefs)
        {
            CellTypeComboBox.Items.Clear();

            foreach (CellDefinition cellDef in cellDefs)
            {
                CellTypeComboBox.Items.Add(cellDef);
            }

            if (CellTypeComboBox.Items.Count > 0)
            {
                CellTypeComboBox.SelectedIndex = 0;
            }
        }


        /// <summary>
        /// Sets the list of groups in the group tree view
        /// </summary>
        /// <param name="groups"></param>
        public void setCellGroups(List<MuCell.Model.SBML.Group> groups)
        {
            int i = 0;

            groupListBox.Items.Clear();
            groupListBox.Sorted = true;

            foreach (MuCell.Model.SBML.Group group in groups)
            {
                groupListBox.Items.Add(group);
                i++;
            }

        }



        /// <summary>
        /// Sets which radio button is selected in the spatial settings tab.
        /// </summary>
        /// <param name="shape"></param>
        public void setBoundaryShape(BoundaryShapes shape)
        {
            switch (shape)
            {
                case BoundaryShapes.Cuboid:

                    cuboidRadioButton.Checked = true;
                    break;

                case BoundaryShapes.Cylinder:

                    cylinderRadioButton.Checked = true;
                    break;

                case BoundaryShapes.Sphere:

                    sphereRadioButton.Checked = true;
                    break;
            }

        }

        /// <summary>
        /// Sets the value of the width numerical component in the spatial settings tab.
        /// </summary>
        /// <param name="width"></param>
        public void setBoundaryWidth(float width)
        {
            boundaryWidthNumericUpDown.Value = (decimal)width;
        }

        /// <summary>
        /// Sets the value of the height numerical component in the spatial settings tab.
        /// </summary>
        /// <param name="width"></param>
        public void setBoundaryHeight(float height)
        {
            boundaryHeightNumericUpDown.Value = (decimal)height;
        }

        /// <summary>
        /// Sets the value of the depth numerical component in the spatial settings tab.
        /// </summary>
        /// <param name="width"></param>
        public void setBoundaryDepth(float depth)
        {
            boundaryDepthNumericUpDown.Value = (decimal)depth;
        }

        /// <summary>
        /// Sets the value of the radius numerical component in the spatial settings tab.
        /// </summary>
        /// <param name="width"></param>
        public void setBoundaryRadius(float radius)
        {
            boundaryRadiusNumericUpDown.Value = (decimal)radius;
        }

        public int GetSelectedGroupIndex()
        {
            if (groupListBox.SelectedItem != null)
            {

                MuCell.Model.SBML.Group group;
                group = (MuCell.Model.SBML.Group)(groupListBox.SelectedItem);
                return group.Index;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Sets which group is selected in the group list box.
        /// </summary>
        /// <param name="group"></param>
        public void setSelectedGroup(MuCell.Model.SBML.Group group)
        {
            if (groupListBox.Items.Contains(group))
            {
                groupListBox.SelectedItem = group;
            }
        }

        /// <summary>
        /// Sets which group is selected in the group list box by the ID of the group.
        /// </summary>
        /// <param name="group"></param>
        public void setSelectedGroupByIndex(int index)
        {
            TestRigs.ErrorLog.LogError("Attempting to set group by index: " + index);

            foreach (Object obj in groupListBox.Items)
            {
                MuCell.Model.SBML.Group group = (MuCell.Model.SBML.Group)obj;
                if (index == group.Index)
                {
                    TestRigs.ErrorLog.LogError("Success");
                    groupListBox.SelectedItem = obj;
                    return;
                }

            }

        }

        /// <summary>
        /// Sets which nutrient is selected in the nutrient list box
        /// </summary>
        /// <param name="nutrient"></param>
        public void setSelectedNutrient(MuCell.Model.NutrientField nutrient)
        {
            if (nutrientFieldListBox.Items.Contains(nutrient))
            {
                nutrientFieldListBox.SelectedItem = nutrient;
            }
        }

        /// <summary>
        /// Sets which nutrient is selected in the nutrient list box given the id
        /// of the nutrient
        /// </summary>
        /// <param name="index"></param>
        public void setSelectedNutrientByIndex(int index)
        {
            foreach (Object obj in nutrientFieldListBox.Items)
            {
                MuCell.Model.NutrientField nut = (MuCell.Model.NutrientField)obj;
                if (index == nut.Index)
                {

                    nutrientFieldListBox.SelectedItem = obj;
                    return;
                }

            }
        }


        /// <summary>
        /// Sets the UI to display the given list of nutrients in the nutrient list box
        /// </summary>
        /// <param name="nutrients"></param>
        public void setNutrients(List<MuCell.Model.NutrientField> nutrients)
        {
            int i = 0;


            nutrientFieldListBox.Items.Clear();
            nutrientFieldListBox.Sorted = true;

            foreach (MuCell.Model.NutrientField nutrient in nutrients)
            {
                nutrientFieldListBox.Items.Add(nutrient);
                i++;
            }
        }

        /// <summary>
        /// Returns the index of the currently selected nutrient in the nutrients
        /// listbox
        /// </summary>
        /// <returns></returns>
        public int GetSelectedNutrientIndex()
        {
            if (nutrientFieldListBox.SelectedItem != null)
            {

                MuCell.Model.NutrientField nutrient;
                nutrient = (MuCell.Model.NutrientField)(nutrientFieldListBox.SelectedItem);
                return nutrient.Index;
            }
            else
            {
                return -1;
            }
        }


        /// <summary>
        /// Sets the list of initial distributions that the user may choose from
        /// </summary>
        /// <param name="List"></param>
        public void setInitalNutrientDistributions()
        {
            /*
            foreach (Object obj in dists)
            {
                initialDistributionComboBox.Items.Add(obj);
            }*/
            String str1 = "Uniform across entire environment";
            String str2 = "Uniform sphere";
            String str3 = "Densely centred sphere";

            initialDistributionComboBox.Items.Add(str1);
            initialDistributionComboBox.Items.Add(str2);
            initialDistributionComboBox.Items.Add(str3);

            int index = initialDistributionComboBox.FindString("Densely");
            initialDistributionComboBox.SelectedIndex = index;

        }


        /// <summary>
        /// Displays a dialog reporting the given list of missing nutrients.
        /// </summary>
        /// <param name="names"></param>
        public void DisplayMissingNutrientsDialog(List<String> names)
        {
            String message = "Unable to add cells of this type because the Environment \r\n";
            message += "does not support the necessary Nutrient Fields. \r\n\r\n";
            message += "Nutrient Field(s) with the following name(s) are required: \r\n\r\n";

            foreach (String name in names)
            {
                message += name + " \r\n";
            }

            MessageBox.Show(message);
        }


        #endregion

        private void updateAddCellsEnabled()
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            int groupIndex = GetSelectedGroupIndex();


            addCellsButton.Enabled = (groupIndex != -1);
            CellTypeComboBox.Enabled = (groupIndex != -1);
            DistributionComboBox.Enabled = (groupIndex != -1);
            PopulationNumericUpDown.Enabled = (groupIndex != -1);
            removeButton.Enabled = (groupIndex != -1);
            RadiusNumericUpDown.Enabled = (groupIndex != -1);


        }


        private void addCellsButton_Click(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            int groupIndex = GetSelectedGroupIndex();

            if (groupIndex == -1)
            {
                //MessageBox = new MessageBox
                MessageBox.Show("Please select an existent cell group or create a new cell group before adding cell populations.");

            }
            else
            {

                controller.addCellPopulation((CellDefinition)CellTypeComboBox.SelectedItem, (int)PopulationNumericUpDown.Value, (float)RadiusNumericUpDown.Value, (DistributionFunction)DistributionComboBox.SelectedItem, spatialViewState.CrossHairPosition, groupIndex);
                spatialViewState.UpdateSelectedGroupBox();
                updateCellCountLabel();
                this.OpenGLPanelXY.Refresh();
                this.OpenGLPanelXZ.Refresh();
                this.OpenGLPanel3D.Refresh();
            }
        }

        private void updateCellCountLabel()
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (GetSelectedGroupIndex() != -1)
            {
                MuCell.Model.SBML.Group grp =
                    controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetGroupObject(GetSelectedGroupIndex());
                cellCountLabel.Text = "Cell Count: " + grp.Cells.Count;
            }
        }




        private void OpenGLPanelXY_Resize(object sender, EventArgs e)
        {

            OpenGLPanelXY.Width = OpenGLPanelXY.Height = Math.Min(OpenGLPanelXY.Width, OpenGLPanelXY.Height);

        }

        private void OpenGLPanelXZ_Resize(object sender, EventArgs e)
        {
            OpenGLPanelXZ.Width = OpenGLPanelXZ.Height = Math.Min(OpenGLPanelXZ.Width, OpenGLPanelXZ.Height);

        }







        private void OpenGLPanel3D_Resize(object sender, EventArgs e)
        {

            OpenGLPanel3D.Width = OpenGLPanel3D.Height = Math.Min(OpenGLPanel3D.Width, OpenGLPanel3D.Height);
        }



        //Event handler for moving the mouse wheel (zoom in and out)
        void OpenGLViewsTableLayoutPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }



            OpenGLCellPlacementPanel mouseOverView = mouseOverOpenGLPanel();




            if (mouseOverView != null)
            {
                // mouseOverView.Ang3D += 0.4f;


                if (e.Delta > 0)
                {
                    mouseOverView.PanelViewState.ZoomIn();
                }
                if (e.Delta < 0)
                {
                    mouseOverView.PanelViewState.ZoomOut();
                }

                mouseOverView.Refresh();
            }
        }


        private OpenGLCellPlacementPanel mouseOverOpenGLPanel()
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return null;
            }


            Point p = PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
            float x1 = ((float)p.X - OpenGLPanelXY.Location.X - OpenGLPanelXY.Padding.Left) / OpenGLPanelXY.GetWidth();
            float y1 = ((float)p.Y - OpenGLPanelXY.Location.Y - OpenGLPanelXY.Padding.Top) / OpenGLPanelXY.GetHeight();
            float x2 = ((float)p.X - OpenGLPanelXZ.Location.X - OpenGLPanelXZ.Padding.Left) / OpenGLPanelXZ.GetWidth();
            float y2 = ((float)p.Y - OpenGLPanelXZ.Location.Y - OpenGLPanelXZ.Padding.Top) / OpenGLPanelXZ.GetHeight();


            if (x1 > 0 && x1 < 1 && y1 > 0 && y1 < 1)
            {
                return OpenGLPanelXY;
            }


            if (x2 > 0 && x2 < 1 && y2 > 0 && y2 < 1)
            {
                return OpenGLPanelXZ;
            }

            return null;
        }

        private void SpatialConfigurationPanelUI_VisibleChanged(object sender, EventArgs e)
        {


            if (this.Visible)
            {
                this.Focus();
            }
        }


        private void BoundaryChanged()
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }

            updateResolutionHelper();
            spatialViewState.UpdateCursorOutOfBounds();
            spatialViewState.ThreeDAnalyzer.UpdateCrossSectionSize(controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.Boundary);
            spatialViewState.ThreeD.UpdateCrossSectionSize(controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.Boundary);
            this.OpenGLPanelXY.Refresh();
            this.OpenGLPanelXZ.Refresh();




        }


        private void cuboidRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }

            if (cuboidRadioButton.Checked)
            {

                liftBoundaryShapeSelected(controller.GetSelectedSimulationParameters());
                DisableUnusedBoundaryNumbericUpDowns();
                BoundaryChanged();

            }
        }

        private void cylinderRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }

            if (cylinderRadioButton.Checked)
            {
                liftBoundaryShapeSelected(controller.GetSelectedSimulationParameters());
                DisableUnusedBoundaryNumbericUpDowns();
                BoundaryChanged();


            }
        }

        private void sphereRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }

            if (sphereRadioButton.Checked)
            {
                liftBoundaryShapeSelected(controller.GetSelectedSimulationParameters());
                DisableUnusedBoundaryNumbericUpDowns();
                BoundaryChanged();

            }
        }




        private void liftBoundaryShapeSelected(SimulationParameters paras)
        {

            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }

            if (cuboidRadioButton.Checked)
            {
                paras.InitialState.SimulationEnvironment.Boundary.Shape = BoundaryShapes.Cuboid;
            }
            else if (cylinderRadioButton.Checked)
            {
                paras.InitialState.SimulationEnvironment.Boundary.Shape = BoundaryShapes.Cylinder;
            }
            else if (sphereRadioButton.Checked)
            {
                paras.InitialState.SimulationEnvironment.Boundary.Shape = BoundaryShapes.Sphere;
            }
        }

        private void boundaryWidthNumericUpDown_ValueChanged(object sender, EventArgs e)
        {

            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }

            controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.Boundary.Width = (float)boundaryWidthNumericUpDown.Value;
            BoundaryChanged();

        }

        private void boundaryHeightNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.Boundary.Height = (float)boundaryHeightNumericUpDown.Value;
            BoundaryChanged();

        }

        private void boundaryDepthNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.Boundary.Depth = (float)boundaryDepthNumericUpDown.Value;
            BoundaryChanged();

        }

        private void boundaryRadiusNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.Boundary.Radius = (float)boundaryRadiusNumericUpDown.Value;
            BoundaryChanged();

        }

        private void addGroupButton_Click(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            controller.AddGroup();

        }


        private void removeButton_Click(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (groupListBox.SelectedItem != null)
            {
                controller.RemoveGroup();
                updateAddCellsEnabled();
                spatialViewState.UpdateSelectedGroupBox();
                this.OpenGLPanelXY.Refresh();
                this.OpenGLPanelXZ.Refresh();
            }
        }

        private void groupListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {


            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }



            if (groupListBox.SelectedItem != null)
            {
                MuCell.Model.SBML.Group group = (MuCell.Model.SBML.Group)groupListBox.SelectedItem;

                ColorDialog cd = new ColorDialog();
                cd.AllowFullOpen = true;
                cd.FullOpen = true;
                cd.Color = group.Col;

                if (cd.ShowDialog() == DialogResult.OK)
                {
                    // MessageBox.Show(cd.Color.ToString());
                    group.Col = cd.Color;
                }
                this.OpenGLPanelXY.Refresh();
                this.OpenGLPanelXZ.Refresh();



            }

        }

        private void groupListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (groupListBox.SelectedItem != null)
            {
                MuCell.Model.SBML.Group group;

                group = (MuCell.Model.SBML.Group)groupListBox.SelectedItem;
                spatialViewState.SelectedGroupIndex = group.Index;

                spatialViewState.UpdateSelectedGroupBox();
                updateCellCountLabel();
                this.OpenGLPanelXY.Refresh();
                this.OpenGLPanelXZ.Refresh();
            }

            updateAddCellsEnabled();
        }


        private void addNutrientFieldButton_Click(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            controller.AddNutrient();
        }

        private void removeNutrientFieldButton_Click(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (nutrientFieldListBox.SelectedItem != null)
            {
                NutrientField nutrient = (NutrientField)nutrientFieldListBox.SelectedItem;


                if (cellDefinitonNutrientDependency(nutrient.Name))
                {
                    String message = "Cannot delete this Nutrient Field: one or more Cell Definitions \r\n";
                    message += "are dependent upon it. ";
                    MessageBox.Show(message);
                    return;
                }



                controller.RemoveNutrient();
                updateNutrientFieldProperties();
                updateResolutionHelper();
                this.OpenGLPanelXY.Refresh();
                this.OpenGLPanelXZ.Refresh();
            }
        }

        private void nutrientFieldListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (nutrientFieldListBox.SelectedItem != null)
            {
                NutrientField nutrient = (NutrientField)nutrientFieldListBox.SelectedItem;

                spatialViewState.SelectedNutrientIndex = nutrient.Index;
                this.OpenGLPanelXY.Refresh();
                this.OpenGLPanelXZ.Refresh();
            }
            else
            {
                spatialViewState.SelectedNutrientIndex = -1;
            }
            updateNutrientFieldProperties();
        }

        private void nutrientFieldListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (nutrientFieldListBox.SelectedItem != null)
            {
                NutrientField nutrient = (NutrientField)nutrientFieldListBox.SelectedItem;

                ColorDialog cd = new ColorDialog();
                cd.AllowFullOpen = true;
                cd.FullOpen = true;
                cd.Color = nutrient.Col;

                if (cd.ShowDialog() == DialogResult.OK)
                {
                    // MessageBox.Show(cd.Color.ToString());
                    nutrient.Col = cd.Color;
                }
                this.OpenGLPanelXY.Refresh();
                this.OpenGLPanelXZ.Refresh();



            }
        }

        public void updateNutrientFieldProperties()
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }



            if (GetSelectedNutrientIndex() == -1)
            {
                initialDistributionComboBox.Enabled = false;
                nutrientsDistributionRadiusNumericUpDown.Enabled = false;
                initialQuantityNumericUpDown.Enabled = false;
                diffusionRateNumericUpDown.Enabled = false;
                resolutoinNumericUpDown.Enabled = false;
                removeNutrientFieldButton.Enabled = false;
                resolutionHelper.Enabled = false;
                viewNutrient.Enabled = false;
            }
            else
            {
                initialDistributionComboBox.Enabled = true;
                initialQuantityNumericUpDown.Enabled = true;
                diffusionRateNumericUpDown.Enabled = true;
                resolutoinNumericUpDown.Enabled = true;
                removeNutrientFieldButton.Enabled = true;
                resolutionHelper.Enabled = true;
                viewNutrient.Enabled = true;

                NutrientField nutrient = controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrientFieldObject(GetSelectedNutrientIndex());

                initialDistributionComboBox.SelectedIndex = (int)nutrient.InitialDistribution;
                nutrientsDistributionRadiusNumericUpDown.Value = (Decimal)nutrient.InitialRadius;
                initialQuantityNumericUpDown.Value = (Decimal)nutrient.InitialQuantity;
                diffusionRateNumericUpDown.Value = (Decimal)nutrient.DiffusionRate;
                resolutoinNumericUpDown.Value = (Decimal)nutrient.Resolution;

                if (nutrient.InitialDistribution != InitialNutrientDistribution.UniformThroughout)
                {
                    nutrientsDistributionRadiusNumericUpDown.Enabled = true;
                }

                updateResolutionHelper();

            }


        }




        private void initialQuantityNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (GetSelectedNutrientIndex() != -1)
            {

                NutrientField nutrient = controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrientFieldObject(GetSelectedNutrientIndex());
                nutrient.InitialQuantity = (float)initialQuantityNumericUpDown.Value;

            }
        }



        private void initialDistributionComboBox_SelectedValueChanged(object sender, EventArgs e)
        {




        }

        private void initialDistributionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            //return if simulation is not yet set
            if (controller == null || controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (initialDistributionComboBox.SelectedIndex != -1)
            {

                if (GetSelectedNutrientIndex() != -1)
                {
                    TestRigs.ErrorLog.LogError("SELECTED INDEX: ");
                    NutrientField nutrient = controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrientFieldObject(GetSelectedNutrientIndex());
                    nutrient.InitialDistribution = (InitialNutrientDistribution)initialDistributionComboBox.SelectedIndex;

                    if (nutrient.InitialDistribution == InitialNutrientDistribution.UniformThroughout)
                    {

                        nutrientsDistributionRadiusNumericUpDown.Enabled = false;
                    }
                    else
                    {
                        nutrientsDistributionRadiusNumericUpDown.Enabled = true;
                    }
                }

            }
        }



        private void nutrientsDistributionRadiusNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (GetSelectedNutrientIndex() != -1)
            {
                NutrientField nutrient = controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrientFieldObject(GetSelectedNutrientIndex());
                nutrient.InitialRadius = (float)nutrientsDistributionRadiusNumericUpDown.Value;
            }
        }

        private void diffusionRateNumericUpDown_ValueChanged(object sender, EventArgs e)
        {

            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (GetSelectedNutrientIndex() != -1)
            {
                NutrientField nutrient = controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrientFieldObject(GetSelectedNutrientIndex());
                nutrient.DiffusionRate = (float)diffusionRateNumericUpDown.Value;
            }
        }

        private void resolutoinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (GetSelectedNutrientIndex() != -1)
            {
                NutrientField nutrient = controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrientFieldObject(GetSelectedNutrientIndex());
                nutrient.Resolution = (float)resolutoinNumericUpDown.Value;
                updateResolutionHelper();
            }

        }

        private void updateResolutionHelper()
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (GetSelectedNutrientIndex() != -1)
            {
                NutrientField nutrient = controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrientFieldObject(GetSelectedNutrientIndex());
                Boundary bounds = controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.Boundary;
                float val = nutrient.EstimateSpaceConsumption(bounds);
                nutrient.UpdateDimensions(bounds);

                String str = "This will generate an array with dimentions: "
                                  + nutrient.Dim[0].ToString("###") +
                              "x" + nutrient.Dim[1].ToString("###") +
                              "x" + nutrient.Dim[2].ToString("###") +
                    " and estimated space consumption (per snapshot) of: " + val.ToString("0.00") + " mb";


                float totalMemory = 0;
                foreach (int nutrientIndex in controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrients())
                {
                    NutrientField nut = controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrientFieldObject(nutrientIndex);
                    totalMemory += nut.EstimateSpaceConsumption(bounds);
                }

                if (totalMemory > 10)
                {
                    str = "WARNING! Total memory consumed by nutrient fields exceeds 10 mb per snapshot. Please consider removing some of the nutrient fields present or decreasing their resolution.";

                    str += " \r\n     Selected Field: " + val.ToString("0.00") + " mb" + "         Total: " + totalMemory.ToString("0.00") + " mb.";

                    Color col =

                    resolutionHelper.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    str += " \r\n\r\n     Total Field Memory: " + totalMemory.ToString("0.00") + " mb.";

                    resolutionHelper.ForeColor = System.Drawing.Color.Black;
                }



                resolutionHelper.Text = str;
                //spaceConsumptionLabel.Text = str;
            }
        }

        private void testFieldButton_Click(object sender, EventArgs e)
        {

            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            //init the currently selected nutrient field

            if (GetSelectedNutrientIndex() != -1)
            {

                NutrientField nut = controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrientFieldObject(GetSelectedNutrientIndex());
                nut.InitField(controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.Boundary);

            }
        }



        /// <summary>
        /// Checks whether or not there is a cell in the inital state of the 
        /// environment which is depentent upon the given nutrient (by name).
        /// </summary>
        /// <param name="nutrientName"></param>
        /// <returns></returns>
        private bool cellDefinitonNutrientDependency(String nutrientName)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return false;
            }


            foreach (CellInstance cell in controller.GetSelectedSimulationParameters().InitialState.Cells)
            {
                foreach (MuCell.Model.SBML.ExtracellularComponents.ComponentWorldStateBase component in cell.Components)
                {
                    foreach (String requiredNutrient in component.ComponentType.GetRequiredNutrientFieldNames())
                    {
                        if (requiredNutrient.Equals(nutrientName))
                        {

                            return true;
                        }
                    }

                }

            }

            return false;

        }



        private void nutrientFieldListBox_MouseUp(object sender, MouseEventArgs e)
        {

            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (e.Button == MouseButtons.Right)
            {
                int nutrientIndex = GetSelectedNutrientIndex();

                if (nutrientIndex != -1)
                {
                    NutrientField nutrient = controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrientFieldObject(nutrientIndex);
                    InputDialog input = new InputDialog("Rename this nutrient", "Rename...", nutrient.Name);

                    if (input.ShowDialog() == DialogResult.OK)
                    {
                        //no change
                        if (nutrient.Name.Equals(input.NewString))
                        {
                            return;
                        }

                        //name already taken
                        if (controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrientByName(input.NewString) != null)
                        {
                            MessageBox.Show("Failed to rename: name already in use");
                            return;
                        }



                        //check that there are no cells that need the old nutrient
                        if (cellDefinitonNutrientDependency(nutrient.Name))
                        {
                            String message = "Failed to rename: one or more Cell Definitions \r\n";
                            message += "are dependent upon this Nutrient Field. ";
                            MessageBox.Show(message);
                            return;
                        }


                        //check that there are no cells that need the old nutrient
                        foreach (CellInstance cell in controller.GetSelectedSimulationParameters().InitialState.Cells)
                        {
                            foreach (MuCell.Model.SBML.ExtracellularComponents.ComponentWorldStateBase component in cell.Components)
                            {
                                foreach (String requiredNutrient in component.ComponentType.GetRequiredNutrientFieldNames())
                                {
                                    if (requiredNutrient.Equals(nutrient.Name))
                                    {
                                        String message = "Failed to rename: one or more Cell Definitions \r\n";
                                        message += "are dependent upon this Nutrient Field. ";
                                        MessageBox.Show(message);
                                        return;
                                    }
                                }

                            }

                        }



                        //otherwise go ahead with the rename:

                        nutrient.Name = input.NewString;
                        controller.updateNutrientList();
                    }


                }

            }
        }

        private void tmpCellTest_Click(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            this.cellsGogo = !this.cellsGogo;
            Random rand = new Random();
            foreach (CellInstance cell in controller.GetSelectedSimulationParameters().InitialState.Cells)
            {
                cell.SetRandomObject(rand);
            }

        }

        private void viewNutrient_Click(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }

            if (GetSelectedNutrientIndex() != -1)
            {
                NutrientField nut = controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.GetNutrientFieldObject(GetSelectedNutrientIndex());
                nut.InitField(controller.GetSelectedSimulationParameters().InitialState.SimulationEnvironment.Boundary);
                float scale = nut.EstimateIntensityScale();
                controller.GetSelectedSimulationParameters().EnvironmentViewState.NutrientIntensityScale = scale;


            }

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //return if simulation is not yet set
            if (controller.GetSelectedSimulationParameters() == null)
            {
                return;
            }


            if (tabControl1.SelectedIndex == 2)
            {
                controller.GetSelectedSimulationParameters().EnvironmentViewState.ThreeD.CrossSectionEnabled = true;
                controller.GetSelectedSimulationParameters().EnvironmentViewState.ThreeD.drawNutrientInitialPos = true;
                controller.GetSelectedSimulationParameters().EnvironmentViewState.XY.drawNutrientInitialPos = true;
                controller.GetSelectedSimulationParameters().EnvironmentViewState.XZ.drawNutrientInitialPos = true;
                this.OpenGLPanelXY.Refresh();
                this.OpenGLPanelXZ.Refresh();

            }
            else
            {
                controller.GetSelectedSimulationParameters().EnvironmentViewState.ThreeD.CrossSectionEnabled = false;
                controller.GetSelectedSimulationParameters().EnvironmentViewState.ThreeD.drawNutrientInitialPos = false;
                controller.GetSelectedSimulationParameters().EnvironmentViewState.XY.drawNutrientInitialPos = false;
                controller.GetSelectedSimulationParameters().EnvironmentViewState.XZ.drawNutrientInitialPos = false;
                this.OpenGLPanelXY.Refresh();
                this.OpenGLPanelXZ.Refresh();
            }
        }




    }
}
