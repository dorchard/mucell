using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows;
using OpenTK.OpenGL;
using OpenTK.OpenGL.Enums;
using MuCell.Model;

namespace MuCell.View.OpenGL
{

    public enum Views { XY = 0, XZ, ThreeD, ThreeDAnalyzer };

    /// <summary>
    /// A class providing an OpenGL panel for rendering a simulation environment
    /// </summary>
    public class OpenGLCellPlacementPanel : OpenGLPanel
    {


        //reference to common state shared amongst views
        private SpatialViewState spatialViewState;
        public SpatialViewState SpatialViewState
        {
            get { return spatialViewState; }
            set
            {
                spatialViewState = value;

                switch (view)
                {
                    case Views.XY:
                        panelViewState = value.XY;
                        break;

                    case Views.XZ:
                        panelViewState = value.XZ;
                        break;

                    case Views.ThreeD:
                        panelViewState = value.ThreeD;
                        break;

                    case Views.ThreeDAnalyzer:
                        panelViewState = value.ThreeDAnalyzer;
                        break;


                }
            }

        }

        //reference to the viewstate currently assigned to this panel
        private OpenGLPanelViewState panelViewState;
        public OpenGLPanelViewState PanelViewState
        {
            get { return panelViewState; }
            set { panelViewState = value; }
        }


        //which perspective to view the environment from
        private Views view;
        public Views View
        {
            get { return view; }
            set { view = value; }
        }



        private List<CellToSort> sortedCells;
  


        /// <summary>
        /// Returns the width of the full OpenGL viewport. This may
        /// be smaller than the panel itself due to padding.
        /// </summary>
        public int GetWidth()
        {
            return this.Width - (this.Padding.Left + this.Padding.Right);

        }
        /// <summary>
        /// Returns the height of the full OpenGL viewport. This may
        /// be smaller than the panel itself due to padding.
        /// </summary>
        public int GetHeight()
        {
            return this.Height - (this.Padding.Top + this.Padding.Bottom);
        }

  

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewState">Definiton of the state of this view</param>
        /// <param name="v">The viewing perspective</param>
        public OpenGLCellPlacementPanel(SpatialViewState viewState, Views v): 
            base() 
        {
            this.spatialViewState = viewState;
            this.view = v;
            this.sortedCells = new List<CellToSort>();

            if (v == Views.XY)
            {
                this.panelViewState = viewState.XY;
            }
            else if (v == Views.XZ)
            {
                this.panelViewState = viewState.XZ;
            }
            else if (v == Views.ThreeD)
            {
                this.panelViewState = viewState.ThreeD;
            }
            else if (v == Views.ThreeDAnalyzer)
            {
                this.panelViewState = viewState.ThreeDAnalyzer;
            }

        }


        /// <summary>
        /// Render the scene
        /// </summary>
        public override void Render()
        {
            if (spatialViewState.SimParams == null || spatialViewState.InitialSimState == null)
            {
             //   setSimpleView();
             //   drawSimpleGradient();
             //   GL.Flush();
                return;
           
            }
            

            //Sets respective perspective view and clears Screen And Depth Buffer
            SetView(GetHeight(), GetWidth());
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            //Sets a simple view to allow a gradient to be drawn in the background
            setSimpleView();
            drawSimpleGradient();

            //Sets respective perspective view
            SetView(GetHeight(), GetWidth());

            switch (view)
            {
                case Views.XY:
                case Views.XZ:
                    DrawWireframe();

                    DrawBoxSurroundingSelectedGroup();
                    DrawBoundarySolid();
                    DrawBoundaryFrame();
                    DrawCrossSecionBorder();

                    SortCells(spatialViewState.InitialSimState);
                    DrawCells(spatialViewState.InitialSimState);
                    DrawNutrientFieldInitialPositions();
                    DrawPointer();


                    break;


                case Views.ThreeD:

                    DrawBoxSurroundingSelectedGroup();
                    DrawBoundarySolid();
                    DrawBoundaryFrame();
                    DrawCrossSecionBorder();
                    if (panelViewState.CrossSectionEnabled)
                    {
                        DrawNutrientField(spatialViewState.InitialSimState, 2.5f);
                    }
                    SortCells(spatialViewState.InitialSimState);
                    DrawCells(spatialViewState.InitialSimState);
                    DrawNutrientFieldInitialPositions();
                    DrawPointer();

                    break;

                case Views.ThreeDAnalyzer:

                    DrawBoundarySolid();
                    DrawBoundaryFrame();
                    DrawCrossSecionBorder();
                    DrawNutrientField(spatialViewState.CurrentSimState,2.5f);
                    SortCells(spatialViewState.CurrentSimState);
                    DrawCells(spatialViewState.CurrentSimState);
                   // DrawPointer();

                    break;
                        
            }







          

            //Force OpenGL command stream to complete
            GL.Flush();
            
        }


        /// <summary>
        /// Draws a simple grid.
        /// </summary>
        private void DrawWireframe()
        {
      
            GL.Disable(EnableCap.DepthTest);
            GL.Begin(BeginMode.Lines);

                for (int n = -400; n <= 400; n+=5)
                {
                    GL.Color3(0.78f, 0.78f, 0.78f);

                    //draw grid in the XY-plane
                    GL.Vertex3(-400 , (float)n , 0.0f);
                    GL.Vertex3(400 , (float)n , 0.0f);
                    GL.Vertex3((float)n , -400 , 0.0f);
                    GL.Vertex3((float)n , 400 , 0.0f);


                    //draw grid in the XZ-plane
                    GL.Vertex3(-400 , 0.0f,(float)n );
                    GL.Vertex3(400 , 0.0f, (float)n );
                    GL.Vertex3((float)n , 0.0f, -400 );
                    GL.Vertex3((float)n , 0.0f, 400 );
                }

            GL.End();
            GL.Enable(EnableCap.DepthTest);


        }


        /// <summary>
        /// Draws a crosshair pointer
        /// </summary>
        private void DrawPointer()
        {
            float x, y, z;
 
            x = (float)spatialViewState.CrossHairPosition.x;
            y = (float)spatialViewState.CrossHairPosition.y;
            z = (float)spatialViewState.CrossHairPosition.z;

            //Disables the depth-test so that the crosshaird connot be obscured
            GL.Disable(EnableCap.DepthTest);
            
            if (spatialViewState.CrossHairOutOfBounds)
            {
                GL.Color4(0.0f, 0.0f, 0.7f, 1.0f);
            }
            else
            {
                GL.Color4(0.8f, 0.0f, 0.0f, 1.0f);

            }


            GL.Begin(BeginMode.Lines);
                GL.Vertex3(x - 5.0f, y, z);
                GL.Vertex3(x + 5.0f, y, z);

                GL.Vertex3(x, y - 5.0f, z);
                GL.Vertex3(x, y + 5.0f, z);

                GL.Vertex3(x,y, z - 5.0f);
                GL.Vertex3(x,y, z + 5.0f);
            GL.End();

            GL.Enable(EnableCap.DepthTest);

        }



        /// <summary>
        /// Sorts cells according to their depth from the camera, and places the result
        /// in sortedCells
        /// </summary>
        /// <param name="simState"></param>
        private void SortCells(StateSnapshot simState)
        {

            if (simState!= null)
            {
                Vector3 pos;

                sortedCells.Clear();

                foreach (CellInstance cell in simState.Cells)
                {
                   
                    pos = cell.CellInstanceSpatialContext.Position;
                    GL.PushMatrix();
                        GL.Translate(pos.x, pos.y, pos.z);
                         sortedCells.Add(new CellToSort(cell,GLDrawHelper.GetDepth()));
                    GL.PopMatrix();
                   
                }


                if (view == Views.ThreeDAnalyzer)
                {
                    sortedCells.Sort();
                }


            }
        }



        /// <summary>
        /// Draws all the cells in the environment
        /// </summary>
        /// <param name="simState"></param>
        private void DrawCells(StateSnapshot simState)
        {
            if (simState!= null)
            {
                Vector3 pos;
                GL.Enable(EnableCap.DepthTest);
                GL.Enable(EnableCap.Texture2d);
                
                foreach (CellToSort cellToSort in sortedCells)
                    {
                        CellInstance cell = cellToSort.cell;
                        MuCell.Model.SBML.Group groupObj = simState.SimulationEnvironment.GetGroupObject(cell.GroupID);  
                      
                        pos = cell.CellInstanceSpatialContext.Position;
                        GL.PushMatrix();

                            GL.Translate(pos.x, pos.y, pos.z);
                            
                            GLDrawHelper.BillboardCheatSphericalBegin(view);

 
                                GL.Color4((float)groupObj.Col.R / 256, (float)groupObj.Col.G / 256, (float)groupObj.Col.B / 256, 1.0f);

                  
                                    GL.BindTexture(TextureTarget.Texture2d, GLTextures.GetTex(0,(int)view));
                                    GL.DepthMask(false);
                                    GL.Begin(BeginMode.Quads);

                                        GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1.10f, -1.10f);
                                        GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1.10f, -1.10f);
                                        GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1.10f, 1.10f);
                                        GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1.10f, 1.10f);

                                    GL.End();
                                    GL.DepthMask(true);

                            GLDrawHelper.BillboardEnd();
                        GL.PopMatrix();
                       
                    }
                    GL.Disable(EnableCap.Texture2d);
            }
        }



        /// <summary>
        /// Draws a nutrient field cross section
        /// </summary>
        /// <param name="simState"></param>
        /// <param name="res"></param>
        private void DrawNutrientField(StateSnapshot simState, float res)
        {

            if (simState != null)
            {



                foreach (int nutrientIndex in simState.SimulationEnvironment.GetNutrients())
                {
                    NutrientField nutrient = simState.SimulationEnvironment.GetNutrientFieldObject(nutrientIndex);
                    float r, g, b;


                    if (!nutrient.FieldLoaded)
                    {
                        continue;
                    }


                    r = (float)nutrient.Col.R / 256;
                    g = (float)nutrient.Col.G / 256;
                    b = (float)nutrient.Col.B / 256;

                    Vector3 pos = new Vector3(0, 0, 0); ;



                    // PanelViewState.CrossSection.Depth

                    float imin = PanelViewState.CrossSectionOffset.x - PanelViewState.CrossSection.Width / 2;
                    float jmin = PanelViewState.CrossSectionOffset.y - PanelViewState.CrossSection.Height / 2;
                    float kmin = PanelViewState.CrossSectionOffset.z - PanelViewState.CrossSection.Depth / 2;

                    float imax = imin + PanelViewState.CrossSection.Width;
                    float jmax = jmin + PanelViewState.CrossSection.Height;
                    float kmax = kmin + PanelViewState.CrossSection.Depth;

                    //vertical cross section
                    if (PanelViewState.CrossSection.Height < PanelViewState.CrossSection.Depth)
                    {
                        jmin = PanelViewState.CrossSectionOffset.y;
                        jmax = PanelViewState.CrossSectionOffset.y + 0.001f;

                    }
                    else
                    {
                        kmin = PanelViewState.CrossSectionOffset.z;
                        kmax = PanelViewState.CrossSectionOffset.z + 0.001f;
                    }


                    GL.Disable(EnableCap.Texture2d);
                    GL.DepthMask(false); //disable depth writes
                    GL.Disable(EnableCap.CullFace);

                    GL.Begin(BeginMode.Quads);

                    if (PanelViewState.CrossSection.Height > PanelViewState.CrossSection.Depth)
                    {

                        float k = kmin;
                        float halfRes = res / 2;
                        

                        for (float i = imin; i < imax; i += res)
                        {
                            for (float j = jmin; j < jmax; j += res)
                            {
                                float level = nutrient.GetNutrientLevelApprox(i, j, k) * spatialViewState.NutrientIntensityScale;
                               // float level = nutrient.GetNutrientLevel(i, j, k);

                                if (level > 1.0f) level = 1.0f;
                                GL.Color4(r, g, b, level);
                                pos.x = i;
                                pos.y = j;
                                pos.z = k;


                                GL.Vertex3(pos.x - halfRes, pos.y - halfRes, pos.z);
                                GL.Vertex3(pos.x - halfRes, pos.y + halfRes, pos.z);
                                GL.Vertex3(pos.x + halfRes, pos.y + halfRes, pos.z);
                                GL.Vertex3(pos.x + halfRes, pos.y - halfRes, pos.z);

                            }

                        }

                    }
                    else
                    {

                        float j = kmin;
                        float halfRes = res / 2;

                        for (float i = imin; i < imax; i += res)
                        {
                            for (float k = kmin; k < kmax; k += res)
                            {
                                 float level = nutrient.GetNutrientLevelApprox(i, j, k);
                               // float level = nutrient.GetNutrientLevel(i, j, k);

                                if (level > 1.0f) level = 1.0f;
                                GL.Color4(r, g, b, level);
                                pos.x = i;
                                pos.y = j;
                                pos.z = k;


                                GL.Vertex3(pos.x - halfRes, pos.y, pos.z - halfRes);
                                GL.Vertex3(pos.x - halfRes, pos.y, pos.z +halfRes);
                                GL.Vertex3(pos.x + halfRes, pos.y, pos.z + halfRes);
                                GL.Vertex3(pos.x + halfRes, pos.y, pos.z - halfRes);

                            }

                        }
                    }
                    

                    GL.End();
                    GL.DepthMask(true);
                    GL.Enable(EnableCap.CullFace);


                }
            }

        }

    

        /// <summary>
        /// Draws icons representing the centre of concentration fields
        /// </summary>
        private void DrawNutrientFieldInitialPositions()
        {

            if (!panelViewState.drawNutrientInitialPos)
            {
                return;
            }
                   


            if (spatialViewState.SimParams != null)
            {
                Vector3 pos;
                float r, g, b;

                foreach (int nutrientIndex in spatialViewState.InitialSimState.SimulationEnvironment.GetNutrients())
                {
                    NutrientField nutrient = spatialViewState.InitialSimState.SimulationEnvironment.GetNutrientFieldObject(nutrientIndex);
                    pos = nutrient.InitialPosition;
                    r = (float)nutrient.Col.R / 256;
                    g = (float)nutrient.Col.G / 256;
                    b = (float)nutrient.Col.B / 256;
                    
                    GL.PushMatrix();
                    GL.Translate(pos.x, pos.y, pos.z);
                    GLDrawHelper.BillboardCheatSphericalBegin(view);


                  
                        //circle the selected nutrient
                        if (nutrientIndex == spatialViewState.SelectedNutrientIndex)
                        {
                            GL.Color4(1.0, 0.0, 0.0, 0.9f);
                            GLDrawHelper.DrawCuboidWireframe(12, 12, 0);
                        }
                        GL.Enable(EnableCap.Texture2d);
                        GL.BindTexture(TextureTarget.Texture2d, GLTextures.GetTex(1, (int)view));
                        GL.DepthMask(false);
                        GL.Begin(BeginMode.Quads);

                            GL.Color3(r, g, b);
                            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-6.0f, -6.0f);
                            //GL.Color3(r * 0.9, g * 0.9, b * 0.9);
                            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(6.0f, -6.0f);
                          //  GL.Color3(r , g , b );
                            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(6.0f, 6.0f);
                           // GL.Color3(r * 0.9, g * 0.9, b * 0.9);
                            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-6.10f, 6.0f);

                        GL.End();
                        GL.DepthMask(true);
                        GL.Disable(EnableCap.Texture2d);
                    GLDrawHelper.BillboardEnd();
                    GL.PopMatrix();
                    

                }

            }
            

        }


        /// <summary>
        /// Draws a box surrounding the currently selected group of cells
        /// </summary>
        private void DrawBoxSurroundingSelectedGroup()
        {
            if (spatialViewState.SelectedGroupIndex != -1)
            {

                
                GL.Color4(0.6f, 0.6f, 0.6f,1.0f);
                GLDrawHelper.DrawWireframeBox(spatialViewState.SelectedGroupBoxCorner1.x,
                                              spatialViewState.SelectedGroupBoxCorner1.y,
                                              spatialViewState.SelectedGroupBoxCorner1.z,
                                              spatialViewState.SelectedGroupBoxCorner2.x,
                                              spatialViewState.SelectedGroupBoxCorner2.y,
                                              spatialViewState.SelectedGroupBoxCorner2.z);

               
            }


        }


        /// <summary>
        /// Draws the solid (but translucent) shape of the boundary
        /// </summary>
        private void DrawBoundarySolid()
        {
            if (spatialViewState.SimParams == null || spatialViewState.InitialSimState == null)
            {
                return;
            }
            int quadric = Glu.NewQuadric();
            Glu.QuadricDrawStyle(quadric, QuadricDrawStyle.Fill);

            switch (spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Shape)
            {

                case BoundaryShapes.Cuboid:
                    GL.Color4(0.85f, 0.75f, 0.75f, 0.2f);
                    GL.DepthMask(false);
                    GLDrawHelper.DrawCuboid(spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Width,
                        spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height,
                        spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Depth);
                    GL.DepthMask(true);

                    break;
                case BoundaryShapes.Cylinder:


                    GL.PushMatrix();

                        GL.Rotate(-90.0f, 1.0f, 0.0f, 0.0f);

                        GL.PushMatrix();
                            GL.Translate(0.0f, 0.0f, spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height / 2);
                            GL.Color4(0.85f, 0.75f, 0.75f, 0.2f);
                            GL.DepthMask(false);
                            Glu.Disk(quadric, 0, spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius, 50, 1);
                            GL.DepthMask(true);
                        GL.PopMatrix();

                        GL.PushMatrix();
                            GL.Rotate(-180.0f, 1.0f, 0.0f, 0.0f);
                            GL.Translate(0.0f, 0.0f, spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height / 2);
                            GL.Color4(0.85f, 0.75f, 0.75f, 0.2f);
                            GL.DepthMask(false);
                            Glu.Disk(quadric, 0, spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius, 50, 1);
                            GL.DepthMask(true);
                        GL.PopMatrix();

                        GL.Translate(0.0f, 0.0f, -spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height / 2);

                        GL.Color4(0.85f, 0.75f, 0.75f, 0.2f);
                        GL.DepthMask(false);
                        Glu.Cylinder(quadric, spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius,
                        spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius,
                        spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height, 50, 1);
                        GL.DepthMask(true);

                    GL.PopMatrix();
                    break;


                case BoundaryShapes.Sphere:

                    GL.Color4(0.85f, 0.75f, 0.75f, 0.2f);
                    GL.DepthMask(false);
                    Glu.Sphere(quadric, spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius, 50, 50);
                    GL.DepthMask(true);

   
                    break;

            }

            Glu.DeleteQuadric(quadric);

            GL.Enable(EnableCap.DepthTest);

        }



        /// <summary>
        /// Draws the frame of the boundary
        /// </summary>
        private void DrawBoundaryFrame()
        {


            if (spatialViewState.SimParams == null)
            {
                return;
            }
            /*
            if (panelViewState.CrossSectionEnabled)
            {
                return;
            }*/


            int quadricWire = Glu.NewQuadric();
            Glu.QuadricDrawStyle(quadricWire, QuadricDrawStyle.Line);

            switch (spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Shape)
            {

                case BoundaryShapes.Cuboid:

                    if (view == Views.ThreeD || view == Views.ThreeDAnalyzer)
                    {
                        GL.Color4(0.85f, 0.75f, 0.75f, 1.0f);
                     
                        GLDrawHelper.DrawCuboidWireframe(spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Width,
                                    spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height,
                                    spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Depth);
                        
                    }

                    if (view != Views.ThreeDAnalyzer)
                    {
                        GL.Disable(EnableCap.DepthTest);
                        GLDrawHelper.DrawCuboidBorder(spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Width,
                                spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height,
                                spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Depth, 4f);
                        GL.Enable(EnableCap.DepthTest);
                    }
                    break;


                case BoundaryShapes.Cylinder:

                    GL.PushMatrix();

                    GL.Rotate(-90.0f, 1.0f, 0.0f, 0.0f);

                        GL.PushMatrix();
                        GL.Translate(0.0f, 0.0f, spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height / 2);


                            if (view == Views.ThreeD || view == Views.ThreeDAnalyzer)
                            {
                                GL.Color4(0.85f, 0.75f, 0.75f, 1.0f);
                                Glu.Disk(quadricWire, 0, spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius, 50, 1);
                            }
                        GL.PopMatrix();

                        GL.PushMatrix();
                            GL.Rotate(-180.0f, 1.0f, 0.0f, 0.0f);
                            GL.Translate(0.0f, 0.0f, spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height / 2);
                            if (view == Views.ThreeD || view == Views.ThreeDAnalyzer)
                            {
                                GL.Color4(0.85f, 0.75f, 0.75f, 1.0f);
                                Glu.Disk(quadricWire, 0, spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius, 50, 1);
                            }
                        GL.PopMatrix();

                        GL.Translate(0.0f, 0.0f, -spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height / 2);


                        if (view == Views.ThreeD || view == Views.ThreeDAnalyzer)
                        {
                            GL.Color4(0.85f, 0.75f, 0.75f, 1.0f);
                            Glu.Cylinder(quadricWire, spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius,
                                spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius,
                                spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height, 50, 1);
                        }

                        GL.Disable(EnableCap.DepthTest);

                        if (view != Views.ThreeDAnalyzer)
                        {
                            GLDrawHelper.DrawCylinderBorder(spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height, spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius, 50, 4f);
                        }
                    GL.PopMatrix();
                    GL.Enable(EnableCap.DepthTest);

                    break;  


                case BoundaryShapes.Sphere:

                    if (view == Views.ThreeD || view == Views.ThreeDAnalyzer)
                    {
                        GL.Color4(0.85f, 0.75f, 0.75f, 1.0f);
                        Glu.Sphere(quadricWire, spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius, 50, 10);
                    }
                    if (view != Views.ThreeDAnalyzer)
                    {
                        GL.Disable(EnableCap.DepthTest);
                        GLDrawHelper.DrawSphereBorder(spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius, 50, 4f);
                        GL.Enable(EnableCap.DepthTest);
                       
                    }
                    break;

            }


            Glu.DeleteQuadric(quadricWire);
            GL.Enable(EnableCap.DepthTest);

        }



        /// <summary>
        /// Draws the wireframe borders illustrating the boundaries of the current 
        /// cross sectional region being viewed
        /// </summary>
        private void DrawCrossSecionBorder()
        {
            
            if (spatialViewState.SimParams == null)
            {
                return;
            }

            if (!panelViewState.CrossSectionEnabled)
            {
                return;
            }

            int quadricWire = Glu.NewQuadric();
            Glu.QuadricDrawStyle(quadricWire, QuadricDrawStyle.Line);

            GL.Color4(0.85f, 0.75f, 0.75f, 1.0f);
            GL.PushMatrix();
                GL.Translate(PanelViewState.CrossSectionOffset.x, PanelViewState.CrossSectionOffset.y, PanelViewState.CrossSectionOffset.z);
                GLDrawHelper.DrawCuboidWireframe(PanelViewState.CrossSection.Width, PanelViewState.CrossSection.Height, PanelViewState.CrossSection.Depth);

                //Cross section from the front (eg cutting a loaf)
                if (PanelViewState.CrossSection.Height > PanelViewState.CrossSection.Depth)
                {

                    switch (spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Shape)
                    {
                        case BoundaryShapes.Cuboid:
                            {
                                float width1, width2, height1, height2,depth;
                                height1 = height2 = spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height;
                                width1 = width2 = spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Width;
                                depth = spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Depth;

                                if (Math.Abs(PanelViewState.CrossSectionOffset.z + PanelViewState.CrossSection.Depth / 2) > depth/2)
                                {
                                    height1 = width1 = 0;
                                }

                                if (Math.Abs(PanelViewState.CrossSectionOffset.z - PanelViewState.CrossSection.Depth / 2) > depth / 2)
                                {
                                    height2 = width2 = 0;
                                }
                                GL.PushMatrix();
                                    GL.Translate(0.0f, 0.0f, PanelViewState.CrossSection.Depth / 2);
                                    //use cube as a rect...
                                    GLDrawHelper.DrawCuboidWireframe(width1, height1, 0);
                                GL.PopMatrix();

                                GL.PushMatrix();
                                    GL.Translate(0.0f, 0.0f, -PanelViewState.CrossSection.Depth / 2);
                                    //use cube as a rect...
                                    GLDrawHelper.DrawCuboidWireframe(width2, height2, 0);
                                GL.PopMatrix();
                                break;
                            }
                        case BoundaryShapes.Cylinder:
                            {
                                float width1, width2, height1, height2;
                                //widths start being used as a temp for depth
                                width1 = Math.Abs((PanelViewState.CrossSectionOffset.z + PanelViewState.CrossSection.Depth / 2));
                                width2 = Math.Abs((PanelViewState.CrossSectionOffset.z - PanelViewState.CrossSection.Depth / 2));
                                float rad = spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius;
                                height1 = height2 = spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height;

                                if (rad * rad - width1 * width1 > 0)
                                {
                                    width1 = (float)Math.Sqrt(rad * rad - width1 * width1);
                                }
                                else
                                {
                                    width1 = 0;
                                    height1 = 0;
                                }

                                if (rad * rad - width2 * width2 > 0)
                                {
                                    width2 = (float)Math.Sqrt(rad * rad - width2 * width2);
                                }
                                else
                                {
                                    width2 = 0;
                                    height2 = 0;
                                }


                                GL.PushMatrix();
                                    GL.Translate(0.0f, 0.0f, PanelViewState.CrossSection.Depth / 2);
                                    //use cube as a rect...
                                    GLDrawHelper.DrawCuboidWireframe(width1 * 2, height1, 0);
                                GL.PopMatrix();

                                GL.PushMatrix();
                                    GL.Translate(0.0f, 0.0f, -PanelViewState.CrossSection.Depth / 2);
                                    //use cube as a rect...
                                    GLDrawHelper.DrawCuboidWireframe(width2 * 2, height2, 0);
                                GL.PopMatrix();
                                //Glu.Sphere(quadricWire, spatialViewState.SimState.SimulationEnvironment.Boundary.Radius, 50, 10);

                                break;
                            }


                        case BoundaryShapes.Sphere:
                            {
                                float radius1, radius2;
                                //widths start being used as a temp for depth
                                radius1 = Math.Abs((PanelViewState.CrossSectionOffset.z + PanelViewState.CrossSection.Depth / 2));
                                radius2 = Math.Abs((PanelViewState.CrossSectionOffset.z - PanelViewState.CrossSection.Depth / 2));
                                float rad = spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius;

                                if (rad * rad - radius1 * radius1 > 0)
                                {
                                    radius1 = (float)Math.Sqrt(rad * rad - radius1 * radius1);
                                }
                                else
                                {
                                    radius1 = 0;
                                }

                                if (rad * rad - radius2 * radius2 > 0)
                                {
                                    radius2 = (float)Math.Sqrt(rad * rad - radius2 * radius2);
                                }
                                else
                                {
                                    radius2 = 0;
                                }


                                GL.PushMatrix();
                                    GL.Translate(0.0f, 0.0f, PanelViewState.CrossSection.Depth / 2);
                                    Glu.PartialDisk(quadricWire, radius1, radius1, 50, 1, 0, 360);
                                GL.PopMatrix();

                                GL.PushMatrix();
                                    GL.Translate(0.0f, 0.0f, -PanelViewState.CrossSection.Depth / 2);
                                    Glu.PartialDisk(quadricWire, radius2, radius2, 50, 1, 0, 360);
                                GL.PopMatrix();
                               
                                break;
                            }
                    }

                }
                //Cross section from the top (eg cutting a tree stump)
                else
                {
                    switch (spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Shape)
                    {
                        case BoundaryShapes.Cuboid:
                            {

                                float width1, width2, depth1, depth2, height;
                                depth1 = depth2 = spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Depth;
                                width1 = width2 = spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Width;
                                height = spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height;

                                if (Math.Abs(PanelViewState.CrossSectionOffset.y + PanelViewState.CrossSection.Height / 2) > height / 2)
                                {
                                    width1 = depth1 = 0;
                                }

                                if (Math.Abs(PanelViewState.CrossSectionOffset.y - PanelViewState.CrossSection.Height / 2) > height / 2)
                                {
                                    width2 = depth2 = 0;
                                }
                                GL.PushMatrix();
                                    GL.Translate(0.0f, PanelViewState.CrossSection.Height / 2, 0.0f);
                                    //use cube as a rect...
                                    GLDrawHelper.DrawCuboidWireframe(width1, 0, depth1);
                                GL.PopMatrix();

                                GL.PushMatrix();
                                    GL.Translate(0.0f, -PanelViewState.CrossSection.Height / 2, 0.0f);
                                    //use cube as a rect...
                                    GLDrawHelper.DrawCuboidWireframe(width2, 0, depth2);
                                GL.PopMatrix();


                                break;
                            }

                        case BoundaryShapes.Cylinder:
                            {

                                float radius, height;
                                radius = spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius;
                                height = spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Height;

                                if (Math.Abs(PanelViewState.CrossSectionOffset.y + PanelViewState.CrossSection.Height / 2) < height / 2)
                                {
                                    GL.PushMatrix();
                                        GL.Translate(0.0f, PanelViewState.CrossSection.Height / 2, 0.0f); 
                                        GL.Rotate(-90.0f, 1.0f, 0.0f, 0.0f);
                                        Glu.PartialDisk(quadricWire, radius, radius, 50, 1, 0, 360);
                                    GL.PopMatrix();
                                }

                                if (Math.Abs(PanelViewState.CrossSectionOffset.y - PanelViewState.CrossSection.Height / 2) < height / 2)
                                {
                                    GL.PushMatrix();
                                        GL.Translate(0.0f, -PanelViewState.CrossSection.Height / 2, 0.0f);
                                        GL.Rotate(-90.0f, 1.0f, 0.0f, 0.0f);
                                        Glu.PartialDisk(quadricWire, radius, radius, 50, 1, 0, 360);
                                    GL.PopMatrix();

                                }

                                GL.PopMatrix();


                                break;

                            }


                        case BoundaryShapes.Sphere:
                            {
                                float radius1, radius2;
                                //widths start being used as a temp for depth
                                radius1 = Math.Abs((PanelViewState.CrossSectionOffset.y + PanelViewState.CrossSection.Height / 2));
                                radius2 = Math.Abs((PanelViewState.CrossSectionOffset.y - PanelViewState.CrossSection.Height / 2));
                                float rad = spatialViewState.InitialSimState.SimulationEnvironment.Boundary.Radius;

                                if (rad * rad - radius1 * radius1 > 0)
                                {
                                    radius1 = (float)Math.Sqrt(rad * rad - radius1 * radius1);
                                }
                                else
                                {
                                    radius1 = 0;
                                }

                                if (rad * rad - radius2 * radius2 > 0)
                                {
                                    radius2 = (float)Math.Sqrt(rad * rad - radius2 * radius2);
                                }
                                else
                                {
                                    radius2 = 0;
                                }

          
                                
                                GL.PushMatrix();
                                    GL.Translate(0.0f,  PanelViewState.CrossSection.Height / 2,0.0f);
                                    GL.Rotate(-90.0f, 1.0f, 0.0f, 0.0f);
                                    Glu.PartialDisk(quadricWire, radius1, radius1, 50, 1, 0, 360);
                                GL.PopMatrix();

                                GL.PushMatrix();
                                    GL.Translate(0.0f, -PanelViewState.CrossSection.Height/ 2,0.0f);
                                    GL.Rotate(-90.0f, 1.0f, 0.0f, 0.0f);
                                    Glu.PartialDisk(quadricWire, radius2, radius2, 50, 1, 0, 360);
                                GL.PopMatrix();
                                //Glu.Sphere(quadricWire, spatialViewState.SimState.SimulationEnvironment.Boundary.Radius, 50, 10);

                                break;
                            }
                    }
                }


            GL.PopMatrix();

            Glu.DeleteQuadric(quadricWire);
      

        }


        /// <summary>
        /// Sets a simple viewport for rendering a background gradient
        /// </summary>
        private void setSimpleView()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, 1, 0, 1,-100, 100);

            // reset modelview matrix stack
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        /// <summary>
        /// Draws a simple background gradient
        /// </summary>
        private void drawSimpleGradient()
        {
            GL.DepthMask(false);
            GL.Disable(EnableCap.CullFace);
            GL.Begin(BeginMode.Quads);
            {
                GL.Color3(0.99f, 0.99f, 0.99f);
                GL.Vertex3(0.0f,0.0f,-10);

                GL.Color3(0.98f, 0.98f, 0.98f);
                GL.Vertex3(0.0f, 1.0f, -10);

                GL.Color3(0.96f, 0.96f, 0.96f);
                GL.Vertex3(1.0f, 1.0f, -10);

                GL.Color3(0.98f, 0.98f, 0.98f);
                GL.Vertex3(1.0f, 0.0f, -10);
            }
            GL.End();



            GL.Enable(EnableCap.CullFace);
            GL.DepthMask(true);
        }


        /// <summary>
        /// Sets the projection view associated with this panel
        /// </summary>
        private void setProjectionView(float width,float height)
        {
            // Reset projection matrix stack
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            const double nRange = 50.0f;


            // Establish clipping volume (left, right, bottom,
            // top, near, far)

            double left, right, bottom, top, near, far;

            near = -nRange*4;
            far = nRange*4;

            left = -nRange * panelViewState.ViewZoom + panelViewState.ViewOffset.x + panelViewState.ViewTempOffset.x;
            right = nRange * panelViewState.ViewZoom + panelViewState.ViewOffset.x + panelViewState.ViewTempOffset.x;

            bottom = -nRange * panelViewState.ViewZoom + panelViewState.ViewOffset.y + panelViewState.ViewTempOffset.y;
            top = nRange * panelViewState.ViewZoom + panelViewState.ViewOffset.y + panelViewState.ViewTempOffset.y;





            switch (view)
            {
                case Views.XY:
                    GL.Ortho(left , right , bottom , top , near, far);
                    break;

                case Views.XZ:
                    //  GL.Ortho(left, right, bottom, top, near, far);
                    GL.Ortho(left , right , bottom , top , -100, 100);

                    GL.Rotate(90.0f, 1.0f, 0.0f, 0.0f);
                    break;

                case Views.ThreeDAnalyzer:
                case Views.ThreeD:
                    //Gl.glFrustum(left, right, bottom, top, nRange / 2, far*2);

                    Glu.Perspective(50.0d, (double)(width/height), 20.0d, 300.0d);
                    break;
            }





            // reset modelview matrix stack
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();


            switch (view)
            {
                case Views.XY:
                    break;

                case Views.XZ:
                    break; 


                case Views.ThreeD:
                    Glu.LookAt(80.0d , 80.0d , 80.0d , 0.0d, 0.0d, 0.0d, 0.0d, 1.0d, 0.0d);
                    GL.Rotate(panelViewState.Ang3D, 0.0f, 1.0f, 0.0f);

                    Boundary bound = spatialViewState.SimParams.InitialState.SimulationEnvironment.Boundary;
                    float depth = (bound.Shape == BoundaryShapes.Cuboid) ? bound.Depth : bound.Radius * 2;


                    panelViewState.CrossSectionOffset.z = depth * 0.48f*(float)Math.Cos(panelViewState.Ang3D*0.1);   
                   
                    break;


                case Views.ThreeDAnalyzer:
                    {

                        Vector3 camPos = panelViewState.currentCamCoords;
                        Vector3 upVect = panelViewState.currentUpVect;
                        Glu.LookAt(camPos.x, camPos.y, camPos.z, 0.0d, 0.0d, 0.0d, upVect.x, upVect.y, upVect.z);
                        GL.Rotate(panelViewState.AngAboutY, 0.0f, 1.0f, 0.0f);

                        panelViewState.CrossSectionOffset.z = 60.0f * (float)Math.Cos(panelViewState.Ang3D * 0.05);
                    }
                    break;
            }

            

        }





        /// <summary>
        /// Sets the view 
        /// </summary>
        public override void SetView(int height, int width)
        {
            // Set viewport to window dimensions.
            GL.Viewport(0, 0, width, height);

            if (spatialViewState.SimParams == null)
            {
                return;
            }

            setProjectionView(width, height);

        }



        /// <summary>
        /// Register a timer tick
        /// </summary>
        public void timer1Tick()
        {
            panelViewState.Animate(view);


        }




    }



    public class CellToSort : IComparable
    {
        public CellInstance cell;
        public float depth;

        public CellToSort(CellInstance cell,float depth)
        {
            this.cell = cell;
            this.depth = depth;
        }


        #region IComparable Members

        public int CompareTo(object obj)
        {

            if (obj is CellToSort)
            {
                CellToSort otherCell = (CellToSort)obj;
                if (this.depth < otherCell.depth)
                {
                    return -1;
                }

                return 1;
            }

            return 0;
           
        }

        #endregion
    }
}
