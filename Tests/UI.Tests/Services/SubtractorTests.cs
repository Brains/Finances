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
	public class SubtractorTests : AssertionHelper
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
		public void PropertyChanged_PrimaryNotAmount_DoesNotCallSubtract()
		{
			Subtractor subtractor = Create();
			IForm primary = For<IForm>();
			subtractor.Add(primary);

			primary.PropertyChanged += Raise("NotAmount");

			primary.DidNotReceive().Subtract(Any<IForm>());
		}

		[Test]
		public void PropertyChanged_SecondaryNotAmount_DoesNotCallSubtract()
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
		public void PropertyChanged_PrimaryAmount_DoesNotCallSubtract()
		{
			Subtractor subtractor = Create();
			IForm primary = For<IForm>();
			subtractor.Add(primary);
			subtractor.Add(For<IForm>());

			primary.PropertyChanged += Raise("Amount");

			primary.DidNotReceive().Subtract(Any<IForm>());
		}

		[Test]
		public void PropertyChanged_SecondaryAmount_CallsSubtract()
		{
			Subtractor subtractor = Create();
			IForm primary = For<IForm>();
			IForm secondary = For<IForm>();
			subtractor.Add(primary);
			subtractor.Add(secondary);

			secondary.PropertyChanged += Raise("Amount");

			primary.Received().Subtract(secondary);
		}

		[Test]
		public void PropertyChanged_SecondaryAmountTwoTimes_CallsSubtractOnlyOnce()
		{
			Subtractor subtractor = Create();
			IForm primary = For<IForm>();
			IForm secondary = For<IForm>();
			subtractor.Add(primary);
			subtractor.Add(secondary);

			secondary.PropertyChanged += Raise("Amount");
			secondary.PropertyChanged += Raise("Amount");

			primary.Received(1).Subtract(secondary);
		}

		[Test]
		public void Add_FormFirstTime_MakesItPrimary()
		{
			Subtractor subtractor = Create();
			IForm primary = For<IForm>();

            subtractor.Add(primary);

		    Expect(subtractor.Primary, EqualTo(primary));
		}

		[Test]
		public void Add_FormSecondTime_DoesNotMakeItPrimary()
		{
			Subtractor subtractor = Create();
			IForm primary = For<IForm>();
            IForm secondary = For<IForm>();

            subtractor.Add(primary);
            subtractor.Add(secondary);

            Expect(subtractor.Primary, Not.EqualTo(secondary));
		}
	}
}