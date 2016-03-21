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

        private IForm AddForm(Subtractor subtractor)
        {
            var form = For<IForm>();
//            form.Amount = 5;
            subtractor.Add(form);

            return form;
        }

        private DelegateEventWrapper<PropertyChangedEventHandler> Raise(string propertyName)
        {
            return NSubstitute.Raise.Event<PropertyChangedEventHandler>(new PropertyChangedEventArgs(propertyName));
        }

        [Test]
        public void PropertyChanged_PrimaryNotAmount_DoesNotCallSubtract()
        {
            var subtractor = Create();
            var primary = AddForm(subtractor);

            primary.PropertyChanged += Raise("NotAmount");

            primary.DidNotReceive().Subtract(Any<decimal>());
        }

        [Test]
        public void PropertyChanged_SecondaryNotAmount_DoesNotCallSubtract()
        {
            var subtractor = Create();
            var primary = AddForm(subtractor);
            var secondary = AddForm(subtractor);

            secondary.PropertyChanged += Raise("NotAmount");

            primary.DidNotReceive().Subtract(Any<decimal>());
        }

        [Test]
        public void PropertyChanged_PrimaryAmount_DoesNotCallSubtract()
        {
            var subtractor = Create();
            var primary = AddForm(subtractor);

            primary.PropertyChanged += Raise("Amount");

            primary.DidNotReceive().Subtract(Any<decimal>());
        }

        [Test]
        public void PropertyChanged_SecondaryAmount_CallsSubtract()
        {
            var subtractor = Create();
            var primary = AddForm(subtractor);
            var secondary = AddForm(subtractor);

            secondary.PropertyChanged += Raise("Amount");

            primary.Received().Subtract(5);
        }

        [Test]
        public void PropertyChanged_SecondaryAmountTwoTimes_CallsSubtractOnlyOnce()
        {
            var subtractor = Create();
            var primary = AddForm(subtractor);
            var secondary = AddForm(subtractor);

            secondary.PropertyChanged += Raise("Amount");
            secondary.PropertyChanged += Raise("Amount");

            primary.Received(1).Subtract(5);
        }

        [Test]
        public void Add_FormFirstTime_MakesItPrimary()
        {
            var subtractor = Create();

            var primary = AddForm(subtractor);

            Expect(subtractor.Primary, EqualTo(primary));
        }

        [Test]
        public void Add_FormSecondTime_DoesNotMakeItPrimary()
        {
            var subtractor = Create();
            AddForm(subtractor);
            var secondary = AddForm(subtractor);

            Expect(subtractor.Primary, Not.EqualTo(secondary));
        }

        [Test]
        public void Remove_PrimaryForm_SetsPrimaryToNull()
        {
            var subtractor = Create();
            var primary = AddForm(subtractor);

            subtractor.Remove(primary);

            Expect(subtractor.Primary, Null);
        }

        [Test]
        public void Remove_NotPrimaryForm_DoesNotSetPrimaryToNull()
        {
            var subtractor = Create();
            subtractor.Add(For<IForm>());

            subtractor.Remove(For<IForm>());

            Expect(subtractor.Primary, Not.Null);
        }
    }
}