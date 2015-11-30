using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class FormsQueue : PropertyChangedBase, IViewModel
	{
		private readonly IFormFactory factory;
		private const int Limit = 5;
		public List<IForm> Forms { get; set; }

		public FormsQueue(IFormFactory factory)
		{
			Forms = new List<IForm>();

			this.factory = factory;
		}

		public void Add()
		{
			Forms.Add(factory.Create());
		}

		public void Remove()
		{
			Forms.RemoveAt(Forms.Count - 1);
		}

		public void Submit()
		{
			Forms.ForEach(form => form.Submit());
		}

		public bool CanAdd() => Forms.Count < Limit;
		public bool CanRemove() => Forms.Any();
		public bool CanSubmit() => false;
	}
}