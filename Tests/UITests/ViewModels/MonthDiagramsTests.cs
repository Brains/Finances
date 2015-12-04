using System;
using System.Collections.Generic;
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
				Create(Expense),
				Create(Income),
				Create(Shared),
				Create(Debt)
			};

			var actual = diagrams.GroupByType(records).Select(grouping => grouping.Key);

			var expected = new[] {Expense, Debt, Expense, Shared, Income};
			Expect(actual, EquivalentTo(expected));
		}

		[Test]
		public void GroupByType_Always_GroupsRecordsByType()
		{
			var diagrams = Create();
			var expected = new Dictionary<Record.Types, Record[]>
			{
				[Expense]	= new []{Create(Expense) },
				[Income]	= new []{Create(Income) },
				[Shared]	= new []{Create(Shared) },
				[Debt]		= new []{Create(Debt) },
			};

			var actual = diagrams.GroupByType(expenses.Records);

			Expect(actual, EquivalentTo(expected));
		}
	}
}