using System;
using System.ComponentModel;
using Caliburn.Micro;
using Common;
using UI.Services;

namespace UI.ViewModels
{
    public class Fund : PropertyChangedBase, IFund
    {
        private readonly IFundsSource source;
        private decimal value;
        public ISaver Saver { get; set; }

        public Fund(IFundsSource source)
        {
            this.source = source;
            this.source.Update += value => Value = value;

            Name = source.GetType().Name;
        }

        public string Name { get; }
        public bool IsReadOnly => Saver == null;

        [Notify]
        public decimal Value
        {
            get { return value; }
            set
            {
                this.value = value;
                Saver?.Save(Name, value);
            }
        }

        public void PullValue() => source.PullValue();
    }

    public interface IFund : INotifyPropertyChanged
    {
        decimal Value { get; set; }
		void PullValue();
    }
}