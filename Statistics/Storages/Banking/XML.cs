using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable InconsistentNaming

namespace Statistics.Storages.Banking
{
	public struct PersonalData
	{
		public string ID;
		public string Card;
		public string Password;

		public PersonalData(string id, string password, string card)
		{
			ID = id;
			Password = password;
			Card = card;
		}
	}


	static class XML
	{
		public static string Repair(XElement file)
		{
			var pattern = @"[^\x20-\x7e]";
			return Regex.Replace(file.ToString(SaveOptions.DisableFormatting), pattern, string.Empty);
		}

		public static string Format(string xml, PersonalData personal)
		{
			xml = InsertPersonalData(xml, personal);
			xml = InsertDatesRange(xml, DateTime.Now.AddDays(-5), DateTime.Now);
			var file = PutSignature(xml, personal);

			return file.ToString(SaveOptions.DisableFormatting);
		}

		private static string InsertPersonalData(string xml, PersonalData personal)
		{
			XElement file = XElement.Parse(xml);

			file.Element("merchant").SetElementValue("id", personal.ID);

			var property = file.Descendants("prop").Single(element => element.Attribute("name").Value == "card");
			property.SetAttributeValue("value", personal.Card);

			return file.ToString(SaveOptions.DisableFormatting);

		}

		private static XElement PutSignature(string xml, PersonalData personal)
		{
			var data = ExtractData(xml);
			var signature = Encryption.CalculateSignature(data + personal.Password);
			var file = InsertSignature(xml, signature);
			return file;
		}

		private static string ExtractData(string xml)
		{
			XElement file = XElement.Parse(xml);

			var data = new StringBuilder();

			foreach (var element in file.Element("data").Nodes())
				data.Append(element.ToString(SaveOptions.DisableFormatting));

			return data.ToString();
		}

		private static XElement InsertSignature(string xml, string signature)
		{
			XElement file = XElement.Parse(xml);
			var signatureElement = file.Element("merchant").Element("signature");
			signatureElement.SetValue(signature);

			return file;
		}

		public static string InsertDatesRange(string xml, DateTime start, DateTime end)
		{
			XElement file = XElement.Parse(xml);

			var properties = file.Descendants("prop").Skip(2).ToList();
			properties.First().SetAttributeValue("value", start.ToString("dd.MM.yyyy"));
			properties.Last().SetAttributeValue("value", end.ToString("dd.MM.yyyy"));

			return file.ToString(SaveOptions.DisableFormatting); 
		}
	}
}