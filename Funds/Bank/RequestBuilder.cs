using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Funds.Bank;
// ReSharper disable PossibleNullReferenceException

namespace Funds.Bank
{
	public struct Data
	{
		public readonly string ID;
		public readonly string Card;
		public readonly string Password;

		public Data(string id, string password, string card)
		{
			ID = id;
			Password = password;
			Card = card;
		}
	}

	public class RequestBuilder
	{
		private readonly IEncryption encryption;

		public RequestBuilder(IEncryption encryption)
		{
			this.encryption = encryption;
		}

		public string Build(string xml, Data secured)
		{
			xml = InsertSecuredData(xml, secured);
			xml = InsertDatesRange(xml, DateTime.Now.AddDays(-5), DateTime.Now);
			var file = InsertSignature(xml, secured);

			return file.ToString(SaveOptions.DisableFormatting);
		}

		public string InsertSecuredData(string xml, Data secured)
		{
			XElement file = XElement.Parse(xml);

			file.Element("merchant").SetElementValue("id", secured.ID);

			file.Descendants("prop")
				.Single(e => e.Attribute("name").Value == "card")
				.SetAttributeValue("value", secured.Card);

			return file.ToString(SaveOptions.DisableFormatting);
		}

		public XElement InsertSignature(string xml, Data personal)
		{
			var data = ExtractDataElement(xml);
			var signature = encryption.CalculateSignature(data + personal.Password);
			var file = InsertSignature(xml, signature);
			return file;
		}

		private XElement InsertSignature(string xml, string signature)
		{
			XElement file = XElement.Parse(xml);
			var signatureElement = file.Element("merchant").Element("signature");
			signatureElement.SetValue(signature);

			return file;
		}

		private string ExtractDataElement(string xml)
		{
			XElement file = XElement.Parse(xml);

			var data = new StringBuilder();

			foreach (var element in file.Element("data").Nodes())
				data.Append(element.ToString(SaveOptions.DisableFormatting));

			return data.ToString();
		}

		public string InsertDatesRange(string xml, DateTime start, DateTime end)
		{
			XElement file = XElement.Parse(xml);

			var properties = file.Descendants("prop").Skip(2).ToList();
			properties.First().SetAttributeValue("value", start.ToString("dd.MM.yyyy"));
			properties.Last().SetAttributeValue("value", end.ToString("dd.MM.yyyy"));

			return file.ToString(SaveOptions.DisableFormatting);
		}

		public string Repair(XElement file)
		{
			var pattern = @"[^\x20-\x7e]";
			return Regex.Replace(file.ToString(SaveOptions.DisableFormatting), pattern, string.Empty);
		}
	}
}