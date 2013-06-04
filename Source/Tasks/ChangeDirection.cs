using System;
using System.Diagnostics;

namespace BulletMLLib
{
	/// <summary>
	/// This task changes the direction a little bit every frame
	/// </summary>
	public class BulletMLChangeDirection : BulletMLTask
	{
		#region Members

		/// <summary>
		/// The amount to change driection every frame
		/// </summary>
		private float DirectionChange;

		/// <summary>
		/// How long to run this task... measured in frames
		/// </summary>
		private int Duration { get; set; }

		/// <summary>
		/// The type of direction change...
		/// </summary>
		ENodeType ChangeType = ENodeType.none;

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public BulletMLChangeDirection(ChangeDirectionNode node, BulletMLTask owner) : base(node, owner)
		{
			Debug.Assert(null != Node);
			Debug.Assert(null != Owner);
		}

		/// <summary>
		/// Init this task and all its sub tasks.  
		/// This method should be called AFTER the nodes are parsed, but BEFORE run is called.
		/// </summary>
		/// <param name="bullet">the bullet this dude is controlling</param>
		protected override void Init(Bullet bullet)
		{
			base.Init(bullet);

			//set the time length to run this dude
			Duration = (int)Node.GetChildValue(ENodeName.term, this);

			//Get the amount to change direction from the nodes
			float value = (float)(Node.GetChildValue(ENodeName.direction, this) * Math.PI / 180); //also make sure to convert to radians

			//How do we want to change direction?
			ChangeType = Node.GetChild(ENodeName.direction).NodeType;
			switch (ChangeType)
			{
				case ENodeType.sequence:
				{
					//We are going to add this amount to the direction every frame
					DirectionChange = value;
				}
				break;

				case ENodeType.absolute:
				{
					//We are going to go in the direction we are given, regardless of where we are pointing right now
					DirectionChange = (float)(value - bullet.Direction);
				}
				break;

				case ENodeType.relative:
				{
					//The direction change will be relative to our current direction
					DirectionChange = (float)(value);
				}
				break;

				default:
				{
					//the direction change is to aim at the enemy
					DirectionChange = ((bullet.GetAimDir() + value) - bullet.Direction);
				}
				break;
			}

			//keep the direction between 0 and 360
			if (DirectionChange > Math.PI)
			{
				DirectionChange -= 2 * (float)Math.PI;
			}
			else if (DirectionChange < -Math.PI)
			{
				DirectionChange += 2 * (float)Math.PI;
			}

			//The sequence type of change direction is unaffected by the duration
			if (ChangeType != ENodeType.sequence)
			{
				//Divide by the duration so we ease into the direction change
				DirectionChange /= (float)Duration;
			}
		}
		
		public override ERunStatus Run(Bullet bullet)
		{
			//change the direction of the bullet by the correct amount
			bullet.Direction += DirectionChange;

			//decrement the amount if time left to run and return End when this task is finished
			Duration--;
			if (Duration <= 0)
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