using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Visualization.Banking
{
	public class PrivatBank
	{
		private readonly string historyUrl = "https://api.privatbank.ua/p24api/rest_fiz";
		private readonly string balanceUrl = "https://api.privatbank.ua/p24api/balance";

		public decimal GetBalance()
		{
			var file = PrepareData();
			var responce = SendData(balanceUrl, file);
			var result = XML.ParseBalance(responce);

			return result;
		}

		public IEnumerable<Tuple<decimal, string, DateTime>> GetHistory()
		{
			var file = PrepareData();
			var responce = SendData(historyUrl, file);
			var result = XML.ParseHistory(responce);

			return result;
		}

		private string SendData(string url, string file)
		{
			WebClient client = new WebClient();
			client.Encoding = Encoding.UTF8;

			var responce = client.UploadString(url, file);

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
