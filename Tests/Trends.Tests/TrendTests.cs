using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NodaTime;
using NUnit.Framework;
using static System.Console;

namespace Trends.Tests
{
	[TestFixture]
	class TrendTests : AssertionHelper
	{
		//------------------------------------------------------------------
		public void Out<T>(IEnumerable<T> list)
		{
			foreach (var item in list)
				WriteLine(item);
		}

		//------------------------------------------------------------------
		[Test]
		public void CalculateTransactionsCalendar_Always_PlaceThemWithRightDate()
		{
			var trend = new Trends.Trend();
            trend.Operations.Add(new Operation(100, new LocalDate(2015, 1, 1), Period.FromDays(3), "Test"));

			trend.CalculateTransactionsCalendar(new LocalDate(2015, 6, 1));

			Expect(trend.Calendar, Has.Exactly(1).Property("Date").EqualTo(new LocalDate(2015, 1, 1)));
			Expect(trend.Calendar, Has.Exactly(1).Property("Date").EqualTo(new LocalDate(2015, 1, 4)));
			Expect(trend.Calendar, Has.Exactly(1).Property("Date").EqualTo(new LocalDate(2015, 1, 7)));
			Expect(trend.Calendar, Has.Exactly(1).Property("Date").EqualTo(new LocalDate(2015, 1, 10)));
			Expect(trend.Calendar, Has.Exactly(1).Property("Date").EqualTo(new LocalDate(2015, 1, 25)));
			Expect(trend.Calendar, Has.Exactly(1).Property("Date").EqualTo(new LocalDate(2015, 1, 31)));
		}

		//------------------------------------------------------------------
		[Test]
		public void CalculateTransactionsCalendar_ToJuneMonth_PlaceThemOnlyUntilJuneMonth()
		{
			var trend = new Trends.Trend();
			trend.Operations.Add(new Operation(100, new LocalDate(2015, 1, 1), Period.FromDays(3), "Test"));

			trend.CalculateTransactionsCalendar(new LocalDate(2015, 6, 1));

			Expect(trend.Calendar, Has.All.Property("Date").Property("Month").LessThan(6));
		}

		//------------------------------------------------------------------
		[Test]
		public void GroupByDate_Always_GroupsTransactionsWithSameDate()
		{
			var trend = new Trends.Trend();
			var date = new LocalDate(2015, 1, 1);
			trend.Calendar.Add(new Transaction(100, date, "1"));
			trend.Calendar.Add(new Transaction(100, date, "2"));
			trend.Calendar.Add(new Transaction(100, date, "3"));

			trend.AggregateTransactionsByDate();

			Expect(trend.Calendar, Count.EqualTo(1));
			Expect(trend.Calendar, All.Property("Date").EqualTo(date));
		}

		//------------------------------------------------------------------
		[Test]
		public void GroupByDate_Always_AggregateTheirAmmountsAndDescriptions()
		{
			var trend = new Trends.Trend();
			var date = new LocalDate(2015, 1, 1);
			trend.Calendar.Add(new Transaction(100, date, "1"));
			trend.Calendar.Add(new Transaction(100, date, "2"));
			trend.Calendar.Add(new Transaction(100, date, "3"));

			trend.AggregateTransactionsByDate();

			Expect(trend.Calendar, All.Property("Amount").EqualTo(300));
			Expect(trend.Calendar, All.Property("Description").EqualTo("1, 2, 3"));
		}

		//------------------------------------------------------------------
		[Test]
		public void GroupByDate_Always_SortsItemsByDate()
		{
			var trend = new Trends.Trend();
			trend.Calendar.Add(new Transaction(100, new LocalDate(2015, 1, 3), "3"));
			trend.Calendar.Add(new Transaction(100, new LocalDate(2015, 1, 6), "6"));
			trend.Calendar.Add(new Transaction(100, new LocalDate(2015, 1, 1), "1"));
			trend.Calendar.Add(new Transaction(100, new LocalDate(2015, 1, 5), "5"));
			trend.Calendar.Add(new Transaction(100, new LocalDate(2015, 1, 2), "2"));
			trend.Calendar.Add(new Transaction(100, new LocalDate(2015, 1, 4), "4"));

			trend.AggregateTransactionsByDate();

			Expect(trend.Calendar, Is.Ordered.By("Date"));
		}

		//------------------------------------------------------------------
		[Test]
		public void CalculateFunds_WithStartFunds_AppliesEachTransactionOnFunds()
		{
			var trend = new Trend();

			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 1), "1"));
			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 2), "2"));
			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 3), "3"));
			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 4), "4"));
			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 4), "5"));

			trend.AggregateTransactionsByDate();
			trend.CalculateFunds(1000);

			decimal[] expect = {900, 800, 700, 500};
			Expect(trend.Funds, EquivalentTo(expect));
		}



	}
}
