using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Trends.Tests
{
#if Disables
class OperationTests : AssertionHelper
	{
		[Test]
		public void NextDate_ByDefault_ReturnsNextOccuranceFromStartDate()
		{
			DateTime start = new DateTime(2000, 1, 1);
			Operation operation = new Operation(start, DatePeriod.FromDays(3));

			var actual = operation.NextDate();

			Expect(actual, EqualTo(start.AddDays(3)));
		}

		[Test]
		public void NextDate_ByDefaultFewTimes_ReturnsNextOccurancesFromStartDate()
		{
			DateTime start = new DateTime(2000, 1, 1);
			Operation operation = new Operation(start, DatePeriod.FromDays(3));
			DateTime[] occurances = new DateTime[5];

			for (int index = 0; index < 5; index++)
				occurances[index] = operation.NextDate();

			var actual = occurances.Select(date => date.Day);
			int[] expected = {4, 7, 10, 13, 16};
            Expect(actual, EqualTo(expected));
		}

		[Test]
		public void NextDate_FirstJune_ReturnsNextOccuranceAfterFirstJune()
		{
			DateTime start = new DateTime(2000, 1, 5);
			DateTime june = new DateTime(2000, 6, 1);
			Operation operation = new Operation(start, DatePeriod.FromMonths(1));

			var actual = operation.NextDate(june);

			Expect(actual, EqualTo(june.AddDays(4)));
		}
	}
#endif

}
