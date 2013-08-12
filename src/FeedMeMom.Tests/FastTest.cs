using System;
using NUnit.Framework;
using System.Linq;

namespace FeedMeMom.Tests
{
	[TestFixture]
	public class FastTest
	{
		[Test]
		public void Test()
		{
			var result = "Pavel Vašek's iPhone".Split(new []{"'s"}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
			Assert.AreEqual("Pavel Vašek", result);
		}

	}
}

