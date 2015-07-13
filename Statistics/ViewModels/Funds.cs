using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Serialization;
using Common;
using Common.Events;
using Finances.Properties;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Statistics.Banking;
using Statistics.Storages;
using Unity = Microsoft.Practices.Unity;

namespace Statistics.ViewModels
{
	public class Funds : BindableBase
	{
		private readonly IExpenses expenses;
		private readonly IEventAggregator events;

		private int total;
		private int balance;
		private int divergence;

		private int exchangeRate;

		public IStorage<int> Upwork { get; set; }
		public IStorage<int> Cards { get; set; }
		public IStorage<int> Cash { get; set; }
		public IStorage<int> Debt { get; set; }

		public Dictionary<Record.Categories, decimal> Debts { get; set; }

		public int Total
		{
			get { return total; }
			set
			{
				total = value;
				OnPropertyChanged(nameof(Total));
			}
		}

		public int Balance
		{
			get { return balance; }
			set
			{
				balance = value;
				OnPropertyChanged(nameof(Balance));
			}
		}

		public int Divergence
		{
			get { return divergence; }
			set
			{
				divergence = value;
				OnPropertyChanged(nameof(Divergence));
			}
		}

		public Funds(IExpenses expenses, IEventAggregator events)
		{
			exchangeRate = Settings.Default.ExchangeRate;

			this.expenses = expenses;
			this.events = events;

			Upwork = new Input(nameof(Upwork));
			Upwork.PropertyChanged += (s, a) => Update();
			Cash = new Input(nameof(Cash));
			Cash.PropertyChanged += (s, a) => Update();
			Cards = new PrivatBank();
			Cards.PropertyChanged += (s, a) => Update();
			Debt = new Debts(expenses, events);
			Debt.PropertyChanged += (s, a) => Update();

			Debts = (Debt as Debts).Calculate();
		}

		private void Update()
		{
			Balance = Cards.Value + Cash.Value + Debt.Value;
			Divergence = Balance - CalculateEstimatedBalance();

			Total = Balance + Upwork.Value * exchangeRate;

			events.GetEvent<UpdateTotal>().Publish(Total);
		}

		public int CalculateEstimatedBalance()
		{
			var previous = expenses.Records.Last(record => record.Type == Record.Types.Balance).Amount;

			var records = expenses.Records.GroupBy(record => record.Type)
			                      .Select(type => new {type.Key, Total = type.Sum(record => record.Amount)})
			                      .ToDictionary(type => type.Key, type => type.Total);

			var spending = records[Record.Types.Expense] + records[Record.Types.Shared];
			var income = records[Record.Types.Income];

			return (int) (previous + income - spending);
		}
	}
}