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
		private readonly Record[] records = FixedRecords.Data;

		[Test]
		public void GroupByType_Always_GroupsRecordsByType()
		{
			var diagrams = new Analyzer();

			var actual = diagrams.GroupByType(records)
								 .Select(grouping => grouping.Key)
								 .ToArray();

			var expected = new[] { Expense, Debt, Shared, Income };
			Expect(actual, EquivalentTo(expected));
		}

		[Test]
		public void FilterByMonth_Always_GivesRecordsFilteredByMonth()
		{
			var diagrams = new Analyzer();

			var actual = diagrams.FilterByMonth(records, 12)
								 .Select(record => record.Date.Month)
								 .ToList();

			Expect(actual, All.EqualTo(12));
			Expect(actual, Count.EqualTo(5));
		}

		[Test]
		public void GroupByCategory_Always_GroupsThem()
		{
			var diagrams = new Analyzer();

			var actual = diagrams.GroupByCategory(records)
								 .Select(grouping => grouping.Key)
								 .ToList();

			var expected = new[] { Food, House, Deposit, Health, Maxim };
			Expect(actual, EquivalentTo(expected));
		}

		[Test]
		public void GroupByDay_Always_GroupsThem()
		{
			var diagrams = new Analyzer();

			var actual = diagrams.GroupByDay(records).ToList();

			var expected = new[] {"1", "2", "3", "4", "5"};
			Expect(actual.Select(g => g.Key), EquivalentTo(expected));
			Expect(actual.Select(g => g.Key), Ordered);
			Expect(actual.Select(g => g.Count()), All.EqualTo(3));
		}

		[Test]
		public void CalculateTotalByMonth_Always_CalculateSummaryAmountForEachMonth()
		{
			var analyzer = new Analyzer();

			var actual = analyzer.CalculateTotalByMonth(records);

			Expect(actual, Count.EqualTo(3));
			Expect(actual.Select(p => p.Key), EquivalentTo(new[] {10, 11, 12}));
			Expect(actual.Select(p => p.Value), All.EqualTo(500));
		}
	}
}