using System.Collections.Generic;
using System.ComponentModel;
using NSubstitute;
using NUnit.Framework;
using UI.Interfaces;
using UI.Services;
using static NSubstitute.Arg;
using static NSubstitute.Substitute;
using Handler = System.ComponentModel.PropertyChangedEventHandler;
using Args = System.ComponentModel.PropertyChangedEventArgs;

namespace UI.Tests.Services
{
	public class SubtractorTests
	{
		[Test]
		public void PropertyChanged_PrimaryFormAmountWithSingleForm_DoesNotCallPrimarySubtract()
		{
			Subtractor subtractor = new Subtractor();
			IForm primary = For<IForm>();
			subtractor.Add(primary);

			primary.Amount = "5";
			primary.PropertyChanged += Raise.Event<Handler>(new Args("Amount"));

			primary.DidNotReceive().Subtract(Any<IForm>());
		} 
		[Test]
		public void PropertyChanged_PrimaryFormAmountWithTwoForms_DoesNotCallPrimarySubtract()
		{
			Subtractor subtractor = new Subtractor();
			IForm primary = For<IForm>();
			subtractor.Add(primary);
			subtractor.Add(For<IForm>());

			primary.Amount = "5";
			primary.PropertyChanged += Raise.Event<Handler>(new Args("Amount"));

			primary.DidNotReceive().Subtract(Any<IForm>());
		}

		[Test]
		public void PropertyChanged_NotAmount_DoesNotCallPrimarySubtract()
		{
			Subtractor subtractor = new Subtractor();
			IForm primary = For<IForm>();
			IForm secondary = For<IForm>();
			subtractor.Add(primary);
			subtractor.Add(secondary);

			secondary.PropertyChanged += Raise.Event<Handler>(new Args("NotAmount"));

			primary.DidNotReceive().Subtract(Any<IForm>());
		}

		[Test]
		public void PropertyChanged_SecondaryFormAmount_CallsPrimarySubtract()
		{
			Subtractor subtractor = new Subtractor();
			IForm primary = For<IForm>();
			IForm secondary = For<IForm>();
			subtractor.Add(primary);
			subtractor.Add(secondary);

			secondary.PropertyChanged += Raise.Event<Handler>(new Args("Amount"));

			primary.Received(1).Subtract(secondary);
		}
	}
}