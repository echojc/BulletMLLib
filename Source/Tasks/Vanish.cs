using System.Diagnostics;

namespace BulletMLLib
{
	internal class BulletMLVanish : BulletMLTask
	{
		#region Methods

		public override BLRunStatus Run(Bullet bullet)
		{
			IBulletManager manager = bullet.MyBulletManager;
			Debug.Assert(null != manager);
			manager.RemoveBullet(bullet);
			return BLRunStatus.End;
		}

		#endregion //Methods
	}
}