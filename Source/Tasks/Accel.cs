using Microsoft.Xna.Framework;

namespace BulletMLLib
{
	/// <summary>
	/// 加速処理
	/// </summary>
	internal class BulletMLAccel : BulletMLTask
	{
		#region Members

		BulletMLNode node;

		int term;

		/// <summary>
		/// The direction to accelerate in 
		/// </summary>
		private Vector2 _Acceleration = Vector2.Zero;

		bool first;

		#endregion //Members

		#region Methods

		public BulletMLAccel(BulletMLNode node)
		{
			this.node = node;
		}

		public override void Init()
		{
			base.Init();
			first = true;
		}

		public override BLRunStatus Run(Bullet bullet)
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
				return BLRunStatus.End;
			}

			bullet.Acceleration += _Acceleration;

			return BLRunStatus.Continue;
		}

		#endregion //Methods
	}
}