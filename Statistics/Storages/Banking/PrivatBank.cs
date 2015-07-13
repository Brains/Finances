using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common;
using Microsoft.Practices.Prism.Mvvm;

namespace Statistics.Storages.Banking
{
	public class PrivatBank : BindableBase, IRecordsProvider, IStorage<int>
	{
		private readonly string historyUrl = "https://api.privatbank.ua/p24api/rest_fiz";
		private readonly string balanceUrl = "https://api.privatbank.ua/p24api/balance";

		public int Value { get; set; }

		public PrivatBank()
		{
			SendRequest();
		}

		private async void SendRequest()
		{
			var file = PrepareData();
			var responce = await SendData(balanceUrl, file);
			Value = (int) Parser.ParseBalance(responce);

			OnPropertyChanged("Value");
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
			var settings = ConfigurationManager.AppSettings;

			return new PersonalData(settings["ID"], settings["Password"], settings["CardNumber"]);
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
