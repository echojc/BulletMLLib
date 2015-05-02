using NUnit.Framework;
using System;
using System.Xml;
using BulletMLLib;
using System.Xml.Schema;

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
		public void ThrowsXmlSchemaValidationExceptionForInvalidSchemas()
		{
			string filename = @"Content\Invalid\InvalidSchema.xml";
			BulletPattern pattern = new BulletPattern();
            var thrown = Assert.Throws<InvalidBulletPatternException>(delegate
            {
			    pattern.ParseXML(filename);
            });

            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown.InnerException);
		}

		[Test()]
		public void SetsIsFinishedCorrectly()
		{
			string filename = @"Content\ActionFireWaitFire.xml";
			BulletPattern pattern = new BulletPattern();
		    pattern.ParseXML(filename);

            MoverManager manager = new MoverManager();
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

            Assert.IsFalse(mover.IsFinished);
			manager.Update(); // fire, wait 1
            Assert.IsFalse(mover.IsFinished);
			manager.Update(); // fire
            Assert.IsTrue(mover.IsFinished);
		}
	}
}

