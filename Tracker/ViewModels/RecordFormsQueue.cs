using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Commands;

namespace Tracker.ViewModels
{
	public class RecordFormsQueue
	{
		private IExpenses expenses;
		public ObservableCollection<RecordForm> Forms { get; set; }
		public ICommand AddRecordCommand { get; private set; }
		public ICommand SubmitCommand { get; private set; }

		//------------------------------------------------------------------
		public RecordFormsQueue(IExpenses expenses)
		{
			this.expenses = expenses;
			AddRecordCommand = new DelegateCommand<object>(o => AddForm());
			SubmitCommand = new DelegateCommand<object>(o => Submit());
			Forms = new ObservableCollection<RecordForm>();
        }

		//------------------------------------------------------------------
		public RecordForm AddForm ()
		{
			var form = new RecordForm(expenses);

			if (Forms.Count == 0) 
				form.MarkPrimary();

			Forms.Add(form);

			return form;
		}

		//------------------------------------------------------------------
		public void SubstractFromPrimary ()
		{
			var primary = decimal.Parse(Forms.First().Amount);
			var secondaries = Total() - primary;

			Forms.First().Amount = (primary - secondaries).ToString(CultureInfo.InvariantCulture);
		}

		//------------------------------------------------------------------
		public decimal Total()
		{
			return Forms.Select(record => decimal.Parse(record.Amount)).Sum();
		}

		//------------------------------------------------------------------
		public void Submit()
		{
			SubstractFromPrimary();
            Forms.ForEach(record => record.Submit());
        }
	}
}
