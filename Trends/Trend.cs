using System.Collections.Generic;
using System.Linq;
using NodaTime;

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
			Operations = new List<Operation>
			{
				new Operation(-300, new LocalDate(2015, 1, 3), Period.FromDays(3), "Food"),
				new Operation(-140, new LocalDate(2015, 1, 5), monthly, "Mozy"),
				new Operation(-30, new LocalDate(2015, 1, 5), monthly, "O3"),
				new Operation(-1800, new LocalDate(2015, 1, 15), monthly, "House"),
				new Operation(1200, new LocalDate(2015, 1, 5), monthly, "Deposit"),
				new Operation(200, new LocalDate(2015, 1, 7), monthly, "Deposit"),
				new Operation(600, new LocalDate(2015, 1, 14), monthly, "Deposit"),
			};

			CreateCalendar();
		}

		//------------------------------------------------------------------
		private void CreateCalendar ()
		{
			Calendar = new List<Transaction>();
			

			foreach (var operation in Operations)
			{
				LocalDate date = operation.Start;

				while (date.Month < 3)
				{
                    Calendar.Add(new Transaction(operation.Amount, date, operation.Description));
					date = date + operation.Period;
				}
			}

			Calendar.Sort();
		}
	}


}
