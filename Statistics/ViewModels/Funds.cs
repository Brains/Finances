using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Common;
using Common.Events;
using Finances.Properties;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Statistics.Storages;
using Statistics.Storages.Banking;
using Unity = Microsoft.Practices.Unity;

namespace Statistics.ViewModels
{
	public class Funds : BindableBase
	{
		private readonly IExpenses expenses;
		private readonly IEventAggregator events;

		public IStorage<int> Upwork { get; set; }
		public IStorage<int> Cards { get; set; }
		public IStorage<int> Cash { get; set; }
		public IStorage<int> Debt { get; set; }

		public int Total { get; set; }
		public int Divergence { get; set; }

		public Funds(IExpenses expenses, IEventAggregator events)
		{
			this.expenses = expenses;
			this.events = events;

			Upwork = new Input(nameof(Upwork));
			Cash = new Input(nameof(Cash));
			Cards = new PrivatBank();
			Debt = new Debts(expenses, events);

			Upwork.PropertyChanged += Update;
			Cash.PropertyChanged += Update;
			Cards.PropertyChanged += Update;
			Debt.PropertyChanged += Update;

			DoSmt();

		}

		private void DoSmt()
		{
			var query = from record in expenses.Records
//						where record.Date.Month == 3
						where record.Description != null
						group record by record.Description.Split().First()
						into grouped
//						where grouped.Key != "In" && grouped.Key != "Out"
						orderby grouped.Count() descending 
						select grouped;

			var asd = query.Take(5)
			               .ToDictionary(g => g.Key, group => group.Sum(record => record.Amount));


		}

		private void Update(object sender, PropertyChangedEventArgs args)
		{
			var balance = Cards.Value + Cash.Value + Debt.Value;

			Divergence = balance - CalculateEstimatedBalance();
			Total = balance + Upwork.Value * Settings.Default.ExchangeRate;

			OnPropertyChanged(nameof(Total));
			OnPropertyChanged(nameof(Divergence));

			events.GetEvent<UpdateTotal>().Publish(Total);
		}

		public int CalculateEstimatedBalance()
		{
			var previous = expenses.Records.Last(record => record.Type == Record.Types.Balance).Amount;

			var records = expenses.Records.GroupBy(record => record.Type)
										  .Select(type => new
										  {
											  type.Key,
											  Total = type.Sum(record => record.Amount)
										  })
										  .ToDictionary(type => type.Key, type => type.Total);

			var spending = records[Record.Types.Expense] + records[Record.Types.Shared];
			var income = records[Record.Types.Income];

			return (int) (previous + income - spending);
		}
	}
}