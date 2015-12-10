namespace Funds.Bank
{
	public class CardSource : FundsSource
	{
		private readonly string url = "https://api.privatbank.ua/p24api/balance";

		public CardSource()
		{
			Name = "CardSource";
		}

		public sealed override void PullValue()
		{
			Value = 100;
		}

		
	}
}