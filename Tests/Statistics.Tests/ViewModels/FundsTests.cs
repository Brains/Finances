using System;
using System.Collections.ObjectModel;
using NSubstitute;
using NUnit.Framework;
using Statistics.Banking;
using Statistics.ViewModels;
using Tracker;
using static NSubstitute.Substitute;
using static Tracker.Record;
using static Tracker.Record.Types;
using static Tracker.Record.Categories;

namespace Statistics.Tests.ViewModels
{
	[TestFixture]
	public class FundsTests : AssertionHelper
	{
		private Funds Create(IExpenses expences = null)
		{
			var bank = For<IFundsStorage>();
			var debt = For<IFundsStorage>();

			return new Funds(expences, bank, debt);
		}

		[Test]
		public void SettterOfAnyProperty_Always_ChangesTotal()
		{
			var funds = Create();

			funds.Cash = 100;
			Expect(funds.Total, EqualTo(100));

			funds.Debts = 100;
			Expect(funds.Total, EqualTo(200));

			funds.Cards = 100;
			Expect(funds.Total, EqualTo(300));
		}

		[Test]
		public void SettterOfUpwork_Always_ChangesTotalAccordinglyToExchangeRate()
		{
			var funds = Create();

            funds.Upwork = 100;

			Expect(funds.Total, EqualTo(100 * Funds.ExchangeRate));
		}

		private DateTime date = new DateTime(1, 1, 1);

		[Test]
		public void CalculateEstimatedBalance_Always_ReturnsItConsidersDifferentRecordTypes()
		{
			var expences = For<IExpenses>();
			var funds = Create(expences);
			expences.Records = new ObservableCollection<Record>
			{
				new Record(1000, Balance, General, "Balance", date),
				new Record(100, Expense, Food, "Expense", date),
				new Record(100, Shared, House, "Shared", date),
				new Record(100, Income, Deposit, "Income", date)
			};

			var actual = funds.CalculateEstimatedBalance();

			Expect(actual, EqualTo(900));
		}
	}
}
