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
		[Test]
		public void SettterOfAnyProperty_Always_ChangesTotal()
		{
			var bank = For<IFundsStorage>();
			var debt = For<IFundsStorage>();
			Funds funds = new Funds(bank, debt);

			funds.Cash = 100;
			Expect(funds.Total, EqualTo(100));

			funds.Debts = 100;
			Expect(funds.Total, EqualTo(200));

			funds.CreditCard = 100;
			Expect(funds.Total, EqualTo(300));

			funds.Upwork = 100;
			Expect(funds.Total, EqualTo(400));

		}
	}
}
