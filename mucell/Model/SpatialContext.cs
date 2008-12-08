using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model
{
    	/// <summary>
    	/// A Cartesian (3-dimensional space) class describing aspects of an entity 
    	/// </summary>
    public class SpatialContext : ICloneable
    {

    	// A Vector3 that specifies the location of an entity in Cartesian space

        private Vector3 position;
        public Vector3 Position
       	{
       		get { return position; }
       		set { position = value; }
       	}
       	
       	// A Vector3 that specifies the velocity of an entity
       	// in terms of its X, Y and Z component velocities
        private Vector3 velocity;
        public Vector3 Velocity
       	{
       		get { return velocity; }
       		set { velocity = value; }
       	}
       	
       	 //the squared radius of the cell body in space
    	private float radius;
        public float Radius
       	{
       		get { return radius; }
       		set { radius = value; }
       	}

        //mass of the cell in pico grams
        private float mass;
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

       	
       	// A Vector3 that specifies the orientation of an entity as a unit vector
        // fromo the entities centre to some
    	private Vector3 orientation;
        public Vector3 Orientation
       	{
       		get { return orientation; }
       		set {orientation = value; }
       	}



        /// <summary>
        /// Accelerate in the given direction
        /// </summary>
        /// <param name="direction"></param>
        public void Accelerate(Vector3 direction)
        {
            velocity.x += direction.x;
            velocity.y += direction.y;
            velocity.z += direction.z;
        }


        /// <summary>
        /// Sets the facing of the cell
        /// </summary>
        /// <param name="direction"></param>
        public void Reorientate(Vector3 direction) {
            direction.unitVect();

            orientation.x = direction.x;
            orientation.y = direction.y;
            orientation.z = direction.z;
        }


       	/// <summary>
       	/// Base Constructor
       	/// </summary>
       	/// <param name="position">
       	/// A <see cref="Vector3"/>
       	/// </param>
       	/// <param name="velocity">
       	/// A <see cref="Vector3"/>
       	/// </param>
       	/// <param name="volume">
       	/// A <see cref="Vector3"/>
       	/// </param>
       	/// <param name="orientation">
       	/// A <see cref="Vector3"/>
       	/// </param>
       	public SpatialContext(Vector3 position, Vector3 velocity, float radius, Vector3 orientation, float mass)
       	{
            this.mass = mass;
       		this.position = position;
       		this.velocity = velocity;
       		this.radius = radius;
       		this.orientation = orientation;
          //  this.angularVelocity = angularVelocity;
       	}


        public SpatialContext()
        {
            this.position = new Vector3(0,0,0);
            this.velocity = new Vector3(0, 0, 0);
            this.radius = 0.0f;
            this.orientation = new Vector3(0, 1, 0);
        }

        public bool exptEquals(SpatialContext other)
        {
            if (this.Position.exptEquals(other.Position) == false)
            {
                Console.Write("SpatialContext objects not equal: ");
                Console.WriteLine("this.Position=" + this.Position + "; other.Position=" + other.Position);
                return false;
            }
            if (this.Velocity.exptEquals(other.Velocity) == false)
            {
                Console.Write("SpatialContext objects not equal: ");
                Console.WriteLine("this.Velocity=" + this.Velocity + "; other.Velocity=" + other.Velocity);
                return false;
            }
            if (this.Radius != other.Radius == false)
            {
                Console.Write("SpatialContext objects not equal: ");
                Console.WriteLine("this.Volume=" + this.Radius + "; other.Volume=" + other.Radius);
                return false;
            }
            if (this.Orientation.exptEquals(other.Orientation) == false)
            {
                Console.Write("SpatialContext objects not equal: ");
                Console.WriteLine("this.Orientation=" + this.Orientation + "; other.Orientation=" + other.Orientation);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Clone SpatialContext
        /// </summary>
        /// <returns></returns>
        public Object Clone()
        {
            SpatialContext newInstance = new SpatialContext(this.position, this.velocity, this.radius, this.orientation, this.mass);
            return newInstance;
        }

    }
}
