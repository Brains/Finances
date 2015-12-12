using System;
using System.Linq;
using Common;
using NUnit.Framework;
using UI.ViewModels;
using static NSubstitute.Substitute;

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
		public void Calculate_Always_CalculatesFundsAfterEachOccurrenceOfOperation()
		{
			var trend = Create();
			trend.Operations = new[]
			{
				new PermanentOperation(-100, new DateTime(1, 1, 1), TimeSpan.FromDays(3), "Food")
			};
			var initial = 1000;
			var start = new DateTime(2, 1, 1);

			var actual = trend.Calculate(initial, start, start.AddMonths(1)).ToList();

			var expectedAmounts = new[] {1000, 900, 800, 700, 600, 500, 400, 300, 200, 100};
			var expectedDays = new[] {1, 4, 7, 10, 13, 16, 19, 22, 25, 28};
			Assert.That(actual.Select(item => item.Amount), Is.EquivalentTo(expectedAmounts));
			Assert.That(actual.Select(item => item.Date.Day), Is.EquivalentTo(expectedDays));
			Assert.That(actual.Select(item => item.Description), Has.All.EqualTo("Food"));
			Assert.That(actual.First().Amount, Is.EqualTo(initial));
		}

		[Test]
		public void CalculateCalendar_Always_ReturnsCalendarWithAllTransactionsUntilEndDate()
		{
			var trend = Create();
			var start = new DateTime(2, 1, 1);
			var end = start.AddMonths(1);

			var actual = trend.CalculateCalendar(start, end - start, TimeSpan.FromDays(3).Ticks).ToList();

			var intervals = actual.Skip(1).Zip(actual, (a, b) => a - b);
			Assert.That(actual.Count, Is.EqualTo(10));
			Assert.That(intervals, Has.All.EqualTo(TimeSpan.FromDays(3)));
			Assert.That(actual.Select(item => item.Date), Has.All.GreaterThanOrEqualTo(start).And.LessThan(end));
		}

		[Test]
		public void ShiftTime_Always_AddsTimeShiftForOperations()
		{
			var trend = Create();
			trend.Operations = new[]
			{
				new PermanentOperation(-100, new DateTime(1, 1, 1), TimeSpan.FromDays(3), "Test"),
				new PermanentOperation(-100, new DateTime(1, 1, 1), TimeSpan.FromDays(3), "Test"),
				new PermanentOperation(-100, new DateTime(1, 1, 1), TimeSpan.FromDays(3), "Test")
			};

			trend.ShiftTime(trend.Operations);

			var hours = trend.Operations.Select(o => o.Start.Hour);
			Assert.That(hours, Is.EquivalentTo(new[] {1, 2, 3}));
		}
	}
}