using NUnit.Framework;
using System;
using BulletMLLib;

namespace BulletMLTests
{
	[TestFixture()]
	public class FireNodeTest
	{
		[Test()]
		public void CreatedFireNode()
		{
			string filename = @"Content\FireEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			Assert.IsNotNull(pattern.RootNode);
		}

		[Test()]
		public void CreatedFireNode1()
		{
			string filename = @"Content\FireEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			//get teh child action node
			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			Assert.IsNotNull(testActionNode);
			Assert.IsNotNull(testActionNode.GetChild(ENodeName.fire));
		}

		[Test()]
		public void CreatedFireNode2()
		{
			string filename = @"Content\FireEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			Assert.IsNotNull(testActionNode.GetChild(ENodeName.fire));
			Assert.IsNotNull(testActionNode.GetChild(ENodeName.fire)as FireNode);
		}

		[Test()]
		public void GotBulletNode()
		{
			string filename = @"Content\FireEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			Assert.IsNotNull(testFireNode.BulletDescriptionNode);
		}

		[Test()]
		public void CreatedTopLevelFireNode()
		{
			string filename = @"Content\FireEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			FireNode testFireNode = pattern.RootNode.GetChild(ENodeName.fire) as FireNode;
			Assert.IsNotNull(testFireNode);
			Assert.IsNotNull(testFireNode.BulletDescriptionNode);
			Assert.AreEqual("test", testFireNode.Label);
		}
	}
}

