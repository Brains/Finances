using System;
using System.Collections.Generic;
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

		public static decimal ParseBalance(string input)
		{
			XElement file = XElement.Parse(input);

			var element = file.Element("data").Element("balance").Value;

			return decimal.Parse(element);
		}


		public static IEnumerable<Tuple<string, string, string>> ParseHistory(string input)
		{
			XElement file = XElement.Parse(input);
			var elements = file.Element("data").Element("info").Elements();

			var history = elements.Skip(1).Select(e => Tuple.Create(
				e.Attribute("amount").Value,
				e.Attribute("terminal").Value, 
				e.Attribute("trandate").Value));

			return history;
		}
	}
}