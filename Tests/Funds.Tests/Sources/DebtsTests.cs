using System;
using Common;
using Funds.Sources;
using NUnit.Framework;
using static Common.Record.Types;
using static Common.Record.Categories;

namespace Funds.Tests.Sources
{
	public class DebtsTests
	{
		public static readonly Record[] Data =
		{
			new Record(100, Debt, Maxim,  "Out", new DateTime(1, 1, 1)),
			new Record(100, Debt, Maxim,  "Out", new DateTime(1, 1, 1)),
			new Record(100, Debt, Andrey, "Out", new DateTime(1, 1, 1)),
			new Record(100, Debt, Andrey, "Out", new DateTime(1, 1, 1)),
			new Record(100, Debt, Maxim,  "In",	 new DateTime(1, 1, 1)),
			new Record(100, Debt, Andrey, "In",  new DateTime(1, 1, 1)),
		};

		[Test]
		public void PullValue_Always_SetsValueToTotalDebt()
		{
			var debts = new Debts();

			debts.PullValue();

			Assert.That(debts.Value, Is.EqualTo(200));
		}
	}
}