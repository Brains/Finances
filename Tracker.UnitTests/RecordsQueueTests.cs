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
		[TestCase("70", "100", "20", "10")]
		[TestCase("120", "300", "120", "60")]
		[TestCase("111", "256", "48", "97")]
		public void SubstractFromPrimary_Always_SubtractsFromFirstRecordAllSubsequentRecordsAmount(string expected, params string[] amounts)
		{
			RecordsQueue queue = new RecordsQueue(null);

			queue.AddEmptyRecord().Amount = amounts[0];
			queue.AddEmptyRecord().Amount = amounts[1];
			queue.AddEmptyRecord().Amount = amounts[2];

			queue.SubstractFromPrimary();

			Expect(queue.Records.First().Amount, EqualTo(expected));
		}

		//------------------------------------------------------------------
		[Test]
		public void Total_ByDefault_ReturnsTotalAmountForAllRecords ()
		{
			RecordsQueue queue = new RecordsQueue(null);

			queue.AddEmptyRecord().Amount = "100";
			queue.AddEmptyRecord().Amount = "20";
			queue.AddEmptyRecord().Amount = "10";

			var total = queue.Total();

			Expect(total, EqualTo(130));
		}
	}
}
