using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tracker;
using Visualization.Banking;

namespace Statistics.Banking
{
	public class PrivatBank : IFundsStorage, IRecordsProvider
	{
		private readonly string historyUrl = "https://api.privatbank.ua/p24api/rest_fiz";
		private readonly string balanceUrl = "https://api.privatbank.ua/p24api/balance";

		public async void Get(Action<decimal> callback)
		{
			var file = PrepareData();
			var responce = await SendData(balanceUrl, file);
			var result = Parser.ParseBalance(responce);

			callback(result);
		}

		public async void Get(Action<IEnumerable<Record>> callback)
		{
			var file = PrepareData();
			var responce = await SendData(historyUrl, file);
			var result = Parser.ParseHistory(responce);

			callback(result);
		}

		private async Task<string> SendData(string url, string file)
		{
			WebClient client = new WebClient();
			client.Encoding = Encoding.UTF8;

			var responce = await client.UploadStringTaskAsync(url, file);

			return responce;
		}

		private string PrepareData()
		{
			var path = Path.Combine("C:\\", "Projects", "Finances", "Data", "Request.xml");
			var xml = XElement.Load(path);

			var file = XML.Repair(xml);
			file = XML.Format(file, XML.ReadPassword(@"Password.xml"));

			return file;
		}
	}
}
