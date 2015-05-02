using NUnit.Framework;
using System;
using BulletMLLib;

namespace BulletMLTests
{
	[TestFixture()]
	public class BulletMLNodeTest
	{
		[Test()]
		public void TestStringToType()
		{
			Assert.AreEqual(BulletMLNode.StringToType(""), ENodeType.none);
			Assert.AreEqual(BulletMLNode.StringToType("none"), ENodeType.none);
			Assert.AreEqual(BulletMLNode.StringToType("aim"), ENodeType.aim);
			Assert.AreEqual(BulletMLNode.StringToType("absolute"), ENodeType.absolute);
			Assert.AreEqual(BulletMLNode.StringToType("relative"), ENodeType.relative);
			Assert.AreEqual(BulletMLNode.StringToType("sequence"), ENodeType.sequence);
		}

		[Test()]
		public void TestEmpty()
		{
			string filename = @"Content\Empty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			Assert.AreEqual(EPatternType.none, pattern.Orientation);

			Assert.IsNotNull(pattern.RootNode);
			Assert.AreEqual(pattern.RootNode.Name, ENodeName.bulletml);
			Assert.AreEqual(pattern.RootNode.NodeType, ENodeType.none);
		}

		[Test()]
		public void TestEmptyFromString()
		{
            BulletPattern pattern = BulletPattern.FromString("<bulletml/>");

			Assert.AreEqual(EPatternType.none, pattern.Orientation);

			Assert.IsNotNull(pattern.RootNode);
			Assert.AreEqual(pattern.RootNode.Name, ENodeName.bulletml);
			Assert.AreEqual(pattern.RootNode.NodeType, ENodeType.none);
		}

		[Test()]
		public void TestEmptyHoriz()
		{
			string filename = @"Content\EmptyHoriz.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			Assert.AreEqual(EPatternType.horizontal, pattern.Orientation);

			Assert.IsNotNull(pattern.RootNode);
			Assert.AreEqual(pattern.RootNode.Name, ENodeName.bulletml);
			Assert.AreEqual(pattern.RootNode.NodeType, ENodeType.none);
		}

		[Test()]
		public void TestEmptyVert()
		{
			string filename = @"Content\EmptyVert.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			Assert.AreEqual(EPatternType.vertical, pattern.Orientation);

			Assert.IsNotNull(pattern.RootNode);
			Assert.AreEqual(pattern.RootNode.Name, ENodeName.bulletml);
			Assert.AreEqual(pattern.RootNode.NodeType, ENodeType.none);
		}

		[Test()]
		public void TestIsParent()
		{
			string filename = @"Content\Empty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			Assert.AreEqual(pattern.RootNode, pattern.RootNode.GetRootNode());
		}

		[Test()]
		public void TestTaskCompletionBehaviour()
		{
			string filename = @"Content\ActionFireWaitFire.xml";
			BulletPattern pattern = new BulletPattern();
		    pattern.ParseXML(filename);

            MoverManager manager = new MoverManager();
            manager.dude.X = 100;
            manager.dude.Y = -100;

			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

            Assert.IsFalse(mover.IsCompletedTasks);
            Assert.AreEqual(1, manager.movers.Count);

            double factor = Math.Sqrt(2) * 0.5;

			manager.Update(); // fire, wait 1
            Assert.IsFalse(mover.IsCompletedTasks);
            Assert.AreEqual(2, manager.movers.Count);
            Mover mover2 = manager.movers[1];
            Assert.AreEqual(0, mover.X);
            Assert.AreEqual(0, mover.Y);
            Assert.AreEqual(1 * factor, mover2.X, 0.0001f);
            Assert.AreEqual(-1 * factor, mover2.Y, 0.0001f);

			manager.Update(); // fire
            Assert.IsTrue(mover.IsCompletedTasks);
            Assert.AreEqual(3, manager.movers.Count);
            Mover mover3 = manager.movers[2];
            Assert.AreEqual(0, mover.X);
            Assert.AreEqual(0, mover.Y);
            Assert.AreEqual(2 * factor, mover2.X, 0.0001f);
            Assert.AreEqual(-2 * factor, mover2.Y, 0.0001f);
            Assert.AreEqual(1 * factor, mover3.X, 0.0001f);
            Assert.AreEqual(-1 * factor, mover3.Y, 0.0001f);

			manager.Update(); // just bullet movement
            Assert.AreEqual(3, manager.movers.Count);
            Assert.AreEqual(0, mover.X);
            Assert.AreEqual(0, mover.Y);
            Assert.AreEqual(3 * factor, mover2.X, 0.0001f);
            Assert.AreEqual(-3 * factor, mover2.Y, 0.0001f);
            Assert.AreEqual(2 * factor, mover3.X, 0.0001f);
            Assert.AreEqual(-2 * factor, mover3.Y, 0.0001f);
		}
	}
}

