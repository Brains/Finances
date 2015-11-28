using System.Collections.Generic;
using Caliburn.Micro;

namespace UI.ViewModels
{
	public class FormsQueue : PropertyChangedBase, IViewModel
	{
		private const int Limit = 5;
		public List<Form> Forms { get; }

		public FormsQueue()
		{
			Forms = new List<Form>();
		}

		public void Add()
		{
			Forms.Add(new Form());
		}

		public void Remove() {}

		public void Submit() {}

		public bool CanAdd() => Forms.Count < Limit;
		public bool CanRemove() => false;
		public bool CanSubmit() => false;
	}
}