using System;
using NodaTime;

namespace Trends
{
	public class Operation
	{
		public decimal Amount { get; set; }
		public LocalDate Start { get; set; }
		public Period Period { get; set; }
		public string Description { get; set; }
	}
}
