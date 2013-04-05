
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

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public BulletMLWait(BulletMLNode node, BulletMLTask owner) : base(node, owner)
		{
		}

		protected override void Init()
		{
			base.Init();
			term = (int)node.GetValue(this) + 1; //初回実行時に一回処理されるため、そのぶん加算しておく
		}

		public override ERunStatus Run(Bullet bullet)
		{
			if (term >= 0)
			{
				term--;
			}

			if (term >= 0)
			{
				return ERunStatus.Stop;
			}
			else
			{
				end = true;
				return ERunStatus.End;
			}
		}

		#endregion //Methods
	}
}