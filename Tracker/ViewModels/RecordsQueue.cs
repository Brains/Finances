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
	public class RecordsQueue
	{
		private IExpenses expenses;
		public ObservableCollection<AddRecord> Records { get; set; }
		public ICommand AddRecordCommand { get; private set; }

		//------------------------------------------------------------------
		public RecordsQueue(IExpenses expenses)
		{
			this.expenses = expenses;
			AddRecordCommand = new DelegateCommand<object>(Add);
			Records = new ObservableCollection<AddRecord>();
        }

		//------------------------------------------------------------------
		private void Add (object obj)
		{
			Records.Add(new AddRecord(expenses));
		}
	}
}
