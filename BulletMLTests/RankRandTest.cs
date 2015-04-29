using NUnit.Framework;
using System;
using BulletMLLib;
using System.Collections.Generic;

namespace BulletMLTests
{
    [TestFixture()]
    public class RankRandTest
    {
		MoverManager manager;
		Myship dude;

		[SetUp()]
		public void setupHarness()
		{
			manager = new MoverManager();
			dude = manager.dude;
		}

        [Test()]
        public void CorrectlyEvalsRank()
        {
			string filename = @"Content\FireDirectionRank.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			BulletMLTask myTask = mover.Tasks[0];
			FireTask testTask = myTask.ChildTasks[0] as FireTask;

            Assert.AreEqual(0.3246f, testTask.FireDirection, delta: 0.0001f);
        }

        [Test()]
        public void CorrectlyEvalsRand()
        {
			string filename = @"Content\FireDirectionRand.xml";
			BulletPattern pattern = new BulletPattern();
			pattern.ParseXML(filename);
			Mover mover = (Mover)manager.CreateBullet();
			mover.InitTopNode(pattern.RootNode);
			BulletMLTask myTask = mover.Tasks[0];
			FireTask testTask = myTask.ChildTasks[0] as FireTask;

			Assert.AreEqual(0.1337f, testTask.FireDirection, delta: 0.0001f);
        }
    }
}

