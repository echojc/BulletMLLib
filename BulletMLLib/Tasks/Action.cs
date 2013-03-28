
namespace BulletMLLib
{
	/// <summary>
	/// Actionタグの処理。
	/// Repeatタグは常についていると仮定し、1～Timesの回数繰り返す。
	/// </summary>
	internal class BulletMLAction : BulletMLTask
	{
		#region Members

		public int repeatNumMax;

		public int repeatNum;

		BulletMLTree node;

		#endregion //Members

		#region Properties

		#endregion //Properties

		#region Methods

		public BulletMLAction(BulletMLTree node, int repeatNumMax)
		{
			this.node = node;
			this.repeatNumMax = repeatNumMax;
		}

		public override void Init()
		{
			base.Init();
			repeatNum = 0;
		}

		public override BLRunStatus Run(BulletMLBullet bullet)
		{
			while (repeatNum < repeatNumMax)
			{
				BLRunStatus runStatus = base.Run(bullet);

				if (runStatus == BLRunStatus.End)
				{
					repeatNum++;
					base.Init();
				}
				else if (runStatus == BLRunStatus.Stop)
				{
					return BLRunStatus.Stop;
				}
				else
				{
					return BLRunStatus.Continue;
				}
			}

			end = true;
			return BLRunStatus.End;
		}

		#endregion //Methods
	}
}