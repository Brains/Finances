using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Commands;

namespace Tracker.ViewModels
{
	public class RecordForm
	{
		// Model
		private readonly IExpenses expenses;

		// Record Fields
		public string Amount { get; set; }
		public string Description { get; set; }
		public Record.Types Type { get; set; }
		public Record.Categories Category { get; set; }
		public Thickness Padding { get; set; }
		public Thickness Border { get; set; }

		//------------------------------------------------------------------
		public IEnumerable<Record.Categories> RecordCategories { get; set; }
		public IEnumerable<Record.Types> RecordTypes { get; set; }

		//------------------------------------------------------------------
		public RecordForm (IExpenses expenses)
		{
			this.expenses = expenses;

			RecordTypes = Enum.GetValues(typeof (Record.Types)).Cast<Record.Types>();
			RecordCategories = Enum.GetValues(typeof (Record.Categories)).Cast<Record.Categories>();

			Padding = new Thickness(40, 5, 5, 5);
			Border = new Thickness(0);
			
        }

		//------------------------------------------------------------------
		public void Submit ()
		{
			if (string.IsNullOrEmpty(Amount) || string.IsNullOrEmpty(Description))
				return;

			decimal amount = Parse(Amount);

			if (Type == Record.Types.Shared)
				Divide(ref amount);

			expenses.Add(amount, Type, Category, Description);
		}

		//------------------------------------------------------------------
		private decimal Parse (string amount)
		{
			return decimal.Parse(amount);
		}

		//------------------------------------------------------------------
		private string ParseIncremental (string amount)
		{
			var amounts = amount.Split('+');
			decimal[] decimals = amounts.Select(decimal.Parse).ToArray();

			return decimals.Sum().ToString(CultureInfo.InvariantCulture);
		}

		//------------------------------------------------------------------
		private void Divide (ref decimal amount)
		{
			decimal customers = 3;
			amount = Math.Round(amount / customers);
		}

		//------------------------------------------------------------------
		public void MarkPrimary ()
		{
			Padding = new Thickness(5);
		}
	}
}