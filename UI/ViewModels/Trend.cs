using System;
using System.Collections.Generic;
using Caliburn.Micro;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class Trend : PropertyChangedBase, IViewModel
	{
		public List<Operation> Operations { get; set; }

		public void LoadOperations()
		{
			var monthly = TimeSpan.FromDays(30.436875);

			Operations.Add(new Operation(-300, new DateTime(2015, 5, 15, 5, 0, 0), TimeSpan.FromDays(3), "Food"));
			Operations.Add(new Operation(-2000, new DateTime(2015, 1, 15, 1, 0, 0), monthly, "House"));
			Operations.Add(new Operation(-1000, new DateTime(2015, 5, 30, 3, 0, 0), TimeSpan.FromDays(20), "Medications"));

			Operations.Add(new Operation(200, new DateTime(2015, 1, 7, 7, 0, 0), monthly, "Deposit"));
			Operations.Add(new Operation(2000, new DateTime(2015, 1, 8, 2, 0, 0), monthly, "Deposit"));
			Operations.Add(new Operation(1000, new DateTime(2015, 1, 17, 4, 0, 0), monthly, "Deposit"));

			Operations.Add(new Operation(-500, new DateTime(2015, 5, 1, 6, 0, 0), TimeSpan.FromDays(7), "Сorrection"));
		}

	}

	public class Operation
	{
		public decimal Amount { get; set; }
		public DateTime Start { get; set; }
		public TimeSpan Period { get; set; }
		public string Description { get; set; }

		public Operation(decimal amount, DateTime start, TimeSpan period, string description)
		{
			Amount = amount;
			Period = period;
			Description = description;
			Start = start;
		}
	}
}