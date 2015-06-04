using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NUnit.Framework;
using Tracker.Views.Converters;

namespace Tracker.UnitTests
{
	[TestFixture]
	class ConvertersTests : AssertionHelper
	{
		[TestCase(-10, "Thin")]
		[TestCase(0, "Thin")]
		[TestCase(10, "Thin")]
		[TestCase(99, "Thin")]
		[TestCase(101, "Thin")]
		[TestCase(155, "Thin")]
		[TestCase(200, "Thin")]
		[TestCase(210, "Thin")]
		[TestCase(310, "Thin")]
		[TestCase(400, "Light")]
		[TestCase(500, "Light")]
		[TestCase(600, "Light")]
		[TestCase(700, "Light")]
		[TestCase(800, "Normal")]
		[TestCase(1100, "Normal")]
		[TestCase(1200, "Bold")]
		[TestCase(1500, "Bold")]
		public void ValueToFontWeight_Always_ReturnsRightFontWeights (decimal value, string expected)
		{
			ValueToFontWeight converter = new ValueToFontWeight();

			var actual = converter.Convert(value, null, null, null);

			Expect(actual.ToString(), EqualTo(expected));
		}

		[TestCase(0, "")]
		[TestCase(74, "74")]
		[TestCase(-247, "-247")]
		public void AmountTextToDecimal_Convert_DecimalToTextCorrectly(decimal value, string expected)
		{
			AmountTextToDecimal converter = new AmountTextToDecimal();

			var actual = converter.Convert(value, null, null, null);

			Expect(actual, EqualTo(expected));
		}

		[TestCase("", 0)]
		[TestCase(null, 0)]
		[TestCase("13", 13)]
		[TestCase("-247", -247)]
		[TestCase("15+25+200", 240)]
		[TestCase("15 + 25 + 200", 240)]
		public void AmountTextToDecimal_ConvertBack_TextToDecimalCorrectly(string value, int expected)
		{
			AmountTextToDecimal converter = new AmountTextToDecimal();

			var actual = converter.ConvertBack(value, null, null, null);

			Expect(actual, EqualTo(expected));

		}

	}
}
