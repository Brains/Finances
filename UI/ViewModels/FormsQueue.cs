using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;

namespace UI.ViewModels
{
	public class FormsQueue : PropertyChangedBase, IViewModel
	{
		private readonly IFormFactory factory;
		private const int Limit = 5;
		public List<Form> Forms { get; }

		public FormsQueue()
		{
			Forms = new List<Form>();
			factory = new FormFactory();
		}

		public void Add()
		{
			Forms.Add(factory.Create());
		}

		public void Remove() {}

		public void Submit() {}

		public bool CanAdd() => Forms.Count < Limit;
		public bool CanRemove() => Forms.Any();
		public bool CanSubmit() => false;
	}
}