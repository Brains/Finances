using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Common;
using MoreLinq;
using UI.Interfaces;
using static System.TimeSpan;

namespace UI.ViewModels
{
	public class Trend : Screen, IViewModel
	{
		public Trend(ISettings settings)
		{
			Operations = settings.PermanentOperations;
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();

			ShiftTime(Operations);
		}

		public void ShiftTime(PermanentOperation[] operations)
		{
			operations.ForEach((operation, index) => operation.Start += FromHours(index + 1));
        }

		public PermanentOperation[] Operations { get; set; }

		public IEnumerable<dynamic> Calculate(decimal startFunds, DateTime start, DateTime end)
		{
			decimal accumulator = 1000;
			var seed = new[] { accumulator };

			var calendars  = Operations.Select(operation => CalculateCalendar(start, end - start, operation));

			var transactions = Enumerable.Zip(Operations, calendars,
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

			var data = Enumerable.Zip(transactions, amounts, (transaction, amount) => new
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
				                 var shift = FromTicks(index * period);
				                 return start + shift;
			                 });
		}
	}
}