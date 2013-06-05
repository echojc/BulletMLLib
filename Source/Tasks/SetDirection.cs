using System;
using System.Diagnostics;

namespace BulletMLLib
{
	/// <summary>
	/// This task sets the direction of a bullet
	/// </summary>
	public class BulletMLSetDirection : BulletMLTask
	{
		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public BulletMLSetDirection(DirectionNode node, BulletMLTask owner) : base(node, owner)
		{
			Debug.Assert(null != Node);
			Debug.Assert(null != Owner);
		}

		#endregion //Methods
	}
}