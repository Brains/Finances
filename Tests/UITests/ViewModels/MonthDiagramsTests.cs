using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using NUnit.Framework;
using Records;
using UI.ViewModels;
using static NSubstitute.Substitute;
using static Records.Record;
using static Records.Record.Categories;
using static Records.Record.Types;

namespace UITests.ViewModels
{
	public class MonthDiagramsTests : AssertionHelper
	{
		private readonly Record[] records = AnalyzerTests.Records;

		private MonthDiagrams Create()
		{
			return new MonthDiagrams(null, null);
		}

		private void Print<T>(IEnumerable<T> items) => items.ForEach(item => Console.WriteLine(item));


		

		[Test]
		public void CalculateBalanceByMonth_Always_CalculateExpencesIncomeRationGroupedByMonth()
		{
			var diagrams = Create();

			var actual = diagrams.CalculateBalanceByMonth();

			Expect(actual, Count.EqualTo(3));
			Expect(actual.Select(pair => pair.Key), EquivalentTo(new[] { 10, 11, 12}));
			Expect(actual.Select(pair => pair.Value), All.EqualTo(500));
		}
	}
}