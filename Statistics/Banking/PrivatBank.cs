using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common;
using Finances.Properties;
using Microsoft.Practices.Prism.Mvvm;
using Statistics.Storages;
using Statistics.ViewModels;
using Visualization.Banking;

namespace Statistics.Banking
{
	public class PrivatBank : BindableBase, IFundsStorage, IRecordsProvider, IStorage<decimal>
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

		public decimal Value { get; set; }
		public string Name { get; set; }

		public PrivatBank()
		{
//			Name = name;

			SendRequest();
		}

		private async void SendRequest()
		{
			var file = PrepareData();
			var responce = await SendData(balanceUrl, file);
			Value = Parser.ParseBalance(responce);

			OnPropertyChanged("Value");
		}


	}
}
