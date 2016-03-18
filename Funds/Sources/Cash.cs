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

            Update += value => settings.Save("Cash", value);
        }

        public event Action<decimal> Update = delegate { };

        public void PullValue()
        {
            Update(decimal.Parse(settings.Cash));
        }
    }
}