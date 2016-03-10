using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UI.Interfaces;
using UI.Services;

namespace UI.Tests.Services
{
	public class SubtractorTests
	{
		[Test]
		public void ChangedPrimaryFormAmount_WithSingleForm_DoesNotCallSubtract()
		{
			Subtractor subtractor = new Subtractor();
			IForm primary = Substitute.For<IForm>();
			subtractor.Add(primary);

			primary.Amount = "5";

			primary.DidNotReceive().Subtract(Arg.Any<IEnumerable<IForm>>());
		} 
		[Test]
		public void ChangedPrimaryFormAmount_WithTwoForms_DoesNotCallSubtract()
		{
			Subtractor subtractor = new Subtractor();
			IForm primary = Substitute.For<IForm>();
			subtractor.Add(primary);
			subtractor.Add(Substitute.For<IForm>());

			primary.Amount = "5";

			primary.DidNotReceive().Subtract(Arg.Any<IEnumerable<IForm>>());
		} 

		[Test]
		public void ChangedPrimaryFormAmount_Never_CallsSubtract()
		{
			Subtractor subtractor = new Subtractor();
			IForm first = Substitute.For<IForm>();
			IForm second = Substitute.For<IForm>();
			subtractor.Add(first);
			subtractor.Add(second);

			first.Amount = "5";

			first.DidNotReceive().Subtract(Arg.Any<IEnumerable<IForm>>());
		} 
	}
}