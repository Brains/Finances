using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;
using Records;
using UI.ViewModels;
using static NSubstitute.Substitute;
using static Records.Record.Categories;
using static Records.Record.Types;

namespace UITests.ViewModels
{
	public class MonthDiagramsTests : AssertionHelper
	{
		private IExpenses expenses;
		private static readonly DateTime Date = DateTime.MinValue;

		private MonthDiagrams Create()
		{
			var records = new[]
			{
				Create(Expense),
                Create(Income),
                Create(Shared),
                Create(Debt),
            };

			expenses = For<IExpenses>();
			expenses.Records = new ObservableCollection<Record>(records);

			return new MonthDiagrams(expenses);
		}

		private static Record Create(Record.Types type) => new Record(10,  type, Food, "", Date);
		private static Record Create(Record.Categories category) => new Record(10,  Expense, category, "", Date);
		private static Record Create(int amount) => new Record(amount, Expense, Food, "", Date);
		private static Record Create(string description) => new Record(10,  Expense, Food, description, Date);

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