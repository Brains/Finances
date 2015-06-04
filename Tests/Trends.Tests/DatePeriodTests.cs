using System;
using NUnit.Framework;

namespace Trends.Tests
{
	[TestFixture]
	class DatePeriodTests : AssertionHelper
	{
		[Test]
		public void Next_FromThreeDays_ReturnsDateIncreasedByThreeDays()
		{
			DatePeriod period = DatePeriod.FromDays(3);

			var actual = period.Next(new DateTime(2000, 1, 1));

			Expect(actual, EqualTo(new DateTime(2000, 1, 4)));
		}

		[Test]
		public void Next_FromOneMonth_ReturnsDateIncreasedByOneMonth()
		{
			DatePeriod period = DatePeriod.FromMonths(1);

			var actual = period.Next(new DateTime(2000, 1, 1));

			Expect(actual, EqualTo(new DateTime(2000, 2, 1)));
		}
	}
}