using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Tracker;

namespace Visualization.Banking
{
	public class PrivatBank
	{
		private readonly string historyUrl = "https://api.privatbank.ua/p24api/rest_fiz";
		private readonly string balanceUrl = "https://api.privatbank.ua/p24api/balance";

		public async void GetBalance(Action<decimal> callback)
		{
			var file = PrepareData();
			var responce = await SendData(balanceUrl, file);
			var result = Parser.ParseBalance(responce);

			callback(result);
		}

		public async void GetHistory(Action<IEnumerable<Record>> callback)
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
			var xml = XElement.Load(@"Request.xml");

			var file = XML.Repair(xml);
			file = XML.Format(file, XML.ReadPassword(@"Password.xml"));

			return file;
		}
	}
}
