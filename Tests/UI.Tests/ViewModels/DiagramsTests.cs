using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using MoreLinq;
using NSubstitute;
using NUnit.Framework;
using Common;
using UI.Services;
using UI.ViewModels;
using static NSubstitute.Substitute;
using static Common.Record;
using Args = System.ComponentModel.PropertyChangedEventArgs;

namespace UI.Tests.ViewModels
{
	public class DiagramsTests : AssertionHelper
	{
		private readonly Record[] records = FixedRecords.Data;
		private IAnalyzer analyzer;
		private IExpenses expenses;

		public IEnumerable<Record> Any => Arg.Any<IEnumerable<Record>>();

		private Diagrams Create()
		{
			analyzer = For<IAnalyzer>();
			expenses = For<IExpenses>();
			expenses.Records = new ObservableCollection<Record>(records);

			return new Diagrams(expenses, analyzer);
		}

		private void Print<T>(IEnumerable<T> items) => items.ForEach(item => Console.WriteLine(item));

		private bool IsExpense(Record record)
		{
			return record.Type == Types.Expense
			       || record.Type == Types.Shared;
		}

		[Test]
		public void UpdateBalanceByMonth_Always_HasExpensesToIncomeRationGroupedByMonth()
		{
			var diagrams = Create();
			var monthes = new[] {10, 11, 12};
			analyzer.CalculateTotalByMonth(Any).Returns(
				monthes.ToDictionary(month => month, month => 300m),
				monthes.ToDictionary(month => month, month => 100m));

			diagrams.Update();

			// Assserts
			var keys = diagrams.BalanceByMonth.Select(pair => pair.Key);
			var expected = new[] {10, 11, 12};
			var expense = diagrams.BalanceByMonth[Types.Expense];
			var income = diagrams.BalanceByMonth[Types.Income];

			Expect(keys, EquivalentTo(new[] {Types.Expense, Types.Income}));
			Expect(expense.Select(p => p.Key), EquivalentTo(expected));
			Expect(income.Select(p => p.Key), EquivalentTo(expected));
			Expect(expense.Select(p => p.Value), All.EqualTo(300));
			Expect(income.Select(p => p.Value), All.EqualTo(100));
		}

		[Test]
		public void UpdateExpenseByCategory_Always_HasExpenseCategoriesWithTotalAmount()
		{
			var diagrams = Create();
			analyzer.GroupByCategory(Any)
			        .Returns(records.Where(IsExpense)
			                        .ToLookup(record => record.Category));

			diagrams.Update();

			// Assserts
			var keys = diagrams.ExpenseByCategory.Select(pair => pair.Key);
			var values = diagrams.ExpenseByCategory.Select(pair => pair.Value);
			var expected = new[] {Categories.Food, Categories.Health, Categories.House};

			Expect(keys, EquivalentTo(expected));
			Expect(values, EquivalentTo(new[] { 600, 300, 300 }));
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
			var expected = new[] {Categories.Deposit};

			Expect(keys, EquivalentTo(expected));
			Expect(values, All.EqualTo(300));
		}

		[Test]
		public void UpdateExpenseByDay_Always_HasExpenceGroupedByDay()
		{
			var diagrams = Create();
			analyzer.GroupByDay(Any)
			        .Returns(records.Where(IsExpense)
			                        .ToLookup(record => record.Date.ToString("%d")));

			diagrams.Update();

			// Assserts
			var actual = diagrams.ExpenseByDay;
			Expect(actual, Count.EqualTo(2));
			Expect(actual["1"].Length, EqualTo(2));
			Expect(actual["2"].Length, EqualTo(1));
			Expect(actual["1"][0],	Property("Category").EqualTo(Categories.Food));
			Expect(actual["1"][1],	Property("Category").EqualTo(Categories.Health));
			Expect(actual["2"],		All.Property("Category").EqualTo(Categories.House));
			Expect(actual["1"][0],	Property("Amount").EqualTo(600));
			Expect(actual["1"][1],	Property("Amount").EqualTo(300));
			Expect(actual["2"],		All.Property("Amount").EqualTo(300));
			Expect(actual["1"][0],	Property("Description").Contain("Novus\nWater"));
			Expect(actual["1"][1],	Property("Description").Contain("Pharmacy"));
			Expect(actual["2"],		All.Property("Description").Contain("O3"));
		}

		[Test]
		public void Update_Always_NotifiesAboutDataSetsChanges()
		{
			var diagrams = Create();
			var handler = For<PropertyChangedEventHandler>();
			diagrams.PropertyChanged += handler;

			diagrams.Update();

			handler.Received(1).Invoke(diagrams, Arg.Is<Args>(args => args.PropertyName == "BalanceByMonth"));
			handler.Received(1).Invoke(diagrams, Arg.Is<Args>(args => args.PropertyName == "ExpenseByCategory"));
			handler.Received(1).Invoke(diagrams, Arg.Is<Args>(args => args.PropertyName == "IncomeByCategory"));
			handler.Received(1).Invoke(diagrams, Arg.Is<Args>(args => args.PropertyName == "ExpenseByDay"));
        }
	}
}