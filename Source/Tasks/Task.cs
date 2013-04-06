using System.Collections.Generic;
using System.Diagnostics;

namespace BulletMLLib
{
	//TODO: these tasks are a wreck... fix all this crap 

	/// <summary>
	/// This is a task..each task is the action from a single xml node, for one bullet.
	/// basically each bullet makes a tree of these to match its pattern
	/// </summary>
	internal class BulletMLTask
	{
		#region Members

		/// <summary>
		/// A list of child tasks of this dude
		/// </summary>
		internal List<BulletMLTask> ChildTasks { get; private set; }

		/// <summary>
		/// The parameter list for this task
		/// </summary>
		internal List<float> ParamList { get; private set; }

		/// <summary>
		/// the parent task of this dude in the tree
		/// </summary>
		internal BulletMLTask Owner { get; set; }

		/// <summary>
		/// The bullet ml node that this dude represents
		/// </summary>
		internal BulletMLNode Node { get; private set; }

		/// <summary>
		/// whether or not this task has finished running
		/// </summary>
		protected bool TaskFinished { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public BulletMLTask(BulletMLNode node, BulletMLTask owner)
		{
			ChildTasks = new List<BulletMLTask>();
			ParamList = new List<float>();
			TaskFinished = false;
			this.Owner = owner;
			this.Node = node; 
		}

		/// <summary>
		/// Init this task and all its sub tasks.  
		/// This method should be called AFTER the nodes are parsed, but BEFORE run is called.
		/// </summary>
		protected virtual void Init()
		{
			TaskFinished = false;

			foreach (BulletMLTask task in ChildTasks)
			{
				task.Init();
			}
		}

		/// <summary>
		/// Run this task and all subtasks against a bullet
		/// This is called once a frame during runtime.
		/// </summary>
		/// <returns>ERunStatus: whether this task is done, paused, or still running</returns>
		/// <param name="bullet">The bullet to update this task against.</param>
		public virtual ERunStatus Run(Bullet bullet)
		{
			//run all the child tasks
			TaskFinished = true;
			for (int i = 0; i < ChildTasks.Count; i++)
			{
				//is the child task finished running?
				if (!ChildTasks[i].TaskFinished)
				{
					//Run the child task...
					ERunStatus childStaus = ChildTasks[i].Run(bullet);
					if (childStaus == ERunStatus.Stop)
					{
						//The child task is paused, so it is not finished
						TaskFinished = false;
						return childStaus;
					}
					else if (childStaus == ERunStatus.Continue)
					{
						//child task needs to do some more work
						TaskFinished = false;
					}
				}
			}

			return (TaskFinished ?  ERunStatus.End : ERunStatus.Continue);
		}

		/// <summary>
		/// Parse a specified node and bullet into this task
		/// </summary>
		/// <param name="myNode">the node for this dude</param>
		/// <param name="bullet">the bullet this dude is controlling</param>
		public virtual void Parse(BulletMLNode myNode, Bullet bullet)
		{
			foreach (BulletMLNode childNode in myNode.ChildNodes)
			{
				//construct the correct type of node
				switch (childNode.Name)
				{
					case ENodeName.repeat:
					{
						Parse(childNode, bullet);
					}
					break;
					case ENodeName.action:
					{
						int repeatNum = 1;
						if (myNode.Parent.Name == ENodeName.repeat)
						{
							repeatNum = (int)myNode.Parent.GetChildValue(ENodeName.times, this);
						}
						BulletMLAction task = new BulletMLAction(repeatNum, myNode, this);
						ChildTasks.Add(task);
						task.Parse(childNode, bullet);
					}
					break;
					case ENodeName.actionRef:
					{
						BulletMLNode refNode = myNode.FindLabelNode(childNode.Label, ENodeName.action);
						int repeatNum = 1;
						if (myNode.Parent.Name == ENodeName.repeat)
						{
							repeatNum = (int)myNode.Parent.GetChildValue(ENodeName.times, this);
						}
						BulletMLAction task = new BulletMLAction(repeatNum, refNode, this);
						ChildTasks.Add(task);

						for (int i = 0; i < childNode.ChildNodes.Count; i++)
						{
							task.ParamList.Add(childNode.ChildNodes[i].GetValue(this));
						}

						task.Parse(refNode, bullet);
					}
					break;
					case ENodeName.changeSpeed:
					{
						ChildTasks.Add(new BulletMLChangeSpeed(childNode, this));
					}
					break;
					case ENodeName.changeDirection:
					{
						ChildTasks.Add(new BulletMLChangeDirection(childNode, this));
					}
					break;
					case ENodeName.fire:
					{
						ChildTasks.Add(new BulletMLFire(childNode, this));
					}
					break;
					case ENodeName.fireRef:
					{
						BulletMLNode refNode = myNode.FindLabelNode(childNode.Label, ENodeName.fire);
						BulletMLFire fire = new BulletMLFire(refNode, this);
						ChildTasks.Add(fire);

						for (int i = 0; i < childNode.ChildNodes.Count; i++)
						{
							fire.ParamList.Add(childNode.ChildNodes[i].GetValue(this));
						}
					}
					break;
					case ENodeName.wait:
					{
						ChildTasks.Add(new BulletMLWait(childNode, this));
					}
					break;
					case ENodeName.speed:
					{
						bullet.GetFireData().speedInit = true;
						bullet.Velocity = Node.GetValue(this);
					}
					break;
					case ENodeName.direction:
					{
						ChildTasks.Add(new BulletMLSetDirection(childNode, this));
					}
					break;
					case ENodeName.vanish:
					{
						ChildTasks.Add(new BulletMLVanish(childNode, this));
					}
					break;
					case ENodeName.accel:
					{
						ChildTasks.Add(new BulletMLAccel(childNode, this));
					}
					break;
				}
			}

			//After all the nodes are read in, initialize the node
			Init();
		}

		#endregion //Methods
	}
}