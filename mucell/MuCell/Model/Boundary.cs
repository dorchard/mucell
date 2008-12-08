using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MuCell.Model
{

    public enum BoundaryShapes { Cuboid = 1, Cylinder, Sphere };


    /// <summary>
    /// This class provides an object representing the boundary of the environment.
    /// </summary>
    public class Boundary
    {

        /// <summary>
        /// Boundary constructor
        /// </summary>
        /// <param name="shape">Boundary geometry</param>
        /// <param name="width">Width of the cuboid</param>
        /// <param name="height">Height of the cuboid / cylinder</param>
        /// <param name="depth">Depth of the cuboid</param>
        /// <param name="radius">Radius of the cylinder / sphere</param>
        public Boundary(BoundaryShapes shape,float width,float height,float depth,float radius)
        {
            this.shape = shape;
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.radius = radius;
        }

        public Boundary()
        {
            this.shape = BoundaryShapes.Cuboid;
            this.width = 0;
            this.height = 0;
            this.depth = 0;
            this.radius = 0;
        }


        //environment boundary settings
        private BoundaryShapes shape;
        [XmlAttribute]
        public BoundaryShapes Shape
        {
            get { return shape; }
            set { shape = value; }
        }

        //radius of sphere / cylinder
        private float radius;
        [XmlAttribute]
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        //height of the cuboid / cylinder
        private float height;
        [XmlAttribute]
        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        //width of the cuboid
        private float width;
        [XmlAttribute]
        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        //depth of the cuboid
        private float depth;
        [XmlAttribute]
        public float Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        /// <summary>
        /// Checks whether or not a given point is inside this boundary.
        /// </summary>
        /// <param name="p">True if the point is inside, false otherwise</param>
        /// <returns></returns>
        public Boolean InsideBoundary(Vector3 p)
        {
            

        
            switch (this.shape)
            {
                case BoundaryShapes.Cuboid:
                    //check the point is inside a cube
                      return (p.x >= -width/2 && p.y >= -height/2 && p.z >= -depth/2 &&
                             p.x <= width/2 && p.y <= height/2 && p.z <= depth/2);

                    break;

                case BoundaryShapes.Cylinder:


                    //check if the point is inside a cylinder
                    Boolean insideCircularCrossection;
                    Boolean insideHeight;

                    insideCircularCrossection =
                        (MathHelper.SquaredDist(p.x, p.z, 0, 0) <= radius * radius);

                   insideHeight =
                        (p.y >= -height/2 && p.y <= height/2);

                    return (insideHeight && insideCircularCrossection);

                    break;

                case BoundaryShapes.Sphere:

                    return MathHelper.SquaredDist(p.x, p.y, p.z, 0, 0, 0) <= radius * radius;

            }

            return false;

        }





    }
}
