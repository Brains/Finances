using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Tracker.ViewModels
{
	class Records
	{
		public List<Record> RecordsList { get; set; }
		//------------------------------------------------------------------
		public Records ()
		{
			Expenses expenses = new Expenses();
			RecordsList = expenses.Records;

			MakeGrouping();
		}

		//------------------------------------------------------------------
		private void MakeGrouping ()
		{
			ICollectionView view = CollectionViewSource.GetDefaultView(RecordsList);

			view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Ascending));
			view.GroupDescriptions.Clear();
			view.GroupDescriptions.Add(new PropertyGroupDescription("Date", new Converters.DateTimeToDateConverter()));
		}
	}
}
