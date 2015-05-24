using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Tracker.ViewModels
{
	public class RecordForm : INotifyPropertyChanged
	{
		// Model
		private readonly IExpenses expenses;
		private string description;
		private Record.Categories category;

		private Dictionary<Record.Types, Record.Categories[]> allowedCategories;
		private Record.Types type;

		// Record Fields
		public decimal Amount { get; set; }

		public Record.Types Type
		{
			get { return type; }
			set
			{
				type = value;
				RecordCategories = allowedCategories[value];
				OnPropertyChanged("RecordCategories");
				Category = RecordCategories.First();
				OnPropertyChanged("Category");
			}
		}

		public Record.Categories Category
		{
			get { return category; }
			set { category = ValidateCategoryForDebt(value); }
		}

		public string Description
		{
			get { return description; }
			set { description = ValidateDescriptionForDebt(value); }
		}

		public List<string> DescriptionSuggestions { get; set; }
		public DateTime Date { get; set; }

		const string DebtIn = "In";
		const string DebtOut = "Out";

		// View needs
		public Thickness Padding { get; set; }
		public Thickness Border { get; set; }
		public IEnumerable<Record.Types> RecordTypes { get; set; }
		public IEnumerable<Record.Categories> RecordCategories { get; set; }

		public RecordForm(IExpenses expenses)
		{
			this.expenses = expenses;

			// Assign date here instead of the Submit() because of Primary and Secondary need to have different time
			Date = DateTime.Now;

			DescriptionSuggestions = new List<string>() { DebtIn, DebtOut, "Novus", "Water"};

			RecordTypes = Enum.GetValues(typeof (Record.Types)).Cast<Record.Types>();
			RecordCategories = Enum.GetValues(typeof (Record.Categories)).Cast<Record.Categories>();

			Padding = new Thickness(40, 5, 5, 5);
			Border = new Thickness(0);

			allowedCategories = new Dictionary<Record.Types, Record.Categories[]>
			{
				[Record.Types.Expense] = new[] {Record.Categories.Food, Record.Categories.General, Record.Categories.Health,
					Record.Categories.House, Record.Categories.Other, Record.Categories.Women, },
				[Record.Types.Debt] = new[] { Record.Categories.Max, Record.Categories.Andrey },
				[Record.Types.Income] = new[] { Record.Categories.ODesk, Record.Categories.Deposit },
				[Record.Types.Shared] = new[] { Record.Categories.Food, Record.Categories.House, Record.Categories.General,
					Record.Categories.Other,  },
			};
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

		private Record.Categories ValidateCategoryForDebt(Record.Categories category)
		{
			if (Type != Record.Types.Debt) return category;
			
			if (category != Record.Categories.Max && category != Record.Categories.Andrey)
				throw new ArgumentException("Debt: only 'Max' or 'Andrey'");

			return category;
		}

		private string ValidateDescriptionForDebt(string description)
		{
			if (Type != Record.Types.Debt) return description;

			if (!DebtIn.Contains(description) && !DebtOut.Contains(description))
				throw new ArgumentException("Debt: only 'In' or 'Out'");

			return description;
		}


		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}