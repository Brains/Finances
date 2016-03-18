using System.ComponentModel;
using Caliburn.Micro;
using Common;
using UI.Services;

namespace UI.ViewModels
{
    public class Fund : PropertyChangedBase, IFund
    {
        private readonly IFundsSource source;

        public Fund(IFundsSource source)
        {
            this.source = source;
            this.source.Update += value => Value = value;
        }

        public string Name { get; set; }
        public bool IsEditable { get; set; }

        [Notify]
        public decimal Value { get; set; }

        public void PullValue() => source.PullValue();
    }

    public interface IFund : INotifyPropertyChanged
    {
        decimal Value { get; set; }
		void PullValue();
    }
}