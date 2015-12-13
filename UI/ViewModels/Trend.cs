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
		public Trend(ISettings settings)
		{
			Operations = settings.PermanentOperations;
		}

		public PermanentOperation[] Operations { get; set; }
		public IEnumerable<Transaction> Transactions { get; set; }

		protected override void OnInitialize()
		{
			base.OnInitialize();

			ShiftTime(Operations);
			Transactions = Calculate(10000, Now, Now.AddMonths(3));
		}

		public IEnumerable<Transaction> Calculate(decimal startFunds, DateTime start, DateTime end)
		{
			decimal accumulator = startFunds;
			var seed = new[] {accumulator};

			var calendars = Operations.Select(operation => CalculateCalendar(start, end - start, operation.Period.Ticks));

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
			});
		}

		public IEnumerable<DateTime> CalculateCalendar(DateTime start, TimeSpan interval, long period)
		{
			var quantity = (int) (interval.Ticks / period);

			return Range(0, quantity).Select(index => start + FromTicks(index * period));
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