using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoreLinq;
using NSubstitute;
using NUnit.Framework;
using Records;
using UI.ViewModels;
using static NSubstitute.Substitute;
using static Records.Record;

namespace UITests.ViewModels
{
	public class MonthDiagramsTests : AssertionHelper
	{
		private readonly Record[] records = FixedRecords.Data;
		private IAnalyzer analyzer;
		private IExpenses expenses;

		private MonthDiagrams Create()
		{
			var spending = new Dictionary<int, decimal>()
			{
				[10] = 300, 
				[11] = 300, 
				[12] = 300, 
			};

			var income = new Dictionary<int, decimal>()
			{
				[10] = 100, 
				[11] = 100, 
				[12] = 100, 
			};

			analyzer = For<IAnalyzer>();
			expenses = For<IExpenses>();
			expenses.Records = new ObservableCollection<Record>(records);
			analyzer.CalculateTotalByMonth(null).ReturnsForAnyArgs(spending, income);

			return new MonthDiagrams(expenses, analyzer);
		}

		private void Print<T>(IEnumerable<T> items) => items.ForEach(item => Console.WriteLine(item));

		[Test]
		public void BalanceByMonth_Always_HasExpensesToIncomeRationGroupedByMonth()
		{
			var diagrams = Create();

			// Assserts
			var keys = diagrams.BalanceByMonth.Select(pair => pair.Key);
			var keysExpected = EquivalentTo(new[] {10, 11, 12});
			var expense = diagrams.BalanceByMonth[Types.Expense];
			var income = diagrams.BalanceByMonth[Types.Income];

			Expect(keys, EquivalentTo(new[] { Types.Expense, Types.Income}));
			Expect(expense.Select(p => p.Key), keysExpected);
			Expect(income.Select(p => p.Key), keysExpected);
			Expect(expense.Select(p => p.Value), All.EqualTo(300));
			Expect(income.Select(p => p.Value), All.EqualTo(100));
		}
	}
}