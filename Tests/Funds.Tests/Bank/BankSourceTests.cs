using Funds.Bank;
using NUnit.Framework;

namespace Funds.Tests.Bank
{
	public class BankSourceTests
	{
		[Test]
		public void PullValue_Always_SetCorrectValue()
		{
			var source = new BankSource();

			source.PullValue();

			Assert.That(source.Value, Is.EqualTo(100));
		}
	}
}