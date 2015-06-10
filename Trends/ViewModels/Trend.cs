using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Statistics.ViewModels;

namespace Trends.ViewModels
{
	public class Trend : BindableBase
	{
		private DateTime start;
		private DateTime end;
		private int total;

		public List<Operation> Operations { get; set; }
		public List<Transaction> Funds { get; set; }

		public Trend()
		{
			Operations = new List<Operation>();
		}

		public Trend(int startFunds, IEventAggregator events) : this()
		{
			LoadOperations();

			events.GetEvent<UpdateTotalEvent>().Subscribe(Update);
		}


		private void Update(int startFunds)
		{
			Funds = Calculate(startFunds, DateTime.Now, DateTime.Now.AddMonths(2));

			OnPropertyChanged("Funds");
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

			operation.FlushDate();

			return calendar;
		}
	}
}