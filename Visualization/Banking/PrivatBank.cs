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

		private string GetBalance()
		{
			var xml = XElement.Load(@"Request.xml");
			string file;
			file = XML.Repair(xml);
			file = XML.Format(file, password);

			WebClient client = new WebClient();
			client.Encoding = Encoding.UTF8;

			return client.UploadString(historyUrl, file);
		}
	}
}
