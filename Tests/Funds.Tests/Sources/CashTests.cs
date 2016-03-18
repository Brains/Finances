using Common;
using Funds.Sources;
using NSubstitute;
using NUnit.Framework;

namespace Funds.Tests.Sources
{
    public class CashTests
    {
        [Test]
        public void PullValue_Always_SavesValueToSettings()
        {
            ISettings settings = Substitute.For<ISettings>();
            settings.Cash = "5";
            var cash = new Cash(settings);

            cash.PullValue();

            settings.Received().Save("Cash", 5);
        }
    }
}