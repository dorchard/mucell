using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using MuCell.Model;

namespace MuCell.View.OpenGL
{

    /// <summary>
    /// This class specifies the common state of the environment being viewed from
    /// all three perspectives in the SpatialConfigurationPanelUI. It provides access
    /// to the initial state of the cell system, as well as tool information common to
    /// all three views (such as cell grouping, grid layout, pointer position etc.).
    /// </summary>
    public class SpatialViewState
    {



        //bottom left back corner of the box surrounding the selected group
        private Vector3 selectedGroupBoxCorner1;
        public Vector3 SelectedGroupBoxCorner1
        {
            get { return selectedGroupBoxCorner1; }
            set { selectedGroupBoxCorner1 = value; }
        }

        //top right front corner of the box surrounding the selected group
        private Vector3 selectedGroupBoxCorner2;
        public Vector3 SelectedGroupBoxCorner2
        {
            get { return selectedGroupBoxCorner2; }
            set { selectedGroupBoxCorner2 = value; }
        }


        //Group ID of the selected group
        private int selectedGroupIndex;
        [XmlAttribute]
        public int SelectedGroupIndex
        {
            get { return selectedGroupIndex; }
            set { selectedGroupIndex = value; }
        }

        //ID of the selected nutrient
        private int selectedNutrientIndex;
        [XmlAttribute]
        public int SelectedNutrientIndex
        {
            get { return selectedNutrientIndex; }
            set { selectedNutrientIndex = value; }
        }


        //whether or not the cursor is in the boundary area
        private Boolean crossHairOutOfBounds;
        [XmlAttribute]
        public Boolean CrossHairOutOfBounds
        {
            get { return crossHairOutOfBounds; }
            set { crossHairOutOfBounds = value; }
        }


        



        private Vector3 crossHairPosition;
        public Vector3 CrossHairPosition
        {
            get { return crossHairPosition; }
            set { crossHairPosition = value; }
        }

        private OpenGLPanelViewState xY;
        public OpenGLPanelViewState XY
        {
            get { return xY; }
            set { xY = value; }
        }

        private OpenGLPanelViewState xZ;
        public OpenGLPanelViewState XZ
        {
            get { return xZ; }
            set { xZ = value; }
        }

        private OpenGLPanelViewState threeD;
        public OpenGLPanelViewState ThreeD
        {
            get { return threeD; }
            set { threeD = value; }
        }

        private OpenGLPanelViewState threeDAnalyzer;
        public OpenGLPanelViewState ThreeDAnalyzer
        {
            get { return threeDAnalyzer; }
            set { threeDAnalyzer = value; }
        }


        //a pointer to the simulation parameters of the currently selected simulation
        private SimulationParameters simParams;
        public SimulationParameters SimParams
        {
            get { return simParams; }
            set { simParams = value; }

        }

        //pointer to current state snapshot to display
        private StateSnapshot initialSimState;
        public StateSnapshot InitialSimState
        {
            get { return initialSimState; }
            set { initialSimState = value; }

        }


        //pointer to current state snapshot to display
        private StateSnapshot currentSimState;
        public StateSnapshot CurrentSimState
        {
            get { return currentSimState; }
            set { currentSimState = value; }

        }

        public float NutrientIntensityScale; 


        /// <summary>
        /// Constructor
        /// </summary>
        public SpatialViewState()
        {
            crossHairPosition.x = 0;
            crossHairPosition.y = 0;
            crossHairPosition.z = 0;
            crossHairOutOfBounds = false;
            XY = new OpenGLPanelViewState();
            XZ = new OpenGLPanelViewState();
            ThreeD = new OpenGLPanelViewState();
            ThreeDAnalyzer = new OpenGLPanelViewState();
            selectedGroupIndex = -1;
            selectedNutrientIndex = -1;

            NutrientIntensityScale = 1.0f; 

            ThreeD.CrossSectionEnabled = false;

            SimParams = null;

        }

        /// <summary>
        /// Updates the boundaries of the box surrounding the currently selected group
        /// </summary>
        public void UpdateSelectedGroupBox()
        {
            
            if (selectedGroupIndex != -1)
            {
                float minX = 1000000, minY = 1000000, minZ = 1000000;
                float maxX = -1000000, maxY = -1000000, maxZ = -1000000;

                if (InitialSimState.SimulationEnvironment.CellsFromGroup(SelectedGroupIndex).Count == 0)
                {

                    selectedGroupBoxCorner1.x = 0;
                    selectedGroupBoxCorner1.y = 0;
                    selectedGroupBoxCorner1.z = 0;

                    selectedGroupBoxCorner2.x = 0;
                    selectedGroupBoxCorner2.y = 0;
                    selectedGroupBoxCorner2.z = 0;
                }

                foreach (CellInstance cell in InitialSimState.SimulationEnvironment.CellsFromGroup(SelectedGroupIndex))
                {
                    minX = Math.Min(minX, cell.CellInstanceSpatialContext.Position.x);
                    minY = Math.Min(minY, cell.CellInstanceSpatialContext.Position.y);
                    minZ = Math.Min(minZ, cell.CellInstanceSpatialContext.Position.z);

                    maxX = Math.Max(maxX, cell.CellInstanceSpatialContext.Position.x);
                    maxY = Math.Max(maxY, cell.CellInstanceSpatialContext.Position.y);
                    maxZ = Math.Max(maxZ, cell.CellInstanceSpatialContext.Position.z);
                }

                selectedGroupBoxCorner1.x = minX;
                selectedGroupBoxCorner1.y = minY;
                selectedGroupBoxCorner1.z = minZ;

                selectedGroupBoxCorner2.x = maxX;
                selectedGroupBoxCorner2.y = maxY;
                selectedGroupBoxCorner2.z = maxZ;

            }


        }



        /// <summary>
        /// Update whether or not the cross hairs location is out of bounds
        /// </summary>
        public void UpdateCursorOutOfBounds()
        {
            crossHairOutOfBounds = InitialSimState.SimulationEnvironment.Boundary.InsideBoundary(CrossHairPosition);

        }


        /// <summary>
        /// sets the position of the cross hair in the environment
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="panel"></param>
        /// <param name="v"></param>
        public void setPointerPosition(float x, float y, OpenGLCellPlacementPanel panel, Views v)
        {
            Vector2 viewOffset;
            Vector2 viewTempOffset;
            float viewZoom;

            switch(v)
            {
                case Views.XY:
                    viewOffset = XY.ViewOffset;
                    viewTempOffset = XY.ViewTempOffset;
                    viewZoom = XY.ViewZoom;
                    break;

                case Views.XZ:
                    viewOffset = XZ.ViewOffset;
                    viewTempOffset = XZ.ViewTempOffset;
                    viewZoom = XZ.ViewZoom;
                    break;

                default:
                    return;

            }

            x -= panel.Padding.Left;
            y -= panel.Padding.Top;

            x -= 4;
            y -= 2;


            x /= panel.GetWidth();
            y /= panel.GetHeight();
     
            x *= 100;
            y *= 100;

            x -= 50;
            x *= viewZoom;
            x += viewOffset.x + viewTempOffset.x;
            



            //spatialViewState.CrossHairPosition.x = 10;

            if (v == Views.XY)
            {

                y = 50 - y;
                y *= viewZoom;
                y += viewOffset.y + viewTempOffset.y;
                

                crossHairPosition.x = x;
                crossHairPosition.y = y;
            }
            else if (v == Views.XZ)
            {
                y -= 50;
                y *= viewZoom;
                y -= viewOffset.y + viewTempOffset.y;
                

                crossHairPosition.x = x;
                crossHairPosition.z = y;
            }

            UpdateCursorOutOfBounds();

        }


    }
}
