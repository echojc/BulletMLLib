using NUnit.Framework;
using System;
using BulletMLLib;

namespace BulletMLTests
{
	[TestFixture()]
	public class FireRefNodeTest
	{
		[Test()]
		public void CreatedFireRefNode()
		{
			string filename = @"Content\FireRef.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			Assert.IsNotNull(pattern.RootNode);
		}

		[Test()]
		public void CreatedFireNode1()
		{
			string filename = @"Content\FireRef.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			//get teh child action node
			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			Assert.IsNotNull(testActionNode);
			Assert.IsNotNull(testActionNode.GetChild(ENodeName.fireRef));
		}

		[Test()]
		public void CreatedFireNode2()
		{
			string filename = @"Content\FireRef.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			Assert.IsNotNull(testActionNode.GetChild(ENodeName.fireRef));
			Assert.IsNotNull(testActionNode.GetChild(ENodeName.fireRef) as FireRefNode);
		}

		[Test()]
		public void GotFireNode()
		{
			string filename = @"Content\FireRef.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireRefNode testFireNode = testActionNode.GetChild(ENodeName.fireRef) as FireRefNode;
			Assert.IsNotNull(testFireNode.ReferencedFireNode);
		}

		[Test()]
		public void GotFireNode1()
		{
			string filename = @"Content\FireRef.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireRefNode testFireNode = testActionNode.GetChild(ENodeName.fireRef) as FireRefNode;
			Assert.IsNotNull(testFireNode.ReferencedFireNode as FireNode);
		}

		[Test()]
		public void GotCorrectFireNode()
		{
			string filename = @"Content\FireRef.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireRefNode testFireNode = testActionNode.GetChild(ENodeName.fireRef) as FireRefNode;
			FireNode fireNode = testFireNode.ReferencedFireNode as FireNode;
			Assert.AreEqual("test", fireNode.Label);
		}

		[Test()]
		public void NoBulletNode()
		{
			string filename = @"Content\FireRef.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			ActionNode testActionNode = pattern.RootNode.GetChild(ENodeName.action) as ActionNode;
			FireRefNode testFireNode = testActionNode.GetChild(ENodeName.fireRef) as FireRefNode;
			Assert.IsNull(testFireNode.BulletDescriptionNode);
		}
	}
}

