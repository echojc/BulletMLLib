using NUnit.Framework;
using System;
using BulletMLLib;

namespace BulletMLTests
{
	[TestFixture()]
	public class BulletNodeTest
	{
		[Test()]
		public void CreatedBulletNode()
		{
			string filename = @"Content\BulletEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			Assert.IsNotNull(pattern.RootNode);
		}

		[Test()]
		public void CreatedBulletNode1()
		{
			string filename = @"Content\BulletEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			BulletNode testBulletNode = pattern.RootNode.GetChild(ENodeName.bullet) as BulletNode;
			Assert.IsNotNull(testBulletNode);
		}

		[Test()]
		public void SetBulletLabelNode()
		{
			string filename = @"Content\BulletEmpty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			BulletNode testBulletNode = pattern.RootNode.GetChild(ENodeName.bullet) as BulletNode;
			Assert.AreEqual("test", testBulletNode.Label);
		}
	}
}

