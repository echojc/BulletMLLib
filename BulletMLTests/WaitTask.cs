using NUnit.Framework;
using System;
using BulletMLLib;

namespace BulletMLTests
{
	[TestFixture()]
	public class WaitTask
	{
		MoverManager manager;
		Myship dude;
		BulletPattern pattern;

		[SetUp()]
		public void setupHarness()
		{
			dude = new Myship();
			manager = new MoverManager(dude.Position);
			pattern = new BulletPattern();
		}

		[Test()]
		public void WaitOneTaskTest()
		{
			string filename = @"Content\WaitOne.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			manager.Update();
			Assert.AreEqual(1, manager.movers.Count);
		}

		[Test()]
		public void WaitOneTaskTest1()
		{
			string filename = @"Content\Vanish.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			manager.Update();
			manager.Update();
			Assert.AreEqual(0, manager.movers.Count);
		}

		[Test()]
		public void WaitZeroTaskTest()
		{
			string filename = @"Content\Vanish.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			manager.Update();
			Assert.AreEqual(0, manager.movers.Count);
		}
	}
}

