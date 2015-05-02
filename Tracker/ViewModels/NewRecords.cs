using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Tracker.ViewModels
{
	public class NewRecords
	{
		private IExpenses expenses;
		public ObservableCollection<AddRecord> AddRecordItems { get; set; }
		public ICommand AddNewRecordItem { get; private set; }

		//------------------------------------------------------------------
		public NewRecords(IExpenses expenses)
		{
			this.expenses = expenses;
			AddNewRecordItem = new DelegateCommand<object>(Add);
			AddRecordItems = new ObservableCollection<AddRecord>();
        }

		//------------------------------------------------------------------
		private void Add (object obj)
		{
			AddRecordItems.Add(new AddRecord(expenses));
		}
	}
}
