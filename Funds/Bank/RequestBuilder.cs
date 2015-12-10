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
			XElement file = XElement.Parse(xml);

			file = InsertSecuredData(file, secured);
			file = InsertDatesRange(file, DateTime.Now.AddDays(-5), DateTime.Now);
			file = InsertSignature(file, secured);

			return file.ToString(SaveOptions.DisableFormatting);
		}

		public XElement InsertSecuredData(XElement file, Data secured)
		{
			file.Descendants("id").Single().Value = secured.ID;
			file.Descendants("prop")
				.Single(e => e.Attribute("name").Value == "card")
				.SetAttributeValue("value", secured.Card);

			return file;
		}

		public XElement InsertDatesRange(XElement file, DateTime start, DateTime end)
		{
			var properties = file.Descendants("prop").Skip(2).ToList();
			properties.First().SetAttributeValue("value", start.ToString("dd.MM.yyyy"));
			properties.Last().SetAttributeValue("value", end.ToString("dd.MM.yyyy"));

			return file;
		}

		public XElement InsertSignature(XElement file, Data personal)
		{
			var data = ExtractDataElement(file);
			var signature = encryption.CalculateSignature(data + personal.Password);

			file.Descendants("signature").Single().Value = signature;

			return file;
		}

		private string ExtractDataElement(XElement file)
		{
			var data = new StringBuilder();

			foreach (var element in file.Element("data").Nodes())
				data.Append(element.ToString(SaveOptions.DisableFormatting));

			return data.ToString();
		}

		public string Repair(XElement file)
		{
			var pattern = @"[^\x20-\x7e]";
			return Regex.Replace(file.ToString(SaveOptions.DisableFormatting), pattern, string.Empty);
		}
	}
}