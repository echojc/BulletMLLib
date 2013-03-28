using System;

namespace BulletMLLib
{
	/// <summary>
	/// 方向設定
	/// </summary>
	internal class BulletMLSetDirection : BulletMLTask
	{
		#region Members

		public BulletMLTree node;

		#endregion //Members

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
			else
			{
				bullet.Direction = bullet.GetAimDir() + value;
			}

			end = true;

			return BLRunStatus.End;
		}

		#endregion //Methods
	}
}