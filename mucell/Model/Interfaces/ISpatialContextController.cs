// ISpatialContextController.cs created with MonoDevelop
// User: dominic at 17:16 23/01/2008
//

using System;

namespace MuCell.Model
{
	
	/// <summary>
	/// Controller interface for the SpatialContext data object.
	/// </summary>
	public interface ISpatialContextController : ISpatialContextView
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
