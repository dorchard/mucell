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
    
            MuCell.View.OpenGL.OpenGLPanelViewState openGLPanelViewState5 = new MuCell.View.OpenGL.OpenGLPanelViewState();
            MuCell.Model.Boundary boundary5 = new MuCell.Model.Boundary();
            MuCell.View.OpenGL.SpatialViewState spatialViewState2 = new MuCell.View.OpenGL.SpatialViewState();
            MuCell.View.OpenGL.OpenGLPanelViewState openGLPanelViewState6 = new MuCell.View.OpenGL.OpenGLPanelViewState();
            MuCell.Model.Boundary boundary6 = new MuCell.Model.Boundary();
            MuCell.View.OpenGL.OpenGLPanelViewState openGLPanelViewState7 = new MuCell.View.OpenGL.OpenGLPanelViewState();
            MuCell.Model.Boundary boundary7 = new MuCell.Model.Boundary();
            MuCell.View.OpenGL.OpenGLPanelViewState openGLPanelViewState8 = new MuCell.View.OpenGL.OpenGLPanelViewState();
            MuCell.Model.Boundary boundary8 = new MuCell.Model.Boundary();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.testButton3 = new System.Windows.Forms.Button();
            this.testButton2 = new System.Windows.Forms.Button();
            this.testButton1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            openGLPanelViewState5.Ang3D = 0F;
            openGLPanelViewState5.AngAboutY = 0F;
            openGLPanelViewState5.Animation = MuCell.View.OpenGL.AnimationState.None;
            openGLPanelViewState5.AnimationCounter = 0F;
            boundary5.Depth = 5F;
            boundary5.Height = 100F;
            boundary5.Radius = 0F;
            boundary5.Shape = MuCell.Model.BoundaryShapes.Cuboid;
            boundary5.Width = 100F;
            openGLPanelViewState5.CrossSection = boundary5;
            openGLPanelViewState5.CrossSectionEnabled = false;
            openGLPanelViewState5.CrossSectionFacing = MuCell.View.OpenGL.CrossSectionFacingDirection.Front;
            openGLPanelViewState5.CurrentCameraPos = MuCell.View.OpenGL.CameraPosition.Corner;
            openGLPanelViewState5.NextCameraPos = MuCell.View.OpenGL.CameraPosition.Front;
            openGLPanelViewState5.ViewZoom = 1F;
            openGLPanelViewState5.ViewZoomLinear = 0F;
            this.OpenGLPanelAnalyser.PanelViewState = openGLPanelViewState5;
            this.OpenGLPanelAnalyser.Size = new System.Drawing.Size(395, 443);
            spatialViewState2.CrossHairOutOfBounds = false;
            spatialViewState2.CurrentSimState = null;
            spatialViewState2.InitialSimState = null;
            spatialViewState2.SelectedGroupIndex = -1;
            spatialViewState2.SelectedNutrientIndex = -1;
            spatialViewState2.SimParams = null;
            openGLPanelViewState6.Ang3D = 0F;
            openGLPanelViewState6.AngAboutY = 0F;
            openGLPanelViewState6.Animation = MuCell.View.OpenGL.AnimationState.None;
            openGLPanelViewState6.AnimationCounter = 0F;
            boundary6.Depth = 5F;
            boundary6.Height = 100F;
            boundary6.Radius = 0F;
            boundary6.Shape = MuCell.Model.BoundaryShapes.Cuboid;
            boundary6.Width = 100F;
            openGLPanelViewState6.CrossSection = boundary6;
            openGLPanelViewState6.CrossSectionEnabled = false;
            openGLPanelViewState6.CrossSectionFacing = MuCell.View.OpenGL.CrossSectionFacingDirection.Front;
            openGLPanelViewState6.CurrentCameraPos = MuCell.View.OpenGL.CameraPosition.Corner;
            openGLPanelViewState6.NextCameraPos = MuCell.View.OpenGL.CameraPosition.Front;
            openGLPanelViewState6.ViewZoom = 1F;
            openGLPanelViewState6.ViewZoomLinear = 0F;
            spatialViewState2.ThreeD = openGLPanelViewState6;
            spatialViewState2.ThreeDAnalyzer = openGLPanelViewState5;
            openGLPanelViewState7.Ang3D = 0F;
            openGLPanelViewState7.AngAboutY = 0F;
            openGLPanelViewState7.Animation = MuCell.View.OpenGL.AnimationState.None;
            openGLPanelViewState7.AnimationCounter = 0F;
            boundary7.Depth = 5F;
            boundary7.Height = 100F;
            boundary7.Radius = 0F;
            boundary7.Shape = MuCell.Model.BoundaryShapes.Cuboid;
            boundary7.Width = 100F;
            openGLPanelViewState7.CrossSection = boundary7;
            openGLPanelViewState7.CrossSectionEnabled = false;
            openGLPanelViewState7.CrossSectionFacing = MuCell.View.OpenGL.CrossSectionFacingDirection.Front;
            openGLPanelViewState7.CurrentCameraPos = MuCell.View.OpenGL.CameraPosition.Corner;
            openGLPanelViewState7.NextCameraPos = MuCell.View.OpenGL.CameraPosition.Front;
            openGLPanelViewState7.ViewZoom = 1F;
            openGLPanelViewState7.ViewZoomLinear = 0F;
            spatialViewState2.XY = openGLPanelViewState7;
            openGLPanelViewState8.Ang3D = 0F;
            openGLPanelViewState8.AngAboutY = 0F;
            openGLPanelViewState8.Animation = MuCell.View.OpenGL.AnimationState.None;
            openGLPanelViewState8.AnimationCounter = 0F;
            boundary8.Depth = 5F;
            boundary8.Height = 100F;
            boundary8.Radius = 0F;
            boundary8.Shape = MuCell.Model.BoundaryShapes.Cuboid;
            boundary8.Width = 100F;
            openGLPanelViewState8.CrossSection = boundary8;
            openGLPanelViewState8.CrossSectionEnabled = false;
            openGLPanelViewState8.CrossSectionFacing = MuCell.View.OpenGL.CrossSectionFacingDirection.Front;
            openGLPanelViewState8.CurrentCameraPos = MuCell.View.OpenGL.CameraPosition.Corner;
            openGLPanelViewState8.NextCameraPos = MuCell.View.OpenGL.CameraPosition.Front;
            openGLPanelViewState8.ViewZoom = 1F;
            openGLPanelViewState8.ViewZoomLinear = 0F;
            spatialViewState2.XZ = openGLPanelViewState8;
            this.OpenGLPanelAnalyser.SpatialViewState = spatialViewState2;
            this.OpenGLPanelAnalyser.TabIndex = 0;
            this.OpenGLPanelAnalyser.View = MuCell.View.OpenGL.Views.ThreeDAnalyzer;
            this.OpenGLPanelAnalyser.Resize += new System.EventHandler(this.OpenGLPanelAnalyser_Resize);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
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
            this.panel1.Location = new System.Drawing.Point(404, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(167, 443);
            this.panel1.TabIndex = 1;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.testButton1);
            this.groupBox1.Controls.Add(this.testButton2);
            this.groupBox1.Controls.Add(this.testButton3);
            this.groupBox1.Location = new System.Drawing.Point(38, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(79, 124);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View";
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
