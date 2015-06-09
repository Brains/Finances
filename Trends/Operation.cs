using System;
using System.Globalization;

namespace Trends
{
	public class Operation
	{
		private DateTime previousDate;

		public decimal Amount { get; set; }
		public DateTime Start { get; set; }
		public DatePeriod Period { get; set; }
		public string Description { get; set; }

		public Operation (decimal amount, DateTime start, DatePeriod period, string description)
		{
			Amount = amount;
			Period = period;
			Description = description;
			Start = start;
			previousDate = Start;
		}

		public DateTime NextDate()
		{
			previousDate = Period.Next(previousDate);

			return previousDate;
		}

		public DateTime NextDate(DateTime after)
		{
			DateTime current = DateTime.MinValue;

			while (current <= after)
				current = NextDate();

			return current;
		}
	}
	
	public struct Transaction 
	{
		public decimal Amount { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }

		public Transaction (decimal amount, DateTime date, string description)
		{
			Amount = amount;
			Date = date;
			Description = description;
		}
	}
}
