using System;
using System.Linq;
using Common;
using NUnit.Framework;
using UI.Services;
using static Common.Record.Types;
using static Common.Record.Categories;

namespace UI.Tests.Services
{
	public class AnalyzerTests : AssertionHelper
	{
		private readonly DateTime date = DateTime.MinValue;

		[Test]
		public void GroupByType_Always_GroupsRecordsByType()
		{
			var diagrams = new Analyzer();

			Record[] records =
{
				new Record(0, Expense,	0, "", date),
				new Record(0, Debt,		0, "", date),
				new Record(0, Income,	0, "", date),
				new Record(0, Shared,	0, "", date),
				new Record(0, Expense,	0, "", date),
				new Record(0, Debt,		0, "", date),
			};				  

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
			Record[] records =
{
				new Record(0, 0, 0, "", new DateTime(1, 10, 1)),
				new Record(0, 0, 0, "", new DateTime(1, 10, 1)),
				new Record(0, 0, 0, "", new DateTime(1, 11, 1)),
				new Record(0, 0, 0, "", new DateTime(1, 11, 1)),
				new Record(0, 0, 0, "", new DateTime(1, 12, 1)),
				new Record(0, 0, 0, "", new DateTime(1, 12, 1)),
			};


			var actual = diagrams.FilterByMonth(records, 12)
								 .Select(record => record.Date.Month)
								 .ToList();

			Expect(actual, All.EqualTo(12));
			Expect(actual, Count.EqualTo(2));
		}

		[Test]
		public void GroupByCategory_Always_GroupsThem()
		{
			var diagrams = new Analyzer();
			Record[] records =
{
				new Record(0, 0,  Food,		"", date),
				new Record(0, 0,  Maxim,	"", date),
				new Record(0, 0,  Deposit,	"", date),
				new Record(0, 0,  Food,		"", date),
				new Record(0, 0,  Health,	"", date),
				new Record(0, 0,  House,    "", date),
				new Record(0, 0,  Maxim,    "", date),
				new Record(0, 0,  House,    "", date),
			};					  
								  
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
			Record[] records =
{
				new Record(0, 0, 0, "", new DateTime(1, 1, 3)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 3)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 6)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 6)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 9)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 9)),
			};

			var actual = diagrams.GroupByDay(records).ToList();

			Expect(actual.Select(g => g.Key), EquivalentTo(new[] {"3", "6", "9"}));
			Expect(actual.Select(g => g.Key), Ordered);
			Expect(actual.Select(g => g.Count()), All.EqualTo(2));
		}

		[Test]
		public void CalculateTotalByMonth_Always_CalculateSummaryAmountForEachMonth()
		{
			var analyzer = new Analyzer();
			Record[] records =
{
				new Record(10, 0, 0, "", new DateTime(1, 10, 1)),
				new Record(20, 0, 0, "", new DateTime(1, 10, 1)),
				new Record(30, 0, 0, "", new DateTime(1, 10, 1)),
				new Record(10, 0, 0, "", new DateTime(1, 11, 1)),
				new Record(20, 0, 0, "", new DateTime(1, 11, 1)),
				new Record(30, 0, 0, "", new DateTime(1, 11, 1)),
				new Record(10, 0, 0, "", new DateTime(1, 12, 1)),
				new Record(20, 0, 0, "", new DateTime(1, 12, 1)),
				new Record(30, 0, 0, "", new DateTime(1, 12, 1)),
			};

			var actual = analyzer.CalculateTotalByMonth(records);

			Expect(actual, Count.EqualTo(3));
			Expect(actual.Select(p => p.Key), EquivalentTo(new[] {10, 11, 12}));
			Expect(actual.Select(p => p.Value), All.EqualTo(60));
		}
	}
}