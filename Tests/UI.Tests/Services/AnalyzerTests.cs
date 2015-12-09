using System.Linq;
using NUnit.Framework;
using Common;
using UI.Services;

namespace UI.Tests.Services
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

			var expected = new[] { Record.Types.Expense, Record.Types.Debt, Record.Types.Shared, Record.Types.Income };
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
			Expect(actual, Count.EqualTo(6));
		}

		[Test]
		public void GroupByCategory_Always_GroupsThem()
		{
			var diagrams = new Analyzer();

			var actual = diagrams.GroupByCategory(records)
								 .Select(grouping => grouping.Key)
								 .ToList();

			var expected = new[] { Record.Categories.Food, Record.Categories.House, Record.Categories.Deposit, Record.Categories.Health, Record.Categories.Maxim };
			Expect(actual, EquivalentTo(expected));
		}

		[Test]
		public void GroupByDay_Always_GroupsThem()
		{
			var diagrams = new Analyzer();

			var actual = diagrams.GroupByDay(records).ToList();

			Expect(actual.Select(g => g.Key), EquivalentTo(new[] {"1", "2", "3"}));
			Expect(actual.Select(g => g.Key), Ordered);
			Expect(actual.Select(g => g.Count()), EquivalentTo(new[] { 9, 6, 3 }));
		}

		[Test]
		public void CalculateTotalByMonth_Always_CalculateSummaryAmountForEachMonth()
		{
			var analyzer = new Analyzer();

			var actual = analyzer.CalculateTotalByMonth(records);

			Expect(actual, Count.EqualTo(3));
			Expect(actual.Select(p => p.Key), EquivalentTo(new[] {10, 11, 12}));
			Expect(actual.Select(p => p.Value), All.EqualTo(600));
		}
	}
}