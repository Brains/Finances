using System;
using System.Linq;
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
		private static readonly DateTime Date = DateTime.MinValue;

		private MonthDiagrams Create()
		{
			return new MonthDiagrams(For<IExpenses>());
		}

		private Record Create(Types type) => new Record(10, type, Food, "", Date);
		private Record Create(Categories category) => new Record(10, Expense, category, "", Date);
		private Record Create(int amount) => new Record(amount, Expense, Food, "", Date);
		private Record Create(string description) => new Record(10, Expense, Food, description, Date);
		private Record Create(DateTime date) => new Record(10, Expense, Food, "", date);

		[Test]
		public void GroupByType_Always_GroupsRecordsByType()
		{
			var diagrams = Create();
			var records = new[]
			{
				Create(Expense),Create(Expense),
				Create(Income),	Create(Income),
				Create(Shared),	Create(Shared),
				Create(Debt),	Create(Debt),
			};

			var actual = diagrams.GroupByType(records).ToArray();

			var expected = new[] {Expense, Debt, Shared, Income};
			Expect(actual.Select(grouping => grouping.Key), EquivalentTo(expected));
			Expect(actual.Select(grouping => grouping.Count()), All.EqualTo(2));
		}

		[Test]
		public void FilterByMonth_Always_GivesRecordsFilteredByMonth()
		{
			var diagrams = Create();
			var records = new[]
			{
				Create(new DateTime(1, 10, 1)),
				Create(new DateTime(1, 11, 1)),
				Create(new DateTime(1, 12, 1))
			};

			var actual = diagrams.FilterByMonth(records, 12)
			                     .Select(record => record.Date.Month)
								 .ToList();

			Expect(actual, Count.EqualTo(1));
			Expect(actual, All.EqualTo(12));
		}

		[Test]
		public void GroupByCategory_Always_GroupsThem()
		{
			var diagrams = Create();
			var records = new[]
			{
				Create(Food),	Create(Food),
				Create(House),	Create(House),
				Create(Deposit),Create(Deposit),
			};

			var actual = diagrams.GroupByCategory(records).ToList();

			var expected = new[] {Food, House, Deposit};
			Expect(actual.Select(grouping => grouping.Key), EquivalentTo(expected));
			Expect(actual.Select(grouping => grouping.Count()), All.EqualTo(2));
		}
	}
}