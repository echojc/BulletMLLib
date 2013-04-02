using System.Collections.Generic;
using System.Diagnostics;

namespace BulletMLLib
{
	/// <summary>
	/// BulletMLタスク
	/// 実際に弾を動かすクラス
	/// </summary>
	public class BulletMLTask
	{
		#region Members

		public enum BLRunStatus
		{
			Continue,
			End,
			Stop
		};

		public List<BulletMLTask> taskList = new List<BulletMLTask>();

		public bool end = false;

		public List<float> paramList = new List<float>();

		public BulletMLTask owner = null;

		#endregion //Members

		#region Properties

		#endregion //Properties

		#region Methods

		public BulletMLTask()
		{
		}

		public virtual void Init()
		{
			end = false;

			foreach (BulletMLTask task in taskList)
			{
				task.Init();
			}
		}

		public virtual BLRunStatus Run(Bullet bullet)
		{
			end = true;
			for (int i = 0; i < taskList.Count; i++)
			{
				if (!taskList[i].end)
				{
					BLRunStatus sts = taskList[i].Run(bullet);
					if (sts == BLRunStatus.Stop)
					{
						end = false;
						return BLRunStatus.Stop;
					}
					else if (sts == BLRunStatus.Continue)
					{
						end = false;
					}
				}
			}

			if (end)
			{
				return BLRunStatus.End;
			}
			else
			{
				return BLRunStatus.Continue;//継続して実行
			}
		}

		//BulletMLNodeの内容を元に、実行のための各種クラスを生成し、自身を初期化する
		public void Parse(BulletMLNode tree, Bullet bullet)
		{
			foreach (BulletMLNode node in tree.children)
			{
				// Action
				switch (node.name)
				{
					case ENodeName.repeat:
						{
							Parse(node, bullet);
						}
						break;
					case ENodeName.action:
						{
							////Debug.WriteLine("Action");
							int repeatNum = 1;
							if (node.parent.name == ENodeName.repeat)
								repeatNum = (int)node.parent.GetChildValue(ENodeName.times, this);
							BulletMLAction task = new BulletMLAction(node, repeatNum);
							task.owner = this;
							taskList.Add(task);
							task.Parse(node, bullet);
						}
						break;
					case ENodeName.actionRef:
						{
							BulletMLNode refNode = tree.GetLabelNode(node.label, ENodeName.action);
							int repeatNum = 1;
							if (node.parent.name == ENodeName.repeat)
							{
								repeatNum = (int)node.parent.GetChildValue(ENodeName.times, this);
							}
							BulletMLAction task = new BulletMLAction(refNode, repeatNum);
							task.owner = this;
							taskList.Add(task);

							// パラメータを取得
							for (int i = 0; i < node.children.Count; i++)
							{
								task.paramList.Add(node.children[i].GetValue(this));
							}

							task.Parse(refNode, bullet);
						}
						break;
					case ENodeName.changeSpeed:
						{
							BulletMLChangeSpeed blChangeSpeed = new BulletMLChangeSpeed(node);
							blChangeSpeed.owner = this;
							taskList.Add(blChangeSpeed);
						}
						break;
					case ENodeName.changeDirection:
						{
							BulletMLChangeDirection blChangeDir = new BulletMLChangeDirection(node);
							blChangeDir.owner = this;
							taskList.Add(blChangeDir);
						}
						break;
					case ENodeName.fire:
						{
							if (taskList == null) taskList = new List<BulletMLTask>();
							BulletMLFire fire = new BulletMLFire(node);
							fire.owner = this;
							taskList.Add(fire);
						}
						break;
					case ENodeName.fireRef:
						{
							if (taskList == null) taskList = new List<BulletMLTask>();
							BulletMLNode refNode = tree.GetLabelNode(node.label, ENodeName.fire);
							BulletMLFire fire = new BulletMLFire(refNode);
							fire.owner = this;
							taskList.Add(fire);

							for (int i = 0; i < node.children.Count; i++)
							{
								fire.paramList.Add(node.children[i].GetValue(this));
							}
						}
						break;
					case ENodeName.wait:
						{
							BulletMLWait wait = new BulletMLWait(node);
							wait.owner = this;
							taskList.Add(wait);
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
							BulletMLSetDirection task = new BulletMLSetDirection(node);
							task.owner = this;
							taskList.Add(task);
						}
						break;
					case ENodeName.vanish:
						{
							BulletMLVanish task = new BulletMLVanish();
							task.owner = this;
							taskList.Add(task);
						}
						break;
					case ENodeName.accel:
						{
							BulletMLAccel task = new BulletMLAccel(node);
							task.owner = this;
							taskList.Add(task);
						}
						break;

					//default:
					//    {
					//        //wtf did you do
					//        Debug.Assert(false);
					//    }
					//    break;
				}
			}
		}

		#endregion //Methods
	}
}