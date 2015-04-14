using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Tracker.ViewModels
{
	class Tracker
	{
		public List<Record> Records { get; set; }
		//------------------------------------------------------------------
		public Tracker ()
		{
			Expenses expenses = new Expenses();
			Records = expenses.Records;

			MakeGrouping();
		}

		//------------------------------------------------------------------
		private void MakeGrouping ()
		{
			ICollectionView view = CollectionViewSource.GetDefaultView(Records);

			view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Ascending));
			view.GroupDescriptions.Clear();
			view.GroupDescriptions.Add(new PropertyGroupDescription("Date", new Converters.DateTimeToDateConverter()));
		}
	}
}
