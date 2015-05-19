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
	internal class RecordFormsQueueTests : AssertionHelper
	{
		private static void AddRecords (int[] amounts, RecordFormsQueue queue)
		{
			queue.AddForm().Amount = amounts[0];
			queue.AddForm().Amount = amounts[1];
			queue.AddForm().Amount = amounts[2];
		}

		private static void FillForms (IEnumerable<RecordForm> forms)
		{
			foreach (var form in forms)
			{
				form.Amount = 10;
				form.Description = "Test";
			}
		}

		[Test]
		public void AddForm_ByDefault_AddsNewToRecords ()
		{
			RecordFormsQueue queue = new RecordFormsQueue(null);

			queue.AddForm();

			Expect(queue.Forms.Count, EqualTo(1));
		}

		[Test]
		public void AddForm_Always_GeneratePropertyChanged()
		{
			bool fired = false;
			RecordFormsQueue queue = new RecordFormsQueue(null);
			queue.PropertyChanged += (s, a) => fired = true;

			queue.AddForm();

			Expect(fired, True);
		}

		[Test]
		public void RemoveForm_Always_GeneratePropertyChanged()
		{
			bool fired = false;
			RecordFormsQueue queue = new RecordFormsQueue(null);
			queue.PropertyChanged += (s, a) => fired = true;

			queue.Forms.Add(null);
			queue.RemoveLastForm();

			Expect(fired, True);
		}

		[Test]
		public void RemoveForm_ByDefault_RemovesLastForm()
		{
			RecordFormsQueue queue = new RecordFormsQueue(null);

			var first = queue.AddForm();
			queue.AddForm();
			queue.RemoveLastForm();

			Expect(queue.Forms.Count, EqualTo(1));
			Expect(queue.Forms, Exactly(1).EqualTo(first));
		}

		[Test]
		public void RemoveForm_FromEmptyCollection_RemovesNothing()
		{
			RecordFormsQueue queue = new RecordFormsQueue(null);

			queue.RemoveLastForm();

			Expect(queue.Forms.Count, EqualTo(0));
		}

		[TestCase (70, 100, 20, 10)]
		[TestCase (120, 300, 120, 60)]
		[TestCase (111, 256, 48, 97)]
		public void SubstractFromPrimary_Always_SubtractsFromFirstRecordAllSubsequentRecordsAmount (int expected, params int[] amounts)
		{
			RecordFormsQueue queue = new RecordFormsQueue(null);
			AddRecords(amounts, queue);

			queue.SubstractSecondariesFromPrimary();

			Expect(queue.Forms.First().Amount, EqualTo(expected));
		}

		[TestCase (130, 100, 20, 10)]
		[TestCase (370, 210, 90, 70)]
		public void Total_ByDefault_ReturnsTotalAmountForAllRecords (int expected, params int[] amounts)
		{
			RecordFormsQueue queue = new RecordFormsQueue(null);
			AddRecords(amounts, queue);

			var total = queue.Total();

			Expect(total, EqualTo(expected));
		}

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

			expenses.Received(3).Add(10, Record.Types.Expense, Record.Categories.Food, "Test", Arg.Any<DateTime>());
		}

		[Test]
		public void Any_NoForms_ReturnsFalse()
		{
			RecordFormsQueue queue = new RecordFormsQueue(null);

			var actual = queue.Any;

			Expect(actual, False);
		}

		[Test]
		public void Any_WithFormsExist_ReturnsTrue()
		{
			RecordFormsQueue queue = new RecordFormsQueue(null);

			queue.AddForm();
			var actual = queue.Any;

			Expect(actual, True);
		}
	}
}
