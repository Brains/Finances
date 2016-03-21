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

        public Fund(IFundsSource source)
        {
            this.source = source;
            this.source.Update += value => Value = value;

            Name = source.GetType().Name;
        }

        [Notify] public decimal Value { get; set; }
        [Notify] public string Text
        {
            get { return Value.ToString("G"); }
            set { SetValue(value); }
        }
        public string Name { get; }
        public bool IsReadOnly => Saver == null;
        public bool IsHitTestVisible => Saver != null;
        public ISaver Saver { get; set; }
        public IAdder Adder { get; set; }

        private void SetValue(string value)
        {
            Value = Adder?.Convert(value) ?? decimal.Parse(value);

            Saver?.Save(Name, Value);
        }

        public void PullValue() => source.PullValue();
    }

    public interface IFund : INotifyPropertyChanged
    {
        decimal Value { get; set; }
		void PullValue();
    }
}