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
		private IEncryption encryption;

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

		private RequestBuilder Create()
		{
			encryption = For<IEncryption>();

			return new RequestBuilder(encryption);
		}

		[Test]
		public void InsertSecuredData_Always_InsertsSecuredDataIntoCorrespondingElements()
		{
			var builder = Create();
			var data = new Data("1111", "", "9999");

			var file = XElement.Parse(Request);
			var actual = builder.InsertSecuredData(file, data);

			Assert.That(actual.Descendants("id").Single().Value, Is.EqualTo("1111"));
			Assert.That(actual.Descendants("prop").First().LastAttribute.Value, Is.EqualTo("9999"));
		}

		[Test]
		public void InsertSignature_Always_InsertsSignatureIntoCorrespondingElement()
		{
			var builder = Create();
			var data = new Data("", "qwerty", "");
			encryption.CalculateSignature(Arg.Any<string>()).Returns("MD5HASH");

			var file = XElement.Parse(Request);
			var actual = builder.InsertSignature(file, data);

			Assert.That(actual.Descendants("signature").Single().Value, Is.EqualTo("MD5HASH"));
		}
	}
}