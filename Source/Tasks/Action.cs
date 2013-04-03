
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

		BulletMLNode node;

		#endregion //Members

		#region Methods

		public BulletMLAction(BulletMLNode node, int repeatNumMax)
		{
			this.node = node;
			this.repeatNumMax = repeatNumMax;
		}

		public override void Init()
		{
			base.Init();
			repeatNum = 0;
		}

		public override BLRunStatus Run(Bullet bullet)
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
					return runStatus;
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