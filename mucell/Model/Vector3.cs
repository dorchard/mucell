// Vector3.cs created with MonoDevelop
// User: riftor at 10:11 23/01/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Xml.Serialization;

namespace MuCell.Model
{
	
	/// <summary>
	/// A 3 dimensional vector struct
	/// </summary>
	public struct Vector3
	{
        [XmlAttribute]
		public float x, y, z;
		
		public Vector3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		
		/// <summary>
		/// Tests if the vector is within lower and upper bounds
		/// </summary>
		/// <param name="lower">
		/// A <see cref="Vector3"/>
		/// </param>
		/// <param name="upper">
		/// A <see cref="Vector3"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool within(Vector3 lower, Vector3 upper)
		{
			return (this.x>=lower.x && this.y>=lower.y && this.z>=lower.z &&
								this.x<=upper.x && this.y<=upper.y && this.z<=upper.z);
		}

        /// <summary>
        /// Computes the dot-product of this vector with a given vector vect.
        /// </summary>
        /// <param name="vect"></param>
        /// <returns>Dot product</returns>
        public float dotProduct(Vector3 vect)
        {
            return this.x * vect.x + this.y * vect.y + this.z * vect.z;
        }


        /// <summary>
        /// Returns the cross-product of this vector and input vector vect.
        /// </summary>
        /// <param name="vect"></param>
        /// <returns>Cross product</returns>
        public Vector3 crossProduct(Vector3 vect)
        {
            Vector3 crossProd = new Vector3();

            crossProd.x = (this.y * vect.z - this.z * vect.y);
            crossProd.y = (this.z * vect.x - this.x * vect.z);
            crossProd.z = (this.x * vect.y - this.y * vect.x);

            return crossProd;
        }


        /// <summary>
        /// Computes the magnitude of this vector
        /// </summary>
        /// <returns></returns>
        public float magnitude()
        {
            return (float)Math.Sqrt(x * x  + y * y + z * z);
        }



        /// <summary>
        /// Converts this vector into a unit vector
        /// </summary>
        public void unitVect()
        {
            float m = magnitude();

            if(m < 0.000000001)
            {
                this.x=0;
                this.y=0;
                this.z=0;
            } 
            else
            {
                this.x = this.x / m;
                this.y = this.y / m;
                this.z = this.z / m;
            }

        }


        public string ToString()
        {
            return string.Format("({0}, {1}, {2})", x, y, z);
        }

        public bool exptEquals(Vector3 other)
        {
            if (this.x != other.x)
            {
                Console.Write("Vector3 objects not equal: ");
                Console.WriteLine("this.x=" + this.x + "; other.x=" + other.x);
                return false;
            }
            if (this.y != other.y)
            {
                Console.Write("Vector3 objects not equal: ");
                Console.WriteLine("this.y=" + this.y + "; other.y=" + other.y);
                return false;
            }
            if (this.z != other.z)
            {
                Console.Write("Vector3 objects not equal: ");
                Console.WriteLine("this.z=" + this.z + "; other.z=" + other.z);
                return false;
            }
            return true;
        }
	}
}
