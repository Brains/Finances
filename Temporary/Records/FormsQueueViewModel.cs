using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Common;
using Microsoft.Practices.ObjectBuilder2;

namespace Temporary.Records
{
	public class FormsQueueViewModel
	{
		private readonly IExpenses expenses;
		public ObservableCollection<FormViewModel> Forms { get; set; }

		// Commands
		public ICommand AddRecordCommand { get; private set; }
		public ICommand RemoveRecordCommand { get; private set; }
		public ICommand SubmitCommand { get; set; }

		public FormsQueueViewModel(IExpenses expenses)
		{
			this.expenses = expenses;
//			AddRecordCommand = new DelegateCommand(() => AddForm());
//			RemoveRecordCommand = new DelegateCommand(RemoveLastForm);
//			SubmitCommand = new DelegateCommand(Submit);
			Forms = new ObservableCollection<FormViewModel>();
		}

		public FormViewModel AddForm()
		{
			var form = new FormViewModel(expenses);

			if (Forms.Count == 0)
				form.MarkPrimary();
			
			Forms.Add(form);

			return form;
		}

		public void RemoveLastForm()
		{
			if (!Forms.Any()) return;

			Forms.Remove(Forms.Last());
		}

		public void SubstractSecondariesFromPrimary()
		{
			var primary = Forms.First().Amount;
			var secondaries = Total() - primary;

			Forms.First().Amount = primary - secondaries;
		}

		private bool Validate()
		{
			var primary = Forms.First().Amount;

			if (primary <= 0)
				return false;

			//			if (primary <= 0) throw new ArgumentException("Primary Amount <= 0");
			//			if (secondaries > primary) throw new ArgumentException("Secondary Amounts >  Primary Amount");

			return true;
		}

		public decimal Total()
		{
			return Enumerable.Sum((IEnumerable<decimal>) Forms.Select(record => record.Amount));
		}

		public void Submit()
		{
			SubstractSecondariesFromPrimary();

			if (!Validate()) return;

			Forms.ForEach(record => record.Submit());

			Forms.Clear();
		}
	}
}