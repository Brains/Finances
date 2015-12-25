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
		public Records() : this(new FixedRecords()) {}

		public Records(IExpenses expenses)
		{
			List = expenses.Records;

			ConfigureView();
		}

		public IEnumerable<Record> List { get; }
		public int RowIndex { get; } = 1;

		private void ConfigureView()
		{
			var view = CollectionViewSource.GetDefaultView(List);

			view.SortDescriptions.Clear();
			view.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));

			view.GroupDescriptions.Clear();
			view.GroupDescriptions.Add(new PropertyGroupDescription("Date", new GroupingMonth()));
			view.GroupDescriptions.Add(new PropertyGroupDescription("Date", new GroupingDate()));
		}
	}
}