using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Events;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.PubSubEvents;
using Tracker;

namespace Statistics.Banking
{
	public class Debts : IFundsStorage
	{
		private readonly IEnumerable<Record> records;
		private Action<decimal> callback;

		public Debts(IExpenses expenses, IEventAggregator eventAggregator)
		{
			eventAggregator.GetEvent<AddRecord>().Subscribe(record => CalculateDebts(callback), true);

			records = expenses.Records;
		}

		void IFundsStorage.Get(Action<decimal> callback)
		{
			this.callback = callback;

			CalculateDebts(callback);
		}

		private void CalculateDebts(Action<decimal> callback)
		{
			var debts = CalculateDebts(records);

			callback(debts.Sum(pair => pair.Value));
		}

		public Dictionary<Record.Categories, decimal> Calculate()
		{
			var debts = CalculateDebts(records);

			return debts;
		}

		private Dictionary<Record.Categories, decimal> CalculateDebts(IEnumerable<Record> records )
		{
			var debts = CalculateAmountsPerDude(records);
			var total = CalculateTotals(records, debts);

			return total;
		}

		private Dictionary<Record.Categories, Dictionary<string, decimal>> CalculateAmountsPerDude(IEnumerable<Record> records)
		{
			return (from record in records.Where(record => record.Type == Record.Types.Debt)
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

		private Dictionary<Record.Categories, decimal> CalculateTotals(IEnumerable<Record> records, Dictionary<Record.Categories, Dictionary<string, decimal>> debts)
		{
			var shared = CalculateShared(records);

			var total = debts.Select(dude => new
			{
				Name = dude.Key,
				Total = shared + Get(dude, "Out") - Get(dude, "In")
			});

			return total.ToDictionary(dude => dude.Name, dude => dude.Total);
		}

		private decimal Get(KeyValuePair<Record.Categories, Dictionary<string, decimal>> dude, string direction)
		{
			decimal value;
			dude.Value.TryGetValue(direction, out value);

			return value;
		}

		private decimal CalculateShared(IEnumerable<Record> records)
		{
			return records.Where(record => record.Type == Record.Types.Shared)
			              .Sum(record => record.Amount);
		}
	}
}