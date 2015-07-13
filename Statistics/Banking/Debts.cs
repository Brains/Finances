using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Common;
using Common.Events;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Statistics.Storages;

namespace Statistics.Banking
{
	public class Debts : BindableBase, IStorage<int>
	{
		public int Value { get; set; }
		public Dictionary<Record.Categories, decimal> Dudes { get; set; }

		public Debts(IExpenses expenses, IEventAggregator events)
		{
			events.GetEvent<AddRecord>().Subscribe(record => Calculate(expenses), true);

			Calculate(expenses);
		}

		private void Calculate(IExpenses expenses)
		{
			Dudes = CalculateDebtsPerDude(expenses.Records);

			Value = (int) Dudes.Sum(pair => pair.Value);
			OnPropertyChanged("Value");
		}

		private Dictionary<Record.Categories, decimal> CalculateDebtsPerDude(IEnumerable<Record> records )
		{
			var debts = CalculateAmountsPerDude(records);
			var total = CalculateTotalsPerDude(records, debts);

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

		private Dictionary<Record.Categories, decimal> CalculateTotalsPerDude(IEnumerable<Record> records, Dictionary<Record.Categories, Dictionary<string, decimal>> debts)
		{
			var shared = CalculateShared(records);

			var total = debts.Select(dude => new
			{
				Name = dude.Key,
				Total = shared + Get(dude, "Out") - Get(dude, "In")
			});

			return total.ToDictionary(dude => dude.Name, dude => dude.Total);
		}

		private decimal CalculateShared(IEnumerable<Record> records)
		{
			return records.Where(record => record.Type == Record.Types.Shared)
			              .Sum(record => record.Amount);
		}

		private decimal Get(KeyValuePair<Record.Categories, Dictionary<string, decimal>> dude, string direction)
		{
			decimal value;
			dude.Value.TryGetValue(direction, out value);

			return value;
		}
	}
}