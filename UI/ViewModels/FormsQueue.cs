using System.Collections.Generic;
using Caliburn.Micro;

namespace UI.ViewModels
{
	public class FormsQueue : PropertyChangedBase, IViewModel
	{
		public List<Form> Forms { get; }

		public void Add()
		{
			
		}

		public void Remove()
		{

		}

		public void Submit()
		{

		}

		public bool CanAdd() => false;
		public bool CanRemove() => false;
		public bool CanSubmit() => false;
	}
}