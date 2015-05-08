using System;
using System.Globalization;
using System.Linq;
using NodaTime;
using NUnit.Framework;
using static System.Console;

namespace Trends.Tests
{
	[TestFixture]
	class Trend
	{
		//------------------------------------------------------------------
		[Test]
		public void Test ()
		{
			var trend = new Trends.Trend();

			trend.Calendar.ForEach(WriteLine);
		}
	}
}
