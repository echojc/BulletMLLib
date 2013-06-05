using NUnit.Framework;
using System;
using BulletMLLib;
using System.IO;

namespace BulletMLTests
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void ValidateTestData()
		{
			//Get all the xml files in the Content\\Samples directory
			foreach (var source in Directory.GetFiles("Content", "*.xml"))
			{
				//load & validate the pattern
				BulletPattern pattern = new BulletPattern();
				pattern.ParseXML(source);
			}
		}
	}
}

