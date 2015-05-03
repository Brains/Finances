using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Tracker.ViewModels;

namespace Tracker.UnitTests
{
	[TestFixture]
	class RecordsQueueTests : AssertionHelper
	{
		//------------------------------------------------------------------
		private static void AddRecords (string[] amounts, RecordsQueue queue)
		{
			queue.AddRecord().Amount = amounts[0];
			queue.AddRecord().Amount = amounts[1];
			queue.AddRecord().Amount = amounts[2];
		}

		//------------------------------------------------------------------
		private static AddRecord CreateRecord (IExpenses expenses)
		{
			return new AddRecord(expenses) {Amount = "10", Description = "Test"};
		}

		//------------------------------------------------------------------
		[TestCase("70", "100", "20", "10")]
		[TestCase("120", "300", "120", "60")]
		[TestCase("111", "256", "48", "97")]
		public void SubstractFromPrimary_Always_SubtractsFromFirstRecordAllSubsequentRecordsAmount(string expected, params string[] amounts)
		{
			RecordsQueue queue = new RecordsQueue(null);
			AddRecords(amounts, queue);

			queue.SubstractFromPrimary();

			Expect(queue.Records.First().Amount, EqualTo(expected));
		}

		//------------------------------------------------------------------
		[TestCase(130, "100", "20", "10")]
		[TestCase(370, "210", "90", "70")]
		public void Total_ByDefault_ReturnsTotalAmountForAllRecords (decimal expected, params string[] amounts)
		{
			RecordsQueue queue = new RecordsQueue(null);
			AddRecords(amounts, queue);

			var total = queue.Total();

			Expect(total, EqualTo(expected));
		}

		//------------------------------------------------------------------
		[Test]
		public void Submit_Always_AddAllRecordsToExpences ()
		{
			var expenses = Substitute.For<IExpenses>();
			RecordsQueue queue = new RecordsQueue(expenses);

			queue.AddRecord(CreateRecord(expenses));
			queue.AddRecord(CreateRecord(expenses));
			queue.AddRecord(CreateRecord(expenses));

			queue.Submit();

			expenses.Received(3).Add(10, Record.Types.Expense, Record.Categories.Food, "Test");
		}


		//------------------------------------------------------------------
		[Test]
		public void AddRecord_Correct_AddsItToRecords ()
		{
			RecordsQueue queue = new RecordsQueue(null);

			var record = new AddRecord(null);
			queue.AddRecord(record);

			Expect(queue.Records, Contains(record));
		}

		//------------------------------------------------------------------
		[Test]
		public void AddRecord_ByDefault_AddsNewToRecords()
		{
			RecordsQueue queue = new RecordsQueue(null);

			queue.AddRecord();

			Expect(queue.Records.Count, EqualTo(1));
		}
	}
}
