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
	public class RecordsQueue
	{
		private IExpenses expenses;
		public ObservableCollection<AddRecord> Records { get; set; }
		public ICommand AddRecordCommand { get; private set; }
		public ICommand SubmitCommand { get; private set; }

		//------------------------------------------------------------------
		public RecordsQueue(IExpenses expenses)
		{
			this.expenses = expenses;
			AddRecordCommand = new DelegateCommand<object>(o => AddRecord());
			SubmitCommand = new DelegateCommand<object>(o => Submit());
			Records = new ObservableCollection<AddRecord>();
        }

		//------------------------------------------------------------------
		public AddRecord AddRecord (AddRecord record = null)
		{
			Records.Add(record ?? new AddRecord(expenses));

			if (Records.Count == 1)
				Records.Last().Border = new Thickness(0);

			return Records.Last();
		}

		//------------------------------------------------------------------
		public void SubstractFromPrimary ()
		{
			var primary = decimal.Parse(Records.First().Amount);
			var secondaries = Total() - primary;

			Records.First().Amount = (primary - secondaries).ToString(CultureInfo.InvariantCulture);
		}

		//------------------------------------------------------------------
		public decimal Total()
		{
			return Records.Select(record => decimal.Parse(record.Amount)).Sum();
		}

		//------------------------------------------------------------------
		public void Submit()
		{
			SubstractFromPrimary();
            Records.ForEach(record => record.Submit());
        }
	}
}
