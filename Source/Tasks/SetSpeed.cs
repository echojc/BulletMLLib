
namespace BulletMLLib
{
	/// <summary>
	/// Speed 処理
	/// </summary>
	internal class BulletMLSetSpeed : BulletMLTask
	{
		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public BulletMLSetSpeed(BulletMLNode node, BulletMLTask owner) : base(node, owner)
		{
		}
		
		public override ERunStatus Run(Bullet bullet)
		{
			bullet.Velocity = node.GetValue(this);
			end = true;
			return ERunStatus.End;
		}

		#endregion //Methods
	}
}