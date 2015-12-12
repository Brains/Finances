using System;

namespace Common
{
	public class PermanentOperation
	{
		public readonly decimal Amount;
		public DateTime Start;
		public readonly TimeSpan Period;
		public readonly string Description;

		public PermanentOperation(decimal amount, DateTime start, TimeSpan period, string description)
		{
			Amount = amount;
			Start = start;
			Period = period;
			Description = description;
		}
	}
}