using System;
using Common;

namespace Funds.Sources
{
    public class Cash : IFundsSource, ISaver
    {
        private readonly ISettings settings;
        private decimal cash;

        public Cash(ISettings settings)
        {
            this.settings = settings;
            cash = decimal.Parse(this.settings.Cash);
        }

        public event Action<decimal> Updated = delegate { };

        public void PullValue() => Updated(cash);

        public void Save(string name, decimal value)
        {
            if (name != "Cash") return;

            cash = value;
            settings.Save(name, value);
        }
    }
}