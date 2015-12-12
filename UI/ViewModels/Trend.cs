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

		public IEnumerable<object> Transactions { get; set; }

		protected override void OnInitialize()
		{
			base.OnInitialize();

			ShiftTime(Operations);
			Transactions = Calculate(0, Now, Now.AddMonths(3));
		}

		public void ShiftTime(PermanentOperation[] operations)
		{
			MoreLinq.MoreEnumerable.ForEach(operations, (operation, index) => operation.Start += FromHours(index + 1));
		}

		public IEnumerable<dynamic> Calculate(decimal startFunds, DateTime start, DateTime end)
		{
			decimal accumulator = 1000;
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

			var data = transactions.Zip(amounts, (transaction, amount) => new
			{
				Amount = amount,
				transaction.Date,
				transaction.Description
			});

			return data;
		}

		private static IEnumerable<DateTime> CalculateCalendar(DateTime start, TimeSpan interval, PermanentOperation operation)
		{
			var period = operation.Period.Ticks;
			var quantity = interval.Ticks / period;

			return Range(0, (int) quantity)
			                 .Select(index => start + FromTicks(index * period));
		}
	}
}