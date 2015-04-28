using NUnit.Framework;
using System;
using BulletMLLib;


namespace BulletMLTests
{
	[TestFixture()]
	public class ChangeDirectionTest
	{
		MoverManager manager;
		Myship dude;
		BulletPattern pattern;

		[SetUp()]
		public void setupHarness()
		{
			manager = new MoverManager();
			dude = manager.dude;
			pattern = new BulletPattern();
		}

		[Test()]
		public void ChangeDirectionAbsSetupCorrect()
		{
			string filename = @"Content\ChangeDirectionAbs.xml";
			pattern.ParseXML(filename);
            dude.X = 0f;
            dude.Y = 100f;
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

			float direction = mover.Direction * 180 / (float)Math.PI;
			Assert.AreEqual(0, (int)direction);
		}

		[Test()]
		public void ChangeDirectionAbs()
		{
			string filename = @"Content\ChangeDirectionAbs.xml";
			pattern.ParseXML(filename);
            dude.X = 0f;
            dude.Y = 100f;
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

			manager.Update();
			float direction = mover.Direction * 180 / (float)Math.PI;
			Assert.AreEqual(45, (int)direction);
		}

		[Test()]
		public void ChangeDirectionAbs1()
		{
			string filename = @"Content\ChangeDirectionAbs.xml";
			pattern.ParseXML(filename);
            dude.X = 0f;
            dude.Y = 100f;
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

			manager.Update();
			manager.Update();
			float direction = mover.Direction * 180 / (float)Math.PI;
			Assert.AreEqual(90, (int)direction);
		}

		[Test()]
		public void ChangeDirectionAimSetupCorrect()
		{
			string filename = @"Content\ChangeDirectionAim.xml";
			pattern.ParseXML(filename);
            dude.X = 100f;
            dude.Y = 0f;
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

			float direction = mover.Direction * 180 / (float)Math.PI;
			Assert.AreEqual(0, (int)direction);
		}

		[Test()]
		public void ChangeDirectionAim()
		{
			string filename = @"Content\ChangeDirectionAim.xml";
			pattern.ParseXML(filename);
            dude.X = 100f;
            dude.Y = 0f;
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

			manager.Update();
			float direction = mover.Direction * 180 / (float)Math.PI;
			Assert.AreEqual(45, (int)Math.Round(direction));
		}

		[Test()]
		public void ChangeDirectionAim1()
		{
			string filename = @"Content\ChangeDirectionAim.xml";
			pattern.ParseXML(filename);
            dude.X = 100f;
            dude.Y = 0f;
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

			manager.Update();
			manager.Update();
			float direction = mover.Direction * 180 / (float)Math.PI;
			Assert.AreEqual(90, (int)Math.Round(direction));
		}

		[Test()]
		public void ChangeDirectionRelSetupCorrect()
		{
			string filename = @"Content\ChangeDirectionRel.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

			float direction = mover.Direction * 180 / (float)Math.PI;
			Assert.AreEqual(0, (int)direction);
		}

		[Test()]
		public void ChangeDirectionRel()
		{
			string filename = @"Content\ChangeDirectionRel.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

			manager.Update();
			float direction = mover.Direction * 180 / (float)Math.PI;
			Assert.AreEqual(-45, (int)direction);
		}

		[Test()]
		public void ChangeDirectionRel1()
		{
			string filename = @"Content\ChangeDirectionRel.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

			manager.Update();
			manager.Update();
			float direction = mover.Direction * 180 / (float)Math.PI;
			Assert.AreEqual(-90, (int)direction);
		}

		[Test()]
		public void ChangeDirectionSeqSetupCorrect()
		{
			string filename = @"Content\ChangeDirectionSeq.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

			float direction = mover.Direction * 180 / (float)Math.PI;
			Assert.AreEqual(0, (int)direction);
		}

		[Test()]
		public void ChangeDirectionSeq()
		{
			string filename = @"Content\ChangeDirectionSeq.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

			manager.Update();
			float direction = mover.Direction * 180 / (float)Math.PI;
			Assert.AreEqual(90, (int)direction);
		}

		[Test()]
		public void ChangeDirectionSeq1()
		{
			string filename = @"Content\ChangeDirectionSeq.xml";
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);

			manager.Update();
			manager.Update();
			float direction = mover.Direction * 180 / (float)Math.PI;
			Assert.AreEqual(180, (int)direction);
		}
	}
}

