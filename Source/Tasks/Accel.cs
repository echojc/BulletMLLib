using Microsoft.Xna.Framework;

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

		/// <summary>
		/// The direction to accelerate in 
		/// </summary>
		private Vector2 _Acceleration = Vector2.Zero;

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

		public override BLRunStatus Run(Bullet bullet)
		{
			if (first)
			{
				first = false;
				term = (int)node.GetChildValue(BLName.term, this);
				switch (node.type)
				{
					case BLType.Sequence:
						{
							_Acceleration.X = node.GetChildValue(BLName.horizontal, this);
							_Acceleration.Y = node.GetChildValue(BLName.vertical, this);
						}
						break;

					case BLType.Relative:
						{
							_Acceleration.X = node.GetChildValue(BLName.horizontal, this) / term;
							_Acceleration.Y = node.GetChildValue(BLName.vertical, this) / term;
						}
						break;

					default:
						{
							_Acceleration.X = (node.GetChildValue(BLName.horizontal, this) - bullet.Acceleration.X) / term;
							_Acceleration.Y = (node.GetChildValue(BLName.vertical, this) - bullet.Acceleration.Y) / term;
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