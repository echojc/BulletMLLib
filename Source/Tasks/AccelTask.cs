
using System.Diagnostics;

namespace BulletMLLib
{
	/// <summary>
	/// This task adds acceleration to a bullet.
	/// </summary>
	public class AccelTask : BulletMLTask
	{
		#region Members

		/// <summary>
		/// How long to run this task... measured in frames
		/// </summary>
		public float Duration { get; private set; }

        public float X { get; private set; }
        public float Y { get; private set; }
		
		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public AccelTask(AccelNode node, BulletMLTask owner) : base(node, owner)
		{
			Debug.Assert(null != Node);
			Debug.Assert(null != Owner);
            X = 0f;
            Y = 0f;
		}

		/// <summary>
		/// this sets up the task to be run.
		/// </summary>
		/// <param name="bullet">Bullet.</param>
		protected override void SetupTask(Bullet bullet)
		{
			//set the accelerataion we are gonna add to the bullet
			Duration = Node.GetChildValue(ENodeName.term, this);

			//check for divide by 0
			if (0.0f == Duration)
			{
				Duration = 1.0f;
			}

			//Get the horizontal node
			HorizontalNode horiz = Node.GetChild(ENodeName.horizontal) as HorizontalNode;
			if (null != horiz)
			{
				//Set the x component of the acceleration
				switch (horiz.NodeType)
				{
					case ENodeType.sequence:
					{
						//Sequence in an acceleration node means "add this amount every frame"
						X = horiz.GetValue(this);
					}
					break;

					case ENodeType.relative:
					{
						//accelerate by a certain amount
						X = horiz.GetValue(this) / Duration;
					}
					break;

					default:
					{
						//accelerate to a specific value
						X = (horiz.GetValue(this) - bullet.AccelerationX) / Duration;
					}
					break;
				}
			}

			//Get the vertical node
			VerticalNode vert = Node.GetChild(ENodeName.vertical) as VerticalNode;
			if (null != vert)
			{
				//set teh y component of the acceleration
				switch (vert.NodeType)
				{
					case ENodeType.sequence:
					{
						//Sequence in an acceleration node means "add this amount every frame"
						Y = vert.GetValue(this);
					}
					break;

					case ENodeType.relative:
					{
						//accelerate by a certain amount
						Y = vert.GetValue(this) / Duration;
					}
					break;

					default:
					{
						//accelerate to a specific value
						Y = (vert.GetValue(this) - bullet.AccelerationY) / Duration;
					}
					break;
				}
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
			//Add the acceleration to the bullet
			bullet.AccelerationX += X;
			bullet.AccelerationY += Y;

			//decrement the amount if time left to run and return End when this task is finished
			Duration -= 1.0f * bullet.TimeSpeed;
			if (Duration <= 0.0f)
			{
				TaskFinished = true;
				return ERunStatus.End;
			}
			else 
			{
				//since this task isn't finished, run it again next time
				return ERunStatus.Continue;
			}
		}

		#endregion //Methods
	}
}