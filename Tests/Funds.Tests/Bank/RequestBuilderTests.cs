using System;
using System.Linq;
using System.Xml.Linq;
using Funds.Bank;
using NSubstitute;
using NUnit.Framework;
using static NSubstitute.Substitute;

namespace Funds.Tests.Bank
{
	public class RequestBuilderTests
	{
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

		private IEncryption encryption;

		private RequestBuilder Create()
		{
			encryption = For<IEncryption>();

			return new RequestBuilder(encryption);
		}

		[Test]
		public void InsertSecuredData_Always_InsertsSecuredDataIntoCorrespondingElements()
		{
			var builder = Create();
			builder.ID = "1111";
			builder.Card = "9999";

			var file = XElement.Parse(Request);
			var actual = builder.InsertSecuredData(file);

			Assert.That(actual.Descendants("id").Single().Value, Is.EqualTo("1111"));
			Assert.That(actual.Descendants("prop").First().LastAttribute.Value, Is.EqualTo("9999"));
		}

		[Test]
		public void InsertSignature_Always_InsertsSignatureIntoCorrespondingElement()
		{
			var builder = Create();
			encryption.CalculateSignature(Arg.Any<string>()).Returns("MD5HASH");

			var file = XElement.Parse(Request);
			var actual = builder.InsertSignature(file);

			var signature = actual.Descendants("signature").Single().Value;
			Assert.That(signature, Is.EqualTo("MD5HASH"));
		}

		[Test]
		public void InsertDatesRange_Always_InsertsDatesIntoCorrespondingElements()
		{
			var builder = Create();
			var date = new DateTime(2010, 1, 6);
			builder.Date = date;

			var file = XElement.Parse(Request);
			var actual = builder.InsertDatesRange(file);

			var dates = actual.Descendants("prop").Skip(2).ToList();
			Assert.That(dates.Last().Attribute("value").Value, Is.EqualTo(date.ToString("dd.MM.yyyy")));
			Assert.That(dates.First().Attribute("value").Value, Is.EqualTo(date.AddDays(-5).ToString("dd.MM.yyyy")));
		}

		[Test]
		public void ExtractDataElement_Always_ExtractsAllDataElement()
		{
			var builder = Create();

			var file = XElement.Parse(Request);
			var actual = builder.ExtractDataElement(file);

			Assert.That(actual, Does.Contain("oper"));
			Assert.That(actual, Does.Contain("wait"));
			Assert.That(actual, Does.Contain("test"));
			Assert.That(actual, Does.Contain("payment"));
		}
	}
}