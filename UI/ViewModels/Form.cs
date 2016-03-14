using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using Common;
using Common.Storages;
using MoreLinq;
using UI.Interfaces;
using UI.Services;
using UI.Services.Amount;
using UI.Views.Converters;
using static Common.Record;

namespace UI.ViewModels
{
	public class Form : PropertyChangedBase, IForm
	{
		private readonly IRecordsStorage aggregator;
		private readonly ISettings settings;
		private Types selectedType;
		private readonly IAdder adder;
		private readonly IAmountFactory factory;
		private IAmount amount;

		public Form(ISettings settings, IRecordsStorage aggregator, IAdder adder, IAmountFactory factory)
		{
			this.settings = settings;
			this.aggregator = aggregator;
			this.adder = adder;
			this.factory = factory;

			amount = factory.Create(selectedType);
			Types = Enum.GetValues(typeof (Types)).Cast<Types>();
			UpdateCategories(selectedType);

			DateTime = DateTime.Now;
			Descriptions = settings.Descriptions;
		}

		public IEnumerable<Types> Types { get; set; }
		public IEnumerable<Categories> Categories { get; set; }

		public Types SelectedType
		{
			get { return selectedType; }
			set
			{
				selectedType = value;
				amount = factory.Create(value);
				UpdateCategories(selectedType);
				Refresh();
			}
		}

		public Categories SelectedCategory { get; set; }

		[Notify]
		public string Description { get; set; }

		public string[] Descriptions { get; set; }
		public DateTime DateTime { get; set; }

		[Notify]
		public string Amount
		{
			get { return amount.Formatted; }
			set { amount.Formatted = value;}
		}

		[Notify]
        decimal IForm.Amount
	    {
            get { return amount.Value; }
            set { amount.Value = value; }
        }

	    [Notify]
		public Brush Background { get; set; } = Brushes.Transparent;

		public void Submit()
		{
			aggregator.Add(new Record(amount.Value, SelectedType, SelectedCategory, Description, DateTime));
		}

		public bool CanSubmit()
		{
			if (amount.Value < 1) return false;
			if (string.IsNullOrWhiteSpace(Description)) return false;

			if (selectedType == Record.Types.Debt 
				&& Description != "In" 
				&& Description != "Out") return false;

			return true;
		}

		public void Subtract(decimal value)
		{
            amount.Value -= value;
            NotifyOfPropertyChange(nameof(Amount));
		}

		private void UpdateCategories(Types type)
		{
			Categories = settings.CategoriesMapping[type];
			SelectedCategory = Categories.First();

		}
	}
}