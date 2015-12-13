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
			var expenses = For<IExpenses>();
			expenses.Records = new ObservableCollection<Record>();

			return new Diagrams(expenses);
		}

		private static DateTime Month(int month)
		{
			return new DateTime(1, month, 1);
		}

		private static DateTime Day(int day)
		{
			return new DateTime(1, 1, day);
		}

		[Test]
		public void FilterByMonth_Always_GivesRecordsFilteredByMonth()
		{
			var diagrams = Create();
			Record[] records =
			{
				new Record(0, 0, 0, "", Month(10)),
				new Record(0, 0, 0, "", Month(10)),
				new Record(0, 0, 0, "", Month(11)),
				new Record(0, 0, 0, "", Month(11)),
				new Record(0, 0, 0, "", Month(12)),
				new Record(0, 0, 0, "", Month(12)),
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
				new Record(100, 0, 0, "", date),
				new Record(100, 0, 0, "", date),
				new Record(100, 0, 0, "", date),
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
				new Record(0, 0, 0, "First", date),
				new Record(0, 0, 0, "Second", date),
				new Record(0, 0, 0, "Third", date),
			};
			var grouping = records.GroupBy(r => r.Category).First();

			var actual = diagrams.SquashRecords(grouping);

			Assert.That(actual.Description, Is.EqualTo("First\nSecond\nThird"));
		}

		[Test]
		public void GroupByDay_Always_ReturnsGroupedByDayCategoryData()
		{
			var diagrams = Create();
			Record[] records =
			{
				new Record(0, 0, Food,	"", Day(3)),
				new Record(0, 0, House, "", Day(3)),
				new Record(0, 0, Food,	"", Day(6)),
				new Record(0, 0, House, "", Day(6)),
				new Record(0, 0, Food,	"", Day(9)),
				new Record(0, 0, House, "", Day(9)),
			};

			var actual = diagrams.GroupByDay(records).ToList();

			Expect(actual.Select(p => p.Key), EquivalentTo(new[] { "3", "6", "9" }));
		}

		[Test]
		public void GroupByDay_WithTwoCategoriesPerDay_ReturnsDaysWithThisTwoCategories()
		{
			var diagrams = Create();
			Record[] records =
			{
				new Record(0, 0, Food,	"", Day(3)),
				new Record(0, 0, House, "", Day(3)),
				new Record(0, 0, Food,	"", Day(6)),
				new Record(0, 0, House, "", Day(6)),
				new Record(0, 0, Food,	"", Day(9)),
				new Record(0, 0, House, "", Day(9)),
			};

			var actual = diagrams.GroupByDay(records).ToList();

			var categories = actual.SelectMany(p => p.Value.Select(d => d.Category));
			Expect(categories, EquivalentTo(new[] { Food, House, Food, House, Food, House }));
		}

		[Test]
		public void GroupByDay_Always_ReturnsOrderedDays()
		{
			var diagrams = Create();
			Record[] records =
			{
				new Record(0, 0, 0, "", Day(6)),
				new Record(0, 0, 0, "", Day(3)),
				new Record(0, 0, 0, "", Day(6)),
				new Record(0, 0, 0, "", Day(9)),
				new Record(0, 0, 0, "", Day(3)),
				new Record(0, 0, 0, "", Day(9)),
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
				new Record(100, 0, 0, "", Month(10)),
				new Record(100, 0, 0, "", Month(10)),
				new Record(100, 0, 0, "", Month(10)),
				new Record(100, 0, 0, "", Month(11)),
				new Record(100, 0, 0, "", Month(11)),
				new Record(100, 0, 0, "", Month(11)),
				new Record(100, 0, 0, "", Month(12)),
				new Record(100, 0, 0, "", Month(12)),
				new Record(100, 0, 0, "", Month(12)),
			};

			var actual = diagrams.CalculateTotalByMonth(records);

			Expect(actual, Count.EqualTo(3));
			Expect(actual.Select(p => p.Key), EquivalentTo(new[] { 10, 11, 12 }));
			Expect(actual.Select(p => p.Value), All.EqualTo(300));
		}

		[Test]
		public void GroupByCategory_Always_ReturnsCategoriesWithTotalAmount()
		{
			var diagrams = Create();
			Record[] records =
			{
				new Record(100, 0, House, "", date),
				new Record(100, 0, House, "", date),
				new Record(100, 0, Deposit, "", date),
				new Record(100, 0, Deposit, "", date),
				new Record(100, 0, Health, "", date),
				new Record(100, 0, Health, "", date),
				new Record(100, 0, Food, "", date),
				new Record(100, 0, Food, "", date),
			};

			var actual = diagrams.GroupByCategory(records);

			Expect(actual.Select(p => p.Key), EquivalentTo(new[] {Food, Health, House, Deposit}));
			Expect(actual.Select(p => p.Value), All.EqualTo(200));
		}

		[Test]
		public void CalculateBalanceByMonth_Always_HasExpensesToIncomeRationGroupedByMonth()
		{
			var diagrams = Create();
			Record[] records =
			{
				new Record(100, Expense,0, "", Month(10)),
				new Record(100, Expense,0, "", Month(11)),
				new Record(100, Expense,0, "", Month(12)),
				new Record(100, Shared,	0, "", Month(10)),
				new Record(100, Shared,	0, "", Month(11)),
				new Record(100, Shared,	0, "", Month(12)),
				new Record(100, Income, 0, "", Month(11)),
				new Record(100, Income, 0, "", Month(10)),
				new Record(100, Income, 0, "", Month(12)),
			};
			var types = records.ToLookup(r => r.Type);

			var actual = diagrams.CalculateBalanceByMonth(types);

			Assert.That(actual.Select(p => p.Key), Is.EquivalentTo(new[] { Expense, Income }));
			Assert.That(actual[Expense].Select(p => p.Key), Is.EquivalentTo(new[] { 10, 11, 12 }));
			Assert.That(actual[Income].Select(p => p.Key),	Is.EquivalentTo(new[] { 10, 11, 12 }));
			Assert.That(actual[Expense].Select(p => p.Value),	All.EqualTo(200));
			Assert.That(actual[Income].Select(p => p.Value),	All.EqualTo(100));
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
