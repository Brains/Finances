using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Records;
using UI.ViewModels;
using static Records.Record.Types;
using static Records.Record.Categories;

namespace UITests.ViewModels
{
	public class AnalyzerTests : AssertionHelper
	{
		public static readonly Record[] Records =
		{
			// October
			new Record(100, Expense,Food,   "Novus",    new DateTime(2015, 10, 1)),
			new Record(100, Income, Deposit,"",         new DateTime(2015, 10, 2)),
			new Record(100, Shared, House,  "O3",       new DateTime(2015, 10, 3)),
			new Record(100, Debt,   Maxim,  "Out",      new DateTime(2015, 10, 4)),
			new Record(100, Expense,Health, "Pharmacy", new DateTime(2015, 10, 5)),

			// November
			new Record(100, Expense,Food,   "Novus",    new DateTime(2015, 11, 1)),
			new Record(100, Income, Deposit,"",         new DateTime(2015, 11, 2)),
			new Record(100, Shared, House,  "O3",       new DateTime(2015, 11, 3)),
			new Record(100, Debt,   Maxim,  "Out",      new DateTime(2015, 11, 4)),
			new Record(100, Expense,Health, "Pharmacy", new DateTime(2015, 11, 5)),

			// December
			new Record(100, Expense,Food,   "Novus",    new DateTime(2015, 12, 1)),
			new Record(100, Income, Deposit,"",         new DateTime(2015, 12, 2)),
			new Record(100, Shared, House,  "O3",       new DateTime(2015, 12, 3)),
			new Record(100, Debt,   Maxim,  "Out",      new DateTime(2015, 12, 4)),
			new Record(100, Expense,Health, "Pharmacy", new DateTime(2015, 12, 5)),
		};

		[Test]
		public void GroupByType_Always_GroupsRecordsByType()
		{
			var diagrams = new Analyzer();

			var actual = diagrams.GroupByType(Records)
								 .Select(grouping => grouping.Key)
								 .ToArray();

			var expected = new[] { Expense, Debt, Shared, Income };
			Expect(actual, EquivalentTo(expected));
		}

		[Test]
		public void FilterByMonth_Always_GivesRecordsFilteredByMonth()
		{
			var diagrams = new Analyzer();

			var actual = diagrams.FilterByMonth(Records, 12)
								 .Select(record => record.Date.Month)
								 .ToList();

			Expect(actual, All.EqualTo(12));
			Expect(actual, Count.EqualTo(5));
		}

		[Test]
		public void GroupByCategory_Always_GroupsThem()
		{
			var diagrams = new Analyzer();

			var actual = diagrams.GroupByCategory(Records)
								 .Select(grouping => grouping.Key)
								 .ToList();

			var expected = new[] { Food, House, Deposit, Health, Maxim };
			Expect(actual, EquivalentTo(expected));
		}

		[Test]
		public void GroupByDay_Always_GroupsThem()
		{
			var diagrams = new Analyzer();

			var actual = diagrams.GroupByDay(Records).ToList();

			var expected = new[] {"1", "2", "3", "4", "5"};
			Expect(actual.Select(g => g.Key), EquivalentTo(expected));
			Expect(actual.Select(g => g.Key), Ordered);
			Expect(actual.Select(g => g.Count()), All.EqualTo(3));
		}

		[Test]
		public void CalculateTotalByMonth_Always_CalculateSummaryAmountForEachMonth()
		{
			var analyzer = new Analyzer();

			var actual = analyzer.CalculateTotalByMonth(Records);

			Expect(actual, Count.EqualTo(3));
			Expect(actual.Select(p => p.Key), EquivalentTo(new[] {10, 11, 12}));
			Expect(actual.Select(p => p.Value), All.EqualTo(500));
		}
	}
}