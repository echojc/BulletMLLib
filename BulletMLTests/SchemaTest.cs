using System;
using System.Xml.Schema;
using System.Collections.Generic;
using NUnit.Framework;
using BulletMLLib;

namespace BulletMLTests
{
	[TestFixture()]
    public class SchemaTest
    {
		[Test()]
		public void RejectsEmptyFireElement()
		{
			string filename = @"Content\Invalid\InvalidSchema.xml";
            var thrown = Assert.Throws<InvalidBulletPatternException>(delegate
            {
			    BulletPattern.FromFile(filename);
            });

            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown.InnerException);
		}

		[Test()]
		public void RequiresBulletOrBulletRefInFireElement()
		{
			string filename = @"Content\Invalid\InvalidFireElement.xml";
            var thrown = Assert.Throws<InvalidBulletPatternException>(delegate
            {
			    BulletPattern.FromFile(filename);
            });

            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown.InnerException);
		}

		[Test()]
		public void RejectsIfBothBulletAndBulletRefAreDefined()
		{
			string filename = @"Content\Invalid\InvalidFireElement2.xml";
            var thrown = Assert.Throws<InvalidBulletPatternException>(delegate
            {
			    BulletPattern.FromFile(filename);
            });

            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown.InnerException);
		}

		[Test()]
		public void AcceptsChildNodesInAnyOrderInFireElement()
		{
			string filename = @"Content\UnorderedFireElement.xml";
            BulletPattern.FromFile(filename);
		}

		[Test()]
		public void AcceptsFireElementWithoutDirectionOrSpeed()
		{
            BulletPattern.FromFile(@"Content\FireEmpty.xml");
            BulletPattern.FromFile(@"Content\FireElementJustSpeed.xml");
            BulletPattern.FromFile(@"Content\FireElementJustDirection.xml");
		}

		[Test()]
		public void ValidChangeDirections()
		{
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <changeDirection><term>1</term><direction>1</direction></changeDirection>
              </action>
            </bulletml>
            ");

            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <changeDirection><direction>1</direction><term>1</term></changeDirection>
              </action>
            </bulletml>
            ");
		}

		[Test()]
		public void InvalidChangeDirections()
		{
            var thrown = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                <bulletml>
                  <action label=""top"">
                    <changeDirection/>
                  </action>
                </bulletml>
                ");
            });

            var thrown2 = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                <bulletml>
                  <action label=""top"">
                    <changeDirection><term>1</term></changeDirection>
                  </action>
                </bulletml>
                ");
            });

            var thrown3 = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                <bulletml>
                  <action label=""top"">
                    <changeDirection><direction>1</direction></changeDirection>
                  </action>
                </bulletml>
                ");
            });

            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown.InnerException);
            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown2.InnerException);
            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown3.InnerException);
		}

		[Test()]
		public void AcceptsSpeedAndTermInAnyOrderInChangeSpeed()
		{
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <changeSpeed><term>1</term><speed>1</speed></changeSpeed>
              </action>
            </bulletml>
            ");

            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <changeSpeed><speed>1</speed><term>1</term></changeSpeed>
              </action>
            </bulletml>
            ");
		}

		[Test()]
		public void RequiresBothSpeedAndTermInChangeSpeed()
		{
            var thrown = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                <bulletml>
                  <action label=""top"">
                    <changeSpeed/>
                  </action>
                </bulletml>
                ");
            });

            var thrown2 = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                <bulletml>
                  <action label=""top"">
                    <changeSpeed><term>1</term></changeSpeed>
                  </action>
                </bulletml>
                ");
            });

            var thrown3 = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                <bulletml>
                  <action label=""top"">
                    <changeSpeed><speed>1</speed></changeSpeed>
                  </action>
                </bulletml>
                ");
            });

            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown.InnerException);
            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown2.InnerException);
            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown3.InnerException);
		}

		[Test()]
		public void AcceptsEmptyVanishElement()
		{
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <vanish/>
              </action>
            </bulletml>
            ");
		}

		[Test()]
		public void RejectsNonEmptyVanishElements()
		{
            var thrown = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                <bulletml>
                  <action label=""top"">
                    <vanish>hi</vanish>
                  </action>
                </bulletml>
                ");
            });

            var thrown2 = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                <bulletml>
                  <action label=""top"">
                    <vanish><bullet/></vanish>
                  </action>
                </bulletml>
                ");
            });

            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown.InnerException);
            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown2.InnerException);
		}

		[Test()]
		public void ValidAccels()
		{
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <accel>
                  <term>0</term>
                  <vertical type=""relative"">0</vertical>
                  <horizontal type=""absolute"">0</horizontal>
                </accel>
              </action>
            </bulletml>
            ");
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <accel>
                  <term>0</term>
                  <horizontal type=""sequence"">0</horizontal>
                </accel>
              </action>
            </bulletml>
            ");
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <accel>
                  <term>0</term>
                  <vertical>0</vertical>
                </accel>
              </action>
            </bulletml>
            ");
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <accel>
                  <term>0</term>
                </accel>
              </action>
            </bulletml>
            ");
		}

		[Test()]
		public void InvalidAccels()
		{
            var thrown = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                <bulletml>
                  <action label=""top"">
                    <accel>
                      <horizontal>0</horizontal>
                      <vertical>0</vertical>
                    </accel>
                  </action>
                </bulletml>
                ");
            });

            var thrown2 = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                <bulletml>
                  <action label=""top"">
                    <accel><term>0</term><horizontal type=""hi"">0</horizontal></accel>
                  </action>
                </bulletml>
                ");
            });

            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown.InnerException);
            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown2.InnerException);
		}

		[Test()]
		public void ValidParams()
		{
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <fireRef label=""test"">
                  <param>0</param>
                </fireRef>
              </action>
              <fire label=""test""><bullet/></fire>
            </bulletml>
            ");
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <fireRef label=""test"">
                  <param>$rank*(1+2)</param>
                </fireRef>
              </action>
              <fire label=""test""><bullet/></fire>
            </bulletml>
            ");
		}

		[Test()]
		public void InvalidParams()
		{
            var thrown = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                    <bulletml>
                      <action label=""top"">
                        <fireRef label=""test"">
                          <param/>
                        </fireRef>
                      </action>
                      <fire label=""test""><bullet/></fire>
                    </bulletml>
                ");
            });

            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown.InnerException);
		}

		[Test()]
		public void ValidWaits()
		{
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <wait>0</wait>
              </action>
            </bulletml>
            ");
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <wait>$rank*(1+2)</wait>
              </action>
            </bulletml>
            ");
		}

		[Test()]
		public void InvalidWaits()
		{
            var thrown = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                    <bulletml>
                      <action label=""top"">
                        <wait/>
                      </action>
                    </bulletml>
                ");
            });

            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown.InnerException);
		}

		[Test()]
		public void InvalidTerms()
		{
            var thrown = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                    <bulletml>
                      <action label=""top"">
                        <accel><term/></accel>
                      </action>
                    </bulletml>
                ");
            });

            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown.InnerException);
		}

		[Test()]
		public void ValidRepeats()
		{
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <repeat>
                  <times>1</times>
                  <action/>
                </repeat>
              </action>
            </bulletml>
            ");
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <repeat>
                  <times>$rank*(1+2)</times>
                  <action/>
                </repeat>
              </action>
            </bulletml>
            ");
            BulletPattern.FromString(@"
            <bulletml>
              <action label=""top"">
                <repeat>
                  <times>1</times>
                  <actionRef label=""test""/>
                </repeat>
              </action>
              <action label=""test""/>
            </bulletml>
            ");
		}

		[Test()]
		public void InvalidRepeats()
		{
            var thrown = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                    <bulletml>
                      <action label=""top"">
                        <repeat/>
                      </action>
                    </bulletml>
                ");
            });

            var thrown2 = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                    <bulletml>
                      <action label=""top"">
                        <repeat>
                          <times/>
                          <action/>
                        </repeat>
                      </action>
                    </bulletml>
                ");
            });

            var thrown3 = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                    <bulletml>
                      <action label=""top"">
                        <repeat/>
                          <action/>
                        </repeat>
                      </action>
                    </bulletml>
                ");
            });

            var thrown4 = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                    <bulletml>
                      <action label=""top"">
                        <repeat>
                          <times>1</times>
                        </repeat>
                      </action>
                    </bulletml>
                ");
            });

            var thrown5 = Assert.Throws<InvalidBulletPatternException>(delegate
            {
                BulletPattern.FromString(@"
                    <bulletml>
                      <action label=""top"">
                        <repeat>
                          <times>1</times>
                          <actionRef label=""test""/>
                          <action/>
                        </repeat>
                      </action>
                      <action label=""test""/>
                    </bulletml>
                ");
            });

            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown.InnerException);
            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown2.InnerException);
            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown3.InnerException);
            Assert.IsInstanceOf<XmlSchemaValidationException>(thrown4.InnerException);
		}
    }
}
