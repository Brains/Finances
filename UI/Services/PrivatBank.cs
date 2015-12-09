using System.Threading.Tasks;
using Caliburn.Micro;

namespace UI.Services
{
	public class PrivatBank : PropertyChangedBase, IFundsSource
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
			set
			{
				this.value = value;
				NotifyOfPropertyChange(nameof(Value));
			}
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
	}
}