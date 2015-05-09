using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NodaTime;
using NodaTime.Extensions;

namespace Trends
{
	public class Trend
	{
		private readonly List<decimal> funds;

		//------------------------------------------------------------------
		public List<Operation> Operations { get; set; }
		public List<Transaction> Calendar { get; set; }

		//------------------------------------------------------------------
		public Trend ()
		{
			Operations = new List<Operation>();
			Calendar = new List<Transaction>();
			funds = new List<decimal>();
		}

		#region Public

		//------------------------------------------------------------------
		public void Calculate(decimal startFunds, LocalDate end)
		{
			CalculateTransactionsCalendar(end);
			AggregateTransactionsByDate();
			CalculateFunds(startFunds);
		}

		//------------------------------------------------------------------
		public List<Funds> GetFunds()
		{
			var output = new List<Funds>();

			for (int index = 0; index < this.funds.Count; index++)
			{
				var transaction = Calendar[index];
				output.Add(new Funds(funds[index], transaction.Date, transaction.Description));
			}

			return output;
		}

		//------------------------------------------------------------------
		public void LoadOperations ()
		{
			var monthly = Period.FromMonths(1);

			Operations.Add(new Operation(-300, new LocalDate(2015, 1, 1), Period.FromDays(3), "Food"));
			Operations.Add(new Operation(-140, new LocalDate(2015, 1, 5), monthly, "Mozy"));
			Operations.Add(new Operation(-30, new LocalDate(2015, 1, 5), monthly, "O3"));
			Operations.Add(new Operation(1200, new LocalDate(2015, 1, 5), monthly, "Deposit"));
			Operations.Add(new Operation(200, new LocalDate(2015, 1, 7), monthly, "Deposit"));
			Operations.Add(new Operation(600, new LocalDate(2015, 1, 14), monthly, "Deposit"));
			Operations.Add(new Operation(-1800, new LocalDate(2015, 1, 15), monthly, "House"));
		}

		#endregion

		//------------------------------------------------------------------
		private void CalculateTransactionsCalendar (LocalDate end)
		{
			foreach (var operation in Operations)
			{
				LocalDate date = operation.Start;

				while (date < end)
				{
					Calendar.Add(new Transaction(operation.Amount, date, operation.Description));

					date = date + operation.Period;
				}
			}
		}

		//------------------------------------------------------------------
		private void AggregateTransactionsByDate()
		{
			var query = from transaction in Calendar
						orderby transaction
						group transaction by transaction.Date
						into grouped
						select grouped.Aggregate((a, b) => a + b);

			Calendar = query.ToList();
		}

		//------------------------------------------------------------------
		private void CalculateFunds (decimal start)
		{
			Calendar.Aggregate(start, (a, b) =>
			{
				var sum = a + b.Amount;
				funds.Add(sum);
				return sum;
			});
		}

		//------------------------------------------------------------------
		private LocalDate GetCurrentDate()
		{
			ZonedClock clock = SystemClock.Instance.InUtc();

			return clock.GetCurrentDate();
		}
	}


}
