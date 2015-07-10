using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Finances.Properties;
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
			var xml = XElement.Parse(Request);

			var file = XML.Repair(xml);
			file = XML.Format(file, ReadPersonalData());

			return file;
		}

		private PersonalData ReadPersonalData()
		{
			var settings = Settings.Default;

			return new PersonalData(settings.PrivatBankID, settings.PrivatBankPassword, settings.PrivatBankCardNumber);
		}

		private const string Request = 
			@"<?xml version=""1.0"" encoding=""UTF-8""?>
			<request version=""1.0"">
				<merchant>
					<id>NULL</id>
					<signature>NULL</signature>
				</merchant>
				<data>
					<oper>cmt</oper>
					<wait>0</wait>
					<test>0</test>
					<payment id="""">
						<prop name=""card"" value=""NULL"" />
						<prop name=""country"" value=""UA"" />
						<prop name=""sd"" value=""NULL"" />
						<prop name=""ed"" value=""NULL"" />
					</payment>
				</data>
			</request>";
	}
}
