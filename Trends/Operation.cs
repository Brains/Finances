using System;
using NodaTime;

namespace Trends
{
	public class Operation
	{
		//------------------------------------------------------------------
		public decimal Amount { get; set; }
		public LocalDate Start { get; set; }
		public Period Period { get; set; }
		public string Description { get; set; }

		//------------------------------------------------------------------
		public Operation (decimal amount, LocalDate start, Period period, string description)
		{
			Amount = amount;
			Period = period;
			Description = description;
			Start = start;
		}

	}


	//------------------------------------------------------------------
	public class Transaction : IComparable<Transaction>
	{
		//------------------------------------------------------------------
		public decimal Amount { get; set; }
		public LocalDate Date { get; set; }
		public string Description { get; set; }

		//------------------------------------------------------------------
		public Transaction (decimal amount, LocalDate date, string description)
		{
			Amount = amount;
			Date = date;
			Description = description;
		}

		//------------------------------------------------------------------
		public int CompareTo (Transaction other)
		{
			if (Date > other.Date) return 1;
			if (Date < other.Date) return -1;

			return 0;
		}

		//------------------------------------------------------------------
		public override string ToString ()
		{
			return $"Description: {Description}, Amount: {Amount}, Date: {Date} ";
		}

		//------------------------------------------------------------------

	}
}
