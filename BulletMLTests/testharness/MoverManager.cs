using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using BulletMLLib;


namespace BulletMLTests
{
	class MoverManager : IBulletManager
	{
		public List<Mover> movers = new List<Mover>();
        public Myship dude = new Myship();

		public float PlayerX
        {
            get { return dude.X; }
        }

        public float PlayerY
        {
            get { return dude.Y; }
        }
        
        public float GameDifficulty
        {
            get { return 0.3246f; }
        }

        public float Random
        {
            get { return 0.1337f; }
        }
		
		public Bullet CreateBullet()
		{
			Mover mover = new Mover(this);
			movers.Add(mover);
			mover.InitNode();
			return mover;
		}
		
		public void RemoveBullet(Bullet deadBullet)
		{
			Mover myMover = deadBullet as Mover;
			if (myMover != null)
			{
				myMover.Used = false;
			}
		}

		public void Update()
		{
			for (int i = 0; i < movers.Count; i++)
			{
				movers[i].Update();
			}

			FreeMovers();
		}

		public void FreeMovers()
		{
			for (int i = 0; i < movers.Count; i++)
			{
				if (!movers[i].Used)
				{
					movers.Remove(movers[i]);
					i--;
				}
			}
		}
	}
}
