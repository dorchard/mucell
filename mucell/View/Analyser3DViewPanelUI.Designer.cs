using MuCell.View.OpenGL;

namespace MuCell.View
{
    partial class Analyser3DViewPanelUI
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


        private void InitOpenGLPanels()
        {
            spatialViewState = new SpatialViewState();
            this.OpenGLPanelAnalyser = new OpenGLCellPlacementPanel(spatialViewState, Views.ThreeDAnalyzer);
        }



        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            spatialViewState = new SpatialViewState();
            this.OpenGLPanelAnalyser = new OpenGLCellPlacementPanel(spatialViewState, Views.ThreeDAnalyzer);
  
            MuCell.View.OpenGL.OpenGLPanelViewState openGLPanelViewState1 = new MuCell.View.OpenGL.OpenGLPanelViewState();
            MuCell.Model.Boundary boundary1 = new MuCell.Model.Boundary();
            MuCell.View.OpenGL.SpatialViewState spatialViewState1 = new MuCell.View.OpenGL.SpatialViewState();
            MuCell.View.OpenGL.OpenGLPanelViewState openGLPanelViewState2 = new MuCell.View.OpenGL.OpenGLPanelViewState();
            MuCell.Model.Boundary boundary2 = new MuCell.Model.Boundary();
            MuCell.View.OpenGL.OpenGLPanelViewState openGLPanelViewState3 = new MuCell.View.OpenGL.OpenGLPanelViewState();
            MuCell.Model.Boundary boundary3 = new MuCell.Model.Boundary();
            MuCell.View.OpenGL.OpenGLPanelViewState openGLPanelViewState4 = new MuCell.View.OpenGL.OpenGLPanelViewState();
            MuCell.Model.Boundary boundary4 = new MuCell.Model.Boundary();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.testButton1 = new System.Windows.Forms.Button();
            this.testButton2 = new System.Windows.Forms.Button();
            this.testButton3 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenGLPanelAnalyser
            // 
            this.OpenGLPanelAnalyser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OpenGLPanelAnalyser.Location = new System.Drawing.Point(3, 3);
            this.OpenGLPanelAnalyser.Name = "OpenGLPanelAnalyser";
            openGLPanelViewState1.Ang3D = 0F;
            openGLPanelViewState1.AngAboutY = 0F;
            openGLPanelViewState1.Animation = MuCell.View.OpenGL.AnimationState.None;
            openGLPanelViewState1.AnimationCounter = 0F;
            boundary1.Depth = 5F;
            boundary1.Height = 100F;
            boundary1.Radius = 0F;
            boundary1.Shape = MuCell.Model.BoundaryShapes.Cuboid;
            boundary1.Width = 100F;
            openGLPanelViewState1.CrossSection = boundary1;
            openGLPanelViewState1.CrossSectionEnabled = false;
            openGLPanelViewState1.CrossSectionFacing = MuCell.View.OpenGL.CrossSectionFacingDirection.Front;
            openGLPanelViewState1.CurrentCameraPos = MuCell.View.OpenGL.CameraPosition.Corner;
            openGLPanelViewState1.NextCameraPos = MuCell.View.OpenGL.CameraPosition.Front;
            openGLPanelViewState1.ViewZoom = 1F;
            openGLPanelViewState1.ViewZoomLinear = 0F;
            this.OpenGLPanelAnalyser.PanelViewState = openGLPanelViewState1;
            this.OpenGLPanelAnalyser.Size = new System.Drawing.Size(448, 443);
            spatialViewState1.CrossHairOutOfBounds = false;
            spatialViewState1.CurrentSimState = null;
            spatialViewState1.InitialSimState = null;
            spatialViewState1.SelectedGroupIndex = -1;
            spatialViewState1.SelectedNutrientIndex = -1;
            spatialViewState1.SimParams = null;
            openGLPanelViewState2.Ang3D = 0F;
            openGLPanelViewState2.AngAboutY = 0F;
            openGLPanelViewState2.Animation = MuCell.View.OpenGL.AnimationState.None;
            openGLPanelViewState2.AnimationCounter = 0F;
            boundary2.Depth = 5F;
            boundary2.Height = 100F;
            boundary2.Radius = 0F;
            boundary2.Shape = MuCell.Model.BoundaryShapes.Cuboid;
            boundary2.Width = 100F;
            openGLPanelViewState2.CrossSection = boundary2;
            openGLPanelViewState2.CrossSectionEnabled = false;
            openGLPanelViewState2.CrossSectionFacing = MuCell.View.OpenGL.CrossSectionFacingDirection.Front;
            openGLPanelViewState2.CurrentCameraPos = MuCell.View.OpenGL.CameraPosition.Corner;
            openGLPanelViewState2.NextCameraPos = MuCell.View.OpenGL.CameraPosition.Front;
            openGLPanelViewState2.ViewZoom = 1F;
            openGLPanelViewState2.ViewZoomLinear = 0F;
            spatialViewState1.ThreeD = openGLPanelViewState2;
            spatialViewState1.ThreeDAnalyzer = openGLPanelViewState1;
            openGLPanelViewState3.Ang3D = 0F;
            openGLPanelViewState3.AngAboutY = 0F;
            openGLPanelViewState3.Animation = MuCell.View.OpenGL.AnimationState.None;
            openGLPanelViewState3.AnimationCounter = 0F;
            boundary3.Depth = 5F;
            boundary3.Height = 100F;
            boundary3.Radius = 0F;
            boundary3.Shape = MuCell.Model.BoundaryShapes.Cuboid;
            boundary3.Width = 100F;
            openGLPanelViewState3.CrossSection = boundary3;
            openGLPanelViewState3.CrossSectionEnabled = false;
            openGLPanelViewState3.CrossSectionFacing = MuCell.View.OpenGL.CrossSectionFacingDirection.Front;
            openGLPanelViewState3.CurrentCameraPos = MuCell.View.OpenGL.CameraPosition.Corner;
            openGLPanelViewState3.NextCameraPos = MuCell.View.OpenGL.CameraPosition.Front;
            openGLPanelViewState3.ViewZoom = 1F;
            openGLPanelViewState3.ViewZoomLinear = 0F;
            spatialViewState1.XY = openGLPanelViewState3;
            openGLPanelViewState4.Ang3D = 0F;
            openGLPanelViewState4.AngAboutY = 0F;
            openGLPanelViewState4.Animation = MuCell.View.OpenGL.AnimationState.None;
            openGLPanelViewState4.AnimationCounter = 0F;
            boundary4.Depth = 5F;
            boundary4.Height = 100F;
            boundary4.Radius = 0F;
            boundary4.Shape = MuCell.Model.BoundaryShapes.Cuboid;
            boundary4.Width = 100F;
            openGLPanelViewState4.CrossSection = boundary4;
            openGLPanelViewState4.CrossSectionEnabled = false;
            openGLPanelViewState4.CrossSectionFacing = MuCell.View.OpenGL.CrossSectionFacingDirection.Front;
            openGLPanelViewState4.CurrentCameraPos = MuCell.View.OpenGL.CameraPosition.Corner;
            openGLPanelViewState4.NextCameraPos = MuCell.View.OpenGL.CameraPosition.Front;
            openGLPanelViewState4.ViewZoom = 1F;
            openGLPanelViewState4.ViewZoomLinear = 0F;
            spatialViewState1.XZ = openGLPanelViewState4;
            this.OpenGLPanelAnalyser.SpatialViewState = spatialViewState1;
            this.OpenGLPanelAnalyser.TabIndex = 0;
            this.OpenGLPanelAnalyser.View = MuCell.View.OpenGL.Views.ThreeDAnalyzer;
            this.OpenGLPanelAnalyser.Resize += new System.EventHandler(this.OpenGLPanelAnalyser_Resize);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.Controls.Add(this.OpenGLPanelAnalyser, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(574, 449);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(457, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(114, 443);
            this.panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.testButton1);
            this.groupBox1.Controls.Add(this.testButton2);
            this.groupBox1.Controls.Add(this.testButton3);
            this.groupBox1.Location = new System.Drawing.Point(17, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(79, 124);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View";
            // 
            // testButton1
            // 
            this.testButton1.Location = new System.Drawing.Point(6, 52);
            this.testButton1.Name = "testButton1";
            this.testButton1.Size = new System.Drawing.Size(65, 27);
            this.testButton1.TabIndex = 0;
            this.testButton1.Text = "Front";
            this.testButton1.UseVisualStyleBackColor = true;
            this.testButton1.Click += new System.EventHandler(this.testButton1_Click);
            // 
            // testButton2
            // 
            this.testButton2.Location = new System.Drawing.Point(6, 85);
            this.testButton2.Name = "testButton2";
            this.testButton2.Size = new System.Drawing.Size(65, 27);
            this.testButton2.TabIndex = 1;
            this.testButton2.Text = "Top-down";
            this.testButton2.UseVisualStyleBackColor = true;
            this.testButton2.Click += new System.EventHandler(this.testButton2_Click);
            // 
            // testButton3
            // 
            this.testButton3.Location = new System.Drawing.Point(6, 19);
            this.testButton3.Name = "testButton3";
            this.testButton3.Size = new System.Drawing.Size(65, 27);
            this.testButton3.TabIndex = 2;
            this.testButton3.Text = "Corner";
            this.testButton3.UseVisualStyleBackColor = true;
            this.testButton3.Click += new System.EventHandler(this.testButton3_Click);
            // 
            // Analyser3DViewPanelUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Analyser3DViewPanelUI";
            this.Size = new System.Drawing.Size(574, 449);
            this.VisibleChanged += new System.EventHandler(this.Analyser3DViewPanelUI_VisibleChanged);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Analyser3DViewPanelUI_MouseWheel);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button testButton1;
        private System.Windows.Forms.Button testButton2;
        private System.Windows.Forms.Button testButton3;
        private System.Windows.Forms.GroupBox groupBox1;
   

    }
}
