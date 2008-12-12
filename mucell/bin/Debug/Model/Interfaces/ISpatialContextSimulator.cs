using System;

namespace MuCell.Model
{

	public interface ISpatialContextSimulator
	{
	
		/// <value>
		/// position has read-write access
		/// </value>
		Vector3 position
		{
			get;
			set;
		}
		
		/// <value>
		/// velocity has read-write access
		/// </value>
       	Vector3 velocity
       	{
       		get;
       		set;
       	}
       	
       	/// <value>
       	/// volume has read-write access
       	/// </value>
       	Vector3 volume {
       		get;
       		set;
       	}
       	
       	/// <value>
       	/// orientation has read-write access
       	/// </value>
       	Vector3 orientation {
       		get;
       		set;
       	}
	
	}
}