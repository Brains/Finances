using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Common;

namespace Finances.ViewModels
{
	public class Form : PropertyChangedBase
	{
		// Model
		private readonly IExpenses expenses;

		// Record Fields
		private Record.Types type;
		public decimal Amount { get; set; }

		public Record.Types Type
		{
			get { return type; }
			set { OnTypeUpdate(value); }
		}

		public Record.Categories Category { get; set; }
		public string Description { get; set; }
		public DateTime Date { get; set; }

		// View needs
		public Thickness Padding { get; set; }
		public Thickness Border { get; set; }
		public IEnumerable<Record.Types> RecordTypes { get; set; }
		public IEnumerable<Record.Categories> AvailableCategories { get; set; }
		private readonly Dictionary<Record.Types, Record.Categories[]> availableCategories;
		public List<string> DescriptionSuggestions { get; set; }

		public Form(IExpenses expenses)
		{
			this.expenses = expenses;

			// Assign date here instead of the Submit() because of Primary and Secondary need to have different time
			Date = DateTime.Now;

			RecordTypes = Enum.GetValues(typeof (Record.Types)).Cast<Record.Types>();

			availableCategories = new Dictionary<Record.Types, Record.Categories[]>
			{
				[Record.Types.Expense] = new[]	{Record.Categories.Food, Record.Categories.Health, Record.Categories.House, Record.Categories.General, Record.Categories.Women, Record.Categories.Other},
				[Record.Types.Debt] = new[]		{Record.Categories.Maxim, Record.Categories.Andrey},
				[Record.Types.Income] = new[]	{Record.Categories.Deposit, Record.Categories.ODesk},
				[Record.Types.Shared] = new[]	{Record.Categories.Food, Record.Categories.House, Record.Categories.General, Record.Categories.Other},
				[Record.Types.Balance] = new[]	{Record.Categories.Other}
			};

			DescriptionSuggestions = new List<string> {"Novus", "Kishenya", "Water", "Hygiene", "Domestic", "Passage",
				"Chasopys", "Freud-House", "BiblioTech", "Vagon", "Ziferblat" };

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

		private void OnTypeUpdate(Record.Types value)
		{
			type = value;
			NotifyOfPropertyChange(nameof(Type));
			SetAvailableCategories(value);
			ClearDescription();
		}

		private void SetAvailableCategories(Record.Types value)
		{
			AvailableCategories = availableCategories[value];
			NotifyOfPropertyChange(nameof(AvailableCategories));
			Category = AvailableCategories.First();
			NotifyOfPropertyChange(nameof(Category));
		}

		private void ClearDescription()
		{
			Description = null;
			NotifyOfPropertyChange(nameof(Description));
		}
	}
}