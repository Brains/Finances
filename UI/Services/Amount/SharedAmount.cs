using System;
using Common;
using UI.Views.Converters;

namespace UI.Services.Amount
{
    public class SharedAmount : Amount
    {
        private readonly ISettings settings;
        private decimal value;

        public SharedAmount(IAdder adder, ISettings settings) : base(adder)
        {
            this.settings = settings;
        }

        public override decimal Value
        {
            get { return value; }
            set { this.value = Math.Round(value/settings.Customers, settings.Precision); }
        }

        public override decimal Total => Value*settings.Customers;
    }
}