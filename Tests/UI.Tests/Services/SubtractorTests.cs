using System.ComponentModel;
using NSubstitute;
using NSubstitute.Core.Events;
using NUnit.Framework;
using UI.Interfaces;
using UI.Services;
using static NSubstitute.Arg;
using static NSubstitute.Substitute;

namespace UI.Tests.Services
{
	public class SubtractorTests
	{
		private Subtractor Create()
		{
			return new Subtractor();
		}

		private DelegateEventWrapper<PropertyChangedEventHandler> Raise(string propertyName)
		{
			return NSubstitute.Raise.Event<PropertyChangedEventHandler>(new PropertyChangedEventArgs(propertyName));
		}

		[Test]
		public void PropertyChanged_PrimaryNotAmount_DoesNotCallPrimarySubtract()
		{
			Subtractor subtractor = Create();
			IForm primary = For<IForm>();
			subtractor.Add(primary);

			primary.PropertyChanged += Raise("NotAmount");

			primary.DidNotReceive().Subtract(Any<IForm>());
		}

		[Test]
		public void PropertyChanged_SecondaryNotAmount_DoesNotCallPrimarySubtract()
		{
			Subtractor subtractor = Create();
			IForm primary = For<IForm>();
			IForm secondary = For<IForm>();
			subtractor.Add(primary);
			subtractor.Add(secondary);

			secondary.PropertyChanged += Raise("NotAmount");

			primary.DidNotReceive().Subtract(Any<IForm>());
		}

		[Test]
		public void PropertyChanged_PrimaryAmount_DoesNotCallPrimarySubtract()
		{
			Subtractor subtractor = Create();
			IForm primary = For<IForm>();
			subtractor.Add(primary);

			primary.PropertyChanged += Raise("Amount");

			primary.DidNotReceive().Subtract(Any<IForm>());
		}

		[Test]
		public void PropertyChanged_SecondaryAmount_CallsPrimarySubtract()
		{
			Subtractor subtractor = Create();
			IForm primary = For<IForm>();
			IForm secondary = For<IForm>();
			subtractor.Add(primary);
			subtractor.Add(secondary);

			secondary.PropertyChanged += Raise("Amount");

			primary.Received().Subtract(secondary);
		}
	}
}