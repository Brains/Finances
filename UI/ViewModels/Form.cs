using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Records;
using UI.Interfaces;
using static Records.Record;

namespace UI.ViewModels
{
	public class Form : PropertyChangedBase, IForm
	{
		private readonly IRecordsStorage aggregator;
		private readonly ISettings settings;
		private Types selectedType;

		public Form(ISettings settings, IRecordsStorage aggregator)
		{
			this.settings = settings;
			this.aggregator = aggregator;

			Types = Enum.GetValues(typeof (Types)).Cast<Types>();
			UpdateCategories(selectedType);

			DateTime = DateTime.Now;
        }

		public IEnumerable<Types> Types { get; set; }
		public IEnumerable<Categories> Categories { get; set; }

		public Types SelectedType
		{
			get { return selectedType; }
			set
			{
				selectedType = value;
				NotifyOfPropertyChange(nameof(SelectedType));
				UpdateCategories(selectedType);
			}
		}

		public Categories SelectedCategory { get; set; }
		public string Description { get; set; }
		public DateTime DateTime { get; set; }

		public decimal Amount { get; set; }

		public void Submit()
		{
			aggregator.Add(new Record(Amount, SelectedType, SelectedCategory, Description, DateTime));
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