using System.ComponentModel;
using System.Runtime.CompilerServices;
using Statistics.Banking;

namespace Statistics.ViewModels
{
	public class Funds : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private int creditCard;
		private int debts;
		private int cash;
		private int upwork;

		public int CreditCard
		{
			get { return creditCard; }
			set { creditCard = value; OnPropertyChanged(); }
		}

		public int Debts
		{
			get { return debts; }
			set { debts = value; OnPropertyChanged(); }
		}

		public int Cash
		{
			get { return cash; }
			set { cash = value; OnPropertyChanged(); }
		}

		public int Upwork
		{
			get { return upwork; }
			set { upwork = value; OnPropertyChanged(); }
		}

		public int Total { get; set; }

		public Funds(IFundsStorage bank, IFundsStorage debt)
		{
			PropertyChanged += UpdateTotal;

			bank.Get(amount => CreditCard = (int) amount);
			debt.Get(amount => Debts = (int) amount);
		}

		private void UpdateTotal(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Total = Upwork + CreditCard + Cash + Debts;
		}

		public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}