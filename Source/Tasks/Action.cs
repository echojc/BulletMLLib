
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

		#endregion //Members

		#region Methods

		public BulletMLAction(int repeatNumMax, BulletMLNode node, BulletMLTask owner) : base(node, owner)
		{
			this.repeatNumMax = repeatNumMax;
		}

		protected override void Init()
		{
			base.Init();
			repeatNum = 0;
		}

		public override ERunStatus Run(Bullet bullet)
		{
			while (repeatNum < repeatNumMax)
			{
				ERunStatus runStatus = base.Run(bullet);

				if (runStatus == ERunStatus.End)
				{
					repeatNum++;
					base.Init();
				}
				else if (runStatus == ERunStatus.Stop)
				{
					return runStatus;
				}
				else
				{
					return ERunStatus.Continue;
				}
			}

			end = true;
			return ERunStatus.End;
		}

		#endregion //Methods
	}
}