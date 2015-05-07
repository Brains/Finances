using System;
using System.Globalization;
using System.Linq;
using NodaTime;
using NUnit.Framework;

namespace Trends.Tests
{
	[TestFixture]
	class Trend
	{
		//------------------------------------------------------------------
		[Test]
		public void Test ()
		{
			Operation operation = new Operation 
			{
				Start = new LocalDate(2009, 10, 6),
				Period = Period.FromMonths(1)
			};

			LocalDate date = operation.Start;
			foreach (var index in Enumerable.Range(0, 5))
			{
				Console.WriteLine(date.ToString());
				date = date + operation.Period;
			}
		}
	}
}
