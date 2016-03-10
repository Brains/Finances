using System.Collections.Generic;
using System.ComponentModel;
using NSubstitute;
using NUnit.Framework;
using UI.Interfaces;
using UI.Services;
using static NSubstitute.Arg;
using static NSubstitute.Substitute;

namespace UI.Tests.Services
{
	public class SubtractorTests
	{
		[Test]
		public void ChangedPrimaryFormAmount_WithSingleForm_DoesNotCallPrimarySubtract()
		{
			Subtractor subtractor = new Subtractor();
			IForm primary = For<IForm>();
			subtractor.Add(primary);

			primary.Amount = "5";

			primary.DidNotReceive().Subtract(Any<IEnumerable<IForm>>());
		} 
		[Test]
		public void ChangedPrimaryFormAmount_WithTwoForms_DoesNotCallPrimarySubtract()
		{
			Subtractor subtractor = new Subtractor();
			IForm primary = For<IForm>();
			subtractor.Add(primary);
			subtractor.Add(For<IForm>());

			primary.Amount = "5";

			primary.DidNotReceive().Subtract(Any<IEnumerable<IForm>>());
		} 

		[Test]
		public void ChangedSecondaryFormAmount_Always_CallsPrimarySubtract()
		{
			Subtractor subtractor = new Subtractor();
			IForm primary = For<IForm>();
			IForm secondary = For<IForm>();
			subtractor.Add(primary);
			subtractor.Add(secondary);

			secondary.PropertyChanged += Raise.Event<PropertyChangedEventHandler>(new PropertyChangedEventArgs("Test"));

			primary.Received(1).Subtract(new []{secondary});
		} 
	}
}