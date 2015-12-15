using System.Windows.Media;
using NUnit.Framework;
using UI.Views.Converters;

namespace UI.Tests.Views.Converters
{
	public class DivergenceColorTests
	{
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(1.4774)]
		[TestCase(5)]
		[TestCase(8.23)]
		[TestCase(-8.23)]
		[TestCase(-5)]
		[TestCase(-1.4774)]
		[TestCase(-0)]
		public void Convert_AbsoluteValueBelowThreshold_ReturnsGrayBrush(decimal value)
		{
			DivergenceColor divergence = new DivergenceColor();

			var actual = (SolidColorBrush) divergence.Convert(value, null, null, null);

			Assert.That(actual.Color, Is.EqualTo(Colors.Gray));
        } 

		[TestCase(10)]
		[TestCase(11)]
		[TestCase(21.4774)]
		[TestCase(995)]
		[TestCase(78.23)]
		[TestCase(-68.23)]
		[TestCase(-15)]
		[TestCase(-10.4774)]
		[TestCase(-10)]
		public void Convert_AbsoluteValueAboveThreshold_ReturnsOrangeRedBrush(decimal value)
		{
			DivergenceColor divergence = new DivergenceColor();

			var actual = (SolidColorBrush) divergence.Convert(value, null, null, null);

			Assert.That(actual.Color, Is.EqualTo(Colors.OrangeRed));
        } 
	}
}