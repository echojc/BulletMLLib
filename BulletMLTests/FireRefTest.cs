using NUnit.Framework;
using System;
using BulletMLLib;

namespace BulletMLTests
{
	[TestFixture()]
	public class FireRefTest
	{
		MoverManager manager;
		Myship dude;
		BulletPattern pattern;

		[SetUp()]
		public void setupHarness()
		{
			manager = new MoverManager();
			dude = manager.dude;
			pattern = new BulletPattern();
		}

		[Test()]
		public void CorrectBullets()
		{
			string filename = @"Content\FireRef.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

            Assert.IsFalse(mover.HasSpawnedBullets);

			manager.Update();

			Assert.AreEqual(2, manager.movers.Count);
            Assert.IsTrue(mover.HasSpawnedBullets);

			mover = manager.movers[1];
			Assert.AreEqual("testBullet", mover.Label);
		}

		[Test()]
		public void CorrectSpeedFromParam()
		{
			string filename = @"Content\FireRefParam.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

			manager.Update();

			Assert.AreEqual(2, manager.movers.Count);

			mover = manager.movers[1];
			Assert.AreEqual("testBullet", mover.Label);
			Assert.AreEqual(15.0f, mover.Speed);
		}
	}
}

