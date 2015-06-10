using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Practices.Prism.Mvvm;
using static Tracker.Record;
using static Tracker.Record.Types;
using static Tracker.Record.Categories;

namespace Tracker.ViewModels
{
	public class RecordForm : BindableBase
	{
		// Model
		private readonly IExpenses expenses;

		// Record Fields
		private Types type;
		public decimal Amount { get; set; }

		public Types Type
		{
			get { return type; }
			set { OnTypeUpdate(value); }
		}

		public Categories Category { get; set; }
		public string Description { get; set; }
		public DateTime Date { get; set; }

		// View needs
		public Thickness Padding { get; set; }
		public Thickness Border { get; set; }
		public IEnumerable<Types> RecordTypes { get; set; }
		public IEnumerable<Categories> AvailableCategories { get; set; }
		private readonly Dictionary<Types, Categories[]> availableCategories;
		public List<string> DescriptionSuggestions { get; set; }

		public RecordForm(IExpenses expenses)
		{
			this.expenses = expenses;

			// Assign date here instead of the Submit() because of Primary and Secondary need to have different time
			Date = DateTime.Now;

			RecordTypes = Enum.GetValues(typeof (Types)).Cast<Types>();

			availableCategories = new Dictionary<Types, Categories[]>
			{
				[Expense] = new[]	{Food, Health, House, General, Women, Other},
				[Debt] = new[]		{Maxim, Andrey},
				[Income] = new[]	{ODesk, Deposit},
				[Shared] = new[]	{Food, House, General, Other},
				[Balance] = new[]	{Other}
			};

			DescriptionSuggestions = new List<string> {"Novus", "Kishenya", "Water", "Hygiene", "Domestic", "Passage" };

			Padding = new Thickness(40, 5, 5, 5);
			Border = new Thickness(0);
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

		private void OnTypeUpdate(Types value)
		{
			type = value;
			OnPropertyChanged("Type");
			SetAvailableCategories(value);
			ClearDescription();
		}

		private void SetAvailableCategories(Types value)
		{
			AvailableCategories = availableCategories[value];
			OnPropertyChanged("AvailableCategories");
			Category = AvailableCategories.First();
			OnPropertyChanged("Category");
		}

		private void ClearDescription()
		{
			Description = null;
			OnPropertyChanged("Description");
		}
	}
}