using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NSubstitute;
using NUnit.Framework;
using Statistics.Banking;
using Tracker;

namespace Statistics.Tests.Banking
{
	public class DebtsTests : AssertionHelper
	{
		private DateTime date = new DateTime(1, 1, 1);

		private void FillDebts(IExpenses expenses)
		{
			expenses.Records = new ObservableCollection<Record>
			{
				new Record(500, Record.Types.Debt, Record.Categories.Maxim, "Out", date),
				new Record(300, Record.Types.Debt, Record.Categories.Andrey, "Out", date),
				new Record(100, Record.Types.Debt, Record.Categories.Maxim, "In", date),
				new Record(100, Record.Types.Debt, Record.Categories.Maxim, "Out", date),
				new Record(200, Record.Types.Debt, Record.Categories.Andrey, "In", date),
				new Record(250, Record.Types.Debt, Record.Categories.Maxim, "Out", date),
				new Record(250, Record.Types.Debt, Record.Categories.Maxim, "In", date)
			};
		}

		[Test]
		public void Get_Always_ReturnsTotalDebt()
		{
			var expenses = Substitute.For<IExpenses>();
			FillDebts(expenses);
			IFundsStorage debts = new Debts(expenses);

			decimal actual = 0;
			debts.Get(number => actual = number);

			Expect(actual, EqualTo(600));
		}

		[Test]
		public void Calculate_Always_ReturnsTotalDebtForEachDude()
		{
			var expenses = Substitute.For<IExpenses>();
			FillDebts(expenses);
			Debts debts = new Debts(expenses);

			var actual = debts.Calculate();

			var expected = new Dictionary<Record.Categories, int>();
			expected[Record.Categories.Maxim] = 500;
			expected[Record.Categories.Andrey] = 100;
			Expect(actual, EquivalentTo(expected));
		}

		[Test]
		public void Calculate_WithoutOutcomeDebts_ReturnsResultBasedOnlyOntoIncomeRecords()
		{
			var expenses = Substitute.For<IExpenses>();
			expenses.Records = new ObservableCollection<Record>
			{
				new Record(100, Record.Types.Debt, Record.Categories.Maxim, "In", date),
				new Record(200, Record.Types.Debt, Record.Categories.Andrey, "In", date),
			};
			Debts debts = new Debts(expenses);

			var actual = debts.Calculate();

			var expected = new Dictionary<Record.Categories, int>
			{
				[Record.Categories.Maxim] = -100,
				[Record.Categories.Andrey] = -200
			};
			Expect(actual, EquivalentTo(expected));
		}

		[Test]
		public void Calculate_WithShareRecords_AddsThemToTotalDebtForEachDude()
		{
			var expenses = Substitute.For<IExpenses>();
			expenses.Records = new ObservableCollection<Record>
			{
				new Record(100, Record.Types.Shared, Record.Categories.House, "Test", date),
				new Record(100, Record.Types.Debt, Record.Categories.Maxim, "Out", date),
				new Record(100, Record.Types.Debt, Record.Categories.Andrey, "Out", date),
				new Record(50, Record.Types.Shared, Record.Categories.House, "Test", date),
			};
			Debts debts = new Debts(expenses);

			var actual = debts.Calculate();

			var expected = new Dictionary<Record.Categories, int>
			{
				[Record.Categories.Maxim] = 250,
				[Record.Categories.Andrey] = 250,
			};
			Expect(actual, EquivalentTo(expected));

		}
	}
}