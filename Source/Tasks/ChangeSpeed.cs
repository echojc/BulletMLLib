
namespace BulletMLLib
{
	/// <summary>
	/// スピード変更
	/// </summary>
	internal class BulletMLChangeSpeed : BulletMLTask
	{
		#region Members

		float changeSpeed;

		int term;

		BulletMLTree node;

		bool first = true;

		#endregion //Members

		#region Methods

		public BulletMLChangeSpeed(BulletMLTree node)
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
				if (node.GetChild(BLName.speed).type == BLType.Sequence)
				{
					changeSpeed = node.GetChildValue(BLName.speed, this);
				}
				else if (node.GetChild(BLName.speed).type == BLType.Relative)
				{
					changeSpeed = node.GetChildValue(BLName.speed, this) / term;
				}
				else
				{
					changeSpeed = (node.GetChildValue(BLName.speed, this) - bullet.Velocity) / term;
				}
			}

			term--;

			bullet.Velocity += changeSpeed;

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