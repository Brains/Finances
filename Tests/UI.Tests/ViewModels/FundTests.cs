using System;
using Common;
using NSubstitute;
using NUnit.Framework;
using UI.ViewModels;
using static NSubstitute.Substitute;

namespace UI.Tests.ViewModels
{
    public class FundTests
    {
        [Test]
        public void SourceUpdate_Always_RaisesUpdatedEvent()
        {
            var source = For<IFundsSource>();
            var fund = new Fund(source);
            var update = For<Action>();
            fund.Updated += update;

            source.Updated += Raise.Event<Action<decimal>>(100m);

            update.Received().Invoke();
        }

        [Test]
        public void TextSetter_Always_RaisesUpdatedEvent()
        {
            var fund = new Fund(For<IFundsSource>());
            var update = For<Action>();
            fund.Updated += update;

            fund.Text = "100";

            update.Received().Invoke();
        }
    }
}