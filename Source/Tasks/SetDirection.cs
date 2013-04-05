using System;

namespace BulletMLLib
{
	/// <summary>
	/// 方向設定
	/// </summary>
	internal class BulletMLSetDirection : BulletMLTask
	{
		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public BulletMLSetDirection(BulletMLNode node, BulletMLTask owner) : base(node, owner)
		{
		}

		public override ERunStatus Run(Bullet bullet)
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

			return ERunStatus.End;
		}

		#endregion //Methods
	}
}