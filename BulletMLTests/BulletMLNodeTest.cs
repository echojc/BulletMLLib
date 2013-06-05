using NUnit.Framework;
using System;
using System.Xml;
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

//		[Test]
//		public void TestBadStringToType()
//		{
//			Assert.Throws(Is.InstanceOf<System.ArgumentException>(), BulletMLNode.StringToType("assnuts"));
//		}

		[Test()]
		public void TestEmpty()
		{
			string filename = @"Content\Empty.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);

			Assert.AreEqual(filename, pattern.Filename);
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

			Assert.AreEqual(filename, pattern.Filename);
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

			Assert.AreEqual(filename, pattern.Filename);
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
	}
}

