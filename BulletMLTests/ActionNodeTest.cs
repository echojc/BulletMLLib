using NUnit.Framework;
using System;
using BulletMLLib;

namespace BulletMLTests
{
	[TestFixture()]
	public class ActionNodeTest
	{
		[Test()]
		public void TestOneTop()
		{
			string filename = @"Content\ActionOneTop.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testNode = pattern.RootNode.FindLabelNode("top", ENodeName.action) as ActionNode;
			Assert.IsNotNull(testNode);
		}

		[Test()]
		public void TestNoRepeatNode()
		{
			string filename = @"Content\ActionOneTop.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testNode = pattern.RootNode.FindLabelNode("top", ENodeName.action) as ActionNode;
			Assert.IsNull(testNode.ParentRepeatNode);
		}

		[Test()]
		public void TestManyTop()
		{
			string filename = @"Content\ActionManyTop.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testNode = pattern.RootNode.FindLabelNode("top1", ENodeName.action) as ActionNode;
			Assert.IsNotNull(testNode);
			testNode = pattern.RootNode.FindLabelNode("top2", ENodeName.action) as ActionNode;
			Assert.IsNotNull(testNode);
		}
	}
}

