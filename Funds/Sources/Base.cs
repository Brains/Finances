using System;
using Common;

namespace Funds.Sources
{
    public abstract class Base : IFundsSource
    {
        private decimal value;

        public string Name { get; protected set; }
        public virtual decimal Value
        {
            get { return value; }
            set
            {
                this.value = value;
                Update();
            }
        }

        public event Action Update = delegate { };

        public abstract void PullValue();
    }
}