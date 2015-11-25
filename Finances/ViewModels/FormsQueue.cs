using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Common;
using Microsoft.Practices.ObjectBuilder2;

namespace Finances.ViewModels
{
	public class FormsQueue : Screen
	{
		private readonly IExpenses expenses;
		public ObservableCollection<Form> Forms { get; set; }

		// Commands
		public ICommand AddRecordCommand { get; private set; }
		public ICommand RemoveRecordCommand { get; private set; }
		public ICommand SubmitCommand { get; set; }

		public FormsQueue(IExpenses expenses)
		{
			this.expenses = expenses;
			Forms = new ObservableCollection<Form>();

			DisplayName = "DisplayName";

        }

		public Form Add()
		{
			var form = new Form(expenses);

			if (Forms.Count == 0)
				form.MarkPrimary();
			
			Forms.Add(form);

			return form;
		}

		public void Remove()
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
			return Forms.Select(record => record.Amount).Sum();
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