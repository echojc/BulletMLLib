
namespace BulletMLLib
{
	/// <summary>
	/// Wait処理
	/// </summary>
	internal class BulletMLWait : BulletMLTask
	{
		#region Members

		/// <summary>
		/// How long to wait
		/// </summary>
		int term;

		BulletMLNode node;

		#endregion //Members

		#region Properties

		#endregion //Properties

		#region Methods

		public BulletMLWait(BulletMLNode node)
		{
			this.node = node;
		}

		public override void Init()
		{
			base.Init();
			term = (int)node.GetValue(this) + 1; //初回実行時に一回処理されるため、そのぶん加算しておく
		}

		public override BLRunStatus Run(Bullet bullet)
		{
			if (term >= 0)
			{
				term--;
			}

			if (term >= 0)
			{
				return BLRunStatus.Stop;
			}
			else
			{
				end = true;
				return BLRunStatus.End;
			}
		}

		#endregion //Methods
	}
}