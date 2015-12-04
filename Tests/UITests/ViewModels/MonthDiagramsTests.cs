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
				new Record(0, Expense, Food, "", Date),
                new Record(0, Income, Food, "", Date),
                new Record(0, Shared, Food, "", Date),
                new Record(0, Debt, Food, "", Date),
            };

			expenses = For<IExpenses>();
			expenses.Records = new ObservableCollection<Record>(records);

			return new MonthDiagrams(expenses);
		}

		[Test]
		public void GroupByType_Always_GroupsRecordsByType()
		{
			var diagrams = Create();
			var expected = new Dictionary<Record.Types, Record[]>
			{
				[Expense]	= new []{new Record(0, Expense, Food, "", Date) },
				[Income]	= new []{new Record(0, Income, Food, "", Date) },
				[Shared]	= new []{new Record(0, Shared, Food, "", Date) },
				[Debt]		= new []{new Record(0, Debt, Food, "", Date) },
			};

			var actual = diagrams.GroupByType(expenses.Records);

			Expect(actual, EquivalentTo(expected));
		}

		private static Record Create(Record.Types type)
		{
			return new Record(0, type, Food, "", Date);
		}
		private static Record Create(Record.Categories category)
		{
			return new Record(0, Expense, category, "", Date);
		}
		private static Record Create(int amount)
		{
			return new Record(amount, Expense, Food, "", Date);
		}
		private static Record Create(string description)
		{
			return new Record(0, Expense, Food, description, Date);
		}

	}
}