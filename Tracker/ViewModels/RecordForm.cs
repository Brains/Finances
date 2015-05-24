using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using static Tracker.Record;
using static Tracker.Record.Types;
using static Tracker.Record.Categories;

namespace Tracker.ViewModels
{
	public class RecordForm : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// Model
		private readonly IExpenses expenses;

		private Dictionary<Types, Categories[]> allowedCategories;
		private Types type;

		// Record Fields
		public decimal Amount { get; set; }

		public Types Type
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

		public Categories Category { get; set; }

		public string Description { get; set; }

		public List<string> DescriptionSuggestions { get; set; }
		public DateTime Date { get; set; }

		// View needs
		public Thickness Padding { get; set; }
		public Thickness Border { get; set; }
		public IEnumerable<Types> RecordTypes { get; set; }
		public IEnumerable<Categories> AvailableCategories { get; set; }

		public RecordForm(IExpenses expenses)
		{
			this.expenses = expenses;

			// Assign date here instead of the Submit() because of Primary and Secondary need to have different time
			Date = DateTime.Now;

			DescriptionSuggestions = new List<string> { "Novus", "Kishenya", "Water" };

			RecordTypes = Enum.GetValues(typeof (Types)).Cast<Types>();
			AvailableCategories = Enum.GetValues(typeof (Categories)).Cast<Categories>();

			Padding = new Thickness(40, 5, 5, 5);
			Border = new Thickness(0);

			allowedCategories = new Dictionary<Types, Categories[]>
			{
				[Expense] = new[] {Food, General, Health, House, Other, Women },
				[Debt] = new[] { Max, Andrey },
				[Income] = new[] { ODesk, Deposit },
				[Shared] = new[] { Food, House, General, Other  }
			};
		}

		public void Submit()
		{
			if (Type == Shared)
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