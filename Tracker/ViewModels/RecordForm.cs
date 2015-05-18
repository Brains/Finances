using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Tracker.ViewModels
{
	public class RecordForm
	{
		// Model
		private readonly IExpenses expenses;

		// Record Fields
		public decimal Amount { get; set; }
		public Record.Types Type { get; set; }
		public Record.Categories Category { get; set; }
		public string Description { get; set; }
		public DateTime Date { get; set; }

		// View needs
		public Thickness Padding { get; set; }
		public Thickness Border { get; set; }
		public IEnumerable<Record.Categories> RecordCategories { get; set; }
		public IEnumerable<Record.Types> RecordTypes { get; set; }

		public RecordForm(IExpenses expenses)
		{
			this.expenses = expenses;

			// Assign date here instead of the Submit() because of Primary and Secondary need to have different time
			Date = DateTime.Now;

			RecordTypes = Enum.GetValues(typeof (Record.Types)).Cast<Record.Types>();
			RecordCategories = Enum.GetValues(typeof (Record.Categories)).Cast<Record.Categories>();

			Padding = new Thickness(40, 5, 5, 5);
			Border = new Thickness(0);
		}

		public void Submit()
		{
			if (Type == Record.Types.Shared)
				Amount = Divide(Amount);

			expenses.Add(Amount, Type, Category, Description, Date);
		}

		private decimal Divide(decimal amount)
		{
			decimal customers = 3;

			return Math.Round(amount/customers);
		}

		public void MarkPrimary()
		{
			Padding = new Thickness(5, 5, 40, 5);
		}
	}
}