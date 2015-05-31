using System.IO;
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

			return Regex.Replace(file.ToString(SaveOptions.DisableFormatting), @"[^\x20-\x7e]", string.Empty);
		}

		public static string Format(string xml, string password)
		{
			var data = ExtractData(xml);
			var signature = Encryption.CalculateSignature(data + password);
			var file = InsertSignature(xml, signature);

			return file.ToString(SaveOptions.DisableFormatting);
		}

		private static XElement InsertSignature(string xml, string signature)
		{
			XElement file = XElement.Parse(xml);
			var signatureElement = file.Element("merchant").Element("signature");
			signatureElement.SetValue(signature);

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

		public static XmlDocument LoadXML(byte[] input)
		{
			XmlDocument doc = new XmlDocument();
			MemoryStream ms = new MemoryStream(input);
			doc.Load(ms);

			return doc;
		}
	}
}