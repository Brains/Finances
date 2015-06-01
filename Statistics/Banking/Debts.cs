using System;
using System.Collections.Generic;
using System.Linq;
using Tracker;

namespace Statistics.Banking
{

	public class Debts : IFundsStorage
	{
		private readonly IEnumerable<Record> records;

		public Debts(IEnumerable<Record> records)
		{
			this.records = records;
		}

		public void Get(Action<decimal> callback)
		{
			var debts = CalculateDebts(records.Where(record => record.Type == Record.Types.Debt));

			callback(debts.Sum(pair => pair.Value));
		}

		public Dictionary<Record.Categories, int> CalculateDebts(IEnumerable<Record> records )
		{
			var debts = (from record in records.Where(record => record.Type == Record.Types.Debt)
						 group record by record.Category
						 into dude
						 select new
						 {
							 Name = dude.Key,
							 Total = (from record in dude
									  group record by record.Description
									  into grouped
									  select new
									  {
										  Direction = grouped.Key,
										  Total = (int)grouped.Sum(record => record.Amount)
									  })
									  .ToDictionary(kind => kind.Direction, kind => kind.Total)
						 });

			var total = debts.Select(dude => new
			{
				dude.Name,
				Total = dude.Total["Out"] - dude.Total["In"]
			})
			.ToDictionary(dude => dude.Name, dude => dude.Total);



			return total;
		}
	}
}