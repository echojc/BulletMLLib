using System.Diagnostics;

namespace BulletMLLib
{
	internal class BulletMLVanish : BulletMLTask
	{
		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public BulletMLVanish(BulletMLNode node, BulletMLTask owner) : base(node, owner)
		{
		}
		
		public override ERunStatus Run(Bullet bullet)
		{
			IBulletManager manager = bullet.MyBulletManager;
			Debug.Assert(null != manager);
			manager.RemoveBullet(bullet);
			return ERunStatus.End;
		}

		#endregion //Methods
	}
}