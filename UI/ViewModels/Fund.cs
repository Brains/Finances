using System;
using System.ComponentModel;
using Caliburn.Micro;
using Common;
using UI.Services;
using UI.Views.Converters;

namespace UI.ViewModels
{
    public class Fund : PropertyChangedBase, IFund
    {
        private readonly IFundsSource source;
        private decimal value;

        public Fund(IFundsSource source)
        {
            this.source = source;
            this.source.Update += value => Value = value;

            Name = source.GetType().Name;
        }

        public ISaver Saver { get; set; }
        public IAdder Adder { get; set; }
        public string Name { get; }
        public bool IsReadOnly => Saver == null;
        public bool IsHitTestVisible => Saver != null;

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