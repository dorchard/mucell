using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenTK.OpenGL;
using OpenTK.OpenGL.Enums;

namespace MuCell.View.OpenGL
{
    /// <summary>
    /// A class extending the Windows Panel UI component providing
    /// OpenGL support
    /// </summary>
    public abstract class OpenGLPanel : System.Windows.Forms.Panel
    {
        private OpenTK.GLControl glControl1;

        

        public OpenGLPanel()
            : base()
        {
            
            InitializeComponent();
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Ccw);


            GLTextures.InitTextureSettings();

            //SetView(this.Height, this.Width);

          
            
        }


        
        private void InitializeComponent()
        {
            this.glControl1 = new OpenTK.GLControl();


            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl1.Location = new System.Drawing.Point(0, 0);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(200, 100);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = true;
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseClick);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            // 
            // OpenGLPanel
            // 
            this.Controls.Add(this.glControl1);
            this.ResumeLayout(false);

        }


        public virtual void SetView(int height, int width)
        {
            // Set viewport to window dimensions.
            GL.Viewport(0, 0, width, height);

            // Reset projection matrix stack
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            const float nRange = 80.0f;
            // Prevent a divide by zero
            if (height == 0)
            {
                height = 1;
            }

            // Establish clipping volume (left, right, bottom,
            // top, near, far)
            if (width <= height)
            {
                GL.Ortho(-nRange, nRange, -nRange * height / width, nRange * height / width, -nRange, nRange);
            }
            else
            {
                GL.Ortho(-nRange * width / height, nRange * width / height, -nRange, nRange, -nRange, nRange);
            }
            // reset modelview matrix stack
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }


        /// <summary>
        /// Paint event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {

            glControl1.Context.MakeCurrent();
            Render();
            glControl1.SwapBuffers();
        }

        /// <summary>
        /// Handles viewport settings when resizing the window.
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetView(this.Height, this.Width);
        }

        /// <summary>
        /// Handles painting of the form.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Clear Screen And Depth Buffer
            glControl1.Context.MakeCurrent();
            Render();
            glControl1.SwapBuffers();
        }



        /// <summary>
        /// Draws a frame.
        /// </summary>
        public abstract void Render();






        /* The following event methods are for bubbling a click event on the
         * OpenGLControl up to the parent.
         * 
         * 
         */

        //Event passed
        public event EventHandler ChildClickEvent;
        public event MouseEventHandler ChildMouseDownEvent;
        public event MouseEventHandler ChildMouseUpEvent;

        // Invoke the click event
        protected void OnChildClickEvent(EventArgs e)
        {
            if (ChildClickEvent != null) ChildClickEvent(this, e);
        }
        
        // Invoke mouse down event
        protected void OnChildMouseDownEvent(MouseEventArgs e)
        {
            if (ChildMouseDownEvent != null) ChildMouseDownEvent(this, e);
        }

        // Invoke mouse up event
        protected void OnChildMouseUpEvent(MouseEventArgs e)
        {
            if (ChildMouseUpEvent != null) ChildMouseUpEvent(this, e);
        }


        // This is called whenever the openGL window is clicked
        private void glControl1_MouseClick(object sender, MouseEventArgs e)
        {
            OnChildClickEvent(e);
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            OnChildMouseDownEvent(e);
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            OnChildMouseUpEvent(e);
        }



    }
}
