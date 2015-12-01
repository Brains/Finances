using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class FormsQueue : PropertyChangedBase, IViewModel
	{
		public IFormFactory Factory { get; }
		private const int Limit = 5;
		public List<IForm> Forms { get; set; }

		public FormsQueue(IFormFactory factory)
		{
			Forms = new List<IForm>();
			Factory = factory;
		}

		public void Add()
		{
			Forms.Add(Factory.Create());
		}

		public void Remove()
		{
			Forms.RemoveAt(Forms.Count - 1);
		}

		public void Submit()
		{
			SubstractFromPrimary();

			Forms.ForEach(form => form.Submit());
		}

		private void SubstractFromPrimary()
		{
			var primary = Forms.First();
			var secondaries = Forms.Skip(1);
			primary.Amount -= secondaries.Sum(form => form.Amount);
		}

		public bool CanAdd => Forms.Count < Limit;
		public bool CanRemove => Forms.Any();
		public bool CanSubmit => Forms.Any();
	}
}