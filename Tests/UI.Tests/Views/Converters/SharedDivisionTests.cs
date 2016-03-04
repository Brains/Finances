using System.Windows.Data;
using NSubstitute;
using NUnit.Framework;
using Common;
using UI.Interfaces;
using static NSubstitute.Substitute;

namespace UI.Tests.Views.Converters
{
	public class SharedDivisionTests : AssertionHelper
	{
		[Test]
		public void ConvertBack_Shared_DividesAmountIntoThree()
		{
			var decorated = For<IValueConverter>();
			var converter = new SharedDivision(decorated);
			var form = For<IForm>();
			form.SelectedType = Record.Types.Shared;
			decorated.ConvertBack("20 + 10", null, form, null).Returns(30m);

			var actual = (decimal) converter.ConvertBack("20 + 10", null, form, null);

			Expect(actual, EqualTo(10));
		}

		[TestCase(Record.Types.Expense)]
		[TestCase(Record.Types.Debt)]
		[TestCase(Record.Types.Income)]
		public void ConvertBack_NotShared_DoesNotDivideAmountIntoThree(Record.Types type)
		{
			var decorated = For<IValueConverter>();
			var converter = new SharedDivision(decorated);
			var form = For<IForm>();
			form.SelectedType = type;
			decorated.ConvertBack("5 + 5", null, form, null).Returns(10m);

			var actual = (decimal) converter.ConvertBack("5 + 5", null, form, null);

			Expect(actual, EqualTo(10));
		}
	}
}