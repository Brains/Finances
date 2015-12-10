using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Funds.Bank;
using MoreLinq;

// ReSharper disable PossibleNullReferenceException

namespace Funds.Bank
{
	public class RequestBuilder
	{
		private readonly IEncryption encryption;
		public string ID { get; set; }
		public string Password { get; set; }
		public string Card { get; set; }

		public RequestBuilder(IEncryption encryption)
		{
			this.encryption = encryption;

			Date = DateTime.Now;

			var settings = ConfigurationManager.AppSettings;
			ID = settings["id"];
			Password = settings["Password"];
			Card = settings["Card"];
		}

		public string Build(string xml)
		{
			XElement file = XElement.Parse(xml);

			InsertSecuredData(file);
			InsertDatesRange(file);
			InsertSignature(file);

			return file.ToString(SaveOptions.DisableFormatting);
		}

		public XElement InsertSecuredData(XElement file)
		{
			file.Descendants("id").Single().Value = ID;
			file.Descendants("prop")
				.Single(e => e.Attribute("name").Value == "card")
				.SetAttributeValue("value", Card);

			return file;
		}

		public XElement InsertDatesRange(XElement file)
		{
			var format = "dd.MM.yyyy";
			var properties = file.Descendants("prop").Skip(2).ToList();
			properties.First().SetAttributeValue("value", Date.AddDays(-5).ToString(format));
			properties.Last().SetAttributeValue("value", Date.ToString(format));

			return file;
		}

		public DateTime Date { get; set; }

		public XElement InsertSignature(XElement file)
		{
			var data = ExtractDataElement(file);

			var signature = encryption.CalculateSignature(data + Password);
			file.Descendants("signature").Single().Value = signature;

			return file;
		}

		public string ExtractDataElement(XElement file)
		{
			var data = new StringBuilder();

			file.Element("data").Nodes()
				.ForEach(node => data.Append(node.ToString(SaveOptions.DisableFormatting)));

			return data.ToString();
		}


		public string Repair(XElement file)
		{
			var pattern = @"[^\x20-\x7e]";
			return Regex.Replace(file.ToString(SaveOptions.DisableFormatting), pattern, string.Empty);
		}
	}
}