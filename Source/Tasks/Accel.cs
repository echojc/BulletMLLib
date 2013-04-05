using Microsoft.Xna.Framework;

namespace BulletMLLib
{
	/// <summary>
	/// 加速処理
	/// </summary>
	internal class BulletMLAccel : BulletMLTask
	{
		#region Members

		int term;

		/// <summary>
		/// The direction to accelerate in 
		/// </summary>
		private Vector2 _Acceleration = Vector2.Zero;

		bool first;

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public BulletMLAccel(BulletMLNode node, BulletMLTask owner) : base(node, owner)
		{
		}

		protected override void Init()
		{
			base.Init();
			first = true;
		}

		public override ERunStatus Run(Bullet bullet)
		{
			if (first)
			{
				first = false;
				term = (int)node.GetChildValue(ENodeName.term, this);
				switch (node.NodeType)
				{
					case ENodeType.sequence:
						{
							_Acceleration.X = node.GetChildValue(ENodeName.horizontal, this);
							_Acceleration.Y = node.GetChildValue(ENodeName.vertical, this);
						}
						break;

					case ENodeType.relative:
						{
							_Acceleration.X = node.GetChildValue(ENodeName.horizontal, this) / term;
							_Acceleration.Y = node.GetChildValue(ENodeName.vertical, this) / term;
						}
						break;

					default:
						{
							_Acceleration.X = (node.GetChildValue(ENodeName.horizontal, this) - bullet.Acceleration.X) / term;
							_Acceleration.Y = (node.GetChildValue(ENodeName.vertical, this) - bullet.Acceleration.Y) / term;
						}
						break;
				}
			}

			term--;
			if (term < 0)
			{
				end = true;
				return ERunStatus.End;
			}

			bullet.Acceleration += _Acceleration;

			return ERunStatus.Continue;
		}

		#endregion //Methods
	}
}