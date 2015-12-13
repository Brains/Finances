using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using MoreLinq;
using NSubstitute;
using NUnit.Framework;
using Common;
using Common.Storages;
using UI.ViewModels;
using static NSubstitute.Substitute;
using static Common.Record.Categories;
using static Common.Record.Types;
using Args = System.ComponentModel.PropertyChangedEventArgs;

namespace UI.Tests.ViewModels
{
	public class DiagramsTests : AssertionHelper
	{
		private readonly Record[] records = FixedRecords.Data;
		private IExpenses expenses;

		public IEnumerable<Record> Any => Arg.Any<IEnumerable<Record>>();

		private Diagrams Create()
		{
			expenses = For<IExpenses>();
			SetRecords(records);

			return new Diagrams(expenses);
		}

		private void SetRecords(Record[] records)
		{
			expenses.Records = new ObservableCollection<Record>(records);
		}

		[Test]
		public void FilterByMonth_Always_GivesRecordsFilteredByMonth()
		{
			var diagrams = Create();
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
		public void GroupByDay_Always_GroupsThem()
		{
			var diagrams = Create();
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

			Expect(actual.Select(g => g.Key), EquivalentTo(new[] { "3", "6", "9" }));
			Expect(actual.Select(g => g.Key), Ordered);
			Expect(actual.Select(g => g.Value.Count()), All.EqualTo(2));
		}

		[Test]
		public void CalculateTotalByMonth_Always_CalculateSummaryAmountForEachMonth()
		{
			var analyzer = Create();
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
			Expect(actual.Select(p => p.Key), EquivalentTo(new[] { 10, 11, 12 }));
			Expect(actual.Select(p => p.Value), All.EqualTo(60));
		}

		[Test]
		public void UpdateBalanceByMonth_Always_HasExpensesToIncomeRationGroupedByMonth()
		{
			var diagrams = Create();
			var monthes = new[] {10, 11, 12};

			diagrams.Update();

			// Assserts
			var keys = diagrams.BalanceByMonth.Select(pair => pair.Key);
			var expected = new[] {10, 11, 12};
			var expense = diagrams.BalanceByMonth[Expense];
			var income = diagrams.BalanceByMonth[Income];

			Expect(keys, EquivalentTo(new[] {Expense, Income}));
			Expect(expense.Select(p => p.Key), EquivalentTo(expected));
			Expect(income.Select(p => p.Key), EquivalentTo(expected));
			Expect(expense.Select(p => p.Value), All.EqualTo(300));
			Expect(income.Select(p => p.Value), All.EqualTo(100));
		}

		[Test]
		public void UpdateExpenseByCategory_Always_HasExpenseCategoriesWithTotalAmount()
		{
			var diagrams = Create();

			diagrams.Update();

			// Assserts
			var keys = diagrams.ExpenseByCategory.Select(pair => pair.Key);
			var values = diagrams.ExpenseByCategory.Select(pair => pair.Value);
			var expected = new[] {Food, Health, House};

			Expect(keys, EquivalentTo(expected));
			Expect(values, EquivalentTo(new[] { 600, 300, 300 }));
		}

		[Test]
		public void UpdateIncomeByCategory_Always_HasIncomeCategoriesWithTotalAmount()
		{
			var diagrams = Create();

			diagrams.Update();

			// Assserts
			var keys = diagrams.IncomeByCategory.Select(pair => pair.Key);
			var values = diagrams.IncomeByCategory.Select(pair => pair.Value);
			var expected = new[] {Deposit};

			Expect(keys, EquivalentTo(expected));
			Expect(values, All.EqualTo(300));
		}

		[Test]
		public void UpdateExpenseByDay_Always_HasExpenceGroupedByDay()
		{
			var diagrams = Create();

			diagrams.Update();

			// Assserts
			var actual = diagrams.ExpenseByDay;
			Expect(actual, Count.EqualTo(2));
			Expect(actual["1"].Length, EqualTo(2));
			Expect(actual["2"].Length, EqualTo(1));
			Expect(actual["1"][0],	Property("Category").EqualTo(Food));
			Expect(actual["1"][1],	Property("Category").EqualTo(Health));
			Expect(actual["2"],		All.Property("Category").EqualTo(House));
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
