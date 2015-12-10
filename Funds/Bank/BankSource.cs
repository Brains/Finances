namespace Funds.Bank
{
	public class BankSource : FundsSource
	{
		private readonly string url = "https://api.privatbank.ua/p24api/balance";

		public BankSource()
		{
			Name = "BankSource";
		}

		public sealed override void PullValue()
		{
			Value = 100;
		}

	}
}