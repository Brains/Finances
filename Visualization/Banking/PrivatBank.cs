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

		private readonly XML xml = new XML();

		private string SendString()
		{
			string xml = this.xml.Fix("Request.xml");

			xml = this.xml.Format(xml, password);

			WebClient client = new WebClient();
			client.Encoding = Encoding.UTF8;

			return client.UploadString(historyUrl, xml);
		}
	}
}
