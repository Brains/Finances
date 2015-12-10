using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Storages;
using static Common.Record;
using static Common.Record.Types;
using Directions = System.Collections.Generic.Dictionary<string, decimal>;

namespace Funds.Sources
{
	public class Debts : Base
	{
		private readonly IExpenses expenses;

		public Debts(IExpenses expenses)
		{
			this.expenses = expenses;
		}

		public Dictionary<Categories, decimal> Dudes { get; set; }

		public override void PullValue()
		{
			var records = expenses.Records;
			var shared = records.Where(r => r.Type == Shared)
			                    .Sum(r => r.Amount);

			var debtType = records.Where(r => r.Type == Debt).ToArray();
			Validate(debtType);

			var debts = CalculateAmountsPerDude(debtType);
			Dudes = CalculateTotalsPerDude(debts, shared);

			Value = Dudes.Sum(pair => pair.Value);
        }

		private static Dictionary<Categories, decimal> CalculateTotalsPerDude(Dictionary<Categories, Directions> debts,
		                                                                      decimal shared)
		{
			return debts.ToDictionary(dude => dude.Key,
			                          dude => shared + dude.Value["Out"] - dude.Value["In"]);
		}

		public Dictionary<Categories, Directions> CalculateAmountsPerDude(IEnumerable<Record> records)
		{
			return records.ToLookup(record => record.Category)
			              .ToDictionary(dude => dude.Key, GetTotalForEachDirection);
		}

		private static Directions GetTotalForEachDirection(IGrouping<Categories, Record> dude)
		{
			return dude.ToLookup(record => record.Description)
			           .ToDictionary(direction => direction.Key,
			                         direction => direction.Sum(record => record.Amount));
		}

		public void Validate(IEnumerable<Record> records)
		{
			var invalid = records.Where(record =>
			{
				var input = record.Description.Equals("In");
				var output = record.Description.Equals("Out");

				return !input && !output;
			});

			if (invalid.Any())
				throw new ArgumentException("Wrong Description for Debt record");
		}
	}
}