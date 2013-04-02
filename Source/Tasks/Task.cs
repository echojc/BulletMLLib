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

		//BulletMLTreeの内容を元に、実行のための各種クラスを生成し、自身を初期化する
		public void Parse(BulletMLTree tree, Bullet bullet)
		{
			foreach (BulletMLTree node in tree.children)
			{
				// Action
				switch (node.name)
				{
					case BLName.repeat:
						{
							Parse(node, bullet);
						}
						break;
					case BLName.action:
						{
							////Debug.WriteLine("Action");
							int repeatNum = 1;
							if (node.parent.name == BLName.repeat)
								repeatNum = (int)node.parent.GetChildValue(BLName.times, this);
							BulletMLAction task = new BulletMLAction(node, repeatNum);
							task.owner = this;
							taskList.Add(task);
							task.Parse(node, bullet);
						}
						break;
					case BLName.actionRef:
						{
							BulletMLTree refNode = tree.GetLabelNode(node.label, BLName.action);
							int repeatNum = 1;
							if (node.parent.name == BLName.repeat)
							{
								repeatNum = (int)node.parent.GetChildValue(BLName.times, this);
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
					case BLName.changeSpeed:
						{
							BulletMLChangeSpeed blChangeSpeed = new BulletMLChangeSpeed(node);
							blChangeSpeed.owner = this;
							taskList.Add(blChangeSpeed);
						}
						break;
					case BLName.changeDirection:
						{
							BulletMLChangeDirection blChangeDir = new BulletMLChangeDirection(node);
							blChangeDir.owner = this;
							taskList.Add(blChangeDir);
						}
						break;
					case BLName.fire:
						{
							if (taskList == null) taskList = new List<BulletMLTask>();
							BulletMLFire fire = new BulletMLFire(node);
							fire.owner = this;
							taskList.Add(fire);
						}
						break;
					case BLName.fireRef:
						{
							if (taskList == null) taskList = new List<BulletMLTask>();
							BulletMLTree refNode = tree.GetLabelNode(node.label, BLName.fire);
							BulletMLFire fire = new BulletMLFire(refNode);
							fire.owner = this;
							taskList.Add(fire);

							for (int i = 0; i < node.children.Count; i++)
							{
								fire.paramList.Add(node.children[i].GetValue(this));
							}
						}
						break;
					case BLName.wait:
						{
							BulletMLWait wait = new BulletMLWait(node);
							wait.owner = this;
							taskList.Add(wait);
						}
						break;
					case BLName.speed:
						{
							bullet.GetFireData().speedInit = true; // 値を明示的にセットしたことを示す
							bullet.Velocity = node.GetValue(this);
						}
						break;
					case BLName.direction:
						{
							BulletMLSetDirection task = new BulletMLSetDirection(node);
							task.owner = this;
							taskList.Add(task);
						}
						break;
					case BLName.vanish:
						{
							BulletMLVanish task = new BulletMLVanish();
							task.owner = this;
							taskList.Add(task);
						}
						break;
					case BLName.accel:
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