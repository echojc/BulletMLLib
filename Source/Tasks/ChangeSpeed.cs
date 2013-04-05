
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

		bool first = true;

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public BulletMLChangeSpeed(BulletMLNode node, BulletMLTask owner) : base(node, owner)
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