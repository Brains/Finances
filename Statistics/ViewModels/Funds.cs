using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Statistics.Banking;
using Tracker;
using Unity = Microsoft.Practices.Unity;

namespace Statistics.ViewModels
{
	public class Funds : BindableBase
	{
		public const int ExchangeRate = 21;

		private readonly IExpenses expenses;
		private readonly IEventAggregator events;

		private int cards;
		private int debt;
		private int cash;
		private int upwork;
		private int total;
		private int balance;
		private int divergence;

		public int Upwork
		{
			get { return upwork; }
			set { upwork = value; OnPropertyChanged(nameof(Upwork)); Update();}
		}

		public int Cards
		{
			get { return cards; }
			set { cards = value; OnPropertyChanged(nameof(Cards)); Update(); }
		}

		public int Cash
		{
			get { return cash; }
			set { cash = value; OnPropertyChanged(nameof(Cash)); Update(); }
		}

		public int Debt
		{
			get { return debt; }
			set { debt = value; OnPropertyChanged(nameof(Debt)); Update(); }
		}

		public Dictionary<Record.Categories, decimal> Debts { get; set; }

		public int Total
		{
			get { return total; }
			set { total = value; OnPropertyChanged(nameof(Total)); }
		}

		public int Balance
		{
			get { return balance; }
			set { balance = value; OnPropertyChanged(nameof(Balance)); }
		}

		public int Divergence
		{
			get { return divergence; }
			set { divergence = value; OnPropertyChanged(nameof(Divergence)); }
		}

		public Funds(IExpenses expenses, [Unity.Dependency("bank")] IFundsStorage bank, [Unity.Dependency("debt")]IFundsStorage debt, IEventAggregator eventAggregator)
		{
			this.expenses = expenses;
			this.events = eventAggregator;

			Load();

            bank.Get(amount => Cards = (int) amount);
			debt.Get(amount => Debt = (int) amount);

			Debts = (debt as Debts).Calculate();
		}

		private void Update()
		{
			Balance = Cards + Cash + Debt;
			Divergence = Balance - CalculateEstimatedBalance();

			Total = Balance + Upwork * ExchangeRate;

			events.GetEvent<UpdateTotalEvent>().Publish(Total);
        }

		private void Load()
		{
			var path = Path.Combine("Data", "Funds.xml");

			XElement file = XElement.Load(path);

			Upwork = int.Parse(file.Element("Upwork").Value);
			Cash = int.Parse(file.Element("Cash").Value);
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