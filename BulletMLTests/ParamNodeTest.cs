using NUnit.Framework;
using System;
using BulletMLLib;

namespace BulletMLTests
{
	[TestFixture()]
	public class ParamNodeTest
	{
		[Test()]
		public void CreatedParamNode()
		{
			string filename = @"Content\FireRefParam.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			Assert.IsNotNull(pattern.RootNode);
		}

		[Test()]
		public void GotParamNode()
		{
			string filename = @"Content\FireRefParam.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireRefNode testFireNode = testActionNode.GetChild(ENodeName.fireRef) as FireRefNode;
			Assert.IsNotNull(testFireNode.GetChild(ENodeName.param));
		}

		[Test()]
		public void GotParamNode1()
		{
			string filename = @"Content\FireRefParam.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireRefNode testFireNode = testActionNode.GetChild(ENodeName.fireRef) as FireRefNode;
			Assert.IsNotNull(testFireNode.GetChild(ENodeName.param) as ParamNode);
		}

		[Test()]
		public void GotParamNode2()
		{
			string filename = @"Content\BulletRefParam.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			BulletRefNode refNode = testFireNode.GetChild(ENodeName.bulletRef) as BulletRefNode;
			Assert.IsNotNull(refNode.GetChild(ENodeName.param) as ParamNode);
		}

		[Test()]
		public void GotParamNode3()
		{
			string filename = @"Content\ActionRefParam.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireNode testFireNode = testActionNode.GetChild(ENodeName.fire) as FireNode;
			BulletNode testBulletNode = testFireNode.GetChild(ENodeName.bullet) as BulletNode;
			ActionRefNode testActionRefNode = testBulletNode.GetChild(ENodeName.actionRef) as ActionRefNode;
			Assert.IsNotNull(testActionRefNode.GetChild(ENodeName.param) as ParamNode);
		}
	}
}

