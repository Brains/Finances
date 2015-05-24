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
		public event PropertyChangedEventHandler PropertyChanged;

		// Model
		private readonly IExpenses expenses;

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
				AvailableCategories = allowedCategories[value];
				OnPropertyChanged("AvailableCategories");
				Category = AvailableCategories.First();
				OnPropertyChanged("Category");
			}
		}

		public Record.Categories Category { get; set; }

		public string Description { get; set; }

		public List<string> DescriptionSuggestions { get; set; }
		public DateTime Date { get; set; }

		// View needs
		public Thickness Padding { get; set; }
		public Thickness Border { get; set; }
		public IEnumerable<Record.Types> RecordTypes { get; set; }
		public IEnumerable<Record.Categories> AvailableCategories { get; set; }

		public RecordForm(IExpenses expenses)
		{
			this.expenses = expenses;

			// Assign date here instead of the Submit() because of Primary and Secondary need to have different time
			Date = DateTime.Now;

			DescriptionSuggestions = new List<string>() { "Novus", "Kishenya", "Water" };

			RecordTypes = Enum.GetValues(typeof (Record.Types)).Cast<Record.Types>();
			AvailableCategories = Enum.GetValues(typeof (Record.Categories)).Cast<Record.Categories>();

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

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}