using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Common;
using Funds.Bank;
using MoreLinq;

// ReSharper disable PossibleNullReferenceException

namespace Funds.Bank
{
	public interface IRequestBuilder
	{
		string Build();
	}

	public class RequestBuilder : IRequestBuilder
	{
		private readonly IEncryption encryption;
		private readonly ISettings settings;

		public RequestBuilder(IEncryption encryption, ISettings settings)
		{
			this.encryption = encryption;
			this.settings = settings;

			Date = DateTime.Now;
		}

		public string Build()
		{
			XElement file = XElement.Parse(settings.BankRequest);

			InsertSecuredData(file);
			InsertDatesRange(file);
			InsertSignature(file);

			return file.ToString(SaveOptions.DisableFormatting);
		}

		public XElement InsertSecuredData(XElement file)
		{
			file.Descendants("id").Single().Value = settings.ID;
			file.Descendants("prop")
				.Single(e => e.Attribute("name").Value == "card")
				.SetAttributeValue("value", settings.Card);

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

			var signature = encryption.CalculateSignature(data + settings.Password);
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