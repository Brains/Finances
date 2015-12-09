using System.ComponentModel;

namespace Funds
{
	public class BankSource : FundsSource
	{
		private readonly string url = "https://api.privatbank.ua/p24api/balance";

		public BankSource()
		{
			Name = "BankSource";

			PullValue();
		}

		public sealed override void PullValue()
		{
			Value = 100;

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