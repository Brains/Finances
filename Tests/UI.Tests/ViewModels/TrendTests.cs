using System;
using System.Collections.Generic;
using MoreLinq;
using NUnit.Framework;
using UI.ViewModels;
using static UI.ViewModels.Trend;

namespace UI.Tests.ViewModels
{
	public class TrendTests
	{
		[Test]
		public void CalculateCalendar()
		{
			var trend = new Trend();
			trend.Operations = new List<PermanentOperation>()
			{
				new PermanentOperation(-100, new DateTime(1, 1, 1), TimeSpan.FromDays(3), "Test")
			};

			DateTime start = new DateTime(2, 1, 1);
			DateTime end = new DateTime(2, 2, 1);
			var calendar = trend.Calculate(0, start, end);

			calendar.ForEach(date => Console.WriteLine(date));
		}
	}
}