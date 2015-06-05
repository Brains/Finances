using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;

namespace Trends.ViewModels
{
	public class Trend
	{
		private readonly List<decimal> money;

		public List<Operation> Operations { get; set; }
		public List<Transaction> Calendar { get; set; }
		public List<Funds> Funds { get; set; }

		public Trend()
		{
			Operations = new List<Operation>();
			Calendar = new List<Transaction>();
			money = new List<decimal>();
			Funds = new List<Funds>();
		}

		public Trend(int startFunds) : this()
		{
			LoadOperations();
			Calculate(5400, new DateTime(2015, 5, 16), new DateTime(2015, 7, 1));
			Funds = GetFunds();
		}

		#region Public

		public void Calculate(decimal startFunds, DateTime start, DateTime end)
		{
			CalculateTransactionsCalendar(start, end);
			CalculateFunds(startFunds);
		}

		public List<Funds> GetFunds()
		{
			var output = new List<Funds>();

			for (int index = 0; index < this.money.Count; index++)
			{
				var transaction = Calendar[index];
				output.Add(new Funds(money[index], transaction.Date, transaction.Description));
			}

			return output;
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

		#endregion

		private void CalculateTransactionsCalendar(DateTime start, DateTime end)
		{
			foreach (var operation in Operations)
			{
				DateTime date = operation.Start;

				while (date < end)
				{
					if (date >= start)
						Calendar.Add(new Transaction(operation.Amount, date, operation.Description));

					date = operation.NextDate();
				}
			}

			Calendar.Sort();
        }

		private void CalculateFunds(decimal start)
		{
			Calendar.Aggregate(start, (a, b) =>
			{
				var sum = a + b.Amount;
				money.Add(sum);
				return sum;
			});
		}
	}
}