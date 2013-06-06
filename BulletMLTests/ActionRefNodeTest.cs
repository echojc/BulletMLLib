using NUnit.Framework;
using System;
using BulletMLLib;

namespace BulletMLTests
{
	[TestFixture()]
	public class ActionRefNodeTest
	{
		[Test()]
		public void ValidXML()
		{
			string filename = @"Content\ActionRefEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			Assert.IsNotNull(pattern.RootNode);
		}

		[Test()]
		public void GotActionRefNode()
		{
			string filename = @"Content\ActionRefEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			Assert.IsNotNull(testActionNode);
		}

		[Test()]
		public void GotActionRefNode1()
		{
			string filename = @"Content\ActionRefEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			Assert.IsNotNull(testFireNode);
		}

		[Test()]
		public void GotActionRefNode2()
		{
			string filename = @"Content\ActionRefEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			BulletNode testBulletNode = testFireNode.GetChild(ENodeName.bullet) as BulletNode;
			Assert.IsNotNull(testBulletNode);
		}

		[Test()]
		public void GotActionRefNode3()
		{
			string filename = @"Content\ActionRefEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			BulletNode testBulletNode = testFireNode.GetChild(ENodeName.bullet) as BulletNode;
			Assert.IsNotNull(testBulletNode.GetChild(ENodeName.actionRef));
		}

		[Test()]
		public void GotActionRefNode4()
		{
			string filename = @"Content\ActionRefEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			BulletNode testBulletNode = testFireNode.GetChild(ENodeName.bullet) as BulletNode;
			Assert.IsNotNull(testBulletNode.GetChild(ENodeName.actionRef) as ActionRefNode);
		}

		[Test()]
		public void FoundActionNode()
		{
			string filename = @"Content\ActionRefEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			BulletNode testBulletNode = testFireNode.GetChild(ENodeName.bullet) as BulletNode;
			ActionRefNode testActionRefNode = testBulletNode.GetChild(ENodeName.actionRef) as ActionRefNode;

			Assert.IsNotNull(testActionRefNode.ReferencedActionNode);
		}

		[Test()]
		public void FoundActionNode1()
		{
			string filename = @"Content\ActionRefEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			BulletNode testBulletNode = testFireNode.GetChild(ENodeName.bullet) as BulletNode;
			ActionRefNode testActionRefNode = testBulletNode.GetChild(ENodeName.actionRef) as ActionRefNode;

			Assert.IsNotNull(testActionRefNode.ReferencedActionNode as ActionNode);
		}

		[Test()]
		public void FoundActionNode2()
		{
			string filename = @"Content\ActionRefEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			BulletNode testBulletNode = testFireNode.GetChild(ENodeName.bullet) as BulletNode;
			ActionRefNode testActionRefNode = testBulletNode.GetChild(ENodeName.actionRef) as ActionRefNode;
			ActionNode refNode = testActionRefNode.ReferencedActionNode as ActionNode;

			Assert.AreEqual(refNode.Label, "test");
		}

		[Test()]
		public void FoundCorrectActionNode()
		{
			string filename = @"Content\ActionRefParam.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			BulletNode testBulletNode = testFireNode.GetChild(ENodeName.bullet) as BulletNode;
			ActionRefNode testActionRefNode = testBulletNode.GetChild(ENodeName.actionRef) as ActionRefNode;
			ActionNode refNode = testActionRefNode.ReferencedActionNode as ActionNode;

			Assert.AreEqual(refNode.Label, "test2");
		}
	}
}
