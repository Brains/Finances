using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Visualization.Banking
{
	class PrivatBank
	{
		private readonly string historyUrl = "https://api.privatbank.ua/p24api/rest_fiz";
		private readonly string balanceUrl = "https://api.privatbank.ua/p24api/balance";
		private readonly string password = "";

		public decimal GetBalance()
		{
			var file = PrepareData();
			var responce = SendData(balanceUrl, file);
			var result = XML.ParseBalance(responce);

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
			string file;

			file = XML.Repair(xml);
			file = XML.Format(file, password);

			return file;
		}
	}
}
