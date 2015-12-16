﻿using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using MoreLinq;
using UI.Interfaces;
using static System.Windows.Media.ColorConverter;

namespace UI.ViewModels
{
	public class FormsQueue : PropertyChangedBase, IViewModel
	{
		private const int Limit = 5;
		public IFormFactory Factory { get; }
		public IObservableCollection<IForm> Forms { get; set; }

		public int RowIndex { get; } = 1;

		public Brush SelectedForm { get; set; }
		public bool CanAdd => Forms.Count < Limit;
		public bool CanRemove => Forms.Any();
		public bool CanSubmit => Forms.Any();

		public FormsQueue(IFormFactory factory)
		{
			Forms = new BindableCollection<IForm>();
			Factory = factory;

			SelectedForm = new SolidColorBrush((Color) ConvertFromString("#66007C9C"));
		}

		public void Add()
		{
			Forms.Add(Factory.Create());

			SetPrimaryColor();
			NotifyAllProperties();
		}

		public void Remove()
		{
			Forms.RemoveAt(Forms.Count - 1);

			SetPrimaryColor();
			NotifyAllProperties();
		}

		public void Submit()
		{
			SubstractFromPrimary();
			Forms.ForEach(form => form.Submit());
			Forms.Clear();

			NotifyAllProperties();
		}

		private void SubstractFromPrimary()
		{
			var primary = Forms.First();
			var secondaries = Forms.Skip(1);
			primary.Amount -= secondaries.Sum(form => form.Amount);
		}

		private void SetPrimaryColor()
		{
			if (Forms.Count > 1)
				Forms.First().Background = SelectedForm;
			else
				Forms.First().Background = Brushes.Transparent;
		}

		private void NotifyAllProperties()
		{
			NotifyOfPropertyChange(nameof(CanAdd));
			NotifyOfPropertyChange(nameof(CanRemove));
			NotifyOfPropertyChange(nameof(CanSubmit));
		}
	}
}