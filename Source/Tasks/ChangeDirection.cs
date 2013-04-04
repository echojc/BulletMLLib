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

		BulletMLNode node;

		bool first = true;

		ENodeType blType = ENodeType.none;

		#endregion //Members

		#region Methods

		public BulletMLChangeDirection(BulletMLNode node)
		{
			this.node = node;
		}

		protected override void Init()
		{
			base.Init();
			first = true;
			term = (int)node.GetChildValue(ENodeName.term, this);
		}

		public override BLRunStatus Run(Bullet bullet)
		{
			if (first)
			{
				first = false;
				float value = (float)(node.GetChildValue(ENodeName.direction, this) * Math.PI / 180);
				blType = node.GetChild(ENodeName.direction).NodeType;
				if (blType == ENodeType.sequence)
				{
					changeDir = value;
				}
				else
				{
					if (blType == ENodeType.absolute)
					{
						changeDir = (float)((value - bullet.Direction));
					}
					else if (blType == ENodeType.relative)
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