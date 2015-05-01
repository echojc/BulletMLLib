using System;

using BulletMLLib;

namespace BulletMLTests
{
	class Mover : Bullet
	{
		#region Properties

		public override float X { get; set; }
		public override float Y { get; set; }

		public bool Used { get; set; }
		
		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLSample.Mover"/> class.
		/// </summary>
		/// <param name="myBulletManager">My bullet manager.</param>
		public Mover(IBulletManager myBulletManager) : base(myBulletManager)
		{
		}

		public void InitNode()
		{
			Used = true;
		}

		#endregion //Methods
	}
}
