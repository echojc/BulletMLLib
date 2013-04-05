using System.Collections.Generic;
using System.Diagnostics;

namespace BulletMLLib
{
	//TODO: these tasks are a wreck... fix all this crap 

	/// <summary>
	/// This is a task..each task is the action from a single xml node, for one bullet.
	/// basically each bullet makes a tree of these to match its pattern
	/// </summary>
	public class BulletMLTask
	{
		#region Members

		/// <summary>
		/// A list of child tasks of this dude
		/// </summary>
		private List<BulletMLTask> _childTasks = new List<BulletMLTask>();

		/// <summary>
		/// whether or not this task has finished running
		/// </summary>
		private bool _end = false;

		/// <summary>
		/// The parameter list for this task
		/// </summary>
		private List<float> _paramList = new List<float>();

		/// <summary>
		/// the parent task of this dude in the tree
		/// </summary>
		private BulletMLTask _owner = null;

		/// <summary>
		/// The bullet ml node that this dude represents
		/// </summary>
		private BulletMLNode _node;

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public BulletMLTask(BulletMLNode node, BulletMLTask owner)
		{
			_node = node;
			_owner = owner;
		}

		protected virtual void Init()
		{
			_end = false;

			foreach (BulletMLTask task in _childTasks)
			{
				task.Init();
			}
		}

		public virtual ERunStatus Run(Bullet bullet)
		{
			_end = true;
			for (int i = 0; i < _childTasks.Count; i++)
			{
				if (!_childTasks[i]._end)
				{
					ERunStatus sts = _childTasks[i].Run(bullet);
					if (sts == ERunStatus.Stop)
					{
						_end = false;
						return sts;
					}
					else if (sts == ERunStatus.Continue)
					{
						_end = false;
					}
				}
			}

			if (_end)
			{
				return ERunStatus.End;
			}
			else
			{
				return ERunStatus.Continue;//継続して実行
			}
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
						BulletMLAction task = new BulletMLAction(myNode, repeatNum);
						_childTasks.Add(task);
						task.Parse(_node, bullet);
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
						BulletMLAction task = new BulletMLAction(refNode, repeatNum);
						_childTasks.Add(task);

						// パラメータを取得
						for (int i = 0; i < childNode.ChildNodes.Count; i++)
						{
							task._paramList.Add(childNode.ChildNodes[i].GetValue(this));
						}

						task.Parse(refNode, bullet);
					}
					break;
					case ENodeName.changeSpeed:
					{
						_childTasks.Add(new BulletMLChangeSpeed(childNode, this));
					}
					break;
					case ENodeName.changeDirection:
					{
						_childTasks.Add(new BulletMLChangeDirection(childNode, this));
					}
					break;
					case ENodeName.fire:
					{
						_childTasks.Add(new BulletMLFire(childNode, this));
					}
					break;
					case ENodeName.fireRef:
					{
						if (_childTasks == null)
						{
							_childTasks = new List<BulletMLTask>();
						}
						BulletMLNode refNode = myNode.FindLabelNode(childNode.Label, ENodeName.fire);
						BulletMLFire fire = new BulletMLFire(refNode, this);
						_childTasks.Add(fire);

						for (int i = 0; i < childNode.ChildNodes.Count; i++)
						{
							fire.paramList.Add(childNode.ChildNodes[i].GetValue(this));
						}
					}
					break;
					case ENodeName.wait:
					{
						_childTasks.Add(new BulletMLWait(childNode, this));
					}
					break;
					case ENodeName.speed:
					{
						bullet.GetFireData().speedInit = true; // 値を明示的にセットしたことを示す
						bullet.Velocity = node.GetValue(this);
					}
					break;
					case ENodeName.direction:
					{
						_childTasks.Add(new BulletMLSetDirection(childNode, this));
					}
					break;
					case ENodeName.vanish:
					{
						_childTasks.Add(new BulletMLVanish(childNode, this));
					}
					break;
					case ENodeName.accel:
					{
						_childTasks.Add(new BulletMLAccel(childNode, this));
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