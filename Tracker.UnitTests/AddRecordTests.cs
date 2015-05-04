using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Tracker.ViewModels;

namespace Tracker.UnitTests
{
	[TestFixture]
	class AddRecordTests : AssertionHelper
	{
		//------------------------------------------------------------------
		[Test]
		public void Submit_Shared_DividesAmountIntoThree()
		{
			var expenses = Substitute.For<IExpenses>();
            RecordForm model = new RecordForm(expenses) {Description = "Test" };
			model.Amount = "9";
			model.Type = Record.Types.Shared;

			model.Submit();

			expenses.Received().Add(3, Record.Types.Shared, Record.Categories.Food, "Test");
		}

		//------------------------------------------------------------------
		[Test]
		public void Submit_NotShared_DontDividesAmount()
		{
			var expenses = Substitute.For<IExpenses>();
			RecordForm model = new RecordForm(expenses) { Description = "Test" };
			model.Amount = "9";

			model.Submit();

			expenses.Received().Add(9, Record.Types.Expense, Record.Categories.Food, "Test");
		}

		//------------------------------------------------------------------
		[TestCase("10", 3)]
		[TestCase("11", 4)]
		public void Submit_Shared_RoundsCorrectlyAfterDividing(string shared, decimal individual)
		{
			var expenses = Substitute.For<IExpenses>();
			RecordForm model = new RecordForm(expenses) { Description = "Test" };
			model.Amount = shared;
			model.Type = Record.Types.Shared;

			model.Submit();

			expenses.Received().Add(individual, Record.Types.Shared, Record.Categories.Food, "Test");
		}
	}
}
