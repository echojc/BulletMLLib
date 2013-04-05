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

		bool first = true;

		ENodeType blType = ENodeType.none;

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public BulletMLChangeDirection(BulletMLNode node, BulletMLTask owner) : base(node, owner)
		{
		}

		protected override void Init()
		{
			base.Init();
			first = true;
			term = (int)node.GetChildValue(ENodeName.term, this);
		}
		
		public override ERunStatus Run(Bullet bullet)
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
				return ERunStatus.End;
			}
			else
			{
				return ERunStatus.Continue;
			}
		}

		#endregion //Methods
	}
}