namespace BulletMLLib
{
	public class BulletMLVanish : BulletMLTask
	{
		#region Members

		#endregion //Members

		#region Properties

		#endregion //Properties

		#region Methods

		public override BLRunStatus Run(BulletMLBullet bullet)
		{
			bullet.Vanish();
			end = true;
			//if(bullet.index == DISP_BULLET_INDEX) Debug.WriteLine("Vanish");
			return BLRunStatus.End;
		}

		#endregion //Methods
	}
}