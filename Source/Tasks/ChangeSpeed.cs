
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

		BulletMLNode node;

		bool first = true;

		#endregion //Members

		#region Methods

		public BulletMLChangeSpeed(BulletMLNode node)
		{
			this.node = node;
		}

		public override void Init()
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
				if (node.GetChild(ENodeName.speed).NodeType == ENodeType.sequence)
				{
					changeSpeed = node.GetChildValue(ENodeName.speed, this);
				}
				else if (node.GetChild(ENodeName.speed).NodeType == ENodeType.relative)
				{
					changeSpeed = node.GetChildValue(ENodeName.speed, this) / term;
				}
				else
				{
					changeSpeed = (node.GetChildValue(ENodeName.speed, this) - bullet.Velocity) / term;
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