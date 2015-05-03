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
			queue.AddEmptyRecord().Amount = amounts[0];
			queue.AddEmptyRecord().Amount = amounts[1];
			queue.AddEmptyRecord().Amount = amounts[2];
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
		[TestCase("100", "20", "10")]
		public void Submit_Always_AddAllRecordsToExpences (params string[] amounts)
		{
			var expenses = Substitute.For<IExpenses>();
			RecordsQueue queue = new RecordsQueue(expenses);
			AddRecords(amounts, queue);

		}
	}
}
