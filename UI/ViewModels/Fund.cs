using System.ComponentModel;
using Caliburn.Micro;
using Common;
using UI.Services;

namespace UI.ViewModels
{
    public class Fund : PropertyChangedBase, IFund
    {
        private readonly IFundsSource source;
        private readonly ISettings settings;
        private decimal value;

        public Fund(IFundsSource source, ISettings settings)
        {
            this.source = source;
            this.source.Update += value => Value = value;
            this.settings = settings;

            Name = source.GetType().Name;
        }

        public string Name { get; }
        public bool IsEditable { get; set; }

        [Notify]
        public decimal Value
        {
            get { return value; }
            set
            {
                settings.Save("Cash", value);
                this.value = value;
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