using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using NUnit.Framework;
using Records;
using UI.ViewModels;
using static NSubstitute.Substitute;
using static Records.Record;
using static Records.Record.Categories;
using static Records.Record.Types;

namespace UITests.ViewModels
{
	public class MonthDiagramsTests : AssertionHelper
	{
		private readonly Record[] records = 
		{
			// October
			new Record(100, Expense,Food,	"Novus",	new DateTime(2015, 10, 1)),
			new Record(100, Income,	Deposit,"",			new DateTime(2015, 10, 2)),
			new Record(100, Shared,	House,	"O3",		new DateTime(2015, 10, 3)),
			new Record(100, Debt,	Maxim,	"Out",		new DateTime(2015, 10, 4)),
			new Record(100, Expense,Health, "Pharmacy", new DateTime(2015, 10, 5)),

			// November
			new Record(100, Expense,Food,	"Novus",	new DateTime(2015, 11, 1)),
			new Record(100, Income,	Deposit,"",			new DateTime(2015, 11, 2)),
			new Record(100, Shared,	House,	"O3",		new DateTime(2015, 11, 3)),
			new Record(100, Debt,	Maxim,	"Out",		new DateTime(2015, 11, 4)),
			new Record(100, Expense,Health, "Pharmacy", new DateTime(2015, 11, 5)),

			// December
			new Record(100, Expense,Food,	"Novus",	new DateTime(2015, 12, 1)),
			new Record(100, Income,	Deposit,"",			new DateTime(2015, 12, 2)),
			new Record(100, Shared,	House,	"O3",		new DateTime(2015, 12, 3)),
			new Record(100, Debt,	Maxim,	"Out",		new DateTime(2015, 12, 4)),
			new Record(100, Expense,Health, "Pharmacy", new DateTime(2015, 12, 5)),
		};

		private MonthDiagrams Create()
		{
			return new MonthDiagrams(null);
		}

		private void Print<T>(IEnumerable<T> items) => items.ForEach(item => Console.WriteLine(item));

		[Test]
		public void GroupByType_Always_GroupsRecordsByType()
		{
			var diagrams = Create();

			var actual = diagrams.GroupByType(records)
			                     .Select(grouping => grouping.Key)
			                     .ToArray();

			var expected = new[] {Expense, Debt, Shared, Income};
			Expect(actual, EquivalentTo(expected));
		}

		[Test]
		public void FilterByMonth_Always_GivesRecordsFilteredByMonth()
		{
			var diagrams = Create();

			var actual = diagrams.FilterByMonth(records, 12)
			                     .Select(record => record.Date.Month)
			                     .ToList();

			Expect(actual, All.EqualTo(12));
			Expect(actual, Count.EqualTo(5));
		}

		[Test]
		public void GroupByCategory_Always_GroupsThem()
		{
			var diagrams = Create();

			var actual = diagrams.GroupByCategory(records)
			                     .Select(grouping => grouping.Key)
			                     .ToList();

			var expected = new[] {Food, House, Deposit, Health, Maxim};
			Expect(actual, EquivalentTo(expected));
		}

		[Test]
		public void GroupByDay_Always_GroupsThem()
		{
			var diagrams = Create();

			var actual = diagrams.GroupByDay(records).ToList();
			                     
			var expected = new[] {"1", "2", "3", "4", "5"};
			var keys = actual.Select(grouping => grouping.Key).ToList();
			Expect(keys, EquivalentTo(expected));
			Expect(keys, Ordered);
			Expect(actual.Select(grouping => grouping.Count()), All.EqualTo(3));
		}
	}
}