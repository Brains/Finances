using NSubstitute;
using NUnit.Framework;
using Statistics.Banking;
using Statistics.ViewModels;
using Tracker;
using static NSubstitute.Substitute;

namespace Statistics.Tests.ViewModels
{
	[TestFixture]
	public class FundsTests : AssertionHelper
	{
		private Funds Create()
		{
			var bank = For<IFundsStorage>();
			var debt = For<IFundsStorage>();

			return new Funds(bank, debt);
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
	}
}
