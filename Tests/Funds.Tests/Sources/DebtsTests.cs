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
using Directions = System.Collections.Generic.Dictionary<string, decimal>;

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
			var expenses = For<IExpenses>();
			expenses.Records = new ObservableCollection<Record>(Data);
			var debts = new Debts(expenses);

			debts.PullValue();

			Assert.That(debts.Value, Is.EqualTo(200));
		}

		[Test]
		public void CalculateAmountsPerDude_Always_ReturnsTotalByDudeForEachDirection()
		{
			var debts = new Debts(null);
			var expected = new Dictionary<Record.Categories, Directions>
			{
				[Maxim] = new Directions(){	["Out"] = 200,	["In"] = 100,},
				[Andrey] = new Directions(){["Out"] = 200,	["In"] = 100, }
			};

			var actual = debts.CalculateAmountsPerDude(Data);

			Assert.That(actual, Is.EquivalentTo(expected));
		}

		[Test]
		public void Validate_ValidRecords_DoesNotThrowExceptions()
		{
			var debts = new Debts(null);

			Assert.That(() => debts.Validate(Data), Throws.Nothing);
		}

		[Test]
		public void Validate_InvalidRecords_ThrowsArgumentException()
		{
			var debts = new Debts(null);
			var invalid = Data.ToList();
			invalid.Add(new Record(100, Debt, Maxim, "ERROR", new DateTime(1, 1, 1)));

			Assert.That(() => debts.Validate(invalid), Throws.ArgumentException.With.Message.Contain("Wrong Description for Debt record"));
		}
	}
}