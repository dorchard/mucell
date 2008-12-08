// ISpatialContextView.cs created with MonoDevelop
// User: dominic:15 23/01/2008
//

using System;

namespace MuCell.Model
{
	
	/// <summary>
	/// View interface for the SpatialContext data object
	/// </summary>
	public interface ISpatialContextView
	{
	
		/// <value>
		/// position vector with read-only access
		/// </value>
		Vector3 position
		{
			get;
		}
		
		/// <value>
		/// velocity vector with read-only access
		/// </value>
       	Vector3 velocity
       	{
       		get;
       	}
       	
       	/// <value>
       	/// volume vector with read-only access
       	/// </value>
       	Vector3 volume
       	{
       		get;
       	}
       	
       	/// <value>
       	/// orientation vector with read-only access
       	/// </value>
       	Vector3 orientation
       	{
       		get;
       	}
	
	}
}
