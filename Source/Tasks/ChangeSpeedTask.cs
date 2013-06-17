using System.Diagnostics;

namespace BulletMLLib
{
	/// <summary>
	/// This task changes the speed a little bit every frame.
	/// </summary>
	public class ChangeSpeedTask : BulletMLTask
	{
		#region Members

		/// <summary>
		/// The amount to change speed every frame
		/// </summary>
		private float SpeedChange { get; set; }

		/// <summary>
		/// How long to run this task... measured in frames
		/// </summary>
		private int Duration { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public ChangeSpeedTask(ChangeSpeedNode node, BulletMLTask owner) : base(node, owner)
		{
			Debug.Assert(null != Node);
			Debug.Assert(null != Owner);
		}

		/// <summary>
		/// This gets called when nested repeat nodes get initialized.
		/// </summary>
		/// <param name="bullet">Bullet.</param>
		public override void HardReset(Bullet bullet)
		{
			base.HardReset(bullet);
			SetupTask(bullet);
		}
		
		/// <summary>
		/// Init this task and all its sub tasks.  
		/// This method should be called AFTER the nodes are parsed, but BEFORE run is called.
		/// </summary>
		/// <param name="bullet">the bullet this dude is controlling</param>
		public override void InitTask(Bullet bullet)
		{
			base.InitTask(bullet);
			SetupTask(bullet);
		}

		/// <summary>
		/// this sets up the task to be run.
		/// </summary>
		/// <param name="bullet">Bullet.</param>
		private void SetupTask(Bullet bullet)
		{
			//set the length of time to run this dude
			Duration = (int)Node.GetChildValue(ENodeName.term, this);

			switch (Node.GetChild(ENodeName.speed).NodeType)
			{
				case ENodeType.sequence:
				{
					SpeedChange = Node.GetChildValue(ENodeName.speed, this);
				}
				break;

				case ENodeType.relative:
				{
					SpeedChange = Node.GetChildValue(ENodeName.speed, this) / Duration;
				}
				break;

				default:
				{
					SpeedChange = (Node.GetChildValue(ENodeName.speed, this) - bullet.Speed) / Duration;
				}
				break;
			}
		}

		/// <summary>
		/// Run this task and all subtasks against a bullet
		/// This is called once a frame during runtime.
		/// </summary>
		/// <returns>ERunStatus: whether this task is done, paused, or still running</returns>
		/// <param name="bullet">The bullet to update this task against.</param>
		public override ERunStatus Run(Bullet bullet)
		{
			bullet.Speed += SpeedChange;

			Duration--;
			if (Duration <= 0)
			{
				TaskFinished = true;
				return ERunStatus.End;
			}
			else
			{
				return ERunStatus.Continue;
			}
		}

		#endregion //Methods
	}
}