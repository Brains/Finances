using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using MoreLinq;
using NUnit.Framework;
using UI.ViewModels;
using static System.Console;
using static NSubstitute.Substitute;
using static UI.ViewModels.Trend;

namespace UI.Tests.ViewModels
{
	public class TrendTests
	{
		private static Trend Create()
		{
			var settings = For<ISettings>();

			return new Trend(settings);
		}

		[Test]
		public void CalculateCalendar()
		{
			var trend = Create();
			trend.Operations = new[]
			{
				new PermanentOperation(-100, new DateTime(1, 1, 1), TimeSpan.FromDays(3), "Test")
			};

			DateTime start = new DateTime(2, 1, 1);
			DateTime end = new DateTime(2, 2, 1);
			var calendar = trend.Calculate(0, start, end);

			calendar.ForEach(WriteLine);
		}

		[Test]
		public void ShiftTime_Always_AddsTimeShiftForOperations()
		{
			var trend = Create();
			trend.Operations = new[]
			{
				new PermanentOperation(-100, new DateTime(1, 1, 1), TimeSpan.FromDays(3), "Test"),
				new PermanentOperation(-100, new DateTime(1, 1, 1), TimeSpan.FromDays(3), "Test"),
				new PermanentOperation(-100, new DateTime(1, 1, 1), TimeSpan.FromDays(3), "Test"),
			};

			trend.ShiftTime(trend.Operations);

			var hours = trend.Operations.Select(o => o.Start.Hour);
			Assert.That(hours, Is.EquivalentTo(new[] { 1, 2, 3 }));
		}
	}
}