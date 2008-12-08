using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.OpenGL;
using OpenTK.OpenGL.Enums;
using MuCell.Model;


namespace MuCell.View.OpenGL
{

    /// <summary>
    /// A class providing static methods for drawing a selection of
    /// primitive shapes.
    /// </summary>
    public class GLDrawHelper
    {
        /// <summary>
        /// Draws a cuboid with the desired dimensions
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        public static void DrawCuboid(float width, float height, float depth)
        {
            width /= 2;
            height /= 2;
            depth /= 2;

            GL.Begin(BeginMode.Quads);
            
                //front face
                GL.Vertex3(-width,-height,-depth);
                GL.Vertex3(-width,height,-depth);
                GL.Vertex3(width,height,-depth);
                GL.Vertex3(width,-height,-depth);

                //back face
                GL.Vertex3(-width,-height,depth);
                GL.Vertex3(width,-height,depth);
                GL.Vertex3(width,height,depth);
                GL.Vertex3(-width,height,depth);


                //left face
                GL.Vertex3(-width, -height, -depth);
                GL.Vertex3(-width, -height, depth);
                GL.Vertex3(-width, height, depth);
                GL.Vertex3(-width, height, -depth);

                //right face
                GL.Vertex3(width, -height, -depth);
                GL.Vertex3(width, height, -depth);
                GL.Vertex3(width, height, depth);
                GL.Vertex3(width, -height, depth);

                //top face
                GL.Vertex3(-width, -height, -depth);
                GL.Vertex3(width, -height, -depth);
                GL.Vertex3(width, -height, depth);
                GL.Vertex3(-width, -height, depth);

                //bottom face
                GL.Vertex3(-width, height, -depth);
                GL.Vertex3(-width, height, depth);
                GL.Vertex3(width, height, depth);
                GL.Vertex3(width, height, -depth);
                
                

            GL.End();

        }

        /// <summary>
        /// Draws a shaded disk with the desired dimensions
        /// </summary>
        /// <param name="innerR"></param>
        /// <param name="outerR"></param>
        /// <param name="segs"></param>
        /// <param name="innerCol"></param>
        /// <param name="outerCol"></param>
        public static void DrawShadedDisk(float innerR, float outerR, int segs,
            OpenTK.Math.Vector4 innerCol, OpenTK.Math.Vector4 outerCol)
        {
            double ang;
            int n;
            OpenTK.Math.Vector3 inner = new OpenTK.Math.Vector3(0.0f,0.0f,0.0f);
            OpenTK.Math.Vector3 outer = new OpenTK.Math.Vector3(0.0f,0.0f,0.0f);

            GL.Begin(BeginMode.QuadStrip);

            for(n=0;n<=segs;n++)
            {
                ang = -Math.PI*3 /2.0d +   Math.PI * 2 * n / segs;

                inner.X = (float)Math.Cos(ang);
                inner.Y = (float)Math.Sin(ang);

                outer.X = inner.X * outerR;
                outer.Y = inner.Y * outerR;

                inner.X *= innerR;
                inner.Y *= innerR;

                GL.Color4(innerCol);
                GL.Vertex3(inner);

                GL.Color4(outerCol);
                GL.Vertex3(outer);

            }
            GL.End();
        }

        /// <summary>
        /// Draws a border around a sphere of the given dimensions
        /// </summary>
        /// <param name="height"></param>
        /// <param name="radius"></param>
        /// <param name="segs"></param>
        /// <param name="thickness"></param>
        public static void DrawCylinderBorder(float height, float radius, int segs, float thickness)
        {
            OpenTK.Math.Vector4 innerCol =  new OpenTK.Math.Vector4(0.3f, 0.3f, 0.3f, 0.6f);
            OpenTK.Math.Vector4 outerCol =  new OpenTK.Math.Vector4(0.3f, 0.3f, 0.3f, 0.0f);
            
            GL.Disable(EnableCap.CullFace);


          //  GLDrawHelper.DrawShadedDisk(radius,radius * 1.05f,segs,innerCol,outerCol);

            GL.PushMatrix();
                GL.Translate(0.0f, 0.0f, height/2);
                GLDrawHelper.DrawShadedDisk(radius, radius + thickness, segs, innerCol, outerCol);

            GL.PopMatrix();


            GL.Begin(BeginMode.Quads);

                //left line
                GL.Color4(innerCol);
                GL.Vertex3(-radius, 0, 0);
                GL.Vertex3(-radius, 0, height);

                GL.Color4(outerCol);
                GL.Vertex3(-radius - thickness, 0, height);
                GL.Vertex3(-radius - thickness, 0, 0);


                //right line
                GL.Color4(innerCol);
                GL.Vertex3(radius, 0, 0);
                GL.Vertex3(radius, 0, height);

                GL.Color4(outerCol);
                GL.Vertex3(radius + thickness, 0, height);
                GL.Vertex3(radius + thickness, 0, 0);

                //top line
                GL.Color4(innerCol);
                GL.Vertex3(-radius, 0, height);
                GL.Vertex3(radius, 0, height);

                GL.Color4(outerCol);
                GL.Vertex3(radius, 0, height + thickness);
                GL.Vertex3(-radius, 0, height + thickness);

                //Bottom line
                GL.Color4(innerCol);
                GL.Vertex3(-radius, 0, 0);
                GL.Vertex3(radius, 0, 0);

                GL.Color4(outerCol);
                GL.Vertex3(radius, 0, - thickness);
                GL.Vertex3(-radius, 0, - thickness);

            GL.End();

            //corner pieces
            GL.Begin(BeginMode.Triangles);
                //left top
                GL.Color4(innerCol);
                GL.Vertex3(-radius, 0, height);
                GL.Color4(outerCol);
                GL.Vertex3(-radius - thickness, 0, height);
                GL.Vertex3(-radius, 0, height + thickness);

                //right top
                GL.Color4(innerCol);
                GL.Vertex3(radius, 0, height);
                GL.Color4(outerCol);
                GL.Vertex3(radius + thickness, 0, height);
                GL.Vertex3(radius, 0, height + thickness);

                //left bottom
                GL.Color4(innerCol);
                GL.Vertex3(-radius, 0, 0);
                GL.Color4(outerCol);
                GL.Vertex3(-radius - thickness, 0, 0);
                GL.Vertex3(-radius, 0, - thickness);

                //right bottom
                GL.Color4(innerCol);
                GL.Vertex3(radius, 0, 0);
                GL.Color4(outerCol);
                GL.Vertex3(radius + thickness, 0, 0);
                GL.Vertex3(radius, 0, - thickness);

            GL.End();


            GL.Enable(EnableCap.CullFace);
        }



        /// <summary>
        /// Draws a border around a cuboid of the given dimensions
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        /// <param name="thickness"></param>
        public static void DrawCuboidBorder(float width, float height, float depth, float thickness)
        {
            width /= 2;
            height /= 2;
            depth /= 2;

            OpenTK.Math.Vector4 innerCol = new OpenTK.Math.Vector4(0.3f, 0.3f, 0.3f, 0.6f);
            OpenTK.Math.Vector4 outerCol = new OpenTK.Math.Vector4(0.3f, 0.3f, 0.3f, 0.0f);

            GL.Disable(EnableCap.CullFace);
            GL.Begin(BeginMode.Quads);

                //left line
                GL.Color4(innerCol);
                GL.Vertex3(-width, -height, 0);
                GL.Vertex3(-width, height, 0);

                GL.Color4(outerCol);
                GL.Vertex3(-width-thickness, height, 0);
                GL.Vertex3(-width-thickness, -height, 0);


                //top line
                GL.Color4(innerCol);
                GL.Vertex3(-width, -height, 0);
                GL.Vertex3(width, -height, 0);

                GL.Color4(outerCol);
                GL.Vertex3(width, -height-thickness, 0);
                GL.Vertex3(-width, -height - thickness, 0);


                //Right line
                GL.Color4(innerCol);
                GL.Vertex3(width, -height, 0);
                GL.Vertex3(width, height, 0);

                GL.Color4(outerCol);
                GL.Vertex3(width+thickness, height, 0); 
                GL.Vertex3(width+thickness, -height, 0);

                //bottom line
                GL.Color4(innerCol);
                GL.Vertex3(width, height, 0);
                GL.Vertex3(-width, height, 0);

                GL.Color4(outerCol);
                GL.Vertex3(-width, height+thickness, 0);
                GL.Vertex3(width, height + thickness, 0);






                //left side line
                GL.Color4(innerCol);
                GL.Vertex3(-width, 0, -depth);
                GL.Vertex3(-width, 0, depth);

                GL.Color4(outerCol);
                GL.Vertex3(-width - thickness, 0, depth);
                GL.Vertex3(-width - thickness, 0, -depth);


                //back line
                GL.Color4(innerCol);
                GL.Vertex3(-width, 0, -depth);
                GL.Vertex3(width, 0, -depth);

                GL.Color4(outerCol);
                GL.Vertex3(width, 0, -depth - thickness);
                GL.Vertex3(-width, 0 , -depth - thickness);


                //right side line
                GL.Color4(innerCol);
                GL.Vertex3(width, 0, -depth);
                GL.Vertex3(width, 0, depth);

                GL.Color4(outerCol);
                GL.Vertex3(width + thickness, 0, depth);
                GL.Vertex3(width + thickness, 0, -depth);

                //front line
                GL.Color4(innerCol);
                GL.Vertex3(width, 0, depth);
                GL.Vertex3(-width, 0, depth);

                GL.Color4(outerCol);
                GL.Vertex3(-width,0, depth + thickness);
                GL.Vertex3(width,0, depth + thickness);

            GL.End();


            GL.Begin(BeginMode.Triangles);

            //top left
            GL.Color4(innerCol);
            GL.Vertex3(-width, -height, 0);
            GL.Color4(outerCol);
            GL.Vertex3(-width - thickness, -height, 0);
            GL.Vertex3(-width, -height - thickness, 0);

            //top right
            GL.Color4(innerCol);
            GL.Vertex3(width, -height, 0);
            GL.Color4(outerCol);
            GL.Vertex3(width + thickness, -height, 0);
            GL.Vertex3(width, -height - thickness, 0);

            //bottom left
            GL.Color4(innerCol);
            GL.Vertex3(-width, height, 0);
            GL.Color4(outerCol);
            GL.Vertex3(-width - thickness, height, 0);
            GL.Vertex3(-width, height + thickness, 0);


            //bottom right
            GL.Color4(innerCol);
            GL.Vertex3(width, height, 0);
            GL.Color4(outerCol);
            GL.Vertex3(width, height + thickness, 0);
            GL.Vertex3(width + thickness, height, 0);

            //back left
            GL.Color4(innerCol);
            GL.Vertex3(-width, 0, -depth);
            GL.Color4(outerCol);
            GL.Vertex3(-width, 0, -depth - thickness);
            GL.Vertex3(-width - thickness, 0, -depth);

            
            //back right
            GL.Color4(innerCol);
            GL.Vertex3(width, 0, -depth);
            GL.Color4(outerCol);
            GL.Vertex3(width, 0, -depth - thickness);
            GL.Vertex3(width + thickness, 0, -depth);

            //front right
            GL.Color4(innerCol);
            GL.Vertex3(width, 0, depth);
            GL.Color4(outerCol);
            GL.Vertex3(width, 0, depth + thickness);
            GL.Vertex3(width + thickness, 0, depth);

            
            //front left
            GL.Color4(innerCol);
            GL.Vertex3(-width, 0, depth);
            GL.Color4(outerCol);
            GL.Vertex3(-width, 0, depth + thickness);
            GL.Vertex3(-width - thickness, 0, depth);

            GL.Vertex3(-width, 0, depth);

            GL.End();
            GL.Enable(EnableCap.CullFace);
        }





        /// <summary>
        /// Draws a border around a sphere of the given dimensions
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="segs"></param>
        /// <param name="thickness"></param>
        public static void DrawSphereBorder(float radius, int segs, float thickness)
        {
            OpenTK.Math.Vector4 innerCol = new OpenTK.Math.Vector4(0.3f, 0.3f, 0.3f, 0.6f);
            OpenTK.Math.Vector4 outerCol = new OpenTK.Math.Vector4(0.3f, 0.3f, 0.3f, 0.0f);

            GL.Disable(EnableCap.CullFace);

            GLDrawHelper.DrawShadedDisk(radius, radius + thickness, segs, innerCol, outerCol);
         
           
            GL.PushMatrix();
                GL.Rotate(90.0f, 1.0f, 0.0f, 0.0f);
                GLDrawHelper.DrawShadedDisk(radius, radius + thickness, segs, innerCol, outerCol);
            GL.PopMatrix();
            

   

            GL.Enable(EnableCap.CullFace);
        }


        /// <summary>
        /// Draws a cuboid wireframe
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        public static void DrawCuboidWireframe(float width, float height, float depth)
        {
            width = width / 2;
            height = height / 2;
            depth = depth / 2;
            DrawWireframeBox(-width, -height, -depth, width, height, depth);
        }


        /// <summary>
        /// Draws a wireframe cube given the bottom left back corner and the top right front corner.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="z1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="z2"></param>
        public static void DrawWireframeBox(float x1, float y1, float z1, float x2, float y2, float z2)
        {
         
            /*
            float width = Math.Min( x2 - x1,5.0f);
            float height = Math.Min( y2 - y1,5.0f);
            float depth = Math.Min( z2 - z1,5.0f);*/

            float width = x2 - x1;
            float height = y2 - y1;
            float depth = z2 - z1;

            GL.Begin(BeginMode.Lines);
                GL.Vertex3(x1, y1, z1);
                GL.Vertex3(x1+width, y1, z1);

                GL.Vertex3(x1, y1, z1);
                GL.Vertex3(x1, y1, z1+depth);

                GL.Vertex3(x1, y1, z1);
                GL.Vertex3(x1, y1+height, z1);


                GL.Vertex3(x2, y2, z2);
                GL.Vertex3(x2 - width, y2, z2);

                GL.Vertex3(x2, y2, z2);
                GL.Vertex3(x2, y2, z2 - depth);

                GL.Vertex3(x2, y2, z2);
                GL.Vertex3(x2, y2- height, z2);


                GL.Vertex3(x2 - width, y2, z2);
                GL.Vertex3(x2 - width, y2-height, z2);

                GL.Vertex3(x2 - width, y2, z2);
                GL.Vertex3(x2 - width, y2, z2 - depth);

                GL.Vertex3(x2, y2 - height, z2);
                GL.Vertex3(x2 - width, y2 - height, z2);

                GL.Vertex3(x2, y2 - height, z2);
                GL.Vertex3(x2, y2 - height, z2 - depth);

                GL.Vertex3(x2, y2, z2 - depth);
                GL.Vertex3(x2 - width, y2, z2 - depth);

                GL.Vertex3(x2, y2, z2 - depth);
                GL.Vertex3(x2, y2-height, z2 - depth);

            GL.End();


        }


        /// <summary>
        /// Sets the OpenGL modelview matrix to obtain a basic billboarding effect
        /// </summary>
        /// <param name="view"></param>
        public static void BillboardCheatSphericalBegin(Views view) {
	
	        float[] modelview = new float[16];
	        int i,j;

	        // save the current modelview matrix
            GL.PushMatrix();


            if (view == Views.XY)
            {
                return;
            }

            if (view == Views.XZ)
            {
                GL.Rotate(-90.0f, 1.0f, 0.0f, 0.0f);
                return;
            }


	        // get the current modelview matrix
            GL.GetFloat(GetPName.ModelviewMatrix, modelview);

	        // undo all rotations
	        // beware all scaling is lost as well 
	        for( i=0; i<3; i++ ) 
	            for( j=0; j<3; j++ ) {
		        if ( i==j )
		            modelview[i*4+j] = 1.0f;
		        else
		            modelview[i*4+j] = 0.0f;
	            }

	        // set the modelview with no rotationss
            GL.LoadMatrix(modelview);
        }



        /// <summary>
        /// Sets the OpenGL modelview matrix to obtain an advanced billboarding effect
        /// </summary>
        public static void BillboardBegin()
        {

            Vector3 objectPos;
            Vector3 XZProg;
            Vector3 lookAt;
            Vector3 upAux;
            float angCos;
            float[] a = new float[16];

            GL.PushMatrix();

            GL.GetFloat(GetPName.ModelviewMatrix, a);
            objectPos.x =  -(a[0] * a[12] + a[1] * a[13] + a[2] * a[14]);
            objectPos.y =  -(a[4] * a[12] + a[5] * a[13] + a[6] * a[14]);
            objectPos.z =  -(a[8] * a[12] + a[9] * a[13] + a[10] * a[14]);


            XZProg.x = objectPos.x;
            XZProg.y = 0.0f;
            XZProg.z = objectPos.z;

            lookAt.x = 0.0f;
            lookAt.y = 0.0f;
            lookAt.z = 1.0f;

            XZProg.unitVect();

            upAux = lookAt.crossProduct(XZProg);
            angCos = lookAt.dotProduct(XZProg);



            if (angCos < 0.9999 && angCos > -0.9999)
            {
                
                GL.Rotate(Math.Acos(angCos) * 180 / Math.PI, upAux.x, upAux.y, upAux.z);	
            }

            objectPos.unitVect();
            angCos = XZProg.dotProduct(objectPos);

            
            if (angCos < 0.9999 && angCos > -0.9999)
            {
                if (objectPos.y < 0) 
                  GL.Rotate(Math.Acos(angCos) * 180 / Math.PI, 1.0f, 0.0f, 0.0f);
                else 
                   GL.Rotate(Math.Acos(angCos) * 180 / Math.PI, -1.0f, 0.0f, 0.0f);
            }
        }

        /// <summary>
        /// Wrapper for ending the billboarding effect
        /// </summary>
        public static void BillboardEnd()
        {
            GL.PopMatrix();
        }

    }
}



