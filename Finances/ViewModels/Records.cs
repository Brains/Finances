using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Caliburn.Micro;
using Finances.Resources.Converters;
using Records;

namespace Finances.ViewModels
{
	public class Records : Screen
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
