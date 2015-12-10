using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Funds.Bank
{
	public class CardSource : FundsSource
	{
		private readonly IRequestBuilder builder;
		private readonly IResponceParser parser;
		private const string Url = "https://api.privatbank.ua/p24api/balance";

		public CardSource(IRequestBuilder builder, IResponceParser parser)
		{
			this.builder = builder;
			this.parser = parser;

			Name = "CardSource";
		}

		public sealed override async void PullValue()
		{
			var client = new WebClient {Encoding = Encoding.UTF8};

			var request = builder.Build();
			var responce = await client.UploadStringTaskAsync(Url, request);

			Value = parser.ParseBalance(responce);
		}
	}
}