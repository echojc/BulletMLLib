
namespace BulletMLLib
{
	/// <summary>
	/// 加速処理
	/// </summary>
	internal class BulletMLAccel : BulletMLTask
	{
		#region Members

		BulletMLTree node;

		int term;

		float verticalAccel;

		float horizontalAccel;

		bool first;

		#endregion //Members

		#region Methods

		public BulletMLAccel(BulletMLTree node)
		{
			this.node = node;
		}

		public override void Init()
		{
			base.Init();
			first = true;
		}

		public override BLRunStatus Run(BulletMLBullet bullet)
		{
			if (first)
			{
				first = false;
				term = (int)node.GetChildValue(BLName.term, this);
				switch (node.type)
				{
					case BLType.Sequence:
						{
							horizontalAccel = node.GetChildValue(BLName.horizontal, this);
							verticalAccel = node.GetChildValue(BLName.vertical, this);
						}
						break;

					case BLType.Relative:
						{
							horizontalAccel = node.GetChildValue(BLName.horizontal, this) / term;
							verticalAccel = node.GetChildValue(BLName.vertical, this) / term;
						}
						break;

					default:
						{
							horizontalAccel = (node.GetChildValue(BLName.horizontal, this) - bullet.spdX) / term;
							verticalAccel = (node.GetChildValue(BLName.vertical, this) - bullet.spdY) / term;
						}
						break;
				}
			}

			term--;
			if (term < 0)
			{
				end = true;
				return BLRunStatus.End;
			}

			bullet.spdX += horizontalAccel;
			bullet.spdY += verticalAccel;

			return BLRunStatus.Continue;
		}

		#endregion //Methods
	}
}