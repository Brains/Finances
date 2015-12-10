using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Storages;
using static Common.Record;

namespace Funds.Sources
{
	public class Debts : Base
	{
		private readonly IExpenses expenses;

		public Debts(IExpenses expenses)
		{
			this.expenses = expenses;
		}

		public override void PullValue()
		{
			Value = 200;

			var res = CalculateAmountsPerDude()
        }

		private Dictionary<Categories, Dictionary<string, decimal>> CalculateAmountsPerDude(IEnumerable<Record> records)
		{
			return (from record in records.Where(record => record.Type == Types.Debt)
					group record by record.Category // Each Dude is a separate Category
					into dude
					select new
					{
						Name = dude.Key,
						Total = (from record in dude
								 group record by record.Description // Descriptions are either "In" or "Out" mean Direction of debt
								 into grouped
								 select new
								 {
									 Direction = grouped.Key,
									 Total = grouped.Sum(record => record.Amount)
								 })
							.ToDictionary(kind => kind.Direction, kind => kind.Total)
					}).ToDictionary(dude => dude.Name, dude => dude.Total);
		}


	}
}