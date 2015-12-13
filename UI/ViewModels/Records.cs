using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Caliburn.Micro;
using Common;
using Common.Storages;
using UI.Interfaces;
using UI.Views.Converters;

namespace UI.ViewModels
{
	public class Records : PropertyChangedBase, IViewModel
	{
		public IEnumerable<Record> List { get; }
		public int RowIndex { get; } = 0;

		public Records(IExpenses expenses)
		{
			List = expenses.Records;

			ConfigureView();
		}

		private void ConfigureView()
		{
			ICollectionView view = CollectionViewSource.GetDefaultView(List);

			view.SortDescriptions.Clear();
			view.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));

			view.GroupDescriptions.Clear();
			view.GroupDescriptions.Add(new PropertyGroupDescription("Date", new GroupingMonth()));
			view.GroupDescriptions.Add(new PropertyGroupDescription("Date", new GroupingDate()));
		}
	}
}
