namespace BulletMLLib
{
	/// <summary>
	/// 方向設定
	/// </summary>
	public class BulletMLSetDirection : BulletMLTask
	{
		#region Members

		public BulletMLTree node;

		#endregion //Members

		#region Properties

		#endregion //Properties

		#region Methods

		public BulletMLSetDirection(BulletMLTree node)
		{
			this.node = node;
		}

		public override BLRunStatus Run(BulletMLBullet bullet)
		{
			BLType blType = node.type;
			float value = (float)(node.GetValue(this) * Math.PI / 180);

			if (blType == BLType.Sequence)
			{
				bullet.Direction = bullet.GetFireData().srcDir + value;
			}
			else if (blType == BLType.Absolute)
			{
				bullet.Direction = value;
			}
			else if (blType == BLType.Relative)
			{
				bullet.Direction = bullet.Direction + value;
			}
			else //if (blType == BLType.Aim || blType == BLType.None)
			{
				bullet.Direction = bullet.GetAimDir() + value;
			}

			//Debug.WriteLine(String.Format("SetDirecton:{0},  (type:{1} val:{2})", bullet.Direction / Math.PI * 180, node.type, value / Math.PI * 180));
			end = true;

			return BLRunStatus.End;
		}

		#endregion //Methods
	}
}