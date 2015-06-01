using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Statistics.ViewModels
{
	public class Funds : INotifyPropertyChanged
	{
		private string creditCard;

		public Funds()
		{
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public string CreditCard
		{
			get { return creditCard; }
			set
			{
				creditCard = value;
				OnPropertyChanged();
			}
		}

		public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}