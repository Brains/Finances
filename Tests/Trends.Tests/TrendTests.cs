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
		private readonly LocalDate june = new LocalDate(2015, 6, 1);

		//------------------------------------------------------------------
		public void Out<T>(IEnumerable<T> list)
		{
			foreach (var item in list)
				WriteLine(item);
		}

		//------------------------------------------------------------------
		[Test]
		public void Calculate_Always_PlaceTransactionsWithRightDate()
		{
			var trend = new Trends.Trend();
            trend.Operations.Add(new Operation(100, new LocalDate(2015, 1, 1), Period.FromDays(3), "Test"));

			trend.Calculate(1000, june);

			Expect(trend.Calendar, Has.Exactly(1).Property("Date").EqualTo(new LocalDate(2015, 1, 1)));
			Expect(trend.Calendar, Has.Exactly(1).Property("Date").EqualTo(new LocalDate(2015, 1, 4)));
			Expect(trend.Calendar, Has.Exactly(1).Property("Date").EqualTo(new LocalDate(2015, 1, 7)));
			Expect(trend.Calendar, Has.Exactly(1).Property("Date").EqualTo(new LocalDate(2015, 1, 10)));
			Expect(trend.Calendar, Has.Exactly(1).Property("Date").EqualTo(new LocalDate(2015, 1, 25)));
			Expect(trend.Calendar, Has.Exactly(1).Property("Date").EqualTo(new LocalDate(2015, 1, 31)));
		}


		//------------------------------------------------------------------
		[Test]
		public void Calculate_ToJuneMonth_PlaceTransactionsOnlyUntilJuneMonth()
		{
			var trend = new Trends.Trend();
			trend.Operations.Add(new Operation(100, new LocalDate(2015, 1, 1), Period.FromDays(3), "Test"));

			trend.Calculate(1000, june);

			Expect(trend.Calendar, Has.All.Property("Date").Property("Month").LessThan(6));
		}

		//------------------------------------------------------------------
		[Test]
		public void Calculate_Always_GroupsTransactionsWithSameDate()
		{
			var trend = new Trends.Trend();
			var date = new LocalDate(2015, 1, 1);
			trend.Calendar.Add(new Transaction(100, date, "1"));
			trend.Calendar.Add(new Transaction(100, date, "2"));
			trend.Calendar.Add(new Transaction(100, date, "3"));

			trend.Calculate(1000, june);

			Expect(trend.Calendar, Count.EqualTo(1));
			Expect(trend.Calendar, All.Property("Date").EqualTo(date));
		}

		//------------------------------------------------------------------
		[Test]
		public void Calculate_Always_AggregateTransactionsAmmountsAndDescriptions()
		{
			var trend = new Trends.Trend();
			var date = new LocalDate(2015, 1, 1);
			trend.Calendar.Add(new Transaction(100, date, "1"));
			trend.Calendar.Add(new Transaction(100, date, "2"));
			trend.Calendar.Add(new Transaction(100, date, "3"));

			trend.Calculate(1000, june);

			Expect(trend.Calendar, All.Property("Amount").EqualTo(300));
			Expect(trend.Calendar, All.Property("Description").EqualTo("1, 2, 3"));
		}

		//------------------------------------------------------------------
		[Test]
		public void Calculate_Always_SortsItemsByDate()
		{
			var trend = new Trends.Trend();
			trend.Calendar.Add(new Transaction(100, new LocalDate(2015, 1, 3), "3"));
			trend.Calendar.Add(new Transaction(100, new LocalDate(2015, 1, 6), "6"));
			trend.Calendar.Add(new Transaction(100, new LocalDate(2015, 1, 1), "1"));
			trend.Calendar.Add(new Transaction(100, new LocalDate(2015, 1, 5), "5"));
			trend.Calendar.Add(new Transaction(100, new LocalDate(2015, 1, 2), "2"));
			trend.Calendar.Add(new Transaction(100, new LocalDate(2015, 1, 4), "4"));

			trend.Calculate(1000, june);

			Expect(trend.Calendar, Is.Ordered.By("Date"));
		}

		//------------------------------------------------------------------
		[Test]
		public void Calculate_WithStartFunds_AppliesEachTransactionOnFundsAmount()
		{
			var trend = new Trend();

			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 1), "1"));
			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 2), "2"));
			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 3), "3"));
			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 4), "4"));
			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 4), "5"));

			trend.Calculate(1000, june);
			var actual = trend.GetFunds().Select(funds => funds.Amount);

			decimal[] expect = {900, 800, 700, 500};
			Expect(actual, EquivalentTo(expect));
		}

		//------------------------------------------------------------------
		[Test]
		public void GetFundsDictionary_Always_ReturnsCorrectDictionary()
		{
			var trend = new Trend();
			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 1), "1"));
			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 2), "2"));
			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 3), "3"));
			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 4), "4"));
			trend.Calendar.Add(new Transaction(-100, new LocalDate(2015, 1, 4), "5"));

			trend.Calculate(1000, june);
			var actual = trend.GetFunds();

			var expected = new List<Funds>
			{
				new Funds(900, new LocalDate(2015, 1, 1), "1"),
				new Funds(800, new LocalDate(2015, 1, 2), "2"),
				new Funds(700, new LocalDate(2015, 1, 3), "3"),
				new Funds(500, new LocalDate(2015, 1, 4), "4, 5"),
			};
			Expect(actual, EqualTo(expected));
		}

	}
}
