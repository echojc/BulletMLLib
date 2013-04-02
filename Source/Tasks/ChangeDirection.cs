using System;

namespace BulletMLLib
{
	/// <summary>
	/// 方向転換処理
	/// </summary>
	internal class BulletMLChangeDirection : BulletMLTask
	{
		#region Members

		float changeDir;

		int term;

		BulletMLTree node;

		bool first = true;

		BLType blType = BLType.None;

		#endregion //Members

		#region Methods

		public BulletMLChangeDirection(BulletMLTree node)
		{
			this.node = node;
		}

		public override void Init()
		{
			base.Init();
			first = true;
			term = (int)node.GetChildValue(BLName.term, this);
		}

		public override BLRunStatus Run(Bullet bullet)
		{
			if (first)
			{
				first = false;
				float value = (float)(node.GetChildValue(BLName.direction, this) * Math.PI / 180);
				blType = node.GetChild(BLName.direction).type;
				if (blType == BLType.Sequence)
				{
					changeDir = value;
				}
				else
				{
					if (blType == BLType.Absolute)
					{
						changeDir = (float)((value - bullet.Direction));
					}
					else if (blType == BLType.Relative)
					{
						changeDir = (float)(value);
					}
					else
					{
						changeDir = (float)((bullet.GetAimDir() + value - bullet.Direction));
					}

					if (changeDir > Math.PI)
					{
						changeDir -= 2 * (float)Math.PI;
					}

					if (changeDir < -Math.PI)
					{
						changeDir += 2 * (float)Math.PI;
					}

					changeDir /= term;
				}
			}

			term--;

			bullet.Direction = bullet.Direction + changeDir;

			if (term <= 0)
			{
				end = true;
				return BLRunStatus.End;
			}
			else
			{
				return BLRunStatus.Continue;
			}
		}

		#endregion //Methods
	}
}