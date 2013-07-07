using System;
using NUnit.Framework;
using FeedMeMom.Common;

namespace FeedMeMom.Tests
{
	[TestFixture]
	public class TimeStopTest
	{
		[Test]
		public void It_should_measure_time_for_start_stop()
		{
			var target = new TimeStop();
			Time.NowIs(new DateTime(2010, 1, 1, 12, 0, 0));
			target.Start();
			Time.NowIs(new DateTime(2010, 1, 1, 12, 5, 0));
			target.Stop();
			Assert.AreEqual(TimeSpan.FromMinutes(5), target.Length);
		}

		[Test]
		public void It_should_measure_time_for_multiple_start_stop()
		{
			var target = new TimeStop();
			Time.NowIs(new DateTime(2010, 1, 1, 12, 0, 0));
			target.Start();

			Time.NowIs(new DateTime(2010, 1, 1, 12, 5, 0));
			target.Stop();
			target.Start();

			Time.NowIs(new DateTime(2010, 1, 1, 12, 10, 0));
			Assert.AreEqual(TimeSpan.FromMinutes(5), target.Length);
			Assert.AreEqual(TimeSpan.FromMinutes(10), target.GetTotalLength());

			target.Stop();
			Assert.AreEqual(TimeSpan.FromMinutes(10), target.Length);
			Assert.AreEqual(TimeSpan.FromMinutes(10), target.GetTotalLength());

			target.Start();
			Time.NowIs(new DateTime(2010, 1, 1, 12, 15, 0));
			Assert.AreEqual(TimeSpan.FromMinutes(10), target.Length);
			Assert.AreEqual(TimeSpan.FromMinutes(15), target.GetTotalLength());

			target.Stop();
			Assert.AreEqual(TimeSpan.FromMinutes(15), target.Length);
			Assert.AreEqual(TimeSpan.FromMinutes(15), target.GetTotalLength());
		}
	}
}

