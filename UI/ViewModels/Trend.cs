using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class Trend : PropertyChangedBase, IViewModel
	{
		public class PermanentOperation
		{
			public readonly decimal Amount;
			public readonly DateTime Start;
			public readonly TimeSpan Period;
			public readonly string Description;

			public PermanentOperation(decimal amount, DateTime start, TimeSpan period, string description)
			{
				Amount = amount;
				Start = start;
				Period = period;
				Description = description;
			}
		}

		public List<PermanentOperation> Operations { get; set; }

		public void LoadOperations()
		{
			var monthly = TimeSpan.FromDays(30.436875);

			Operations = new List<PermanentOperation>
			{
				new PermanentOperation(-300, new DateTime(2015, 5, 15, 5, 0, 0), TimeSpan.FromDays(3), "Food"),
				new PermanentOperation(-2000, new DateTime(2015, 1, 15, 1, 0, 0), monthly, "House"),
				new PermanentOperation(-1000, new DateTime(2015, 5, 30, 3, 0, 0), TimeSpan.FromDays(20), "Medications"),
				new PermanentOperation(200, new DateTime(2015, 1, 7, 7, 0, 0), monthly, "Deposit"),
				new PermanentOperation(2000, new DateTime(2015, 1, 8, 2, 0, 0), monthly, "Deposit"),
				new PermanentOperation(1000, new DateTime(2015, 1, 17, 4, 0, 0), monthly, "Deposit"),
				new PermanentOperation(-500, new DateTime(2015, 5, 1, 6, 0, 0), TimeSpan.FromDays(7), "Сorrection")
			};


		}

		public IEnumerable<dynamic> Calculate(decimal startFunds, DateTime start, DateTime end)
		{
			decimal accumulator = 1000;
			var seed = new[] { accumulator };

			var calendars  = Operations.Select(operation => CalculateCalendar(start, end - start, operation));

			var transactions = Operations.Zip(calendars,
			                                  (operation, dates) => dates.Select(date => new
			                                  {
				                                  Amount = operation.Amount,
				                                  Date = date,
				                                  Description = operation.Description
			                                  }))
			                             .SelectMany(operation => operation.ToArray())
			                             .OrderBy(transaction => transaction.Date);

			var amounts = transactions.Select(transaction => accumulator += transaction.Amount);
			amounts = seed.Concat(amounts);

			var data = transactions.Zip(amounts, (transaction, amount) => new
			{
				Amount = amount,
				Date = transaction.Date,
				Description = transaction.Description
			});

			return data;
		}

		private static IEnumerable<DateTime> CalculateCalendar(DateTime start, TimeSpan interval, PermanentOperation operation)
		{
			var period = operation.Period.Ticks;
			var quantity = interval.Ticks / period;

			return Enumerable.Range(0, (int) quantity)
			                 .Select(index =>
			                 {
				                 var shift = TimeSpan.FromTicks(index * period);
				                 return start + shift;
			                 });
		}
	}

}