using System.Collections.Generic;
using System.Linq;
using NodaTime;
using NodaTime.Extensions;

namespace Trends
{
	public class Trend
	{
		private readonly Period monthly = Period.FromMonths(1);

		//------------------------------------------------------------------
		public List<Operation> Operations { get; set; }
		public List<Transaction> Calendar { get; set; }

		//------------------------------------------------------------------
		public Trend ()
		{
			Operations = new List<Operation>();
			Calendar = new List<Transaction>();
		}

		//------------------------------------------------------------------
		public void LoadOperations ()
		{
			Operations.Add(new Operation(-300, new LocalDate(2015, 1, 1), Period.FromDays(3), "Food"));
			Operations.Add(new Operation(-140, new LocalDate(2015, 1, 5), monthly, "Mozy"));
			Operations.Add(new Operation(-30, new LocalDate(2015, 1, 5), monthly, "O3"));
			Operations.Add(new Operation(1200, new LocalDate(2015, 1, 5), monthly, "Deposit"));
			Operations.Add(new Operation(200, new LocalDate(2015, 1, 7), monthly, "Deposit"));
			Operations.Add(new Operation(600, new LocalDate(2015, 1, 14), monthly, "Deposit"));
			Operations.Add(new Operation(-1800, new LocalDate(2015, 1, 15), monthly, "House"));
		}

		//------------------------------------------------------------------
		public void FillCalendar ()
		{
			PutOperationsOnCalendar(new LocalDate(2015, 6, 1));
		}

		//------------------------------------------------------------------
		public void PutOperationsOnCalendar (LocalDate end)
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
		public List<Transaction> GroupByDate(List<Transaction> calendar)
		{
			var query = from transaction in Calendar
						orderby transaction
						group transaction by transaction.Date
						into grouped
						select grouped.Aggregate((a, b) => a + b);

			return query.ToList();
		}

		//------------------------------------------------------------------
		private LocalDate GetCurrentDate()
		{
			ZonedClock clock = SystemClock.Instance.InUtc();

			return clock.GetCurrentDate();
		}

	}


}
