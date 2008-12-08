using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MuCell.Controller;
using System.Drawing;
using System.Drawing.Drawing2D;
using MuCell.Model;
using MuCell.Model.SBML;
using System.Runtime.InteropServices;
using MuCell.Model.SBML.ExtracellularComponents;


/// <summary>
/// This class extends the Panel class and implements the ICellDefinitionsPanelUI 
/// interface, providing a concrete graphical Panel for displaying and editing
/// cell definitions
/// 
/// This panel is to be a child of ExperimentTreePanel
/// 
/// </summary>

namespace MuCell.View
{
    public enum EditMode
    {
        Pointer, AddSpecies, AddReactions, ReactionLinks, Eraser, Test, ModifierLinks, AddComponents, AutoRearrange
    }
    class CellDefinitionsPanelUI : UserControl, ICellDefinitionsPanelUI, IDrawingInterface
    {
        private Button ProteinBtn;
        private Button reactionBtn;
        private Button rearrangeBtn;
        private Button loadBtn;
        private Button saveBtn;
        private TrackBar trackBar1;
        private Label label3;
        private Button linkBtn;
        private PictureBox pictureBox1;
        private PropertyGrid propertyGrid1;

        private CellDefinitionsPanelController controller;
        private List<IModelComponent> selectedComponents;
        private bool leftDown;
        private bool rightDown;
        private Vector2 moveStart;
        private Vector2 totalMouseMove;
        private Vector2 lastMousePos;
        private Button pointerBtn;
        private Button eraserBtn;
        private EditMode editMode = EditMode.Pointer;

        private Vector2 viewTranslate = new Vector2(0f,0f);
        private Vector2 viewCentre = new Vector2(0f, 0f);
        private OpenFileDialog openFileDialog1;
        private Button testBtn;
        private float viewScale = 1.0f;
        private ImageList imageList1;
        private System.ComponentModel.IContainer components;
        private PictureBox stopIcon;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel3;
        private FlowLayoutPanel flowLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel4;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label label1;
        private Button modifierLinkBtn;
        private ComboBox componentSelectionBox;
        private Button addComponentBtn;
        private SaveFileDialog saveFileDialog1;

        private CellInstance cellOverlay;
        
        enum ArrowType
        {
            Promotion, Demotion, LineOnly
        }

        public CellDefinitionsPanelUI()
            : base()
        {
            InitializeComponent();
            viewCentre = new Vector2(this.pictureBox1.Width * 0.5f, this.pictureBox1.Height * 0.5f);
            selectedComponents = new List<IModelComponent>();
            componentSelectionBox.DataSource = ComponentBase.ComponentFactoryTypes();
        }
        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CellDefinitionsPanelUI));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ProteinBtn = new System.Windows.Forms.Button();
            this.reactionBtn = new System.Windows.Forms.Button();
            this.rearrangeBtn = new System.Windows.Forms.Button();
            this.loadBtn = new System.Windows.Forms.Button();
            this.saveBtn = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.linkBtn = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.pointerBtn = new System.Windows.Forms.Button();
            this.eraserBtn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.testBtn = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.stopIcon = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.modifierLinkBtn = new System.Windows.Forms.Button();
            this.componentSelectionBox = new System.Windows.Forms.ComboBox();
            this.addComponentBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopIcon)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(765, 581);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // ProteinBtn
            // 
            this.ProteinBtn.AutoSize = true;
            this.ProteinBtn.Image = ((System.Drawing.Image)(resources.GetObject("ProteinBtn.Image")));
            this.ProteinBtn.Location = new System.Drawing.Point(3, 75);
            this.ProteinBtn.Name = "ProteinBtn";
            this.ProteinBtn.Size = new System.Drawing.Size(79, 30);
            this.ProteinBtn.TabIndex = 4;
            this.ProteinBtn.Text = "Species";
            this.ProteinBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ProteinBtn.UseVisualStyleBackColor = true;
            this.ProteinBtn.Click += new System.EventHandler(this.ProteinBtn_Click);
            // 
            // reactionBtn
            // 
            this.reactionBtn.AutoSize = true;
            this.reactionBtn.Image = ((System.Drawing.Image)(resources.GetObject("reactionBtn.Image")));
            this.reactionBtn.Location = new System.Drawing.Point(186, 39);
            this.reactionBtn.Name = "reactionBtn";
            this.reactionBtn.Size = new System.Drawing.Size(89, 30);
            this.reactionBtn.TabIndex = 5;
            this.reactionBtn.Text = "Reaction";
            this.reactionBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.reactionBtn.UseVisualStyleBackColor = true;
            this.reactionBtn.Click += new System.EventHandler(this.reactionBtn_Click);
            // 
            // rearrangeBtn
            // 
            this.rearrangeBtn.Location = new System.Drawing.Point(165, 3);
            this.rearrangeBtn.Name = "rearrangeBtn";
            this.rearrangeBtn.Size = new System.Drawing.Size(75, 23);
            this.rearrangeBtn.TabIndex = 7;
            this.rearrangeBtn.Text = "Rearrange";
            this.rearrangeBtn.UseVisualStyleBackColor = true;
            this.rearrangeBtn.Click += new System.EventHandler(this.rearrangeBtn_Click);
            // 
            // loadBtn
            // 
            this.loadBtn.Location = new System.Drawing.Point(3, 3);
            this.loadBtn.Name = "loadBtn";
            this.loadBtn.Size = new System.Drawing.Size(75, 23);
            this.loadBtn.TabIndex = 8;
            this.loadBtn.Text = "Add SBML";
            this.loadBtn.UseVisualStyleBackColor = true;
            this.loadBtn.Click += new System.EventHandler(this.loadBtn_Click);
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(84, 3);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 9;
            this.saveBtn.Text = "Save SBML";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.BackColor = System.Drawing.SystemColors.Window;
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBar1.Location = new System.Drawing.Point(46, 3);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(716, 47);
            this.trackBar1.TabIndex = 10;
            this.trackBar1.Value = 5;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Zoom:";
            // 
            // linkBtn
            // 
            this.linkBtn.AutoSize = true;
            this.linkBtn.Image = ((System.Drawing.Image)(resources.GetObject("linkBtn.Image")));
            this.linkBtn.Location = new System.Drawing.Point(3, 39);
            this.linkBtn.Name = "linkBtn";
            this.linkBtn.Size = new System.Drawing.Size(70, 30);
            this.linkBtn.TabIndex = 13;
            this.linkBtn.Text = "Links";
            this.linkBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.linkBtn.UseVisualStyleBackColor = true;
            this.linkBtn.Click += new System.EventHandler(this.linkBtn_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 173);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(272, 405);
            this.propertyGrid1.TabIndex = 14;
            this.propertyGrid1.Validating += new System.ComponentModel.CancelEventHandler(this.propertyGrid1_Validating);
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // pointerBtn
            // 
            this.pointerBtn.AutoSize = true;
            this.pointerBtn.Image = ((System.Drawing.Image)(resources.GetObject("pointerBtn.Image")));
            this.pointerBtn.Location = new System.Drawing.Point(3, 3);
            this.pointerBtn.Name = "pointerBtn";
            this.pointerBtn.Size = new System.Drawing.Size(74, 30);
            this.pointerBtn.TabIndex = 15;
            this.pointerBtn.Text = "Pointer";
            this.pointerBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.pointerBtn.UseVisualStyleBackColor = true;
            this.pointerBtn.Click += new System.EventHandler(this.pointerBtn_Click);
            // 
            // eraserBtn
            // 
            this.eraserBtn.AutoSize = true;
            this.eraserBtn.Image = ((System.Drawing.Image)(resources.GetObject("eraserBtn.Image")));
            this.eraserBtn.Location = new System.Drawing.Point(83, 3);
            this.eraserBtn.Name = "eraserBtn";
            this.eraserBtn.Size = new System.Drawing.Size(86, 30);
            this.eraserBtn.TabIndex = 16;
            this.eraserBtn.Text = "Eraser";
            this.eraserBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.eraserBtn.UseVisualStyleBackColor = true;
            this.eraserBtn.Click += new System.EventHandler(this.eraserBtn_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "blank.xml";
            this.openFileDialog1.Filter = "SBML files (.xml)|*.xml|All files|*.*";
            this.openFileDialog1.Title = "Import SBML file";
            // 
            // testBtn
            // 
            this.testBtn.AutoSize = true;
            this.testBtn.ImageIndex = 0;
            this.testBtn.ImageList = this.imageList1;
            this.testBtn.Location = new System.Drawing.Point(175, 3);
            this.testBtn.Name = "testBtn";
            this.testBtn.Size = new System.Drawing.Size(100, 30);
            this.testBtn.TabIndex = 17;
            this.testBtn.Text = "Test model";
            this.testBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.testBtn.UseVisualStyleBackColor = true;
            this.testBtn.Click += new System.EventHandler(this.testBtn_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "test-go.png");
            this.imageList1.Images.SetKeyName(1, "test-stop.png");
            // 
            // stopIcon
            // 
            this.stopIcon.Image = ((System.Drawing.Image)(resources.GetObject("stopIcon.Image")));
            this.stopIcon.InitialImage = null;
            this.stopIcon.Location = new System.Drawing.Point(660, 0);
            this.stopIcon.Name = "stopIcon";
            this.stopIcon.Size = new System.Drawing.Size(31, 27);
            this.stopIcon.TabIndex = 19;
            this.stopIcon.TabStop = false;
            this.stopIcon.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.22662F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 284F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.88983F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.110169F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1055, 646);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.propertyGrid1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(774, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.43201F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.56799F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(278, 581);
            this.tableLayoutPanel3.TabIndex = 22;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.71676F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.28323F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(278, 170);
            this.tableLayoutPanel4.TabIndex = 15;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.pointerBtn);
            this.flowLayoutPanel2.Controls.Add(this.eraserBtn);
            this.flowLayoutPanel2.Controls.Add(this.testBtn);
            this.flowLayoutPanel2.Controls.Add(this.linkBtn);
            this.flowLayoutPanel2.Controls.Add(this.modifierLinkBtn);
            this.flowLayoutPanel2.Controls.Add(this.reactionBtn);
            this.flowLayoutPanel2.Controls.Add(this.ProteinBtn);
            this.flowLayoutPanel2.Controls.Add(this.componentSelectionBox);
            this.flowLayoutPanel2.Controls.Add(this.addComponentBtn);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 21);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(278, 149);
            this.flowLayoutPanel2.TabIndex = 18;
            // 
            // modifierLinkBtn
            // 
            this.modifierLinkBtn.AutoSize = true;
            this.modifierLinkBtn.Image = ((System.Drawing.Image)(resources.GetObject("modifierLinkBtn.Image")));
            this.modifierLinkBtn.Location = new System.Drawing.Point(79, 39);
            this.modifierLinkBtn.Name = "modifierLinkBtn";
            this.modifierLinkBtn.Size = new System.Drawing.Size(101, 30);
            this.modifierLinkBtn.TabIndex = 18;
            this.modifierLinkBtn.Text = "Modifier Link";
            this.modifierLinkBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.modifierLinkBtn.UseVisualStyleBackColor = true;
            this.modifierLinkBtn.Click += new System.EventHandler(this.modifierLinkBtn_Click);
            // 
            // componentSelectionBox
            // 
            this.componentSelectionBox.FormattingEnabled = true;
            this.componentSelectionBox.Location = new System.Drawing.Point(88, 79);
            this.componentSelectionBox.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.componentSelectionBox.Name = "componentSelectionBox";
            this.componentSelectionBox.Size = new System.Drawing.Size(121, 21);
            this.componentSelectionBox.TabIndex = 19;
            // 
            // addComponentBtn
            // 
            this.addComponentBtn.AutoSize = true;
            this.addComponentBtn.Location = new System.Drawing.Point(215, 75);
            this.addComponentBtn.Name = "addComponentBtn";
            this.addComponentBtn.Size = new System.Drawing.Size(60, 30);
            this.addComponentBtn.TabIndex = 20;
            this.addComponentBtn.Text = "Select";
            this.addComponentBtn.UseVisualStyleBackColor = true;
            this.addComponentBtn.Click += new System.EventHandler(this.addComponentBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Tools:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.loadBtn);
            this.flowLayoutPanel1.Controls.Add(this.saveBtn);
            this.flowLayoutPanel1.Controls.Add(this.rearrangeBtn);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(774, 590);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(278, 53);
            this.flowLayoutPanel1.TabIndex = 23;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.trackBar1, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 590);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(765, 53);
            this.tableLayoutPanel2.TabIndex = 24;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "SBML files (.xml)|*.xml|All files|*.*";
            this.saveFileDialog1.Title = "Save to SBML file";
            // 
            // CellDefinitionsPanelUI
            // 
            this.AutoSize = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.stopIcon);
            this.Name = "CellDefinitionsPanelUI";
            this.Size = new System.Drawing.Size(1055, 646);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopIcon)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }
        #region IControllable<CellDefinitionsPanelController> Members

        void IControllable<CellDefinitionsPanelController>.setController(CellDefinitionsPanelController controller)
        {
            this.controller = controller;
        }

        #endregion

        #region ICellDefinitionsPanelUI Members

        public void addCellDefinition(MuCell.Model.CellDefinition cellDefinition)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void setCellDefinition(string cellDefinitionName)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public void editCellDefinition(MuCell.Model.CellDefinition cellDefinition)
        {
            clearSelection();
            this.changeEditMode(EditMode.Pointer,true);
            viewCentre = new Vector2(this.pictureBox1.Width * 0.5f, this.pictureBox1.Height * 0.5f);
            this.Refresh();
        }
        public delegate void RefreshCallBack();
        public void refresh()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new RefreshCallBack(this.Refresh));
            }
            else
            {
                this.Refresh();
            }
        }
        public void clearSelection()
        {
            this.selectedComponents = new List<IModelComponent>();
            propertyGrid1.SelectedObject = null;
        }
        public bool isVisible()
        {
            return this.Visible;
        }
        public void removeSelection()
        {
            CellDefinition definition = controller.getCurrentCellDefinition();

            if (definition != null)
            {
                Model.SBML.Model model = definition.getModel();
                if (model != null)
                {
                    if (selectedComponents.Count > 0)
                    {
                        MacroCommand groupDelete = new MacroCommand();
                        foreach (IModelComponent selectedComponent in selectedComponents)
                        {
                            if (selectedComponent is Species)
                            {
                                Species selectedSpecies = (Species)selectedComponent;

                                MacroCommand aDelete = removeSpecies(model, selectedSpecies);
                                groupDelete.addCommand(aDelete);
                            }
                            else if (selectedComponent is Reaction)
                            {
                                Reaction reaction = (Reaction)selectedComponent;

                                MacroCommand aDelete = removeReaction(model, reaction);
                                groupDelete.addCommand(aDelete);
                            }
                            else if (selectedComponent is ComponentBase)
                            {
                                ComponentBase component = (ComponentBase)selectedComponent;

                                MacroCommand aDelete = removeComponent(model, component);
                                groupDelete.addCommand(aDelete);
                            }
                        }
                        controller.commandPerformed(groupDelete);
                        clearSelection();
                        refresh();
                    }
                }
            }
        }

        private MacroCommand removeReaction(Model.SBML.Model model, Reaction reaction)
        {
            //build a macro command containing any reaction link changes and finally the reaction removal
            MacroCommand groupCommand = new MacroCommand();

            List<SpeciesReference> reactants = reaction.Reactants;
            List<SpeciesReference> products = reaction.Products;
            List<ModifierSpeciesReference> modifiers=reaction.Modifiers;

            foreach (SpeciesReference speciesReference in reactants)
            {
                ReactionLinkCommand linkCommand = new ReactionLinkCommand(speciesReference, reaction, MuCell.View.ReactionLinkCommand.LinkType.Reactant, false);
                groupCommand.addCommand(linkCommand);
            }
            foreach (SpeciesReference speciesReference in products)
            {
                ReactionLinkCommand linkCommand = new ReactionLinkCommand(speciesReference, reaction, MuCell.View.ReactionLinkCommand.LinkType.Product, false);
                groupCommand.addCommand(linkCommand);
            }
            foreach (ModifierSpeciesReference modifierReference in modifiers)
            {
                ModifierLinkCommand linkCommand = new ModifierLinkCommand(modifierReference, reaction, false);
                groupCommand.addCommand(linkCommand);
            }

            ReactionCommand removeCommand = new ReactionCommand(reaction, false, model);
            groupCommand.addCommand(removeCommand);

            groupCommand.doAction();
            return groupCommand;
        }
        private MacroCommand removeComponent(Model.SBML.Model model, ComponentBase component)
        {
            //build a macro command containing any link changes and finally the component removal
            MacroCommand groupCommand = new MacroCommand();

            for (int i = 0; i < component.getReactantNames().Length; i++)
            {
                SpeciesReference reference = component.getSpeciesReference(i, ComponentLinkType.Input);
                if (reference != null)
                {
                    ComponentLinkCommand linkCommand = new ComponentLinkCommand(reference, component, i, ComponentLinkType.Input, false);
                    groupCommand.addCommand(linkCommand);
                }
            }
            for (int i = 0; i < component.getProductNames().Length; i++)
            {
                SpeciesReference reference = component.getSpeciesReference(i, ComponentLinkType.Output);
                if (reference != null)
                {
                    ComponentLinkCommand linkCommand = new ComponentLinkCommand(reference, component, i, ComponentLinkType.Output, false);
                    groupCommand.addCommand(linkCommand);
                }
            }

            ComponentBaseCommand removeCommand = new ComponentBaseCommand(component, false, model);
            groupCommand.addCommand(removeCommand);

            groupCommand.doAction();
            return groupCommand;
        }
        private MacroCommand removeSpecies(Model.SBML.Model model, Species selectedSpecies)
        {
            //build a macro command containing any reaction (or component) link changes and finally the species removal
            MacroCommand groupCommand = new MacroCommand();

            List<Reaction> reactions = model.listOfReactions;
            foreach (Reaction reaction in reactions)
            {
                List<SpeciesReference> reactants = reaction.Reactants;
                List<SpeciesReference> products = reaction.Products;
                List<ModifierSpeciesReference> modifiers = reaction.Modifiers;

                foreach (SpeciesReference speciesReference in reactants)
                {
                    if (speciesReference.species.ID.Equals(selectedSpecies.ID))
                    {
                        ReactionLinkCommand linkCommand = new ReactionLinkCommand(speciesReference, reaction, MuCell.View.ReactionLinkCommand.LinkType.Reactant, false);
                        groupCommand.addCommand(linkCommand);
                    }
                }
                foreach (SpeciesReference speciesReference in products)
                {
                    if (speciesReference.species.ID.Equals(selectedSpecies.ID))
                    {
                        ReactionLinkCommand linkCommand = new ReactionLinkCommand(speciesReference, reaction, MuCell.View.ReactionLinkCommand.LinkType.Product, false);
                        groupCommand.addCommand(linkCommand);
                    }
                }
                foreach(ModifierSpeciesReference modifierReference in modifiers)
                {
                    if (modifierReference.species.ID.Equals(selectedSpecies.ID))
                    {
                        ModifierLinkCommand linkCommand = new ModifierLinkCommand(modifierReference, reaction, false);
                        groupCommand.addCommand(linkCommand);
                    }
                }
            }
            List<ComponentBase> components = model.listOfComponents;

            foreach (ComponentBase component in components)
            {
                for (int i = 0; i < component.getReactantNames().Length; i++)
                {
                    SpeciesReference reference = component.getSpeciesReference(i, ComponentLinkType.Input);
                    if (reference != null && reference.SpeciesID.Equals(selectedSpecies.ID))
                    {
                        ComponentLinkCommand linkCommand = new ComponentLinkCommand(reference, component, i, ComponentLinkType.Input, false);
                        groupCommand.addCommand(linkCommand);
                    }
                }
                for (int i = 0; i < component.getProductNames().Length; i++)
                {
                    SpeciesReference reference = component.getSpeciesReference(i, ComponentLinkType.Output);
                    if (reference != null && reference.SpeciesID.Equals(selectedSpecies.ID))
                    {
                        ComponentLinkCommand linkCommand = new ComponentLinkCommand(reference, component, i, ComponentLinkType.Output, false);
                        groupCommand.addCommand(linkCommand);
                    }
                }
            }

            SpeciesCommand removeCommand = new SpeciesCommand(selectedSpecies, false, model);
            groupCommand.addCommand(removeCommand);

            groupCommand.doAction();
            return groupCommand;
        }
        public float getViewWidth()
        {
            return pictureBox1.Width;
        }
        public float getViewHeight()
        {
            return pictureBox1.Height;
        }
        public void overlayCellInstance(CellInstance cellInstance)
        {
            cellOverlay = cellInstance;
            this.refresh();
        }

        #endregion

        private void drawArrow(IModelComponent start, IModelComponent end, Pen penInit, PaintEventArgs e, ArrowType arrowType)
        {
            System.Drawing.Pen penA = new System.Drawing.Pen(penInit.Color, 1.2f);
            System.Drawing.Pen penB = new System.Drawing.Pen(penInit.Color, 2.2f);
            Vector2 startEdge = start.getClosestPoint(end.getPosition());
            Vector2 endEdge = end.getClosestPoint(start.getPosition());
            //e.Graphics.DrawBezier(pen, startEdge.x, startEdge.y, endEdge.x, endEdge.y, endEdge.x, endEdge.y, endEdge.x, endEdge.y);
            e.Graphics.DrawLine(penA, startEdge.x, startEdge.y, endEdge.x, endEdge.y);

            if (arrowType != ArrowType.LineOnly)
            {
                float xToB = endEdge.x - startEdge.x;
                float yToB = endEdge.y - startEdge.y;
                double angleAway = Math.Atan2(-yToB, -xToB);

                double headAngleChange = Math.PI * 0.125;
                float arrowSize = 8f;
                if (arrowType == ArrowType.Demotion)
                {
                    headAngleChange = Math.PI * 0.5f;
                    arrowSize = 6f;
                }

                for (int i = 0; i < 2; i++)
                {
                    //find angle away, then project for some distance
                    double angleAwayNow = angleAway;
                    if (i == 0)
                    {
                        angleAwayNow += headAngleChange;
                    }
                    else
                    {
                        angleAwayNow -= headAngleChange;
                    }

                    Vector2 arrowOffset = new Vector2((float)Math.Cos(angleAwayNow) * arrowSize, (float)Math.Sin(angleAwayNow) * arrowSize);
                    Vector2 lineEnd = endEdge + arrowOffset;

                    e.Graphics.DrawLine(penB, endEdge.x, endEdge.y, lineEnd.x, lineEnd.y);
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            System.Drawing.Pen pen = new System.Drawing.Pen(Color.Black, 1f);
            Font font = new Font("Arial", 8f);
            Font boldFont = new Font("Arial", 24f, FontStyle.Bold);
            viewTranslate = new Vector2((((pictureBox1.Width * 0.5f) / viewScale) - viewCentre.x) * viewScale, (((pictureBox1.Height * 0.5f) / viewScale) - viewCentre.y) * viewScale);



            e.Graphics.TranslateTransform(viewTranslate.x, viewTranslate.y);
            //e.Graphics.TranslateTransform(-viewCentre.x, -viewCentre.y);
            e.Graphics.ScaleTransform(viewScale, viewScale);
            
            //e.Graphics.DrawBezier(pen, 0, 0, 10, 12, 30, 200, 200, 60);
            if (controller != null)
            {
                CellDefinition definition = controller.getCurrentCellDefinition();


                //get the bounds of the view area in world coordinates so as to draw grid lines
                Vector2 topLeft = getActualMousePosition(new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
                Vector2 bottomRight = getActualMousePosition(new MouseEventArgs(MouseButtons.Left, 0, pictureBox1.Width, pictureBox1.Height, 0));
                float gridSize = 32f;


                bool drawing = false;
                if (definition != null)
                {
                    Model.SBML.Model model = definition.getModel();

                    if (model != null)
                    {
                        drawing = true;
                        //draw the grid first, but only when there's a definition there
                        pen.Color = Color.LightGray;
                        //round the min values down to the nearest grid block
                        float minGridX = (float)Math.Floor(topLeft.x / gridSize) * gridSize;
                        float maxGridX = bottomRight.x;
                        float minGridY = (float)Math.Floor(topLeft.y / gridSize) * gridSize;
                        float maxGridY = bottomRight.y;

                        for (float currentGridX = minGridX; currentGridX < maxGridX; currentGridX += gridSize)
                        {
                            e.Graphics.DrawLine(pen, currentGridX, minGridY, currentGridX, maxGridY);
                        }
                        for (float currentGridY = minGridY; currentGridY < maxGridY; currentGridY += gridSize)
                        {
                            e.Graphics.DrawLine(pen, minGridX, currentGridY, maxGridX, currentGridY);
                        }

                        pen.Color = Color.Black;

                        List<Species> species = model.listOfSpecies;

                        List<Reaction> reactions = model.listOfReactions;

                        List<ComponentBase> components = model.listOfComponents;

                        foreach (Reaction r in reactions)
                        {
                            float reactionX = r.xPosition;
                            float reactionY = r.yPosition;

                            ArrowType arrowType = ArrowType.Promotion;
                            if (r.Reversible)
                            {
                                arrowType = ArrowType.LineOnly;
                            }
                            else
                            {
                                //if the reaction is demotion draw different arrows?
                            }

                            List<SpeciesReference> reactants = r.Reactants;
                            List<SpeciesReference> products = r.Products;
                            List<ModifierSpeciesReference> modifiers=r.Modifiers;
                            pen.Color = Color.Blue;
                            foreach (SpeciesReference reactant in reactants)
                            {
                                //check to see if there is an arrow in the opposite direction
                                bool hasMatching = false;
                                if (r.Reversible)
                                {
                                    hasMatching = true;
                                }
                                else
                                {
                                    foreach (SpeciesReference product in products)
                                    {
                                        if (product.species == reactant.species)
                                        {
                                            hasMatching = true;
                                            break;
                                        }
                                    }
                                }

                                if (hasMatching)
                                {
                                    pen.Color = Color.Purple;
                                }
                                drawArrow(reactant.species, r, pen, e, arrowType);
                                if (hasMatching)
                                {
                                    drawArrow(r, reactant.species, pen, e, arrowType);
                                    pen.Color = Color.Blue;
                                }

                                /*foreach (SpeciesReference product in products)
                                {
                                    //draw line between through some reaction point
                                    e.Graphics.DrawLine(pen, reactant.species.xPosition, reactant.species.yPosition, reactionX, reactionY);
                                    pen.Color = Color.Red;
                                    e.Graphics.DrawLine(pen, reactionX, reactionY,product.species.xPosition,product.species.yPosition);
                                    pen.Color = Color.Black;*/
                                /*if (reactant.species.id==product.species.id)
                                {
                                    //Console.WriteLine("Reactant "+reactant.species.name+" is the same as the product "+product.species.name);
                                    Vector2 lineVector = new Vector2(reactionX-reactant.species.xPosition, reactionY-reactant.species.yPosition);
                                    lineVector.normalise();
                                    lineVector.makePerpendicular();
                                    lineVector = lineVector * 10f;
                                    e.Graphics.DrawBezier(pen, reactant.species.xPosition, reactant.species.yPosition, reactionX-lineVector.x, reactionY-lineVector.y, reactionX+lineVector.x, reactionY+lineVector.y, product.species.xPosition, product.species.yPosition);
                                }
                                else
                                {
                                    e.Graphics.DrawBezier(pen, reactant.species.xPosition, reactant.species.yPosition, reactionX, reactionY, reactionX, reactionY, product.species.xPosition, product.species.yPosition);
                                }
                            }*/
                            }
                            pen.Color = Color.Red;

                            foreach (SpeciesReference product in products)
                            {
                                bool hasMatching = false;

                                foreach (SpeciesReference reactant in reactants)
                                {
                                    if (product.species == reactant.species)
                                    {
                                        hasMatching = true;
                                        break;
                                    }
                                }
                                if ((!hasMatching) && r.Reversible)
                                {
                                    pen.Color = Color.Purple;
                                    drawArrow(product.species, r, pen, e, arrowType);
                                    drawArrow(r, product.species, pen, e, arrowType);
                                    pen.Color = Color.Red;
                                }
                                else if (!hasMatching)
                                {
                                    drawArrow(r, product.species, pen, e, arrowType);
                                }
                            }
                            pen.Color = Color.Green;
                            //dashed lines are nice but cause a known bug in winforms GDI where it's very prone to giving an Out of Memory exception
                            //pen.DashStyle = DashStyle.Dash;

                            foreach (ModifierSpeciesReference modifier in modifiers)
                            {
                                drawArrow(modifier.species, r, pen, e, ArrowType.Promotion);
                            }

                            pen.Color = Color.Black;
                            //pen.DashStyle = DashStyle.Solid;
                        }
                        foreach (ComponentBase component in components)
                        {
                            //draw links from component points to species

                            pen.Color = Color.Red;
                            for (int i = 0; i < component.getReactantNames().Length; i++)
                            {
                                SpeciesReference reference = component.getSpeciesReference(i, ComponentLinkType.Input);
                                if (reference != null)
                                {
                                    drawArrow(reference.species, component.getLinkPoint(i, ComponentLinkType.Input), pen, e, ArrowType.Promotion);
                                }
                            }
                            pen.Color = Color.Blue;
                            for (int i = 0; i < component.getProductNames().Length; i++)
                            {
                                SpeciesReference reference = component.getSpeciesReference(i, ComponentLinkType.Output);
                                if (reference != null)
                                {
                                    drawArrow(component.getLinkPoint(i, ComponentLinkType.Output),reference.species, pen, e, ArrowType.Promotion);
                                }
                            }
                        }
                        pen.Color = Color.Black;

                        //draw an arrow if the user is currently dragging a new reaction link
                        if ((editMode == EditMode.ReactionLinks || editMode==EditMode.ModifierLinks) && leftDown)
                        {
                            foreach (IModelComponent selectedComponent in selectedComponents)
                            {
                                CursorComponent cursor = new CursorComponent(lastMousePos);
                                this.drawArrow(selectedComponent, cursor, pen, e, ArrowType.Promotion);
                            }
                        }

                        //draw a dotted box if the user is dragging with nothing selected (unless control is held down)
                        if (editMode == EditMode.Pointer && leftDown)
                        {
                            if (selectedComponents.Count == 0 || ApplicationUI.ModifierKeys == Keys.Control)
                            {
                                pen.DashStyle = DashStyle.Dot;
                                float topLeftX = moveStart.x;
                                float bottomRightX = lastMousePos.x;
                                float topLeftY = moveStart.y;
                                float bottomRightY = lastMousePos.y;

                                if (topLeftX > bottomRightX)
                                {
                                    float temp = topLeftX;
                                    topLeftX = bottomRightX;
                                    bottomRightX = temp;
                                }
                                if (topLeftY > bottomRightY)
                                {
                                    float temp = topLeftY;
                                    topLeftY = bottomRightY;
                                    bottomRightY = temp;
                                }

                                e.Graphics.DrawRectangle(pen, topLeftX, topLeftY, bottomRightX-topLeftX, bottomRightY-topLeftY);
                                pen.DashStyle = DashStyle.Solid;
                            }
                        }

                        //show where the mouse cursor is when erasing(helpful for debugging view scaling/translation)
                        if (editMode == EditMode.Eraser && leftDown)
                        {
                            float mouseWidth = 4f;
                            float mouseHeight = 4f;
                            e.Graphics.DrawEllipse(pen, lastMousePos.x - (mouseWidth * 0.5f), lastMousePos.y - (mouseHeight * 0.5f), mouseWidth, mouseHeight);
                        }


                        foreach (Species s in species)
                        {
                            float iconWidth = s.getWidth();
                            float iconHeight = s.getHeight();
                            //Console.WriteLine("Drawing species "+s.name);
                            e.Graphics.DrawEllipse(pen, s.xPosition - (iconWidth * 0.5f), s.yPosition - (iconHeight * 0.5f), iconWidth, iconHeight);

                            String speciesLabel = s.ID;

                            if (cellOverlay != null)
                            {
                                speciesLabel += " = " + Decimal.Round((decimal)cellOverlay.getSpeciesAmount(s.ID), 3);
                            }

                            SizeF textSize = e.Graphics.MeasureString(speciesLabel, font);
                            //SizeF boldTextSize = e.Graphics.MeasureString(speciesLabel, boldFont);

                            float stringX=s.xPosition - (textSize.Width * 0.5f);
                            float stringY=(s.yPosition + s.getHeight() * 1.25f) - (textSize.Height * 0.5f);

                            //pen.Color = Color.FromArgb(200, Color.White);
                       
                            //e.Graphics.FillRectangle(pen.Brush, stringX, stringY, textSize.Width, textSize.Height);
                            //e.Graphics.DrawString(speciesLabel, boldFont, pen.Brush, s.xPosition - (boldTextSize.Width * 0.5f), (s.yPosition + s.getHeight() * 1.25f) - (boldTextSize.Height * 0.5f));
                            //pen.Color = Color.Black;

                            e.Graphics.DrawString(speciesLabel, font, pen.Brush,stringX , stringY);

                        }

                        foreach (ComponentBase component in components)
                        {
                            float iconWidth = component.getWidth();
                            float iconHeight = component.getHeight();
                            float xPosition = component.getPosition().x;
                            float yPosition = component.getPosition().y;
                            e.Graphics.DrawEllipse(pen, xPosition - (iconWidth * 0.5f), yPosition - (iconHeight * 0.5f), iconWidth, iconHeight);

                            String componentLabel = component.ID;

                            SizeF textSize = e.Graphics.MeasureString(componentLabel, font);
                            e.Graphics.DrawString(componentLabel, font, pen.Brush, xPosition - (textSize.Width * 0.5f), (yPosition + iconHeight * 1.25f) - (textSize.Height * 0.5f));

                            //components also have link points
                            //loop through the inputs and outputs, drawing each one with label

                            String[] reactantNames = component.getReactantNames();
                            String[] productNames = component.getProductNames();


                            for (int i = 0; i < reactantNames.Length; i++)
                            {
                                String linkLabel = reactantNames[i];
                                Vector2 linkOffset=component.getLinkPointOffset(i, ComponentLinkType.Input);
                                Vector2 linkPosition = component.getPosition() + linkOffset;
                                float linkRadius = component.getLinkPointRadius(i, ComponentLinkType.Input);
                                e.Graphics.DrawEllipse(pen, linkPosition.x - linkRadius, linkPosition.y - linkRadius, linkRadius * 2f, linkRadius * 2f);

                                Vector2 labelPosition = linkPosition - new Vector2(linkRadius, 0f);
                                SizeF labelSize = e.Graphics.MeasureString(linkLabel, font);
                                e.Graphics.DrawString(linkLabel, font, pen.Brush, labelPosition.x - (labelSize.Width), labelPosition.y - (labelSize.Height * 0.5f));
                            }
                            for (int i = 0; i < productNames.Length; i++)
                            {
                                String linkLabel = productNames[i];
                                Vector2 linkOffset = component.getLinkPointOffset(i, ComponentLinkType.Output);
                                Vector2 linkPosition = component.getPosition() + linkOffset;
                                float linkRadius = component.getLinkPointRadius(i, ComponentLinkType.Output);
                                e.Graphics.DrawEllipse(pen, linkPosition.x - linkRadius, linkPosition.y - linkRadius, linkRadius * 2f, linkRadius * 2f);

                                Vector2 labelPosition = linkPosition + new Vector2(linkRadius, 0f);
                                SizeF labelSize = e.Graphics.MeasureString(linkLabel, font);
                                e.Graphics.DrawString(linkLabel, font, pen.Brush, labelPosition.x, labelPosition.y - (labelSize.Height * 0.5f));
                            }
                        }

                        //highlight selected one
                        foreach (IModelComponent selectedComponent in selectedComponents)
                        {
                            float iconWidth = selectedComponent.getWidth() + 6;
                            float iconHeight = selectedComponent.getHeight() + 6;
                            float selectedX = selectedComponent.getPosition().x;
                            float selectedY = selectedComponent.getPosition().y;
                            e.Graphics.DrawEllipse(pen, selectedX - (iconWidth * 0.5f), selectedY - (iconHeight * 0.5f), iconWidth, iconHeight);
                        }

                        //highlight closest relevant component when dragging links
                        IModelComponent relevant=null;
                        if (leftDown)
                        {
                            String selectedID="";
                            if (selectedComponents.Count > 0)
                            {
                                if (selectedComponents[0] is SBase)
                                {
                                    selectedID = ((SBase)selectedComponents[0]).ID;
                                }
                            }

                            if (editMode == EditMode.ModifierLinks)
                            {
                                if (selectedComponents.Count > 0)
                                {
                                    relevant = getClosestOverlappingComponent(model, lastMousePos, false);
                                    if (!(relevant is Reaction))
                                    {
                                        relevant = null;
                                    }
                                    else
                                    {
                                        Reaction reaction = (Reaction)relevant;
                                        foreach (SimpleSpeciesReference reactionSpecies in reaction.Reactants)
                                        {
                                            if (reactionSpecies.SpeciesID.Equals(selectedID))
                                            {
                                                relevant = null;
                                                break;
                                            }
                                        }
                                        if (relevant != null)
                                        {
                                            foreach (SimpleSpeciesReference reactionSpecies in reaction.Products)
                                            {
                                                if (reactionSpecies.SpeciesID.Equals(selectedID))
                                                {
                                                    relevant = null;
                                                    break;
                                                }
                                            }
                                        }
                                        if (relevant != null)
                                        {
                                            foreach (SimpleSpeciesReference reactionSpecies in reaction.Modifiers)
                                            {
                                                if (reactionSpecies.SpeciesID.Equals(selectedID))
                                                {
                                                    relevant = null;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (editMode == EditMode.ReactionLinks)
                            {
                                relevant = getClosestOverlappingComponent(model, lastMousePos, true);
                                if (selectedComponents.Count > 0)
                                {
                                    Reaction linkReaction=null;
                                    Species linkSpecies=null;
                                    if (selectedComponents[0] is ComponentLink)
                                    {
                                        if (relevant is ComponentLink || relevant is Reaction)
                                        {
                                            relevant = null;
                                        }
                                    }
                                    else if (selectedComponents[0] is Species)
                                    {
                                        linkSpecies = (Species)selectedComponents[0];
                                        if (relevant is Species)
                                        {
                                            relevant = null;
                                        }
                                        else if (relevant is Reaction)
                                        {
                                            linkReaction = (Reaction)relevant;
                                        }
                                    }
                                    else if (selectedComponents[0] is Reaction)
                                    {
                                        linkReaction = (Reaction)selectedComponents[0];
                                        if (relevant is Reaction || relevant is ComponentLink)
                                        {
                                            relevant = null;
                                        }
                                        else if (relevant is Species)
                                        {
                                            linkSpecies = (Species)relevant;
                                        }
                                    }

                                    if (linkReaction != null && linkSpecies !=null)
                                    {
                                        foreach (SimpleSpeciesReference modifier in linkReaction.Modifiers)
                                        {
                                            if (modifier.SpeciesID.Equals(selectedID))
                                            {
                                                relevant = null;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (relevant != null)
                            {
                                float iconWidth = relevant.getWidth() + 2;
                                float iconHeight = relevant.getHeight() + 2;
                                float selectedX = relevant.getPosition().x;
                                float selectedY = relevant.getPosition().y;
                                e.Graphics.DrawEllipse(pen, selectedX - (iconWidth * 0.5f), selectedY - (iconHeight * 0.5f), iconWidth, iconHeight);
                            }
                        }

                        foreach (Reaction r in reactions)
                        {
                            float reactionX = r.xPosition;
                            float reactionY = r.yPosition;
                            float reactionWidth = r.getWidth();
                            float reactionHeight = r.getHeight();

                            e.Graphics.DrawEllipse(pen, reactionX - (reactionWidth * 0.5f), reactionY - (reactionHeight * 0.5f), reactionWidth, reactionHeight);

                            String reactionLabel;
                            if (r.KineticLaw != null)
                            {
                                reactionLabel = r.ID + "=" + r.KineticLaw.Formula;
                            }
                            else
                            {
                                reactionLabel = r.ID;
                            }
                            SizeF textSize = e.Graphics.MeasureString(reactionLabel, font);
                            e.Graphics.DrawString(reactionLabel, font, pen.Brush, reactionX - (textSize.Width * 0.5f), (reactionY + r.getHeight() * 2f) - (textSize.Height * 0.5f));


                        }
                    }
                }

                if (!drawing)
                {
                    String message="Select or create a cell definition to begin";
                    e.Graphics.DrawString(message, font, Brushes.Black, viewCentre.x - (e.Graphics.MeasureString(message, font).Width * 0.5f), viewCentre.y - (e.Graphics.MeasureString(message, font).Height * 0.5f));
                }
            }
        }
        private IModelComponent getClosestOverlappingComponent(Model.SBML.Model model, Vector2 mousePos, bool onlySubComponents)
        {
            IModelComponent selectedComponent = null;
            float closest = float.MaxValue;
            List<Species> species = model.listOfSpecies;
            foreach (Species s in species)
            {
                float iconWidth = s.getWidth() + 12f;

                Vector2 pos = s.getPosition();
                float dist = (float)Vector2.getDistance(mousePos, pos);

                if (dist < iconWidth && dist < closest)
                {
                    selectedComponent = s;
                    closest = dist;
                }
            }
            List<Reaction> reactions = model.listOfReactions;
            foreach (Reaction r in reactions)
            {
                float reactionWidth = r.getWidth()+12f;

                Vector2 pos = r.getPosition();
                float dist = (float)Vector2.getDistance(mousePos, pos);

                if (dist < reactionWidth && dist < closest)
                {
                    selectedComponent = r;
                    closest = dist;
                }
            }

            List<ComponentBase> components = model.listOfComponents;
            foreach (ComponentBase c in components)
            {
                float iconWidth = c.getWidth() + 12f;
                Vector2 pos = c.getPosition();



                if (onlySubComponents)
                {
                    for (int i = 0; i < c.getReactantNames().Length; i++)
                    {
                        Vector2 linkPos = c.getLinkPointOffset(i, ComponentLinkType.Input) + pos;
                        float linkRadius = (c.getLinkPointRadius(i, ComponentLinkType.Input)*2f)+12f;
                        float linkDist = (float)Vector2.getDistance(mousePos, linkPos);

                        if (linkDist < linkRadius && linkDist < closest)
                        {
                            selectedComponent = c.getLinkPoint(i, ComponentLinkType.Input);
                            closest = linkDist;
                        }
                    }
                    for (int i = 0; i < c.getProductNames().Length; i++)
                    {
                        Vector2 linkPos = c.getLinkPointOffset(i, ComponentLinkType.Output) + pos;
                        float linkRadius = (c.getLinkPointRadius(i, ComponentLinkType.Output)*2f)+12f;
                        float linkDist = (float)Vector2.getDistance(mousePos, linkPos);

                        if (linkDist < linkRadius && linkDist < closest)
                        {
                            selectedComponent = c.getLinkPoint(i, ComponentLinkType.Output);
                            closest = linkDist;
                        }
                    }
                }
                else
                {
                    float dist = (float)Vector2.getDistance(mousePos, pos);
                    if (dist < iconWidth && dist < closest)
                    {
                        selectedComponent = c;
                        closest = dist;
                    }
                }
            }

            return selectedComponent;
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
             //find closest overlapping model component and store
            Vector2 mousePos = getActualMousePosition(e);
            //do any translation needed
            lastMousePos = mousePos;
            
            CellDefinition definition = controller.getCurrentCellDefinition();

            if (definition != null)
            {
                Model.SBML.Model model = definition.getModel();
                if (model != null)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        leftDown = true;
                        moveStart = mousePos;
                        totalMouseMove = new Vector2(0f, 0f);
                        if (editMode == EditMode.Pointer || editMode == EditMode.ReactionLinks || editMode==EditMode.ModifierLinks)
                        {
                            if (!((ApplicationUI.ModifierKeys == Keys.Control || selectedComponents.Count>1) && editMode==EditMode.Pointer))
                            {
                                clearSelection();
                            }
                            IModelComponent selectedComponent = getClosestOverlappingComponent(model, mousePos,editMode!=EditMode.Pointer);

                            if (selectedComponent != null)
                            {
                                moveStart = mousePos;
                                if (!selectedComponents.Contains(selectedComponent))
                                {
                                    if (!(editMode == EditMode.ModifierLinks && !(selectedComponent is Species)))
                                    {
                                        if (selectedComponent is Species)
                                        {
                                            propertyGrid1.SelectedObject = (Species)selectedComponent;
                                        }
                                        else if (selectedComponent is Reaction)
                                        {
                                            propertyGrid1.SelectedObject = (Reaction)selectedComponent;
                                        }
                                        else if (selectedComponent is ComponentBase)
                                        {
                                            propertyGrid1.SelectedObject = (ComponentBase)selectedComponent;
                                        }


                                        selectedComponents.Add(selectedComponent);

                                        propertyGrid1.Refresh();
                                    }
                                }
                                else if(ApplicationUI.ModifierKeys == Keys.Control)
                                {
                                    selectedComponents.Remove(selectedComponent);
                                    if (propertyGrid1.SelectedObject == selectedComponent)
                                    {
                                        propertyGrid1.SelectedObject = null;
                                        propertyGrid1.Refresh();
                                    }
                                }
                            }
                            else
                            {
                                if (!(ApplicationUI.ModifierKeys == Keys.Control))
                                {
                                    clearSelection();
                                }
                                if (editMode == EditMode.Pointer)
                                {
                                    //start drag selection
                                    moveStart = mousePos;
                                }
                            }
                        }
                        else if (editMode == EditMode.AddSpecies)
                        {
                            clearSelection();
                            String defaultID = "Default";
                            String idToUse = defaultID;
                            int idSuffix = 1;
                            while (model.findObject(idToUse) != null)
                            {
                                idToUse = defaultID + idSuffix;
                                idSuffix++;
                            }

                            Species newSpecies = new Species(idToUse);
                            newSpecies.InitialConcentration = -1;
                            newSpecies.setPosition(mousePos.x, mousePos.y);
                            SpeciesCommand addSpeciesCommand = new SpeciesCommand(newSpecies, true, model);
                            addSpeciesCommand.doAction();
                            controller.commandPerformed(addSpeciesCommand);

                            changeEditMode(EditMode.Pointer, true);
                        }
                        else if (editMode == EditMode.AddReactions)
                        {
                            clearSelection();
                            String defaultID = "R";

                            int idSuffix = 1;
                            String idToUse = defaultID + idSuffix;
                            while (model.findObject(idToUse) != null)
                            {
                                idSuffix++;
                                idToUse = defaultID + idSuffix;
                            }

                            Reaction newReaction = new Reaction(idToUse);
                            newReaction.setPosition(mousePos.x, mousePos.y);

                            newReaction.KineticLaw = new KineticLaw(model);
                            ReactionCommand addReactionCommand = new ReactionCommand(newReaction, true, model);
                            addReactionCommand.doAction();
                            controller.commandPerformed(addReactionCommand);

                            changeEditMode(EditMode.Pointer, true);
                        }
                        else if (editMode == EditMode.AddComponents)
                        {
                            clearSelection();
                            String componentType = (String)componentSelectionBox.SelectedItem;

                            String defaultID = componentType;
                            String idToUse = defaultID;
                            int idSuffix = 1;
                            while (model.findObject(idToUse) != null)
                            {
                                idToUse = defaultID + idSuffix;
                                idSuffix++;
                            }

                            ComponentBase component = ComponentBase.ComponentFactory(componentType);
                            component.ID = idToUse;
                            component.setPosition(mousePos.x, mousePos.y);

                            ComponentBaseCommand command = new ComponentBaseCommand(component, true, model);
                            command.doAction();
                            controller.commandPerformed(command);

                            changeEditMode(EditMode.Pointer, true);
                        }
                        else if (editMode == EditMode.Eraser)
                        {
                            if (eraseArea(mousePos, model))
                            {
                                clearSelection();
                            }
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        rightDown = true;
                    }
                }
            }
            this.refresh();
        }
        private Vector2 getActualMousePosition(MouseEventArgs e)
        {
            return new Vector2(((e.X / viewScale) - ((((pictureBox1.Width * 0.5f) / viewScale) - viewCentre.x))), ((e.Y / viewScale) - ((((pictureBox1.Height * 0.5f) / viewScale) - viewCentre.y))));
        }
        private bool eraseArea(Vector2 position, Model.SBML.Model model)
        {
            float eraseRadius = 4f;
            bool erasedAnything = false;

            List<Reaction> reactions = model.listOfReactions;
            Reaction closestLinkReaction = null;
            SimpleSpeciesReference closestLinkSpeciesReference = null;
            ReactionLinkCommand.LinkType closestLinkType = ReactionLinkCommand.LinkType.Reactant;
            double closestLinkDistance = double.MaxValue;
            foreach (Reaction r in reactions)
            {
                //do circle to line intersection test for reaction links

                List<SpeciesReference> reactants = r.Reactants;
                List<SpeciesReference> products = r.Products;
                List<ModifierSpeciesReference> modifiers = r.Modifiers;

                foreach (SpeciesReference speciesReference in reactants)
                {
                    double lineDistance=Vector2.lineCircleDistance(r.getPosition(), speciesReference.species.getPosition(), position);
                    if (lineDistance < eraseRadius && lineDistance<closestLinkDistance)
                    {
                        closestLinkDistance = lineDistance;
                        closestLinkReaction = r;
                        closestLinkSpeciesReference = speciesReference;
                        closestLinkType = ReactionLinkCommand.LinkType.Reactant;
                    }
                }
                foreach (SpeciesReference speciesReference in products)
                {
                    double lineDistance = Vector2.lineCircleDistance(r.getPosition(), speciesReference.species.getPosition(), position);
                    if (lineDistance < eraseRadius && lineDistance < closestLinkDistance)
                    {
                        closestLinkDistance = lineDistance;
                        closestLinkReaction = r;
                        closestLinkSpeciesReference = speciesReference;
                        closestLinkType = ReactionLinkCommand.LinkType.Product;
                    }
                }
                foreach (ModifierSpeciesReference speciesReference in modifiers)
                {
                    double lineDistance = Vector2.lineCircleDistance(r.getPosition(), speciesReference.species.getPosition(), position);
                    if (lineDistance < eraseRadius && lineDistance < closestLinkDistance)
                    {
                        closestLinkDistance = lineDistance;
                        closestLinkReaction = r;
                        closestLinkSpeciesReference = speciesReference;
                        closestLinkType = ReactionLinkCommand.LinkType.Modifier;
                    }
                }
            }

            if (closestLinkReaction != null && closestLinkSpeciesReference!=null)
            {
                if (closestLinkSpeciesReference is ModifierSpeciesReference)
                {
                    ModifierLinkCommand linkCommand = new ModifierLinkCommand((ModifierSpeciesReference)closestLinkSpeciesReference, closestLinkReaction, false);
                    linkCommand.doAction();
                    controller.commandPerformed(linkCommand);
                    return true;
                }
                else if (closestLinkSpeciesReference is SpeciesReference)
                {
                    ReactionLinkCommand linkCommand = new ReactionLinkCommand((SpeciesReference)closestLinkSpeciesReference, closestLinkReaction, closestLinkType, false);
                    linkCommand.doAction();
                    controller.commandPerformed(linkCommand);
                    return true;
                }
            }


            foreach (Reaction r in reactions)
            {
                double distance = Vector2.getDistance(position, r.getPosition());
                if (distance < (r.getWidth()*0.5) + eraseRadius)
                {
                    controller.commandPerformed(removeReaction(model, r));
                    return true;
                }
            }

            List<Species> species = model.listOfSpecies;


            foreach (Species s in species)
            {
                double distance = Vector2.getDistance(position, s.getPosition());
                if (distance < (s.getWidth()*0.5)+eraseRadius)
                {
                    controller.commandPerformed(removeSpecies(model, s));
                    return true;
                }
            }

            List<ComponentBase> components = model.listOfComponents;

            ComponentLink closestLinkPoint = null;
            closestLinkSpeciesReference = null;
            closestLinkDistance = double.MaxValue;
            foreach (ComponentBase c in components)
            {
                //do circle to line intersection test for component links
                SpeciesReference[] reactants=c.Reactants;
                SpeciesReference[] products=c.Products;
                for (int i = 0; i < reactants.Length; i++)
                {
                    SpeciesReference reference = reactants[i];
                    ComponentLink linkPoint = c.getLinkPoint(i, ComponentLinkType.Input);
                    if (reference != null)
                    {
                        double lineDistance = Vector2.lineCircleDistance(linkPoint.getPosition(), reference.species.getPosition(), position);
                        if (lineDistance < eraseRadius && lineDistance < closestLinkDistance)
                        {
                            closestLinkDistance = lineDistance;
                            closestLinkPoint = linkPoint;
                            closestLinkSpeciesReference = reference;
                        }
                    }
                }
                for (int i = 0; i < products.Length; i++)
                {
                    SpeciesReference reference = products[i];
                    ComponentLink linkPoint = c.getLinkPoint(i, ComponentLinkType.Output);
                    if (reference != null)
                    {
                        double lineDistance = Vector2.lineCircleDistance(linkPoint.getPosition(), reference.species.getPosition(), position);
                        if (lineDistance < eraseRadius && lineDistance < closestLinkDistance)
                        {
                            closestLinkDistance = lineDistance;
                            closestLinkPoint = linkPoint;
                            closestLinkSpeciesReference = reference;
                        }
                    }
                }
            }

            if (closestLinkPoint != null && closestLinkSpeciesReference != null)
            {
                ComponentLinkCommand linkCommand = new ComponentLinkCommand((SpeciesReference)closestLinkSpeciesReference, closestLinkPoint.getParent(), closestLinkPoint.getLinkNumber(), closestLinkPoint.getLinkType(), false);
                linkCommand.doAction();
                controller.commandPerformed(linkCommand);
                return true;
            }

            foreach (ComponentBase component in components)
            {
                double distance = Vector2.getDistance(position, component.getPosition());
                if (distance < (component.getWidth() * 0.5) + eraseRadius)
                {
                    controller.commandPerformed(removeComponent(model,component));
                    return true;
                }
            }


            return erasedAnything;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
            Vector2 mousePos = getActualMousePosition(e);
            //Console.WriteLine("Mouse moved to "+mousePos.x+","+mousePos.y+" from "+lastMousePos.x+","+lastMousePos.y);
            //do any translation needed
            Vector2 mouseChange = mousePos - lastMousePos;
            lastMousePos = mousePos;
            if (leftDown & !rightDown)
            {
                if (editMode == EditMode.Pointer)
                {
                    if (!(ApplicationUI.ModifierKeys == Keys.Control))
                    {
                        foreach (IModelComponent selectedComponent in selectedComponents)
                        {
                            if (mouseChange.getLength() > 0)
                            {
                                
                                selectedComponent.setPosition(selectedComponent.getPosition().x + mouseChange.x, selectedComponent.getPosition().y + mouseChange.y);

                            }
                        }
                        totalMouseMove += mouseChange;
                        //Console.WriteLine("Total mouse move = " + totalMouseMove.x + "," + totalMouseMove.y);
                    }
                    pictureBox1.Refresh();
                }
                else if (editMode == EditMode.ReactionLinks || editMode == EditMode.ModifierLinks)
                {
                    if (selectedComponents.Count>0)
                    {
                        pictureBox1.Refresh();
                    }
                }
                else if (editMode == EditMode.Eraser)
                {
                    CellDefinition definition = controller.getCurrentCellDefinition();

                    if (definition != null)
                    {
                        Model.SBML.Model model = definition.getModel();
                        if (model != null)
                        {
                            bool erasedAnything = eraseArea(mousePos, model);
                            if (erasedAnything)
                            {
                                clearSelection();
                            }
                            this.refresh();
                        }
                    }
                }
            }
            if (rightDown)
            {
                viewCentre -= mouseChange;
                lastMousePos -=mouseChange;
                /*if (leftDown)
                {
                    if (editMode == EditMode.Pointer)
                    {
                        foreach (IModelComponent selectedComponent in selectedComponents)
                        {
                            if (mouseChange.getLength() > 0)
                            {
                                
                                selectedComponent.setPosition(selectedComponent.getPosition().x - mouseChange.x, selectedComponent.getPosition().y - mouseChange.y);
                            }
                        }
                    }
                }*/
                this.refresh();
            }
        }
        private void addBoxSelection(Vector2 topLeft, Vector2 bottomRight, Model.SBML.Model model)
        {
            //find everything completely inside box, add if not already selected


            //put everything in one list to avoid code duplication
            List<IModelComponent> allComponents = new List<IModelComponent>();
            foreach (Species s in model.listOfSpecies)
            {
                allComponents.Add(s);
            }
            foreach (Reaction r in model.listOfReactions)
            {
                allComponents.Add(r);
            }
            foreach (ComponentBase c in model.listOfComponents)
            {
                allComponents.Add(c);
            }

            
            foreach (IModelComponent c in allComponents)
            {
                if (c.getPosition().x - (c.getWidth() * 0.5f) < topLeft.x)
                {
                    continue;
                }
                else if (c.getPosition().x + (c.getWidth() * 0.5f) > bottomRight.x)
                {
                    continue;
                }
                else if (c.getPosition().y - (c.getHeight() * 0.5f) < topLeft.y)
                {
                    continue;
                }
                else if (c.getPosition().y + (c.getHeight() * 0.5f) > bottomRight.y)
                {
                    continue;
                }
                else
                {
                    if (!selectedComponents.Contains(c))
                    {
                        selectedComponents.Add(c);
                    }
                }
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Vector2 mousePos = getActualMousePosition(e);
            lastMousePos = mousePos;
            if (e.Button == MouseButtons.Left)
            {
                leftDown = false;

                CellDefinition definition = controller.getCurrentCellDefinition();

                if (definition != null)
                {
                    Model.SBML.Model model = definition.getModel();
                    if (model != null)
                    {
                        if (editMode == EditMode.Pointer)
                        {
                            if (selectedComponents.Count > 0)
                            {
                                if (totalMouseMove.x != 0 || totalMouseMove.y != 0)
                                {
                                    MacroCommand groupMove = new MacroCommand();
                                    foreach (IModelComponent selectedComponent in selectedComponents)
                                    {
                                        MoveComponentCommand command = new MoveComponentCommand(selectedComponent, selectedComponent.getPosition() - totalMouseMove, selectedComponent.getPosition());
                                        groupMove.addCommand(command);
                                    }
                                    controller.commandPerformed(groupMove);
                                }
                            }
                            if(selectedComponents.Count==0 ||(ApplicationUI.ModifierKeys == Keys.Control))
                            {
                                //select everything in the box
                                float topLeftX = moveStart.x;
                                float bottomRightX = lastMousePos.x;
                                float topLeftY = moveStart.y;
                                float bottomRightY = lastMousePos.y;

                                if (topLeftX > bottomRightX)
                                {
                                    float temp = topLeftX;
                                    topLeftX = bottomRightX;
                                    bottomRightX = temp;
                                }
                                if (topLeftY > bottomRightY)
                                {
                                    float temp = topLeftY;
                                    topLeftY = bottomRightY;
                                    bottomRightY = temp;
                                }

                                addBoxSelection(new Vector2(topLeftX, topLeftY), new Vector2(bottomRightX, bottomRightY),model);
                            }
                        }
                        else if (editMode == EditMode.ReactionLinks)
                        {
                            if (selectedComponents.Count==1)
                            {
                                IModelComponent selectedComponent = selectedComponents[0];
                                //find closest component, try and link
                                IModelComponent closestComponent = getClosestOverlappingComponent(model, mousePos,true);

                                if (closestComponent != null)
                                {
                                    if (closestComponent is Reaction && selectedComponent is Species)
                                    {
                                        Species a = (Species)selectedComponent;
                                        Reaction b = (Reaction)closestComponent;

                                        //check if there's a link there already (in the same direction or either if the reaction is reversible)
                                        bool linkExists = false;
                                        List<SpeciesReference> reactants = b.Reactants;
                                        foreach (SpeciesReference speciesReference in reactants)
                                        {
                                            if (speciesReference.species.ID.Equals(a.ID))
                                            {
                                                linkExists = true;
                                                break;
                                            }
                                        }
                                        List<ModifierSpeciesReference> modifiers = b.Modifiers;
                                        foreach (ModifierSpeciesReference speciesReference in modifiers)
                                        {
                                            if (speciesReference.species.ID.Equals(a.ID))
                                            {
                                                linkExists = true;
                                                break;
                                            }
                                        }
                                        //if the reaction is reversible (so 2 way already), then the species only needs one link
                                        if (b.Reversible)
                                        {
                                            List<SpeciesReference> products = b.Products;
                                            foreach (SpeciesReference speciesReference in products)
                                            {
                                                if (speciesReference.species.ID.Equals(a.ID))
                                                {
                                                    linkExists = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (!linkExists)
                                        {
                                            ReactionLinkCommand command = new ReactionLinkCommand(new SpeciesReference(a, 1), b, MuCell.View.ReactionLinkCommand.LinkType.Reactant, true);
                                            command.doAction();
                                            controller.commandPerformed(command);
                                        }
                                    }
                                    else if (closestComponent is Species && selectedComponent is Reaction)
                                    {
                                        Reaction a = (Reaction)selectedComponent;
                                        Species b = (Species)closestComponent;

                                        //check if there's a link there already (in the same direction or either if the reaction is reversible)
                                        bool linkExists = false;

                                        List<SpeciesReference> products = a.Products;
                                        foreach (SpeciesReference speciesReference in products)
                                        {
                                            if (speciesReference.species.ID.Equals(b.ID))
                                            {
                                                linkExists = true;
                                                break;
                                            }
                                        }
                                        List<ModifierSpeciesReference> modifiers = a.Modifiers;
                                        foreach (ModifierSpeciesReference speciesReference in modifiers)
                                        {
                                            if (speciesReference.species.ID.Equals(b.ID))
                                            {
                                                linkExists = true;
                                                break;
                                            }
                                        }

                                        //if the reaction is reversible (so 2 way already), then the species only needs one link
                                        if (a.Reversible)
                                        {
                                            List<SpeciesReference> reactants = a.Reactants;
                                            foreach (SpeciesReference speciesReference in reactants)
                                            {
                                                if (speciesReference.species.ID.Equals(b.ID))
                                                {
                                                    linkExists = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (!linkExists)
                                        {
                                            ReactionLinkCommand command = new ReactionLinkCommand(new SpeciesReference(b, 1), a, MuCell.View.ReactionLinkCommand.LinkType.Product, true);
                                            command.doAction();
                                            controller.commandPerformed(command);
                                        }
                                    }
                                    else if ((closestComponent is Species && selectedComponent is ComponentLink) || (closestComponent is ComponentLink && selectedComponent is Species))
                                    {
                                        ComponentLink linkComponent;
                                        Species linkSpecies;
                                        if (closestComponent is Species)
                                        {
                                            linkSpecies = (Species)closestComponent;
                                            linkComponent = (ComponentLink)selectedComponent;
                                        }
                                        else
                                        {
                                            linkSpecies = (Species)selectedComponent;
                                            linkComponent = (ComponentLink)closestComponent;
                                        }

                                        //check to see if there is blank space, so then you can create and do ComponentLinkCommand
                                        ComponentBase component = linkComponent.getParent();

                                        SpeciesReference currentReference = component.getSpeciesReference(linkComponent.getLinkNumber(), linkComponent.getLinkType());
                                        if (currentReference == null)
                                        {
                                            ComponentLinkCommand command = new ComponentLinkCommand(new SpeciesReference(linkSpecies, 1), component, linkComponent.getLinkNumber(), linkComponent.getLinkType(), true);
                                            command.doAction();
                                            controller.commandPerformed(command);
                                        }

                                    }
                                }
                            }
                        }
                        else if (editMode == EditMode.ModifierLinks)
                        {
                            if (selectedComponents.Count == 1)
                            {
                                IModelComponent selectedComponent = selectedComponents[0];
                                //find closest component, try and link
                                IModelComponent closestComponent = getClosestOverlappingComponent(model, mousePos,false);

                                if (closestComponent != null)
                                {
                                    if (closestComponent is Reaction && selectedComponent is Species)
                                    {
                                        Species a = (Species)selectedComponent;
                                        Reaction b = (Reaction)closestComponent;

                                        //check if it's a modifier already
                                        bool linkExists = false;
                                        List<ModifierSpeciesReference> modifiers = b.Modifiers;
                                        foreach (ModifierSpeciesReference modifierReference in modifiers)
                                        {
                                            if (modifierReference.species.ID.Equals(a.ID))
                                            {
                                                linkExists = true;
                                                break;
                                            }
                                        }
                                        //probably shouldn't let user add a species as a modifier if it's already a reactant or product
                                        List<SpeciesReference> products = b.Products;
                                        foreach (SpeciesReference speciesReference in products)
                                        {
                                            if (speciesReference.species.ID.Equals(a.ID))
                                            {
                                                linkExists = true;
                                                break;
                                            }
                                        }
                                        List<SpeciesReference> reactants = b.Reactants;
                                        foreach (SpeciesReference speciesReference in reactants)
                                        {
                                            if (speciesReference.species.ID.Equals(a.ID))
                                            {
                                                linkExists = true;
                                                break;
                                            }
                                        }
                                        if (!linkExists)
                                        {
                                            ModifierLinkCommand command = new ModifierLinkCommand(new ModifierSpeciesReference(a), b, true);
                                            command.doAction();
                                            controller.commandPerformed(command);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                rightDown = false;
            }
            this.refresh();
        }
        public void changeEditMode(EditMode newMode, bool internalCall)
        {
            if (editMode == EditMode.Test)
            {
                if (internalCall)
                {
                    controller.stopTestModel();
                }
                else
                {
                    // swap image with the hidden button
                    Image temp = testBtn.Image;
                    testBtn.Image = this.stopIcon.Image;
                    testBtn.Text = "Test Model";
                    this.stopIcon.Image = temp;
                }
            }
            if (editMode == EditMode.AutoRearrange)
            {
                if (internalCall)
                {
                    //stop the rearranging thread and call back here later
                    controller.stopAutoRearranging();
                }
                else
                {
                    //so the reset the button
                    rearrangeBtn.Text = "Rearrange";
                }
            }
            editMode = newMode;
            if (editMode == EditMode.AddReactions || editMode == EditMode.AddSpecies || editMode==EditMode.AddComponents)
            {
                pictureBox1.Cursor = Cursors.Cross;
            }
            else if (editMode == EditMode.Eraser)
            {
                pictureBox1.Cursor = Cursors.Arrow;
            }
            else if (editMode == EditMode.Test)
            {
                pictureBox1.Cursor = Cursors.No;
                // swap image with the hidden button
                Image temp = testBtn.Image;
                testBtn.Image = this.stopIcon.Image;
                testBtn.Text = "Stop test";
                this.stopIcon.Image = temp;
            }
            else if (editMode == EditMode.AutoRearrange)
            {
                pictureBox1.Cursor = Cursors.No;
                rearrangeBtn.Text = "Stop auto-rearrange";
            }
            else if (editMode == EditMode.Pointer)
            {
                pictureBox1.Cursor = Cursors.Default;
            }
            else
            {
                pictureBox1.Cursor = Cursors.Hand;
            }

            //show current mode's button as disabled
            reactionBtn.Enabled = true;
            ProteinBtn.Enabled = true;
            eraserBtn.Enabled = true;
            pointerBtn.Enabled = true;
            linkBtn.Enabled = true;
            modifierLinkBtn.Enabled = true;
            addComponentBtn.Enabled = true;
            testBtn.Enabled = true;
            rearrangeBtn.Enabled = true;
            switch (editMode)
            {
                case EditMode.AddReactions:
                    reactionBtn.Enabled = false;
                    break;
                case EditMode.AddSpecies:
                    ProteinBtn.Enabled = false;
                    break;
                case EditMode.Eraser:
                    eraserBtn.Enabled = false;
                    break;
                case EditMode.Pointer:
                    pointerBtn.Enabled = false;
                    break;
                case EditMode.ReactionLinks:
                    linkBtn.Enabled = false;
                    break;
                case EditMode.ModifierLinks:
                    modifierLinkBtn.Enabled = false;
                    break;
                case EditMode.AddComponents:
                    addComponentBtn.Enabled = false;
                    break;
                case EditMode.Test:
                    rearrangeBtn.Enabled = false;
                    break;
                case EditMode.AutoRearrange:
                    testBtn.Enabled = false;
                    break;
            }
        }

        private void ProteinBtn_Click(object sender, EventArgs e)
        {
            changeEditMode(EditMode.AddSpecies,true);
        }

        private void pointerBtn_Click(object sender, EventArgs e)
        {
            changeEditMode(EditMode.Pointer,true);
        }

        private void reactionBtn_Click(object sender, EventArgs e)
        {
            changeEditMode(EditMode.AddReactions,true);
        }

        private void linkBtn_Click(object sender, EventArgs e)
        {
            changeEditMode(EditMode.ReactionLinks,true);
        }
        private void modifierLinkBtn_Click(object sender, EventArgs e)
        {
            changeEditMode(EditMode.ModifierLinks, true);
        }

        private void eraserBtn_Click(object sender, EventArgs e)
        {
            changeEditMode(EditMode.Eraser,true);
        }

        private void addComponentBtn_Click(object sender, EventArgs e)
        {
            changeEditMode(EditMode.AddComponents, true);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            viewScale = 0.5f+(0.1f * trackBar1.Value);
            this.refresh();
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Console.WriteLine("Property changing");
            if (e.ChangedItem.GridItemType == GridItemType.Property)
            {
                Console.WriteLine("Property=" + e.ChangedItem.PropertyDescriptor.Name);

                CellDefinition definition = controller.getCurrentCellDefinition();

                if (definition != null)
                {
                    Model.SBML.Model model = definition.getModel();
                    if (model != null)
                    {
                        if (selectedComponents.Count == 1)
                        {
                            IModelComponent component = selectedComponents[0];

                            MacroCommand propertyChangeCommands = new MacroCommand();
                            propertyChangeCommands.addCommand(new PropertyChangeCommand(component, e.ChangedItem.PropertyDescriptor.Name, e.ChangedItem.Value, e.OldValue));

                            if (e.ChangedItem.PropertyDescriptor.Name.Equals("ID"))
                            {
                                //have to try renaming it manually before making the command

                                SBase entity = (SBase)component;
                                String oldID = (String)e.OldValue;
                                String newID = (String)e.ChangedItem.Value;
                                String olderID = entity.getOldID();

                                //set the right name order and update id
                                entity.ID = oldID;
                                entity.ID = newID;
                                try
                                {
                                    model.updateID(entity);

                                    RenameEntityCommand renameCommand = new RenameEntityCommand((SBase)component, (String)e.OldValue, (String)e.ChangedItem.Value, model);
                                    propertyChangeCommands.addCommand(renameCommand);
                                }
                                catch (DuplicateSBMLObjectIdException exception)
                                {
                                    MessageBox.Show("Entity ID '"+newID+"' already in use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    entity.ID = olderID;
                                    entity.ID = oldID;
                                    propertyGrid1.Refresh();
                                    return;
                                }

                                
                            }
                            else if (e.ChangedItem.PropertyDescriptor.Name.Equals("Formula"))
                            {
                                //only a reaction should have a formula
                                if (component is Reaction)
                                {
                                    Reaction reaction = (Reaction)component;
                                    //reaction.GetType().GetProperty("ID").SetValue(reaction,null,null)

                                    //check to see what identifiers have been parsed, and link up any unlinked species, or ask to add unknowns as parameters

                                    List<Species> reactionSpecies = reaction.KineticLaw.SpeciesFromFormula();
                                    foreach (Species species in reactionSpecies)
                                    {
                                        bool isLinked = false;
                                        List<SpeciesReference> allReferences = new List<SpeciesReference>(reaction.Reactants);
                                        allReferences.AddRange(reaction.Products);
                                        foreach (SpeciesReference reference in allReferences)
                                        {
                                            if (reference.SpeciesID.Equals(species.ID))
                                            {
                                                isLinked = true;
                                                break;
                                            }
                                        }
                                        foreach (ModifierSpeciesReference reference in reaction.Modifiers)
                                        {
                                            if (reference.SpeciesID.Equals(species.ID))
                                            {
                                                isLinked = true;
                                                break;
                                            }
                                        }

                                        if (!isLinked)
                                        {
                                            ReactionLinkCommand command = new ReactionLinkCommand(new SpeciesReference(species, 1), reaction, MuCell.View.ReactionLinkCommand.LinkType.Reactant, true);
                                            command.doAction();
                                            propertyChangeCommands.addCommand(command);
                                        }
                                    }
                                    List<UnknownEntity> unknowns = reaction.KineticLaw.UnknownEntitiesFromFormula();

                                    foreach (UnknownEntity unknown in unknowns)
                                    {
                                        Parameter parameter = new Parameter();
                                        parameter.ID = unknown.ID;
                                        parameter.Value = 1;
                                        parameter.Constant = true;

                                        MessageBox.Show("Adding unknown identifier " + parameter.ID + " as a parameter in the reaction","Formula Parsing message",MessageBoxButtons.OK,MessageBoxIcon.Information);

                                        ParameterCommand addParameterCommand = new ParameterCommand(parameter, reaction,model);
                                        addParameterCommand.doAction();
                                        propertyChangeCommands.addCommand(addParameterCommand);
                                    }

                                    reaction.Formula = reaction.Formula;
                                }
                            }

                            controller.commandPerformed(propertyChangeCommands);
                        }
                    }
                }

            }
            this.refresh();
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            if (controller.getCurrentCellDefinition() != null)
            {
                openFileDialog1.InitialDirectory = System.Environment.CurrentDirectory;
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    String fileName = openFileDialog1.FileName;
                    if (fileName != null && fileName.Length > 0)
                    {
                        bool success = controller.getCurrentCellDefinition().addSBMLModel(fileName);
                        if (success)
                        {
                            controller.showDefinition(controller.getCurrentCellDefinition());
                        }
                    }
                }
            }
        }
        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (controller.getCurrentCellDefinition() != null)
            {
                saveFileDialog1.InitialDirectory = System.Environment.CurrentDirectory;
                DialogResult result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    String fileName = saveFileDialog1.FileName;
                    if (fileName != null && fileName.Length > 0)
                    {
                        try
                        {
                            bool success = controller.getCurrentCellDefinition().getModel().saveToXml(fileName);
                            if (success)
                            {
                                controller.getCurrentCellDefinition().LastSBMLPath = fileName;
                                MessageBox.Show("File successfully saved to " + fileName, "File Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch(Exception exception)
                        {
                            MessageBox.Show(exception.Message+"\n"+exception.InnerException.Message,"Error saving file",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void testBtn_Click(object sender, EventArgs e)
        {
            //When pressed, go into test mode.
            //Create a new simulation with starting state of a single cell instance from the current model
            //Set the simulation running, and refresh the view
            if (this.editMode != EditMode.Test)
            {
                controller.runTestModel();
            }
            else
            {
                //Pressing it again should stop the simulation.  Changing mode should also end the simulation.
                controller.stopTestModel();
            }
            
        }

        private void propertyGrid1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Console.WriteLine("Validating");
        }

        private void rearrangeBtn_Click(object sender, EventArgs e)
        {
            if (this.editMode != EditMode.AutoRearrange)
            {
                controller.runAutoRerrange();
            }
            else
            {
                controller.stopAutoRearranging();
            }
        }





        #region IDrawingInterface Members

        public delegate void redrawGraphPanelCallback();
        private bool waitingForRedraw = false;
        public void redrawGraphPanel()
        {

            //thread safe?
            if (this.InvokeRequired)
            {
                if (!waitingForRedraw)
                {
                    waitingForRedraw = true;
                    this.Invoke(new redrawGraphPanelCallback(this.redrawGraphPanel));
                }
            }
            else
            {
                pictureBox1.Refresh();
                waitingForRedraw = false;
            }
        }

        public delegate Vector2 getScreenTopLeftInWorldCoordinatesCallback();
        public Vector2 getScreenTopLeftInWorldCoordinates()
        {
            if (this.InvokeRequired)
            {
                return (Vector2)this.Invoke(new getScreenTopLeftInWorldCoordinatesCallback(this.getScreenTopLeftInWorldCoordinates));
            }
            else
            {
                Vector2 topLeft = getActualMousePosition(new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
                return topLeft;
            }
        }

        public delegate Vector2 getScreenBottomRightInWorldCoordinatesCallback();
        public Vector2 getScreenBottomRightInWorldCoordinates()
        {
            if (this.InvokeRequired)
            {
                return (Vector2)this.Invoke(new getScreenBottomRightInWorldCoordinatesCallback(this.getScreenBottomRightInWorldCoordinates));
            }
            else
            {
                Vector2 bottomRight = getActualMousePosition(new MouseEventArgs(MouseButtons.Left, 0, pictureBox1.Width, pictureBox1.Height, 0));
                return bottomRight;
            }
        }

        #endregion


        /*
        private void hackNumericUpDown_ValueChanged(object sender, EventArgs e)
        {

            TestRigs.GlobalData.useHack = true;
            TestRigs.GlobalData.hackSpeciesName = hackTextBox.Text;
            TestRigs.GlobalData.hackSpeciesValue = (float)hackNumericUpDown.Value;

        }*/


    }
}
