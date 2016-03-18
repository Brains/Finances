using System.ComponentModel;
using Caliburn.Micro;
using Common;
using UI.Services;

namespace UI.ViewModels
{
    public class Fund : PropertyChangedBase, IFund
    {
        private readonly IFundsSource source;

        public Fund(IFundsSource source, bool isEditable)
        {
            this.source = source;
            this.source.Update += value => Value = value;

            Name = source.GetType().Name;
            IsEditable = isEditable;
        }

        public string Name { get; }
        public bool IsEditable { get; }
        [Notify] public decimal Value { get; set; }

        public void PullValue() => source.PullValue();
    }

    public interface IFund : INotifyPropertyChanged
    {
        decimal Value { get; set; }
		void PullValue();
    }
}