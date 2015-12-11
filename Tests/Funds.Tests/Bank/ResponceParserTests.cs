using Funds.Bank;
using NUnit.Framework;

namespace Funds.Tests.Bank
{
	public class ResponceParserTests
	{
		[Test]
		public void ParseBalance_EmptyInput_ReturnsZero()
		{
			var parser = new ResponceParser();

			var actual = parser.ParseBalance("<root/>");

			Assert.That(actual, Is.EqualTo(0));
		}

		[Test]
		public void ParseBalance_CorrectInput_ReturnsZero()
		{
			var parser = new ResponceParser();

			var actual = parser.ParseBalance("<root><av_balance>100</av_balance></root>");

			Assert.That(actual, Is.EqualTo(100));
		}
	}
}