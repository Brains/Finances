using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;

namespace Trends.ViewModels
{
	public class Trend
	{
		private DateTime start;
		private DateTime end;

		public List<Operation> Operations { get; set; }
		public List<Transaction> Funds { get; set; }

		public Trend()
		{
			Operations = new List<Operation>();
		}

		public Trend(int startFunds) : this()
		{
			LoadOperations();
			Funds = Calculate(5400, new DateTime(2015, 5, 16), new DateTime(2015, 7, 1));
		}

		public void LoadOperations()
		{
			var monthly = DatePeriod.FromMonths(1);

			Operations.Add(new Operation(-300, new DateTime(2015, 5, 15, 5, 0, 0), DatePeriod.FromDays(3), "Food"));
			Operations.Add(new Operation(-2000, new DateTime(2015, 1, 15, 1, 0, 0), monthly, "House"));
			Operations.Add(new Operation(-1000, new DateTime(2015, 5, 30, 3, 0, 0), DatePeriod.FromDays(20), "Medications"));

			Operations.Add(new Operation(1300, new DateTime(2015, 1, 5, 2, 0, 0), monthly, "Deposit"));
			Operations.Add(new Operation(200, new DateTime(2015, 1, 7, 7, 0, 0), monthly, "Deposit"));
			Operations.Add(new Operation(900, new DateTime(2015, 1, 17, 4, 0, 0), monthly, "Deposit"));

			Operations.Add(new Operation(-300, new DateTime(2015, 5, 1, 6, 0, 0), DatePeriod.FromDays(7), "Сorrection"));
		}

		public List<Transaction> Calculate(decimal startFunds, DateTime startDate, DateTime endDate)
		{
			start = startDate;
			end = endDate;

			var initial = new Transaction(startFunds, start, "Initial");

			var funds = Operations.SelectMany(CalculateCalendar)
			                      .OrderBy(transaction => transaction.Date)
			                      .Aggregate(new List<Transaction>() { initial }, Aggregate)
			                      .ToList();

			return funds;
		}

		private List<Transaction> Aggregate(List<Transaction> list, Transaction transaction)
		{
			var previous = list.Last();
			var amount = previous.Amount + transaction.Amount;
			var sum = new Transaction(amount, transaction.Date, transaction.Description);

			list.Add(sum);

			return list;
		}

		private List<Transaction> CalculateCalendar(Operation operation)
		{
			DateTime date = operation.Start;

			List<Transaction> calendar = new List<Transaction>();

			while (date < end)
			{
				if (date >= start)
					calendar.Add(new Transaction(operation.Amount, date, operation.Description));

				date = operation.NextDate();
			}

			return calendar;
		}
	}
}