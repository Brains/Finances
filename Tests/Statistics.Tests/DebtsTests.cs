using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NSubstitute;
using NUnit.Framework;
using Statistics.Banking;
using Statistics.ViewModels;
using Tracker;
using static Tracker.Record;
using static Tracker.Record.Types;
using static Tracker.Record.Categories;

namespace Statistics.Tests
{
	public class DebtsTests : AssertionHelper
	{
		private DateTime date = new DateTime(1, 1, 1);

		[Test]
		public void Get_Always_ReturnsTotalDebt()
		{
			var expenses = Substitute.For<IExpenses>();
			FillDebts(expenses);
			Debts debts = new Debts(expenses.Records);

			decimal actual = 0;
			debts.Get(number => actual = number);

			Expect(actual, EqualTo(600));
		}

		[Test]
		public void Calculate_Always_ReturnsTotalDebtForEachDude()
		{
			var expenses = Substitute.For<IExpenses>();
			FillDebts(expenses);
			Debts debts = new Debts(expenses.Records);

			var actual = debts.Calculate();

			var expected = new Dictionary<Categories, int>();
			expected[Maxim] = 500;
			expected[Andrey] = 100;
			Expect(actual, EquivalentTo(expected));
		}

		private void FillDebts(IExpenses expenses)
		{
			expenses.Records = new ObservableCollection<Record>
			{
				new Record(500, Debt, Maxim, "Out", date),
				new Record(300, Debt, Andrey, "Out", date),
				new Record(100, Debt, Maxim, "In", date),
				new Record(100, Debt, Maxim, "Out", date),
				new Record(200, Debt, Andrey, "In", date),
				new Record(250, Debt, Maxim, "Out", date),
				new Record(250, Debt, Maxim, "In", date)
			};
		}
	}
}