using System;
using System.Linq;
using System.Xml.Linq;

namespace Funds.Bank
{
	public interface IResponceParser
	{
		decimal ParseBalance(string responce);
	}

	public class ResponceParser : IResponceParser
	{
		public decimal ParseBalance(string input)
		{
			var file = XElement.Parse(input);
			string element;

			try
			{
				var descendants = file.Descendants("av_balance");
				element = descendants.First().Value;
			}
			catch (InvalidOperationException)
			{
				element = "0";
			}

			return decimal.Parse(element);
		}
	}
}