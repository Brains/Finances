using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Tracker;

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

		public static string InsertDatesRange(string xml, DateTime start, DateTime end)
		{
			XElement file = XElement.Parse(xml);

			var properties = file.Descendants("prop").Skip(2).ToList();
			properties.First().SetAttributeValue("value", start.ToString("dd.MM.yyyy"));
			properties.Last().SetAttributeValue("value", end.ToString("dd.MM.yyyy"));

			return file.ToString(SaveOptions.DisableFormatting); 
		}

		public static string ReadPassword(string input)
		{
			var path = Path.Combine("C:\\", "Projects", "Finances", "Data", input);
			XElement file = XElement.Load(path);

			return file.Value;
		}
	}
}