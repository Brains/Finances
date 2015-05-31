using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
// ReSharper disable PossibleNullReferenceException
// ReSharper disable InconsistentNaming

namespace Visualization.Banking
{
	static class XML
	{
		public static string Repair(XElement file)
		{
			var pattern = @"[^\x20-\x7e]";
			return Regex.Replace(file.ToString(SaveOptions.DisableFormatting), pattern, string.Empty);
		}

		public static string Format(string xml, string password)
		{
			xml = InsertDatesRange(xml, DateTime.Now.AddDays(-5), DateTime.Now);
			var data = ExtractData(xml);
			var signature = Encryption.CalculateSignature(data + password);
			var file = InsertSignature(xml, signature);

			return file.ToString(SaveOptions.DisableFormatting);
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

		public static string InsertDatesRange(string xml, DateTime startDate, DateTime endDate)
		{
			XElement file = XElement.Parse(xml);

//			XElement start = new XElement("prop");
//			start.SetAttributeValue("name", "sd");
//			start.SetAttributeValue("value", startDate.ToString("dd.MM.yyyy"));
//
//			XElement end = new XElement("prop");
//			end.SetAttributeValue("name", "ed");
//			end.SetAttributeValue("value", endDate.ToString("dd.MM.yyyy"));
//
//			file.Element("data").Element("payment").Add(start);
//			file.Element("data").Element("payment").Add(end);


			var props = file.Descendants("prop").Skip(2);
			props.First().SetAttributeValue("value", startDate.ToString("dd.MM.yyyy"));
			props.Last().SetAttributeValue("value", endDate.ToString("dd.MM.yyyy"));


			return file.ToString(SaveOptions.DisableFormatting); 
		}

		public static decimal ParseBalance(string input)
		{
			XElement file = XElement.Parse(input);

			var element = file.Descendants("balance").First().Value;

			return Math.Round(decimal.Parse(element));
		}

		public static IEnumerable<Tuple<decimal, string, DateTime>> ParseHistory(string input)
		{
			XElement file = XElement.Parse(input);
			var elements = file.Descendants("statement");

			var history = elements.Select(ParseStatement);

			return history;
		}

		private static Tuple<decimal, string, DateTime> ParseStatement(XElement e)
		{
			var amountText = e.Attribute("amount").Value.Replace("UAH", String.Empty);
			var amount = decimal.Parse(amountText);

			var time = TimeSpan.Parse(e.Attribute("trantime").Value);
			var date = DateTime.Parse(e.Attribute("trandate").Value);

			return Tuple.Create(amount, e.Attribute("terminal").Value, date.Add(time));
		}

		public static string ReadPassword(string input)
		{
			var path = Path.Combine("C:\\", "Projects", "Finances", "Data", input);
			XElement file = XElement.Load(path);

			return file.Value;
		}
	}
}