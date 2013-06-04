using System;
using System.Diagnostics;

namespace BulletMLLib
{
	/// <summary>
	/// A task to shoot a bullet
	/// </summary>
	public class BulletMLFire : BulletMLTask
	{
		#region Members

		/// <summary>
		/// The direction that this task will fire a bullet.
		/// </summary>
		/// <value>The fire direction.</value>
		private float FireDirection { get; set; }

		/// <summary>
		/// The speed that this task will fire a bullet.
		/// </summary>
		/// <value>The fire speed.</value>
		private float FireSpeed { get; set; }

		/// <summary>
		/// Flag used to tell if this is the first time this task has been run
		/// Used to determine if we should use the "initial" or "sequence" nodes to set bullets.
		/// </summary>
		/// <value><c>true</c> if initial run; otherwise, <c>false</c>.</value>
		private bool InitialRun { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLFire"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public BulletMLFire(FireNode node, BulletMLTask owner) : base(node, owner)
		{
			Debug.Assert(null != Node);
			Debug.Assert(null != Owner);

			InitialRun = true;
		}

		/// <summary>
		/// Init this task and all its sub tasks.  
		/// This method should be called AFTER the nodes are parsed, but BEFORE run is called.
		/// </summary>
		/// <param name="bullet">the bullet this dude is controlling</param>
		protected override void Init(Bullet bullet)
		{
			base.Init(bullet);

			//fisrt get the fire node
			FireNode myFireNode = Node as FireNode;

			//Set the direction to shoot the bullet

			//is this the first time it has ran?  If there isn't a sequence node, we don't care!
			if (InitialRun || (null == myFireNode.SequenceDirectionNode))
			{
				//Set the fire direction to the "initial" value
			}
			else if (null != myFireNode.SequenceDirectionNode)
			{
				//else if there is a sequence node, add the value to the "shoot direction"
			}
			else
			{
				//aim it at the player dude
			}

			//Set the speed to shoot the bullet

			//is this the first time it has ran?  If there isn't a sequence node, we don't care!
			if (InitialRun || (null == myFireNode.SequenceSpeedNode))
			{
				//set the shoot speed to the "initial" value.
			}
			else if (null != myFireNode.SequenceSpeedNode)
			{
			}
			else
			{
				//set it to the speed of the current bullet
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
//			//Find which direction to shoot the new bullet
//			if (DirNode != null)
//			{
//				//get the direction continade in the node
//				float newBulletDirection = (int)DirNode.GetValue(this) * (float)Math.PI / (float)180;
//				switch (DirNode.NodeType)
//				{
//					case ENodeType.sequence:
//					{
//						bullet.GetFireData().srcDir += newBulletDirection;
//					}
//					break;
//
//					case ENodeType.absolute:
//					{
//						bullet.GetFireData().srcDir = newBulletDirection;
//					}
//					break;
//
//					case ENodeType.relative:
//					{
//						bullet.GetFireData().srcDir = newBulletDirection + bullet.Direction;
//					}
//					break;
//
//					default:
//					{
//						bullet.GetFireData().srcDir = newBulletDirection + bullet.GetAimDir();
//					}
//					break;
//				}
//			}
//			else
//			{
//				//otherwise if no direction node, aim the bullet at the bad guy
//				bullet.GetFireData().srcDir = bullet.GetAimDir();
//			}
//
//			//Create the new bullet
//			Bullet newBullet = bullet.MyBulletManager.CreateBullet();
//
//			if (newBullet == null)
//			{
//				//wtf did you do???
//				TaskFinished = true;
//				return ERunStatus.End;
//			}
//
//			//initialize the bullet from a reference node, or our bullet node
//			if (RefNode != null)
//			{
//				//Add an empty task to the bullet and populate it with all the params
//				BulletMLTask bulletBlankTask = newBullet.CreateTask();
//
//				//Add all the params to the new task we just added to that bullet
//				for (int i = 0; i < RefNode.ChildNodes.Count; i++)
//				{
//					bulletBlankTask.ParamList.Add(RefNode.ChildNodes[i].GetValue(this));
//				}
//
//				//init the bullet now that all our stuff is prepopulated
//				BulletMLNode subNode = bullet.MyNode.GetRootNode().FindLabelNode(RefNode.Label, ENodeName.bullet);
//				newBullet.Init(subNode);
//			}
//			else
//			{
//				//if there is no ref node, there has to be  bullet node
//				newBullet.Init(BulletNode);
//			}
//
//			//set the location of the new bullet
//			newBullet.X = bullet.X;
//			newBullet.Y = bullet.Y;
//
//			//set the owner of the new bullet to this dude
//			newBullet._tasks[0].Owner = this;
//
//			//set the direction of the new bullet
//			newBullet.Direction = bullet.GetFireData().srcDir;
//
//			//Has the speed for new bullets been set in the source bullet yet?
//			if (!bullet.GetFireData().speedInit && newBullet.GetFireData().speedInit)
//			{
//				bullet.GetFireData().srcSpeed = newBullet.Velocity;
//				bullet.GetFireData().speedInit = true;
//			}
//			else
//			{
//				//find the speed for new bullets and store it in the source bullet
//				if (SpeedNode != null)
//				{
//					//Get the speed from a speed node
//					float newBulletSpeed = SpeedNode.GetValue(this);
//					if (SpeedNode.NodeType == ENodeType.sequence || SpeedNode.NodeType == ENodeType.relative)
//					{
//						bullet.GetFireData().srcSpeed += newBulletSpeed;
//					}
//					else
//					{
//						bullet.GetFireData().srcSpeed = newBulletSpeed;
//					}
//				}
//				else
//				{
//					if (!newBullet.GetFireData().speedInit)
//					{
//						bullet.GetFireData().srcSpeed = 1;
//					}
//					else
//					{
//						bullet.GetFireData().srcSpeed = newBullet.Velocity;
//					}
//				}
//			}
//
//			newBullet.GetFireData().speedInit = false;
//			newBullet.Velocity = bullet.GetFireData().srcSpeed;
//
			TaskFinished = true;
			return ERunStatus.End;
		}

		#endregion //Methods
	}
}