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
		private readonly DateTime date = new DateTime(1, 1, 1);

		private Diagrams Create()
		{
			return new Diagrams(null);
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
		public void SquashRecords_Always_AggregateRecordsAmountInGroup()
		{
			var diagrams = Create();
			Record[] records =
			{
				new Record(100, 0, Food, "First", date),
				new Record(100, 0, Food, "Second", date),
				new Record(100, 0, Food, "Third", date),
			};
			var grouping = records.GroupBy(r => r.Category).First();

			var actual = diagrams.SquashRecords(grouping);

			Assert.That(actual.Amount, Is.EqualTo(300));
		}

		[Test]
		public void SquashRecords_Always_AggregateRecordsDescriptionInGroup()
		{
			var diagrams = Create();
			Record[] records =
			{
				new Record(100, 0, Food, "First", date),
				new Record(100, 0, Food, "Second", date),
				new Record(100, 0, Food, "Third", date),
			};
			var grouping = records.GroupBy(r => r.Category).First();

			var actual = diagrams.SquashRecords(grouping);

			Assert.That(actual.Description, Is.EqualTo("First\nSecond\nThird"));
		}

		[Test]
		public void GroupByDay_Always_ReturnsGroupedByDayRecords()
		{
			var diagrams = Create();
			Record[] records =
			{
				new Record(0, 0, 0, "", new DateTime(1, 1, 6)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 3)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 6)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 9)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 3)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 9)),
			};

			var actual = diagrams.GroupByDay(records).ToList();

			Expect(actual.Select(g => g.Key), EquivalentTo(new[] { "3", "6", "9" }));
		}
		[Test]
		public void GroupByDay_Always_ReturnsOrderedRecords()
		{
			var diagrams = Create();
			Record[] records =
			{
				new Record(0, 0, 0, "", new DateTime(1, 1, 6)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 3)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 6)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 9)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 3)),
				new Record(0, 0, 0, "", new DateTime(1, 1, 9)),
			};

			var actual = diagrams.GroupByDay(records).ToList();

			Expect(actual.Select(g => g.Key), Ordered);
		}

		[Test]
		public void CalculateTotalByMonth_Always_ReturnsSummaryAmountForEachMonth()
		{
			var diagrams = Create();
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

			var actual = diagrams.CalculateTotalByMonth(records);

			Expect(actual, Count.EqualTo(3));
			Expect(actual.Select(p => p.Key), EquivalentTo(new[] { 10, 11, 12 }));
			Expect(actual.Select(p => p.Value), All.EqualTo(60));
		}

		[Test]
		public void CalculateBalanceByMonth_Always_HasExpensesToIncomeRationGroupedByMonth()
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
		public void GroupByCategory_Always_ReturnsCategoriesWithTotalAmount()
		{
			var diagrams = Create();
			Record[] records =
			{
				new Record(100, 0, House, "", date),
				new Record(100, 0, Food, "", date),
				new Record(100, 0, Deposit, "", date),
				new Record(100, 0, Health, "", date),
				new Record(100, 0, House, "", date),
				new Record(100, 0, Health, "", date),
				new Record(100, 0, Deposit, "", date),
				new Record(100, 0, Food, "", date),
			};

			var actual = diagrams.GroupByCategory(records);

			Expect(actual.Select(p => p.Key), EquivalentTo(new[] {Food, Health, House, Deposit}));
			Expect(actual.Select(p => p.Value), All.EqualTo(200));
		}

		[Test]
		public void GroupByDay_Always_ReturnsCategoriesGroupedByDay()
		{
			var diagrams = Create();
			Record[] records =
{
				new Record(100, 0, House, "", date),
				new Record(100, 0, Food, "", date),
				new Record(100, 0, Deposit, "", date),
				new Record(100, 0, Health, "", date),
				new Record(100, 0, House, "", date),
				new Record(100, 0, Health, "", date),
				new Record(100, 0, Deposit, "", date),
				new Record(100, 0, Food, "", date),
			};

			var actual = diagrams.GroupByDay(records);

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
