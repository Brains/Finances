using System.Windows.Media;
using Common;
using NUnit.Framework;
using UI.Views.Converters;

namespace UI.Tests.Views.Converters
{
	public class EnumToColorTests
	{
		private object Color(string expected)
		{
			return ColorConverter.ConvertFromString(expected);
		}

		[TestCase(Record.Types.Expense, "Chocolate")]
		[TestCase(Record.Types.Debt, "SteelBlue")]
		[TestCase(Record.Types.Shared, "#E0BB00")]
		[TestCase(Record.Types.Income, "#789E27")]
		public void Convert_RecordType_ReturnsCorrespondBrush(Record.Types type, string expected)
		{
			EnumToColor converter = new EnumToColor();

			var actual = (SolidColorBrush) converter.Convert(type, null, null, null);

			Assert.That(actual.Color, Is.EqualTo(Color(expected)));
		}

		[TestCase(null)]
		[TestCase(Record.Categories.House)]
		[TestCase("NoWay")]
		public void Convert_InvalidValueWithNullParameter_ReturnsBlackBrush(object value)
		{
			EnumToColor converter = new EnumToColor();

			var actual = (SolidColorBrush) converter.Convert(value, null, null, null);

			Assert.That(actual.Color, Is.EqualTo(Colors.Black));
		}

		[TestCase(null)]
		[TestCase(Record.Categories.House)]
		[TestCase("NoWay")]
		public void Convert_InvalidValueWithBrushInParameter_ReturnsTheBrush(object value)
		{
			EnumToColor converter = new EnumToColor();

			var actual = (SolidColorBrush) converter.Convert(value, null, Brushes.DodgerBlue, null);

			Assert.That(actual.Color, Is.EqualTo(Colors.DodgerBlue));
		}

		[Test]
		public void Convert_IntegerLargerThanEnum_ReturnsBlackBrush()
		{
			EnumToColor converter = new EnumToColor();

			var actual = (SolidColorBrush) converter.Convert(10, null, null, null);

			Assert.That(actual.Color, Is.EqualTo(Colors.Black));
		}
	}
}