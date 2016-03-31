using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Common;
using Common.Storages;
using UI.Interfaces;
using static System.DateTime;
using static System.Linq.Enumerable;
using static System.TimeSpan;
using static Common.Record.Types;

namespace UI.ViewModels
{
	public class Trend : Screen, IViewModel
	{
	    private readonly IExpenses expenses;
	    private readonly ISettings settings;

	    public Trend(IExpenses expenses, ISettings settings)
	    {
	        this.expenses = expenses;
	        this.settings = settings;

	        Interval = settings.HistoryInterval;
	    }

	    public IEnumerable<Transaction> Transactions { get; set; }
        public int Interval { get; set; }
	    public DateTime Now { get; set; } = DateTime.Now;

        protected override void OnInitialize()
		{
			base.OnInitialize();

            Transactions = Calculate(expenses.Records);
		}

	    public IEnumerable<Transaction> Calculate(IEnumerable<Record> records)
	    {
	        decimal accumulator = 0;

	        var transactions = CombineByDay(records)
	            .OrderBy(record => record.Date)
                .Select(transaction =>
	            {
	                transaction.Total = accumulator += transaction.Amount;
	                return transaction;
	            })
	            .Where(IsShown)
	            .ToList();

            return transactions;
	    }

	    public IEnumerable<Transaction> CombineByDay(IEnumerable<Record> records)
	    {
	        return records.GroupBy(record => record.Date.Date)
	                      .Select(day => new Transaction
	                      {
	                          Date = day.Key,
	                          Amount = day.Sum(record => GetAmount(record)),
	                          Category = day.GroupBy(record => record.Category)
	                                        .Select(record => record.Key.ToString())
	                                        .Aggregate((a, b) => $"{a}\n{b}"),
	                          Description = day.Select(record => record.Description)
	                                           .Aggregate((a, b) => $"{a}\n{b}")
	                      });
	    }

	    public decimal GetAmount(Record record)
        {
	        var amount = record.Amount;

	        if (record.Type == Debt && record.Description == "In") return amount;
	        if (record.Type == Income) return amount;
	        if (record.Type == Shared) return -amount * settings.Customers;

	        return -amount;
        }

	    public bool IsShown(Transaction transaction)
	    {
	        return Now - transaction.Date < FromDays(Interval);
	    }

	    public class Transaction
		{
		    public decimal Amount { get; set; }
			public decimal Total { get; set; }
			public DateTime Date { get; set; }
			public string Category { get; set; }
			public string Description { get; set; }
		}
	}
}