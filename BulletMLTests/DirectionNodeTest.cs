using NUnit.Framework;
using System;
using BulletMLLib;

namespace BulletMLTests
{
	[TestFixture()]
	public class DirectionNodeTest
	{
		[Test()]
		public void CreatedDirectionNode()
		{
			string filename = @"Content\FireDirection.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			Assert.IsNotNull(pattern.RootNode);
		}

		[Test()]
		public void CreatedDirectionNode1()
		{
			string filename = @"Content\FireDirection.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			Assert.IsNotNull(testActionNode.GetChild(ENodeName.fire));
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			Assert.IsNotNull(testFireNode);
		}

		[Test()]
		public void CreatedDirectionNode2()
		{
			string filename = @"Content\FireDirection.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			Assert.IsNotNull(testFireNode.GetChild(ENodeName.direction));
			Assert.IsNotNull(testFireNode.GetChild(ENodeName.direction) as DirectionNode);
		}

		[Test()]
		public void DirectionNodeDefaultValue()
		{
			string filename = @"Content\FireDirection.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			DirectionNode testDirectionNode = testFireNode.GetChild(ENodeName.direction) as DirectionNode;

			Assert.AreEqual(ENodeType.aim, testDirectionNode.NodeType);
		}

		[Test()]
		public void DirectionNodeAim()
		{
			string filename = @"Content\FireDirectionAim.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			DirectionNode testDirectionNode = testFireNode.GetChild(ENodeName.direction) as DirectionNode;

			Assert.AreEqual(ENodeType.aim, testDirectionNode.NodeType);
		}

		[Test()]
		public void DirectionNodeAbsolute()
		{
			string filename = @"Content\FireDirectionAbsolute.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			DirectionNode testDirectionNode = testFireNode.GetChild(ENodeName.direction) as DirectionNode;

			Assert.AreEqual(ENodeType.absolute, testDirectionNode.NodeType);
		}

		[Test()]
		public void DirectionNodeSequence()
		{
			string filename = @"Content\FireDirectionSequence.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			DirectionNode testDirectionNode = testFireNode.GetChild(ENodeName.direction) as DirectionNode;

			Assert.AreEqual(ENodeType.sequence, testDirectionNode.NodeType);
		}

		[Test()]
		public void DirectionNodeRelative()
		{
			string filename = @"Content\FireDirectionRelative.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			DirectionNode testDirectionNode = testFireNode.GetChild(ENodeName.direction) as DirectionNode;

			Assert.AreEqual(ENodeType.relative, testDirectionNode.NodeType);
		}
	}
}

