using System.Windows.Data;
using NSubstitute;
using NUnit.Framework;
using Records;
using UI.Interfaces;
using UI.Views.Converters;
using static NSubstitute.Substitute;

namespace UI.Tests.Views.Converters
{
	public class AmountConverterTests : AssertionHelper
	{
		[Test]
		public void Convert_ZeroAmount_ReturnsEmptyString()
		{
			var converter = new AmountConverter(null);

			var actual = converter.Convert(0m, null, null, null);

			Expect(actual, EqualTo(string.Empty));
		}

		[TestCase(10, "10")]
		[TestCase(20, "20")]
		[TestCase(1144, "1144")]
		public void Convert_NonZeroAmount_ReturnsItsString(decimal amount, string expected)
		{
			var converter = new AmountConverter(null);

			var actual = converter.Convert(amount, null, null, null);

			Expect(actual, EqualTo(expected));
		}

		[Test]
		public void ConvertBack_Always_ParsesDecimalFromString()
		{
			var adder = For<IAdder>();
			var converter = new AmountConverter(adder);
			adder.Convert("20").Returns(20m);

			var actual = converter.ConvertBack("20", null, null, null);

			Expect(actual, EqualTo(20m));
		}

		[Test]
		public void ConvertBack_Always_SummarizesDecimalsFromString()
		{
			var adder = For<IAdder>();
			var converter = new AmountConverter(adder);
			adder.Convert("20 + 10").Returns(30m);

			var actual = converter.ConvertBack("20 + 10", null, null, null);

			Expect(actual, EqualTo(30m));
		}

	}
}