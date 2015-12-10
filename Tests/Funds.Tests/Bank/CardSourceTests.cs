using Funds.Bank;
using NUnit.Framework;

namespace Funds.Tests.Bank
{
	public class CardSourceTests
	{
		[Test]
		public void PullValue_Always_SetCorrectValue()
		{
			var source = new CardSource();

			source.PullValue();

			Assert.That(source.Value, Is.EqualTo(100));
		}
	}
}