using System.Linq;
using Caliburn.Micro;
using MoreLinq;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class FormsQueue : PropertyChangedBase, IViewModel
	{
		private const int Limit = 5;
		public IFormFactory Factory { get; }
		public IObservableCollection<IForm> Forms { get; set; }

		public int RowIndex { get; } = 1;
		
		public bool CanAdd => Forms.Count < Limit;
		public bool CanRemove => Forms.Any();
		public bool CanSubmit => Forms.Any();

		public FormsQueue(IFormFactory factory)
		{
			Forms = new BindableCollection<IForm>();
			Factory = factory;
		}

		public void Add()
		{
			Forms.Add(Factory.Create());

			NotifyAllProperties();
		}

		public void Remove()
		{
			Forms.RemoveAt(Forms.Count - 1);

			NotifyAllProperties();
		}

		public void Submit()
		{
			SubstractFromPrimary();
			Forms.ForEach(form => form.Submit());
			Forms.Clear();

			NotifyAllProperties();
		}

		private void SubstractFromPrimary()
		{
			var primary = Forms.First();
			var secondaries = Forms.Skip(1);
			primary.Amount -= secondaries.Sum(form => form.Amount);
		}

		private void NotifyAllProperties()
		{
			NotifyOfPropertyChange(nameof(CanAdd));
			NotifyOfPropertyChange(nameof(CanRemove));
			NotifyOfPropertyChange(nameof(CanSubmit));
		}
	}
}