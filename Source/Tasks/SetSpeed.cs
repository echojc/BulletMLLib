
namespace BulletMLLib
{
	/// <summary>
	/// Speed 処理
	/// </summary>
	internal class BulletMLSetSpeed : BulletMLTask
	{
		#region Members

		BulletMLNode node;

		#endregion //Members

		#region Methods

		public BulletMLSetSpeed(BulletMLNode node)
		{
			this.node = node;
		}

		public override BLRunStatus Run(Bullet bullet)
		{
			bullet.Velocity = node.GetValue(this);
			end = true;
			return BLRunStatus.End;
		}

		#endregion //Methods
	}
}