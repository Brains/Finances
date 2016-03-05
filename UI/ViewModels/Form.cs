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
using UI.Views.Converters;
using static Common.Record;

namespace UI.ViewModels
{
	public class Form : PropertyChangedBase, IForm
	{
		private readonly IRecordsStorage aggregator;
		private readonly ISettings settings;
		private Types selectedType;
		private Brush background = Brushes.Transparent;
		private string description;
		private readonly IAdder adder;
		private IAmountFactory factory;
		private IAmount amount;

		public Form(ISettings settings, IRecordsStorage aggregator, IAdder adder, IAmountFactory factory)
		{
			this.settings = settings;
			this.aggregator = aggregator;
			this.adder = adder;
			this.factory = factory;

			Types = Enum.GetValues(typeof (Types)).Cast<Types>();
			UpdateCategories(selectedType);
			Amount = factory.Create(selectedType);

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
				Amount = factory.Create(value);
				NotifyOfPropertyChange(nameof(SelectedType));
				UpdateCategories(selectedType);
			}
		}

		public Categories SelectedCategory { get; set; }

		public string Description
		{
			get { return description; }
			set
			{
				if (value == description) return;
				description = value;
				NotifyOfPropertyChange();
			}
		}

		public string[] Descriptions { get; set; }
		public DateTime DateTime { get; set; }

		public IAmount Amount
		{
			get { return amount; }
			set { amount = value; }
		}

		public Brush Background
		{
			get { return background; }
			set
			{
				if (Equals(value, background)) return;
				background = value;
				NotifyOfPropertyChange();
			}
		}

		public void Submit()
		{
			aggregator.Add(new Record(Amount.Value, SelectedType, SelectedCategory, Description, DateTime));
		}

		public bool CanSubmit()
		{
			if (Amount.Value < 1) return false;
			if (string.IsNullOrWhiteSpace(Description)) return false;

			if (selectedType == Record.Types.Debt 
				&& Description != "In" 
				&& Description != "Out") return false;

			return true;
		}

		private void UpdateCategories(Types type)
		{
			Categories = settings.CategoriesMapping[type];
			SelectedCategory = Categories.First();

			NotifyOfPropertyChange(nameof(Categories));
			NotifyOfPropertyChange(nameof(SelectedCategory));
		}
	}
}