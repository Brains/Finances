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
	    private IList<Transaction> transactions;

		public Trend(IExpenses expenses, ISettings settings)
		{
		    this.expenses = expenses;
		    Operations = settings.PermanentOperations;
		}

	    public PermanentOperation[] Operations { get; set; }

		public IList<Transaction> Transactions
		{
			get { return transactions; }
			set
			{
				if (Equals(value, transactions)) return;
				transactions = value;
				NotifyOfPropertyChange();
			}
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();

		    var records = expenses.Records.Where(record => record.Type != Record.Types.Debt);

            Calculate(records);
		}

	    private void Calculate(IEnumerable<Record> records)
	    {
	        decimal accumulator = 0;

	        var combined = CombineByDay(records);
	        Transactions = combined.OrderBy(record => record.Date)
	                              .Select(record => new Transaction(accumulator += record.Amount, record))
	                              .Skip(200)
	                              .ToList();
	    }

	    private IEnumerable<Record> CombineByDay(IEnumerable<Record> records)
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

	    private decimal GetAmount(Record record)
        {
            if (record.Type == Record.Types.Income)
                return record.Amount;

            if (record.Type == Record.Types.Debt
                && record.Description == "In")
                return record.Amount;

            return -record.Amount;
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