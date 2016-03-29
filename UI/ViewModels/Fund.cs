using System;
using System.ComponentModel;
using Caliburn.Micro;
using Common;
using UI.Services;
using UI.Views.Converters;
using Action = System.Action;

namespace UI.ViewModels
{
    public class Fund : PropertyChangedBase, IFund
    {
        private readonly IFundsSource source;
        private decimal value;

        public Fund(IFundsSource source)
        {
            this.source = source;
            this.source.Updated += value => Value = value;

            Name = source.GetType().Name;
        }

        public event Action Updated = delegate {};

        public decimal Value
        {
            get { return value; }
            set
            {
                if (value == this.value) return;
                this.value = value;
                Updated();
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(Text));
            }
        }

        public string Text
        {
            get { return Value.ToString("N0"); }
            set { SetValue(value); }
        }
        public string Name { get; }
        public bool IsReadOnly => Saver == null;
        public bool IsHitTestVisible => Saver != null;
        public ISaver Saver { get; set; }
        public IAdder Adder { get; set; }

        private void SetValue(string text)
        {
            Value = Adder?.Convert(text) ?? decimal.Parse(text);

            Saver?.Save(Name, Value);
        }

        public void PullValue() => source.PullValue();
    }

    public interface IFund : INotifyPropertyChanged
    {
        decimal Value { get; set; }
		void PullValue();
        event Action Updated;
    }
}