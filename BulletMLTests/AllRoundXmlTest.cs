using NUnit.Framework;
using System;
using BulletMLLib;

namespace BulletMLTests
{
	[TestFixture()]
	public class AllRoundXmlTest
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
		public void CreatedActionRefTask()
		{
			string filename = @"Content\AllRound.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			ActionTask testTask = mover.FindTaskByLabelAndName("circle", ENodeName.actionRef) as ActionTask;
			Assert.IsNotNull(testTask);
		}

		[Test()]
		public void CreatedActionRefTask1()
		{
			string filename = @"Content\AllRound.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			ActionTask testTask = mover.FindTaskByLabelAndName("circle", ENodeName.actionRef) as ActionTask;

			Assert.AreEqual(ENodeName.actionRef, testTask.Node.Name);
		}

		[Test()]
		public void CreatedActionRefTask2()
		{
			string filename = @"Content\AllRound.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			ActionTask testTask = mover.FindTaskByLabelAndName("circle", ENodeName.actionRef) as ActionTask;

			Assert.AreEqual("circle", testTask.Node.Label);
		}

		[Test()]
		public void CreatedActionTask()
		{
			string filename = @"Content\AllRound.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			ActionTask testTask = mover.FindTaskByLabelAndName("circle", ENodeName.actionRef) as ActionTask;
			Assert.AreEqual(1, testTask.ChildTasks.Count);
		}

		[Test()]
		public void CreatedActionTask1()
		{
			string filename = @"Content\AllRound.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			ActionTask testTask = mover.FindTaskByLabelAndName("circle", ENodeName.actionRef) as ActionTask;
			Assert.IsNotNull(testTask.ChildTasks[0]);
		}

		[Test()]
		public void CreatedActionTask2()
		{
			string filename = @"Content\AllRound.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			ActionTask testTask = mover.FindTaskByLabelAndName("circle", ENodeName.actionRef) as ActionTask;
			ActionTask testActionTask = testTask.ChildTasks[0] as ActionTask;
			Assert.IsNotNull(testActionTask);
		}

		[Test()]
		public void CreatedActionTask3()
		{
			string filename = @"Content\AllRound.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			ActionTask testTask = mover.FindTaskByLabelAndName("circle", ENodeName.actionRef) as ActionTask;
			ActionTask testActionTask = testTask.ChildTasks[0] as ActionTask;
			Assert.IsNotNull(testActionTask.Node);
		}

		[Test()]
		public void CreatedActionTask4()
		{
			string filename = @"Content\AllRound.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			ActionTask testTask = mover.FindTaskByLabelAndName("circle", ENodeName.actionRef) as ActionTask;
			ActionTask testActionTask = testTask.ChildTasks[0] as ActionTask;
			Assert.AreEqual(ENodeName.action, testActionTask.Node.Name);
		}

		[Test()]
		public void CreatedActionTask5()
		{
			string filename = @"Content\AllRound.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			ActionTask testTask = mover.FindTaskByLabelAndName("circle", ENodeName.actionRef) as ActionTask;
			ActionTask testActionTask = testTask.ChildTasks[0] as ActionTask;
			Assert.AreEqual("circle", testActionTask.Node.Label);
		}

		[Test()]
		public void CreatedActionTask10()
		{
			string filename = @"Content\AllRound.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			ActionTask testTask = mover.FindTaskByLabelAndName("circle", ENodeName.action) as ActionTask;
			Assert.IsNotNull(testTask);
		}

		[Test()]
		public void CorrectNumberOfBullets()
		{
			string filename = @"Content\AllRound.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			manager.Update();

			//there should be 11 bullets
			Assert.AreEqual(21, manager.movers.Count);
		}
	}
}

