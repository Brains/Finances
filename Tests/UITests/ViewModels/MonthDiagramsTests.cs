using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoreLinq;
using NSubstitute;
using NUnit.Framework;
using Records;
using UI.ViewModels;
using static NSubstitute.Substitute;
using static Records.Record;

namespace UITests.ViewModels
{
	public class MonthDiagramsTests : AssertionHelper
	{
		private readonly Record[] records = FixedRecords.Data;
		private IAnalyzer analyzer;
		private IExpenses expenses;

		public IEnumerable<Record> Any => Arg.Any<IEnumerable<Record>>();

		private MonthDiagrams Create()
		{
			analyzer = For<IAnalyzer>();
			expenses = For<IExpenses>();
			expenses.Records = new ObservableCollection<Record>(records);

			return new MonthDiagrams(expenses, analyzer);
		}

		private void Print<T>(IEnumerable<T> items) => items.ForEach(item => Console.WriteLine(item));

		[Test]
		public void UpdateBalanceByMonth_Always_HasExpensesToIncomeRationGroupedByMonth()
		{
			var diagrams = Create();
			var monthes = new[]{10, 11, 12};
			analyzer.CalculateTotalByMonth(Any).Returns(
				        monthes.ToDictionary(month => month, month => 300m),
				        monthes.ToDictionary(month => month, month => 100m));

			diagrams.Update();

			// Assserts
			var keys = diagrams.BalanceByMonth.Select(pair => pair.Key);
			var keysExpected = new[] {10, 11, 12};
			var expense = diagrams.BalanceByMonth[Types.Expense];
			var income = diagrams.BalanceByMonth[Types.Income];

			Expect(keys, EquivalentTo(new[] {Types.Expense, Types.Income}));
			Expect(expense.Select(p => p.Key), EquivalentTo(keysExpected));
			Expect(income.Select(p => p.Key), EquivalentTo(keysExpected));
			Expect(expense.Select(p => p.Value), All.EqualTo(300));
			Expect(income.Select(p => p.Value), All.EqualTo(100));
		}

		[Test]
		public void UpdateExpenseByCategory_Always_HasExpenseCategoriesWithTotalAmount()
		{
			var diagrams = Create();
			analyzer.GroupByCategory(Any)
			        .Returns(records.Where(r => r.Type == Types.Expense || r.Type == Types.Shared)
			                        .ToLookup(record => record.Category));

			diagrams.Update();


			// Assserts
			var keys = diagrams.ExpenseByCategory.Select(pair => pair.Key);
			var values = diagrams.ExpenseByCategory.Select(pair => pair.Value);
			var keysExpected = new[] {Categories.Food, Categories.Health, Categories.House};

			Expect(keys, EquivalentTo(keysExpected));
			Expect(values, All.EqualTo(300));
		}

		[Test]
		public void UpdateIncomeByCategory_Always_HasIncomeCategoriesWithTotalAmount()
		{
			var diagrams = Create();
			analyzer.GroupByCategory(Any)
					.Returns(records.Where(r => r.Type == Types.Income)
									.ToLookup(record => record.Category));

			diagrams.Update();

			// Assserts
			var keys = diagrams.IncomeByCategory.Select(pair => pair.Key);
			var values = diagrams.IncomeByCategory.Select(pair => pair.Value);
			var keysExpected = new[] {Categories.Deposit};

			Expect(keys, EquivalentTo(keysExpected));
			Expect(values, All.EqualTo(300));
		}
	}
}