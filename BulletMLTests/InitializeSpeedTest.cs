using NUnit.Framework;
using System;
using BulletMLLib;

namespace BulletMLTests
{
	[TestFixture()]
	public class InitializeSpeedTest
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
		public void bulletCreated()
		{
			string filename = @"Content\FireSpeed.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			mover.Speed = 100;

			manager.Update();
			Assert.AreEqual(manager.movers.Count, 2);
		}

		[Test()]
		public void bulletDefaultSpeed()
		{
			string filename = @"Content\FireEmpty.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			BulletMLTask myTask = mover.Tasks[0];
			FireTask testTask = myTask.ChildTasks[0] as FireTask;

			Assert.IsNull(testTask.InitialSpeedTask);
			Assert.IsNull(testTask.SequenceSpeedTask);
			Assert.IsNull(testTask.InitialDirectionTask);
			Assert.IsNull(testTask.SequenceDirectionTask);
		}

		[Test()]
		public void bulletDefaultSpeed1()
		{
			string filename = @"Content\FireEmpty.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			mover.Speed = 100.0f;
			BulletMLTask myTask = mover.Tasks[0];
			FireTask testTask = myTask.ChildTasks[0] as FireTask;

			FireTask testDude = new FireTask(testTask.Node as FireNode, testTask);
			testDude.InitTask(mover);

			Assert.IsNull(testDude.InitialSpeedTask);
			Assert.IsNull(testDude.SequenceSpeedTask);
			Assert.IsNull(testDude.InitialDirectionTask);
			Assert.IsNull(testDude.SequenceDirectionTask);

			Assert.AreEqual(100.0f, testDude.FireSpeed);
		}

		[Test()]
		public void bulletDefaultSpeed2()
		{
			string filename = @"Content\FireEmpty.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			mover.Speed = 100;
			manager.Update();
			Mover testDude = manager.movers[1];

			Assert.AreEqual(100.0f, testDude.Speed);
		}

		[Test()]
		public void SpeedDefault()
		{
			string filename = @"Content\FireSpeed.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			mover.Speed = 100;
			manager.Update();
			Mover testDude = manager.movers[1];

			Assert.AreEqual(5.0f, testDude.Speed);
		}

		[Test()]
		public void AbsSpeedDefault()
		{
			string filename = @"Content\FireSpeedAbsolute.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			mover.Speed = 100;
			manager.Update();
			Mover testDude = manager.movers[1];

			Assert.AreEqual(5.0f, testDude.Speed);
		}

		[Test()]
		public void RelSpeedDefault()
		{
			string filename = @"Content\FireSpeedRelative.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			mover.Speed = 100;
			manager.Update();
			Mover testDude = manager.movers[1];

			Assert.AreEqual(105.0f, testDude.Speed);
		}

		[Test()]
		public void RightInitSpeed()
		{
			string filename = @"Content\FireSpeedBulletSpeed.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			mover.Speed = 100;
			manager.Update();
			Mover testDude = manager.movers[1];

			Assert.AreEqual(5.0f, testDude.Speed);
		}
	}
}

