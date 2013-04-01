
namespace BulletMLLib
{
	internal class BulletMLVanish : BulletMLTask
	{
		#region Methods

		public override BLRunStatus Run(BulletMLBullet bullet)
		{
			bullet.Vanish();
			end = true;
			return BLRunStatus.End;
		}

		#endregion //Methods
	}
}