﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Common;
using Tracker.Views.Converters;

namespace Tracker.ViewModels
{
	public class Records
	{
		// Model
		private readonly IExpenses expenses;


		public ObservableCollection<Record> RecordsList => expenses.Records;

		public Records (IExpenses expenses)
		{
			this.expenses = expenses;
			expenses.Load();

			Group();
		}

		private void Group ()
		{
			ICollectionView view = CollectionViewSource.GetDefaultView(RecordsList);

			view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
			view.GroupDescriptions.Clear();
			view.GroupDescriptions.Add(new PropertyGroupDescription("Date", new NumberToMonthConverter()));
			view.GroupDescriptions.Add(new PropertyGroupDescription("Date", new DateTimeToDateConverter()));
		}
	}
}
