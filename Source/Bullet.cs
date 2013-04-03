using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace BulletMLLib
{
	/// <summary>
	/// This is the bullet class that outside assemblies will interact with.
	/// Just inherit from this class and override the abstract functions!
	/// </summary>
	public abstract class Bullet
	{
		#region Members

		/// <summary>
		/// The direction this bullet is travelling.  Measured as an angle in radians
		/// </summary>
		private float _direction;

		/// <summary>
		/// A bullet manager that manages this bullet.
		/// </summary>
		/// <value>My bullet manager.</value>
		private readonly IBulletManager _bulletManager;

		//TODO: what do these members do?

		public List<BulletMLTask> tasks;

		private List<FireData> fireData;

		public BulletMLNode tree;

		private int activeTaskNum = 0;

		#endregion //Members

		#region Properties

		/// <summary>
		/// The acceleration of this bullet
		/// </summary>
		/// <value>The accel, in pixels/frame^2</value>
		public Vector2 Acceleration { get; set; }

		/// <summary>
		/// Gets or sets the velocity
		/// </summary>
		/// <value>The velocity, in pixels/frame</value>
		public float Velocity { get; set; }

		/// <summary>
		/// Abstract property to get the X location of this bullet.
		/// measured in pixels from upper left
		/// </summary>
		/// <value>The horizontrla position.</value>
		public abstract float X
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the y parameter of the location
		/// measured in pixels from upper left
		/// </summary>
		/// <value>The vertical position.</value>
		public abstract float Y
		{
			get;
			set;
		}

		/// <summary>
		/// Gets my bullet manager.
		/// </summary>
		/// <value>My bullet manager.</value>
		public IBulletManager MyBulletManager
		{
			get
			{
				return _bulletManager;
			}
		}

		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		/// <value>The direction in radians.</value>
		public float Direction
		{
			get
			{
				return _direction;
			}
			set
			{
				_direction = value;

				if (_direction > 2 * Math.PI)
				{
					_direction -= (float)(2 * Math.PI);
				}
				else if (_direction < 0)
				{
					_direction += (float)(2 * Math.PI);
				}
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.Bullet"/> class.
		/// </summary>
		/// <param name="myBulletManager">My bullet manager.</param>
		public Bullet(IBulletManager myBulletManager)
		{
			//grba the bullet manager for this dude
			Debug.Assert(null != myBulletManager);
			_bulletManager = myBulletManager;

			Acceleration = Vector2.Zero;
			tasks = new List<BulletMLTask>();
			tasks.Add(new BulletMLTask());
			fireData = new List<FireData>();
			fireData.Add(new FireData());

			//TODO: creates a new thing and then initializes them???
			foreach (BulletMLTask t in tasks)
			{
				t.Init();
			}
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

			X += Acceleration.X + (float)(Math.Sin(Direction) * Velocity);
			Y += Acceleration.Y + (float)(-Math.Cos(Direction) * Velocity);

			if (endNum == tasks.Count)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Get the direction to aim that bullet
		/// </summary>
		/// <returns>angle to target the bullet</returns>
		internal float GetAimDir()
		{
			//get the player position so we can aim at that little fucker
			Vector2 shipPos = Vector2.Zero;
			Debug.Assert(null != MyBulletManager);
			shipPos = MyBulletManager.PlayerPosition(this);
			
			//get the angle at that dude
			float val = (float)Math.Atan2((shipPos.X - X), -(shipPos.Y - Y));
			return val;
		}

		//TODO: sort these shitty methods out

		//木構造のトップからの初期化
		public void InitTop(BulletMLNode node)
		{
			//トップノードからの初期化
			this.tree = node;

			BulletMLNode tree = node.FindLabelNode("top", ENodeName.action);
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
					BulletMLNode tree2 = node.FindLabelNode("top" + i, ENodeName.action);
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
		internal void Init(BulletMLNode node)
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

		#endregion //Methods
	}
}
