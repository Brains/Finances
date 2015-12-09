using System.Security.Cryptography;
using System.Text;

// ReSharper disable InconsistentNaming

namespace Funds
{
	internal interface IEncryption
	{
		string CalculateSignature(string data);
	}

	internal class Encryption : IEncryption
	{
		public string CalculateSignature(string data)
		{
			var md5 = GetHexString(CalculateMD5(data));
			var sha1 = GetHexString(CalculateSHA(md5));

			return sha1;
		}

		private byte[] CalculateMD5(string input)
		{
			var md5 = MD5.Create();
			var bytes = Encoding.UTF8.GetBytes(input);

			return md5.ComputeHash(bytes);
		}

		private byte[] CalculateSHA(string input)
		{
			var sha1 = SHA1.Create();
			var bytes = Encoding.UTF8.GetBytes(input);

			return sha1.ComputeHash(bytes);
		}

		private string GetHexString(byte[] bytes)
		{
			var builder = new StringBuilder();

			foreach (var b in bytes)
			{
				var hex = b.ToString("x2");
				builder.Append(hex);
			}

			return builder.ToString();
		}
	}
}