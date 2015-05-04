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
	internal class RecordsQueueTests : AssertionHelper
	{
		//------------------------------------------------------------------
		private static void AddRecords (string[] amounts, RecordFormsQueue queue)
		{
			queue.AddForm().Amount = amounts[0];
			queue.AddForm().Amount = amounts[1];
			queue.AddForm().Amount = amounts[2];
		}

		//------------------------------------------------------------------
		private static void FillForms (IEnumerable<RecordForm> forms)
		{
			foreach (var form in forms)
			{
				form.Amount = "10";
				form.Description = "Test";
			}
		}

		//------------------------------------------------------------------
		[Test]
		public void AddForm_ByDefault_AddsNewToRecords ()
		{
			RecordFormsQueue queue = new RecordFormsQueue(null);

			queue.AddForm();

			Expect(queue.Forms.Count, EqualTo(1));
		}

		//------------------------------------------------------------------
		[TestCase ("70", "100", "20", "10")]
		[TestCase ("120", "300", "120", "60")]
		[TestCase ("111", "256", "48", "97")]
		public void SubstractFromPrimary_Always_SubtractsFromFirstRecordAllSubsequentRecordsAmount (string expected, params string[] amounts)
		{
			RecordFormsQueue queue = new RecordFormsQueue(null);
			AddRecords(amounts, queue);

			queue.SubstractFromPrimary();

			Expect(queue.Forms.First().Amount, EqualTo(expected));
		}

		//------------------------------------------------------------------
		[TestCase (130, "100", "20", "10")]
		[TestCase (370, "210", "90", "70")]
		public void Total_ByDefault_ReturnsTotalAmountForAllRecords (decimal expected, params string[] amounts)
		{
			RecordFormsQueue queue = new RecordFormsQueue(null);
			AddRecords(amounts, queue);

			var total = queue.Total();

			Expect(total, EqualTo(expected));
		}

		//------------------------------------------------------------------
		[Test]
		public void Submit_Always_AddAllRecordsToExpences ()
		{
			var expenses = Substitute.For<IExpenses>();
			RecordFormsQueue queue = new RecordFormsQueue(expenses);

			queue.AddForm();
			queue.AddForm();
			queue.AddForm();
			FillForms(queue.Forms);

			queue.Submit();

			expenses.Received(3).Add(10, Record.Types.Expense, Record.Categories.Food, "Test");
		}
	}
}