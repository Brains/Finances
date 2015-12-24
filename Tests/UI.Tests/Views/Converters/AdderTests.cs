using System;
using NUnit.Framework;
using UI.Views.Converters;

namespace UI.Tests.Views.Converters
{
	public class AdderTests : AssertionHelper
	{
		[Test]
		public void Convert_Null_ThrowsArgumentNullException()
		{
			var adder = new Adder();

			Expect(() => adder.Convert(null), Throws.ArgumentNullException
			                                        .With.Message.Contains("Empty")
			                                        .And.Property("ParamName").Contains("amount"));
		}

		[Test]
		public void Convert_Empty_ThrowsArgumentNullException()
		{
			var adder = new Adder();

			Expect(() => adder.Convert(string.Empty), Throws.ArgumentNullException
			                                                .With.Message.Contains("Empty")
			                                                .And.Property("ParamName").Contains("amount"));
		}

		[Test]
		public void Convert_PositiveSimpleNumber_ConvertsItToDecimal()
		{
			var adder = new Adder();

			var actual = adder.Convert("100");

			Expect(actual, EqualTo(100m));
		}

		[Test]
		public void Convert_NegativeSimpleNumber_ThrowsArgumentException()
		{
			var adder = new Adder();

			Expect(() => adder.Convert("-100"), Throws.ArgumentException);
			Expect(() => adder.Convert("100-"), Throws.ArgumentException);
			Expect(() => adder.Convert("100-"), Throws.ArgumentException);
		}

		[Test]
		public void Convert_CorrectComplexNumber_ConvertsItToDecimalOfSum()
		{
			var adder = new Adder();

			var actual = adder.Convert("100 + 100");

			Expect(actual, EqualTo(200m));
		}

		[Test]
		public void Convert_CorrectComplexNumber_ThrowsNothing()
		{
			var adder = new Adder();

			Expect(() => adder.Convert("100+"), Throws.Nothing);
			Expect(() => adder.Convert("100 + "), Throws.Nothing);
			Expect(() => adder.Convert("100+ "), Throws.Nothing);
		}

		[Test]
		public void Convert_IncorrectComplexNumber_ThrowsFormatException()
		{
			var adder = new Adder();

			Expect(() => adder.Convert("-100-"), Throws.TypeOf<FormatException>());
			Expect(() => adder.Convert("awd"), Throws.TypeOf<FormatException>());
			Expect(() => adder.Convert("+@#$"), Throws.TypeOf<FormatException>());
		}

		[Test]
		public void Convert_NegativeComplexNumber_ThrowsArgumentException()
		{
			var adder = new Adder();

			Expect(() => adder.Convert("-100"), Throws.ArgumentException
			                                          .With.Message.Contains("Negative")
			                                          .And.Property("ParamName").Contains("sum"));

			Expect(() => adder.Convert("100-"), Throws.ArgumentException
			                                          .With.Message.Contains("Negative")
			                                          .And.Property("ParamName").Contains("sum"));

			Expect(() => adder.Convert("-100 + 20"), Throws.ArgumentException
			                                               .With.Message.Contains("Negative")
			                                               .And.Property("ParamName").Contains("sum"));

			Expect(() => adder.Convert("-100 + -20"), Throws.ArgumentException
			                                                .With.Message.Contains("Negative")
			                                                .And.Property("ParamName").Contains("sum"));
		}
	}
}