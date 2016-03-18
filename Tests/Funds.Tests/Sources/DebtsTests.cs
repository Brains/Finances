using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Common;
using Common.Storages;
using Funds.Sources;
using NUnit.Framework;
using static Common.Record.Types;
using static Common.Record.Categories;
using static NSubstitute.Substitute;

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

		private Debts Create(IExpenses expenses = null)
		{
			return new Debts(expenses);
		}

		[Test]
		public void PullValue_Always_SetsValueToTotalDebt()
		{
			var expenses = For<IExpenses>();
			expenses.Records = new ObservableCollection<Record>(Data);
			var debts = Create(expenses);
		    decimal actual = 0;
		    debts.Update += value => actual = value;

            debts.PullValue();

			Assert.That(actual, Is.EqualTo(200));
		}

		[Test]
		public void CalculateAmountsPerDude_Always_ReturnsTotalByDudeForEachDirection()
		{
			var debts = Create();
			var expected = new Dictionary<Record.Categories, decimal>
			{
				[Maxim] = 100,
				[Andrey] = 100,
			};

			var actual = debts.Calculate(Data);

			Assert.That(actual, Is.EquivalentTo(expected));
		}

		[Test]
		public void Validate_ValidRecords_DoesNotThrowExceptions()
		{
			var debts = Create();

			Assert.That(() => debts.Validate(Data), Throws.Nothing);
		}

		[Test]
		public void Validate_InvalidRecords_ThrowsArgumentException()
		{
			var debts = Create();
			var invalid = Data.ToList();
			invalid.Add(new Record(100, Debt, Maxim, "ERROR", new DateTime(1, 1, 1)));

			Assert.That(() => debts.Validate(invalid), Throws.ArgumentException.With.Message.Contain("Wrong Description for Debt record"));
		}
	}
}