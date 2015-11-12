using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Common;
using static Common.Record;
using static Common.Record.Types;
using static Common.Record.Categories;

namespace Temporary.Records
{
	public class FormViewModel : PropertyChangedBase
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

		public FormViewModel(IExpenses expenses)
		{
			this.expenses = expenses;

			// Assign date here instead of the Submit() because of Primary and Secondary need to have different time
			Date = DateTime.Now;

			RecordTypes = Enum.GetValues(typeof (Types)).Cast<Types>();

			availableCategories = new Dictionary<Types, Categories[]>
			{
				[Expense] = new[]	{Food, Health, House, General, Women, Other},
				[Debt] = new[]		{Maxim, Andrey},
				[Income] = new[]	{Deposit, ODesk},
				[Shared] = new[]	{Food, House, General, Other},
				[Balance] = new[]	{Other}
			};

			DescriptionSuggestions = new List<string> {"Novus", "Kishenya", "Water", "Hygiene", "Domestic", "Passage",
				"Chasopys", "Freud-House", "BiblioTech", "Vagon", "Ziferblat" };

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
			NotifyOfPropertyChange(nameof(Type));
			SetAvailableCategories(value);
			ClearDescription();
		}

		private void SetAvailableCategories(Types value)
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