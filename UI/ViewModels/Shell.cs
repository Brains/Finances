using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Caliburn.Micro;
using Common;
using Common.Storages;
using UI.Views.Converters;

namespace UI.ViewModels
{
	public interface IShell {}

	public class Shell : PropertyChangedBase, IShell
	{
		public IScreen SelectedItem { get; set; }
		public Shell(IExpenses expenses)
		{
			List = expenses.Records;

			ConfigureView();

		}
		public IEnumerable<Record> List { get; }
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