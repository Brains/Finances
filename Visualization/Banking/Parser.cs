using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Tracker;

namespace Visualization.Banking
{
	static class Parser
	{
		private static readonly Dictionary<Record.Categories, string[]> categoriesMarkers;

		static Parser()
		{
			categoriesMarkers = new Dictionary<Record.Categories, string[]>
			{
				[Record.Categories.Food] = new []{ "Новус", "FUDMEREZHA" },
				[Record.Categories.Health] = new []{ "Аптека" },
				[Record.Categories.General] = new []{ "MOZY" },
				[Record.Categories.House] = new []{ "телекоммуникационные" },
			};
		}

		public static decimal ParseBalance(string input)
		{
			XElement file = XElement.Parse(input);

			var element = file.Descendants("balance").First().Value;

			return Math.Round(decimal.Parse(element));
		}

		public static IEnumerable<Record> ParseHistory(string input)
		{
			XElement file = XElement.Parse(input);
			var elements = file.Descendants("statement");

			var history = elements.Select(ParseStatement);

			return history;
		}

		private static Record ParseStatement(XElement element)
		{
			var tuple = ParseText(element);

			string description = tuple.Item2;
			var category = ParseCategory(description);

			return new Record(tuple.Item1, Record.Types.Expense, category, description.Remove(15), tuple.Item3);
		}


		private static Record.Categories ParseCategory(string description)
		{
			return categoriesMarkers.Where(markers => markers.Value.Any(description.Contains))
			                        .Select(markers => markers.Key)
			                        .FirstOrDefault();
		}

		private static Tuple<decimal, string, DateTime> ParseText(XElement e)
		{
			var amountText = e.Attribute("amount").Value
				.Replace("UAH", String.Empty)
				.Replace("USD", String.Empty);
			var amount = Math.Round(decimal.Parse(amountText));

			var time = TimeSpan.Parse(e.Attribute("trantime").Value);
			var date = DateTime.Parse(e.Attribute("trandate").Value);

			return Tuple.Create(amount, e.Attribute("terminal").Value, date.Add(time));
		}
	}
}