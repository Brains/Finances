using Funds.Bank;
using NUnit.Framework;

namespace Funds.Tests.Bank
{
	public class ResponceParserTests
	{
		[Test]
		public void ParseBalance_InvalidInput_ReturnsZero()
		{
			var parser = new ResponceParser();

			var actual = parser.ParseBalance("Invalid");

			Assert.That(actual, Is.EqualTo(0));
		}
	}
}