using System;
using System.Net;
using System.Text;
using Common;
using Funds.Bank;

namespace Funds.Sources
{
    public class Card : IFundsSource
    {
        private const string Url = "https://api.privatbank.ua/p24api/balance";
        private readonly IRequestBuilder builder;
        private readonly IResponceParser parser;

        public Card(IRequestBuilder builder, IResponceParser parser)
        {
            this.builder = builder;
            this.parser = parser;
        }

        public event Action<decimal> Update = delegate { };

        public async void PullValue()
        {
            var client = new WebClient {Encoding = Encoding.UTF8};

            var request = builder.Build();
            var responce = await client.UploadStringTaskAsync(Url, request);

            Update(parser.ParseBalance(responce));
        }
    }
}