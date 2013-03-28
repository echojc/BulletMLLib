using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace BulletMLLib
{
	/// <summary>
	/// BulletMLライブラリ内部で使用する弾を表す。
	/// IBulletMLBulletInterfaceを継承したクラスに、変数として一つ持たせておくこと。
	/// </summary>
	public class BulletMLBullet
	{
		#region Members

		private IBulletMLBulletInterface ibullet;

		//private float dir;
		public float spdX; //<accel>で使用。移動処理でspeedと加算する必要がある
		public float spdY; //<accel>で使用。移動処理でspeedと加算する必要がある
		public List<BulletMLTask> tasks;
		private List<FireData> fireData;
		public BulletMLTree tree;
		private int activeTaskNum = 0; // 現在処理中のtasksのインデクス

		#endregion //Members

		#region Properties

		public float X { get { return ibullet.X; } set { ibullet.X = value; } }
		public float Y { get { return ibullet.Y; } set { ibullet.Y = value; } }
		public float Speed { get { return ibullet.Speed; } set { ibullet.Speed = value; } }

		public float Direction
		{
			get
			{
				return ibullet.Dir;
			}
			set
			{
				float dir = value;

				if (dir > 2 * Math.PI)
					dir -= (float)(2 * Math.PI);
				else if (dir < 0)
					dir += (float)(2 * Math.PI);

				ibullet.Dir = dir;
			}
		}

		#endregion //Properties

		#region Methods

		public BulletMLBullet(IBulletMLBulletInterface ibullet)
		{
			this.ibullet = ibullet;
			tasks = new List<BulletMLTask>();
			tasks.Add(new BulletMLTask());
			fireData = new List<FireData>();
			fireData.Add(new FireData());
			foreach (BulletMLTask t in tasks)
				t.Init();

			//task = new BulletMLTask();
			//task.Init();
		}

		/// <summary>
		/// BulletMLを動作させる
		/// </summary>
		/// <returns>処理が終了していたらtrue</returns>
		public bool Run()
		{
			int endNum = 0;
			for (int i = 0; i < tasks.Count; i++)
			{
				activeTaskNum = i;
				BulletMLAction.BLRunStatus result = tasks[i].Run(this);
				if (result == BulletMLTask.BLRunStatus.End)
				{
					endNum++;
				}
			}

			X = X + spdX + (float)(Math.Sin(ibullet.Dir) * Speed);
			Y = Y + spdY + (float)(-Math.Cos(ibullet.Dir) * Speed);

			if (endNum == tasks.Count)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//木構造のトップからの初期化
		public void InitTop(BulletMLTree node)
		{
			//トップノードからの初期化
			this.tree = node;

			BulletMLTree tree = node.GetLabelNode("top", BLName.action);
			if (tree != null)
			{
				BulletMLTask task = tasks[0];
				task.taskList.Clear();
				task.Parse(tree, this);
				task.Init();
			}
			else
			{
				for (int i = 1; i < 10; i++)
				{
					BulletMLTree tree2 = node.GetLabelNode("top" + i, BLName.action);
					if (tree2 != null)
					{
						if (i > 1)
						{
							tasks.Add(new BulletMLTask());
							fireData.Add(new FireData());
						}

						BulletMLTask task = tasks[i - 1];
						task.taskList.Clear();
						task.Parse(tree2, this);
						task.Init();
					}
				}
			}

		}

		//枝の途中からの初期化
		public void Init(BulletMLTree node)
		{
			BulletMLTask task = tasks[0];
			task.taskList.Clear();
			task.Parse(node, this);
			task.Init();
			this.tree = node;
		}

		public FireData GetFireData()
		{
			return fireData[activeTaskNum];
		}

		/// <summary>
		/// Get the direction to aim that bullet
		/// </summary>
		/// <returns>angle to target the bullet</returns>
		internal float GetAimDir()
		{
			//get the player position so we can aim at that little fucker
			Vector2 shipPos = Vector2.Zero;
			Debug.Assert(null != BulletMLManager.PlayerPosition);
			shipPos = BulletMLManager.PlayerPosition();

			//get the angle at that dude
			float val = (float)Math.Atan2((shipPos.X - X), -(shipPos.Y - Y));
			return val;
		}

		public void Vanish() 
		{
			ibullet.Vanish(); 
		}

		internal BulletMLBullet GetNewBullet()
		{
			return ibullet.GetNewBullet();
		}

		#endregion //Methods
	}
}
