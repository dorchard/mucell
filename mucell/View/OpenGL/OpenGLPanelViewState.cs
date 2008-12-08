using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using MuCell.Model;

namespace MuCell.View.OpenGL
{

    public enum CrossSectionFacingDirection { Front = 0, Top };
    public enum CameraPosition { Corner = 0, Front, Top };
    public enum AnimationState { None = 0, FlyToResting, FlyToNext };

    /// <summary>
    /// This class represents the state of a particularly OpenGLPanel (view).
    /// </summary>
    public class OpenGLPanelViewState
    {

        //angle of a 3D view
        private float ang3D;
        [XmlAttribute]
        public float Ang3D
        {
            get { return ang3D; }
            set { ang3D = value; }
        }



        //angle of a 3D view
        private float angAboutY;
        [XmlAttribute]
        public float AngAboutY
        {
            get { return angAboutY; }
            set
            {
                angAboutY = value;

                while (angAboutY > (float)(360))
                {
                    angAboutY -= (float)(360);
                }

                while (angAboutY < 0)
                {
                    angAboutY += (float)(360);
                }

            }
        }

        //camera animation variables
        public float oldAngAboutY;
        public float oldZoomLinear;
        public Vector3 currentCamCoords;
        public Vector3 oldCamCoords;
        public Vector3 currentUpVect;
        public Vector3 oldUpVect;

        public bool drawNutrientInitialPos;


        //current position of the camera
        private CameraPosition currentCameraPos;
        public CameraPosition CurrentCameraPos
        {
            get { return currentCameraPos; }
            set { currentCameraPos = value; }
        }

        //where the camera is moving to
        private CameraPosition nextCameraPos;
        public CameraPosition NextCameraPos
        {
            get { return nextCameraPos; }
            set { nextCameraPos = value; }
        }

        //animation state of the camera
        private AnimationState animation;
        public AnimationState Animation
        {
            get { return animation; }
            set { animation = value; }
        }

        //animation counter to determine how far into an animation the view is
        private float animationCounter;
        public float AnimationCounter
        {
            get { return animationCounter; }
            set { animationCounter = value; }
        }

        //whether or not the 3D view is limited to a crossectional display
        private Boolean crossSectionEnabled;
        public Boolean CrossSectionEnabled
        {
            get { return crossSectionEnabled; }
            set { crossSectionEnabled = value; }
        }

        //the direction in which the cross section is oriented
        private CrossSectionFacingDirection crossSectionFacing;
        public CrossSectionFacingDirection CrossSectionFacing
        {
            get { return crossSectionFacing; }
            set { crossSectionFacing = value; }
        }


        //a cuboid boundary describing the cross sectional area (size)
        private Boundary crossSection;
        public Boundary CrossSection
        {
            get { return crossSection; }
            set { crossSection = value; }
        }
        
        //the offset of the cross sectional area (position)
        public Vector3 CrossSectionOffset;



        //zoom on XY / XZ views - actual scale
        private float viewZoom;
        [XmlAttribute]
        public float ViewZoom
        {
            get { return viewZoom; }
            set { viewZoom = value; }
        }

        //zoom on XY / XZ view - linear scale
        private float viewZoomLinear;
        [XmlAttribute]
        public float ViewZoomLinear
        {
            get { return viewZoomLinear; }
            set { viewZoomLinear = value; }

        }


        //scrolling on XY / XZ views
        private Vector2 viewOffset;
        public Vector2 ViewOffset
        {
            get { return viewOffset; }
            set { viewOffset = value; }
        }
        //scrolling on XY / XZ views - tempory value
        private Vector2 viewTempOffset;
        public Vector2 ViewTempOffset
        {
            get { return viewTempOffset; }
            set { viewTempOffset = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenGLPanelViewState()
        {
            ang3D = 0;
            viewZoom = 1;
            viewZoomLinear = 0;

            viewOffset = new Vector2(0.0f, 0.0f);
            viewTempOffset = new Vector2(0.0f, 0.0f);

            drawNutrientInitialPos = false;

             crossSection = new Boundary(BoundaryShapes.Cuboid, 100, 100, 5, 0);
            crossSectionFacing = CrossSectionFacingDirection.Front;
            CrossSectionOffset = new Vector3(0,0,0);
            crossSectionEnabled = false;


            currentCameraPos = CameraPosition.Corner;
            nextCameraPos = CameraPosition.Front;
            animationCounter = 0.0f;
            angAboutY = 0.0f;
            oldAngAboutY = 0.0f;

            currentCamCoords = new Vector3(80.0f, 80.0f, 80.0f);
            oldCamCoords = new Vector3(0.0f, 0.0f, 0.0f);
            currentUpVect = new Vector3(0.0f, 1.0f, 0.0f);
            oldUpVect = new Vector3(0.0f, 0.0f, 0.0f);

        }

        /// <summary>
        /// Updates the size of the cross section given a boundary
        /// </summary>
        /// <param name="bounds"></param>
        public void UpdateCrossSectionSize(Boundary bounds)
        {
            UpdateCrossSectionSize(this.crossSectionFacing, bounds);
        }

        /// <summary>
        /// updates the size of the cross section given a facing and boundary
        /// </summary>
        /// <param name="facing"></param>
        /// <param name="bounds"></param>
        public void UpdateCrossSectionSize(CrossSectionFacingDirection facing, Boundary bounds)
        {
            float width = 0;
            float height = 0;
            float depth = 0;

            this.crossSectionFacing = facing;

            switch (bounds.Shape)
            {

                case BoundaryShapes.Cuboid:

                    width = bounds.Width;
                    height = bounds.Height;
                    depth = bounds.Depth;

                    break;

                case BoundaryShapes.Cylinder:

                    width = depth = bounds.Radius * 2;
                    height = bounds.Height;

                    break;

                case BoundaryShapes.Sphere:

                    width = height = depth = bounds.Radius * 2;

                    break;

            }

            switch (facing)
            {

                case CrossSectionFacingDirection.Front:
                    crossSection = new Boundary(BoundaryShapes.Cuboid, width * 1.3f, height * 1.3f, depth*0.05f, 0);
                    break;

                case CrossSectionFacingDirection.Top:
                    crossSection = new Boundary(BoundaryShapes.Cuboid, width * 1.3f, height * 0.05f, depth * 1.3f, 0);
                    break;

            }



        }




        /// <summary>
        /// Sets the temporary scroll offset.
        /// </summary>
        /// <param name="px">Horizontal offset</param>
        /// <param name="py">Vertical offset</param>
        /// <param name="panel">The panel object</param>
        public void SetTempScrollOffset(int px, int py,OpenGLCellPlacementPanel panel)
        {
            float x = (float)px;
            float y = (float)py;

            x /= panel.GetWidth();
            y /= panel.GetHeight();

            x *= -100;
            y *= 100;

            viewTempOffset.x = x*viewZoom;
            viewTempOffset.y = y*viewZoom;


        }

        /// <summary>
        /// Updates the scroll offset
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <param name="panel"></param>
        public void UpdateScrollOffset(int px, int py, OpenGLCellPlacementPanel panel)
        {
            float x = (float)px;
            float y = (float)py;

            x /= panel.GetWidth();
            y /= panel.GetHeight();

            x *= -100;
            y *= 100;

            viewTempOffset.x = 0;
            viewTempOffset.y = 0;
            viewOffset.x += x * viewZoom;
            viewOffset.y += y * viewZoom;
        }

        /// <summary>
        /// Zoom in in the current context
        /// </summary>
        public void ZoomIn()
        {


            viewZoomLinear -= 0.2f;
            if(viewZoomLinear < - 20){
                viewZoomLinear = -20f;
            }

            viewZoom = (float)Math.Pow(2,viewZoomLinear);
        

        }

        /// <summary>
        /// Zoom out in the current context
        /// </summary>
        public void ZoomOut()
        {

            viewZoomLinear += 0.2f;
            if (viewZoomLinear > 1.4)
            {
                viewZoomLinear = 1.4f;
            }

            viewZoom = (float)Math.Pow(2, viewZoomLinear);


        }






        /// <summary>
        /// Convert input X (in the range [0,1]) to a smooth cosine funtion
        /// varying from  0 to 1.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private float cosHelper(float x)
        {
            return (float)((1.0d - Math.Cos(x * Math.PI)) / 2);
        }

        /// <summary>
        /// Return fixed camera locations for each position
        /// </summary>
        /// <param name="camPos"></param>
        /// <returns></returns>
        private Vector3 CamPosHelper(CameraPosition camPos)
        {
            Vector3 pos = new Vector3();

            switch (camPos)
            {
                case CameraPosition.Corner:
                    pos.x = pos.y = pos.z = 80.0f;
                    break;

                case CameraPosition.Front:
                    pos.x = 0.0f;
                    pos.y = 0.0f;
                    pos.z = 160.0f;
                    break;

                case CameraPosition.Top:
                    pos.x = 0.0f;
                    pos.y = 160.0f;
                    pos.z = 0.0f;
                    break;

            }

            return pos;
        }


        /// <summary>
        /// Return fixed camera up vector for each location
        /// </summary>
        /// <param name="camPos"></param>
        /// <returns></returns>
        private Vector3 CamUpVectHelper(CameraPosition camPos)
        {
            Vector3 pos = new Vector3();

            switch (camPos)
            {
                case CameraPosition.Corner:
                    pos.x = pos.z = 0.0f;
                    pos.y = 1.0f;
                    break;

                case CameraPosition.Front:
                    pos.x = pos.z = 0.0f;
                    pos.y = 1.0f;
                    break;

                case CameraPosition.Top:
                    pos.x = pos.y = 0.0f;
                    pos.z = -1.0f;
                    break;

            }

            return pos;
        }

        //note: set newCameraPos to current camera pos if you want to just return to the rest position
        public void BeginAnimationToNewCameraPos(CameraPosition newCameraPosition)
        {


                animation = AnimationState.FlyToResting;
                nextCameraPos = newCameraPosition;
                oldAngAboutY = angAboutY;
                oldZoomLinear = ViewZoomLinear;
                animationCounter = 0.0f;

        }

        /// <summary>
        /// Animation polling method
        /// </summary>
        /// <param name="view"></param>
        public void Animate(Views view)
        {
           


            if (view == Views.ThreeDAnalyzer)
            {
                Vector3 restCoords = CamPosHelper(CurrentCameraPos);
                Vector3 restUpVect = CamUpVectHelper(CurrentCameraPos);


                currentCamCoords.x = restCoords.x;
                currentCamCoords.y = restCoords.y;
                currentCamCoords.z = restCoords.z;

                currentUpVect.x = restUpVect.x;
                currentUpVect.y = restUpVect.y;
                currentUpVect.z = restUpVect.z;


                if (CurrentCameraPos == CameraPosition.Corner)
                {

                    currentCamCoords.x += ViewZoomLinear * 10;
                    currentCamCoords.y += ViewZoomLinear * 10;
                    currentCamCoords.z += ViewZoomLinear * 10;

                    if (Animation == AnimationState.None)
                    {
                        AngAboutY += 0.5f;
                    }
                }


                if (Animation == AnimationState.FlyToResting)
                {


                    if (CurrentCameraPos == CameraPosition.Corner)
                    {
                        //Rotate along smallest angle to 0 (or 360).
                        AngAboutY = (oldAngAboutY < 180) ? oldAngAboutY * (1 - cosHelper(AnimationCounter)) :
                                                           oldAngAboutY + (360 - oldAngAboutY) * (cosHelper(AnimationCounter));
                        ViewZoomLinear = oldZoomLinear * (1 - cosHelper(AnimationCounter));
                    }



                    AnimationCounter += 0.04f;
                    if (AnimationCounter > 1.0f)
                    {
                        AnimationCounter = 0.0f;

                        //animation over, just going to rest
                        if (NextCameraPos == CurrentCameraPos)
                        {
                            Animation = AnimationState.None;
                        }
                        else
                        {
                            //otherwise we wish to fly to a new camera position
                            Animation = AnimationState.FlyToNext;
                            oldCamCoords = currentCamCoords;
                            oldUpVect = currentUpVect;
                            AngAboutY = 0.0f;
                        }
                        
                    }

     


                }


        


                if (Animation == AnimationState.FlyToNext)
                {
                    Vector3 newRestCoords = CamPosHelper(NextCameraPos);
                    Vector3 newUpVect = CamUpVectHelper(NextCameraPos);

                    AnimationCounter += 0.04f;
                    if (AnimationCounter > 1.0f)
                    {
                        AnimationCounter = 1.0f;
                        CurrentCameraPos = NextCameraPos;
                        Animation = AnimationState.None;
                    }

                    currentCamCoords.x = oldCamCoords.x * (1 - cosHelper(AnimationCounter)) + newRestCoords.x * cosHelper(AnimationCounter);
                    currentCamCoords.y = oldCamCoords.y * (1 - cosHelper(AnimationCounter)) + newRestCoords.y * cosHelper(AnimationCounter);
                    currentCamCoords.z = oldCamCoords.z * (1 - cosHelper(AnimationCounter)) + newRestCoords.z * cosHelper(AnimationCounter);
                    
                    currentUpVect.x = oldUpVect.x * (1 - cosHelper(AnimationCounter)) + newUpVect.x * cosHelper(AnimationCounter);
                    currentUpVect.y = oldUpVect.y * (1 - cosHelper(AnimationCounter)) + newUpVect.y * cosHelper(AnimationCounter);
                    currentUpVect.z = oldUpVect.z * (1 - cosHelper(AnimationCounter)) + newUpVect.z * cosHelper(AnimationCounter);

                   // TestRigs.ErrorLog.LogError("Current up vect: x + " + currentUpVect.x + " y: " + currentUpVect.y + " z: " + currentUpVect.z);
                  //  TestRigs.ErrorLog.LogError("Rext: x + " + currentCamCoords.x + " y: " + currentCamCoords.y + " z: " + currentCamCoords.z);
                
                   

                }



            }
        }



    }
}
