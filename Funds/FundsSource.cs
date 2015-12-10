using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Common;

namespace Funds
{
	public abstract class FundsSource : IFundsSource, INotifyPropertyChanged
	{
		private decimal value;

		public virtual decimal Value
		{
			get { return value; }
			set { this.value = value; OnPropertyChanged();}
		}

		public string Name { get; protected set; }

		public abstract void PullValue() ;

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}