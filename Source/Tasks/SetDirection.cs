using System;

namespace BulletMLLib
{
	/// <summary>
	/// 方向設定
	/// </summary>
	internal class BulletMLSetDirection : BulletMLTask
	{
		#region Members

		public BulletMLNode node;

		#endregion //Members

		#region Methods

		public BulletMLSetDirection(BulletMLNode node)
		{
			this.node = node;
		}

		public override BLRunStatus Run(Bullet bullet)
		{
			ENodeType ENodeType = node.NodeType;
			float value = (float)(node.GetValue(this) * Math.PI / 180);

			if (ENodeType == ENodeType.sequence)
			{
				bullet.Direction = bullet.GetFireData().srcDir + value;
			}
			else if (ENodeType == ENodeType.absolute)
			{
				bullet.Direction = value;
			}
			else if (ENodeType == ENodeType.relative)
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