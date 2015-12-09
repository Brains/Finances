using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Common;

namespace Funds
{
	public class PrivatBank : INotifyPropertyChanged, IFundsSource
	{
		private readonly string url = "https://api.privatbank.ua/p24api/balance";
		private decimal value;

		public PrivatBank()
		{
			UpdateValue();
		}

		public string Name { get; set; } = "PrivatBank";

		public decimal Value
		{
			get { return value; }
			set { this.value = value; OnPropertyChanged();}
		}

		private async void UpdateValue()
		{
			await Task.Delay(5000);

			Value = 5;
		}

		private const string Request =
			@"<?xml version=""1.0"" encoding=""UTF-8""?>
			<request version=""1.0"">
				<merchant>
					<id>NULL</id>
					<signature>NULL</signature>
				</merchant>
				<data>
					<oper>cmt</oper>
					<wait>0</wait>
					<test>0</test>
					<payment id="""">
						<prop name=""card"" value=""NULL"" />
						<prop name=""country"" value=""UA"" />
						<prop name=""sd"" value=""NULL"" />
						<prop name=""ed"" value=""NULL"" />
					</payment>
				</data>
			</request>";

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}