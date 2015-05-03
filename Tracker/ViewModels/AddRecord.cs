using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Tracker.ViewModels
{
	public class AddRecord
	{
		// Model
		private readonly IExpenses expenses;

		// Record Fields
		public string Amount { get; set; }
		public string Description { get; set; }
        public Record.Types Type { get; set; }
		public Record.Categories Category { get; set; }
		public ICommand SubmitCommand { get; private set; }

		//------------------------------------------------------------------
		public IEnumerable<Record.Categories> RecordCategories { get; set; }
		public IEnumerable<Record.Types> RecordTypes { get; set; }

		//------------------------------------------------------------------
		public AddRecord (IExpenses expenses)
		{
			this.expenses = expenses;
			SubmitCommand = new DelegateCommand<object>(arg => Submit());

			RecordTypes = Enum.GetValues(typeof(Record.Types)).Cast<Record.Types>();
			RecordCategories = Enum.GetValues(typeof(Record.Categories)).Cast<Record.Categories>();
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
//			var amounts = amount.Split('+');
//			decimal[] decimals = amounts.Select(decimal.Parse).ToArray();
//			decimals.Sum();

			return decimal.Parse(amount);
		}

		//------------------------------------------------------------------
		private void Divide(ref decimal amount)
		{
			decimal customers = 3;
			amount = Math.Round(amount / customers);
		}
	}
}
