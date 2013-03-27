
namespace BulletMLLib
{
	/// <summary>
	/// スピード変更
	/// </summary>
	public class BulletMLChangeSpeed : BulletMLTask
	{
		#region Members

		float changeSpeed;
		int term;
		BulletMLTree node;
		bool first = true;

		#endregion //Members

		#region Properties

		#endregion //Properties

		#region Methods

		public BulletMLChangeSpeed(BulletMLTree node)
		{
			this.node = node;
		}

		public override void Init()
		{
			base.Init();
			first = true;
			term = (int)node.GetChildValue(BLName.Term, this);
		}

		public override BLRunStatus Run(BulletMLBullet bullet)
		{
			if (first)
			{
				first = false;
				if (node.GetChild(BLName.Speed).type == BLType.Sequence)
				{
					changeSpeed = node.GetChildValue(BLName.Speed, this);
				}
				else if (node.GetChild(BLName.Speed).type == BLType.Relative)
				{
					changeSpeed = node.GetChildValue(BLName.Speed, this) / term;
				}
				else
				{
					changeSpeed = (node.GetChildValue(BLName.Speed, this) - bullet.Speed) / term;
				}
			}

			term--;

			bullet.Speed += changeSpeed;

			// if (bullet.index == DISP_BULLET_INDEX)  Debug.WriteLine(String.Format("ChangeSpeed:{0} (type:{1} val:{2})", bullet.Speed, node.GetChild(BLName.Speed).type, node.GetChildValue(BLName.Speed, this)));

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