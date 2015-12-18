using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Common;
using UI.Interfaces;
using static System.DateTime;
using static System.Linq.Enumerable;
using static System.TimeSpan;

namespace UI.ViewModels
{
	public class Trend : Screen, IViewModel
	{
		private IList<Transaction> transactions;

		public Trend(ISettings settings)
		{
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

			ShiftTime(Operations);
			Transactions = Calculate(5000, Today, Today.AddMonths(2));
		}

		public IList<Transaction> Calculate(decimal startFunds, DateTime start, DateTime end)
		{
			decimal accumulator = startFunds;
			var seed = new[] {accumulator};

			var calendars = Operations.Select(operation => CalculateCalendar(start, end - start, operation));

			var transactions = Operations.Zip(calendars,
			                                  (operation, dates) => dates.Select(date => new
			                                  {
				                                  operation.Amount,
				                                  Date = date,
				                                  operation.Description
			                                  }))
			                             .SelectMany(operation => operation.ToArray())
			                             .OrderBy(transaction => transaction.Date);

			var amounts = transactions.Select(transaction => accumulator += transaction.Amount);
			amounts = seed.Concat(amounts);

			return transactions.Zip(amounts, (transaction, amount) => new Transaction
			{
				Amount = amount,
				Date = transaction.Date,
				Description = transaction.Description
			}).ToList();
		}

		public IEnumerable<DateTime> CalculateCalendar(DateTime start, TimeSpan interval, PermanentOperation operation)
		{
			var period = operation.Period.Ticks;
			var quantity = (int) (interval.Ticks / period);

			var calendar = Range(0, quantity).Select(index => operation.Start + FromTicks(index * period));
			
			return calendar.Where(date => date > start);
		}

		public void ShiftTime(PermanentOperation[] operations)
		{
			MoreLinq.MoreEnumerable.ForEach(operations, (operation, index) => operation.Start += FromHours(index + 1));
		}

		public class Transaction
		{
			public decimal Amount { get; set; }
			public DateTime Date { get; set; }
			public string Description { get; set; }
		}
	}
}