using System;
using Common;

namespace Funds.Sources
{
    public class Cash : IFundsSource
    {
        private readonly ISettings settings;

        public Cash(ISettings settings)
        {
            this.settings = settings;
        }

        public event Action<decimal> Updated = delegate { };

        public void PullValue() => Updated(decimal.Parse(settings.Cash));
    }
}