using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace Visualization.Banking
{
	class Encryption
	{
		public static string CalculateSignature(string data)
		{
			var md5 = GetHexString(CalculateMD5(data));
			var sha1 = GetHexString(CalculateSHA(md5));

			return sha1;
		}

		private static byte[] CalculateMD5(string input)
		{
			MD5 md5 = System.Security.Cryptography.MD5.Create();
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);

			return md5.ComputeHash(bytes);
		}

		private static byte[] CalculateSHA(string input)
		{
			SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
			byte[] bytes = Encoding.UTF8.GetBytes(input);

			return sha1.ComputeHash(bytes);
		}

		private static string GetHexString(byte[] bytes)
		{
			var builder = new StringBuilder();

			foreach (byte b in bytes)
			{
				var hex = b.ToString("x2");
				builder.Append(hex);
			}

			return builder.ToString();
		}
	}
}
