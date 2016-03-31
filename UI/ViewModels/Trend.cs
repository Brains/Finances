using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Common;
using Common.Storages;
using UI.Interfaces;
using static System.DateTime;
using static System.Linq.Enumerable;
using static System.TimeSpan;

namespace UI.ViewModels
{
	public class Trend : Screen, IViewModel
	{
	    private readonly IExpenses expenses;

	    public Trend(IExpenses expenses)
		{
		    this.expenses = expenses;
		}

		public IEnumerable<Transaction> Transactions { get; set; }
        public int Interval { get; set; } = 60;
	    public DateTime Now { get; set; } = DateTime.Now;

        protected override void OnInitialize()
		{
			base.OnInitialize();

		    var records = expenses.Records.Where(record => record.Type != Record.Types.Debt);

            Transactions = Calculate(records);
		}

	    public IEnumerable<Transaction> Calculate(IEnumerable<Record> records)
	    {
	        decimal accumulator = 0;

	        return CombineByDay(records).OrderBy(record => record.Date)
	                                    .Select(record => new Transaction(accumulator += record.Amount, record))
	                                    .Where(IsShown)
                                        .ToList();
	    }

	    public IEnumerable<Record> CombineByDay(IEnumerable<Record> records)
	    {
	        return records.GroupBy(record => record.Date.Date)
	                      .Select(day => new Record
	                      {
	                          Date = day.Key,
	                          Amount = day.Sum(record => GetAmount(record)),
	                          Description = day.Select(record => record.Description)
	                                           .Aggregate((a, b) => $"{a}\n{b}")
	                      });
	    }

	    public decimal GetAmount(Record record)
        {
            if (record.Type == Record.Types.Income)
                return record.Amount;
            if (record.Type == Record.Types.Debt
                && record.Description == "In")
                return record.Amount;

            return -record.Amount;
        }

	    public bool IsShown(Transaction transaction)
	    {
	        return Now - transaction.Date < FromDays(Interval);
	    }

	    public class Transaction
		{
		    public Transaction(decimal total, Record record)
		    {
		        Total = total;
                Amount = record.Amount;
		        Date = record.Date;
		        Description = record.Description;
		    }

		    public decimal Amount { get; set; }
			public decimal Total { get; set; }
			public DateTime Date { get; set; }
			public string Description { get; set; }
		}
	}
}