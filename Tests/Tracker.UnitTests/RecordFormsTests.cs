using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Tracker.ViewModels;
using static Tracker.Record;
using static Tracker.Record.Types;
using static Tracker.Record.Categories;

namespace Tracker.UnitTests
{
	[TestFixture]
	class RecordFormTests : AssertionHelper
	{
		[Test]
		public void Submit_Shared_DividesAmountIntoThree()
		{
			var expenses = Substitute.For<IExpenses>();
            RecordForm model = new RecordForm(expenses) {Description = "Test" };
			model.Amount = 9;
			model.Type = Shared;

			model.Submit();

			expenses.Received().Add(3, Shared, Food, "Test", Arg.Any<DateTime>());
		}

		[Test]
		public void Submit_NotShared_DontDividesAmount()
		{
			var expenses = Substitute.For<IExpenses>();
			RecordForm model = new RecordForm(expenses) { Description = "Test" };
			model.Amount = 9;

			model.Submit();

			expenses.Received().Add(9, Expense, Food, "Test", Arg.Any<DateTime>());
		}

		[TestCase(10, 3)]
		[TestCase(11, 4)]
		public void Submit_Shared_RoundsCorrectlyAfterDividing(int shared, int individual)
		{
			var expenses = Substitute.For<IExpenses>();
			RecordForm model = new RecordForm(expenses) { Description = "Test" };
			model.Type = Shared;
			model.Amount = shared;

			model.Submit();

			expenses.Received().Add(individual, Shared, Food, "Test", DateTime.Now);
		}

		[TestCase(Expense, new[] {Food, General, Health, House, Other, Women })]
		[TestCase(Debt, new[] { Maxim, Andrey })]
		[TestCase(Income, new[] { ODesk, Deposit })]
		[TestCase(Shared, new[] { Food, House, General, Other })]
		[TestCase(Balance, new[] { Other })]
        public void SetType_Always_ChangeRecordCategories(Types type, Categories[] expected)
		{
			RecordForm model = new RecordForm(null);

			model.Type = type;

			Expect(model.AvailableCategories, EquivalentTo(expected));
		}
	}
}
