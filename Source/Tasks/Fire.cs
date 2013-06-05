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

		/// <summary>
		/// If this fire node shoots from a bullet ref node, this will be a task created for it.
		/// This is needed so the params of the bullet ref can be set correctly.
		/// </summary>
		/// <value>The bullet reference task.</value>
		private BulletMLTask BulletRefTask { get; set; }

		/// <summary>
		/// The node we are going to use to set the direction of any bullets shot with this task
		/// </summary>
		/// <value>The dir node.</value>
		public BulletMLSetDirection InitialDirectionTask { get; set; }

		/// <summary>
		/// The node we are going to use to set the speed of any bullets shot with this task
		/// </summary>
		/// <value>The speed node.</value>
		public BulletMLSetSpeed InitialSpeedTask { get; set; }

		/// <summary>
		/// If there is a sequence direction node used to increment the direction of each successive bullet that is fired
		/// </summary>
		/// <value>The sequence direction node.</value>
		public BulletMLSetDirection SequenceDirectionTask { get; set; }

		/// <summary>
		/// If there is a sequence direction node used to increment the direction of each successive bullet that is fired
		/// </summary>
		/// <value>The sequence direction node.</value>
		public BulletMLSetSpeed SequenceSpeedTask { get; set; }

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
		/// Parse a specified node and bullet into this task
		/// </summary>
		/// <param name="myNode">the node for this dude</param>
		/// <param name="bullet">the bullet this dude is controlling</param>
		public override void Parse(Bullet bullet)
		{
			if (null == bullet)
			{
				throw new NullReferenceException("bullet argument cannot be null");
			}

			foreach (BulletMLNode childNode in Node.ChildNodes)
			{
				ParseChildNode(childNode, bullet);
			}

			//Setup all the direction nodes
			GetDirectionTasks(this);
			GetDirectionTasks(BulletRefTask);

			//setup all the speed nodes
			GetSpeedNodes(this);
			GetSpeedNodes(BulletRefTask);

			//After all the nodes are read in, initialize the node
			Init(bullet);
		}

		/// <summary>
		/// Parse a specified node and bullet into this task
		/// </summary>
		/// <param name="myNode">the node for this dude</param>
		/// <param name="bullet">the bullet this dude is controlling</param>
		public override void ParseChildNode(BulletMLNode childNode, Bullet bullet)
		{
			Debug.Assert(null != childNode);
			Debug.Assert(null != bullet);

			if (ENodeName.bulletRef == childNode.Name)
			{
				//Create a task for the bullet ref 
				BulletRefTask = new BulletMLTask(childNode, this);

				//populate the params of the bullet ref
				for (int i = 0; i < childNode.ChildNodes.Count; i++)
				{
					BulletRefTask.ParamList.Add(childNode.ChildNodes[i].GetValue(this));
				}
			}
			else
			{
				//run the node through the base class if we don't want it
				base.ParseChildNode(childNode, bullet);
			}
		}

		/// <summary>
		/// Init this task and all its sub tasks.  
		/// This method should be called AFTER the nodes are parsed, but BEFORE run is called.
		/// </summary>
		/// <param name="bullet">the bullet this dude is controlling</param>
		protected override void Init(Bullet bullet)
		{
			base.Init(bullet);

			//get the direction to shoot the bullet

			//is this the first time it has ran?  If there isn't a sequence node, we don't care!
			if (InitialRun || (null == SequenceDirectionTask))
			{
				//do we have an initial direction node?
				if (null != InitialDirectionTask)
				{
					//Set the fire direction to the "initial" value
					float newBulletDirection = (int)InitialDirectionTask.GetNodeValue() * (float)Math.PI / (float)180;
					switch (InitialDirectionTask.Node.NodeType)
					{
						case ENodeType.absolute:
						{
							//the new bullet points right at a particular direction
							FireDirection = newBulletDirection;
						}
						break;

						case ENodeType.relative:
						{
							//the new bullet direction will be relative to the old bullet
							FireDirection = newBulletDirection + bullet.Direction;
						}
						break;

						default:
						{
							//aim the bullet at the player
							FireDirection = newBulletDirection + bullet.GetAimDir();
						}
						break;
					}
				}
				else
				{
					//There isn't an initial direction task, so just aim at the bad guy.
					//aim the bullet at the player
					FireDirection = bullet.GetAimDir();
				}
			}
			else if (null != SequenceDirectionTask)
			{
				//else if there is a sequence node, add the value to the "shoot direction"
				FireDirection += (int)SequenceDirectionTask.GetNodeValue() * (float)Math.PI / (float)180;
			}
			else
			{
				//aim it at the player dude
				FireDirection = bullet.GetAimDir();
			}

			//Set the speed to shoot the bullet

			//is this the first time it has ran?  If there isn't a sequence node, we don't care!
			if (InitialRun || (null == SequenceSpeedTask))
			{
				//do we have an initial speed node?
				if (null != InitialSpeedTask)
				{
					//set the shoot speed to the "initial" value.
					float newBulletSpeed = (int)InitialSpeedTask.GetNodeValue();
					switch (InitialSpeedTask.Node.NodeType)
					{
						case ENodeType.absolute:
						{
							//the new bullet shoots at a predeterminde speed
							FireSpeed = newBulletSpeed;
						}
						break;

						case ENodeType.relative:
						{
							//the new bullet speed will be relative to the old bullet
							FireSpeed = newBulletSpeed + bullet.Velocity;
						}
						break;

						default:
						{
							//use the old bullet speed
							FireSpeed = bullet.Velocity;
						}
						break;
					}
				}
				else
				{
					//there is no initial speed task, use the old dude's speed
					FireSpeed = bullet.Velocity;
				}
			}
			else if (null != SequenceSpeedTask)
			{
				//else if there is a sequence node, add the value to the "shoot direction"
				FireSpeed += (int)SequenceSpeedTask.GetNodeValue();
			}
			else
			{
				//set it to the speed of the current bullet
				FireSpeed = bullet.Velocity;
			}

			//make sure we don't overwrite the initial values if we aren't supposed to
			InitialRun = false;
		}

		/// <summary>
		/// Run this task and all subtasks against a bullet
		/// This is called once a frame during runtime.
		/// </summary>
		/// <returns>ERunStatus: whether this task is done, paused, or still running</returns>
		/// <param name="bullet">The bullet to update this task against.</param>
		public override ERunStatus Run(Bullet bullet)
		{
			//Create the new bullet
			Bullet newBullet = bullet.MyBulletManager.CreateBullet();

			if (newBullet == null)
			{
				//wtf did you do???
				TaskFinished = true;
				return ERunStatus.End;
			}

			//initialize the bullet with the bullet node stored in the Fire node
			FireNode myFireNode = Node as FireNode;
			Debug.Assert(null != myFireNode);
			newBullet.Init(myFireNode.BulletDescriptionNode);

			//set the location of the new bullet
			newBullet.X = bullet.X;
			newBullet.Y = bullet.Y;

			//set the owner of the new bullet to this dude
			newBullet._tasks[0].Owner = this;

			//set the direction of the new bullet
			newBullet.Direction = FireDirection;

			//set teh speed of the new bullet
			newBullet.Velocity = FireSpeed;

			TaskFinished = true;
			return ERunStatus.End;
		}

		/// <summary>
		/// Given a node, pull the direction nodes out from underneath it and store them if necessary
		/// </summary>
		/// <param name="taskToCheck">task to check if has a child direction node.</param>
		private void GetDirectionTasks(BulletMLTask taskToCheck)
		{
			if (null == taskToCheck)
			{
				return;
			}

			//check if the dude has a direction node
			DirectionNode dirNode = taskToCheck.Node.GetChild(ENodeName.direction) as DirectionNode;
			if (null != dirNode)
			{
				//check if it is a sequence type of node
				if (ENodeType.sequence == dirNode.NodeType)
				{
					//do we need a sequence node?
					if (null == SequenceDirectionTask)
					{
						//store it in the sequence direction node
						SequenceDirectionTask = new BulletMLSetDirection(dirNode as DirectionNode, taskToCheck);
					}
				}
				else
				{
					//else do we need an initial node?
					if (null == InitialDirectionTask)
					{
						//store it in the initial direction node
						InitialDirectionTask = new BulletMLSetDirection(dirNode as DirectionNode, taskToCheck);
					}
				}
			}
		}

		/// <summary>
		/// Given a node, pull the speed nodes out from underneath it and store them if necessary
		/// </summary>
		/// <param name="nodeToCheck">Node to check.</param>
		private void GetSpeedNodes(BulletMLTask taskToCheck)
		{
			if (null == taskToCheck)
			{
				return;
			}

			//check if the dude has a speed node
			SpeedNode spdNode = taskToCheck.Node.GetChild(ENodeName.speed) as SpeedNode;
			if (null != spdNode)
			{
				//check if it is a sequence type of node
				if (ENodeType.sequence == spdNode.NodeType)
				{
					//do we need a sequence node?
					if (null == SequenceSpeedTask)
					{
						//store it in the sequence speed node
						SequenceSpeedTask = new BulletMLSetSpeed(spdNode as SpeedNode, taskToCheck);
					}
				}
				else
				{
					//else do we need an initial node?
					if (null == InitialSpeedTask)
					{
						//store it in the initial speed node
						InitialSpeedTask = new BulletMLSetSpeed(spdNode as SpeedNode, taskToCheck);
					}
				}
			}
		}

		#endregion //Methods
	}
}